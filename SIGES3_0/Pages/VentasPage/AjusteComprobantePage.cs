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
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
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
                Assert.Fail($"No se encontró la pestaña '{tab}' en el modal de ajuste.");

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

            var input = _wait.Until(ExpectedConditions.ElementToBeClickable(
                VentasLocators.AjusteComprobante.ImporteDetalleInput));
            ScrollTo(input);
            input.Clear();
            input.SendKeys(importe);
            input.SendKeys(Keys.Tab);
            Thread.Sleep(500);
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
            Thread.Sleep(500);

            var input = _wait.Until(ExpectedConditions.ElementToBeClickable(
                VentasLocators.AjusteComprobante.CantidadDevolverInput));
            input.Clear();
            input.SendKeys(cantidad);
            input.SendKeys(Keys.Tab);
            Thread.Sleep(500);
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
            wait.Until(d => d.FindElements(VentasLocators.AjusteComprobante.DetalleAumentoInput).Any());
            Thread.Sleep(400);

            for (int intento = 0; intento < 3; intento++)
            {
                try
                {
                    var input = _driver.FindElements(VentasLocators.AjusteComprobante.DetalleAumentoInput)
                        .FirstOrDefault() ?? throw new Exception("No se encontró el campo para ingresar el aumento del valor.");

                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);
                    Thread.Sleep(300);

                    input = _driver.FindElements(VentasLocators.AjusteComprobante.DetalleAumentoInput)
                        .FirstOrDefault() ?? throw new Exception("No se encontró el campo para ingresar el aumento del valor.");

                    input.Click();
                    input.SendKeys(Keys.Control + "a");
                    input.SendKeys(monto);
                    input.SendKeys(Keys.Tab);
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
                Assert.Fail($"No se encontró la opción de pago '{tipo}'.");

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
                Assert.Fail($"No se encontró la opción de entrega '{tipo}'.");

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
                Assert.Fail($"No se encontró la opción de devolución '{tipo}'.");

            ClickSeguro(el);
            Thread.Sleep(800);
        }

        // ── Guardar ajuste ─────────────────────────────────────────────────
        public void ClickGuardarAjuste()
        {
            IWebElement? btn = _driver.FindElements(VentasLocators.AjusteComprobante.GuardarAjuste)
                .FirstOrDefault(Visible);
            btn ??= _driver.FindElements(VentasLocators.AjusteComprobante.GuardarAjusteFallback)
                .FirstOrDefault(Visible);

            if (btn == null)
                Assert.Fail("No se encontró el botón Guardar en el modal de ajuste.");

            ScrollTo(btn);
            ClickSeguro(btn);
            Thread.Sleep(3000);
        }

        // ── Verificaciones ─────────────────────────────────────────────────
        public void VerificarAjusteExitoso()
        {
            var waitCorto = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
            try
            {
                waitCorto.Until(d =>
                    d.FindElements(VentasLocators.AjusteComprobante.MensajeExito).Any(Visible) ||
                    !string.IsNullOrWhiteSpace(ObtenerTextoModalResultado()) ||
                    !d.FindElements(By.XPath("//div[contains(normalize-space(),'Ajuste de Comprobante')]")).Any(Visible));
            }
            catch (WebDriverTimeoutException) { }

            var mensajeExito = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeExito)
                .FirstOrDefault(Visible);
            bool hayExito = mensajeExito != null;
            string textoExito = ResumirTextoResultado(mensajeExito?.Text);

            var modalAjuste = _driver.FindElements(By.XPath("//div[contains(normalize-space(),'Ajuste de Comprobante')]"))
                .FirstOrDefault(Visible);
            bool modalCerrado = modalAjuste == null;
            string textoModalResultado = ObtenerTextoModalResultado();

            Console.WriteLine(
                $"[Ajuste][Exito] hayExito={hayExito}, modalCerrado={modalCerrado}, mensaje='{textoExito}', modalResultado='{textoModalResultado}'.");
            if (hayExito || modalCerrado)
                Console.WriteLine("[Ajuste][ResultadoFinal] Ajuste con evidencia de éxito detectada.");

            if (hayExito || modalCerrado)
            {
                CerrarPopupSiExiste();
                return;
            }

            if (!string.IsNullOrWhiteSpace(textoModalResultado))
                Assert.Fail($"No se genero el comprobante de ajuste. Modal de resultado: '{textoModalResultado}'.");

            Assert.Fail("No se genero el comprobante de ajuste. No se detecto modal, toast ni alerta de resultado; el modal de ajuste siguio abierto.");

            Assert.IsTrue(hayExito || modalCerrado,
                "No se detectó confirmación de éxito ni cierre del modal de ajuste.");

            CerrarPopupSiExiste();
        }

        public void VerificarBloqueoGuardar()
        {
            Thread.Sleep(1000);

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

            bool hayEvidenciaDeBloqueo = hayMensajeCampos || hayMensajeError || botonDeshabilitado;
            if (hayEvidenciaDeBloqueo)
                Console.WriteLine("[Ajuste][ResultadoFinal] El sistema bloqueó el guardado con evidencia visible.");

            Assert.IsTrue(
                hayEvidenciaDeBloqueo,
                "Se esperaba que el sistema bloqueara el guardado con evidencia visible " +
                "(mensaje de validación/error o botón Guardar deshabilitado), pero no se detectó ninguna.");
        }

        public void VerificarMensajeMontoMayor()
        {
            Thread.Sleep(1000);
            bool hay = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeMontoMayor)
                .Any(Visible);
            Assert.IsTrue(hay,
                "Se esperaba el mensaje 'Es necesario que el monto de nota sea menor al total.'");
        }

        public void VerificarMensajeCantidadMayor()
        {
            Thread.Sleep(1000);
            bool hay = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeCantidadMayor)
                .Any(Visible);
            Assert.IsTrue(hay,
                "Se esperaba el mensaje 'Es necesario que la cantidad a devolver sea menor a la cantidad entregada.'");
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
                Assert.Fail("No se encontró el botón Invalidar habilitado en el modal.");

            ScrollTo(btn!);
            _wait.Until(_ => BotonAccionActivo(btn!));
            ClickSeguro(btn!);

            var btnConfirmar = _driver.FindElements(VentasLocators.ViewSales.AcceptInvalidation)
                .FirstOrDefault(BotonAccionActivo);
            if (btnConfirmar != null)
            {
                ScrollTo(btnConfirmar);
                ClickSeguro(btnConfirmar);
            }

            Thread.Sleep(3000);
        }

        // ── Verificar invalidación exitosa ──────────────────────────────────
        public void IntentarInvalidarEnModal()
        {
            _wait.Until(d => d.FindElements(VentasLocators.InvalidarVenta.ModalInvalidar).Any(Visible));

            var btn = _driver.FindElements(VentasLocators.InvalidarVenta.BotonInvalidar)
                .FirstOrDefault(Visible);

            if (btn == null)
                Assert.Fail("No se encontró el botón Invalidar en el modal.");

            if (!BotonAccionActivo(btn))
            {
                Console.WriteLine("Botón Invalidar no activo; se omite el click final.");
                return;
            }

            ScrollTo(btn);
            ClickSeguro(btn);

            var btnConfirmar = _driver.FindElements(VentasLocators.ViewSales.AcceptInvalidation)
                .FirstOrDefault(BotonAccionActivo);
            if (btnConfirmar != null)
            {
                ScrollTo(btnConfirmar);
                ClickSeguro(btnConfirmar);
            }

            Thread.Sleep(3000);
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
            var mensajeError = _driver.FindElements(VentasLocators.AjusteComprobante.MensajeError)
                .FirstOrDefault(Visible)?.Text?.Trim();
            string textoExito = ResumirTextoResultado(mensajeExito?.Text);

            Console.WriteLine(
                $"[Invalidar][Resultado] hayExito={hayExito}, mensajeExito='{textoExito}', " +
                $"mensajeError='{mensajeError ?? string.Empty}'.");

            if (!hayExito && !string.IsNullOrWhiteSpace(mensajeError))
                Assert.Fail($"No se detectÃ³ mensaje de confirmaciÃ³n de invalidaciÃ³n exitosa. Error visible: '{mensajeError}'.");

            Assert.IsTrue(hayExito,
                "No se detectó mensaje de confirmación de invalidación exitosa (toast-success o swal2-success).");

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
                "Se esperaba que la sección Entrega no se muestre en la invalidación fuera de plazo.");
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
                "Se esperaba el mensaje 'Fuera de plazo (usar Nota de Crédito)'.");
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
                "Se esperaba que el botón Invalidar estuviera deshabilitado o que el sistema mostrara un mensaje de campos requeridos.");
        }

        // ── Helpers privados ───────────────────────────────────────────────
        public void VerificarOpcionDelModalAjustesNoVisible(string opcion)
        {
            Thread.Sleep(500);

            var locator = LocatorOpcionAccion(opcion);
            bool visible = _driver.FindElements(locator).Any(Visible);

            Console.WriteLine($"[Ajuste][Opciones] opcion='{opcion}', visible={visible}.");

            Assert.IsFalse(visible,
                $"No se esperaba ver la opción '{opcion}' en el modal ajustes de comprobante.");
        }

        public void VerificarModalClonarVisible()
        {
            bool visible = _driver.FindElements(VentasLocators.AjusteComprobante.ModalClonar)
                .Any(Visible);

            Assert.IsTrue(visible, "Se esperaba visualizar el modal 'Clonar venta'.");
        }

        public void SeleccionarPestanaModoClonar(string modo)
        {
            var tab = _wait.Until(d =>
                d.FindElements(VentasLocators.AjusteComprobante.PestanaModoClonar(modo))
                    .FirstOrDefault(Visible));

            if (tab == null)
                Assert.Fail($"No se encontro la pestaña '{modo}' en el modal Clonar venta.");

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
                Assert.Fail($"No se encontro la entrega '{tipo}' en el modal Clonar venta.");

            ScrollTo(opcion);
            ClickSeguro(opcion);
            Thread.Sleep(800);
        }

        public void ClickClonarVenta()
        {
            var boton = _wait.Until(d =>
                d.FindElements(VentasLocators.AjusteComprobante.BotonClonarVenta)
                    .FirstOrDefault(Visible));

            if (boton == null)
                Assert.Fail("No se encontro el boton 'Clonar venta' en el modal.");

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
                "No se detecto confirmacion de exito ni cierre del modal Clonar venta.");
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
                   texto.Contains("monto de nota", StringComparison.OrdinalIgnoreCase);
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
                    l.Contains("Es necesario", StringComparison.OrdinalIgnoreCase))
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

            try
            {
                item = _wait.Until(d => d.FindElements(locator).FirstOrDefault(Visible));
            }
            catch (WebDriverTimeoutException)
            {
                item = null;
            }

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
                var okBtn = _driver.FindElements(
                    By.XPath("//button[normalize-space()='OK' or contains(@class,'ok-button')]"))
                    .FirstOrDefault(Visible);
                if (okBtn != null)
                {
                    ClickSeguro(okBtn);
                    Thread.Sleep(500);
                }

                var cancelBtn = _driver.FindElements(
                    By.XPath("//button[normalize-space()='Cancelar']"))
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
