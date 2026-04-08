using NUnit.Framework;
using SIGES3_0.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Globalization;
using System.Text;

namespace SIGES3_0.Pages.VentasPage
{
    public class NuevaVentaPage
    {
        private readonly IWebDriver driver;
        private readonly Utilities utilities;
        private readonly WebDriverWait wait;
        private bool _wasSaveEnabled = false;
        private bool _wasSaveExecuted = false;
        private string _lastObservedMessage = string.Empty;

        public NuevaVentaPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // Paso: selecciona familia X y concepto Y
        public void SelectProductFlow(string familia, string concepto)
        {
            _wasSaveEnabled = false;
            _wasSaveExecuted = false;
            _lastObservedMessage = string.Empty;

            WaitForFormReady();
            ToggleIgvAndDetUnif();
            SelectProduct(familia, concepto);
        }

        // Paso: actualiza la cantidad X
        public void UpdateQuantityFlow(string cantidad) => UpdateQuantity(cantidad);

        // Paso: ingresa el documento del cliente X
        // Abre el acordón Facturación, ingresa el documento, busca y captura popup si aparece.
        public void EnterDocumentAndSearch(string documento)
        {
            ExpandBillingAccordion();

            Log($"Paso 5a - Ingresar documento {documento}");
            var clienteInput = Find(VentasLocators.NuevaVenta.ClienteBuscar);
            clienteInput.Clear();
            clienteInput.SendKeys(documento);
            Thread.Sleep(1000);

            var typedDocument = (clienteInput.GetAttribute("value") ?? string.Empty).Trim();
            Assert.That(typedDocument, Is.EqualTo(documento),
                $"El documento ingresado no coincide. Esperado={documento}, Actual={typedDocument}");

            Log("Paso 5b - Click lupa");
            Click(VentasLocators.NuevaVenta.ClienteLupa);
            Thread.Sleep(1500);

            var textoCliente = driver.FindElements(By.XPath(
                    "//*[contains(@class,'alias') or contains(@class,'client-name') or " +
                    "contains(@class,'text-primary') or contains(@class,'label-client')]" +
                    "[normalize-space()]"))
                .Select(e => { try { return e.Text?.Trim(); } catch { return null; } })
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));
            Log($"Paso 5c - Resultado cliente: {textoCliente ?? "(sin texto de cliente visible)"}");

