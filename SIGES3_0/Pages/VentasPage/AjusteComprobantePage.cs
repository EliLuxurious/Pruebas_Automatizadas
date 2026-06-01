using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SIGES3_0.Pages.Helpers;
using SeleniumExtras.WaitHelpers;

namespace SIGES3_0.Pages.VentasPage
{
    public class AjusteComprobantePage
    {
        private readonly IWebDriver _driver;
        private readonly Utilities _utilities;
        private readonly WebDriverWait _wait;

        public AjusteComprobantePage(IWebDriver driver)
        {
            _driver = driver;
            _utilities = new Utilities(driver);
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(35));
        }

        // ── Filtrar ventas por fecha de ayer ─────────────────────────────────
        public void FiltrarVentasPorFechaAyer() => FiltrarVentasPorDiasAtras(1);

        // ── Filtrar ventas por N días atrás ───────────────────────────────────
        public void FiltrarVentasPorDiasAtras(int dias)
        {
            var fecha = DateTime.Now.AddDays(-dias).ToString("dd/MM/yyyy");
            Console.WriteLine($"[AjusteComprobante] Filtrando ventas con rango: {fecha} 12:00 am - {fecha} 11:59 pm");

            var inicioLocator = EsperarLocadorFecha(
                VentasLocators.ViewSales.InitialDate,
                VentasLocators.ViewSales.FechaHoraInicial);
            IngresarFechaHoraJS(inicioLocator, $"{fecha} 12:00 am");
            Thread.Sleep(400);

            var finLocator = EsperarLocadorFecha(
                VentasLocators.ViewSales.FinalDate,
                VentasLocators.ViewSales.FechaHoraFinal);
            IngresarFechaHoraJS(finLocator, $"{fecha} 11:59 pm");
            Thread.Sleep(400);

            var btn = _wait.Until(ExpectedConditions.ElementToBeClickable(VentasLocators.ViewSales.QueryButton));
            ScrollTo(btn);
            ClickSeguro(btn);
            Thread.Sleep(3000);

            var filas = _driver.FindElements(By.XPath("//tbody/tr")).Count;
            Console.WriteLine($"[AjusteComprobante] Resultados después del filtrado ({dias} día(s) atrás): {filas} fila(s) en la tabla");
        }

        // ── Abrir modal de ajuste ───────────────────────────────────────────
        public void ClickAccionPrimerComprobante()
        {
            Thread.Sleep(1000);

            // 1. Verificar que la tabla tenga al menos una fila
            bool hayFilas = _driver.FindElements(VentasLocators.AjusteComprobante.TablaFilaPrimera).Any();
            if (!hayFilas)
            {
                try
                {
                    _wait.Until(d => d.FindElements(VentasLocators.AjusteComprobante.TablaFilaPrimera).Any());
                }
                catch (WebDriverTimeoutException)
                {
                    var tbodyText = _driver.FindElements(By.CssSelector("tbody")).FirstOrDefault()?.Text ?? "(vacío)";
                    Assert.Fail(
                        $"La tabla de Ver Ventas no tiene resultados después del filtrado. " +
                        $"{ObtenerDiagnosticoSistema()} " +
                        $"Verifique que la fecha del filtro corresponda a la fecha de registro de la venta. " +
                        $"Contenido de tbody: '{tbodyText}'");
                }
            }

            // 2. Buscar el botón de acción con locator principal y fallback
            IWebElement? btn = null;
            try
            {
                btn = _wait.Until(d =>
                    d.FindElements(VentasLocators.AjusteComprobante.AccionPrimerComprobante)
                     .FirstOrDefault(Visible)
                    ?? d.FindElements(VentasLocators.AjusteComprobante.AccionPrimerComprobanteFallback)
                        .FirstOrDefault(Visible));
            }
            catch (WebDriverTimeoutException)
            {
                var totalFilas = _driver.FindElements(By.XPath("//tbody/tr")).Count;
                var totalColumnas = _driver.FindElements(By.XPath("//tbody/tr[1]/td")).Count;
                Assert.Fail(
                    $"La tabla tiene {totalFilas} fila(s) y {totalColumnas} columna(s) en la primera fila, " +
                    $"pero no se encontró el botón de acción. " +
                    $"{ObtenerDiagnosticoSistema()} " +
                    $"El locator principal apunta a td[11] pero la tabla tiene {totalColumnas} columnas. " +
                    $"Revise VentasLocators.AjusteComprobante.AccionPrimerComprobante.");
            }

            ScrollTo(btn!);
            ClickSeguro(btn!);
            Thread.Sleep(2000);
        }

        public void AbrirModalAjustesDeComprobante()
        {
            ClickAccionPrimerComprobante();
            EsperarMenuAccionesVisible();
        }

        // ── Seleccionar tab del modal ───────────────────────────────────────
        public void SeleccionarTabAjuste(string tab)
        {
            By locator = tab.Trim().ToLower() switch
            {
                "nota de débito" or "nota de debito" => VentasLocators.AjusteComprobante.TabNotaDebito,
                "nota de crédito" or "nota de credito" => VentasLocators.AjusteComprobante.TabNotaCredito,
                "ver documento" => VentasLocators.AjusteComprobante.TabVerDocumento,
                "invalidar" => VentasLocators.AjusteComprobante.TabInvalidar,
                _ => throw new Exception($"Tab de ajuste no reconocido: {tab}")
            };

            var el = _driver.FindElements(locator).FirstOrDefault(Visible);
            if (el == null)
            {
                ClickAccionPrimerComprobante();
                el = _wait.Until(d => d.FindElements(locator).FirstOrDefault(Visible));
            }

            if (el == null)
                Assert.Fail($"No se encontró la pestaña '{tab}' en el modal de ajuste. " + ObtenerDiagnosticoSistema());

            ScrollTo(el);
            ClickSeguro(el);
            Thread.Sleep(1500);
        }

        // ── Tipo de nota de débito ──────────────────────────────────────────
        public void SeleccionarTipoNotaDebito(string tipo)
        {
            var select = new SelectElement(
                _wait.Until(ExpectedConditions.ElementToBeClickable(
                    VentasLocators.AjusteComprobante.TipoNotaDebitoSelect)));
            select.SelectByText(tipo);
            Thread.Sleep(1000);
        }

        // ── Tipo de nota de crédito ─────────────────────────────────────────
        public void SeleccionarTipoNotaCredito(string tipo)
        {
            // Strategy 1: native <select> (in case backend renders a real select element)
            var nativeSelect = _driver
                .FindElements(By.XPath("//label[contains(normalize-space(),'Tipo de nota de cr')]/following::select[1]"))
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
            if (nativeSelect != null)
            {
                new SelectElement(nativeSelect).SelectByText(tipo);
                Thread.Sleep(1000);
                return;
            }

            // Strategy 2: custom dropdown — find trigger by placeholder text or label proximity
            var trigger = _driver
                .FindElements(By.XPath(
                    "//*[contains(@class,'select-trigger')][contains(normalize-space(),'Selecciona un tipo de nota')] | " +
                    "//label[contains(normalize-space(),'Tipo de nota de cr')]/following::*[contains(@class,'select-trigger')][1]"))
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

            if (trigger == null)
                trigger = _wait.Until(ExpectedConditions.ElementToBeClickable(
                    VentasLocators.AjusteComprobante.TipoNotaCreditoSelect));

            ClickSeguro(trigger);
            Thread.Sleep(500);

            var opcion = _wait.Until(ExpectedConditions.ElementToBeClickable(
                VentasLocators.AjusteComprobante.TipoNotaCreditoOpcion(tipo)));
            ClickSeguro(opcion);
            Thread.Sleep(1000);
        }

        // ── Comprobante destino ─────────────────────────────────────────────
        public void SeleccionarComprobanteDestino(string comprobante)
        {
            try
            {
                var select = new SelectElement(
                    _wait.Until(ExpectedConditions.ElementToBeClickable(
                        VentasLocators.AjusteComprobante.ComprobanteSelect)));
                select.SelectByText(comprobante);
            }
            catch
            {
                Console.WriteLine($"Comprobante '{comprobante}' ya seleccionado o autocompletado.");
            }
            Thread.Sleep(500);
        }

