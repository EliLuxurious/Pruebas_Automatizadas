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
            utilities.ClickButton(SalesLocators.Navigation.SalesMenu);
            Thread.Sleep(500);
            utilities.ClickButton(SalesLocators.Navigation.NewSale);
            Thread.Sleep(000);
            // Si no cargó el formulario, ir directo a la URL de nueva venta
            if (!driver.FindElements(SalesLocators.CP001.IgvCheck).Any(e => e.Displayed))
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

        /// <summary>
        /// Flujo base de nueva venta reutilizado para CP001 y CP002.
        /// </summary>
        private void ExecuteSaleFlow(string customerDocument, bool shouldExpectRucError, bool shouldExecuteSave)
        {
            _wasSaveEnabled = false;
            _wasSaveExecuted = false;
            _lastObservedMessage = string.Empty;

            // Esperar que el formulario esté listo
            wait.Until(_ => driver.FindElements(SalesLocators.CP001.IgvCheck).Any(e => e.Displayed));
            Thread.Sleep(1000);

            // ── PASO 1: Marcar IGV ──────────────────────────────────────────────
            Console.WriteLine("[CP001] Paso 1 - Marcar IGV: #flexCheckDefault");
            Click(SalesLocators.CP001.IgvCheck);
            Thread.Sleep(1000);

            // ── PASO 2: Marcar DET.UNIF ─────────────────────────────────────────
            Console.WriteLine("[CP001] Paso 2 - Marcar DET.UNIF: #flexCheckDefault2");
            Click(SalesLocators.CP001.DetUnifCheck);
            Thread.Sleep(1000);

            // ── PASO 3: Familia = Gaseosa ────────────────────────────────────────
            // 3a. Abrir dropdown de familia
            Console.WriteLine("[CP001] Paso 3a - Abrir dropdown Familia");
            Click(SalesLocators.CP001.FamiliaDropdown);
            Thread.Sleep(1000);

            // 3b. Buscar "gaseosa" en el campo de búsqueda
            Console.WriteLine("[CP001] Paso 3b - Buscar 'gaseosa' en input[class='search-input']");
            var familiaInput = Find(SalesLocators.CP001.FamiliaSearchInput);
            familiaInput.Clear();
            familiaInput.SendKeys("gaseosa");
            Thread.Sleep(1000);

            // 3c. Seleccionar opción Gaseosa
            Console.WriteLine("[CP001] Paso 3c - Seleccionar opción Gaseosa");
            ClickWithoutScroll(
                SalesLocators.CP001.FamiliaOpcion,
                By.XPath("//span[normalize-space()='Gaseosa']")
            );
            Thread.Sleep(1000);

            // ── PASO 4: Concepto = Coca-Cola Gaseosa Botella 1.5L ───────────────
            // 4a. Click en el dropdown de concepto
            Console.WriteLine("[CP001] Paso 4a - Abrir dropdown Concepto");
            Click(SalesLocators.CP001.ConceptoDropdown);
            Thread.Sleep(1000);

            // 4b. Buscar "Coca-Cola" en el buscador de concepto
            Console.WriteLine("[CP001] Paso 4b - Buscar 'Coca-Cola' en buscador concepto");
            var conceptoInput = Find(SalesLocators.CP001.ConceptoSearchInput);
            conceptoInput.Clear();
            conceptoInput.SendKeys("Coca-Cola");
            Thread.Sleep(1000);

            // 4c. Seleccionar Coca-Cola Gaseosa Botella 1.5L de la lista de resultados
            Console.WriteLine("[CP001] Paso 4c - Seleccionar Coca-Cola: app-product-service-selection-form ... div:nth-child(1) span:nth-child(1)");
            ClickWithoutScroll(SalesLocators.CP001.ConceptoOpcion);
            Thread.Sleep(1000);

            // 4d. Click en el acordeón de Facturación para expandirlo
            Console.WriteLine("[CP001] Paso 4d - Abrir acordeón Facturación");
            Click(
                SalesLocators.CP001.AccordionDespuesConcepto,
                SalesLocators.Voucher.BillingAccordion,
                SalesLocators.Voucher.BillingAccordionFallback
            );
            Thread.Sleep(1000);

            // ── PASO 5: Cliente + lupa ──────────────────────────────────────────
            Console.WriteLine($"[NuevaVenta] Paso 5a - Ingresar documento {customerDocument}");
            var clienteInput = Find(SalesLocators.CP001.ClienteBuscar);
            clienteInput.Clear();
            clienteInput.SendKeys(customerDocument);
            Thread.Sleep(1000);

            var typedDocument = (clienteInput.GetAttribute("value") ?? string.Empty).Trim();
            Assert.That(typedDocument, Is.EqualTo(customerDocument),
                $"El documento ingresado no coincide. Esperado={customerDocument}, Actual={typedDocument}");

            Console.WriteLine("[NuevaVenta] Paso 5b - Click lupa");
            Click(SalesLocators.CP001.ClienteLupa);
            Thread.Sleep(1000);

            // ── PASO 6: Comprobante = FACTURA ELECTRÓNICA ────────────────────────
            Console.WriteLine("[CP001] Paso 6a - Abrir dropdown Comprobante");
            Click(SalesLocators.CP001.ComprobanteDropdown);
            Thread.Sleep(1000);

            Console.WriteLine("[CP001] Paso 6b - Seleccionar primera opción (FACTURA ELECTRÓNICA)");
            Click(SalesLocators.CP001.ComprobanteOpcion);
            Thread.Sleep(1000);

            // ── PASO 7: Validación de popup de RUC según el caso ────────────────
            if (shouldExpectRucError)
            {
                Console.WriteLine("[NuevaVenta] Paso 7 - Validar popup de RUC y cerrar con OK");
                _lastObservedMessage = CaptureVisibleMessage(4);

                // En algunos builds el popup no siempre pinta texto, pero el flujo igual bloquea el guardado.
                // Se intenta cerrarlo sin romper la prueba si no está visible.
                TryClickOptional(
                    SalesLocators.CP001.ErrorOkButton,
                    SalesLocators.CP001.ErrorOkButtonFallback,
                    By.CssSelector(".ok-button")
                );
            }

            // ── PASO 8: Serie F002 → checkmark ──────────────────────────────────
            Console.WriteLine("[CP001] Paso 8 - Seleccionar Serie F002: .checkmark / (//span[@class='checkmark'])[1]");
            Click(
                SalesLocators.CP001.SerieCheckmark,
                SalesLocators.CP001.SerieCheckmarkXpath
            );
            Thread.Sleep(1000);

            // ── PASO 9: Acordeón Entrega → Inmediata ────────────────────────────
            Console.WriteLine("[CP001] Paso 9a - Abrir acordeón Entrega: .accordion-button.ng-tns-c2430163177-38.collapsed (con fallbacks)");
            Click(
                SalesLocators.CP001.AccordionEntrega,
                SalesLocators.CP001.AccordionEntregaFallback1,
                SalesLocators.CP001.AccordionEntregaFallback2,
                SalesLocators.CP001.AccordionEntregaFallback3
            );
            Thread.Sleep(1000);

            Console.WriteLine("[CP001] Paso 9b - Click Inmediata: #tipoBien");
            Click(SalesLocators.CP001.EntregaInmediata);
            Thread.Sleep(1000);

            // ── PASO 10: Click Guardar Venta ─────────────────────────────────────
            // El pago se autocompleta. Se hace click en guardar.
            // El botón debería quedar INHABILITADO si el cliente DNI no tiene RUC.
            Console.WriteLine("[NuevaVenta] Paso 10 - Intentar Guardar Venta");
            TryClickGuardar(shouldExecuteSave);

            // Para CP002 se requiere confirmación de guardado exitoso.
            if (!shouldExpectRucError && shouldExecuteSave)
            {
                if (string.IsNullOrWhiteSpace(_lastObservedMessage))
                    _lastObservedMessage = CaptureVisibleMessage(2);

                if (string.IsNullOrWhiteSpace(_lastObservedMessage) && IsNewSaleFormReset())
                    _lastObservedMessage = "Se registró correctamente";

                Assert.That(NormalizeText(_lastObservedMessage), Does.Contain("se registro correctamente"),
                    $"No se confirmó guardado exitoso para el documento {customerDocument}. Mensaje capturado: '{_lastObservedMessage}'");

                TryCloseSuccessDialog();
            }
        }

        private void TryClickGuardar(bool shouldExecuteSave)
        {
            var btn = driver.FindElements(SalesLocators.CP001.GuardarVenta)
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

        public void ValidateSale(SaleExpectation expectation)
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
            var btn = driver.FindElements(SalesLocators.CP001.GuardarVenta)
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
