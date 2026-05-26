using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SIGES3_0.Pages.Helpers;
using SeleniumExtras.WaitHelpers;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace SIGES3_0.Pages.VentasPage
{
    public class ReportesPage
    {
        private readonly IWebDriver _driver;
        private readonly Utilities _utilities;
        private readonly WebDriverWait _wait;
        private string _fechaInicioFiltroAntesReporte = string.Empty;
        private string _fechaFinalFiltroAntesReporte = string.Empty;

        public ReportesPage(IWebDriver driver)
        {
            _driver = driver;
            _utilities = new Utilities(driver);
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(12));
        }

        public void SeleccionarVista(string tabName)
        {
            try
            {
                ClickRobusto(VentasLocators.Reportes.VistaReporte(tabName.Trim()), $"la vista '{tabName}'");
            }
            catch (Exception ex) when (ex is WebDriverTimeoutException || ex is NoSuchElementException || ex is InvalidOperationException)
            {
                throw new Exception($"No se encontró la vista '{tabName}' en Reportes. Valores válidos: comprobantes, series, conceptos, vendedor, grupos, excepciones.");
            }
            Thread.Sleep(1000);
        }

        // ── Tab: Comprobantes ────────────────────────────────────────────────────
        public void SeleccionarTipoComprobante(string tipoComprobante)
        {
            _utilities.ClickButton(VentasLocators.Reportes.TipoComprobanteDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.TipoComprobanteOption(tipoComprobante));
            Thread.Sleep(1000);
        }

        public void SeleccionarSerie(string serie)
        {
            _utilities.ClickButton(VentasLocators.Reportes.SerieDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.SerieOption(serie));
        }

        // ── Tab: Series ──────────────────────────────────────────────────────────
        // Dropdown "Comprobante y Serie" vive DENTRO de la tarjeta POR SERIE.
        // Formato: "Todos" | "XX : YYYY"  (ej: "01 : F002", "03 : B002")
        public void SeleccionarComprobanteSerie(string valor)
        {
            _utilities.ClickButton(VentasLocators.Reportes.ComprobanteSerieDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.ComprobanteSerieOpcion(valor));
        }

        // ── Tab: Conceptos ───────────────────────────────────────────────────────
        public void SeleccionarPuntoVenta(string puntoVenta)
        {
            // Si ya está como chip seleccionado, omitir (evita deseleccionar por comportamiento toggle)
            var chips = _driver.FindElements(VentasLocators.Reportes.PuntoVentaChip(puntoVenta));
            if (chips.Any(e => { try { return e.Displayed; } catch { return false; } }))
            {
                Thread.Sleep(500);
                return;
            }

            SeleccionarEnDropdownBuscable(
                VentasLocators.Reportes.PuntoVentaDropdown,
                VentasLocators.Reportes.PuntoVentaSearch,
                VentasLocators.Reportes.PuntoVentaOption(puntoVenta),
                puntoVenta,
                $"el punto de venta '{puntoVenta}'",
                () => EstaOpcionSeleccionada(VentasLocators.Reportes.PuntoVentaChip(puntoVenta), VentasLocators.Reportes.PuntoVentaDropdown, puntoVenta));
        }

        public void SeleccionarFamilia(string familia)
        {
            if (EstaOpcionSeleccionada(ChipFamilia(familia), VentasLocators.Reportes.FamiliaDropdown, familia))
                return;

            SeleccionarEnDropdownBuscable(
                VentasLocators.Reportes.FamiliaDropdown,
                VentasLocators.Reportes.FamiliaSearch,
                VentasLocators.Reportes.FamiliaOption(familia),
                familia,
                $"la familia '{familia}'",
                () => EstaOpcionSeleccionada(ChipFamilia(familia), VentasLocators.Reportes.FamiliaDropdown, familia));
        }

        public void SeleccionarCaracteristica(string caracteristica, string tarjeta)
        {
            SeleccionarEnDropdownBuscable(
                VentasLocators.Reportes.CaracteristicaDropdown(tarjeta),
                VentasLocators.Reportes.CaracteristicaSearch(tarjeta),
                VentasLocators.Reportes.CaracteristicaOpcion(caracteristica),
                caracteristica,
                $"la caracteristica '{caracteristica}' de la tarjeta '{tarjeta}'",
                () => TriggerContieneTexto(VentasLocators.Reportes.CaracteristicaDropdown(tarjeta), caracteristica));
            _driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
            Thread.Sleep(500);
        }

        // ── Tab: Vendedor ────────────────────────────────────────────────────────
        public void SeleccionarVendedor(string vendedor)
        {
            var chips = _driver.FindElements(VentasLocators.Reportes.VendedorChip(vendedor));
            if (chips.Any(e => { try { return e.Displayed; } catch { return false; } }))
            { Thread.Sleep(500); return; }

            SeleccionarEnDropdownBuscable(
                VentasLocators.Reportes.VendedorDropdown,
                VentasLocators.Reportes.VendedorSearch,
                VentasLocators.Reportes.VendedorOption(vendedor),
                vendedor,
                $"el vendedor '{vendedor}'",
                () => EstaOpcionSeleccionada(VentasLocators.Reportes.VendedorChip(vendedor), VentasLocators.Reportes.VendedorDropdown, vendedor));
        }

        public void SeleccionarFiltroEnTarjeta(string valor, string filtro, string tarjeta)
        {
            SeleccionarEnDropdownBuscable(
                VentasLocators.Reportes.FiltroEnTarjeta(tarjeta, filtro),
                VentasLocators.Reportes.FiltroSearch(tarjeta, filtro),
                VentasLocators.Reportes.FiltroOpcion(valor),
                valor,
                $"el valor '{valor}' del filtro '{filtro}' en la tarjeta '{tarjeta}'",
                () => TriggerContieneTexto(VentasLocators.Reportes.FiltroEnTarjeta(tarjeta, filtro), valor));
            _driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
            Thread.Sleep(500);
        }

        // ── Tab: Grupos ──────────────────────────────────────────────────────────
        public void SeleccionarEstablecimiento(string establecimiento)
        {
            var chips = _driver.FindElements(VentasLocators.Reportes.EstablecimientoChip(establecimiento));
            if (chips.Any(e => { try { return e.Displayed; } catch { return false; } }))
            { Thread.Sleep(500); return; }

            SeleccionarEnDropdownBuscable(
                VentasLocators.Reportes.EstablecimientoDropdown,
                VentasLocators.Reportes.EstablecimientoSearch,
                VentasLocators.Reportes.EstablecimientoOption(establecimiento),
                establecimiento,
                $"el establecimiento '{establecimiento}'",
                () => EstaOpcionSeleccionada(VentasLocators.Reportes.EstablecimientoChip(establecimiento), VentasLocators.Reportes.EstablecimientoDropdown, establecimiento));
        }


        public void ClickVerReporte(string tarjeta)
        {
            var locator = VentasLocators.Reportes.VerReporteEnTarjeta(tarjeta);
            IWebElement btn = null;
            try
            {
                btn = _wait.Until(d =>
                    d.FindElements(locator).FirstOrDefault(e =>
                    { try { return e.Displayed; } catch { return false; } }));
            }
            catch { }

            if (btn == null)
                throw new Exception($"No se encontró el botón VER REPORTE para la tarjeta '{tarjeta}'.");

            CapturarFechasFiltroAntesDeReporte();

            var handlesAntes = _driver.WindowHandles.ToList();
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btn);
            Thread.Sleep(300);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(2000);

            string handleActual = null;
            try { handleActual = _driver.CurrentWindowHandle; } catch { }
            var handlesDespues = _driver.WindowHandles.ToList();
            if (handleActual == null || !handlesDespues.Contains(handleActual))
            {
                if (handlesDespues.Any()) _driver.SwitchTo().Window(handlesDespues.Last());
            }
            else
            {
                var nuevaPestana = handlesDespues.Except(handlesAntes).FirstOrDefault();
                if (nuevaPestana != null) _driver.SwitchTo().Window(nuevaPestana);
            }

            try
            {
                EsperarHasta(() => HayErrorReporteVisible() || ReporteFueRenderizado(), 25);
            }
            catch
            {
            }
        }

        // ── Verificaciones ───────────────────────────────────────────────────────
        public bool VerificarReporteGenerado()
        {
            try
            {
                Thread.Sleep(2000);

                if (HayErrorReporteVisible())
                    return false;

                var waitReporte = new WebDriverWait(_driver, TimeSpan.FromSeconds(30))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(250)
                };

                waitReporte.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

                bool reporteVisible = waitReporte.Until(_ =>
                {
                    if (HayErrorReporteVisible())
                        return false;

                    return ReporteFueRenderizado();
                });

                if (reporteVisible)
                {
                    Console.WriteLine($"[Reportes][Resultado] Reporte visible. {ConstruirDiagnosticoResultado()}");
                    return true;
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"[Reportes][Resultado] Timeout esperando el reporte. {ConstruirDiagnosticoResultado()}");
            }

            return !HayErrorReporteVisible() && ReporteFueRenderizado();
        }

        public void ValidarReporteGeneradoConContenidoYFechas()
        {
            bool sinError = !HayErrorReporteVisible();
            bool reporteConContenido = EsperarReporteConContenido();
            bool fechasCoinciden = FechasFiltroCoincidenConEvidenciaReporte(out string diagnosticoFechas);

            Assert.Multiple(() =>
            {
                Assert.That(sinError, Is.True, $"El reporte muestra un error visible. {ConstruirDiagnosticoResultado()}");
                Assert.That(reporteConContenido, Is.True, $"El reporte abrio, pero no se encontro contenido real. {ConstruirDiagnosticoResultado()}");
                Assert.That(fechasCoinciden, Is.True, diagnosticoFechas);
            });
        }

        public bool VerificarBotonHabilitado(string tarjeta)
        {
            var btn = _driver.FindElements(VentasLocators.Reportes.VerReporteEnTarjeta(tarjeta))
                             .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
            if (btn == null) return false;
            return btn.Enabled && btn.GetAttribute("disabled") == null;
        }

        public void ValidarResultadoReporte(string resultadoEsperado)
        {
            switch (resultadoEsperado.Trim().ToLower())
            {
                case "no permite aplicar el filtro inhabilitado":
                    Assert.IsFalse(_driver.Url.Contains("/sales/report/view"),
                        "El sistema no debería haber generado el reporte con fechas inválidas.");
                    break;
                case "aplica el filtro correctamente":
                    Assert.IsTrue(VerificarReporteGenerado(),
                        "Se esperaba que el filtro se aplicara y el reporte se generara correctamente.");
                    break;
                default:
                    Assert.Fail($"Resultado esperado no reconocido: '{resultadoEsperado}'");
                    break;
            }
        }
        // =========================
        // REUTILIZABLE Ventas
        // =========================

        // Modulo y submodulos

        public void AccederModulo(string modulo)
        {
            var locator = By.XPath($"//span[normalize-space()='{modulo}']/ancestor::a[1]");
            _wait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();
        }

        public void AccederSubmodulo(string submodulo)
        {
            var locator = By.XPath($"//span[contains(text(),'{submodulo}')]");
            _wait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();
        }

        private void ClickRobusto(By locator, string descripcion)
        {
            Exception? ultimaExcepcion = null;

            for (int intento = 0; intento < 4; intento++)
            {
                try
                {
                    var element = EsperarInteractivo(locator);
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
                    Thread.Sleep(200);

                    try
                    {
                        element.Click();
                    }
                    catch (ElementClickInterceptedException)
                    {
                        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
                    }

                    Thread.Sleep(300);
                    return;
                }
                catch (StaleElementReferenceException ex)
                {
                    ultimaExcepcion = ex;
                    Thread.Sleep(300);
                }
                catch (WebDriverException ex) when (ex.Message.Contains("stale element reference", StringComparison.OrdinalIgnoreCase))
                {
                    ultimaExcepcion = ex;
                    Thread.Sleep(300);
                }
            }

            throw new Exception(
                $"No se pudo hacer clic en {descripcion} porque el DOM se actualiz\u00f3 mientras Selenium intentaba interactuar con ese elemento.",
                ultimaExcepcion);
        }

        private void ClickElementoRobusto(IWebElement element, string descripcion)
        {
            Exception? ultimaExcepcion = null;

            for (int intento = 0; intento < 4; intento++)
            {
                try
                {
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
                    Thread.Sleep(200);

                    try
                    {
                        element.Click();
                    }
                    catch (ElementClickInterceptedException)
                    {
                        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
                    }

                    Thread.Sleep(300);
                    return;
                }
                catch (StaleElementReferenceException ex)
                {
                    ultimaExcepcion = ex;
                    Thread.Sleep(300);
                }
                catch (WebDriverException ex) when (ex.Message.Contains("stale element reference", StringComparison.OrdinalIgnoreCase))
                {
                    ultimaExcepcion = ex;
                    Thread.Sleep(300);
                }
            }

            throw new Exception(
                $"No se pudo hacer clic en {descripcion} porque la opcion visible cambio mientras Selenium intentaba seleccionarla.",
                ultimaExcepcion);
        }

        private void SeleccionarEnDropdownBuscable(
            By triggerLocator,
            By buscadorLocator,
            By opcionLocator,
            string textoBusqueda,
            string descripcion,
            Func<bool> criterioSeleccionado)
        {
            Exception? ultimaExcepcion = null;

            for (int intento = 0; intento < 4; intento++)
            {
                try
                {
                    AbrirDropdownBuscable(triggerLocator, buscadorLocator, descripcion);
                    EsperarContenidoDropdown(buscadorLocator, 8);
                    var inputBusqueda = EscribirEnBuscadorVisible(buscadorLocator, textoBusqueda);
                    if (IntentarSeleccionPorTeclado(inputBusqueda, criterioSeleccionado))
                    {
                        CerrarDropdownAbierto();
                        return;
                    }

                    ClickOpcionDropdown(opcionLocator, textoBusqueda, descripcion);
                    EsperarHasta(criterioSeleccionado);
                    CerrarDropdownAbierto();
                    return;
                }
                catch (StaleElementReferenceException ex)
                {
                    ultimaExcepcion = ex;
                    CerrarDropdownAbierto();
                    Thread.Sleep(300);
                }
                catch (WebDriverTimeoutException ex)
                {
                    ultimaExcepcion = ex;
                    CerrarDropdownAbierto();
                    Thread.Sleep(300);
                }
                catch (WebDriverException ex) when (
                    ex.Message.Contains("stale element reference", StringComparison.OrdinalIgnoreCase) ||
                    ex.Message.Contains("no such element", StringComparison.OrdinalIgnoreCase))
                {
                    ultimaExcepcion = ex;
                    CerrarDropdownAbierto();
                    Thread.Sleep(300);
                }
            }

            throw new Exception(
                $"No se pudo seleccionar {descripcion} en el dropdown, incluso despues de reintentar la busqueda por texto. {ConstruirDiagnosticoDropdown(triggerLocator, buscadorLocator, textoBusqueda)}",
                ultimaExcepcion);
        }

        private void ClickOpcionDropdown(By opcionLocator, string textoBusqueda, string descripcion)
        {
            var opcionFlexible = BuscarOpcionVisibleDropdown(textoBusqueda);
            if (opcionFlexible != null)
            {
                ClickElementoRobusto(opcionFlexible, descripcion);
                return;
            }

            var opcionConScroll = BuscarOpcionDropdownConScroll(textoBusqueda);
            if (opcionConScroll != null)
            {
                ClickElementoRobusto(opcionConScroll, descripcion);
                return;
            }

            ClickRobusto(opcionLocator, descripcion);
        }

        private void AbrirDropdownBuscable(By triggerLocator, By buscadorLocator, string descripcion)
        {
            Exception? ultimaExcepcion = null;

            CerrarDropdownAbierto();
            Thread.Sleep(150);

            for (int intento = 0; intento < 4; intento++)
            {
                try
                {
                    var trigger = EsperarInteractivo(triggerLocator);
                    ClickElementoSinJavascript(trigger);

                    if (EsperarDropdownAbierto(triggerLocator, buscadorLocator, 2))
                        return;

                    if (IntentarAbrirDropdownConIcono(triggerLocator, buscadorLocator))
                        return;

                    if (IntentarAbrirDropdownConTeclado(triggerLocator, buscadorLocator))
                        return;
                }
                catch (Exception ex) when (
                    ex is StaleElementReferenceException ||
                    ex is WebDriverTimeoutException ||
                    ex is ElementClickInterceptedException ||
                    ex is InvalidOperationException ||
                    ex is WebDriverException)
                {
                    ultimaExcepcion = ex;
                    CerrarDropdownAbierto();
                }

                Thread.Sleep(300);
            }

            throw new Exception($"No se pudo abrir {descripcion} para mostrar sus opciones.", ultimaExcepcion);
        }

        private void ClickElementoSinJavascript(IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
            Thread.Sleep(150);
            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                CerrarDropdownAbierto();
                Thread.Sleep(250);
                element.Click();
            }
            Thread.Sleep(250);
        }

        private bool IntentarAbrirDropdownConIcono(By triggerLocator, By buscadorLocator)
        {
            try
            {
                var trigger = EsperarInteractivo(triggerLocator, 5);
                var icono = trigger.FindElements(By.XPath(
                        ".//*[contains(@class,'chevron') or contains(@class,'arrow') or contains(@class,'caret') or self::svg or self::i]"))
                    .FirstOrDefault(elemento =>
                    {
                        try { return elemento.Displayed && elemento.Enabled; }
                        catch { return false; }
                    });

                if (icono == null)
                    return false;

                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", icono);
                Thread.Sleep(100);
                icono.Click();
                Thread.Sleep(250);
                return EsperarDropdownAbierto(triggerLocator, buscadorLocator, 2);
            }
            catch
            {
                return false;
            }
        }

        private bool IntentarAbrirDropdownConTeclado(By triggerLocator, By buscadorLocator)
        {
            try
            {
                var trigger = EsperarInteractivo(triggerLocator, 5);
                trigger.SendKeys(Keys.Enter);
                Thread.Sleep(150);

                if (EsperarDropdownAbierto(triggerLocator, buscadorLocator, 2))
                    return true;

                trigger.SendKeys(Keys.Space);
                Thread.Sleep(250);
                return EsperarDropdownAbierto(triggerLocator, buscadorLocator, 2);
            }
            catch
            {
                return false;
            }
        }

        private void EsperarContenidoDropdown(By buscadorLocator, int timeoutSeconds)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds))
            {
                PollingInterval = TimeSpan.FromMilliseconds(200)
            };

            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            wait.Until(_ =>
                HayElementoVisible(buscadorLocator) ||
                HayContenedorOpcionesVisible() ||
                HayOpcionesDropdownVisibles());
        }

        private bool EsperarDropdownAbierto(By triggerLocator, By buscadorLocator, int timeoutSeconds)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                };

                wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

                return wait.Until(_ => DropdownEstaAbierto(triggerLocator, buscadorLocator));
            }
            catch
            {
                return false;
            }
        }

        private bool DropdownEstaAbierto(By triggerLocator, By buscadorLocator)
        {
            if (HayElementoVisible(buscadorLocator) || HayContenedorOpcionesVisible() || HayOpcionesDropdownVisibles())
                return true;

            return _driver.FindElements(triggerLocator).Any(element =>
            {
                try
                {
                    if (!element.Displayed)
                        return false;

                    var ariaExpanded = element.GetAttribute("aria-expanded") ?? string.Empty;
                    if (ariaExpanded.Equals("true", StringComparison.OrdinalIgnoreCase))
                        return true;

                    var clases = element.GetAttribute("class") ?? string.Empty;
                    return clases.Contains("open", StringComparison.OrdinalIgnoreCase) ||
                           clases.Contains("active", StringComparison.OrdinalIgnoreCase) ||
                           clases.Contains("expanded", StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            });
        }

        private bool HayContenedorOpcionesVisible()
        {
            return _driver.FindElements(By.XPath(
                "//div[contains(@class,'options-container')] | //div[contains(@class,'dropdown-menu')] | //*[@role='listbox']"))
                .Any(element =>
                {
                    try { return element.Displayed; }
                    catch { return false; }
                });
        }

        private bool HayOpcionesDropdownVisibles()
        {
            return BuscarOpcionVisibleDropdown(string.Empty) != null;
        }

        private IWebElement? EscribirEnBuscadorVisible(By locator, string texto)
        {
            var input = EsperarUltimoVisible(locator, 5);
            if (input == null)
                return null;

            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);
            Thread.Sleep(150);

            try
            {
                input.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].focus();", input);
            }

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(100);
            input.SendKeys(texto);
            Thread.Sleep(500);
            return input;
        }

        private bool IntentarSeleccionPorTeclado(IWebElement? input, Func<bool> criterioSeleccionado)
        {
            if (input == null)
                return false;

            try
            {
                input.SendKeys(Keys.ArrowDown);
                Thread.Sleep(150);
                input.SendKeys(Keys.Enter);

                try
                {
                    EsperarHasta(criterioSeleccionado, 3);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private IWebElement EsperarInteractivo(By locator, int timeoutSeconds = 20)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds))
            {
                PollingInterval = TimeSpan.FromMilliseconds(200)
            };

            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            return wait.Until(driver =>
                driver.FindElements(locator).FirstOrDefault(element =>
                {
                    try
                    {
                        return element.Displayed && element.Enabled;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                }));
        }

        private IWebElement? EsperarUltimoVisible(By locator, int timeoutSeconds = 5)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                };

                wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

                return wait.Until(driver =>
                {
                    var visibles = driver.FindElements(locator)
                        .Where(elemento =>
                        {
                            try
                            {
                                return elemento.Displayed && elemento.Enabled;
                            }
                            catch (StaleElementReferenceException)
                            {
                                return false;
                            }
                        })
                        .ToList();

                    return visibles.Any() ? visibles.Last() : null;
                });
            }
            catch
            {
                return null;
            }
        }

        private void EsperarVisible(By locator, int timeoutSeconds = 20)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds))
            {
                PollingInterval = TimeSpan.FromMilliseconds(200)
            };

            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            wait.Until(driver =>
                driver.FindElements(locator).Any(element =>
                {
                    try
                    {
                        return element.Displayed;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                }));
        }

        private void EsperarHasta(Func<bool> condicion, int timeoutSeconds = 10)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds))
            {
                PollingInterval = TimeSpan.FromMilliseconds(200)
            };

            wait.Until(_ =>
            {
                try
                {
                    return condicion();
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });
        }

        private bool EstaOpcionSeleccionada(By chipLocator, By triggerLocator, string texto)
        {
            return HayElementoVisible(chipLocator) || TriggerContieneTexto(triggerLocator, texto);
        }

        private bool HayElementoVisible(By locator)
        {
            return _driver.FindElements(locator).Any(element =>
            {
                try
                {
                    return element.Displayed;
                }
                catch
                {
                    return false;
                }
            });
        }

        private bool TriggerContieneTexto(By locator, string texto)
        {
            return _driver.FindElements(locator).Any(element =>
            {
                try
                {
                    return element.Displayed &&
                           (element.Text ?? string.Empty).Contains(texto, StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            });
        }

        private bool HayErrorReporteVisible()
        {
            return _driver.FindElements(By.XPath(
                "//*[contains(@class,'toast-error') or contains(@class,'alert-danger')" +
                " or (contains(@class,'swal2-popup') and .//*[contains(@class,'swal2-error-icon')])]"))
                .Any(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });
        }

        private void CapturarFechasFiltroAntesDeReporte()
        {
            _fechaInicioFiltroAntesReporte = ObtenerValorInputVisible(VentasLocators.Reportes.FechaHoraInicial);
            _fechaFinalFiltroAntesReporte = ObtenerValorInputVisible(VentasLocators.Reportes.FechaHoraFinal);
        }

        private string ObtenerValorInputVisible(By locator)
        {
            return _driver.FindElements(locator)
                .Where(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                })
                .Select(e =>
                {
                    try { return (e.GetAttribute("value") ?? e.Text ?? string.Empty).Trim(); }
                    catch { return string.Empty; }
                })
                .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v)) ?? string.Empty;
        }

        private bool EsperarReporteConContenido()
        {
            try
            {
                var waitReporte = new WebDriverWait(_driver, TimeSpan.FromSeconds(30))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(250)
                };

                waitReporte.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
                return waitReporte.Until(_ => HayContenidoRealReporte());
            }
            catch
            {
                return false;
            }
        }

        private bool HayContenidoRealReporte()
        {
            if (HayErrorReporteVisible() || TextoPaginaIndicaReporteVacio())
                return false;

            return HayTablaReporteConFilas() ||
                   TextoPaginaIndicaReporteConDatos();
        }

        private bool HayTablaReporteConFilas()
        {
            return _driver.FindElements(By.XPath(
                    "//div[contains(@class,'table-responsive')]//tbody/tr[td] | " +
                    "//table//tbody/tr[td] | " +
                    "//ngx-datatable//*[contains(@class,'datatable-body-row')]"))
                .Any(e =>
                {
                    try { return e.Displayed && !string.IsNullOrWhiteSpace(e.Text); }
                    catch { return false; }
                });
        }

        private bool HayPdfRenderizado()
        {
            try
            {
                var resultado = ((IJavaScriptExecutor)_driver).ExecuteScript(@"
                    const visible = el => {
                        const rect = el.getBoundingClientRect();
                        const style = window.getComputedStyle(el);
                        return rect.width > 80 && rect.height > 80 &&
                            style.visibility !== 'hidden' && style.display !== 'none';
                    };

                    const canvasVisible = Array.from(document.querySelectorAll('canvas'))
                        .some(canvas => visible(canvas));
                    const pageVisible = Array.from(document.querySelectorAll(
                        '.pdfViewer .page, .page[data-loaded=""true""], ngx-extended-pdf-viewer .page'))
                        .some(page => visible(page));
                    const frameVisible = Array.from(document.querySelectorAll('iframe, embed, object'))
                        .some(frame => visible(frame) && (frame.src || frame.data));

                    return canvasVisible || pageVisible || frameVisible;
                ");

                return resultado is bool visible && visible;
            }
            catch
            {
                return false;
            }
        }

        private bool TextoPaginaIndicaReporteConDatos()
        {
            var textoPagina = ObtenerTextoPagina();
            if (string.IsNullOrWhiteSpace(textoPagina) || TextoIndicaVacio(textoPagina))
                return false;

            if (textoPagina.Contains("TOTAL GENERAL", StringComparison.OrdinalIgnoreCase) &&
                ContieneImporteMayorACero(textoPagina))
            {
                return true;
            }

            if (textoPagina.Contains("VENTAS CONFIRMADAS", StringComparison.OrdinalIgnoreCase) &&
                ContieneDocumentoVenta(textoPagina))
            {
                return true;
            }

            return ContieneFilaResumenConCantidad(textoPagina);
        }

        private bool TextoPaginaIndicaReporteVacio() => TextoIndicaVacio(ObtenerTextoPagina());

        private static bool TextoIndicaVacio(string texto)
        {
            return texto.Contains("No hay datos", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("Sin resultado", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("Sin datos", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("no se encontraron", StringComparison.OrdinalIgnoreCase);
        }

        private static bool ContieneDocumentoVenta(string texto)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(
                texto,
                @"\b(?:B|F|NV|NC|ND)[A-Z0-9]{0,4}-\d+\b",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        private static bool ContieneFilaResumenConCantidad(string texto)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(
                texto,
                @"\b(?:BOLETA|FACTURA|NOTA\s+DE\s+VENTA|NOTA\s+DE\s+CREDITO|NOTA\s+DE\s+DEBITO)\b[\s\S]{0,120}\b[1-9]\d*\b",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        private static bool ContieneImporteMayorACero(string texto)
        {
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(
                         texto,
                         @"\b\d{1,3}(?:,\d{3})*(?:\.\d{2})\b|\b\d+\.\d{2}\b"))
            {
                string numero = match.Value.Replace(",", string.Empty);
                if (decimal.TryParse(numero, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal importe) &&
                    importe > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool FechasFiltroCoincidenConEvidenciaReporte(out string diagnostico)
        {
            string evidencia = ObtenerEvidenciaReporte();
            bool inicioOk = ContieneFechaEsperada(evidencia, _fechaInicioFiltroAntesReporte);
            bool finalOk = ContieneFechaEsperada(evidencia, _fechaFinalFiltroAntesReporte);

            diagnostico =
                $"No se encontro evidencia de que el reporte use las mismas fechas seleccionadas. " +
                $"Inicio esperado='{_fechaInicioFiltroAntesReporte}' encontrado={inicioOk}. " +
                $"Final esperado='{_fechaFinalFiltroAntesReporte}' encontrado={finalOk}. {ConstruirDiagnosticoResultado()}";

            return inicioOk && finalOk;
        }

        private string ObtenerEvidenciaReporte()
        {
            var partes = new List<string>();

            try { partes.Add(_driver.Url ?? string.Empty); } catch { }
            partes.Add(ObtenerTextoPagina());

            try
            {
                var recursos = ((IJavaScriptExecutor)_driver).ExecuteScript(@"
                    const recursos = performance.getEntriesByType('resource')
                        .map(entry => entry.name || '')
                        .filter(name => /report|sale|fecha|date|document|pdf/i.test(name));
                    const storage = [];
                    for (const area of [localStorage, sessionStorage]) {
                        for (let i = 0; i < area.length; i++) {
                            const key = area.key(i);
                            const value = area.getItem(key) || '';
                            if (/report|sale|fecha|date/i.test(key + value)) {
                                storage.push(key + '=' + value);
                            }
                        }
                    }
                    return [...recursos, ...storage].join('\n');
                ")?.ToString() ?? string.Empty;

                partes.Add(recursos);
            }
            catch
            {
            }

            string evidencia = string.Join("\n", partes);
            try { return Uri.UnescapeDataString(evidencia); }
            catch { return evidencia; }
        }

        private static bool ContieneFechaEsperada(string evidencia, string fechaEsperada)
        {
            if (string.IsNullOrWhiteSpace(fechaEsperada))
                return false;

            string evidenciaNormalizada = evidencia.Replace("%2F", "/", StringComparison.OrdinalIgnoreCase);
            return VariantesFecha(fechaEsperada)
                .Any(variante => evidenciaNormalizada.Contains(variante, StringComparison.OrdinalIgnoreCase));
        }

        private static IEnumerable<string> VariantesFecha(string valor)
        {
            string valorLimpio = (valor ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(valorLimpio))
                yield break;

            yield return valorLimpio;

            if (TryParseFechaFiltro(valorLimpio, out DateTime fecha))
            {
                yield return fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                yield return fecha.ToString("d/MM/yyyy", CultureInfo.InvariantCulture);
                yield return fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                yield return fecha.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                yield return fecha.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                yield return fecha.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                yield return fecha.ToString("d/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                yield return fecha.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                yield return fecha.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                yield return fecha.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
                yield return FormatearFechaVisible(fecha);
                yield break;
            }

            string fechaTexto = valorLimpio.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(fechaTexto))
            {
                yield return fechaTexto;
            }
        }

        private static bool TryParseFechaFiltro(string valor, out DateTime fecha)
        {
            fecha = default;

            string normalizado = string.Join(" ", valor.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Replace("a. m.", "AM", StringComparison.OrdinalIgnoreCase)
                .Replace("p. m.", "PM", StringComparison.OrdinalIgnoreCase)
                .Replace("a.m.", "AM", StringComparison.OrdinalIgnoreCase)
                .Replace("p.m.", "PM", StringComparison.OrdinalIgnoreCase);

            string[] formatos =
            {
                "dd/MM/yyyy HH:mm:ss", "d/MM/yyyy HH:mm:ss",
                "dd/M/yyyy HH:mm:ss",  "d/M/yyyy HH:mm:ss",
                "dd/MM/yyyy H:mm:ss",  "d/MM/yyyy H:mm:ss",
                "dd/M/yyyy H:mm:ss",   "d/M/yyyy H:mm:ss",
                "dd/MM/yyyy HH:mm", "d/MM/yyyy HH:mm",
                "dd/M/yyyy HH:mm",  "d/M/yyyy HH:mm",
                "dd/MM/yyyy H:mm",  "d/MM/yyyy H:mm",
                "dd/M/yyyy H:mm",   "d/M/yyyy H:mm",
                "dd/MM/yyyy hh:mm tt", "d/MM/yyyy hh:mm tt",
                "dd/M/yyyy hh:mm tt",  "d/M/yyyy hh:mm tt",
                "dd/MM/yyyy h:mm tt",  "d/MM/yyyy h:mm tt",
                "dd/M/yyyy h:mm tt",   "d/M/yyyy h:mm tt",
                "MM/dd/yyyy hh:mm tt", "M/dd/yyyy hh:mm tt",
                "MM/d/yyyy hh:mm tt",  "M/d/yyyy hh:mm tt",
                "MM/dd/yyyy h:mm tt",  "M/dd/yyyy h:mm tt",
                "MM/d/yyyy h:mm tt",   "M/d/yyyy h:mm tt",
                "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm",
                "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy",
                "yyyy-MM-dd", "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy"
            };

            return DateTime.TryParseExact(
                normalizado,
                formatos,
                new CultureInfo("en-US"),
                DateTimeStyles.None,
                out fecha);
        }

        private static string FormatearFechaVisible(DateTime fecha)
        {
            string ampm = fecha.ToString("tt", new CultureInfo("en-US")).Equals("AM", StringComparison.OrdinalIgnoreCase)
                ? "a. m."
                : "p. m.";

            return $"{fecha:dd/MM/yyyy h:mm} {ampm}";
        }

        private bool ReporteFueRenderizado()
        {
            return UrlEsVistaReporte() ||
                   HayElementoVisible(VentasLocators.Reportes.HeaderReporteResultado) ||
                   HayElementoVisible(By.XPath(
                       "//*[@id='viewer' or @id='toolbar' or @id='pageNumber' or @id='numPages' or contains(@class,'pdfViewer') or contains(@class,'ngx-extended-pdf-viewer')]")) ||
                   TextoPaginaIndicaReporte();
        }

        private bool UrlEsVistaReporte()
        {
            try
            {
                var url = _driver.Url ?? string.Empty;
                return url.Contains("/sales/report/view", StringComparison.OrdinalIgnoreCase) ||
                       url.Contains("/sales/report/document", StringComparison.OrdinalIgnoreCase) ||
                       url.StartsWith("blob:", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private bool TextoPaginaIndicaReporte()
        {
            var textoPagina = ObtenerTextoPagina();
            if (string.IsNullOrWhiteSpace(textoPagina))
                return false;

            return textoPagina.Contains("REPORTE POR", StringComparison.OrdinalIgnoreCase) ||
                   textoPagina.Contains("VENTAS CONFIRMADAS POR", StringComparison.OrdinalIgnoreCase) ||
                   textoPagina.Contains("VENTAS INVALIDADAS POR", StringComparison.OrdinalIgnoreCase) ||
                   textoPagina.Contains("No hay datos", StringComparison.OrdinalIgnoreCase) ||
                   textoPagina.Contains("Sin resultado", StringComparison.OrdinalIgnoreCase) ||
                   textoPagina.Contains("Sin datos", StringComparison.OrdinalIgnoreCase);
        }

        private string ObtenerTextoPagina()
        {
            try
            {
                var body = _driver.FindElements(By.TagName("body")).FirstOrDefault(element =>
                {
                    try { return element.Displayed; }
                    catch { return false; }
                });

                if (body != null)
                    return NormalizarTexto(body.Text);
            }
            catch
            {
            }

            try
            {
                return NormalizarTexto(((IJavaScriptExecutor)_driver)
                    .ExecuteScript("return document.body ? document.body.innerText : '';")?.ToString() ?? string.Empty);
            }
            catch
            {
                return string.Empty;
            }
        }

        private string ConstruirDiagnosticoResultado()
        {
            string url;
            string titulo;

            try { url = _driver.Url ?? string.Empty; }
            catch { url = string.Empty; }

            try { titulo = _driver.Title ?? string.Empty; }
            catch { titulo = string.Empty; }

            var textoPagina = ObtenerTextoPagina();
            if (textoPagina.Length > 220)
                textoPagina = textoPagina.Substring(0, 220);

            return $"url='{url}', titulo='{titulo}', texto='{textoPagina}'";
        }

        private void CerrarDropdownAbierto()
        {
            try
            {
                for (int intento = 0; intento < 3; intento++)
                {
                    _driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
                    Thread.Sleep(150);

                    if (!HayDropdownVisible())
                        return;
                }

                new WebDriverWait(_driver, TimeSpan.FromSeconds(2))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(_ => !HayDropdownVisible());
            }
            catch
            {
            }
        }

        private bool HayDropdownVisible()
        {
            return HayContenedorOpcionesVisible() || HayOpcionesDropdownVisibles();
        }

        private IWebElement? BuscarOpcionVisibleDropdown(string texto)
        {
            var candidatos = _driver.FindElements(By.XPath(
                    "//span[contains(@class,'option-label')] | " +
                    "//div[contains(@class,'option-item')] | " +
                    "//a[contains(@class,'dropdown-item')] | " +
                    "//*[@role='option']"))
                .Where(element =>
                {
                    try
                    {
                        return element.Displayed && element.Enabled;
                    }
                    catch
                    {
                        return false;
                    }
                })
                .ToList();

            var textoNormalizado = NormalizarTexto(texto);

            var exacto = candidatos.FirstOrDefault(element =>
            {
                try
                {
                    return NormalizarTexto(element.Text).Equals(textoNormalizado, StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            });
            if (exacto != null)
                return exacto;

            return candidatos.FirstOrDefault(element =>
            {
                try
                {
                    return NormalizarTexto(element.Text).Contains(textoNormalizado, StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            });
        }

        private IWebElement? BuscarOpcionDropdownConScroll(string texto)
        {
            for (int intento = 0; intento < 20; intento++)
            {
                var visible = BuscarOpcionVisibleDropdown(texto);
                if (visible != null)
                    return visible;

                if (!ScrollDropdownOpciones())
                    break;

                Thread.Sleep(250);
            }

            return BuscarOpcionVisibleDropdown(texto);
        }

        private bool ScrollDropdownOpciones()
        {
            try
            {
                var contenedor = ObtenerContenedorOpcionesVisible();
                if (contenedor == null)
                    return false;

                var estado = ((IJavaScriptExecutor)_driver).ExecuteScript(@"
                    const root = arguments[0];
                    if (!root) return null;

                    const candidatos = [root, ...root.querySelectorAll('*')];
                    const scrollable = candidatos.find(el => el && el.scrollHeight > el.clientHeight + 5) || root;

                    const topAntes = scrollable.scrollTop;
                    const incremento = Math.max(scrollable.clientHeight - 40, 120);
                    scrollable.scrollTop = Math.min(scrollable.scrollTop + incremento, scrollable.scrollHeight);

                    return {
                        before: topAntes,
                        after: scrollable.scrollTop,
                        clientHeight: scrollable.clientHeight,
                        scrollHeight: scrollable.scrollHeight
                    };
                ", contenedor) as System.Collections.Generic.Dictionary<string, object>;

                if (estado == null)
                    return false;

                var before = Convert.ToDouble(estado["before"]);
                var after = Convert.ToDouble(estado["after"]);
                var clientHeight = Convert.ToDouble(estado["clientHeight"]);
                var scrollHeight = Convert.ToDouble(estado["scrollHeight"]);

                return after > before && scrollHeight > clientHeight + 5;
            }
            catch
            {
                return false;
            }
        }

        private IWebElement? ObtenerContenedorOpcionesVisible()
        {
            return _driver.FindElements(By.XPath(
                    "//div[contains(@class,'options-container')] | " +
                    "//div[contains(@class,'dropdown-menu')] | " +
                    "//*[@role='listbox']"))
                .FirstOrDefault(element =>
                {
                    try { return element.Displayed; }
                    catch { return false; }
                });
        }

        private static string NormalizarTexto(string texto) =>
            (texto ?? string.Empty).Trim().Replace("\r", " ").Replace("\n", " ");

        private string ConstruirDiagnosticoDropdown(By triggerLocator, By buscadorLocator, string textoBusqueda)
        {
            try
            {
                var triggerTexto = _driver.FindElements(triggerLocator)
                    .Where(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    })
                    .Select(e =>
                    {
                        try { return (e.Text ?? string.Empty).Trim(); }
                        catch { return string.Empty; }
                    })
                    .FirstOrDefault() ?? string.Empty;

                var buscadores = _driver.FindElements(buscadorLocator)
                    .Where(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    })
                    .Select(e =>
                    {
                        try { return (e.GetAttribute("value") ?? string.Empty).Trim(); }
                        catch { return string.Empty; }
                    })
                    .ToList();

                var opciones = _driver.FindElements(By.XPath(
                        "//span[contains(@class,'option-label')] | " +
                        "//div[contains(@class,'option-item')] | " +
                        "//a[contains(@class,'dropdown-item')] | " +
                        "//*[@role='option']"))
                    .Where(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    })
                    .Select(e =>
                    {
                        try { return NormalizarTexto(e.Text); }
                        catch { return string.Empty; }
                    })
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
                    .Take(10)
                    .ToList();

                var triggerHtml = _driver.FindElements(triggerLocator)
                    .Where(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    })
                    .Select(e =>
                    {
                        try { return e.GetAttribute("outerHTML") ?? string.Empty; }
                        catch { return string.Empty; }
                    })
                    .FirstOrDefault() ?? string.Empty;

                if (triggerHtml.Length > 300)
                    triggerHtml = triggerHtml.Substring(0, 300);

                return $"Diagnostico: trigger='{triggerTexto}', busqueda='{textoBusqueda}', inputs=[{string.Join(" | ", buscadores)}], opciones=[{string.Join(" | ", opciones)}], triggerHtml='{triggerHtml}'";
            }
            catch (Exception ex)
            {
                return $"Diagnostico no disponible: {ex.Message}";
            }
        }

        private static By ChipFamilia(string familia) =>
            By.XPath($"//label[contains(.,'Familia')]/following-sibling::app-dropdown-search//*[not(contains(@class,'placeholder')) and contains(normalize-space(),'{familia}')]");
    }
}
