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

        // ─── MODO DE VENTA ────────────────────────────────────────────────────────────

        // Paso: selecciona el modo de venta (VENTA NORMAL / VENTA MODO CAJA / VENTA POR CONTINGENCIA)
        // Resetea el estado del escenario y espera que el formulario esté listo.
        public void SelectSaleModeFlow(string modo)
        {
            _wasSaveEnabled = false;
            _wasSaveExecuted = false;
            _lastObservedMessage = string.Empty;

            WaitForFormReady();

            if (string.IsNullOrWhiteSpace(modo) || modo.Trim() == "-")
                return;

            Log($"Seleccionando modo de venta: {modo}");
            Click(VentasLocators.NuevaVenta.ModoVenta(modo));
            Thread.Sleep(1000);
        }

        // Paso: ingresa la fecha de emision (solo para Venta Modo Caja y Contingencia)
        public void SetFechaEmisionFlow(string fecha)
        {
            if (string.IsNullOrWhiteSpace(fecha) || fecha.Trim() == "-")
                return;

            Log($"Ingresando fecha de emisión: {fecha}");
            var input = Find(VentasLocators.NuevaVenta.FechaEmision);
            input.Clear();
            input.SendKeys(fecha);
            input.SendKeys(Keys.Tab);
            Thread.Sleep(500);
        }

        // ─── DETALLE ─────────────────────────────────────────────────────────────────

        // Paso: configura IGV Y|N y Detalle Unificado Y|N
        public void ConfigurarIgvDetUnif(string igv, string detUnificado)
        {
            bool activarIgv = igv.Equals("Y", StringComparison.OrdinalIgnoreCase);
            bool activarDet = detUnificado.Equals("Y", StringComparison.OrdinalIgnoreCase);
            Log($"Configurando IGV={igv}, DetUnificado={detUnificado}");
            SetCheckbox(VentasLocators.NuevaVenta.IgvCheck, activarIgv);
            Thread.Sleep(500);
            SetCheckbox(VentasLocators.NuevaVenta.DetUnifCheck, activarDet);
            Thread.Sleep(500);
        }

        // ─── FACTURACIÓN ─────────────────────────────────────────────────────────────

        // Paso: selecciona el punto de venta (solo para Venta Modo Caja)
        public void SelectPuntoVentaFlow(string puntoVenta)
        {
            if (string.IsNullOrWhiteSpace(puntoVenta) || puntoVenta.Trim() == "-")
                return;

            Log($"Seleccionando punto de venta: {puntoVenta}");

            // El dropdown puede estar ya abierto (bi-chevron-up) si la sección se expandió antes.
            // Verificar sin espera: si la opción ya es visible, no hacer clic en el chevron.
            var opcionVisible = driver.FindElements(VentasLocators.NuevaVenta.PuntoVentaOpcion(puntoVenta))
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (!opcionVisible)
            {
                Click(VentasLocators.NuevaVenta.PuntoVentaChevron);
                Thread.Sleep(800);
            }

            Click(VentasLocators.NuevaVenta.PuntoVentaOpcion(puntoVenta));
            Thread.Sleep(800);
        }

        // Paso: selecciona el vendedor (solo para Venta Modo Caja)
        public void SelectVendorFlow(string vendedor)
        {
            if (string.IsNullOrWhiteSpace(vendedor) || vendedor.Trim() == "-")
                return;

            Log($"Seleccionando vendedor: {vendedor}");
            Click(VentasLocators.NuevaVenta.VendedorChevron);
            Thread.Sleep(800);
            Click(VentasLocators.NuevaVenta.VendedorOption(vendedor));
            Thread.Sleep(800);
        }

        // Paso: busca cliente, selecciona comprobante y serie en la sección Facturación.
        public void ConfigurarFacturacionNuevaVenta(string comprobante, string serie, string cliente)
        {
            AbrirSeccionFacturacionSiNecesario();
            BuscarClienteNuevaVenta(cliente);
            SeleccionarComprobanteNuevaVenta(comprobante);

            Thread.Sleep(500);
            var popup = CaptureVisibleMessage(2);
            if (!string.IsNullOrWhiteSpace(popup) && IsBlockingMessage(popup) && string.IsNullOrWhiteSpace(_lastObservedMessage))
            {
                Log($"Popup bloqueante en Facturación: {popup}");
                _lastObservedMessage = popup;
                TryClickOptional(
                    VentasLocators.NuevaVenta.ErrorOkButton,
                    VentasLocators.NuevaVenta.ErrorOkButtonFallback,
                    By.CssSelector(".ok-button")
                );
                return;
            }

            SeleccionarSerieNuevaVenta(serie);
        }

        // ─── ENTREGA ─────────────────────────────────────────────────────────────────

        // Paso: abre acordeón Entrega, selecciona tipo (Inmediata/Diferida) y abre Guía de remisión si aplica.
        public void ConfigurarEntregaNuevaVenta(string entrega, string guiaRemision)
        {
            Log($"Configurando entrega: tipo='{entrega}', guia='{guiaRemision}'");

            // 1. Abrir acordeón Entrega si los radios aún no son visibles
            bool radiosVisible = driver.FindElements(VentasLocators.Delivery.ImmediateLabel)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (!radiosVisible)
            {
                Log("Abriendo sección Entrega...");
                TryClickOptional(
                    VentasLocators.NuevaVenta.AccordionEntrega,
                    VentasLocators.NuevaVenta.AccordionEntregaFallback1
                );
                Thread.Sleep(800);
            }

            // 2. Seleccionar tipo de entrega
            if (entrega.Trim().Equals("Inmediata", StringComparison.OrdinalIgnoreCase))
                TryClickOptional(VentasLocators.Delivery.ImmediateLabel, VentasLocators.Delivery.Immediate);
            else if (entrega.Trim().Equals("Diferida", StringComparison.OrdinalIgnoreCase))
                TryClickOptional(VentasLocators.NuevaVenta.EntregaDiferida, VentasLocators.Delivery.DeferredLabel);
            Thread.Sleep(500);

            // 3. Si GuiaRemision = false, no hay nada más que hacer
            if (!guiaRemision.Trim().Equals("true", StringComparison.OrdinalIgnoreCase)) return;

            // 4. Buscar botón "Guía de remisión" — la estructura del DOM en NuevaVenta puede diferir
            //    de VerPedidos (donde el botón está en //div[@id='collapse-entrega']).
            //    Se intenta con locators progresivamente más amplios para mayor robustez.
            Thread.Sleep(500);
            IWebElement? btnGuia = null;
            try
            {
                var shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                btnGuia = shortWait.Until(d =>
                {
                    // Intento 1: dentro de #collapse-entrega (estructura VerPedidos)
                    var b = d.FindElements(By.XPath("//div[@id='collapse-entrega']//button[contains(normalize-space(),'remi')]"))
                             .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
                    if (b != null) return b;
                    // Intento 2: cualquier <button> visible que contenga 'remi' (sin restricción de contenedor)
                    b = d.FindElements(By.XPath("//button[contains(normalize-space(),'remi')]"))
                         .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
                    if (b != null) return b;
                    // Intento 3: <a> o <div class='btn'> con 'remi' (Angular puede renderizar botones como otros elementos)
                    return d.FindElements(By.XPath("//*[self::a or (self::div and contains(@class,'btn'))][contains(normalize-space(),'remi')]"))
                             .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
                });
            }
            catch { btnGuia = null; }

            if (btnGuia == null)
            {
                // Diagnóstico: listar todos los botones visibles para identificar el locator correcto
                Log("=== DIAGNÓSTICO: botones visibles en página ===");
                foreach (var b in driver.FindElements(By.XPath("//button | //a[contains(@class,'btn')]"))
                    .Where(e => { try { return e.Displayed; } catch { return false; } }))
                {
                    try { Log($"  ELEM: <{b.TagName}> text='{b.Text?.Trim()}' class='{b.GetAttribute("class")}' id='{b.GetAttribute("id")}'"); }
                    catch { }
                }
                Log("=== FIN DIAGNÓSTICO ===");
                Log("Botón 'Guia de remisión' no encontrado.");
                _lastObservedMessage = "Boton de guia de remision no encontrado";
                return;
            }

            bool deshabilitado = !btnGuia.Enabled
                || btnGuia.GetAttribute("disabled") != null
                || (btnGuia.GetAttribute("class") ?? "").Contains("disabled")
                || !btnGuia.GetCssValue("pointer-events").Equals("auto", StringComparison.OrdinalIgnoreCase);

            if (deshabilitado)
            {
                Log("Botón 'Guia de remisión' deshabilitado — cliente sin RUC/DNI.");
                _lastObservedMessage = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                return;
            }

            ScrollToCenter(btnGuia);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuia);
            Thread.Sleep(1200);
            Log("'Guia de remisión' abierta correctamente.");
        }

        // ─── GUÍA DE REMISIÓN (NuevaVenta) ───────────────────────────────────────────

        // GuiaRemisionPage.txtPesoBruto/txtNumeroBultos usan clases Bootstrap (g-2 mb-3)
        // que no existen en el formulario de NuevaVenta — se anclan al label en su lugar.
        public void IngresarPesoBrutoNV(string valor)
        {
            if (EsValorOmitible(valor)) return;
            EscribirCampoGuia(By.XPath(
                "//label[contains(normalize-space(),'Peso') or contains(normalize-space(),'PESO')]" +
                "/following::input[not(@type='hidden') and not(@type='date')][1]"), valor.Trim());
        }

        public void IngresarNumeroBultosNV(string valor)
        {
            if (EsValorOmitible(valor)) return;
            EscribirCampoGuia(By.XPath(
                "//label[contains(normalize-space(),'Bulto') or contains(normalize-space(),'BULTO')]" +
                "/following::input[not(@type='hidden') and not(@type='date')][1]"), valor.Trim());
        }

        private static bool EsValorOmitible(string valor) =>
            string.IsNullOrWhiteSpace(valor) ||
            valor.Trim().Equals("NA", StringComparison.OrdinalIgnoreCase);

        private void EscribirCampoGuia(By locator, string valor)
        {
            var el = Find(locator);
            ScrollToCenter(el);
            try { el.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", el); }
            el.SendKeys(Keys.Control + "a");
            el.SendKeys(Keys.Delete);
            Thread.Sleep(150);
            el.SendKeys(valor);
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                arguments[0].blur();
            ", el);
            el.SendKeys(Keys.Tab);
            Thread.Sleep(400);
        }

        // ─── PAGO, GUARDAR Y VALIDAR ──────────────────────────────────────────────────

        // Then: valida el resultado de venta contra la tabla de decisión
        public void ValidarResultadoVenta(string resultadoEsperado)
        {
            if (string.IsNullOrWhiteSpace(resultadoEsperado) || resultadoEsperado.Trim() == "-")
                return;

            var norm = NormalizeText(resultadoEsperado);
            if (norm.Contains("guarda exitosamente"))
            {
                Assert.That(_wasSaveEnabled, Is.True,
                    $"Guardar debería estar habilitado (venta exitosa). Mensaje capturado: '{_lastObservedMessage}'");
                Assert.That(_wasSaveExecuted, Is.True,
                    "El guardado debería haberse ejecutado.");
                if (string.IsNullOrWhiteSpace(_lastObservedMessage) && IsNewSaleFormReset())
                    _lastObservedMessage = "Se registró correctamente";
                Assert.That(NormalizeText(_lastObservedMessage), Does.Contain("registr").Or.Contain("correct"),
                    $"Mensaje de éxito no encontrado. Actual: '{_lastObservedMessage}'");
            }
            else
            {
                Assert.That(_wasSaveEnabled, Is.False,
                    $"Guardar debería estar deshabilitado. Resultado esperado: '{resultadoEsperado}'. Mensaje capturado: '{_lastObservedMessage}'");
                Log($"Validación no exitosa: esperado='{resultadoEsperado}', capturado='{_lastObservedMessage}'");
            }

            TryCloseSuccessDialog();
        }

        // Paso: configura el pago X
        public void ConfigurePaymentFlow(string pago) => UpdatePayment(pago);

        // Paso: guarda la venta
        // Intenta hacer click en Guardar. Si el botón está deshabilitado, lo informa y no falla.
        // Captura el mensaje resultante sin sobrescribir mensajes de popup previos.
        public void GuardarVentaFlow()
        {
            Log("Paso 10 - Intentando guardar venta...");
            _wasSaveEnabled = false;
            _wasSaveExecuted = false;

            // Cerrar modal bloqueante si existe ANTES de interactuar con el formulario.
            // No retornar: el modal puede ser una advertencia informativa; el estado real
            // del botón Guardar determina si la venta puede proceder.
            if (TryHandleBlockingModal())
            {
                Log("Modal bloqueante cerrado antes de Guardar — continuando con el flujo.");
                Thread.Sleep(500);
            }

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

            ScrollToCenter(btn);
            try
            {
                btn.Click();
            }
            catch (ElementClickInterceptedException)
            {
                Log("ElementClickInterceptedException — modal interceptó el click en Guardar.");
                TryHandleBlockingModal();
                _wasSaveEnabled = false;
                return;
            }
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

        // ─── HELPERS PRIVADOS ────────────────────────────────────────────────────────

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

        private void SetCheckbox(By locator, bool shouldBeChecked)
        {
            var checkbox = driver.FindElements(locator)
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
            if (checkbox == null) return;

            bool isChecked = checkbox.Selected;
            if (isChecked != shouldBeChecked)
            {
                ScrollToCenter(checkbox);
                checkbox.Click();
                Thread.Sleep(300);
            }
        }

        private void BuscarClienteNuevaVenta(string cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente) || cliente == "00000000" || cliente.Trim() == "-")
            {
                Log("Cliente VARIOS / sin identificar — omitiendo búsqueda.");
                return;
            }

            AbrirSeccionFacturacionSiNecesario();

            Log($"Buscando cliente: {cliente}");
            var input = Find(
                By.Id("DocumentoIdentidad"),
                VentasLocators.NuevaVenta.ClienteBuscar,
                VentasLocators.Customer.DocumentFieldByLabel
            );
            ScrollToCenter(input);
            input.Clear();
            input.SendKeys(cliente);
            Thread.Sleep(300);
            try { Click(VentasLocators.NuevaVenta.ClienteLupa); }
            catch { input.SendKeys(Keys.Enter); }
            Thread.Sleep(2000);
        }

        private void AbrirSeccionFacturacionSiNecesario()
        {
            bool visible = driver.FindElements(By.Id("DocumentoIdentidad"))
                .Any(e => { try { return e.Displayed; } catch { return false; } })
                || driver.FindElements(VentasLocators.NuevaVenta.ClienteBuscar)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (visible) return;

            Log("Abriendo sección Facturación...");
            TryClickOptional(
                By.XPath("//div[contains(@id,'heading-collapse-factur')]//button"),
                By.XPath("//button[contains(@class,'accordion-button')][contains(normalize-space(),'Facturaci')]")
            );
            Thread.Sleep(800);
        }

        private void SeleccionarComprobanteNuevaVenta(string comprobante)
        {
            if (string.IsNullOrWhiteSpace(comprobante)) return;
            string textoOpcion = NormalizarTextComprobante(comprobante);
            Log($"Seleccionando comprobante: {textoOpcion}");
            // Paso 1: abrir el dropdown con el chevron
            Click(
                VentasLocators.NuevaVenta.ComprobanteChevron,
                VentasLocators.NuevaVenta.ComprobanteChevronFallback
            );
            Thread.Sleep(800);
            // Paso 2: seleccionar la opción
            Click(
                VentasLocators.NuevaVenta.ComprobanteOpcion(textoOpcion),
                VentasLocators.NuevaVenta.ComprobanteOpcionFallback(textoOpcion)
            );
            Thread.Sleep(800);
        }

        private static string NormalizarTextComprobante(string comprobante)
        {
            string t = (comprobante ?? "").Trim().ToUpperInvariant();
            if (t.Contains("NOTA DE VENTA")) return "NOTA DE VENTA(INTERNA)";
            if (t.Contains("FACTURA"))       return "FACTURA ELECTRONICA";
            if (t.Contains("BOLETA"))        return "BOLETA DE VENTA ELECTRONICA";
            return t;
        }

        private void SeleccionarSerieNuevaVenta(string serie)
        {
            if (string.IsNullOrWhiteSpace(serie) || serie.Trim() == "-") return;
            bool hayRadios =
                driver.FindElements(VentasLocators.Voucher.SeriesRadio)
                    .Any(e => { try { return e.Displayed; } catch { return false; } })
                || driver.FindElements(By.XPath(
                    "//div[contains(@id,'collapse-factur')]//input[@type='radio']"))
                    .Any(e => { try { return e.Displayed; } catch { return false; } });
            if (!hayRadios)
            {
                Log($"Serie auto-asignada (única disponible). Serie esperada: {serie}");
                return;
            }
            Log($"Seleccionando serie: {serie}");
            Click(
                VentasLocators.NuevaVenta.SeriePorTexto(serie),
                VentasLocators.Voucher.SeriesByText(serie)
            );
            Thread.Sleep(500);
        }

        private void UpdatePayment(string pago)
        {
            if (string.IsNullOrWhiteSpace(pago)) return;

            if (driver.FindElements(By.CssSelector(".modal-overlay"))
                .Any(e => { try { return e.Displayed; } catch { return false; } }))
            {
                Log($"[Pago] Modal bloqueante activo — omitiendo configuración de pago '{pago}'.");
                return;
            }

            if (pago.Equals("Contado", StringComparison.OrdinalIgnoreCase))
            {
                Log("Configurando pago Contado...");
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
                TryClickOptional(VentasLocators.Payment.CashTypeLabelText, VentasLocators.Payment.CashTypeLabel);
                Thread.Sleep(500);
                TryClickOptional(VentasLocators.Payment.CashMethod, VentasLocators.Payment.CashMethodFallback);
                Thread.Sleep(800);
            }
            else if (pago.Equals("Incompleto", StringComparison.OrdinalIgnoreCase))
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
                    ScrollToCenter(okButton);
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

        private bool TryHandleBlockingModal()
        {
            bool hayModal = driver.FindElements(By.CssSelector(".modal-overlay"))
                .Any(e => { try { return e.Displayed; } catch { return false; } });
            if (!hayModal) return false;

            Log("Modal bloqueante detectado — capturando mensaje y cerrando.");
            var msg = driver.FindElements(By.CssSelector(".modal-overlay p, .modal-content p, .modal-body p"))
                .Where(e => { try { return e.Displayed; } catch { return false; } })
                .Select(e => e.Text?.Trim())
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t))
                ?? CaptureVisibleMessage(1);

            if (!string.IsNullOrWhiteSpace(msg) && string.IsNullOrWhiteSpace(_lastObservedMessage))
                _lastObservedMessage = msg;

            var okBtn = driver.FindElements(By.XPath("//button[normalize-space()='OK' or normalize-space()='Aceptar']"))
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
            if (okBtn != null)
            {
                try { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", okBtn); }
                catch { }
                Thread.Sleep(800);
            }
            return true;
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

                    ScrollToCenter(element);
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

        private void ScrollToCenter(IWebElement el)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView({block:'center',behavior:'instant'});", el);
            Thread.Sleep(300);
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
                        ScrollToCenter(el);
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

    }