            // Capturar popup si aparece (ej: total mayor a 700 con cliente VARIOS + Boleta)
            // Se ignoran mensajes informativos de éxito del sistema (ej: "Se completó los campos").
            Thread.Sleep(500);
            var popupMsg = CaptureVisibleMessage(2);
            if (!string.IsNullOrWhiteSpace(popupMsg))
            {
                Log($"Popup detectado al resolver cliente: {popupMsg}");
                if (IsBlockingMessage(popupMsg))
                    _lastObservedMessage = popupMsg;
                else
                    Log($"Mensaje informativo ignorado (no bloqueante): {popupMsg}");
                TryClickOptional(
                    VentasLocators.NuevaVenta.ErrorOkButton,
                    VentasLocators.NuevaVenta.ErrorOkButtonFallback,
                    By.CssSelector(".ok-button")
                );
            }
        }

        // Paso: selecciona comprobante X con serie Y
        // Ejecuta DESPUÉS de EnterDocumentAndSearch: la validación de RUC
        // se activa al seleccionar Factura cuando ya hay un DNI ingresado.
        public void SelectVoucherFlow(string comprobante, string serie)
        {
            Log($"Seleccionando comprobante: {comprobante}");
            SelectVoucherTypeAndSeries(comprobante, serie);

            // Capturar popup si aparece (ej: RUC requerido al seleccionar Factura con DNI)
            // Se ignoran mensajes informativos de éxito del sistema (ej: "Se completó los campos").
            Thread.Sleep(500);
            var popupMsg = CaptureVisibleMessage(2);
            if (!string.IsNullOrWhiteSpace(popupMsg))
            {
                Log($"Popup detectado al seleccionar comprobante: {popupMsg}");
                if (IsBlockingMessage(popupMsg) && string.IsNullOrWhiteSpace(_lastObservedMessage))
                    _lastObservedMessage = popupMsg;
                else
                    Log($"Mensaje informativo ignorado (no bloqueante): {popupMsg}");
                TryClickOptional(
                    VentasLocators.NuevaVenta.ErrorOkButton,
                    VentasLocators.NuevaVenta.ErrorOkButtonFallback,
                    By.CssSelector(".ok-button")
                );
            }
        }

        // Paso: selecciona tipo de entrega X
        public void SelectDeliveryFlow(string entrega) => SelectDeliveryType(entrega);

        // Paso: configura el pago X (modos legados: Completo, Incompleto, Credito, CreditoInicial)
        public void ConfigurePaymentFlow(string pago) => UpdatePayment(pago);

        // Paso: selecciona el tipo de pago (Contado / Credito / "-" para omitir)
        public void SelectPaymentTypeFlow(string tipoPago)
        {
            bool skip = string.IsNullOrWhiteSpace(tipoPago) || tipoPago.Trim() == "-";
            if (skip) return;

            bool pagoYaVisible = driver.FindElements(VentasLocators.Payment.CashTypeLabelText)
                .Any(e => { try { return e.Displayed; } catch { return false; } });
            if (!pagoYaVisible)
            {
                try { Click(VentasLocators.Payment.PaymentAccordionButton, VentasLocators.Payment.PaymentAccordionButtonFallback); Thread.Sleep(1000); } catch { }
            }

            if (tipoPago.Equals("Contado", StringComparison.OrdinalIgnoreCase))
            {
                Log("Seleccionando tipo de pago: Contado");
                Click(VentasLocators.Payment.CashTypeLabelText, VentasLocators.Payment.CashTypeLabel);
                Thread.Sleep(1000);
                VerificarCamposPagoAutoRelleno();
            }
            else if (tipoPago.Equals("Credito", StringComparison.OrdinalIgnoreCase))
            {
                Log("Seleccionando tipo de pago: Crédito");
                Click(VentasLocators.Payment.CreditTypeLabelText, VentasLocators.Payment.QuickCreditTypeLabel);
                Thread.Sleep(1000);
            }
        }

        // Paso: ingresa el monto inicial para pago a crédito ("-" para omitir)
        public void EnterPaymentInitialAmountFlow(string monto)
        {
            bool skip = string.IsNullOrWhiteSpace(monto) || monto.Trim() == "-";
            if (skip) return;

            Log($"Ingresando monto inicial del pago: {monto}");
            var montoInput = Find(VentasLocators.Payment.CreditInitialAmountInput);
            montoInput.Clear();
            montoInput.SendKeys(monto);
            montoInput.SendKeys(Keys.Tab);
            Thread.Sleep(1000);

            var recibido = Find(VentasLocators.Payment.CashReceivedNewSale);
            recibido.Clear();
            recibido.SendKeys(monto);
            recibido.SendKeys(Keys.Tab);
            Thread.Sleep(1000);
        }

        // Precondición: crea N notas de venta en bucle con los mismos datos
        public void CrearNotasDeVenta(int n, string familia, string concepto, string cantidad, string documento)
        {
            for (int i = 0; i < n; i++)
            {
                Log($"Creando NV {i + 1} de {n}...");
                SelectProductFlow(familia, concepto);
                UpdateQuantityFlow(cantidad);
                EnterDocumentAndSearch(documento);
                SelectVoucherFlow("NOTA DE VENTA(INTERNA)", "NV02");
                SelectDeliveryFlow("Inmediata");
                ConfigurePaymentFlow("Completo");
                GuardarVentaFlow();
                VerifyConfirmationMessage("Se registr");
                Thread.Sleep(1000);
            }
        }

        // Paso: guarda la venta
        // Intenta hacer click en Guardar. Si el botón está deshabilitado, lo informa y no falla.
        // Captura el mensaje resultante sin sobrescribir mensajes de popup previos.
        public void GuardarVentaFlow()
        {
            Log("Paso 10 - Intentando guardar venta...");
            _wasSaveEnabled = false;
            _wasSaveExecuted = false;

            var btn = driver.FindElements(VentasLocators.NuevaVenta.GuardarVenta)
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

            if (btn == null)
            {
                Log("Botón Guardar no encontrado en el DOM.");
                return;
            }

            _wasSaveEnabled = IsSaveEnabled();
            Log($"Botón Guardar habilitado: {_wasSaveEnabled}");

            if (!_wasSaveEnabled)
            {
                // Capturar la validación actualmente visible en el formulario.
                // Sobrescribir _lastObservedMessage: el mensaje de validación del form tiene
                // prioridad sobre cualquier popup informativo capturado en pasos anteriores.
                var validacion = CapturarValidaciones();
                _lastObservedMessage = !string.IsNullOrWhiteSpace(validacion)
                    ? validacion
                    : "Formulario incompleto (sin mensaje de validación visible)";
                Log($"Guardar DESHABILITADO — Validación activa: '{_lastObservedMessage}'");
                return;
            }

            utilities.ScrollViewElement(btn);
            btn.Click();
            Thread.Sleep(2000);
            _wasSaveExecuted = true;

            // Resultado del guardado: form reiniciado = éxito, mensaje visible = error post-guardado
            var msg = CaptureVisibleMessage(3);
            if (IsNewSaleFormReset())
                _lastObservedMessage = "Se registró correctamente";
            else if (!string.IsNullOrWhiteSpace(msg))
                _lastObservedMessage = msg;

            Log($"Resultado: Habilitado={_wasSaveEnabled}, Ejecutado={_wasSaveExecuted}, Mensaje='{_lastObservedMessage}'");
        }

        // Then: verifica el mensaje de confirmacion X
        public void VerifyConfirmationMessage(string mensajeEsperado)
        {
            if (string.IsNullOrWhiteSpace(mensajeEsperado))
            {
                Log("Sin mensaje de confirmación esperado. Validación omitida.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_lastObservedMessage))
                _lastObservedMessage = CaptureVisibleMessage(2);

            if (string.IsNullOrWhiteSpace(_lastObservedMessage) && IsNewSaleFormReset())
                _lastObservedMessage = "Se registró correctamente";

            Assert.That(NormalizeText(_lastObservedMessage), Does.Contain(NormalizeText(mensajeEsperado)),
                $"Mensaje de confirmación incorrecto. Esperado='{mensajeEsperado}', Actual='{_lastObservedMessage}'");

            TryCloseSuccessDialog();
        }

        private void WaitForFormReady()
        {
            if (!driver.FindElements(VentasLocators.NuevaVenta.IgvCheck).Any(e => e.Displayed))
            {
                var baseUrl = new Uri(driver.Url).GetLeftPart(UriPartial.Authority);
                driver.Navigate().GoToUrl(baseUrl + "/sales/new-sales");
                Thread.Sleep(3000);
            }
            wait.Until(_ => driver.FindElements(VentasLocators.NuevaVenta.IgvCheck).Any(e => e.Displayed));
            Thread.Sleep(1000);
        }

        private void ToggleIgvAndDetUnif()
        {
            Log("Marcar IGV");
            Click(VentasLocators.NuevaVenta.IgvCheck);
            Thread.Sleep(1000);

            Log("Marcar DET.UNIF");
            Click(VentasLocators.NuevaVenta.DetUnifCheck);
            Thread.Sleep(1000);
        }

        private void SelectProduct(string family, string concept)
        {
            Log("Seleccionar Familia");
            Click(VentasLocators.NuevaVenta.FamiliaDropdown);
            Thread.Sleep(1000);

            var familiaInput = Find(VentasLocators.NuevaVenta.FamiliaSearchInput);
            familiaInput.Clear();
            familiaInput.SendKeys(family);
            Thread.Sleep(1000);

            // Click en la opción que coincide con el texto (case-insensitive)
            ClickWithoutScroll(
                By.XPath($"//div[contains(@class,'options-container')]//span[contains(@class,'option-label') and contains(translate(normalize-space(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),'{family.ToLower()}')]")
            );
            Thread.Sleep(1000);

            Log("Seleccionar Concepto");
            Click(VentasLocators.NuevaVenta.ConceptoDropdown);
            Thread.Sleep(1000);

            var conceptoInput = Find(VentasLocators.NuevaVenta.ConceptoSearchInput);
            conceptoInput.Clear();
            conceptoInput.SendKeys(concept); 
            Thread.Sleep(1000);

            ClickWithoutScroll(VentasLocators.NuevaVenta.ConceptoOpcion);
            Thread.Sleep(1000);
        }

        private void UpdateQuantity(string cantidad)
        {
            if (!string.IsNullOrWhiteSpace(cantidad))
            {
                Log($"Actualizando Cantidad a {cantidad}");
                var quantityInput = Find(VentasLocators.Detail.QuantityInputs);
                quantityInput.Clear();
                quantityInput.SendKeys(cantidad);
                quantityInput.SendKeys(Keys.Tab);
                Thread.Sleep(500);
            }
        }

        private void ExpandBillingAccordion()
        {
            Log("Abrir acordeón Facturación");

            // Si el contenido de facturación ya está visible (dropdown), no hacer click para evitar CERRAR el acordeón abierto.
            bool yaAbierto = driver.FindElements(VentasLocators.NuevaVenta.ComprobanteDropdown)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (yaAbierto)
            {
                Log("Acordeón Facturación ya está abierto. No se cierra.");
                return;
            }

            Click(
                VentasLocators.Voucher.BillingAccordion,
                VentasLocators.Voucher.BillingAccordionFallback
            );
            Thread.Sleep(1000);

            // Validar que el acordeón se expandió correctamente
            bool abiertoDespuesDeClick = driver.FindElements(VentasLocators.NuevaVenta.ComprobanteDropdown)
                .Any(e => { try { return e.Displayed; } catch { return false; } });
            Assert.That(abiertoDespuesDeClick, Is.True,
                "El acordeón Facturación no se expandió correctamente. El dropdown Comprobante no es visible.");
        }

        private void SelectVoucherTypeAndSeries(string voucherText, string seriesText)
        {
            Log($"Paso 6a - Abrir dropdown Comprobante");

            // Localizar el dropdown y hacer scroll para asegurar visibilidad
            var dropdownEl = Find(VentasLocators.NuevaVenta.ComprobanteDropdown);
            Assert.That(dropdownEl, Is.Not.Null,
                "No se encontró el dropdown de Comprobante en la sección Facturación.");
            utilities.ScrollViewElement(dropdownEl);
            Thread.Sleep(500);

            dropdownEl.Click();
            Thread.Sleep(1000);

            // Validar que el dropdown se abrió (options-container visible)
            var optionsLocator = By.CssSelector("div.options-container");
            bool dropdownAbierto = driver.FindElements(optionsLocator)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (!dropdownAbierto)
            {
                Log("AVISO: Dropdown comprobante no se abrió. Reintentando con scroll...");
                utilities.ScrollViewElement(dropdownEl);
                Thread.Sleep(300);
                dropdownEl.Click();
                Thread.Sleep(1000);

                dropdownAbierto = driver.FindElements(optionsLocator)
                    .Any(e => { try { return e.Displayed; } catch { return false; } });
            }

            Assert.That(dropdownAbierto, Is.True,
                $"El dropdown de Comprobante no se abrió después de 2 intentos. No se puede seleccionar '{voucherText}'.");

            Log($"Paso 6b - Seleccionar opción ({voucherText})");
            Click(VentasLocators.NuevaVenta.ComprobanteOpcionPorTexto(voucherText));
            Thread.Sleep(1000);

            // Validar que el dropdown se cerró (opción fue seleccionada)
            Thread.Sleep(300);
            bool dropdownCerrado = !driver.FindElements(optionsLocator)
                .Any(e => { try { return e.Displayed; } catch { return false; } });
            Log($"Opción '{voucherText}' seleccionada. Dropdown cerrado: {dropdownCerrado}");

            // Cuando el comprobante tiene una sola serie asignada, el sistema la auto-asigna
            // y NO muestra los radio buttons. Solo se selecciona si son visibles (multiples series).
            bool haySeriesVisibles = driver.FindElements(VentasLocators.Voucher.SeriesRadio)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (!haySeriesVisibles)
            {
                Log($"Paso 8 - Serie auto-asignada (comprobante tiene una sola serie). Serie esperada: {seriesText}");
                return;
            }

            Log($"Paso 8 - Seleccionar Serie {seriesText}");
            Click(
                VentasLocators.NuevaVenta.SeriePorTexto(seriesText),
                VentasLocators.Voucher.SeriesByText(seriesText),
                VentasLocators.NuevaVenta.SerieCheckmark,
                VentasLocators.NuevaVenta.SerieCheckmarkXpath
            );
            Thread.Sleep(1000);
        }

        private void SelectDeliveryType(string entrega)
        {
            Log("Verificar/Abrir sección Entrega");

            // ImmediateLabel en lugar de Immediate: los radio buttons en Angular suelen estar
            // ocultos con CSS (opacity:0 / position:absolute) por lo que Displayed=false
            // aunque la sección esté abierta. El label siempre es visible.
            bool entregaYaVisible = driver.FindElements(VentasLocators.Delivery.ImmediateLabel)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (!entregaYaVisible)
            {
                Log("Sección Entrega cerrada. Abriendo acordeón...");
                Click(
                    VentasLocators.NuevaVenta.AccordionEntrega,
                    VentasLocators.NuevaVenta.AccordionEntregaFallback1
                );
                Thread.Sleep(1000);

                bool abiertaDespuesDeClick = driver.FindElements(VentasLocators.Delivery.ImmediateLabel)
                    .Any(e => { try { return e.Displayed; } catch { return false; } });
                Assert.That(abiertaDespuesDeClick, Is.True,
                    "La sección Entrega no se abrió. Verifique los selectores del acordeón.");
            }
            else
            {
                Log("Sección Entrega ya está abierta.");
            }

            if (!string.IsNullOrWhiteSpace(entrega) && entrega.Equals("Diferida", StringComparison.OrdinalIgnoreCase))
            {
                Log("Seleccionando tipo Diferida");
                Click(VentasLocators.NuevaVenta.EntregaDiferida);
            }
            else
            {
                Log("Seleccionando tipo Inmediata");
                // ImmediateLabel en lugar del radio oculto: equivalente y más robusto
                Click(VentasLocators.Delivery.ImmediateLabel);
            }
            Thread.Sleep(1000);
        }

        private void UpdatePayment(string tipoPago, string montoInicial)
        {
            bool sinCambio = string.IsNullOrWhiteSpace(tipoPago) || tipoPago.Trim() == "-";
            if (sinCambio) return;

            Log($"Configurando pago: tipoPago={tipoPago}, montoInicial={montoInicial}");

            bool pagoYaVisible = driver.FindElements(VentasLocators.Payment.CashTypeLabelText)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (!pagoYaVisible)
            {
                try {
                    Click(VentasLocators.Payment.PaymentAccordionButton,
                          VentasLocators.Payment.PaymentAccordionButtonFallback);
                    Thread.Sleep(1000);
                } catch { }
            }

            if (tipoPago.Equals("Contado", StringComparison.OrdinalIgnoreCase))
            {
                Click(VentasLocators.Payment.CashTypeLabelText,
                      VentasLocators.Payment.CashTypeLabel);
                Thread.Sleep(1000);
                VerificarCamposPagoAutoRelleno();
                return;
            }
            Click(VentasLocators.Payment.CreditTypeLabelText,
                  VentasLocators.Payment.QuickCreditTypeLabel);
            Thread.Sleep(1000);

            bool tieneMonto = !string.IsNullOrWhiteSpace(montoInicial) && montoInicial.Trim() != "-";
            if (tieneMonto)
            {
                var montoInput = Find(VentasLocators.Payment.CreditInitialAmountInput);
                montoInput.Clear();
                montoInput.SendKeys(montoInicial);
                montoInput.SendKeys(Keys.Tab);
                Thread.Sleep(1000);

                var recibido = Find(VentasLocators.Payment.CashReceivedNewSale);
                recibido.Clear();
                recibido.SendKeys(montoInicial);
                recibido.SendKeys(Keys.Tab);
                Thread.Sleep(1000);
            }
        }

        private void UpdatePayment(string pago)
        {
            if (string.IsNullOrWhiteSpace(pago)) return;

            if (pago.Equals("Incompleto", StringComparison.OrdinalIgnoreCase))
            {
                Log("Modificando pago a incompleto...");
                try {
                    Click(VentasLocators.Payment.PaymentAccordionButton,
                          VentasLocators.Payment.PaymentAccordionButtonFallback);
                    Thread.Sleep(1000);
                } catch { }

                var amountInput = Find(VentasLocators.Payment.CashReceivedNewSale);
                amountInput.Clear();
                amountInput.SendKeys("1");
                amountInput.SendKeys(Keys.Tab);
                Thread.Sleep(1000);
            }
            else if (pago.Equals("Credito", StringComparison.OrdinalIgnoreCase))
            {
                Log("Configurando pago a crédito rápido (sin contado, con cuotas)...");
                bool pagoYaVisible = driver.FindElements(VentasLocators.Payment.CreditTypeLabelText)
                    .Any(e => { try { return e.Displayed; } catch { return false; } });
                if (!pagoYaVisible)
                {
                    try {
                        Click(VentasLocators.Payment.PaymentAccordionButton,
                              VentasLocators.Payment.PaymentAccordionButtonFallback);
                        Thread.Sleep(1000);
                    } catch { }
                }
                Click(VentasLocators.Payment.CreditTypeLabelText,
                      VentasLocators.Payment.QuickCreditTypeLabel);
                Thread.Sleep(1000);
            }
            else if (pago.Equals("CreditoInicial", StringComparison.OrdinalIgnoreCase))
            {
                Log("Configurando pago a crédito con monto inicial (contado parcial + cuotas)...");
                bool pagoYaVisible = driver.FindElements(VentasLocators.Payment.CreditTypeLabelText)
                    .Any(e => { try { return e.Displayed; } catch { return false; } });
                if (!pagoYaVisible)
                {
                    try {
                        Click(VentasLocators.Payment.PaymentAccordionButton,
                              VentasLocators.Payment.PaymentAccordionButtonFallback);
                        Thread.Sleep(1000);
                    } catch { }
                }
                Click(VentasLocators.Payment.CreditTypeLabelText,
                      VentasLocators.Payment.QuickCreditTypeLabel);
                Thread.Sleep(1000);

                var montoInicial = Find(VentasLocators.Payment.CreditInitialAmountInput);
                montoInicial.Clear();
                montoInicial.SendKeys("1");
                montoInicial.SendKeys(Keys.Tab);
                Thread.Sleep(1000);

                var recibido = Find(VentasLocators.Payment.CashReceivedNewSale);
                recibido.Clear();
                recibido.SendKeys("1");
                recibido.SendKeys(Keys.Tab);
                Thread.Sleep(1000);
            }
        }

        public void ValidateSale(VentaExpectation expectation)
        {
            if (expectation.SaveShouldBeEnabled.HasValue)
            {
                var esperado = expectation.SaveShouldBeEnabled.Value;
                if (_wasSaveEnabled && !esperado)
                {
                    Log("ERROR: La venta se GUARDÓ. El botón estaba HABILITADO cuando debería estar INHABILITADO (Factura a cliente DNI sin RUC).");
                }
                
                Assert.That(_wasSaveEnabled, Is.EqualTo(esperado),
                    $"Resultado Guardar: esperado={(esperado ? "HABILITADO (DEBE GUARDAR)" : "INHABILITADO (NO DEBE GUARDAR)")}, actual={(_wasSaveEnabled ? "HABILITADO (GUARDÓ LA VENTA)" : "INHABILITADO (NO GUARDÓ)")}.");
            }

            if (expectation.SaveShouldBeExecuted.HasValue)
            {
                Assert.That(_wasSaveExecuted, Is.EqualTo(expectation.SaveShouldBeExecuted.Value),
                    $"Ejecución del guardado inválida. Esperado={expectation.SaveShouldBeExecuted.Value}, Actual={_wasSaveExecuted}.");
            }

            if (!string.IsNullOrWhiteSpace(expectation.ExpectedMessage))
            {
                Assert.That(NormalizeText(_lastObservedMessage), Does.Contain(NormalizeText(expectation.ExpectedMessage)),
                    $"Mensaje esperado no encontrado. Esperado='{expectation.ExpectedMessage}', Actual='{_lastObservedMessage}'.");
            }
        }

        private string CaptureVisibleMessage(int timeoutSeconds)
        {
            var until = DateTime.UtcNow.AddSeconds(Math.Max(1, timeoutSeconds));
            while (DateTime.UtcNow <= until)
            {
                var message = driver.FindElements(By.XPath("//*[contains(@class,'swal2-html-container') or contains(@class,'swal2-content') or contains(@class,'custom-error-message') or contains(@class,'toast') or contains(@class,'alert')][normalize-space()]"))
                    .Where(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    })
                    .Select(e => e.Text?.Trim())
                    .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

                if (!string.IsNullOrWhiteSpace(message))
                    return message;

                Thread.Sleep(300);
            }

            return string.Empty;
        }

        private void TryCloseSuccessDialog()
        {
            // 1. Cerrar popup "Correcto / Se registró correctamente" (botón OK)
            var okButton = driver.FindElements(By.XPath("//button[normalize-space()='OK' or contains(@class,'ok-button')]"))
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (okButton != null)
            {
                try
                {
                    utilities.ScrollViewElement(okButton);
                    okButton.Click();
                    Thread.Sleep(800);
                }
                catch
                {
                    // Si no se puede cerrar el popup OK, continúa.
                }
            }

            // 2. Cerrar modal "Venta registrada XXXX" (botón Cancelar)
            //    Este modal aparece justo después del OK para ofrecer envío por correo/WhatsApp.
            var cancelButton = driver.FindElements(By.XPath("//button[normalize-space()='Cancelar']"))
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (cancelButton != null)
            {
                try
                {
                    cancelButton.Click();
                    Thread.Sleep(800);
                }
                catch { }
            }
        }

        private bool IsNewSaleFormReset()
        {
            var indicator = driver.FindElements(By.XPath("//*[contains(normalize-space(),'Ningún producto seleccionado') or contains(normalize-space(),'Ningun producto seleccionado')]") )
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });
            return indicator != null;
        }

        private void TryClickOptional(params By[] locators)
        {
            foreach (var loc in locators)
            {
                try
                {
                    var element = driver.FindElements(loc)
                        .FirstOrDefault(e =>
                        {
                            try { return e.Displayed && e.Enabled; }
                            catch { return false; }
                        });

                    if (element == null)
                        continue;

                    utilities.ScrollViewElement(element);
                    element.Click();
                    Thread.Sleep(700);
                    return;
                }
                catch
                {
                }
            }
        }

        private IWebElement Find(params By[] locators)
        {
            foreach (var loc in locators)
            {
                var el = driver.FindElements(loc).FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
                if (el != null) return el;
            }
            throw new NoSuchElementException($"No se encontró: {string.Join(" | ", locators.Select(l => l.ToString()))}");
        }

        private void Click(params By[] locators)
        {
            foreach (var loc in locators)
            {
                try
                {
                    var el = wait.Until(d =>
                    {
                        var elements = d.FindElements(loc);
                        return elements.FirstOrDefault(e => { try { return e.Displayed && e.Enabled; } catch { return false; } });
                    });
                    if (el != null)
                    {
                        utilities.ScrollViewElement(el);
                        el.Click();
                        Thread.Sleep(300);
                        return;
                    }
                }
                catch { continue; }
            }
            throw new NoSuchElementException($"No se pudo hacer clic: {string.Join(" | ", locators.Select(l => l.ToString()))}");
        }

        private void ClickWithoutScroll(params By[] locators)
        {
            foreach (var loc in locators)
            {
                try
                {
                    var el = wait.Until(d =>
                    {
                        var elements = d.FindElements(loc);
                        return elements.FirstOrDefault(e => { try { return e.Displayed && e.Enabled; } catch { return false; } });
                    });
                    if (el != null)
                    {
                        el.Click();
                        Thread.Sleep(300);
                        return;
                    }
                }
                catch { continue; }
            }
            throw new NoSuchElementException($"No se pudo hacer clic (sin scroll): {string.Join(" | ", locators.Select(l => l.ToString()))}");
        }

        // Verifica que el sistema auto-rellenó correctamente Monto y Recibido con el total de la venta,
        // y que Vuelto = 0 (pago exacto). Falla con mensaje claro si los campos quedan vacíos o no coinciden.
        private void VerificarCamposPagoAutoRelleno()
        {
            string Leer(By loc) =>
                driver.FindElements(loc)
                      .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } })
                      ?.GetAttribute("value")?.Trim() ?? string.Empty;

            var monto    = Leer(VentasLocators.Payment.CashAmount);
            var recibido = Leer(VentasLocators.Payment.CashReceivedNewSale);
            var vuelto   = Leer(VentasLocators.Payment.Change);

            Log($"[Pago] Monto={monto} | Recibido={recibido} | Vuelto={vuelto}");

            Assert.That(monto, Is.Not.Empty.And.Not.EqualTo("0"),
                "Campo Monto no fue auto-rellenado con el total de la venta.");
            Assert.That(recibido, Is.EqualTo(monto),
                $"Campo Recibido ({recibido}) no coincide con Monto ({monto}). El auto-relleno falló.");
            Assert.That(vuelto, Is.EqualTo("0").Or.EqualTo("0.00").Or.Empty,
                $"Vuelto ({vuelto}) debería ser 0 cuando Recibido = Monto.");
        }

        private bool IsSaveEnabled()
        {
            try
            {
                // Esperar hasta 3 segundos para dar tiempo a que Angular aplique la directiva [disabled]
                var shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                shortWait.Until(d =>
                {
                    var b = d.FindElements(VentasLocators.NuevaVenta.GuardarVenta)
                             .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
                    if (b == null) return false;
                    var c = b.GetAttribute("class") ?? "";
                    var a = b.GetAttribute("aria-disabled") ?? "";
                    return !b.Enabled || c.Contains("disabled") || a == "true";
                });
            }
            catch { /* Timeout: Probablemente todavía está habilitado */ }

            var btn = driver.FindElements(VentasLocators.NuevaVenta.GuardarVenta)
                            .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
            if (btn == null) return false;
            var classes = btn.GetAttribute("class") ?? "";
            var ariaDisabled = btn.GetAttribute("aria-disabled") ?? "";
            return btn.Enabled && !classes.Contains("disabled") && ariaDisabled != "true";
        }

        public bool WasSaveEnabled => _wasSaveEnabled;

        // Captura el primer mensaje de validación visible: primero toasts/popups bloqueantes,
        // luego invalid-feedback / text-danger inline en el formulario.
        private string CapturarValidaciones()
        {
            var popup = CaptureVisibleMessage(1);
            if (!string.IsNullOrWhiteSpace(popup) && IsBlockingMessage(popup))
                return popup;

            return driver.FindElements(By.XPath(
                    "//*[contains(@class,'invalid-feedback') or contains(@class,'text-danger') or " +
                    "contains(@class,'custom-error-message')][normalize-space()]"))
                .Where(e => { try { return e.Displayed; } catch { return false; } })
                .Select(e => e.Text?.Trim())
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t)) ?? string.Empty;
        }

        // Devuelve true si el mensaje es una validación bloqueante real (error/advertencia del negocio).
        // Devuelve false para mensajes informativos de éxito del sistema que no representan un problema.
        private static void Log(string msg) =>
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {msg}");

        private static bool IsBlockingMessage(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return false;
            var n = NormalizeText(msg);
            // Mensajes informativos del sistema que confirman que el formulario está bien —
            // no son errores de validación ni impiden guardar.
            if (n.Contains("completo los campos") || n.Contains("campos requeridos correctamente"))
                return false;
            return true;
        }

        private static string NormalizeText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var formD = value.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(formD.Length);

            foreach (var c in formD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(char.ToLowerInvariant(c));
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }

    public sealed class SaleHeaderData
    {
        public string SalesFlow { get; set; } = "Nueva Venta";
        public string SaleMode { get; set; } = "VENTA NORMAL";
        public string Family { get; set; } = string.Empty;
        public bool ApplyIgv { get; set; }
        public bool ApplyUnifiedDetail { get; set; }
        public string CustomerType { get; set; } = string.Empty;
        public string CustomerValue { get; set; } = string.Empty;
        public string InvoiceType { get; set; } = string.Empty;
        public string Series { get; set; } = string.Empty;
        public string IssueDate { get; set; } = string.Empty;
        public string DeliveryType { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentAmount { get; set; } = string.Empty;
    }

    public sealed class SaleProductData
    {
        public string Concept { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public string UnitPrice { get; set; } = string.Empty;
    }

    public sealed class DiscountData
    {
        public bool Enabled { get; set; }
        public string Scope { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string TargetProduct { get; set; } = string.Empty;
    }

    public sealed class VentaExpectation
    {
        public bool? SaveShouldBeEnabled { get; set; }
        public bool? SaveShouldBeExecuted { get; set; }
        public string ExpectedMessage { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;
    }
}