        // ── Serie ───────────────────────────────────────────────────────────
        public void SeleccionarSerie(string serie)
        {
            var label = _driver.FindElements(VentasLocators.AjusteComprobante.SerieLabel(serie))
                .FirstOrDefault(Visible);
            if (label != null)
            {
                ClickSeguro(label);
                Thread.Sleep(500);
                return;
            }

            var radio = _driver.FindElements(VentasLocators.AjusteComprobante.SerieRadio(serie))
                .FirstOrDefault(e => { try { return e.Enabled; } catch { return false; } });
            if (radio != null)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", radio);
                Thread.Sleep(500);
                return;
            }

            Console.WriteLine($"Serie '{serie}' posiblemente ya seleccionada.");
        }

        // ── Motivo o Sustento ───────────────────────────────────────────────
        public void IngresarMotivoSustento(string motivo)
        {
            var input = _wait.Until(ExpectedConditions.ElementToBeClickable(
                VentasLocators.AjusteComprobante.MotivoSustento));
            input.Clear();
            input.SendKeys(motivo);
            Thread.Sleep(300);
        }

        // ── Monto del interés (ND - Intereses por mora) ─────────────────────
        public void IngresarMontoInteres(string monto)
        {
            var input = _wait.Until(ExpectedConditions.ElementToBeClickable(
                VentasLocators.AjusteComprobante.MontoInteres));
            input.Clear();
            input.SendKeys(monto);
            input.SendKeys(Keys.Tab);
            Thread.Sleep(500);
        }

        // ── Importe NC (NC - Descuento global) ─────────────────────────────
        public void IngresarImporteNC(string importe)
        {
            ExpandirSeccion("Detalle");
            Thread.Sleep(500);

            var input = _wait.Until(ExpectedConditions.ElementToBeClickable(
                VentasLocators.AjusteComprobante.ImporteNCInput));
            ScrollTo(input);
            input.Clear();
            input.SendKeys(importe);
            input.SendKeys(Keys.Tab);
            Thread.Sleep(500);
        }

        // ── Importe detalle por ítem (NC - Descuento por ítem) ──────────────
        public void IngresarImporteDetalle(string importe)
        {
            ExpandirSeccion("Detalle");
            Thread.Sleep(500);

            // Reintento por StaleElementReferenceException: el DOM puede refrescarse
            // despues de expandir la seccion antes de que el input sea estable.
            for (int intento = 0; intento < 3; intento++)
            {
                try
                {
                    var input = _wait.Until(ExpectedConditions.ElementToBeClickable(
                        VentasLocators.AjusteComprobante.ImporteDetalleInput));
                    ScrollTo(input);
                    input.Clear();
                    input.SendKeys(importe);
                    input.SendKeys(Keys.Tab);
                    Thread.Sleep(500);
                    return;
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine($"[NC] StaleElement en ImporteDetalle intento {intento + 1} - reintentando.");
                    Thread.Sleep(400);
                }
            }
        }

        // ── Cantidad a devolver (NC - Devolución por ítem) ──────────────────
        public void IngresarCantidadDevolver(string cantidad)
        {
            if (string.IsNullOrWhiteSpace(cantidad) || cantidad.Trim() == "-")
            {
                Console.WriteLine("Cantidad a devolver omitida.");
                return;
            }

            Console.WriteLine($"Ingresando cantidad a devolver: {cantidad}");
            ExpandirSeccion("Detalle");
            Thread.Sleep(800);

            IWebElement? input = null;
            for (int intento = 0; intento < 3; intento++)
            {
                try
                {
                    input = _wait.Until(d => d.FindElements(VentasLocators.AjusteComprobante.CantidadDevolverInput)
                        .FirstOrDefault(e => { try { return e.Displayed && e.Enabled; } catch { return false; } }));
                    if (input == null) { Thread.Sleep(400); continue; }
                    ScrollTo(input);
                    Thread.Sleep(200);
                    // Intenta clic normal; si no es interactuable usa JS
                    try { input.Click(); }
                    catch { ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", input); }
                    input.Clear();
                    input.SendKeys(cantidad);
                    input.SendKeys(Keys.Tab);
                    Thread.Sleep(500);
                    return;
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine($"[NC] StaleElement en CantidadDevolver intento {intento + 1}.");
                    Thread.Sleep(400);
                }
                catch (ElementNotInteractableException)
                {
                    Console.WriteLine($"[NC] ElementNotInteractable en CantidadDevolver - usando JS.");
                    if (input != null)
                    {
                        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('input', {bubbles:true}));", input, cantidad);
                        Thread.Sleep(500);
                        return;
                    }
                }
            }
        }

        // ── Detalle aumento valor (ND - Aumento en el valor) ────────────────
        public void IngresarTotalAumentoValor(string monto)
        {
            var header = _driver.FindElements(VentasLocators.AjusteComprobante.DetalleNotaDebitoHeader)
                .FirstOrDefault();
            if (header != null)
            {
                ScrollTo(header);
                ClickSeguro(header);
                Thread.Sleep(800);
            }

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(d => d.FindElements(VentasLocators.AjusteComprobante.DetalleAumentoInput).Any(BotonAccionActivo));
            Thread.Sleep(400);

            for (int intento = 0; intento < 3; intento++)
            {
                try
                {
                    var input = _driver.FindElements(VentasLocators.AjusteComprobante.DetalleAumentoInput)
                        .FirstOrDefault(BotonAccionActivo) ?? throw new Exception("No se encontró el campo interactuable para ingresar el aumento del valor.");

                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);
                    Thread.Sleep(300);

                    input = _driver.FindElements(VentasLocators.AjusteComprobante.DetalleAumentoInput)
                        .FirstOrDefault(BotonAccionActivo) ?? throw new Exception("No se encontró el campo interactuable para ingresar el aumento del valor.");

                    ClickSeguro(input);
                    input.SendKeys(Keys.Control + "a");
                    input.SendKeys(monto);
                    ((IJavaScriptExecutor)_driver).ExecuteScript(
                        "arguments[0].dispatchEvent(new Event('change', { bubbles: true })); arguments[0].blur();",
                        input);
                    Thread.Sleep(500);
                    return;
                }
                catch (StaleElementReferenceException)
                {
                    if (intento == 2) throw;
                    Thread.Sleep(500);
                }
            }
        }

        // ── Expandir sección accordion ──────────────────────────────────────
        public void ExpandirSeccion(string nombre)
        {
            var seccion = _driver.FindElements(VentasLocators.AjusteComprobante.SeccionAccordion(nombre))
                .FirstOrDefault(Visible);

            if (seccion == null)
            {
                Console.WriteLine($"Sección '{nombre}' no encontrada o no visible.");
                return;
            }

            ScrollTo(seccion);
            Thread.Sleep(200);

            if (!SeccionEstaExpandida(seccion))
            {
                ClickSeguro(seccion);
                Thread.Sleep(800);
            }
        }

        // ── Pago — Tipo (Contado / Crédito) ────────────────────────────────
        public void SeleccionarTipoPagoAjuste(string tipo)
        {
            AsegurarSeccionPagoDisponible();
            Thread.Sleep(500);

            By locator = tipo.Trim().ToLower() switch
            {
                "contado" => VentasLocators.AjusteComprobante.PagoContadoRadio,
                "credito" or "crédito" => VentasLocators.AjusteComprobante.PagoCreditoRadio,
                _ => throw new Exception($"Tipo de pago no reconocido: {tipo}")
            };

            var el = _wait.Until(d => d.FindElements(locator).FirstOrDefault(Visible));
            if (el == null)
                Assert.Fail($"No se encontró la opción de pago '{tipo}'. " + ObtenerDiagnosticoSistema());

            ClickSeguro(el);
            Thread.Sleep(800);
        }

        // ── Pago — Monto inicial (Crédito) ─────────────────────────────────
        public void IngresarMontoInicial(string monto)
        {
            var input = _wait.Until(ExpectedConditions.ElementToBeClickable(
                VentasLocators.AjusteComprobante.MontoInicialInput));
            ScrollTo(input);
            Thread.Sleep(200);
            input = _driver.FindElements(VentasLocators.AjusteComprobante.MontoInicialInput)
                .FirstOrDefault() ?? throw new Exception("No se encontró el campo Monto inicial.");
            input.Click();
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(monto);
            input.SendKeys(Keys.Tab);
            Thread.Sleep(500);
        }

        // ── Pago — Medio de pago (Efectivo, etc.) ──────────────────────────
        public void SeleccionarMedioPago(string medio)
        {
            if (medio.Trim().Equals("Efectivo", StringComparison.OrdinalIgnoreCase))
            {
                var tab = _wait.Until(d =>
                    d.FindElements(VentasLocators.AjusteComprobante.MedioPagoEfectivo)
                     .FirstOrDefault(Visible));
                if (tab != null)
                {
                    ClickSeguro(tab);
                    Thread.Sleep(500);
                }
            }
        }

        // ── Pago — Observación ──────────────────────────────────────────────
        public void IngresarObservacion(string observacion)
        {
            var input = _driver.FindElements(VentasLocators.AjusteComprobante.ObservacionPago)
                .FirstOrDefault(Visible);
            if (input != null)
            {
                input.Clear();
                input.SendKeys(observacion);
                Thread.Sleep(300);
            }
        }

        // ── Entrega (NC) ───────────────────────────────────────────────────
        public void SeleccionarEntrega(string tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo) || tipo.Trim() == "-")
            {
                Console.WriteLine("Tipo de entrega en ajuste omitido.");
                return;
            }

            ExpandirSeccion("Entrega");
            Thread.Sleep(500);

            By locator = tipo.Trim().ToLower() switch
            {
                "inmediata" => VentasLocators.AjusteComprobante.EntregaInmediata,
                "diferida" => VentasLocators.AjusteComprobante.EntregaDiferida,
                _ => throw new Exception($"Tipo de entrega no reconocido: {tipo}")
            };

            var el = _wait.Until(d => d.FindElements(locator).FirstOrDefault(Visible));
            if (el == null)
                Assert.Fail($"No se encontró la opción de entrega '{tipo}'. " + ObtenerDiagnosticoSistema());

            ClickSeguro(el);
            Thread.Sleep(500);
        }

        // ── Devolución (NC — Pago) ─────────────────────────────────────────
        public void SeleccionarDevolucion(string tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo) || tipo.Trim() == "-")
            {
                Console.WriteLine("Tipo de devolución omitido.");
                return;
            }

            AsegurarSeccionPagoDisponible();
            Thread.Sleep(500);

            By locator = tipo.Trim().ToLower() switch
            {
                "contado" => VentasLocators.AjusteComprobante.DevolucionContado,
                "credito" or "crédito" => VentasLocators.AjusteComprobante.DevolucionCredito,
                _ => throw new Exception($"Tipo de devolución no reconocido: {tipo}")
            };

            var el = _wait.Until(d => d.FindElements(locator).FirstOrDefault(Visible));
            if (el == null)
                Assert.Fail($"No se encontró la opción de devolución '{tipo}'. " + ObtenerDiagnosticoSistema());

            ClickSeguro(el);
            Thread.Sleep(800);
        }

        // ── Guardar ajuste ─────────────────────────────────────────────────
        // Espera a que el boton Guardar se habilite (Angular puede tardar en validar)
        // antes de hacer clic. Asi se evita hacer clic en un boton deshabilitado que
        // no dispararia el guardado y provocaria un falso "no se genero el comprobante".
        public void ClickGuardarAjuste()
        {
            IWebElement? btn = _driver.FindElements(VentasLocators.AjusteComprobante.GuardarAjuste)
                .FirstOrDefault(Visible);
            btn ??= _driver.FindElements(VentasLocators.AjusteComprobante.GuardarAjusteFallback)
                .FirstOrDefault(Visible);

            if (btn == null)
                Assert.Fail("No se encontró el botón Guardar en el modal de ajuste. " + ObtenerDiagnosticoSistema());

            ScrollTo(btn);

            // Espera hasta 5s a que el boton quede habilitado.
            try
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(5)) { PollingInterval = TimeSpan.FromMilliseconds(300) }
                    .Until(d =>
                    {
                        var b = d.FindElements(VentasLocators.AjusteComprobante.GuardarAjuste).FirstOrDefault(Visible)
                            ?? d.FindElements(VentasLocators.AjusteComprobante.GuardarAjusteFallback).FirstOrDefault(Visible);
                        if (b == null) return false;
                        bool deshabilitado = !b.Enabled || b.GetAttribute("disabled") != null
                            || (b.GetAttribute("class") ?? "").Contains("disabled");
                        return !deshabilitado;
                    });
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("[Ajuste][Guardar] El boton Guardar sigue deshabilitado tras 5s - el sistema bloquea el guardado.");
            }

            // Re-localiza por si el DOM cambio durante la espera.
            btn = _driver.FindElements(VentasLocators.AjusteComprobante.GuardarAjuste).FirstOrDefault(Visible)
                ?? _driver.FindElements(VentasLocators.AjusteComprobante.GuardarAjusteFallback).FirstOrDefault(Visible);
            if (btn == null)
                Assert.Fail("El botón Guardar desaparecio del modal de ajuste. " + ObtenerDiagnosticoSistema());

            ClickSeguro(btn);
            Thread.Sleep(3000);
        }

        // ── Verificaciones ─────────────────────────────────────────────────
        // Verifica que el ajuste (NC/ND) se haya generado correctamente.
        // Clasifica el resultado en: EXITO, FALLA DEL SISTEMA (HTTP 4xx/5xx),
        // VALIDACION (campos/popup) o DESCONOCIDO — para que el reporte indique
        // claramente si el fallo es del sistema o de la automatizacion.
        public void VerificarAjusteExitoso()
        {
            // 1. Espera hasta 15s un desenlace definitivo: exito, popup de error o cierre del modal.
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            try
            {
                wait.Until(d =>
                    d.FindElements(VentasLocators.AjusteComprobante.MensajeExito).Any(Visible) ||
                    d.FindElements(By.CssSelector(".swal2-popup")).Any(Visible) ||
                    d.FindElements(By.CssSelector(".toast-error, .toast-success")).Any(Visible) ||
                    !string.IsNullOrWhiteSpace(ObtenerTextoModalResultado()) ||
                    !d.FindElements(By.XPath("//div[contains(normalize-space(),'Ajuste de Comprobante')]")).Any(Visible));
            }
            catch (WebDriverTimeoutException) { }

            // 2. Captura el texto de cualquier popup swal2 visible (titulo + cuerpo).
            string textoPopup = string.Empty;
            var swalPopup = _driver.FindElements(By.CssSelector(".swal2-popup")).FirstOrDefault(Visible);
            if (swalPopup != null)
            {
                string titulo = _driver.FindElements(By.CssSelector(".swal2-title")).FirstOrDefault(Visible)?.Text?.Trim() ?? string.Empty;
                string cuerpo = _driver.FindElements(By.CssSelector(".swal2-html-container")).FirstOrDefault(Visible)?.Text?.Trim() ?? string.Empty;
                textoPopup = $"{titulo} {cuerpo}".Trim();
            }

            // Junta todo el texto visible relevante para clasificar y diagnosticar.
            var mensajesVisibles = string.Join(" | ",
                _driver.FindElements(By.CssSelector(".toast, .toast-message, .swal2-popup, [role='alert'], .alert"))
                    .Where(Visible).Select(e => e.Text?.Trim()).Where(t => !string.IsNullOrEmpty(t)).Distinct());
            string textoModalResultado = ObtenerTextoModalResultado();
            string textoTotal = $"{textoPopup} | {mensajesVisibles} | {textoModalResultado}";
            string textoNorm = textoTotal.ToLowerInvariant();

            bool esExitoPopup = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeExito).Any(Visible);
            bool modalCerrado = !_driver.FindElements(By.XPath("//div[contains(normalize-space(),'Ajuste de Comprobante')]")).Any(Visible);

            // 3. EXITO: toast/swal de exito, modal cerrado, o texto de confirmacion.
            bool hayExito = esExitoPopup || modalCerrado ||
                textoNorm.Contains("registr") || textoNorm.Contains("generad") ||
                textoNorm.Contains("correctamente") && !textoNorm.Contains("complete");

            if (hayExito && !textoNorm.Contains("http") && !textoNorm.Contains("error") && !textoNorm.Contains("400") && !textoNorm.Contains("500"))
            {
                Console.WriteLine($"[Ajuste][Exito] Ajuste generado. modalCerrado={modalCerrado}, mensaje='{ResumirTextoResultado(textoTotal)}'.");
                CerrarPopupSiExiste();
                return;
            }

            // 4. Cierra el popup para no bloquear el navegador antes de fallar.
            try
            {
                _driver.FindElements(By.CssSelector(".swal2-confirm, .swal2-popup button"))
                    .FirstOrDefault(Visible)?.Click();
                Thread.Sleep(300);
            }
            catch { /* no critico */ }

            // 5. FALLA DEL SISTEMA: error HTTP del backend (no es problema de automatizacion).
            if (textoNorm.Contains("http failure") || textoNorm.Contains("register") ||
                textoNorm.Contains("400") || textoNorm.Contains("500") || textoNorm.Contains("internal"))
            {
                Assert.Fail($"⚠️ FALLA DEL SISTEMA (no de la automatizacion): el backend rechazo el ajuste. " +
                    $"Mensaje del sistema: '{textoPopup}'. {ObtenerDiagnosticoSistema()}");
            }

            // 6. VALIDACION: el sistema pide completar campos o muestra advertencia.
            if (textoNorm.Contains("complete") || textoNorm.Contains("campos requeridos") ||
                textoNorm.Contains("necesario") || textoNorm.Contains("seccion de pago"))
            {
                Assert.Fail($"El sistema bloqueo el guardado por validacion de campos. " +
                    $"Mensaje del sistema: '{(string.IsNullOrEmpty(textoPopup) ? mensajesVisibles : textoPopup)}'. {ObtenerDiagnosticoSistema()}");
            }

            // 7. DESCONOCIDO: no se detecto ningun desenlace claro.
            Assert.Fail(
                "No se genero el comprobante de ajuste ni se detecto error claro. " +
                $"Mensajes visibles: '{(string.IsNullOrEmpty(mensajesVisibles) ? "ninguno" : mensajesVisibles)}'. " +
                $"{ObtenerDiagnosticoSistema()}");
        }

        public void VerificarBloqueoGuardar()
        {
            Thread.Sleep(1000);

            // El sistema puede mostrar un popup de advertencia con boton OK antes de bloquear.
            // Se detecta, registra como evidencia de bloqueo y se cierra para poder continuar.
            bool hayPopupAdvertencia = false;
            try
            {
                var popupOk = _driver.FindElements(By.XPath(
                    "//button[normalize-space()='OK' or normalize-space()='Ok' or normalize-space()='Aceptar'][ancestor::*[contains(@class,'swal2') or contains(@class,'modal')]]"))
                    .FirstOrDefault(Visible);
                if (popupOk != null)
                {
                    hayPopupAdvertencia = true;
                    Console.WriteLine("[Ajuste][BloqueoGuardar] Popup de advertencia detectado - cerrando con OK.");
                    popupOk.Click();
                    Thread.Sleep(500);
                }
            }
            catch { /* no critico */ }

            var mensajeCampos = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeCamposRequeridos)
                .FirstOrDefault(Visible);
            var mensajeError = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeError)
                .FirstOrDefault(Visible);

            bool hayMensajeCampos = mensajeCampos != null;
            bool hayMensajeError = mensajeError != null;
            string textoMensajeCampos = mensajeCampos?.Text?.Trim() ?? string.Empty;
            string textoMensajeError = mensajeError?.Text?.Trim() ?? string.Empty;

            IWebElement? btn = _driver.FindElements(VentasLocators.AjusteComprobante.GuardarAjuste)
                .FirstOrDefault(Visible);
            btn ??= _driver.FindElements(VentasLocators.AjusteComprobante.GuardarAjusteFallback)
                .FirstOrDefault(Visible);

            bool botonVisible = btn != null;
            bool botonEnabled = btn?.Enabled ?? false;
            bool botonDeshabilitado = btn == null || !botonEnabled
                || (btn?.GetAttribute("disabled") != null)
                || ((btn?.GetAttribute("class")) ?? string.Empty).Contains("disabled");

            bool modalSigueAbierto = _driver.FindElements(
                By.XPath("//div[contains(normalize-space(),'Ajuste de Comprobante')]"))
                .Any(Visible);

            Console.WriteLine(
                $"[Ajuste][BloqueoGuardar] mensajeCampos={hayMensajeCampos}('{textoMensajeCampos}'), " +
                $"mensajeError={hayMensajeError}('{textoMensajeError}'), " +
                $"botonVisible={botonVisible}, botonEnabled={botonEnabled}, " +
                $"botonDeshabilitado={botonDeshabilitado}, modalSigueAbierto={modalSigueAbierto}");

            bool hayEvidenciaDeBloqueo = hayMensajeCampos || hayMensajeError || botonDeshabilitado || hayPopupAdvertencia;
            if (hayEvidenciaDeBloqueo)
                Console.WriteLine("[Ajuste][ResultadoFinal] El sistema bloqueó el guardado con evidencia visible.");

            // Captura todo lo visible para dar contexto claro del fallo
            var mensajesVisiblesBloqueo = string.Join(" | ",
                _driver.FindElements(By.CssSelector(".toast, .toast-message, .swal2-popup, [role='alert'], .alert"))
                    .Where(Visible).Select(e => e.Text?.Trim()).Where(t => !string.IsNullOrEmpty(t)));

            Assert.IsTrue(
                hayEvidenciaDeBloqueo,
                $"Se esperaba bloqueo del guardado pero el sistema no mostro evidencia. " +
                $"Boton Guardar: {(botonVisible ? (botonEnabled ? "habilitado" : "deshabilitado") : "no encontrado")}. " +
                $"Mensaje campos: '{textoMensajeCampos}'. " +
                $"Mensaje error: '{textoMensajeError}'. " +
                $"Popup advertencia: {hayPopupAdvertencia}. " +
                $"Otros mensajes visibles: '{(string.IsNullOrEmpty(mensajesVisiblesBloqueo) ? "ninguno" : mensajesVisiblesBloqueo)}'. " +
                ObtenerDiagnosticoSistema());
        }

        // Verifica que el sistema rechace un importe de NC mayor al total (NC014).
        // Un HTTP 4xx/5xx no es una validacion funcional: es una falla visible del sistema.
        public void VerificarMensajeMontoMayor()
        {
            Thread.Sleep(1000);

            try
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(8)) { PollingInterval = TimeSpan.FromMilliseconds(300) }
                    .Until(d =>
                        d.FindElements(VentasLocators.AjusteComprobante.MensajeMontoMayor).Any(Visible) ||
                        d.FindElements(By.CssSelector(".swal2-popup, .toast, .toast-message, [role='alert'], .alert"))
                            .Where(Visible)
                            .Select(e => e.Text?.Trim() ?? string.Empty)
                            .Any(t => TextoPareceFallaHttp(t) || TextoPareceValidacionMontoMayor(t)));
            }
            catch (WebDriverTimeoutException)
            {
                // Se valida igualmente el estado final abajo; el timeout solo evita leer la UI antes del 400 async.
            }

            // Cierra cualquier popup que el sistema haya mostrado, capturando su texto.
            string textoPopup = string.Empty;
            var swal = _driver.FindElements(By.CssSelector(".swal2-popup")).FirstOrDefault(Visible);
            if (swal != null)
            {
                textoPopup = swal.Text?.Trim() ?? string.Empty;
                CerrarPopupSiExiste();
            }

            bool mensajeEspecifico = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeMontoMayor).Any(Visible);

            var textoVisible = (textoPopup + " " + string.Join(" ",
                _driver.FindElements(By.CssSelector(".toast, .toast-message, [role='alert'], .alert, .text-danger, .invalid-feedback"))
                    .Where(Visible).Select(e => e.Text?.Trim()))).ToLowerInvariant();

            if (TextoPareceFallaHttp(textoVisible))
            {
                Assert.Fail(
                    "El sistema mostro una falla tecnica al validar el monto mayor al total. " +
                    $"Se esperaba una validacion funcional de negocio, no un HTTP 4xx/5xx. " +
                    $"Texto del sistema: '{(string.IsNullOrWhiteSpace(textoPopup) ? textoVisible : textoPopup)}'. " +
                    ObtenerDiagnosticoSistema());
            }

            bool validacionFuncional = TextoPareceValidacionMontoMayor(textoVisible);

            IWebElement? btn = _driver.FindElements(VentasLocators.AjusteComprobante.GuardarAjuste)
                .FirstOrDefault(Visible);
            btn ??= _driver.FindElements(VentasLocators.AjusteComprobante.GuardarAjusteFallback)
                .FirstOrDefault(Visible);
            bool botonDeshabilitado = btn == null || !BotonAccionActivo(btn);

            Assert.IsTrue(mensajeEspecifico || validacionFuncional || botonDeshabilitado,
                "Se esperaba que el sistema rechazara el monto mayor al total con validacion funcional visible o boton Guardar deshabilitado. " +
                $"Texto del sistema: '{(string.IsNullOrEmpty(textoPopup) ? "ninguno" : textoPopup)}'. " +
                ObtenerDiagnosticoSistema());

            Console.WriteLine($"[Ajuste][MontoMayor] Bloqueo funcional confirmado. Especifico={mensajeEspecifico}, validacion={validacionFuncional}, botonDeshabilitado={botonDeshabilitado}.");
        }

        public void VerificarMensajeCantidadMayor()
        {
            Thread.Sleep(1000);
            bool hay = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeCantidadMayor)
                .Any(Visible);
            Assert.IsTrue(hay,
                "Se esperaba el mensaje del sistema 'Es necesario que la cantidad a devolver sea menor a la cantidad entregada.'. " +
                ObtenerDiagnosticoSistema());
        }

        // ── Observación de invalidación ────────────────────────────────────
        public void IngresarObservacionInvalidacion(string observacion)
        {
            var input = _wait.Until(ExpectedConditions.ElementToBeClickable(
                VentasLocators.InvalidarVenta.ObservacionInvalidar));
            input.Click();
            input.Clear();
            input.SendKeys(observacion);

            // La UI habilita el submit al confirmar el cambio y perder foco.
            ((IJavaScriptExecutor)_driver).ExecuteScript(
                "arguments[0].dispatchEvent(new Event('input', { bubbles: true }));" +
                "arguments[0].dispatchEvent(new Event('change', { bubbles: true }));" +
                "arguments[0].dispatchEvent(new Event('blur', { bubbles: true }));",
                input);
            Thread.Sleep(500);
        }

        // ── Click Invalidar en el modal ────────────────────────────────────
        public void ClickInvalidarEnModal()
        {
            _wait.Until(d => d.FindElements(VentasLocators.InvalidarVenta.ModalInvalidar).Any(Visible));

            var btn = _wait.Until(d =>
                d.FindElements(VentasLocators.InvalidarVenta.BotonInvalidar)
                 .FirstOrDefault(BotonAccionActivo));

            if (btn == null)
                Assert.Fail("No se encontró el botón Invalidar habilitado en el modal. " + ObtenerDiagnosticoSistema());

            ScrollTo(btn!);
            _wait.Until(_ => BotonAccionActivo(btn!));
            ClickSeguro(btn!);

            ConfirmarInvalidacionSiAparece();

            Thread.Sleep(3000);
        }

        // ── Verificar invalidación exitosa ──────────────────────────────────
        public void IntentarInvalidarEnModal()
        {
            _wait.Until(d => d.FindElements(VentasLocators.InvalidarVenta.ModalInvalidar).Any(Visible));

            var btn = _driver.FindElements(VentasLocators.InvalidarVenta.BotonInvalidar)
                .FirstOrDefault(Visible);

            if (btn == null)
                Assert.Fail("No se encontró el botón Invalidar en el modal. " + ObtenerDiagnosticoSistema());

            if (!BotonAccionActivo(btn))
            {
                Console.WriteLine("Botón Invalidar no activo; se omite el click final.");
                return;
            }

            ScrollTo(btn);
            ClickSeguro(btn);

            ConfirmarInvalidacionSiAparece();

            Thread.Sleep(3000);
        }

        private void ConfirmarInvalidacionSiAparece()
        {
            try
            {
                var waitConfirmacion = new WebDriverWait(_driver, TimeSpan.FromSeconds(6))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(250)
                };
                waitConfirmacion.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

                var btnConfirmar = waitConfirmacion.Until(d =>
                    d.FindElements(VentasLocators.ViewSales.AcceptInvalidation)
                        .FirstOrDefault(BotonAccionActivo));

                if (btnConfirmar == null)
                    return;

                Console.WriteLine($"[Invalidar][Confirmacion] Aceptando confirmacion visible: '{btnConfirmar.Text?.Trim()}'.");
                ScrollTo(btnConfirmar);
                ClickSeguro(btnConfirmar);
                Thread.Sleep(1500);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("[Invalidar][Confirmacion] No aparecio modal de confirmacion tras el click final.");
            }
        }

        public void VerificarInvalidacionExitosa()
        {
            var waitCorto = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            bool hayExito = false;
            try
            {
                waitCorto.Until(d =>
                    d.FindElements(VentasLocators.InvalidarVenta.MensajeExitoInvalidar).Any(Visible) ||
                    d.FindElements(VentasLocators.AjusteComprobante.MensajeExito).Any(Visible));
                hayExito = true;
            }
            catch (WebDriverTimeoutException) { }

            var mensajeExito = _driver.FindElements(VentasLocators.InvalidarVenta.MensajeExitoInvalidar)
                .Concat(_driver.FindElements(VentasLocators.AjusteComprobante.MensajeExito))
                .FirstOrDefault(Visible);
            var mensajeError = ObtenerTextoModalResultado();
            if (string.IsNullOrWhiteSpace(mensajeError))
            {
                mensajeError = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeError)
                    .Where(Visible)
                    .Select(e => ResumirTextoResultado(e.Text))
                    .FirstOrDefault(TextoPareceMensajeSistema);
            }
            string textoExito = ResumirTextoResultado(mensajeExito?.Text);

            Console.WriteLine(
                $"[Invalidar][Resultado] hayExito={hayExito}, mensajeExito='{textoExito}', " +
                $"mensajeError='{mensajeError ?? string.Empty}'.");

            if (!hayExito && !string.IsNullOrWhiteSpace(mensajeError) && TextoPareceErrorSistema(mensajeError))
                Assert.Fail($"El sistema respondió con error al invalidar la venta: '{mensajeError}'.");

            if (!hayExito && !string.IsNullOrWhiteSpace(mensajeError))
                Assert.Fail($"No se detectÃ³ mensaje de confirmaciÃ³n de invalidaciÃ³n exitosa. Error visible: '{mensajeError}'.");

            Assert.IsTrue(hayExito,
                "No se detectó mensaje de confirmación de invalidación exitosa (toast-success o swal2-success). " +
                ObtenerDiagnosticoSistema());

            Console.WriteLine("[Invalidar][ResultadoFinal] La invalidación se procesó correctamente.");
            CerrarPopupSiExiste();
        }

        public void VerificarSeccionEntregaNoVisibleEnInvalidacion()
        {
            Thread.Sleep(500);

            bool visible = _driver.FindElements(VentasLocators.InvalidarVenta.SeccionEntregaAccordion)
                .Any(Visible);

            Console.WriteLine($"[Invalidar][FueraDePlazo] seccionEntregaVisible={visible}.");

            Assert.IsFalse(visible,
                "Se esperaba que la sección Entrega no se muestre en la invalidación fuera de plazo. " +
                ObtenerDiagnosticoSistema());
        }

        public void SeleccionarEntregaInvalidacion(string tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo) || tipo.Trim() == "-")
                return;

            _wait.Until(d => d.FindElements(VentasLocators.InvalidarVenta.ModalInvalidar).Any(Visible));

            var seccion = _driver.FindElements(VentasLocators.InvalidarVenta.SeccionEntregaAccordion)
                .FirstOrDefault(Visible);
            Assert.That(seccion, Is.Not.Null,
                "No se encontro la seccion Entrega en el modal de invalidacion. " + ObtenerDiagnosticoSistema());

            ScrollTo(seccion!);
            ClickSeguro(seccion!);
            Thread.Sleep(500);

            By locator = tipo.Trim().ToLowerInvariant() switch
            {
                "inmediata" => VentasLocators.InvalidarVenta.EntregaInmediata,
                "diferida" => VentasLocators.InvalidarVenta.EntregaDiferida,
                _ => throw new Exception($"Tipo de devolucion para invalidacion no reconocido: {tipo}")
            };

            var opcion = _wait.Until(d => d.FindElements(locator).FirstOrDefault(Visible));
            Assert.That(opcion, Is.Not.Null,
                $"No se encontro la devolucion '{tipo}' en el modal de invalidacion. " + ObtenerDiagnosticoSistema());

            ScrollTo(opcion!);
            ClickSeguro(opcion!);
            Thread.Sleep(500);
        }

        public void VerificarMensajeFueraDePlazoInvalidacion()
        {
            var waitCorto = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
            bool hayMensaje = false;
            try
            {
                waitCorto.Until(d => d.FindElements(VentasLocators.InvalidarVenta.MensajeFueraDePlazoInvalidar).Any(Visible));
                hayMensaje = true;
            }
            catch (WebDriverTimeoutException) { }

            var textoMensaje = _driver.FindElements(VentasLocators.InvalidarVenta.MensajeFueraDePlazoInvalidar)
                .FirstOrDefault(Visible)?.Text;
            textoMensaje = ResumirTextoResultado(textoMensaje);

            Console.WriteLine(
                $"[Invalidar][FueraDePlazo] mensajeDetectado={hayMensaje}, texto='{textoMensaje}'.");
            if (hayMensaje)
                Console.WriteLine("[Invalidar][ResultadoFinal] El sistema mostró el mensaje de fuera de plazo.");

            Assert.IsTrue(hayMensaje,
                "Se esperaba el mensaje del sistema 'Fuera de plazo (usar Nota de Credito)'. " +
                ObtenerDiagnosticoSistema());
        }

        // ── Verificar botón Invalidar no activo (CP067) ────────────────────
        public void VerificarBotonInvalidarNoVisible()
        {
            Thread.Sleep(500);

            bool visible = _driver.FindElements(VentasLocators.InvalidarVenta.BotonInvalidar)
                .Any(Visible);

            Console.WriteLine($"[Invalidar][FueraDePlazo] botonInvalidarVisible={visible}.");
            if (!visible)
                Console.WriteLine("[Invalidar][ResultadoFinal] El botón final Invalidar no se mostró por fuera de plazo.");

            Assert.IsFalse(visible,
                "No se esperaba ver el botÃ³n final Invalidar en la invalidaciÃ³n fuera de plazo.");
        }

        public void VerificarBotonInvalidarNoActivo()
        {
            Thread.Sleep(1000);

            var btn = _driver.FindElements(VentasLocators.InvalidarVenta.BotonInvalidar)
                .FirstOrDefault(Visible);

            bool botonDeshabilitado = btn == null || !BotonAccionActivo(btn);

            bool hayMensajeCampos = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeCamposRequeridos)
                .Any(Visible);

            Assert.IsTrue(botonDeshabilitado || hayMensajeCampos,
                "Se esperaba que el botón Invalidar estuviera deshabilitado o que el sistema mostrara un mensaje de campos requeridos. " +
                ObtenerDiagnosticoSistema());
        }

        // ── Helpers privados ───────────────────────────────────────────────
        public void VerificarOpcionDelModalAjustesNoVisible(string opcion)
        {
            Thread.Sleep(500);

            var locator = LocatorOpcionAccion(opcion);
            bool visible = _driver.FindElements(locator).Any(Visible);

            Console.WriteLine($"[Ajuste][Opciones] opcion='{opcion}', visible={visible}.");

            Assert.IsFalse(visible,
                $"No se esperaba ver la opción '{opcion}' en el modal ajustes de comprobante. " +
                ObtenerDiagnosticoSistema());
        }

        public void VerificarModalClonarVisible()
        {
            bool visible = _driver.FindElements(VentasLocators.AjusteComprobante.ModalClonar)
                .Any(Visible);

            Assert.IsTrue(visible, "Se esperaba visualizar el modal 'Clonar venta'. " + ObtenerDiagnosticoSistema());
        }

        public void SeleccionarPestanaModoClonar(string modo)
        {
            var tab = _wait.Until(d =>
                d.FindElements(VentasLocators.AjusteComprobante.PestanaModoClonar(modo))
                    .FirstOrDefault(Visible));

            if (tab == null)
                Assert.Fail($"No se encontro la pestaña '{modo}' en el modal Clonar venta. " + ObtenerDiagnosticoSistema());

            ScrollTo(tab);
            ClickSeguro(tab);
            Thread.Sleep(800);
        }

        public void ModificarCantidadPrimerItemClonar(string cantidad)
        {
            var input = _wait.Until(ExpectedConditions.ElementToBeClickable(
                VentasLocators.AjusteComprobante.CantidadPrimerItemClonar));

            ScrollTo(input);
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            input.SendKeys(cantidad);
            input.SendKeys(Keys.Tab);
            Thread.Sleep(800);
        }

        public void SeleccionarEntregaClonar(string tipo)
        {
            bool opcionesVisibles = _driver.FindElements(VentasLocators.AjusteComprobante.EntregaInmediataClonar).Any(Visible)
                || _driver.FindElements(VentasLocators.AjusteComprobante.EntregaDiferidaClonar).Any(Visible);

            if (!opcionesVisibles)
            {
                var seccion = _driver.FindElements(VentasLocators.AjusteComprobante.SeccionEntregaClonar)
                    .FirstOrDefault(Visible);

                if (seccion != null)
                {
                    ScrollTo(seccion);
                    ClickSeguro(seccion);
                    Thread.Sleep(600);
                }
            }

            By locator = tipo.Trim().Equals("Diferida", StringComparison.OrdinalIgnoreCase)
                ? VentasLocators.AjusteComprobante.EntregaDiferidaClonar
                : VentasLocators.AjusteComprobante.EntregaInmediataClonar;

            var opcion = _wait.Until(d => d.FindElements(locator).FirstOrDefault(Visible));
            if (opcion == null)
                Assert.Fail($"No se encontro la entrega '{tipo}' en el modal Clonar venta. " + ObtenerDiagnosticoSistema());

            ScrollTo(opcion);
            ClickSeguro(opcion);
            Thread.Sleep(800);
        }

        // Modifica el cliente en el modal Clonar venta buscando por DNI o RUC.
        // El campo de cliente esta dentro de la seccion Facturacion colapsada — se expande primero.
        public void ModificarClienteClonar(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento) || documento.Trim() == "-") return;

            // Expandir la seccion Facturacion dentro del modal si esta colapsada
            var seccionFacturacion = _driver.FindElements(By.XPath(
                "//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Clonar venta')]]" +
                "//*[contains(normalize-space(),'Facturaci')][contains(@class,'accordion') or contains(@class,'section') or self::button or self::h5 or self::div[@role='button']]"))
                .FirstOrDefault(Visible);

            if (seccionFacturacion != null)
            {
                bool clienteVisible = _driver.FindElements(VentasLocators.AjusteComprobante.ClienteInputClonar)
                    .Any(Visible);
                if (!clienteVisible)
                {
                    Console.WriteLine("[ClonarVenta] Expandiendo seccion Facturacion...");
                    seccionFacturacion.Click();
                    Thread.Sleep(600);
                }
            }

            var input = _wait.Until(d =>
                d.FindElements(VentasLocators.AjusteComprobante.ClienteInputClonar)
                    .FirstOrDefault(Visible));
            Assert.IsNotNull(input, $"No se encontro el campo de cliente en el modal Clonar venta. " +
                "Verifique que la seccion Facturacion este expandida.");

            input.Clear();
            input.SendKeys(documento.Trim());
            Thread.Sleep(200);

            try
            {
                var lupa = _driver.FindElements(VentasLocators.AjusteComprobante.ClienteLupaClonar)
                    .FirstOrDefault(Visible);
                if (lupa != null) lupa.Click();
                else input.SendKeys(OpenQA.Selenium.Keys.Enter);
            }
            catch { input.SendKeys(OpenQA.Selenium.Keys.Enter); }

            Thread.Sleep(1500);
            Console.WriteLine($"[ClonarVenta] Cliente modificado a: {documento}");
        }

        public void ClickClonarVenta()
        {
            var boton = _wait.Until(d =>
                d.FindElements(VentasLocators.AjusteComprobante.BotonClonarVenta)
                    .FirstOrDefault(Visible));

            if (boton == null)
                Assert.Fail("No se encontro el boton 'Clonar venta' en el modal. " + ObtenerDiagnosticoSistema());

            ScrollTo(boton);
            ClickSeguro(boton);
            Thread.Sleep(1500);
        }

        public void VerificarClonacionExitosa()
        {
            Thread.Sleep(2000);

            bool hayExito = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeExito)
                .Any(Visible);
            bool modalCerrado = !_driver.FindElements(VentasLocators.AjusteComprobante.ModalClonar)
                .Any(Visible);

            Console.WriteLine($"[Clonar][Resultado] hayExito={hayExito}, modalCerrado={modalCerrado}.");

            Assert.IsTrue(
                hayExito || modalCerrado,
                "No se detecto confirmacion de exito ni cierre del modal Clonar venta. " +
                ObtenerDiagnosticoSistema());
        }

        private string ObtenerDiagnosticoSistema()
        {
            var texto = ObtenerMensajesSistemaVisibles();
            return string.IsNullOrWhiteSpace(texto)
                ? "Mensaje visible del sistema: (ninguno)."
                : $"Mensaje visible del sistema: '{texto}'.";
        }

        private string ObtenerMensajesSistemaVisibles()
        {
            var textos = new List<string>();

            By[] locators =
            {
                VentasLocators.AjusteComprobante.MensajeCamposRequeridos,
                VentasLocators.AjusteComprobante.MensajeError,
                VentasLocators.AjusteComprobante.MensajeMontoMayor,
                VentasLocators.AjusteComprobante.MensajeCantidadMayor,
                VentasLocators.AjusteComprobante.MensajeExito,
                VentasLocators.InvalidarVenta.MensajeFueraDePlazoInvalidar,
                VentasLocators.InvalidarVenta.MensajeExitoInvalidar,
                By.CssSelector(".swal2-popup, .toast, .toast-message, .toast-error, .toast-warning, .toast-success"),
                By.CssSelector(".alert, .alert-danger, .alert-warning, .alert-success, [role='alert']"),
                By.CssSelector(".invalid-feedback, .text-danger, .validation-message")
            };

            foreach (var locator in locators)
            {
                try
                {
                    textos.AddRange(_driver.FindElements(locator)
                        .Where(Visible)
                        .Select(e => ResumirTextoResultado(e.Text))
                        .Where(TextoPareceMensajeSistema));
                }
                catch (WebDriverException)
                {
                    // El DOM puede cambiar mientras aparece/cierra un toast.
                }
            }

            return string.Join(" | ", textos
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .Take(6));
        }

        private string ObtenerTextoModalResultado()
        {
            var selectoresResultado = new[]
            {
                ".swal2-popup",
                ".toast, .toast-message, .toast-error, .toast-warning, .toast-success",
                ".alert-danger, .alert-warning, .alert-success"
            };

            foreach (var selector in selectoresResultado)
            {
                var texto = _driver.FindElements(By.CssSelector(selector))
                    .Where(Visible)
                    .Select(e => ResumirTextoResultado(e.Text))
                    .FirstOrDefault(TextoPareceResultado);

                if (!string.IsNullOrWhiteSpace(texto))
                    return texto;
            }

            return string.Empty;
        }

        private static bool TextoPareceResultado(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            if (texto.Length > 500)
                return false;

            return texto.Contains("Correcto", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("Error", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("exito", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("exitos", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("registro correctamente", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("generado correctamente", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("Es necesario", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("Complete los campos", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("no permite", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("monto de nota", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("invalidar", StringComparison.OrdinalIgnoreCase);
        }

        private static bool TextoPareceMensajeSistema(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            if (texto.Length > 500)
                return false;

            return texto.Contains("Correcto", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("Error", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("exito", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("exitos", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("Fuera de plazo", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("Es necesario", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("Complete los campos", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("campos requeridos", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("no permite", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("monto de nota", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("cantidad a devolver", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("invalidar", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("registr", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("generad", StringComparison.OrdinalIgnoreCase);
        }

        private static bool TextoPareceErrorSistema(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            return texto.Contains("Error", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("no permite", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("no se pudo", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("inval", StringComparison.OrdinalIgnoreCase);
        }

        private static bool TextoPareceFallaHttp(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            return texto.Contains("Http failure", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("failure response", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("register-credit-note", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains(" 400", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains(": 400", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains(" 500", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains(": 500", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("Internal Server Error", StringComparison.OrdinalIgnoreCase);
        }

        private static bool TextoPareceValidacionMontoMayor(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            return texto.Contains("monto", StringComparison.OrdinalIgnoreCase) &&
                   (texto.Contains("menor", StringComparison.OrdinalIgnoreCase) ||
                    texto.Contains("total", StringComparison.OrdinalIgnoreCase)) ||
                   texto.Contains("Complete los campos", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("campos requeridos", StringComparison.OrdinalIgnoreCase) ||
                   texto.Contains("Es necesario", StringComparison.OrdinalIgnoreCase);
        }

        private static string ResumirTextoResultado(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var lineas = texto
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            var relevantes = lineas
                .Where(l =>
                    l.Contains("Correcto", StringComparison.OrdinalIgnoreCase) ||
                    l.Contains("Se registró correctamente", StringComparison.OrdinalIgnoreCase) ||
                    l.Contains("Se registro correctamente", StringComparison.OrdinalIgnoreCase) ||
                    l.Contains("Fuera de plazo", StringComparison.OrdinalIgnoreCase) ||
                    l.Contains("Error", StringComparison.OrdinalIgnoreCase) ||
                    l.Contains("Es necesario", StringComparison.OrdinalIgnoreCase) ||
                    l.Contains("invalidar", StringComparison.OrdinalIgnoreCase))
                .Distinct()
                .ToList();

            if (relevantes.Count > 0)
                return string.Join(" | ", relevantes);

            var compacto = string.Join(" ", lineas);
            return compacto.Length <= 180
                ? compacto
                : $"{compacto[..177]}...";
        }

        public void SeleccionarOpcionDesdeAcciones(string opcion)
        {
            if (VistaResultadoDeAccionAbierta(opcion))
                return;

            var locator = LocatorOpcionAccion(opcion);
            IWebElement? item = null;

            // Primera búsqueda con el modal ya abierto
            try
            {
                item = _wait.Until(d => d.FindElements(locator).FirstOrDefault(Visible));
            }
            catch (WebDriverTimeoutException)
            {
                item = null;
            }

            // Si no encontró, espera un poco más sin cerrar el modal
            // (algunas opciones como "Invalidar" cargan después de otras como "Nota de crédito")
            if (item == null)
            {
                try
                {
                    var waitExtra = new WebDriverWait(_driver, TimeSpan.FromSeconds(15))
                    {
                        PollingInterval = TimeSpan.FromMilliseconds(500)
                    };
                    waitExtra.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
                    item = waitExtra.Until(d => d.FindElements(locator).FirstOrDefault(Visible));
                }
                catch (WebDriverTimeoutException)
                {
                    item = null;
                }
            }

            // Último recurso: reabrir modal y esperar
            if (item == null)
            {
                AbrirModalAjustesDeComprobante();
                try
                {
                    item = _wait.Until(d => d.FindElements(locator).FirstOrDefault(Visible));
                }
                catch (WebDriverTimeoutException)
                {
                    item = null;
                }
            }

            if (item == null)
            {
                Assert.Fail(
                    $"No se encontró la opción '{opcion}' en las acciones del comprobante. " +
                    $"{ObtenerDiagnosticoSistema()} " +
                    $"Opciones visibles: {ObtenerOpcionesAccionVisibles()}");
            }

            ScrollTo(item);
            ClickSeguro(item);
            EsperarResultadoDeOpcion(opcion);
            Thread.Sleep(1500);
        }

        private bool VistaResultadoDeAccionAbierta(string opcion)
        {
            string normalizado = opcion.Trim().ToLowerInvariant();

            return normalizado switch
            {
                "invalidar" => _driver.FindElements(VentasLocators.InvalidarVenta.ModalInvalidar).Any(Visible),
                "nota de débito" or "nota de debito" =>
                    _driver.FindElements(VentasLocators.AjusteComprobante.TipoNotaDebitoSelect).Any(Visible),
                "nota de crédito" or "nota de credito" =>
                    _driver.FindElements(By.XPath(
                        "//label[contains(normalize-space(),'Tipo de nota de cr')] | " +
                        "//select[@id='tipoNotaDeCredito'] | " +
                        "//*[contains(@class,'select-trigger')][contains(normalize-space(),'Selecciona un tipo de nota')]"))
                    .Any(Visible),
                "clonar" =>
                    _driver.FindElements(VentasLocators.AjusteComprobante.ModalClonar).Any(Visible),
                _ => false
            };
        }

        private By LocatorOpcionAccion(string opcion)
        {
            string texto = opcion.Trim();
            string normalizado = texto.ToLowerInvariant();
            string mayuscula = texto.ToUpperInvariant();

            if (normalizado is "nota de débito" or "nota de debito")
                return VentasLocators.AjusteComprobante.TabNotaDebito;

            if (normalizado is "nota de crédito" or "nota de credito")
                return VentasLocators.AjusteComprobante.TabNotaCredito;

            if (normalizado == "ver documento")
                return VentasLocators.AjusteComprobante.TabVerDocumento;

            if (normalizado == "invalidar")
                return VentasLocators.AjusteComprobante.TabInvalidar;

            return VentasLocators.AjusteComprobante.OpcionAccionEnModal(texto);
        }

        private void EsperarMenuAccionesVisible()
        {
            _wait.Until(d =>
                d.FindElements(VentasLocators.AjusteComprobante.TabNotaDebito).Any(Visible) ||
                d.FindElements(VentasLocators.AjusteComprobante.TabNotaCredito).Any(Visible) ||
                d.FindElements(VentasLocators.AjusteComprobante.TabVerDocumento).Any(Visible) ||
                d.FindElements(VentasLocators.AjusteComprobante.TabInvalidar).Any(Visible));
        }

        private void AsegurarSeccionPagoDisponible()
        {
            for (int intento = 0; intento < 3; intento++)
            {
                ExpandirSeccion("Pago");
                Thread.Sleep(500);

                bool hayOpciones = _driver.FindElements(VentasLocators.AjusteComprobante.DevolucionContado).Any(Visible)
                    || _driver.FindElements(VentasLocators.AjusteComprobante.DevolucionCredito).Any(Visible)
                    || _driver.FindElements(VentasLocators.AjusteComprobante.PagoContadoRadio).Any(Visible)
                    || _driver.FindElements(VentasLocators.AjusteComprobante.PagoCreditoRadio).Any(Visible)
                    || _driver.FindElements(VentasLocators.AjusteComprobante.MontoInicialInput).Any(Visible);

                if (hayOpciones)
                    return;

                var seccionPago = _driver.FindElements(VentasLocators.AjusteComprobante.SeccionAccordion("Pago"))
                    .FirstOrDefault(Visible);
                if (seccionPago != null)
                {
                    ScrollTo(seccionPago);
                    ClickSeguro(seccionPago);
                    Thread.Sleep(800);
                }
            }

            Console.WriteLine("[Ajuste] La seccion Pago no mostro opciones visibles tras varios intentos.");
        }

        private static bool SeccionEstaExpandida(IWebElement seccion)
        {
            string ariaExpanded = seccion.GetAttribute("aria-expanded") ?? string.Empty;
            if (ariaExpanded.Equals("true", StringComparison.OrdinalIgnoreCase))
                return true;

            if (ariaExpanded.Equals("false", StringComparison.OrdinalIgnoreCase))
                return false;

            string clases = seccion.GetAttribute("class") ?? string.Empty;
            return !clases.Contains("collapsed", StringComparison.OrdinalIgnoreCase);
        }

        private void EsperarResultadoDeOpcion(string opcion)
        {
            string normalizado = opcion.Trim().ToLowerInvariant();

            switch (normalizado)
            {
                case "invalidar":
                    _wait.Until(d => d.FindElements(VentasLocators.InvalidarVenta.ModalInvalidar).Any(Visible));
                    break;
                case "nota de débito":
                case "nota de debito":
                    _wait.Until(d => d.FindElements(VentasLocators.AjusteComprobante.TipoNotaDebitoSelect).Any(Visible));
                    break;
                case "nota de crédito":
                case "nota de credito":
                    _wait.Until(d =>
                        d.FindElements(By.XPath(
                            "//label[contains(normalize-space(),'Tipo de nota de cr')] | " +
                            "//select[@id='tipoNotaDeCredito'] | " +
                            "//*[contains(@class,'select-trigger')][contains(normalize-space(),'Selecciona un tipo de nota')]"))
                        .Any(Visible));
                    break;
                case "clonar":
                    _wait.Until(d => d.FindElements(VentasLocators.AjusteComprobante.ModalClonar).Any(Visible));
                    break;
            }
        }

        private string ObtenerOpcionesAccionVisibles()
        {
            var opciones = _driver.FindElements(
                    By.XPath("//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Ajuste de Comprobante')]]//button[normalize-space()] | " +
                             "//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Ajuste de Comprobante')]]//a[normalize-space()] | " +
                             "//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Ajuste de Comprobante')]]//span[normalize-space()]"))
                .Where(Visible)
                .Select(e => (e.Text ?? string.Empty).Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList();

            return opciones.Any() ? string.Join(" | ", opciones) : "(sin opciones visibles)";
        }

        private void CerrarPopupSiExiste()
        {
            try
            {
                var okBtn = _driver.FindElements(By.CssSelector(".swal2-confirm, .swal2-popup button, button.ok-button"))
                    .FirstOrDefault(Visible)
                    ?? _driver.FindElements(
                    By.XPath("//button[normalize-space()='OK' or normalize-space()='Ok' or normalize-space()='Aceptar']"))
                    .FirstOrDefault(Visible);
                if (okBtn != null)
                {
                    ClickSeguro(okBtn);
                    Thread.Sleep(500);
                    return;
                }

                var cancelBtn = _driver.FindElements(
                    By.XPath("//button[normalize-space()='Cancelar'][ancestor::*[contains(@class,'swal2')]]"))
                    .FirstOrDefault(Visible);
                if (cancelBtn != null)
                {
                    ClickSeguro(cancelBtn);
                    Thread.Sleep(500);
                }
            }
            catch { }
        }

        private By EsperarLocadorFecha(By primario, By alternativo, int timeoutSeconds = 10)
        {
            var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);
            while (DateTime.UtcNow < deadline)
            {
                if (_driver.FindElements(primario).Count > 0)
                    return primario;
                if (_driver.FindElements(alternativo).Count > 0)
                    return alternativo;
                Thread.Sleep(500);
            }
            return alternativo;
        }

        private void IngresarFechaHoraJS(By locator, string valor)
        {
            var el = _driver.FindElement(locator);
            var js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript(
                "var el = arguments[0]; var val = arguments[1];" +
                "el.removeAttribute('readonly');" +
                "var setter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;" +
                "setter.call(el, val);" +
                "el.dispatchEvent(new Event('input', { bubbles: true }));" +
                "el.dispatchEvent(new Event('change', { bubbles: true }));",
                el, valor);
            Thread.Sleep(300);
        }

        private void ScrollTo(IWebElement element)
        {
            ((IJavaScriptExecutor)_driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
            Thread.Sleep(200);
        }

        private void ClickSeguro(IWebElement element)
        {
            try { element.Click(); }
            catch { ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element); }
        }

        private static bool BotonAccionActivo(IWebElement element)
        {
            try
            {
                var classes = element.GetAttribute("class") ?? string.Empty;
                var ariaDisabled = element.GetAttribute("aria-disabled") ?? string.Empty;

                return element.Displayed
                    && element.Enabled
                    && element.GetAttribute("disabled") == null
                    && !ariaDisabled.Equals("true", StringComparison.OrdinalIgnoreCase)
                    && !classes.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static bool Visible(IWebElement e)
        {
            try { return e != null && e.Displayed; }
            catch { return false; }
        }
    }
}
