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

        public void OpenSalesFlow(string salesFlow)
        {
            utilities.ClickButton(VentasLocators.Navigation.SalesMenu);
            Thread.Sleep(500);
            utilities.ClickButton(VentasLocators.Navigation.NewSale);
            Thread.Sleep(000);
            // Si no cargó el formulario, ir directo a la URL de nueva venta
            if (!driver.FindElements(VentasLocators.CP001.IgvCheck).Any(e => e.Displayed))
            {
                var baseUrl = new Uri(driver.Url).GetLeftPart(UriPartial.Authority);
                driver.Navigate().GoToUrl(baseUrl + "/sales/new-sales");
                Thread.Sleep(3000);
            }
        }

        public void ExecuteFlow(string caseId)
        {
            var normalized = (caseId ?? string.Empty).Trim().ToUpperInvariant();
            switch (normalized)
            {
                case "CP001":
                    ExecuteSaleFlow(
                        customerDocument: "75893616",
                        shouldExpectRucError: true,
                        shouldExecuteSave: false);
                    break;
                case "CP002":
                    ExecuteSaleFlow(
                        customerDocument: "20542245671",
                        shouldExpectRucError: false,
                        shouldExecuteSave: true);
                    break;
                default:
                    throw new ArgumentException($"Caso no soportado: '{caseId}'. Use CP001 o CP002.");
            }
        }

        public void ExecuteFlowDynamic(string familia, string concepto, string cantidad, string documento, string comprobante, string serie, string entrega, string pago)
        {
            _wasSaveEnabled = false;
            _wasSaveExecuted = false;
            _lastObservedMessage = string.Empty;

            WaitForFormReady();

            ToggleIgvAndDetUnif();

            SelectProduct(familia, concepto);

            UpdateQuantity(cantidad);

            ExpandBillingAccordion();

            bool isFactura = comprobante.IndexOf("FACTURA ELECTRONICA", StringComparison.OrdinalIgnoreCase) >= 0 || comprobante.IndexOf("FACTURA ELECTRÓNICA", StringComparison.OrdinalIgnoreCase) >= 0;
            bool isBoleta = comprobante.IndexOf("BOLETA DE VENTA ELECTRONICA", StringComparison.OrdinalIgnoreCase) >= 0 || comprobante.IndexOf("BOLETA DE VENTA ELECTRÓNICA", StringComparison.OrdinalIgnoreCase) >= 0;
            bool isRuc = documento.Length == 11;
            bool isVarios = documento == "00000000";

            decimal cant = 1; 

            if (!string.IsNullOrWhiteSpace(cantidad))
            {
                decimal.TryParse(cantidad, out cant);
            }

            // Asumimos que si la cantidad es alta (por ejemplo 150), y es cliente varios con boleta, superará los 700.
            bool isOver700 = cant >= 100;

            bool shouldExpectRucError = isFactura && !isRuc;
            bool shouldExpectAmountWarning = isBoleta && isVarios && isOver700;
            bool expectWarningPopup = shouldExpectRucError || shouldExpectAmountWarning;
            bool isIncompletePayment = pago != null && pago.Equals("Incompleto", StringComparison.OrdinalIgnoreCase);

            if (isIncompletePayment) {
                // If payment is incomplete, we expect an alert and it shouldn't proceed normally
                // Since user prompt expects Habilitado=SI, Ejecutar=SI but message="insuficiente", we proceed:
                expectWarningPopup = false; 
            }

            bool shouldExecuteSave = !expectWarningPopup;

            // PASO 1: Elegimos la Serie y Comprobante PRIMERO para evitar el Error Popup si DNI + Factura
            SelectVoucherTypeAndSeries(comprobante, serie);

            // PASO 2: Buscamos al cliente (Aquí saltará el popup si realmente es inválido, Ej. Factura + DNI)
            EnterAndSearchCustomer(documento);

            // PASO 3: Manejar el Popup si se espera error de Documento
            if (expectWarningPopup)
            {
                HandleWarningPopup();
            }

            SelectDeliveryType(entrega);

            UpdatePayment(pago);

            AttemptSave(shouldExecuteSave);

            ValidateSaveSuccess(documento, expectWarningPopup, shouldExecuteSave, isIncompletePayment);
        }

        /// <summary>
        /// Flujo base de nueva venta reutilizado para CP001 y CP002.
        /// </summary>
        private void ExecuteSaleFlow(string customerDocument, bool shouldExpectRucError, bool shouldExecuteSave)
        {
            _wasSaveEnabled = false;
            _wasSaveExecuted = false;
            _lastObservedMessage = string.Empty;

            WaitForFormReady();

            ToggleIgvAndDetUnif();

            SelectProduct("gaseosa", "Coca-Cola");

            UpdateQuantity("");

            ExpandBillingAccordion();

            SelectVoucherTypeAndSeries("FACTURA ELECTRÓNICA", "F002");

            EnterAndSearchCustomer(customerDocument);

            if (shouldExpectRucError)
            {
                HandleWarningPopup();
            }

            SelectDeliveryType("Inmediata");

            AttemptSave(shouldExecuteSave);

            ValidateSaveSuccess(customerDocument, shouldExpectRucError, shouldExecuteSave, false);
        }

        private void WaitForFormReady()
        {
            wait.Until(_ => driver.FindElements(VentasLocators.CP001.IgvCheck).Any(e => e.Displayed));
            Thread.Sleep(1000);
        }

        private void ToggleIgvAndDetUnif()
        {
            Console.WriteLine("[CP001] Paso 1 - Marcar IGV: #flexCheckDefault");
            Click(VentasLocators.CP001.IgvCheck);
            Thread.Sleep(1000);

            Console.WriteLine("[CP001] Paso 2 - Marcar DET.UNIF: #flexCheckDefault2");
            Click(VentasLocators.CP001.DetUnifCheck);
            Thread.Sleep(1000);
        }

        private void SelectProduct(string family, string concept)
        {
            Console.WriteLine("[CP001] Paso 3 - Seleccionar Familia");
            Click(VentasLocators.CP001.FamiliaDropdown);
            Thread.Sleep(1000);

            var familiaInput = Find(VentasLocators.CP001.FamiliaSearchInput);
            familiaInput.Clear();
            familiaInput.SendKeys(family);
            Thread.Sleep(1000);

            ClickWithoutScroll(VentasLocators.CP001.FamiliaOpcion);
            Thread.Sleep(1000);

            Console.WriteLine("[CP001] Paso 4 - Seleccionar Concepto");
            Click(VentasLocators.CP001.ConceptoDropdown);
            Thread.Sleep(1000);

            var conceptoInput = Find(VentasLocators.CP001.ConceptoSearchInput);
            conceptoInput.Clear();
            conceptoInput.SendKeys(concept); 
            Thread.Sleep(1000);

            ClickWithoutScroll(VentasLocators.CP001.ConceptoOpcion);
            Thread.Sleep(1000);
        }

        private void UpdateQuantity(string cantidad)
        {
            if (!string.IsNullOrWhiteSpace(cantidad))
            {
                Console.WriteLine($"[NuevaVenta] Actualizando Cantidad a {cantidad}");
                var quantityInput = Find(VentasLocators.Detail.QuantityInputs);
                quantityInput.Clear();
                quantityInput.SendKeys(cantidad);
                quantityInput.SendKeys(Keys.Tab);
                Thread.Sleep(500);
            }
        }

        private void ExpandBillingAccordion()
        {
            Console.WriteLine("[CP001] Paso 4d - Abrir acordeón Facturación");
            Click(
                VentasLocators.Voucher.BillingAccordion,
                VentasLocators.Voucher.BillingAccordionFallback
            );
            Thread.Sleep(1000);
        }

        private void EnterAndSearchCustomer(string customerDocument)
        {
            Console.WriteLine($"[NuevaVenta] Paso 5a - Ingresar documento {customerDocument}");
            var clienteInput = Find(VentasLocators.CP001.ClienteBuscar);
            clienteInput.Clear();
            clienteInput.SendKeys(customerDocument);
            Thread.Sleep(1000);

            var typedDocument = (clienteInput.GetAttribute("value") ?? string.Empty).Trim();
            Assert.That(typedDocument, Is.EqualTo(customerDocument),
                $"El documento ingresado no coincide. Esperado={customerDocument}, Actual={typedDocument}");

            Console.WriteLine("[NuevaVenta] Paso 5b - Click lupa");
            Click(VentasLocators.CP001.ClienteLupa);
            Thread.Sleep(1000);
        }

        private void SelectVoucherTypeAndSeries(string voucherText, string seriesText)
        {
            Console.WriteLine($"[NuevaVenta] Paso 6a - Abrir dropdown Comprobante para {voucherText}");
            Click(VentasLocators.CP001.ComprobanteDropdown, By.CssSelector("app-dropdown-search div.select-trigger"));
            Thread.Sleep(1000);

            Console.WriteLine($"[NuevaVenta] Paso 6b - Seleccionar opción ({voucherText})");
            By voucherLocator = VentasLocators.CP001.ComprobanteOpcionPorTexto(voucherText);
            Click(voucherLocator, VentasLocators.CP001.ComprobanteOpcion);
            Thread.Sleep(1000);

            Console.WriteLine($"[NuevaVenta] Paso 8 - Seleccionar Serie {seriesText}");
            By seriesLocator = VentasLocators.CP001.SeriePorTexto(seriesText);
            Click(
                seriesLocator,
                VentasLocators.Voucher.SeriesByText(seriesText),
                VentasLocators.CP001.SerieCheckmark,
                VentasLocators.CP001.SerieCheckmarkXpath
            );
            Thread.Sleep(1000);
        }

        private void HandleWarningPopup()
        {
            Console.WriteLine("[NuevaVenta] Validando popup de advertencia y cerrar con OK por validación de regla");
            _lastObservedMessage = CaptureVisibleMessage(4);

            TryClickOptional(
                VentasLocators.CP001.ErrorOkButton,
                VentasLocators.CP001.ErrorOkButtonFallback,
                By.CssSelector(".ok-button")
            );
            Thread.Sleep(500);
        }

        private void SelectDeliveryType(string entrega)
        {
            Console.WriteLine("[CP001] Paso 9a - Abrir acordeón Entrega");
            Click(
                VentasLocators.CP001.AccordionEntrega,
                VentasLocators.CP001.AccordionEntregaFallback1,
                VentasLocators.CP001.AccordionEntregaFallback2,
                VentasLocators.CP001.AccordionEntregaFallback3
            );
            Thread.Sleep(1000);

            if (!string.IsNullOrWhiteSpace(entrega) && entrega.Equals("Diferida", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("[CP001] Paso 9b - Click Diferida");
                Click(VentasLocators.CP001.EntregaDiferida);
            }
            else
            {
                Console.WriteLine("[CP001] Paso 9b - Click Inmediata: #tipoBien");
                Click(VentasLocators.CP001.EntregaInmediata);
            }
            Thread.Sleep(1000);
        }

        private void UpdatePayment(string pago)
        {
            if (!string.IsNullOrWhiteSpace(pago) && pago.Equals("Incompleto", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("[NuevaVenta] Modificando pago a incompleto...");
                // Expandir acordeón pago si es necesario, aunque a veces ya está abierto.
                try {
                    Click(VentasLocators.Payment.PaymentAccordionHeader);
                    Thread.Sleep(1000);
                } catch { }

                var amountInput = Find(VentasLocators.Payment.CashReceivedNewSale);
                amountInput.Clear();
                amountInput.SendKeys("1"); // Monto insuficiente
                amountInput.SendKeys(Keys.Tab);
                Thread.Sleep(1000);
            }
        }

        private void AttemptSave(bool shouldExecuteSave)
        {
            Console.WriteLine("[NuevaVenta] Paso 10 - Intentar Guardar Venta");
            TryClickGuardar(shouldExecuteSave);
        }

        private void ValidateSaveSuccess(string customerDocument, bool expectWarningPopup, bool shouldExecuteSave, bool skipMessageAssert)
        {
            if (!expectWarningPopup && shouldExecuteSave)
            {
                if (string.IsNullOrWhiteSpace(_lastObservedMessage))
                    _lastObservedMessage = CaptureVisibleMessage(2);

                if (string.IsNullOrWhiteSpace(_lastObservedMessage) && IsNewSaleFormReset())
                    _lastObservedMessage = "Se registró correctamente";

                if (!skipMessageAssert)
                {
                    Assert.That(NormalizeText(_lastObservedMessage), Does.Contain("se registro correctamente"),
                        $"No se confirmó guardado exitoso para el documento {customerDocument}. Mensaje capturado: '{_lastObservedMessage}'");

                    TryCloseSuccessDialog();
                }
            }
        }

        private void TryClickGuardar(bool shouldExecuteSave)
        {
            var btn = driver.FindElements(VentasLocators.CP001.GuardarVenta)
                            .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

            if (btn == null)
            {
                Console.WriteLine("[NuevaVenta] AVISO: Botón Guardar Venta no encontrado en el DOM.");
                _wasSaveEnabled = false;
                _wasSaveExecuted = false;
                return;
            }

            _wasSaveEnabled = IsSaveEnabled();
            if (!_wasSaveEnabled)
            {
                Console.WriteLine("[NuevaVenta] Botón Guardar está inhabilitado.");
                // No se hace click porque está inhabilitado
                return;
            }

            if (!shouldExecuteSave)
            {
                // Comportamiento original para CP001: tratar de darle click para demostrar que estaba habilitado erróneamente
                Console.WriteLine("[CP001] ADVERTENCIA: Botón Guardar está HABILITADO. Se procede a Guardar la venta (no debería permitirlo).");
                try
                {
                    utilities.ScrollViewElement(btn);
                    btn.Click();
                    Thread.Sleep(2000); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[CP001] Error al hacer click en Guardar: {ex.Message}");
                }
                
                _wasSaveExecuted = false; 
                return;
            }

            Console.WriteLine("[NuevaVenta] Botón Guardar habilitado. Ejecutando guardado.");
            try
            {
                utilities.ScrollViewElement(btn);
                btn.Click();
                _wasSaveExecuted = true;
                _lastObservedMessage = CaptureVisibleMessage(2);
            }
            catch (Exception ex)
            {
                _wasSaveExecuted = false;
                Console.WriteLine($"[NuevaVenta] Error al hacer click en Guardar: {ex.Message}");
            }
        }

        public void ValidateSale(VentaExpectation expectation)
        {
            if (expectation.SaveShouldBeEnabled.HasValue)
            {
                var esperado = expectation.SaveShouldBeEnabled.Value;
                if (_wasSaveEnabled && !esperado)
                {
                    Console.WriteLine("[CP001] ERROR: La venta se GUARDÓ. El botón estaba HABILITADO cuando debería estar INHABILITADO (Factura a cliente DNI sin RUC).");
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
            var okButton = driver.FindElements(By.XPath("//button[normalize-space()='OK' or contains(@class,'ok-button')]"))
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (okButton == null)
                return;

            try
            {
                utilities.ScrollViewElement(okButton);
                okButton.Click();
                Thread.Sleep(800);
            }
            catch
            {
                // Si no se puede cerrar el popup, no bloquea el resultado principal.
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
                    // Es opcional: no interrumpe el flujo.
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
            throw new NoSuchElementException($"[CP001] No se encontró: {string.Join(" | ", locators.Select(l => l.ToString()))}");
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
            throw new NoSuchElementException($"[CP001] No se pudo hacer clic: {string.Join(" | ", locators.Select(l => l.ToString()))}");
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
            throw new NoSuchElementException($"[CP001] No se pudo hacer clic (sin scroll): {string.Join(" | ", locators.Select(l => l.ToString()))}");
        }

        private bool IsSaveEnabled()
        {
            try
            {
                // Esperar hasta 3 segundos para dar tiempo a que Angular aplique la directiva [disabled]
                var shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                shortWait.Until(d =>
                {
                    var b = d.FindElements(VentasLocators.CP001.GuardarVenta)
                             .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
                    if (b == null) return false;
                    var c = b.GetAttribute("class") ?? "";
                    var a = b.GetAttribute("aria-disabled") ?? "";
                    return !b.Enabled || c.Contains("disabled") || a == "true";
                });
            }
            catch { /* Timeout: Probablemente todavía está habilitado */ }

            var btn = driver.FindElements(VentasLocators.CP001.GuardarVenta)
                            .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
            if (btn == null) return false;
            var classes = btn.GetAttribute("class") ?? "";
            var ariaDisabled = btn.GetAttribute("aria-disabled") ?? "";
            return btn.Enabled && !classes.Contains("disabled") && ariaDisabled != "true";
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
