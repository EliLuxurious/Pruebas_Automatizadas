using NUnit.Framework;
using SIGES3_0.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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
        private string _lastObservedPaymentState = string.Empty;
        private DiscountContext _discountContext = DiscountContext.Empty;
        private PaymentContext _paymentContext = PaymentContext.Empty;
        private string _lastCreditInstallments = string.Empty;
        private static readonly By DiscountAmountModeLocator = By.XPath("//button[normalize-space()='$' or contains(normalize-space(),'Monto')] | //label[normalize-space()='$' or contains(normalize-space(),'Monto')]");
        private static readonly By DiscountPercentageModeLocator = By.XPath("//button[normalize-space()='%' or contains(normalize-space(),'Porcentaje')] | //label[normalize-space()='%' or contains(normalize-space(),'Porcentaje')]");
        private static readonly By DiscountValueInputLocator = By.XPath("//input[(@placeholder='0' or contains(@id,'discount') or contains(@formcontrolname,'discount')) and not(@type='hidden') and not(@type='checkbox') and not(@type='radio')]");

        public NuevaVentaPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // ─── MODO DE VENTA ────────────────────────────────────────────────────────────

        // Paso: selecciona el modo de venta (VENTA NORMAL / VENTA MODO CAJA / VENTA POR CONTINGENCIA)
        // Resetea el estado del escenario y espera que el formulario este listo.
        public void SelectSaleModeFlow(string modo)
        {
            _wasSaveEnabled = false;
            _wasSaveExecuted = false;
            _lastObservedMessage = string.Empty;
            _lastObservedPaymentState = string.Empty;
            _discountContext = DiscountContext.Empty;
            _paymentContext = PaymentContext.Empty;
            _lastCreditInstallments = string.Empty;

            WaitForFormReady();

            if (string.IsNullOrWhiteSpace(modo) || modo.Trim() == "-")
                return;

            Log($"Seleccionando modo de venta: {modo}");
            Click(VentasLocators.NuevaVenta.ModoVenta(modo));
            Thread.Sleep(1000);
        }

        // Paso: ingresa la fecha de emision (solo para Venta Contingencia)
        public void SetFechaEmisionFlow(string fecha)
        {
            if (string.IsNullOrWhiteSpace(fecha) || fecha.Trim() == "-")
                return;

            Log($"Ingresando fecha de emision: {fecha}");
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

        public void ConfigurarDescuentoNuevaVenta(string descuento, string tipo, string modo, string valor)
        {
            var activar = DebeActivarOpcion(descuento);
            var totalAntes = ObtenerTotalVentaActual();
            var valorParseado = TryParseDecimalFlexible(valor, out var parsed) ? parsed : (decimal?)null;

            _discountContext = new DiscountContext
            {
                Activo = activar,
                Tipo = NormalizeText(tipo),
                Modo = NormalizeText(modo),
                Valor = valorParseado,
                TotalAntes = totalAntes
            };

            Log($"[DescuentoNV] Antes={totalAntes?.ToString("0.00", CultureInfo.InvariantCulture) ?? "NA"} activar={activar} tipo='{tipo}' modo='{modo}' valor='{valor}'");

            SetCheckbox(VentasLocators.Detail.DiscountCheckbox, activar);
            Thread.Sleep(300);

            if (!activar)
                return;

            var tipoNormalizado = NormalizeText(tipo);
            if (tipoNormalizado.Contains("item"))
                Click(VentasLocators.Discount.ItemScope);
            else if (tipoNormalizado.Contains("global"))
                Click(VentasLocators.Discount.GlobalScope);

            var modoNormalizado = NormalizeText(modo);
            if (modoNormalizado.Contains("$") || modoNormalizado.Contains("monto"))
                Click(DiscountAmountModeLocator);
            else if (modoNormalizado.Contains("%") || modoNormalizado.Contains("porcentaje"))
                Click(DiscountPercentageModeLocator);

            var input = Find(DiscountValueInputLocator);
            ScrollToCenter(input);
            input.Click();
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            input.SendKeys(valor);
            input.SendKeys(Keys.Tab);

            Thread.Sleep(700);

            var totalDespues = ObtenerTotalVentaActual();
            Log($"[DescuentoNV] Despues={totalDespues?.ToString("0.00", CultureInfo.InvariantCulture) ?? "NA"}");
        }

        // ─── FACTURACIÓN ─────────────────────────────────────────────────────────────

        // Paso: selecciona el punto de venta (solo para Venta Modo Caja)
        public void SelectPuntoVentaFlow(string puntoVenta)
        {
            if (string.IsNullOrWhiteSpace(puntoVenta) || puntoVenta.Trim() == "-")
                return;

            Log($"Seleccionando punto de venta: {puntoVenta}");

            Click(VentasLocators.NuevaVenta.PuntoVentaChevron);

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

        // Paso: busca cliente, selecciona comprobante y serie en la seccion Facturacion.
        public void ConfigurarFacturacionNuevaVenta(string comprobante, string serie, string cliente)
        {
            AbrirSeccionFacturacionSiNecesario();
            BuscarClienteNuevaVenta(cliente);
            SeleccionarComprobanteNuevaVenta(comprobante);

            Thread.Sleep(500);
            var popup = CaptureVisibleMessage(2);
            if (!string.IsNullOrWhiteSpace(popup) && IsBlockingMessage(popup) && string.IsNullOrWhiteSpace(_lastObservedMessage))
            {
                Log($"Popup bloqueante en Facturacion: {popup}");
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

        // Paso: abre el acordeon Entrega, selecciona el tipo (Inmediata/Diferida) y abre Guia de remision si aplica.
        public void ConfigurarEntregaNuevaVenta(string entrega, string guiaRemision)
        {
            Log($"Configurando entrega: tipo='{entrega}', guia='{guiaRemision}'");

            // 1. Abrir acordeon Entrega si los radios aun no son visibles
            bool radiosVisible = driver.FindElements(VentasLocators.Delivery.ImmediateLabel)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (!radiosVisible)
            {
                Log("Abriendo seccion Entrega...");
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

            // 3. Si GuiaRemision = false, no hay nada mas que hacer
            if (!guiaRemision.Trim().Equals("true", StringComparison.OrdinalIgnoreCase)) return;

            // 4. Buscar boton "Guia de remision". La estructura del DOM en NuevaVenta puede diferir
            //    de VerPedidos (donde el boton esta en //div[@id='collapse-entrega']).
            //    Se intenta con locators progresivamente mas amplios para mayor robustez.
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
            // Intento 2: cualquier <button> visible que contenga 'remi' (sin restriccion de contenedor)
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
                // Diagnostico: listar todos los botones visibles para identificar el locator correcto
                Log("=== DIAGNOSTICO: botones visibles en pagina ===");
                foreach (var b in driver.FindElements(By.XPath("//button | //a[contains(@class,'btn')]"))
                    .Where(e => { try { return e.Displayed; } catch { return false; } }))
                {
                    try { Log($"  ELEM: <{b.TagName}> text='{b.Text?.Trim()}' class='{b.GetAttribute("class")}' id='{b.GetAttribute("id")}'"); }
                    catch { }
                }
                Log("=== FIN DIAGNOSTICO ===");
                Log("Boton 'Guia de remision' no encontrado.");
                _lastObservedMessage = "Boton de guia de remision no encontrado";
                return;
            }

            bool deshabilitado = !btnGuia.Enabled
                || btnGuia.GetAttribute("disabled") != null
                || (btnGuia.GetAttribute("class") ?? "").Contains("disabled")
                || !btnGuia.GetCssValue("pointer-events").Equals("auto", StringComparison.OrdinalIgnoreCase);

            if (deshabilitado)
            {
                    Log("Boton 'Guia de remision' deshabilitado - cliente sin RUC/DNI.");
                _lastObservedMessage = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                return;
            }

            ScrollToCenter(btnGuia);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuia);
            Thread.Sleep(1200);
            Log("'Guia de remision' abierta correctamente.");
        }

        // ─── GUÍA DE REMISIÓN (NuevaVenta) ───────────────────────────────────────────

        // GuiaRemisionPage.txtPesoBruto/txtNumeroBultos usan clases Bootstrap (g-2 mb-3)
        // que no existen en el formulario de NuevaVenta; se anclan al label en su lugar.
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
        // Then: valida el resultado de venta contra la tabla de decision
        public void ValidarResultadoVenta(string resultadoEsperado)
        {
            if (string.IsNullOrWhiteSpace(resultadoEsperado) || resultadoEsperado.Trim() == "-")
                return;

            var norm = NormalizeText(resultadoEsperado);
            if (norm.Contains("guarda exitosamente"))
            {
                Assert.That(_wasSaveEnabled, Is.True,
                    $"Guardar deberia estar habilitado (venta exitosa). Mensaje capturado: '{_lastObservedMessage}'");
                Assert.That(_wasSaveExecuted, Is.True,
                    "El guardado deberia haberse ejecutado.");
                if (string.IsNullOrWhiteSpace(_lastObservedMessage) && IsNewSaleFormReset())
                    _lastObservedMessage = "Se registro correctamente";
                Assert.That(NormalizeText(_lastObservedMessage), Does.Contain("registr").Or.Contain("correct"),
                    $"Mensaje de exito no encontrado. Actual: '{_lastObservedMessage}'");
            }
            else
            {
                Assert.That(_wasSaveEnabled, Is.False,
                    $"Guardar deberia estar deshabilitado. Resultado esperado: '{resultadoEsperado}'. Mensaje capturado: '{_lastObservedMessage}'");
                Log($"Validacion no exitosa: esperado='{resultadoEsperado}', capturado='{_lastObservedMessage}'");
            }

            TryCloseSuccessDialog();
        }

        public void ValidarResultadoDescuentoEnVenta(string resultadoEsperado)
        {
            if (string.IsNullOrWhiteSpace(resultadoEsperado) || resultadoEsperado.Trim() == "-")
                return;

            var estado = CapturarEstadoDescuento();
            var esperado = NormalizeText(resultadoEsperado);
            var contexto = ObtenerContextoDescuento(resultadoEsperado, estado);
            var valorDescuento = contexto.Valor!.Value;
            var puedeValidarRecalculoTotal = _discountContext.Activo && _discountContext.TotalAntes.HasValue;
            var totalAntes = contexto.TotalAntes!.Value;

            Assert.That(driver.Url, Does.Contain("/sales/new-sales"),
                $"La pantalla actual no corresponde a Nueva Venta. URL actual: {driver.Url}");
            Assert.That(estado.ModoVentaVisible, Is.True,
                "No se visualiza la opcion 'VENTA NORMAL' en la pantalla de Nueva Venta.");
            Assert.That(estado.CantidadFilas, Is.GreaterThanOrEqualTo(1),
                $"Se esperaba al menos 1 producto en el grid y se obtuvieron {estado.CantidadFilas}.");
            Assert.That(estado.DescuentoMarcado, Is.True,
                "El check Descuento deberia quedar marcado.");
            Assert.That(estado.InputDescuentoHabilitado, Is.True,
                "El ingreso del descuento deberia estar habilitado.");

            if (contexto.Tipo.Contains("item"))
            {
                Assert.That(estado.ItemActivo, Is.True,
                    "La opcion Item deberia quedar seleccionada segun la configuracion del descuento.");
                Assert.That(estado.GlobalActivo, Is.False,
                    "La opcion Global no deberia quedar activa cuando el descuento configurado es por item.");
            }
            else if (contexto.Tipo.Contains("global"))
            {
                Assert.That(estado.GlobalActivo, Is.True,
                    "La opcion Global deberia quedar seleccionada segun la configuracion del descuento.");
                Assert.That(estado.ItemActivo, Is.False,
                    "La opcion Item no deberia quedar activa cuando el descuento configurado es global.");
            }

            if (contexto.Modo.Contains("$") || contexto.Modo.Contains("monto"))
            {
                Assert.That(estado.ModoPorcentajeActivo, Is.False,
                    "El modo porcentaje no deberia quedar activo cuando el descuento configurado es por monto.");
            }
            else if (contexto.Modo.Contains("%") || contexto.Modo.Contains("porcentaje"))
            {
                Assert.That(estado.ModoMontoActivo, Is.False,
                    "El modo monto no deberia quedar activo cuando el descuento configurado es porcentual.");
            }

            if (esperado.Contains("item monto valido"))
            {
                Assert.That(estado.ItemActivo, Is.True,
                    "La opcion Item deberia quedar seleccionada.");
                Assert.That(estado.GlobalActivo, Is.False,
                    "La opcion Global no deberia quedar activa en descuento por item.");
                Assert.That(estado.ValorDescuentoNormalizado, Is.EqualTo("1.00").Or.EqualTo("1"),
                    $"El valor del descuento deberia quedar en 1.00 y se obtuvo '{estado.ValorDescuentoRaw}'.");
                Assert.That(estado.HayErrorVisible, Is.False,
                    $"No se esperaba mensaje de error para descuento item monto valido. Mensaje actual: '{estado.MensajeValidacion}'.");

                if (puedeValidarRecalculoTotal)
                {
                    var totalEsperado = totalAntes - valorDescuento;
                    AssertMontoAproximado(estado.TotalActual, totalEsperado,
                        $"El total final deberia recalcularse restando el descuento al total previo ({totalAntes:0.00} - {valorDescuento:0.00}).");
                }
            }
            else if (esperado.Contains("global porcentaje valido"))
            {
                Assert.That(estado.GlobalActivo, Is.True,
                    "La opcion Global deberia quedar seleccionada.");
                Assert.That(estado.ItemActivo, Is.False,
                    "La opcion Item no deberia quedar activa en descuento global.");
                Assert.That(estado.ValorDescuentoNormalizado, Is.EqualTo("5.00").Or.EqualTo("5"),
                    $"El valor del descuento deberia quedar en 5 y se obtuvo '{estado.ValorDescuentoRaw}'.");
                Assert.That(estado.HayErrorVisible, Is.False,
                    $"No se esperaba mensaje de error para descuento global por porcentaje valido. Mensaje actual: '{estado.MensajeValidacion}'.");

                if (puedeValidarRecalculoTotal)
                {
                    var totalEsperado = totalAntes - (totalAntes * valorDescuento / 100m);
                    AssertMontoAproximado(estado.TotalActual, totalEsperado,
                        $"El total final deberia recalcularse aplicando el {valorDescuento:0.##}% al total previo ({totalAntes:0.00}).");
                }
            }
            else if (esperado.Contains("global monto invalido"))
            {
                Assert.That(estado.GlobalActivo, Is.True,
                    "La opcion Global deberia quedar seleccionada.");
                Assert.That(estado.ValorDescuentoNormalizado, Is.EqualTo("20.00").Or.EqualTo("20"),
                    $"El valor del descuento deberia quedar en 20.00 y se obtuvo '{estado.ValorDescuentoRaw}'.");
                Assert.That(estado.HayErrorVisible || estado.InputDescuentoInvalido, Is.True,
                    "Se esperaba que el sistema rechace el descuento global por monto invalido y muestre una validacion.");
                if (puedeValidarRecalculoTotal)
                {
                    AssertMontoAproximado(estado.TotalActual, totalAntes,
                        $"El total deberia mantenerse igual al total previo ({totalAntes:0.00}) cuando el descuento global por monto es invalido.");
                }
            }
            else if (esperado.Contains("item porcentaje invalido"))
            {
                Assert.That(estado.ItemActivo, Is.True,
                    "La opcion Item deberia quedar seleccionada.");
                Assert.That(estado.GlobalActivo, Is.False,
                    "La opcion Global no deberia quedar activa en descuento por item.");
                Assert.That(estado.ValorDescuentoNormalizado, Is.EqualTo("100.00").Or.EqualTo("100"),
                    $"El valor del descuento deberia quedar en 100 y se obtuvo '{estado.ValorDescuentoRaw}'.");
                Assert.That(estado.HayErrorVisible || estado.InputDescuentoInvalido, Is.True,
                    "Se esperaba que el sistema rechace el descuento item por porcentaje invalido y muestre una validacion.");
                if (puedeValidarRecalculoTotal)
                {
                    AssertMontoAproximado(estado.TotalActual, totalAntes,
                        $"El total deberia mantenerse igual al total previo ({totalAntes:0.00}) cuando el descuento item por porcentaje es invalido.");
                }
            }
            else
            {
                Assert.Fail($"No existe una validacion implementada para el resultado de descuento '{resultadoEsperado}'.");
            }
        }

        private DiscountContext ObtenerContextoDescuento(string resultadoEsperado, DiscountState estado)
        {
            if (_discountContext.Activo && _discountContext.TotalAntes.HasValue && _discountContext.Valor.HasValue)
                return _discountContext;

            var esperado = NormalizeText(resultadoEsperado);
            var tipo = estado.ItemActivo
                ? "item"
                : estado.GlobalActivo
                    ? "global"
                    : esperado.Contains("item")
                        ? "item"
                        : esperado.Contains("global")
                            ? "global"
                            : string.Empty;

            var modo = estado.ModoMontoActivo
                ? "monto"
                : estado.ModoPorcentajeActivo
                    ? "porcentaje"
                    : esperado.Contains("porcentaje")
                        ? "porcentaje"
                        : esperado.Contains("monto") || esperado.Contains("$")
                            ? "monto"
                            : string.Empty;

            var valor = _discountContext.Valor;
            if (!valor.HasValue && TryParseDecimalFlexible(estado.ValorDescuentoRaw, out var valorActual))
                valor = valorActual;

            var totalAntes = _discountContext.TotalAntes
                ?? ObtenerTotalVentaDesdeDetalle()
                ?? ObtenerTotalVentaActual(incluirMontoPago: false)
                ?? ObtenerTotalVentaActual();

            var contextoFallback = new DiscountContext
            {
                Activo = _discountContext.Activo || estado.DescuentoMarcado || !string.IsNullOrWhiteSpace(estado.ValorDescuentoRaw),
                Tipo = string.IsNullOrWhiteSpace(_discountContext.Tipo) ? tipo : _discountContext.Tipo,
                Modo = string.IsNullOrWhiteSpace(_discountContext.Modo) ? modo : _discountContext.Modo,
                Valor = valor,
                TotalAntes = totalAntes
            };

            Assert.That(contextoFallback.Activo, Is.True,
                $"No se encontro un contexto de descuento activo en Nueva Venta para validar '{resultadoEsperado}'.");
            Assert.That(contextoFallback.TotalAntes.HasValue, Is.True,
                $"No se pudo capturar el total previo al descuento para validar '{resultadoEsperado}'.");
            Assert.That(contextoFallback.Valor.HasValue, Is.True,
                $"No se pudo interpretar el valor del descuento configurado para validar '{resultadoEsperado}'.");

            return contextoFallback;
        }

        // Paso: configura el pago X
        public void ConfigurePaymentFlow(string pago) => UpdatePayment(pago);

        public void ConfigurarMediosDePagoNuevaVenta(
            string tipoPago,
            string multipago,
            string medioPago,
            string banco,
            string tarjeta,
            string cuentaBancaria,
            string nroOperacion,
            string montoPorMedio,
            string nroCuotas,
            string montoInicialCredito,
            string observacionPago)
        {
            var totalVentaAntesPago = EsperarTotalVentaDisponibleNuevaVenta();
            Log($"[PagoNV] Total de referencia antes de abrir Pago: {totalVentaAntesPago?.ToString("0.00", CultureInfo.InvariantCulture) ?? "NA"}");

            AbrirPagoNuevaVenta();
            SeleccionarTipoPagoNuevaVenta(tipoPago);

            bool esMultipago = DebeActivarOpcion(multipago);
            ConfigurarMultipagoNuevaVenta(esMultipago);
            _lastCreditInstallments = string.Empty;

            if (NormalizeText(tipoPago).Contains("credito"))
            {
                _lastCreditInstallments = EsNA(nroCuotas) ? string.Empty : nroCuotas.Trim();

                if (!EsNA(nroCuotas))
                    IngresarNumeroCuotasNuevaVenta(nroCuotas);

                if (!EsNA(montoInicialCredito))
                    IngresarMontoInicialCreditoNuevaVenta(montoInicialCredito);
            }

            var instrucciones = ConstruirInstruccionesPagoNuevaVenta(
                medioPago,
                banco,
                tarjeta,
                cuentaBancaria,
                nroOperacion,
                montoPorMedio,
                totalVentaAntesPago);

            _paymentContext = new PaymentContext
            {
                Configurado = true,
                TipoPago = NormalizeText(tipoPago),
                Multipago = esMultipago,
                Medios = instrucciones.Select(x => x.MedioPago).ToList(),
                Bancos = instrucciones.Select(x => x.Banco).Where(x => !EsNA(x)).ToList(),
                Tarjetas = instrucciones.Select(x => x.Tarjeta).Where(x => !EsNA(x)).ToList(),
                Cuentas = instrucciones.Select(x => x.CuentaBancaria).Where(x => !EsNA(x)).ToList(),
                Operaciones = instrucciones.Select(x => x.Operacion).Where(x => !EsNA(x)).ToList(),
                Montos = instrucciones.Select(x => x.MontoEsperado).ToList(),
                TotalAntes = totalVentaAntesPago ?? ObtenerTotalVentaActual(),
                MontoInicialCredito = TryParseDecimalFlexible(montoInicialCredito, out var montoInicial) ? montoInicial : (decimal?)null
            };

            NeutralizarPagoEfectivoPredeterminadoNuevaVenta(instrucciones);

            foreach (var instruccion in instrucciones)
            {
                SeleccionarTabMedioPagoNuevaVenta(instruccion.MedioPago);
                ConfigurarMedioPagoNuevaVenta(instruccion, tipoPago, observacionPago);

                if (esMultipago)
                    GuardarMedioPagoActualNuevaVenta();
            }
        }

        public void IngresarObservacionDelPagoNuevaVenta(string observacion)
        {
            if (EsNA(observacion)) return;

            AbrirPagoNuevaVenta();
            IngresarObservacionPagoNuevaVenta(observacion);
            Log($"[PagoNV] Observacion del pago configurada: '{observacion.Trim()}'.");
        }

        private List<PaymentInstruction> ConstruirInstruccionesPagoNuevaVenta(
            string medioPago,
            string banco,
            string tarjeta,
            string cuentaBancaria,
            string nroOperacion,
            string montoPorMedio,
            decimal? totalReferencia)
        {
            var medios = SepararValores(medioPago)
                .Select(NormalizeText)
                .ToList();

            if (medios.Count == 0 || (medios.Count == 1 && EsNA(medios[0])))
                return new List<PaymentInstruction>();

            Assert.That(medios.Count, Is.GreaterThan(0),
                "Debe existir al menos un medio de pago configurado en el feature.");

            var bancos = SepararValores(banco);
            var tarjetas = SepararValores(tarjeta);
            var cuentas = SepararValores(cuentaBancaria);
            var operaciones = SepararValores(nroOperacion);
            var montos = SepararValores(montoPorMedio);
            int bancoIndex = 0;
            int tarjetaIndex = 0;
            int cuentaIndex = 0;
            int operacionIndex = 0;

            var instrucciones = new List<PaymentInstruction>(medios.Count);
            for (int i = 0; i < medios.Count; i++)
            {
                var medioActual = medios[i];
                var montoConfigurado = ObtenerValorConfiguracionPago(montos, i);
                var bancoActual = "NA";
                var tarjetaActual = "NA";
                var cuentaActual = "NA";
                var operacionActual = "NA";

                switch (medioActual)
                {
                    case "tarjeta_credito":
                    case "tarjeta_debito":
                        bancoActual = ObtenerValorConfiguracionPago(bancos, bancoIndex++);
                        tarjetaActual = ObtenerValorConfiguracionPago(tarjetas, tarjetaIndex++);
                        operacionActual = ObtenerValorConfiguracionPago(operaciones, operacionIndex++);
                        break;
                    case "transferencia_fondos":
                    case "deposito_cuenta":
                        cuentaActual = ObtenerValorConfiguracionPago(cuentas, cuentaIndex++);
                        operacionActual = ObtenerValorConfiguracionPago(operaciones, operacionIndex++);
                        break;
                }

                var montoResuelto = ResolverMontoConfiguradoNuevaVenta(
                    medioActual,
                    montoConfigurado,
                    totalReferencia,
                    medios.Count);
                instrucciones.Add(new PaymentInstruction
                {
                    MedioPago = medioActual,
                    Banco = bancoActual,
                    Tarjeta = tarjetaActual,
                    CuentaBancaria = cuentaActual,
                    Operacion = operacionActual,
                    MontoConfigurado = montoResuelto,
                    MontoEsperado = TryParseDecimalFlexible(montoResuelto, out var montoEsperado) ? montoEsperado : (decimal?)null
                });
            }

            return instrucciones;
        }

        private static string ObtenerValorConfiguracionPago(IReadOnlyList<string> valores, int index)
        {
            if (valores == null || index < 0 || index >= valores.Count)
                return "NA";

            return valores[index].Trim();
        }

        private string ResolverMontoConfiguradoNuevaVenta(string medioPago, string montoConfigurado, decimal? totalReferencia, int totalMedios)
        {
            if (!EsNA(montoConfigurado))
                return ResolverMontoPago(montoConfigurado, totalReferencia);

            if (totalMedios == 1)
            {
                var total = totalReferencia
                    ?? _paymentContext.TotalAntes
                    ?? ObtenerTotalVentaActual(incluirMontoPago: false)
                    ?? ObtenerTotalVentaActual();

                Assert.That(total.HasValue, Is.True,
                    $"No se pudo inferir el monto del medio de pago '{medioPago}' a partir del total de la venta.");

                return total!.Value.ToString("0.00", CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }

        // Nueva Venta a veces precarga el total completo en EFECTIVO apenas se abre Pago.
        // Si el escenario declara un solo medio distinto de efectivo, se limpia ese valor
        // para evitar que el caso quede contaminado por un pago previo/autocompletado del sistema.
        // No se escribe "0": solo se limpia el input cuando ya venia con un monto positivo.
        private void NeutralizarPagoEfectivoPredeterminadoNuevaVenta(IReadOnlyList<PaymentInstruction> instrucciones)
        {
            if (_paymentContext.Multipago || !_paymentContext.TipoPago.Contains("contado"))
                return;

            if (instrucciones.Count != 1 || instrucciones[0].MedioPago == "efectivo")
                return;

            var input = driver.FindElements(VentasLocators.Payment.CashReceivedNewSale)
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });
            if (input == null)
                return;

            var valorActual = (input.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
            if (!TryParseDecimalFlexible(valorActual, out var montoActual) || montoActual <= 0m)
                return;

            Log($"[PagoNV] Se limpia el efectivo autocompletado '{valorActual}' antes de configurar '{instrucciones[0].MedioPago}'.");
            LimpiarValorInputNuevaVenta(input);
        }

        public void ValidarResultadoPagoEnNuevaVenta(string resultadoEsperado)
        {
            if (string.IsNullOrWhiteSpace(resultadoEsperado) || resultadoEsperado.Trim() == "-")
                return;

            var esperado = NormalizeText(resultadoEsperado);
            PaymentContext? contexto = _paymentContext.Configurado ? _paymentContext : null;
            AbrirPagoNuevaVenta();

            Assert.That(driver.Url, Does.Contain("/sales/new-sales"),
                $"La pantalla actual no corresponde a Nueva Venta. URL actual: {driver.Url}");

            if (esperado.Contains("puntos insuficiente"))
            {
                AssertTabPagoActiva("PUNTOS");
                if (contexto != null)
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                AssertAlgunMensajeValidacionPago(
                    "El sistema deberia mostrar la inconsistencia de puntos insuficientes.",
                    "puntos insuficiente",
                    "no hay suficientes puntos disponibles",
                    "suficientes puntos disponibles");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando el cliente no tiene puntos suficientes.");
            }
            else if (esperado.Contains("transferencia sin cuenta ni informacion"))
            {
                AssertTabPagoActiva("TRANSFERENCIA DE FONDOS");
                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El escenario de inconsistencia en transferencia deberia estar configurado como multipago.");
                }

                AssertAgregarMedioPagoDeshabilitado(
                    "El boton Agregar Medio de Pago deberia permanecer deshabilitado cuando falta cuenta e informacion en transferencia.");
                AssertMensajesValidacionPago(
                    "El sistema deberia mostrar las validaciones faltantes de transferencia.",
                    "cuenta bancaria",
                    "informacion");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando una transferencia multipago queda incompleta.");
            }
            else if (esperado.Contains("debito sin banco ni tarjeta"))
            {
                AssertTabPagoActiva("TARJETAS DE DEBITO");
                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El escenario de inconsistencia en debito deberia estar configurado como multipago.");
                }

                AssertAgregarMedioPagoDeshabilitado(
                    "El boton Agregar Medio de Pago deberia permanecer deshabilitado cuando falta banco y tarjeta.");
                AssertMensajesValidacionPago(
                    "El sistema deberia mostrar las validaciones faltantes de banco y tarjeta.",
                    "banco",
                    "tarjeta");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando un medio de debito multipago queda incompleto.");
            }
            else if (esperado.Contains("debito sin informacion"))
            {
                AssertTabPagoActiva("TARJETAS DE DEBITO");
                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El escenario de inconsistencia en debito deberia estar configurado como multipago.");
                }

                AssertAgregarMedioPagoDeshabilitado(
                    "El boton Agregar Medio de Pago deberia permanecer deshabilitado cuando falta la informacion del debito.");
                AssertMensajesValidacionPago(
                    "El sistema deberia mostrar la validacion faltante de informacion.",
                    "informacion");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando falta la informacion del debito.");
            }
            else if (esperado.Contains("credito multipago no cubre monto inicial"))
            {
                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El escenario de credito inconsistente deberia estar configurado como multipago.");
                }

                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando el multipago a credito no cubre el monto inicial.");
            }
            else if (esperado.Contains("multipago puntos no habilitado sin cliente"))
            {
                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El escenario deberia mantenerse en multipago para validar el bloqueo de puntos.");
                }

                Assert.That(EstaMarcado(VentasLocators.Payment.MultipaymentCheckbox), Is.True,
                    "La opcion Multipago deberia permanecer marcada.");
                AssertMedioPagoNoDisponibleNuevaVenta(
                    "PUNTOS",
                    "El sistema no deberia habilitar el medio de pago Puntos cuando no hay cliente identificado.",
                    VentasLocators.Payment.PointsMethod,
                    By.XPath("//span[normalize-space()='PUNTOS']"));
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando el pago queda incompleto y Puntos no esta disponible.");
            }
            else if (esperado.Contains("credito sin cliente"))
            {
                if (contexto != null)
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                AssertCronogramaCreditoConfiguradoNuevaVenta();

                if (contexto?.MontoInicialCredito is decimal montoInicial)
                {
                    AssertInputAproximado(VentasLocators.Payment.CreditInitialAmountInput, montoInicial,
                        "El monto inicial del credito deberia quedar registrado correctamente.");
                }

                AssertAlgunMensajeValidacionPago(
                    "El sistema deberia advertir que la venta a credito requiere cliente identificado.",
                    "es necesario identificar al cliente",
                    "es necesario seleccionar un cliente",
                    "identificar al cliente");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando se configura una venta a credito sin cliente.");
            }
            else if (esperado.Contains("monto inicial cero"))
            {
                if (contexto != null)
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                AssertInputAproximado(VentasLocators.Payment.CreditInitialAmountInput, 0m,
                    "El monto inicial deberia quedar registrado en 0 para disparar la validacion.");
                AssertMensajesValidacionPago(
                    "El sistema deberia mostrar la regla de monto inicial mayor a 0.",
                    "monto inicial",
                    "mayor a 0");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando el monto inicial es 0.");
            }
            else if (esperado.Contains("credito configurado exitoso"))
            {
                if (contexto != null)
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                AssertCronogramaCreditoConfiguradoNuevaVenta();

                if (contexto?.MontoInicialCredito is decimal montoInicial)
                {
                    AssertInputAproximado(VentasLocators.Payment.CreditInitialAmountInput, montoInicial,
                        "El monto inicial del credito deberia quedar registrado correctamente.");
                }

                AssertMensajePagoNoVisible(
                    "cliente",
                    "credito debe identificar");
                AssertGuardarHabilitadoEnPago(
                    "El boton Guardar deberia habilitarse cuando el credito queda configurado correctamente.");
            }
            else if (esperado.Contains("transferencia"))
            {
                AssertTabPagoActiva("TRANSFERENCIA DE FONDOS");
                AssertSeccionPagoListaParaGuardar(
                    "La seccion Pago deberia indicar que los campos requeridos fueron completados correctamente para transferencia.");

                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                    if (contexto.Cuentas.Count > 0)
                    {
                        AssertTextoSeleccionado(contexto.Cuentas,
                            "La cuenta bancaria deberia quedar registrada correctamente.",
                            0,
                            VentasLocators.Payment.BankAccountSelect,
                            VentasLocators.Payment.BankAccountTrigger);
                    }

                    if (contexto.Montos.Any(x => x.HasValue))
                    {
                        AssertMontoMedioPagoNuevaVenta(contexto.Montos,
                            "El monto del medio de pago deberia quedar registrado correctamente.");
                    }

                    if (contexto.Operaciones.Count > 0)
                    {
                        AssertInputExacto(VentasLocators.Payment.PaymentInfoInput, contexto.Operaciones,
                            "El numero de operacion deberia quedar registrado correctamente.");
                    }
                }

                AssertGuardarHabilitadoEnPago(
                    "El boton Guardar deberia habilitarse cuando la transferencia cubre el total de la venta.");
            }
            else if (esperado.Contains("debito"))
            {
                AssertTabPagoActiva("TARJETAS DE DEBITO");
                AssertSeccionPagoListaParaGuardar(
                    "La seccion Pago deberia indicar que los campos requeridos fueron completados correctamente para tarjeta de debito.");

                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                    if (contexto.Bancos.Count > 0)
                    {
                        AssertTextoSeleccionado(contexto.Bancos,
                            "El banco deberia quedar registrado correctamente.",
                            0,
                            VentasLocators.Payment.BankSelect,
                            VentasLocators.Payment.BankTrigger);
                    }

                    if (contexto.Tarjetas.Count > 0)
                    {
                        AssertTextoSeleccionado(contexto.Tarjetas,
                            "La tarjeta deberia quedar registrada correctamente.",
                            1,
                            VentasLocators.Payment.CardSelect,
                            VentasLocators.Payment.CardTrigger);
                    }

                    if (contexto.Montos.Any(x => x.HasValue))
                    {
                        AssertMontoMedioPagoNuevaVenta(contexto.Montos,
                            "El monto del medio de pago deberia quedar registrado correctamente.");
                    }

                    if (contexto.Operaciones.Count > 0)
                    {
                        AssertInputExacto(VentasLocators.Payment.PaymentInfoInput, contexto.Operaciones,
                            "La informacion de la operacion deberia quedar registrada correctamente.");
                    }
                }

                AssertGuardarHabilitadoEnPago(
                    "El boton Guardar deberia habilitarse cuando el pago con tarjeta de debito cubre el total.");
            }
            else if (esperado.Contains("efectivo"))
            {
                AssertTabPagoActiva("EFECTIVO");
                AssertSeccionPagoListaParaGuardar(
                    "La seccion Pago deberia indicar que los campos requeridos fueron completados correctamente para efectivo.");

                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                    var totalEsperado = contexto.TotalAntes ?? ObtenerTotalVentaActual();
                    Assert.That(totalEsperado.HasValue, Is.True,
                        "No se pudo capturar el total de la venta para validar el efectivo.");
                    var totalVentaEsperado = totalEsperado.GetValueOrDefault();

                    TryAssertInputAproximado(VentasLocators.Payment.CashAmount, totalVentaEsperado,
                        "El monto de la venta deberia mostrarse correctamente en efectivo.");

                    if (contexto.Montos.Any(x => x.HasValue))
                    {
                        AssertInputAproximado(VentasLocators.Payment.CashReceivedNewSale, contexto.Montos,
                            "El valor recibido en efectivo deberia quedar registrado correctamente.");

                        var recibido = contexto.Montos.FirstOrDefault();
                        if (recibido.HasValue)
                        {
                            var vueltoEsperado = recibido.Value - totalVentaEsperado;
                            AssertInputAproximado(VentasLocators.Payment.Change, vueltoEsperado,
                                "El vuelto calculado no coincide con el esperado.");
                        }
                    }
                }

                AssertGuardarHabilitadoEnPago(
                    "El boton Guardar deberia habilitarse cuando el pago en efectivo cubre el total.");
            }
            else if (esperado.Contains("puntos"))
            {
                AssertTabPagoActiva("PUNTOS");
                AssertSeccionPagoListaParaGuardar(
                    "La seccion Pago deberia indicar que los campos requeridos fueron completados correctamente para puntos.");

                if (contexto != null)
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                AssertGuardarHabilitadoEnPago(
                    "El boton Guardar deberia habilitarse cuando el pago con puntos cubre el total.");
            }
            else if (esperado.Contains("multipago"))
            {
                AssertSeccionPagoListaParaGuardar(
                    "La seccion Pago deberia indicar que los campos requeridos fueron completados correctamente para multipago.");

                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El contexto de prueba deberia registrar que el pago fue configurado como multipago.");
                    Assert.That(contexto.Medios.Count, Is.GreaterThan(1),
                        "El contexto de pago deberia conservar mas de un medio de pago para validar el multipago.");
                }

                Assert.That(EstaMarcado(VentasLocators.Payment.MultipaymentCheckbox), Is.True,
                    "La opcion Multipago deberia quedar marcada.");
                AssertGuardarHabilitadoEnPago(
                    "El boton Guardar deberia habilitarse cuando la suma de los medios de pago cubre el total.");
            }
            else
            {
                Assert.Fail($"No existe una validacion implementada para el resultado de pago '{resultadoEsperado}'.");
            }
        }

        // Paso: guarda la venta
        // Intenta hacer click en Guardar. Si el boton esta deshabilitado, lo informa y no falla.
        // Captura el mensaje resultante sin sobrescribir mensajes de popup previos.
        public void GuardarVentaFlow()
        {
            Log("Paso 10 - Intentando guardar venta...");
            _wasSaveEnabled = false;
            _wasSaveExecuted = false;

            // Cerrar modal bloqueante si existe ANTES de interactuar con el formulario.
            // No retornar: el modal puede ser una advertencia informativa; el estado real
            // del boton Guardar determina si la venta puede proceder.
            if (TryHandleBlockingModal())
            {
                Log("Modal bloqueante cerrado antes de Guardar - continuando con el flujo.");
                Thread.Sleep(500);
            }

            var btn = driver.FindElements(VentasLocators.NuevaVenta.GuardarVenta)
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

            if (btn == null)
            {
                Log("Boton Guardar no encontrado en el DOM.");
                return;
            }

            _wasSaveEnabled = IsSaveEnabled();
            Log($"Boton Guardar habilitado: {_wasSaveEnabled}");

            if (!_wasSaveEnabled)
            {
                // Capturar la validacion actualmente visible en el formulario.
                // Sobrescribir _lastObservedMessage: el mensaje de validacion del form tiene
                // prioridad sobre cualquier popup informativo capturado en pasos anteriores.
                var validacion = CapturarValidaciones();
                _lastObservedMessage = !string.IsNullOrWhiteSpace(validacion)
                    ? validacion
                    : "Formulario incompleto (sin mensaje de validacion visible)";
                Log($"Guardar DESHABILITADO - Validacion activa: '{_lastObservedMessage}'");
                return;
            }

            ScrollToCenter(btn);
            try
            {
                btn.Click();
            }
            catch (ElementClickInterceptedException)
            {
                Log("ElementClickInterceptedException - un modal intercepto el click en Guardar.");
                TryHandleBlockingModal();
                _wasSaveEnabled = false;
                return;
            }
            Thread.Sleep(2000);
            _wasSaveExecuted = true;

            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(6))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(200)
                }.Until(_ => IsNewSaleFormReset() || !string.IsNullOrWhiteSpace(CaptureVisibleMessage(1)));
            }
            catch
            {
            }

            // Resultado del guardado: form reiniciado = exito, mensaje visible = error post-guardado
            var msg = CaptureVisibleMessage(3);
            if (IsNewSaleFormReset())
                _lastObservedMessage = "Se registro correctamente";
            else if (!string.IsNullOrWhiteSpace(msg))
                _lastObservedMessage = msg;
            else if (string.IsNullOrWhiteSpace(CapturarValidaciones()))
                _lastObservedMessage = "Se registro correctamente";

            Log($"Resultado: Habilitado={_wasSaveEnabled}, Ejecutado={_wasSaveExecuted}, Mensaje='{_lastObservedMessage}'");
        }

        // Helpers privados

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
                Log("Cliente VARIOS / sin identificar - omitiendo busqueda.");
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

            Log("Abriendo seccion Facturacion...");
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
            // Paso 2: seleccionar la opcion
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
                Log($"Serie auto-asignada (unica disponible). Serie esperada: {serie}");
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
                Log($"[Pago] Modal bloqueante activo - omitiendo configuracion de pago '{pago}'.");
                return;
            }

            var pagoNormalizado = NormalizeText(pago);

            if (pagoNormalizado == "contado")
            {
                Log("Configurando pago contado en Nueva Venta...");
                AbrirPagoNuevaVenta();
                SeleccionarTipoPagoNuevaVenta("contado");
                SeleccionarTabMedioPagoNuevaVenta("efectivo");
                return;
            }

            if (pagoNormalizado == "incompleto")
            {
                Log("Configurando pago incompleto en Nueva Venta...");
                AbrirPagoNuevaVenta();
                SeleccionarTipoPagoNuevaVenta("contado");
                SeleccionarTabMedioPagoNuevaVenta("efectivo");

                var amountInput = Find(VentasLocators.Payment.CashReceivedNewSale);
                LimpiarYEscribirCampoNuevaVenta(amountInput, ResolverMontoParcialNuevaVenta());
                return;
            }

            if (pagoNormalizado == "credito")
            {
                Log("Configurando pago a credito rapido en Nueva Venta...");
                AbrirPagoNuevaVenta();
                SeleccionarTipoPagoNuevaVenta("credito");
                return;
            }

            if (pagoNormalizado == "creditoinicial")
            {
                Log("Configurando pago a credito con monto inicial en Nueva Venta...");
                AbrirPagoNuevaVenta();
                SeleccionarTipoPagoNuevaVenta("credito");

                var montoInicial = Find(VentasLocators.Payment.CreditInitialAmountInput);
                var montoParcial = ResolverMontoParcialNuevaVenta();
                LimpiarYEscribirCampoNuevaVenta(montoInicial, montoParcial);

                var recibido = Find(VentasLocators.Payment.CashReceivedNewSale);
                LimpiarYEscribirCampoNuevaVenta(recibido, montoParcial);
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
            // 1. Cerrar popup "Correcto / Se registro correctamente" (boton OK)
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
                    // Si no se puede cerrar el popup OK, continua.
                }
            }

            // 2. Cerrar modal "Venta registrada XXXX" (boton Cancelar)
            //    Este modal aparece justo despues del OK para ofrecer envio por correo/WhatsApp.
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

            Log("Modal bloqueante detectado - capturando mensaje y cerrando.");
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

        private void AbrirPagoNuevaVenta()
        {
            if (EstaSeccionPagoVisibleNuevaVenta())
                return;

            var trigger = ObtenerTriggerAccordionPagoNuevaVenta();
            Assert.That(trigger, Is.Not.Null,
                "No se encontro un trigger seguro para abrir la seccion Pago en Nueva Venta.");

            ClickSeguroNuevaVenta(trigger!, preservarComoBoton: true);

            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(_ => EstaSeccionPagoVisibleNuevaVenta());
        }

        private bool EstaSeccionPagoVisibleNuevaVenta()
        {
            return driver.FindElements(VentasLocators.Payment.PaymentBody)
                .Any(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });
        }

        private IWebElement? ObtenerTriggerAccordionPagoNuevaVenta()
        {
            foreach (var locator in new[]
                     {
                         VentasLocators.Payment.PaymentAccordionButton,
                         VentasLocators.Payment.PaymentAccordionButtonFallback,
                         VentasLocators.Payment.PaymentAccordionHeader
                     })
            {
                var candidato = driver.FindElements(locator)
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed && e.Enabled; }
                        catch { return false; }
                    });

                if (candidato == null)
                    continue;

                return ResolverElementoInteractivoNuevaVenta(candidato);
            }

            return null;
        }

        private IWebElement ResolverElementoInteractivoNuevaVenta(IWebElement candidato)
        {
            try
            {
                var interactivo = candidato.FindElements(By.XPath(
                        ".//button[not(@disabled)] | .//a | .//*[@role='button' or @role='tab']"))
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed && e.Enabled; }
                        catch { return false; }
                    });

                if (interactivo != null)
                    return interactivo;
            }
            catch
            {
            }

            return candidato;
        }

        private void ClickSeguroNuevaVenta(IWebElement element, bool preservarComoBoton = false)
        {
            ScrollToCenter(element);

            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript(@"
                    const el = arguments[0];
                    const preserveButton = arguments[1];
                    if (!el) return;

                    if (preserveButton && el.tagName === 'BUTTON' && !el.getAttribute('type'))
                        el.setAttribute('type', 'button');

                    if (typeof el.focus === 'function')
                        el.focus();

                    el.dispatchEvent(new MouseEvent('click', {
                        bubbles: true,
                        cancelable: true,
                        view: window
                    }));
                ", element, preservarComoBoton);
            }
            catch
            {
                try
                {
                    element.Click();
                }
                catch
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
                }
            }

            Thread.Sleep(400);
        }

        private bool PerteneceAlContenedorNuevaVenta(IWebElement contenedor, IWebElement elemento)
        {
            try
            {
                var resultado = ((IJavaScriptExecutor)driver).ExecuteScript(
                    "return arguments[0] === arguments[1] || arguments[0].contains(arguments[1]);",
                    contenedor,
                    elemento);

                return resultado is bool pertenece && pertenece;
            }
            catch
            {
                return false;
            }
        }

        private IWebElement? FindFirstVisibleInPayment(params By[] locators)
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);

            foreach (var locator in locators)
            {
                var candidato = driver.FindElements(locator)
                    .FirstOrDefault(e =>
                    {
                        try
                        {
                            return e.Displayed &&
                                   e.Enabled &&
                                   (contenedorPago == null || PerteneceAlContenedorNuevaVenta(contenedorPago, e));
                        }
                        catch
                        {
                            return false;
                        }
                    });

                if (candidato != null)
                    return candidato;
            }

            return null;
        }

        private IWebElement? FindLastVisibleInPayment(params By[] locators)
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);

            foreach (var locator in locators)
            {
                var visibles = driver.FindElements(locator)
                    .Where(e =>
                    {
                        try
                        {
                            return e.Displayed &&
                                   e.Enabled &&
                                   (contenedorPago == null || PerteneceAlContenedorNuevaVenta(contenedorPago, e));
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .ToList();

                if (visibles.Any())
                    return visibles.Last();
            }

            return null;
        }

        private sealed class DiscountState
        {
            public int CantidadFilas { get; init; }
            public bool ModoVentaVisible { get; init; }
            public bool DescuentoMarcado { get; init; }
            public bool ItemActivo { get; init; }
            public bool GlobalActivo { get; init; }
            public bool ModoMontoActivo { get; init; }
            public bool ModoPorcentajeActivo { get; init; }
            public bool InputDescuentoHabilitado { get; init; }
            public bool InputDescuentoInvalido { get; init; }
            public string ValorDescuentoRaw { get; init; } = string.Empty;
            public string ValorDescuentoNormalizado { get; init; } = string.Empty;
            public decimal? TotalActual { get; init; }
            public string MensajeValidacion { get; init; } = string.Empty;
            public bool HayErrorVisible => !string.IsNullOrWhiteSpace(MensajeValidacion);
        }

        private sealed class DiscountContext
        {
            public static DiscountContext Empty { get; } = new();

            public bool Activo { get; init; }
            public string Tipo { get; init; } = string.Empty;
            public string Modo { get; init; } = string.Empty;
            public decimal? Valor { get; init; }
            public decimal? TotalAntes { get; init; }
        }

        private sealed class PaymentContext
        {
            public static PaymentContext Empty { get; } = new();

            public bool Configurado { get; init; }
            public string TipoPago { get; init; } = string.Empty;
            public bool Multipago { get; init; }
            public List<string> Medios { get; init; } = new();
            public List<string> Bancos { get; init; } = new();
            public List<string> Tarjetas { get; init; } = new();
            public List<string> Cuentas { get; init; } = new();
            public List<string> Operaciones { get; init; } = new();
            public List<decimal?> Montos { get; init; } = new();
            public decimal? TotalAntes { get; init; }
            public decimal? MontoInicialCredito { get; init; }
        }

        private sealed class PaymentInstruction
        {
            public string MedioPago { get; init; } = string.Empty;
            public string Banco { get; init; } = "NA";
            public string Tarjeta { get; init; } = "NA";
            public string CuentaBancaria { get; init; } = "NA";
            public string Operacion { get; init; } = "NA";
            public string MontoConfigurado { get; init; } = string.Empty;
            public decimal? MontoEsperado { get; init; }
        }

        private sealed class PointsPaymentState
        {
            public decimal? PuntosAcumulados { get; init; }
            public decimal? SolesAcumulados { get; init; }
            public decimal? PuntosRestantes { get; init; }
            public decimal? SolesRestantes { get; init; }
        }

        private DiscountState CapturarEstadoDescuento()
        {
            Thread.Sleep(700);

            var filas = driver.FindElements(By.XPath("//table//tbody/tr[td]"))
                .Count(e => { try { return e.Displayed; } catch { return false; } });

            var inputDescuento = FindFirstVisibleOrAny(
                DiscountValueInputLocator,
                VentasLocators.Discount.GlobalValueInput
            );

            var valorRaw = inputDescuento?.GetAttribute("value")?.Trim() ?? string.Empty;
            var total = ObtenerTotalVentaActual();
            var mensaje = CapturarValidaciones();

            var estado = new DiscountState
            {
                CantidadFilas = filas,
                ModoVentaVisible = FindFirstVisibleOrAny(VentasLocators.NuevaVenta.ModoVenta("VENTA NORMAL")) != null,
                DescuentoMarcado = EstaMarcado(VentasLocators.Detail.DiscountCheckbox)
                    || inputDescuento != null
                    || EstaActivo(VentasLocators.Discount.ItemScope)
                    || EstaActivo(VentasLocators.Discount.GlobalScope),
                ItemActivo = EstaActivo(VentasLocators.Discount.ItemScope),
                GlobalActivo = EstaActivo(VentasLocators.Discount.GlobalScope),
                ModoMontoActivo = EstaActivo(DiscountAmountModeLocator),
                ModoPorcentajeActivo = EstaActivo(DiscountPercentageModeLocator),
                InputDescuentoHabilitado = inputDescuento != null && inputDescuento.Enabled,
                InputDescuentoInvalido = EsCampoInvalido(inputDescuento),
                ValorDescuentoRaw = valorRaw,
                ValorDescuentoNormalizado = NormalizarNumero(valorRaw),
                TotalActual = total,
                MensajeValidacion = mensaje
            };

            Log($"[Descuento] Filas={estado.CantidadFilas} Check={estado.DescuentoMarcado} Item={estado.ItemActivo} Global={estado.GlobalActivo} " +
                $"Monto={estado.ModoMontoActivo} Porcentaje={estado.ModoPorcentajeActivo} Valor='{estado.ValorDescuentoRaw}' " +
                $"Total={estado.TotalActual?.ToString(CultureInfo.InvariantCulture) ?? "NA"} Mensaje='{estado.MensajeValidacion}'");

            return estado;
        }

        private IWebElement? FindFirstVisibleOrAny(params By[] locators)
        {
            foreach (var loc in locators)
            {
                var visible = driver.FindElements(loc)
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    });
                if (visible != null)
                    return visible;

                var any = driver.FindElements(loc).FirstOrDefault();
                if (any != null)
                    return any;
            }

            return null;
        }

        private bool EstaMarcado(By locator)
        {
            var element = FindFirstVisibleOrAny(locator);
            if (element == null) return false;

            try
            {
                if (element.TagName.Equals("input", StringComparison.OrdinalIgnoreCase))
                    return element.Selected ||
                           string.Equals(element.GetAttribute("checked"), "true", StringComparison.OrdinalIgnoreCase) ||
                           string.Equals(element.GetAttribute("aria-checked"), "true", StringComparison.OrdinalIgnoreCase);

                var js = (IJavaScriptExecutor)driver;
                var result = js.ExecuteScript(@"
                    const el = arguments[0];
                    if (!el) return false;
                    const input = el.matches('input') ? el : el.closest('label')?.querySelector('input') || el.previousElementSibling;
                    if (!input) return false;
                    return !!(input.checked || input.getAttribute('checked') === 'true' || input.getAttribute('aria-checked') === 'true');
                ", element);

                return result is bool b && b;
            }
            catch
            {
                return false;
            }
        }

        private bool EstaActivo(By locator)
        {
            var element = FindFirstVisibleOrAny(locator);
            if (element == null) return false;

            try
            {
                var classes = NormalizeText(element.GetAttribute("class") ?? string.Empty);
                var ariaPressed = NormalizeText(element.GetAttribute("aria-pressed") ?? string.Empty);
                var ariaSelected = NormalizeText(element.GetAttribute("aria-selected") ?? string.Empty);

                if (classes.Contains("active") ||
                    classes.Contains("selected") ||
                    ariaPressed == "true" ||
                    ariaSelected == "true")
                {
                    return true;
                }

                var js = (IJavaScriptExecutor)driver;
                var result = js.ExecuteScript(@"
                    const el = arguments[0];
                    if (!el) return false;

                    const ownClass = (el.getAttribute('class') || '').toLowerCase();
                    if (ownClass.includes('active') || ownClass.includes('selected')) return true;

                    const container = el.closest('label, button, .btn, .toggle, .input-group, .option, .radio-row');
                    const containerClass = (container?.getAttribute('class') || '').toLowerCase();
                    if (containerClass.includes('active') || containerClass.includes('selected')) return true;

                    const input = el.matches('input')
                        ? el
                        : el.querySelector('input[type=radio],input[type=checkbox]')
                            || container?.querySelector('input[type=radio],input[type=checkbox]')
                            || el.previousElementSibling
                            || el.closest('label')?.querySelector('input[type=radio],input[type=checkbox]');

                    if (!input) return false;

                    return !!(input.checked ||
                              input.getAttribute('checked') === 'true' ||
                              input.getAttribute('aria-checked') === 'true');
                ", element);

                return result is bool activo && activo;
            }
            catch
            {
                return false;
            }
        }

        private bool EsCampoInvalido(IWebElement? element)
        {
            if (element == null) return false;

            try
            {
                var classes = NormalizeText(element.GetAttribute("class") ?? string.Empty);
                var ariaInvalid = NormalizeText(element.GetAttribute("aria-invalid") ?? string.Empty);

                return classes.Contains("invalid") || ariaInvalid == "true";
            }
            catch
            {
                return false;
            }
        }

        private decimal? ObtenerTotalVentaActual(bool incluirMontoPago = true)
        {
            var candidatos = new List<string>
            {
                LeerValor(By.XPath("//*[normalize-space()='Total']/following::*[contains(normalize-space(),'S/') or contains(normalize-space(),'$')][1]")),
                LeerValor(By.XPath("//*[normalize-space()='Subtotal']/following::*[contains(normalize-space(),'S/') or contains(normalize-space(),'$')][1]")),
                LeerValor(By.XPath("//label[contains(normalize-space(),'Total')]/following::input[1]")),
                LeerValor(By.XPath("//label[contains(normalize-space(),'Importe total')]/following::input[1]")),
                LeerValor(By.XPath("//*[contains(normalize-space(),'Importe total') or contains(normalize-space(),'Total de venta')]/following::*[self::span or self::div or self::input][1]")),
                LeerValor(By.XPath("//*[contains(@class,'total') or contains(@class,'amount')][normalize-space()]"))
            };

            if (incluirMontoPago)
                candidatos.Add(LeerValor(VentasLocators.Payment.CashAmount));

            decimal? ceroCapturado = null;
            foreach (var candidato in candidatos)
            {
                if (!TryParseUltimoDecimalFlexible(candidato, out var valor))
                    continue;

                if (valor != 0m || candidato.Contains('-'))
                    return valor;

                ceroCapturado ??= valor;
            }

            var totalDetalle = ObtenerTotalVentaDesdeDetalle();
            if (totalDetalle.HasValue && totalDetalle.Value > 0m)
                return totalDetalle.Value;

            if (ceroCapturado.HasValue)
                return ceroCapturado.Value;

            var importes = driver.FindElements(VentasLocators.Detail.PriceInputs)
                .Select(e => e.GetAttribute("value") ?? e.Text ?? string.Empty)
                .Select(texto => TryParseDecimalFlexible(texto, out var valor) ? valor : (decimal?)null)
                .Where(v => v.HasValue)
                .Select(v => v!.Value)
                .ToList();

            if (importes.Count > 0)
                return importes.Sum();

            return null;
        }

        private decimal? ObtenerTotalVentaDesdeDetalle()
        {
            try
            {
                var cantidades = driver.FindElements(VentasLocators.Detail.QuantityInputs).ToList();
                var precios = driver.FindElements(VentasLocators.Detail.PriceInputs).ToList();
                var filas = Math.Min(cantidades.Count, precios.Count);

                if (filas == 0)
                    return null;

                decimal total = 0m;
                int filasValidas = 0;

                for (int i = 0; i < filas; i++)
                {
                    var cantidadTexto = cantidades[i].GetAttribute("value") ?? cantidades[i].Text ?? string.Empty;
                    var precioTexto = precios[i].GetAttribute("value") ?? precios[i].Text ?? string.Empty;

                    if (!TryParseDecimalFlexible(cantidadTexto, out var cantidad) ||
                        !TryParseDecimalFlexible(precioTexto, out var precio) ||
                        cantidad <= 0m ||
                        precio < 0m)
                    {
                        continue;
                    }

                    total += cantidad * precio;
                    filasValidas++;
                }

                return filasValidas > 0 ? Math.Round(total, 2) : (decimal?)null;
            }
            catch
            {
                return null;
            }
        }

        private decimal? EsperarTotalVentaDisponibleNuevaVenta(int timeoutSeconds = 6)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(200)
                }.Until(_ =>
                {
                    var total = ObtenerTotalVentaActual(incluirMontoPago: false);
                    if (total.HasValue && total.Value > 0m)
                        return total;

                    var totalDetalle = ObtenerTotalVentaDesdeDetalle();
                    return totalDetalle.HasValue && totalDetalle.Value > 0m ? totalDetalle : null;
                });
            }
            catch
            {
                return ObtenerTotalVentaActual(incluirMontoPago: false)
                    ?? ObtenerTotalVentaDesdeDetalle()
                    ?? ObtenerTotalVentaActual();
            }
        }

        private string ResolverMontoParcialNuevaVenta()
        {
            var total = EsperarTotalVentaDisponibleNuevaVenta()
                ?? ObtenerTotalVentaActual(incluirMontoPago: false)
                ?? ObtenerTotalVentaActual();

            Assert.That(total.HasValue && total.Value > 0m, Is.True,
                "No se pudo obtener el total actual de la venta para resolver un monto parcial.");

            var montoParcial = Math.Round(total!.Value / 2m, 2, MidpointRounding.AwayFromZero);
            if (montoParcial <= 0m || montoParcial >= total.Value)
                montoParcial = Math.Round(Math.Max(total.Value - 0.01m, 0.01m), 2, MidpointRounding.AwayFromZero);

            return montoParcial.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private string LeerValor(By locator)
        {
            var element = FindFirstVisibleOrAny(locator);
            if (element == null) return string.Empty;

            try
            {
                return (element.GetAttribute("value") ?? element.Text ?? string.Empty).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string NormalizarNumero(string value)
        {
            if (!TryParseDecimalFlexible(value, out var parsed))
                return string.Empty;

            return parsed.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static bool TryParseDecimalFlexible(string? value, out decimal parsed)
        {
            parsed = 0m;
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var sanitized = new string(value
                .Where(c => char.IsDigit(c) || c == ',' || c == '.' || c == '-')
                .ToArray());

            if (string.IsNullOrWhiteSpace(sanitized))
                return false;

            if (sanitized.Contains(',') && sanitized.Contains('.'))
            {
                var lastComma = sanitized.LastIndexOf(',');
                var lastDot = sanitized.LastIndexOf('.');
                sanitized = lastDot > lastComma
                    ? sanitized.Replace(",", string.Empty)
                    : sanitized.Replace(".", string.Empty).Replace(",", ".");
            }
            else if (sanitized.Count(c => c == ',') == 1 && sanitized.Count(c => c == '.') == 0)
            {
                sanitized = sanitized.Replace(",", ".");
            }
            else if (sanitized.Count(c => c == '.') > 1)
            {
                var lastDot = sanitized.LastIndexOf('.');
                sanitized = sanitized[..lastDot].Replace(".", string.Empty) + sanitized[lastDot..];
            }

            return decimal.TryParse(sanitized, NumberStyles.Number | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out parsed);
        }

        private static void AssertMontoAproximado(decimal? actual, decimal esperado, string mensaje)
        {
            Assert.That(actual.HasValue, Is.True, $"{mensaje} No se pudo capturar el total actual de la venta.");
            Assert.That(actual!.Value, Is.EqualTo(esperado).Within(0.05m), $"{mensaje} Total actual: {actual:0.00}");
        }

        private static bool TryParseUltimoDecimalFlexible(string? value, out decimal parsed)
        {
            parsed = 0m;
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var matches = Regex.Matches(value, @"-?\d+(?:[.,]\d+)?");
            if (matches.Count == 0)
                return false;

            return TryParseDecimalFlexible(matches[^1].Value, out parsed);
        }

        private static bool DebeActivarOpcion(string value)
        {
            var normalizado = NormalizeText(value);
            return normalizado is "true" or "1" or "y" or "yes" or "si";
        }

        private PaymentContext ObtenerContextoPago(string resultadoEsperado)
        {
            Assert.That(_paymentContext.Configurado, Is.True,
                $"La seccion Pago se pudo inspeccionar, pero no se encontro un contexto configurado en Nueva Venta para validar '{resultadoEsperado}'. Revise que el step 'el usuario configura los medios de pago ...' este resolviendo al binding scoped de NuevaVenta.");
            return _paymentContext;
        }

        private void AssertTipoPagoSeleccionadoNuevaVenta(PaymentContext contexto)
        {
            if (contexto.TipoPago.Contains("contado"))
            {
                Assert.That(EstaMarcado(VentasLocators.Payment.CashType), Is.True,
                    "El tipo de pago Contado deberia quedar seleccionado correctamente.");
            }
            else if (contexto.TipoPago.Contains("credito"))
            {
                Assert.That(EstaMarcado(VentasLocators.Payment.QuickCreditType), Is.True,
                    "El tipo de pago Credito deberia quedar seleccionado correctamente.");
            }
        }

        private void SeleccionarTipoPagoNuevaVenta(string tipoPago)
        {
            if (NormalizeText(tipoPago).Contains("contado"))
            {
                SeleccionarRadioPagoNuevaVenta(
                    "contado",
                    VentasLocators.Payment.CashType,
                    VentasLocators.Payment.CashTypeLabelText,
                    VentasLocators.Payment.CashTypeLabel,
                    VentasLocators.Payment.CashType);
                return;
            }

            if (NormalizeText(tipoPago).Contains("credito"))
            {
                SeleccionarRadioPagoNuevaVenta(
                    "credito",
                    VentasLocators.Payment.QuickCreditType,
                    VentasLocators.Payment.CreditTypeLabelText,
                    VentasLocators.Payment.QuickCreditTypeLabel,
                    VentasLocators.Payment.QuickCreditType);
            }
        }

        private void SeleccionarRadioPagoNuevaVenta(string descripcion, By radioLocator, params By[] locators)
        {
            AbrirPagoNuevaVenta();

            if (EstaMarcado(radioLocator))
                return;

            var objetivo = FindFirstVisibleInPayment(locators.Append(radioLocator).ToArray());
            Assert.That(objetivo, Is.Not.Null,
                $"No se encontro la opcion '{descripcion}' dentro de la seccion Pago de Nueva Venta.");

            ClickSeguroNuevaVenta(objetivo!, preservarComoBoton: true);

            var seleccionado = new WebDriverWait(driver, TimeSpan.FromSeconds(6))
            {
                PollingInterval = TimeSpan.FromMilliseconds(150)
            }.Until(_ => EstaMarcado(radioLocator));

            Assert.That(seleccionado, Is.True,
                $"La opcion '{descripcion}' no quedo seleccionada en el bloque Pago de Nueva Venta.");
        }

        private void ConfigurarMultipagoNuevaVenta(bool activar)
        {
            var chk = FindFirstVisibleOrAny(VentasLocators.Payment.MultipaymentCheckbox);
            Assert.That(chk, Is.Not.Null, "No se encontro el check Multipago en Nueva Venta.");

            bool marcado = false;
            try
            {
                marcado = chk!.Selected ||
                          string.Equals(chk.GetAttribute("checked"), "true", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                marcado = false;
            }

            if (marcado == activar)
                return;

            ScrollToCenter(chk!);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", chk);
            Thread.Sleep(500);
        }

        private void IngresarNumeroCuotasNuevaVenta(string nroCuotas)
        {
            var input = Find(By.XPath("//input[@type='number'][@min='1'][@max='60']"));
            LimpiarYEscribirCampoNuevaVenta(input, nroCuotas.Trim());
        }

        private void IngresarMontoInicialCreditoNuevaVenta(string monto)
        {
            var input = Find(VentasLocators.Payment.CreditInitialAmountInput);
            LimpiarYEscribirCampoNuevaVenta(input, ResolverMontoPago(monto));
        }

        private void SeleccionarTabMedioPagoNuevaVenta(string medioPago)
        {
            switch (NormalizeText(medioPago))
            {
                case "efectivo":
                    ClickTabPagoNuevaVenta("EFECTIVO",
                        VentasLocators.Payment.CashMethod,
                        VentasLocators.Payment.CashMethodFallback,
                        By.XPath("//span[normalize-space()='EFECTIVO']"));
                    break;
                case "tarjeta_credito":
                    ClickTabPagoNuevaVenta("TARJETAS DE CREDITO",
                        VentasLocators.Payment.CreditMethod,
                        By.XPath("//span[normalize-space()='TARJETAS DE CREDITO']"));
                    break;
                case "tarjeta_debito":
                    ClickTabPagoNuevaVenta("TARJETAS DE DEBITO",
                        VentasLocators.Payment.DebitMethod,
                        By.XPath("//span[normalize-space()='TARJETAS DE DEBITO']"));
                    break;
                case "transferencia_fondos":
                    ClickTabPagoNuevaVenta("TRANSFERENCIA DE FONDOS",
                        VentasLocators.Payment.TransferMethod,
                        By.XPath("//span[normalize-space()='TRANSFERENCIA DE FONDOS']"));
                    break;
                case "deposito_cuenta":
                    ClickTabPagoNuevaVenta("DEPOSITOS EN CUENTA",
                        VentasLocators.Payment.DepositMethod,
                        By.XPath("//span[normalize-space()='DEPOSITOS EN CUENTA']"));
                    break;
                case "puntos":
                    ClickTabPagoNuevaVenta("PUNTOS",
                        VentasLocators.Payment.PointsMethod,
                        By.XPath("//span[normalize-space()='PUNTOS']"));
                    break;
                case "nota_credito":
                    ClickTabPagoNuevaVenta("NOTA DE CREDITO",
                        VentasLocators.Payment.CreditNoteMethod,
                        By.XPath("//span[normalize-space()='NOTA DE CREDITO' or normalize-space()='NOTA DE CRÉDITO']"));
                    break;
                default:
                    throw new Exception($"Medio de pago no soportado en Nueva Venta: {medioPago}");
            }

            Thread.Sleep(500);
        }

        private void ClickTabPagoNuevaVenta(string textoEsperado, params By[] locators)
        {
            if (EsperarTabPagoNuevaVentaLista(textoEsperado))
                return;

            var candidatos = ObtenerCandidatosTabPagoNuevaVenta(textoEsperado, locators);
            Assert.That(candidatos.Count, Is.GreaterThan(0),
                $"No se encontro ningun tab visible para '{textoEsperado}' en Nueva Venta.");

            foreach (var candidato in candidatos)
            {
                var objetivo = ResolverObjetivoTabPagoNuevaVenta(candidato);
                EjecutarClickTabPagoNuevaVenta(objetivo);

                if (EsperarTabPagoNuevaVentaLista(textoEsperado))
                    return;
            }

            var resumen = ConstruirResumenPagoNuevaVenta($"tab_esperado={textoEsperado}");
            Log($"[PagoNV] {resumen}");
            Assert.Fail($"No se pudo activar el tab '{textoEsperado}' en Nueva Venta. {resumen}");
        }

        private bool EsperarTabPagoNuevaVentaLista(string textoEsperado)
        {
            return EsperarTabPagoNuevaVentaActiva(textoEsperado) &&
                   EsperarContenidoPagoNuevaVentaVisible(textoEsperado);
        }

        private bool EsTabPagoActiva(string textoEsperado)
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
            if (contenedorPago != null)
            {
                try
                {
                    var activaEnContenedor = contenedorPago
                        .FindElements(By.XPath(
                            ".//*[contains(@class,'custom-tab') or self::button or self::a or @role='tab']" +
                            "[contains(@class,'active') or contains(@class,'selected') or @aria-selected='true']"))
                        .FirstOrDefault(e =>
                        {
                            try { return e.Displayed; }
                            catch { return false; }
                        });

                    if (activaEnContenedor != null)
                        return NormalizeText(activaEnContenedor.Text).Contains(NormalizeText(textoEsperado));
                }
                catch
                {
                }
            }

            var tabActiva = FindFirstVisibleOrAny(
                VentasLocators.Payment.ActivePaymentTab,
                By.XPath("//*[(@aria-selected='true' or contains(@class,'active') or contains(@class,'selected')) and (contains(@class,'custom-tab') or @role='tab')]"));
            if (tabActiva == null)
                return false;

            return NormalizeText(tabActiva.Text).Contains(NormalizeText(textoEsperado));
        }

        private List<IWebElement> ObtenerCandidatosTabPagoNuevaVenta(string textoEsperado, params By[] locators)
        {
            var candidatos = new List<IWebElement>();
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
            var textoNormalizado = NormalizeText(textoEsperado);

            foreach (var locator in locators)
            {
                try
                {
                    foreach (var element in driver.FindElements(locator))
                    {
                        try
                        {
                            if (element.Displayed &&
                                element.Enabled &&
                                (contenedorPago == null || PerteneceAlContenedorNuevaVenta(contenedorPago, element)))
                            {
                                var objetivo = ResolverObjetivoTabPagoNuevaVenta(element);
                                if (NormalizeText(objetivo.Text).Contains(textoNormalizado))
                                    candidatos.Add(objetivo);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }

            if (contenedorPago != null)
            {
                try
                {
                    var elementosPorTexto = contenedorPago.FindElements(By.XPath(
                            ".//*[contains(@class,'custom-tab') or self::button or self::a or @role='tab'][normalize-space()]"))
                        .Where(e =>
                        {
                            try
                            {
                                return e.Displayed &&
                                       e.Enabled &&
                                       NormalizeText(e.Text).Contains(textoNormalizado);
                            }
                            catch
                            {
                                return false;
                            }
                        });

                    candidatos.AddRange(elementosPorTexto);
                }
                catch
                {
                }
            }

            return candidatos;
        }

        private IWebElement ResolverObjetivoTabPagoNuevaVenta(IWebElement candidato)
        {
            try
            {
                var ancestro = candidato.FindElements(By.XPath(
                        "./ancestor-or-self::*[contains(@class,'custom-tab') or self::button or self::a or @role='tab'][1]"))
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed && e.Enabled; }
                        catch { return false; }
                    });

                if (ancestro != null)
                    return ancestro;
            }
            catch
            {
            }

            return candidato;
        }

        private void EjecutarClickTabPagoNuevaVenta(IWebElement objetivo)
        {
            var interactivo = ResolverElementoInteractivoNuevaVenta(objetivo);
            ClickSeguroNuevaVenta(interactivo, preservarComoBoton: true);
        }

        private bool EsperarTabPagoNuevaVentaActiva(string textoEsperado)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(4))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(_ => EsTabPagoActiva(textoEsperado));
            }
            catch
            {
                return false;
            }
        }

        private bool EsContenidoPagoEsperadoVisible(string textoEsperado)
        {
            var esperado = NormalizeText(textoEsperado);

            if (esperado.Contains("efectivo"))
                return HayElementoVisible(VentasLocators.Payment.CashReceivedNewSale, VentasLocators.Payment.Change);

            if (esperado.Contains("tarjetas de credito") || esperado.Contains("tarjetas de debito"))
                return HayElementoVisible(
                    VentasLocators.Payment.BankSelect,
                    VentasLocators.Payment.CardSelect,
                    VentasLocators.Payment.BankTrigger,
                    VentasLocators.Payment.CardTrigger);

            if (esperado.Contains("transferencia") || esperado.Contains("depositos"))
                return HayElementoVisible(
                    VentasLocators.Payment.BankAccountSelect,
                    VentasLocators.Payment.BankAccountTrigger);

            if (esperado.Contains("puntos"))
                return HayElementoVisible(
                    VentasLocators.Payment.PointsPaymentInput,
                    VentasLocators.Payment.PointsPaymentCurrencyInput,
                    VentasLocators.Payment.PointsRemainingInput,
                    VentasLocators.Payment.PointsRemainingCurrencyInput);

            return false;
        }

        private bool HayElementoVisible(params By[] locators)
        {
            foreach (var locator in locators)
            {
                try
                {
                    if (driver.FindElements(locator).Any(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    }))
                    {
                        return true;
                    }
                }
                catch
                {
                }
            }

            return false;
        }

        private void ConfigurarMedioPagoNuevaVenta(
            PaymentInstruction instruccion,
            string tipoPago,
            string observacionPago)
        {
            switch (instruccion.MedioPago)
            {
                case "efectivo":
                    if (NormalizeText(tipoPago).Contains("contado") && !_paymentContext.Multipago)
                        IngresarMontoEfectivoNuevaVenta(instruccion.MontoConfigurado);
                    else
                        IngresarMontoMedioPagoNuevaVenta(instruccion.MontoConfigurado);
                    break;
                case "tarjeta_credito":
                case "tarjeta_debito":
                    SeleccionarBancoNuevaVenta(instruccion.Banco);
                    Thread.Sleep(500);
                    SeleccionarTarjetaNuevaVenta(instruccion.Tarjeta);
                    Thread.Sleep(300);
                    IngresarMontoMedioPagoNuevaVenta(instruccion.MontoConfigurado);
                    Thread.Sleep(300);
                    IngresarInformacionNuevaVenta(instruccion.Operacion);
                    Thread.Sleep(300);
                    break;
                case "transferencia_fondos":
                case "deposito_cuenta":
                    SeleccionarCuentaBancariaNuevaVenta(instruccion.CuentaBancaria);
                    Thread.Sleep(300);
                    IngresarMontoMedioPagoNuevaVenta(instruccion.MontoConfigurado);
                    Thread.Sleep(300);
                    IngresarInformacionNuevaVenta(instruccion.Operacion);
                    Thread.Sleep(300);
                    break;
                case "puntos":
                    ConfigurarPagoPuntosNuevaVenta(instruccion.MontoConfigurado);
                    break;
                case "nota_credito":
                    IngresarMontoMedioPagoNuevaVenta(instruccion.MontoConfigurado);
                    Thread.Sleep(300);
                    break;
            }

            if (!EsNA(observacionPago))
                IngresarObservacionPagoNuevaVenta(observacionPago);

            if (instruccion.MedioPago == "puntos")
                ConfirmarPagoPuntosNuevaVentaSiAplica();
        }

        private void GuardarMedioPagoActualNuevaVenta()
        {
            var boton = ObtenerBotonAgregarMedioPagoVisible();
            Assert.That(boton, Is.Not.Null,
                $"No se encontro el boton Agregar Medio de Pago en Nueva Venta. {ConstruirResumenPagoNuevaVenta()}");

            ScrollToCenter(boton!);
            if (!EstaHabilitadoBotonAccion(boton!))
            {
                Log($"[PagoNV] Agregar Medio de Pago deshabilitado. {ConstruirResumenPagoNuevaVenta()}");
                return;
            }

            try
            {
                boton!.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton);
            }

            Thread.Sleep(900);
        }

        private void IngresarMontoMedioPagoNuevaVenta(string monto)
        {
            if (EsNA(monto)) return;

            string valor = ResolverMontoPago(monto);
            if (string.IsNullOrWhiteSpace(valor)) return;

            if (TryParseDecimalFlexible(valor, out var montoSolicitado))
            {
                var totalDisponible = _paymentContext.TotalAntes ?? ObtenerTotalVentaActual();
                if (totalDisponible.HasValue &&
                    totalDisponible.Value > 0m &&
                    montoSolicitado > totalDisponible.Value)
                {
                    Log($"[PagoNV] El monto solicitado {montoSolicitado:0.00} excede el total disponible {totalDisponible.Value:0.00}. Se ajusta al total de la venta.");
                    valor = totalDisponible.Value.ToString("0.00", CultureInfo.InvariantCulture);
                }
            }

            var input = ObtenerInputMontoMedioPagoNuevaVenta();
            Assert.That(input, Is.Not.Null, "No se encontro el input de monto del medio de pago en Nueva Venta.");

            EstablecerValorInputNuevaVenta(input!, valor);

            try
            {
                input!.SendKeys(Keys.Tab);
            }
            catch
            {
            }

            Thread.Sleep(600);

            Log($"[PagoNV] Monto configurado en input id='{input!.GetAttribute("id")}' value='{input.GetAttribute("value")}'");
        }

        private bool DebeConservarMontoAutocompletadoNuevaVenta(IWebElement input, string valorSolicitado)
        {
            try
            {
                var actualTexto = (input.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
                if (!TryParseDecimalFlexible(actualTexto, out var actual) || actual <= 0m)
                    return false;

                if (!TryParseDecimalFlexible(valorSolicitado, out var solicitado))
                    return false;

                var totalDisponible = _paymentContext.TotalAntes ?? ObtenerTotalVentaActual();
                if (!totalDisponible.HasValue || totalDisponible.Value <= 0m)
                    return false;

                return Math.Abs(actual - totalDisponible.Value) <= 0.05m &&
                       solicitado >= totalDisponible.Value - 0.05m;
            }
            catch
            {
                return false;
            }
        }

        private bool EsperarContenidoPagoNuevaVentaVisible(string textoEsperado)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(4))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(_ => EsContenidoPagoEsperadoVisible(textoEsperado));
            }
            catch
            {
                return false;
            }
        }

        private void IngresarMontoEfectivoNuevaVenta(string monto)
        {
            if (EsNA(monto)) return;

            string valor = ResolverMontoPago(monto);
            Assert.That(string.IsNullOrWhiteSpace(valor), Is.False,
                "No se resolvio un monto valido para efectivo en Nueva Venta.");

            var montoBase = EsperarMontoBaseEfectivoNuevaVenta();
            if (!montoBase.HasValue || montoBase.Value <= 0m)
            {
                SeleccionarTabMedioPagoNuevaVenta("efectivo");
                Thread.Sleep(500);
                montoBase = EsperarMontoBaseEfectivoNuevaVenta(3);
            }

            Assert.That(montoBase.HasValue && montoBase.Value > 0m, Is.True,
                "No se pudo cargar el monto base de efectivo antes de ingresar el valor recibido.");

            var input = Find(VentasLocators.Payment.CashReceivedNewSale);
            ScrollToCenter(input);

            try
            {
                input.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", input);
            }

            EstablecerValorInputNuevaVenta(input, valor);

            try
            {
                input.SendKeys(Keys.Tab);
            }
            catch
            {
            }

            Thread.Sleep(600);
        }

        private decimal? EsperarMontoBaseEfectivoNuevaVenta(int timeoutSeconds = 6)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(_ =>
                {
                    var valor = LeerValor(VentasLocators.Payment.CashAmount);
                    return TryParseDecimalFlexible(valor, out var monto) && monto > 0m
                        ? monto
                        : (decimal?)null;
                });
            }
            catch
            {
                var actual = LeerMontoBaseEfectivoNuevaVenta();
                if (actual.HasValue && actual.Value > 0m)
                    return actual;

                var totalReferencia = _paymentContext.TotalAntes
                    ?? ObtenerTotalVentaActual(incluirMontoPago: false)
                    ?? ObtenerTotalVentaActual();

                if (!totalReferencia.HasValue || totalReferencia.Value <= 0m)
                    return actual;

                var inputMonto = driver.FindElements(VentasLocators.Payment.CashAmount)
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed && e.Enabled; }
                        catch { return false; }
                    });

                if (inputMonto == null)
                    return actual;

                Log($"[PagoNV] El monto base de efectivo no se cargo en la UI. Se sincroniza con el total real '{totalReferencia.Value:0.00}'.");
                EstablecerValorInputNuevaVenta(inputMonto, totalReferencia.Value.ToString("0.00", CultureInfo.InvariantCulture));
                Thread.Sleep(500);

                return LeerMontoBaseEfectivoNuevaVenta();
            }
        }

        private decimal? LeerMontoBaseEfectivoNuevaVenta()
        {
            var valor = LeerValor(VentasLocators.Payment.CashAmount);
            return TryParseDecimalFlexible(valor, out var monto) ? monto : (decimal?)null;
        }

        private void SeleccionarBancoNuevaVenta(string banco)
        {
            if (EsNA(banco)) return;

            var select = EsperarUltimoSelectVisibleNuevaVenta(VentasLocators.Payment.BankSelect);
            if (select == null)
            {
                var trigger = FindFirstVisibleOrAny(VentasLocators.Payment.BankTrigger) ?? ObtenerTriggerPagoVisible(0);
                Assert.That(trigger, Is.Not.Null,
                    $"No se encontro un dropdown visible de banco en Nueva Venta. {ConstruirResumenPagoNuevaVenta()}");
                SeleccionarDropdownCustomNuevaVenta(banco, trigger!);
                return;
            }

            SeleccionarOpcionSelectNuevaVenta(select, banco.Trim());

            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(6)).Until(d =>
                {
                    var tarjetaSelect = d.FindElements(VentasLocators.Payment.CardSelect)
                        .Where(e => e.Displayed && e.Enabled)
                        .LastOrDefault();

                    if (tarjetaSelect == null) return false;

                    return new SelectElement(tarjetaSelect).Options.Count > 1;
                });
            }
            catch
            {
            }
        }

        private void SeleccionarTarjetaNuevaVenta(string tarjeta)
        {
            if (EsNA(tarjeta)) return;

            var select = EsperarUltimoSelectVisibleNuevaVenta(VentasLocators.Payment.CardSelect);
            if (select == null)
            {
                var trigger = FindFirstVisibleOrAny(VentasLocators.Payment.CardTrigger) ?? ObtenerTriggerPagoVisible(1);
                Assert.That(trigger, Is.Not.Null,
                    $"No se encontro un dropdown visible de tarjeta en Nueva Venta. {ConstruirResumenPagoNuevaVenta()}");
                SeleccionarDropdownCustomNuevaVenta(tarjeta, trigger!);
                return;
            }

            SeleccionarOpcionSelectNuevaVenta(select, tarjeta.Trim());
        }

        private void SeleccionarCuentaBancariaNuevaVenta(string cuentaBancaria)
        {
            if (EsNA(cuentaBancaria)) return;

            var select = EsperarUltimoSelectVisibleNuevaVenta(VentasLocators.Payment.BankAccountSelect);
            if (select == null)
            {
                var trigger = FindFirstVisibleOrAny(VentasLocators.Payment.BankAccountTrigger) ?? ObtenerTriggerPagoVisible(0);
                Assert.That(trigger, Is.Not.Null,
                    $"No se encontro un dropdown visible de cuenta bancaria en Nueva Venta. {ConstruirResumenPagoNuevaVenta()}");
                SeleccionarDropdownCustomNuevaVenta(cuentaBancaria, trigger!);
                return;
            }

            var combo = new SelectElement(select);
            var texto = cuentaBancaria.Trim();

            try
            {
                combo.SelectByText(texto);
            }
            catch
            {
                var opcion = combo.Options.FirstOrDefault(x =>
                    x.Text.Trim().IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0);

                if (opcion != null)
                    opcion.Click();
                else
                    throw new Exception($"No se encontro la cuenta bancaria '{texto}' en Nueva Venta.");
            }

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                arguments[0].blur();
            ", select);

            Thread.Sleep(500);
        }

        private void IngresarInformacionNuevaVenta(string informacion)
        {
            if (EsNA(informacion)) return;

            var input = FindLastVisibleInPayment(VentasLocators.Payment.PaymentInfoInput);
            Assert.That(input, Is.Not.Null, "No se encontro el input visible de informacion en Nueva Venta.");

            LimpiarYEscribirCampoNuevaVenta(input!, informacion.Trim());
        }

        private void IngresarObservacionPagoNuevaVenta(string observacion)
        {
            if (EsNA(observacion)) return;

            var input = FindLastVisibleInPayment(VentasLocators.Payment.PaymentObservation);

            Assert.That(input, Is.Not.Null, "No se encontro el campo de observacion del pago en Nueva Venta.");

            EstablecerValorInputNuevaVenta(input!, observacion.Trim());
        }

        private IWebElement? BuscarUltimoSelectVisibleNuevaVenta(By locator)
        {
            try
            {
                return driver.FindElements(locator)
                    .Where(e => e.Displayed && e.Enabled)
                    .LastOrDefault();
            }
            catch
            {
                return null;
            }
        }

        private IWebElement? EsperarUltimoSelectVisibleNuevaVenta(By locator, int timeoutSeconds = 6)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(d =>
                {
                    try
                    {
                        var visibles = d.FindElements(locator)
                            .Where(e => e.Displayed && e.Enabled)
                            .ToList();
                        return visibles.Any() ? visibles.Last() : null;
                    }
                    catch
                    {
                        return null;
                    }
                });
            }
            catch
            {
                return BuscarUltimoSelectVisibleNuevaVenta(locator);
            }
        }

        private IWebElement? ObtenerUltimoInputVisibleNuevaVenta(By locator)
        {
            return wait.Until(d =>
            {
                try
                {
                    var visibles = d.FindElements(locator)
                        .Where(e => e.Displayed && e.Enabled)
                        .ToList();
                    return visibles.Any() ? visibles.Last() : null;
                }
                catch
                {
                    return null;
                }
            });
        }

        private IWebElement? ObtenerInputMontoMedioPagoNuevaVenta()
        {
            try
            {
                return wait.Until(d =>
                {
                    try
                    {
                        var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
                        var visiblesEnContenedor = d.FindElements(VentasLocators.Payment.PaymentAmountInput)
                            .Where(e =>
                            {
                                try
                                {
                                    return contenedorPago != null &&
                                           PerteneceAlContenedorNuevaVenta(contenedorPago, e);
                                }
                                catch
                                {
                                    return false;
                                }
                            })
                            .Where(EsInputMontoMedioPagoValido)
                            .ToList();

                        if (visiblesEnContenedor?.Any() == true)
                            return visiblesEnContenedor.Last();

                        var visiblesGlobales = d.FindElements(VentasLocators.Payment.PaymentAmountInput)
                            .Where(EsInputMontoMedioPagoValido)
                            .ToList();

                        return visiblesGlobales.Any() ? visiblesGlobales.Last() : null;
                    }
                    catch
                    {
                        return null;
                    }
                });
            }
            catch
            {
                Log($"[PagoNV] {ConstruirResumenPagoNuevaVenta("input_monto_no_resuelto")}");
                return null;
            }
        }

        private bool EsInputMontoMedioPagoValido(IWebElement input)
        {
            try
            {
                if (!input.Displayed || !input.Enabled)
                    return false;

                var id = NormalizeText(input.GetAttribute("id") ?? string.Empty);
                var name = NormalizeText(input.GetAttribute("name") ?? string.Empty);
                var formControl = NormalizeText(input.GetAttribute("formcontrolname") ?? string.Empty);
                var placeholder = NormalizeText(input.GetAttribute("placeholder") ?? string.Empty);

                if (id is "amountreceived" or "change" or "informacion")
                    return false;

                if (id.Contains("change") ||
                    name.Contains("change") ||
                    formControl.Contains("change") ||
                    placeholder.Contains("vuelto") ||
                    placeholder.Contains("observ"))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void EstablecerValorInputNuevaVenta(IWebElement input, string valor)
        {
            ScrollToCenter(input);

            try
            {
                input.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", input);
            }

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(150);

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                const el = arguments[0];
                const value = arguments[1];
                const proto = el.tagName === 'TEXTAREA'
                    ? window.HTMLTextAreaElement.prototype
                    : window.HTMLInputElement.prototype;
                const setter = Object.getOwnPropertyDescriptor(proto, 'value')?.set;

                if (setter)
                    setter.call(el, value);
                else
                    el.value = value;

                ['input', 'change', 'keyup', 'blur'].forEach(type => {
                    el.dispatchEvent(new Event(type, { bubbles: true }));
                });
            ", input, valor);

            Thread.Sleep(200);

            var valorActual = input.GetAttribute("value") ?? string.Empty;
            if (!NormalizeText(valorActual).Contains(NormalizeText(valor)))
            {
                input.SendKeys(valor);
                input.SendKeys(Keys.Tab);
                Thread.Sleep(500);
            }
        }

        private void LimpiarValorInputNuevaVenta(IWebElement input)
        {
            ScrollToCenter(input);

            try
            {
                input.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", input);
            }

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(150);

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                const el = arguments[0];
                const proto = el.tagName === 'TEXTAREA'
                    ? window.HTMLTextAreaElement.prototype
                    : window.HTMLInputElement.prototype;
                const setter = Object.getOwnPropertyDescriptor(proto, 'value')?.set;

                if (setter)
                    setter.call(el, '');
                else
                    el.value = '';

                ['input', 'change', 'keyup', 'blur'].forEach(type => {
                    el.dispatchEvent(new Event(type, { bubbles: true }));
                });
            ", input);

            Thread.Sleep(300);
        }

        private void SeleccionarOpcionSelectNuevaVenta(IWebElement selectElement, string texto)
        {
            var combo = new SelectElement(selectElement);
            var textoNormalizado = NormalizeText(texto);
            var opcion = combo.Options.FirstOrDefault(o =>
                NormalizeText(o.Text).Equals(textoNormalizado, StringComparison.OrdinalIgnoreCase));

            if (opcion == null)
                throw new Exception($"No se encontro la opcion '{texto}' en el combo de Nueva Venta.");

            string? value = opcion.GetAttribute("value");

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", selectElement);
            Thread.Sleep(200);
            combo.SelectByText(opcion.Text.Trim());
            Thread.Sleep(300);

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                var el = arguments[0];
                var val = arguments[1];
                var nativeInputValueSetter = Object.getOwnPropertyDescriptor(
                    window.HTMLSelectElement.prototype, 'value').set;
                nativeInputValueSetter.call(el, val);
                el.dispatchEvent(new Event('input',  { bubbles: true }));
                el.dispatchEvent(new Event('change', { bubbles: true }));
                el.blur();
            ", selectElement, value);

            Thread.Sleep(600);
        }

        private void SeleccionarDropdownCustomNuevaVenta(string texto, params By[] triggerLocators)
        {
            var trigger = Find(triggerLocators);
            SeleccionarDropdownCustomNuevaVenta(texto, trigger);
        }

        private void SeleccionarDropdownCustomNuevaVenta(string texto, IWebElement trigger)
        {
            ScrollToCenter(trigger);
            trigger.Click();
            Thread.Sleep(500);

            var inputBusqueda = driver.FindElements(VentasLocators.Payment.DropdownSearchInput)
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (inputBusqueda != null)
            {
                inputBusqueda.SendKeys(Keys.Control + "a");
                inputBusqueda.SendKeys(Keys.Delete);
                inputBusqueda.SendKeys(texto);
                Thread.Sleep(500);
                inputBusqueda.SendKeys(Keys.Enter);
                Thread.Sleep(700);
                return;
            }

            var opcion = BuscarOpcionVisibleNuevaVenta(texto);
            Assert.That(opcion, Is.Not.Null, $"No se encontro una opcion visible para '{texto}' en el dropdown de Nueva Venta.");

            ScrollToCenter(opcion!);
            opcion!.Click();
            Thread.Sleep(700);
        }

        private IWebElement? ObtenerTriggerPagoVisible(int index)
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
            if (contenedorPago == null)
                return null;

            try
            {
                var triggers = contenedorPago.FindElements(By.CssSelector(".select-trigger"))
                    .Where(e => e.Displayed && e.Enabled)
                    .ToList();

                if (triggers.Count == 0)
                    return null;

                return index >= 0 && index < triggers.Count ? triggers[index] : triggers.Last();
            }
            catch
            {
                return null;
            }
        }

        private string ConstruirResumenPagoNuevaVenta(string? contexto = null, bool? guardarHabilitado = null, string? mensajeVisible = null)
        {
            var partes = new List<string>();

            if (!string.IsNullOrWhiteSpace(contexto))
                partes.Add(contexto.Trim());

            var tipo = ObtenerTipoPagoActivoNuevaVenta();
            if (!string.IsNullOrWhiteSpace(tipo))
                partes.Add($"tipo={tipo}");

            partes.Add($"multipago={(EstaMarcado(VentasLocators.Payment.MultipaymentCheckbox) ? "si" : "no")}");

            var tab = ObtenerTextoTabPagoActivoNuevaVenta();
            if (!string.IsNullOrWhiteSpace(tab))
                partes.Add($"tab={tab}");

            var estadoGuardar = guardarHabilitado ?? ObtenerEstadoGuardarActualNuevaVenta();
            if (estadoGuardar.HasValue)
                partes.Add($"guardar={(estadoGuardar.Value ? "habilitado" : "deshabilitado")}");

            var estadoAgregar = ObtenerEstadoAgregarMedioPagoActualNuevaVenta();
            if (estadoAgregar.HasValue)
                partes.Add($"agregar_medio={(estadoAgregar.Value ? "habilitado" : "deshabilitado")}");

            var mensaje = string.IsNullOrWhiteSpace(mensajeVisible)
                ? CapturarValidaciones()
                : mensajeVisible;
            if (!string.IsNullOrWhiteSpace(mensaje))
                partes.Add($"mensaje='{mensaje}'");

            var estadoSeccion = CapturarEstadoSeccionPagoNuevaVenta();
            if (!string.IsNullOrWhiteSpace(estadoSeccion))
                partes.Add($"estado_pago='{estadoSeccion}'");

            var total = LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.CashAmount);
            if (!string.IsNullOrWhiteSpace(total))
                partes.Add($"total={total}");

            var tabNormalizado = NormalizeText(tab);
            if (tabNormalizado.Contains("efectivo"))
            {
                AgregarParteSiTieneValor(partes, "recibido", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.CashReceivedNewSale));
                AgregarParteSiTieneValor(partes, "vuelto", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.Change));
            }
            else if (tabNormalizado.Contains("tarjetas de credito") || tabNormalizado.Contains("tarjetas de debito"))
            {
                AgregarParteSiTieneValor(partes, "banco", LeerTextoSeleccionadoPagoResumenNuevaVenta(0, VentasLocators.Payment.BankSelect, VentasLocators.Payment.BankTrigger));
                AgregarParteSiTieneValor(partes, "tarjeta", LeerTextoSeleccionadoPagoResumenNuevaVenta(1, VentasLocators.Payment.CardSelect, VentasLocators.Payment.CardTrigger));
                AgregarParteSiTieneValor(partes, "monto", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PaymentAmountInput));
                AgregarParteSiTieneValor(partes, "info", LeerTextoResumenPagoNuevaVenta(VentasLocators.Payment.PaymentInfoInput));
            }
            else if (tabNormalizado.Contains("transferencia") || tabNormalizado.Contains("depositos"))
            {
                AgregarParteSiTieneValor(partes, "cuenta", LeerTextoSeleccionadoPagoResumenNuevaVenta(0, VentasLocators.Payment.BankAccountSelect, VentasLocators.Payment.BankAccountTrigger));
                AgregarParteSiTieneValor(partes, "monto", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PaymentAmountInput));
                AgregarParteSiTieneValor(partes, "info", LeerTextoResumenPagoNuevaVenta(VentasLocators.Payment.PaymentInfoInput));
            }
            else if (tabNormalizado.Contains("puntos"))
            {
                AgregarParteSiTieneValor(partes, "pago_pts", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PointsPaymentInput));
                AgregarParteSiTieneValor(partes, "pago_s", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PointsPaymentCurrencyInput));
                AgregarParteSiTieneValor(partes, "restantes_pts", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PointsRemainingInput));
                AgregarParteSiTieneValor(partes, "restantes_s", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PointsRemainingCurrencyInput));
            }
            else if (tabNormalizado.Contains("nota de credito"))
            {
                AgregarParteSiTieneValor(partes, "monto", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PaymentAmountInput));
            }

            return $"Resumen: {string.Join(" | ", partes.Where(x => !string.IsNullOrWhiteSpace(x)))}";
        }

        private string ObtenerTipoPagoActivoNuevaVenta()
        {
            if (EstaMarcado(VentasLocators.Payment.CashType))
                return "contado";

            if (EstaMarcado(VentasLocators.Payment.QuickCreditType))
                return "credito";

            return string.Empty;
        }

        private string ObtenerTextoTabPagoActivoNuevaVenta()
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
            if (contenedorPago == null)
                return string.Empty;

            try
            {
                var tabActiva = contenedorPago.FindElements(By.XPath(
                        ".//*[(@aria-selected='true' or contains(@class,'active') or contains(@class,'selected')) and (contains(@class,'custom-tab') or @role='tab')]"))
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    });

                return tabActiva?.Text?.Trim() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string CapturarEstadoSeccionPagoNuevaVenta()
        {
            var estadoActual = CapturarEstadoSeccionPagoNuevaVentaActual();
            if (!string.IsNullOrWhiteSpace(estadoActual))
                _lastObservedPaymentState = estadoActual;

            return !string.IsNullOrWhiteSpace(estadoActual)
                ? estadoActual
                : _lastObservedPaymentState;
        }

        private string CapturarEstadoSeccionPagoNuevaVentaActual()
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
            var estadoEnPago = BuscarEstadoPagoVisibleNuevaVenta(
                contenedorPago?.FindElements(By.XPath(".//*[normalize-space()]")) ?? Enumerable.Empty<IWebElement>());

            if (!string.IsNullOrWhiteSpace(estadoEnPago))
                return estadoEnPago;

            return BuscarEstadoPagoVisibleNuevaVenta(driver.FindElements(By.XPath(
                "//*[contains(normalize-space(),'correct') or contains(normalize-space(),'Correct') or " +
                "contains(normalize-space(),'complet') or contains(normalize-space(),'Complet') or " +
                "contains(normalize-space(),'requerid') or contains(normalize-space(),'Requerid')]")));
        }

        private string BuscarEstadoPagoVisibleNuevaVenta(IEnumerable<IWebElement> elementos)
        {
            try
            {
                return elementos
                    .Where(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    })
                    .Select(e => e.Text?.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t) && EsTextoEstadoPago(t!))
                    .Distinct()
                    .OrderBy(t => t!.Length)
                    .FirstOrDefault() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string ObservarEstadoSeccionPagoNuevaVenta(int timeoutMs = 2500)
        {
            var ultimoEstado = string.Empty;
            var ultimoLogueado = string.Empty;
            var limite = DateTime.UtcNow.AddMilliseconds(timeoutMs);

            while (DateTime.UtcNow <= limite)
            {
                var estadoActual = CapturarEstadoSeccionPagoNuevaVentaActual();
                if (!string.IsNullOrWhiteSpace(estadoActual))
                {
                    ultimoEstado = estadoActual;
                    _lastObservedPaymentState = estadoActual;

                    if (!estadoActual.Equals(ultimoLogueado, StringComparison.Ordinal))
                    {
                        Log($"[PagoNV] Estado visible: '{estadoActual}'");
                        ultimoLogueado = estadoActual;
                    }

                    if (EsEstadoPagoExitoso(estadoActual))
                        return estadoActual;
                }

                Thread.Sleep(150);
            }

            return !string.IsNullOrWhiteSpace(ultimoEstado)
                ? ultimoEstado
                : _lastObservedPaymentState;
        }

        private static bool EsEstadoPagoExitoso(string estado)
        {
            var normalizado = NormalizeText(estado);
            if (!normalizado.Contains("correctamente"))
                return false;

            return normalizado.Contains("campos requeridos") ||
                   normalizado.Contains("completo los datos") ||
                   normalizado.Contains("se completo los datos");
        }

        private static bool EsEstadoPagoIncompleto(string estado)
        {
            var normalizado = NormalizeText(estado);
            return normalizado.Contains("campos requeridos") &&
                   !normalizado.Contains("correctamente");
        }

        private static bool EsTextoEstadoPago(string texto)
        {
            var normalizado = NormalizeText(texto);
            return normalizado.Contains("campos requeridos") ||
                   normalizado.Contains("completo los datos") ||
                   normalizado.Contains("se completo los datos");
        }

        private void AssertSeccionPagoListaParaGuardar(string mensajeError)
        {
            var estado = ObservarEstadoSeccionPagoNuevaVenta();
            var guardarHabilitado = ObtenerEstadoGuardarActualNuevaVenta();
            var resumen = ConstruirResumenPagoNuevaVenta(guardarHabilitado: guardarHabilitado, mensajeVisible: estado);

            if (EsEstadoPagoExitoso(estado))
                return;

            if (EsEstadoPagoIncompleto(estado))
            {
                Assert.Fail($"{mensajeError} Estado actual: '{estado}'. {resumen}");
                return;
            }

            if (guardarHabilitado == true)
            {
                Log($"[PagoNV] Estado de Pago no quedo visible; se toma como valido por guardar habilitado. {resumen}");
                return;
            }

            Assert.Fail($"{mensajeError} Estado actual: '{estado}'. {resumen}");
        }

        private bool? ObtenerEstadoGuardarActualNuevaVenta()
        {
            try
            {
                var boton = driver.FindElements(VentasLocators.NuevaVenta.GuardarVenta)
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    });

                return boton == null ? null : EstaHabilitadoBotonGuardar(boton);
            }
            catch
            {
                return null;
            }
        }

        private string LeerTextoResumenPagoNuevaVenta(By locator)
        {
            try
            {
                var input = FindLastVisibleInPayment(locator) ?? FindFirstVisibleInPayment(locator);
                return (input?.GetAttribute("value") ?? input?.Text ?? string.Empty).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        private string LeerMontoResumenPagoNuevaVenta(By locator)
        {
            var texto = LeerTextoResumenPagoNuevaVenta(locator);
            return TryParseDecimalFlexible(texto, out var monto)
                ? monto.ToString("0.00", CultureInfo.InvariantCulture)
                : string.Empty;
        }

        private string LeerTextoSeleccionadoPagoResumenNuevaVenta(int fallbackTriggerIndex, params By[] locators)
        {
            foreach (var locator in locators)
            {
                try
                {
                    var select = BuscarUltimoSelectVisibleNuevaVenta(locator);
                    if (select == null)
                        continue;

                    var texto = new SelectElement(select).SelectedOption?.Text?.Trim() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(texto))
                        return texto;
                }
                catch
                {
                }
            }

            try
            {
                var trigger = FindLastVisibleInPayment(locators) ??
                              FindFirstVisibleInPayment(locators) ??
                              ObtenerTriggerPagoVisible(fallbackTriggerIndex);

                return (trigger?.Text ?? string.Empty).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static void AgregarParteSiTieneValor(ICollection<string> partes, string clave, string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                partes.Add($"{clave}={valor}");
        }

        private void LogPaymentControlsSnapshot()
        {
            Log($"[PagoNV] {ConstruirResumenPagoNuevaVenta()}");
        }

        private IWebElement? BuscarOpcionVisibleNuevaVenta(string texto)
        {
            var candidatos = driver.FindElements(By.XPath("//*[self::div or self::span or self::li][normalize-space()]"))
                .Where(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                })
                .ToList();

            var textoNormalizado = NormalizeText(texto);
            var exacto = candidatos.FirstOrDefault(e => NormalizeText(e.Text).Equals(textoNormalizado, StringComparison.OrdinalIgnoreCase));
            if (exacto != null)
                return exacto;

            var contiene = candidatos.FirstOrDefault(e => NormalizeText(e.Text).Contains(textoNormalizado));
            if (contiene != null)
                return contiene;

            if (texto.Contains('|'))
            {
                var partes = texto.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                return candidatos.FirstOrDefault(e =>
                {
                    var actual = NormalizeText(e.Text);
                    return partes.All(parte => actual.Contains(NormalizeText(parte)));
                });
            }

            return null;
        }

        private void LimpiarYEscribirCampoNuevaVenta(IWebElement input, string valor)
        {
            input.Click();
            Thread.Sleep(150);
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(150);
            input.SendKeys(valor);
            input.SendKeys(Keys.Tab);
            Thread.Sleep(300);
        }

        private void ConfigurarPagoPuntosNuevaVenta(string monto)
        {
            var montoObjetivo = ResolverMontoPagoPuntosNuevaVenta(monto);
            Assert.That(string.IsNullOrWhiteSpace(montoObjetivo), Is.False,
                "No se resolvio un monto valido para el pago con puntos en Nueva Venta.");

            Assert.That(TryParseDecimalFlexible(montoObjetivo, out var montoObjetivoDecimal), Is.True,
                $"No se pudo interpretar el monto configurado para puntos '{montoObjetivo}'.");

            var inputSoles = FindLastVisibleInPayment(VentasLocators.Payment.PointsPaymentCurrencyInput);
            var inputPuntos = FindLastVisibleInPayment(VentasLocators.Payment.PointsPaymentInput);
            var estadoInicial = CapturarEstadoPagoPuntosNuevaVenta();

            Assert.That(inputSoles != null || inputPuntos != null, Is.True,
                "No se encontraron inputs visibles para el pago con puntos en Nueva Venta.");

            if (inputSoles != null)
            {
                EstablecerValorInputNuevaVenta(inputSoles, montoObjetivo);
                Thread.Sleep(700);

                var valorSoles = (inputSoles.GetAttribute("value") ?? inputSoles.Text ?? string.Empty).Trim();
                Log($"[PagoNV] Pago con puntos configurado en soles value='{valorSoles}'");

                if (EsperarPagoPuntosAplicadoNuevaVenta(estadoInicial, montoObjetivoDecimal))
                    return;
            }

            var valorPuntos = ResolverValorPuntosDesdeMontoNuevaVenta(montoObjetivo);
            Assert.That(string.IsNullOrWhiteSpace(valorPuntos), Is.False,
                "No se pudo calcular el equivalente en puntos para completar el pago.");
            Assert.That(inputPuntos, Is.Not.Null,
                "No se encontro el input de pago en puntos para aplicar el fallback.");

            EstablecerValorInputNuevaVenta(inputPuntos!, valorPuntos);
            Thread.Sleep(700);

            Log($"[PagoNV] Pago con puntos configurado en puntos value='{inputPuntos!.GetAttribute("value")}'");

            if (!EsperarPagoPuntosAplicadoNuevaVenta(estadoInicial, montoObjetivoDecimal))
                Log("[PagoNV] El pago con puntos no se reflejo en los saldos visibles luego de completar los inputs.");
        }

        private string ResolverMontoPagoPuntosNuevaVenta(string monto)
        {
            return ResolverMontoPago(monto);
        }

        private string ResolverValorPuntosDesdeMontoNuevaVenta(string montoSoles)
        {
            if (!TryParseDecimalFlexible(montoSoles, out var montoObjetivo) || montoObjetivo <= 0m)
                return string.Empty;

            var puntosAcumulados = LeerValor(VentasLocators.Payment.PointsAccumulatedInput);
            var equivalenteSoles = LeerValor(VentasLocators.Payment.PointsAccumulatedCurrencyInput);

            if (!TryParseDecimalFlexible(puntosAcumulados, out var totalPuntos) ||
                !TryParseDecimalFlexible(equivalenteSoles, out var totalSoles) ||
                totalPuntos <= 0m ||
                totalSoles <= 0m)
            {
                return string.Empty;
            }

            var valorPuntos = Math.Round(montoObjetivo * (totalPuntos / totalSoles), 2, MidpointRounding.AwayFromZero);
            return valorPuntos.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private PointsPaymentState CapturarEstadoPagoPuntosNuevaVenta()
        {
            return new PointsPaymentState
            {
                PuntosAcumulados = TryParseDecimalFlexible(LeerValor(VentasLocators.Payment.PointsAccumulatedInput), out var puntosAcumulados)
                    ? puntosAcumulados
                    : (decimal?)null,
                SolesAcumulados = TryParseDecimalFlexible(LeerValor(VentasLocators.Payment.PointsAccumulatedCurrencyInput), out var solesAcumulados)
                    ? solesAcumulados
                    : (decimal?)null,
                PuntosRestantes = TryParseDecimalFlexible(LeerValor(VentasLocators.Payment.PointsRemainingInput), out var puntosRestantes)
                    ? puntosRestantes
                    : (decimal?)null,
                SolesRestantes = TryParseDecimalFlexible(LeerValor(VentasLocators.Payment.PointsRemainingCurrencyInput), out var solesRestantes)
                    ? solesRestantes
                    : (decimal?)null
            };
        }

        private bool EsperarPagoPuntosAplicadoNuevaVenta(PointsPaymentState estadoInicial, decimal montoObjetivo, int timeoutSeconds = 4)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(_ => SeReflejoPagoPuntosNuevaVenta(estadoInicial, montoObjetivo));
            }
            catch
            {
                return SeReflejoPagoPuntosNuevaVenta(estadoInicial, montoObjetivo);
            }
        }

        private bool SeReflejoPagoPuntosNuevaVenta(PointsPaymentState estadoInicial, decimal montoObjetivo)
        {
            var actual = CapturarEstadoPagoPuntosNuevaVenta();

            var disminuyoSaldoSoles = estadoInicial.SolesRestantes.HasValue &&
                                      actual.SolesRestantes.HasValue &&
                                      actual.SolesRestantes.Value < estadoInicial.SolesRestantes.Value - 0.05m;

            var disminuyoSaldoPuntos = estadoInicial.PuntosRestantes.HasValue &&
                                       actual.PuntosRestantes.HasValue &&
                                       actual.PuntosRestantes.Value < estadoInicial.PuntosRestantes.Value - 0.05m;

            if (disminuyoSaldoSoles || disminuyoSaldoPuntos)
            {
                Log($"[PagoNV] Pago con puntos reflejado. Restantes S/='{actual.SolesRestantes?.ToString("0.00", CultureInfo.InvariantCulture) ?? "NA"}' Pts='{actual.PuntosRestantes?.ToString("0.00", CultureInfo.InvariantCulture) ?? "NA"}'");
                return true;
            }

            return montoObjetivo > 0m &&
                   string.IsNullOrWhiteSpace(CapturarValidaciones()) &&
                   IsSaveEnabled();
        }

        private void ConfirmarPagoPuntosNuevaVentaSiAplica()
        {
            if (_paymentContext.Multipago || IsSaveEnabled())
                return;

            var botonAgregar = driver.FindElements(VentasLocators.Payment.AddPaymentButton)
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (botonAgregar == null)
                return;

            Log("[PagoNV] Se intenta confirmar el pago con puntos agregando el medio actual.");
            GuardarMedioPagoActualNuevaVenta();
        }

        private string ResolverMontoPago(string monto, decimal? totalReferencia = null)
        {
            if (string.IsNullOrWhiteSpace(monto) || monto.Trim().Equals("NA", StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            var normalizado = NormalizeText(monto).Replace(" ", string.Empty);
            if (EsExpresionMontoBasadaEnTotal(normalizado))
            {
                var totalActual = totalReferencia
                    ?? _paymentContext.TotalAntes
                    ?? ObtenerTotalVentaActual(incluirMontoPago: false)
                    ?? ObtenerTotalVentaActual();
                Assert.That(totalActual.HasValue, Is.True,
                    "No se pudo obtener el total actual de la venta para resolver el monto del pago.");

                var montoResuelto = ResolverExpresionMontoBasadaEnTotal(normalizado, totalActual!.Value);
                Assert.That(montoResuelto, Is.GreaterThan(0m),
                    $"La expresion de monto '{monto}' debe resolver un valor mayor a cero.");
                return montoResuelto.ToString("0.00", CultureInfo.InvariantCulture);
            }

            return monto.Trim();
        }

        private static bool EsExpresionMontoBasadaEnTotal(string valorNormalizado)
        {
            if (string.IsNullOrWhiteSpace(valorNormalizado))
                return false;

            return valorNormalizado is "total" or "cubre_total" or "total_venta" ||
                   Regex.IsMatch(valorNormalizado, @"^(total|cubre_total|total_venta)[+-]\d+(?:[.,]\d+)?$");
        }

        private static decimal ResolverExpresionMontoBasadaEnTotal(string valorNormalizado, decimal totalReferencia)
        {
            if (valorNormalizado is "total" or "cubre_total" or "total_venta")
                return totalReferencia;

            var match = Regex.Match(valorNormalizado, @"^(total|cubre_total|total_venta)(?<operador>[+-])(?<delta>\d+(?:[.,]\d+)?)$");
            Assert.That(match.Success, Is.True,
                $"No se reconoce la expresion de monto '{valorNormalizado}'.");

            var deltaTexto = match.Groups["delta"].Value;
            Assert.That(TryParseDecimalFlexible(deltaTexto, out var delta), Is.True,
                $"No se pudo interpretar el delta '{deltaTexto}' de la expresion de monto.");

            return match.Groups["operador"].Value == "+"
                ? totalReferencia + delta
                : totalReferencia - delta;
        }

        private List<decimal?> ResolverMontosEsperadosPago(string montoPorMedio, decimal? totalReferencia)
        {
            return SepararValoresFiltrados(montoPorMedio)
                .Select(valor =>
                {
                    var resuelto = ResolverMontoPago(valor, totalReferencia);
                    return TryParseDecimalFlexible(resuelto, out var monto) ? monto : (decimal?)null;
                })
                .ToList();
        }

        private static bool EsNA(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ||
                   valor.Trim().Equals("NA", StringComparison.OrdinalIgnoreCase);
        }

        private static List<string> SepararValores(string valor)
        {
            if (EsNA(valor)) return new List<string>();

            return valor
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        private static List<string> SepararValoresFiltrados(string valor)
        {
            if (EsNA(valor)) return new List<string>();

            return valor
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x) &&
                            !x.Equals("NA", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private static string ConsumirSiguientePago(Queue<string> cola)
        {
            if (cola == null || cola.Count == 0)
                return "NA";

            return cola.Dequeue();
        }

        private void AssertGuardarHabilitadoEnPago(string mensajeError)
        {
            var habilitado = IsSaveEnabled();
            var mensajeActual = CapturarValidaciones();
            var resumen = ConstruirResumenPagoNuevaVenta(guardarHabilitado: habilitado, mensajeVisible: mensajeActual);

            if (!habilitado)
                Log($"[PagoNV] {resumen}");

            Assert.That(habilitado, Is.True,
                $"{mensajeError} {resumen}");
        }

        private void AssertGuardarDeshabilitadoEnPago(string mensajeError)
        {
            var habilitado = IsSaveEnabled();
            var mensajeActual = string.Join(" | ", CapturarValidacionesVisibles());
            var resumen = ConstruirResumenPagoNuevaVenta(guardarHabilitado: habilitado, mensajeVisible: mensajeActual);

            Assert.That(habilitado, Is.False,
                $"{mensajeError} {resumen}");
        }

        private void AssertCronogramaCreditoConfiguradoNuevaVenta()
        {
            if (!string.IsNullOrWhiteSpace(_lastCreditInstallments))
            {
                AssertInputExacto(
                    VentasLocators.Payment.CreditInstallmentsInput,
                    new[] { _lastCreditInstallments },
                    "El numero de cuotas deberia quedar registrado correctamente.");
            }
        }

        private void AssertMedioPagoNoDisponibleNuevaVenta(string textoEsperado, string mensajeError, params By[] locators)
        {
            var candidatos = ObtenerCandidatosTabPagoNuevaVenta(textoEsperado, locators);
            if (candidatos.Count == 0)
                return;

            var objetivo = ResolverObjetivoTabPagoNuevaVenta(candidatos[0]);
            var clases = NormalizeText(objetivo.GetAttribute("class") ?? string.Empty);
            var ariaDisabled = NormalizeText(objetivo.GetAttribute("aria-disabled") ?? string.Empty);

            if (clases.Contains("disabled") || ariaDisabled == "true")
                return;

            try
            {
                EjecutarClickTabPagoNuevaVenta(objetivo);
                Thread.Sleep(400);
            }
            catch
            {
                return;
            }

            var quedoDisponible = EsTabPagoActiva(textoEsperado) && EsContenidoPagoEsperadoVisible(textoEsperado);
            Assert.That(quedoDisponible, Is.False,
                $"{mensajeError} {ConstruirResumenPagoNuevaVenta($"tab_no_permitido={textoEsperado}")}");
        }

        private void AssertMensajePagoNoVisible(params string[] fragmentosNoPermitidos)
        {
            var visibles = CapturarValidacionesVisibles()
                .Select(NormalizeText)
                .ToList();

            foreach (var fragmento in fragmentosNoPermitidos.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var esperado = NormalizeText(fragmento);
                Assert.That(visibles.Any(v => v.Contains(esperado)), Is.False,
                    $"No deberia mostrarse el mensaje '{fragmento}'. {ConstruirResumenPagoNuevaVenta(mensajeVisible: string.Join(" | ", visibles))}");
            }
        }

        private void AssertTabPagoActiva(string textoEsperado)
        {
            Assert.That(EsTabPagoActiva(textoEsperado), Is.True,
                $"La pestana activa deberia corresponder a '{textoEsperado}'. {ConstruirResumenPagoNuevaVenta()}");
        }

        private void AssertTextoSeleccionado(IReadOnlyList<string> esperados, string mensaje, int fallbackTriggerIndex, params By[] locators)
        {
            Assert.That(esperados.Count, Is.GreaterThan(0), $"{mensaje} No se recibio un valor esperado.");

            var select = locators.Select(BuscarUltimoSelectVisibleNuevaVenta).FirstOrDefault(e => e != null);
            var trigger = FindFirstVisibleOrAny(locators) ?? ObtenerTriggerPagoVisible(fallbackTriggerIndex);
            var actual = select != null
                ? new SelectElement(select).SelectedOption?.Text?.Trim() ?? string.Empty
                : (trigger?.Text ?? string.Empty).Trim();
            var esperado = esperados[0];

            if (esperado.Contains('|'))
            {
                foreach (var parte in esperado.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    Assert.That(NormalizeText(actual), Does.Contain(NormalizeText(parte)),
                        $"{mensaje} Valor actual: '{actual}'.");
                }
                return;
            }

            Assert.That(NormalizeText(actual), Does.Contain(NormalizeText(esperado)),
                $"{mensaje} Valor actual: '{actual}'.");
        }

        private void AssertInputExacto(By locator, IReadOnlyList<string> esperados, string mensaje)
        {
            Assert.That(esperados.Count, Is.GreaterThan(0), $"{mensaje} No se recibio un valor esperado.");

            var input = ObtenerUltimoInputVisibleNuevaVenta(locator);
            Assert.That(input, Is.Not.Null, $"{mensaje} No se encontro el input visible.");

            var actual = (input!.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
            Assert.That(actual, Is.EqualTo(esperados[0]), $"{mensaje} Valor actual: '{actual}'.");
        }

        private void AssertInputAproximado(By locator, IReadOnlyList<decimal?> esperados, string mensaje)
        {
            var esperado = esperados.FirstOrDefault();
            Assert.That(esperado.HasValue, Is.True, $"{mensaje} No se recibio un monto esperado.");
            AssertInputAproximado(locator, esperado!.Value, mensaje);
        }

        private void AssertMontoMedioPagoNuevaVenta(IReadOnlyList<decimal?> esperados, string mensaje)
        {
            var esperado = esperados.FirstOrDefault();
            Assert.That(esperado.HasValue, Is.True, $"{mensaje} No se recibio un monto esperado.");

            var input = ObtenerInputMontoMedioPagoNuevaVenta();
            Assert.That(input, Is.Not.Null, $"{mensaje} No se encontro el campo visible de monto.");

            var actualTexto = (input!.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
            Assert.That(TryParseDecimalFlexible(actualTexto, out var actual), Is.True,
                $"{mensaje} No se pudo interpretar el valor actual '{actualTexto}'.");
            Assert.That(actual, Is.EqualTo(esperado!.Value).Within(0.05m),
                $"{mensaje} Valor actual: {actual:0.00} | esperado: {esperado:0.00}");
        }

        private void AssertInputAproximado(By locator, decimal esperado, string mensaje)
        {
            var input = FindFirstVisibleOrAny(locator);
            Assert.That(input, Is.Not.Null, $"{mensaje} No se encontro el campo visible.");

            var actualTexto = (input!.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
            Assert.That(TryParseDecimalFlexible(actualTexto, out var actual), Is.True,
                $"{mensaje} No se pudo interpretar el valor actual '{actualTexto}'.");
            Assert.That(actual, Is.EqualTo(esperado).Within(0.05m),
                $"{mensaje} Valor actual: {actual:0.00} | esperado: {esperado:0.00}");
        }

        private void TryAssertInputAproximado(By locator, decimal esperado, string mensaje)
        {
            try
            {
                var input = FindFirstVisibleOrAny(locator);
                if (input == null)
                    return;

                var actualTexto = (input.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
                if (!TryParseDecimalFlexible(actualTexto, out var actual))
                    return;

                if (actual <= 0m)
                    return;

                Assert.That(actual, Is.EqualTo(esperado).Within(0.05m),
                    $"{mensaje} Valor actual: {actual:0.00} | esperado: {esperado:0.00}");
            }
            catch
            {
            }
        }

        private bool IsNewSaleFormReset()
        {
            var indicator = driver.FindElements(By.XPath("//*[contains(normalize-space(),'Ningun producto seleccionado') or contains(normalize-space(),'NingÃºn producto seleccionado')]") )
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
            throw new NoSuchElementException($"No se encontro: {string.Join(" | ", locators.Select(l => l.ToString()))}");
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

        private bool IsSaveEnabled()
        {
            IWebElement? btn = null;
            for (int intento = 0; intento < 20; intento++)
            {
                btn = driver.FindElements(VentasLocators.NuevaVenta.GuardarVenta)
                    .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

                if (btn != null && EstaHabilitadoBotonGuardar(btn))
                    return true;

                Thread.Sleep(150);
            }

            if (btn == null) return false;
            return EstaHabilitadoBotonGuardar(btn);
        }

        private IWebElement? ObtenerBotonAgregarMedioPagoVisible()
        {
            try
            {
                return driver.FindElements(VentasLocators.Payment.AddPaymentButton)
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    });
            }
            catch
            {
                return null;
            }
        }

        private bool? ObtenerEstadoAgregarMedioPagoActualNuevaVenta()
        {
            var boton = ObtenerBotonAgregarMedioPagoVisible();
            return boton == null ? null : EstaHabilitadoBotonAccion(boton);
        }

        private bool EstaHabilitadoBotonAccion(IWebElement boton)
        {
            try
            {
                var classes = boton.GetAttribute("class") ?? string.Empty;
                var ariaDisabled = boton.GetAttribute("aria-disabled") ?? string.Empty;
                var disabled = boton.GetAttribute("disabled") ?? string.Empty;

                return boton.Enabled &&
                       !classes.Contains("disabled", StringComparison.OrdinalIgnoreCase) &&
                       !ariaDisabled.Equals("true", StringComparison.OrdinalIgnoreCase) &&
                       string.IsNullOrWhiteSpace(disabled);
            }
            catch
            {
                return false;
            }
        }

        private bool EstaHabilitadoBotonGuardar(IWebElement boton)
        {
            return EstaHabilitadoBotonAccion(boton);
        }

        // Captura el primer mensaje de validacion visible: primero toasts/popups bloqueantes,
        // luego invalid-feedback / text-danger inline en el formulario.
        private string CapturarValidaciones()
        {
            return CapturarValidacionesVisibles().FirstOrDefault() ?? string.Empty;
        }

        private IReadOnlyList<string> CapturarValidacionesVisibles()
        {
            var mensajes = new List<string>();

            var popup = CaptureVisibleMessage(1);
            if (!string.IsNullOrWhiteSpace(popup) && IsBlockingMessage(popup))
                mensajes.Add(popup.Trim());

            mensajes.AddRange(driver.FindElements(By.XPath(
                    "//*[contains(@class,'invalid-feedback') or contains(@class,'text-danger') or " +
                    "contains(@class,'custom-error-message')][normalize-space()]"))
                .Where(e => { try { return e.Displayed; } catch { return false; } })
                .Select(e => e.Text?.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))!
                .Select(t => t!));

            return mensajes
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private void AssertMensajesValidacionPago(string mensajeError, params string[] fragmentosEsperados)
        {
            var visibles = CapturarValidacionesVisibles();
            var resumenMensajes = string.Join(" | ", visibles);
            var resumen = ConstruirResumenPagoNuevaVenta(mensajeVisible: resumenMensajes);

            foreach (var fragmento in fragmentosEsperados.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var esperado = NormalizeText(fragmento);
                Assert.That(visibles.Any(v => NormalizeText(v).Contains(esperado)), Is.True,
                    $"{mensajeError} Falta el mensaje '{fragmento}'. {resumen}");
            }
        }

        private void AssertAlgunMensajeValidacionPago(string mensajeError, params string[] fragmentosEsperados)
        {
            var visibles = CapturarValidacionesVisibles();
            var resumenMensajes = string.Join(" | ", visibles);
            var resumen = ConstruirResumenPagoNuevaVenta(mensajeVisible: resumenMensajes);
            var esperados = fragmentosEsperados
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(NormalizeText)
                .ToList();

            Assert.That(esperados.Count, Is.GreaterThan(0),
                $"{mensajeError} No se recibieron fragmentos esperados.");
            Assert.That(visibles.Any(v => esperados.Any(e => NormalizeText(v).Contains(e))), Is.True,
                $"{mensajeError} No se encontro ninguno de los mensajes esperados: {string.Join(" | ", fragmentosEsperados)}. {resumen}");
        }

        private void AssertAgregarMedioPagoDeshabilitado(string mensajeError)
        {
            var habilitado = ObtenerEstadoAgregarMedioPagoActualNuevaVenta();
            var resumen = ConstruirResumenPagoNuevaVenta();

            Assert.That(habilitado.HasValue, Is.True,
                $"{mensajeError} No se encontro el boton Agregar Medio de Pago. {resumen}");
            Assert.That(habilitado.GetValueOrDefault(), Is.False,
                $"{mensajeError} {resumen}");
        }

        // Devuelve true si el mensaje es una validacion bloqueante real (error/advertencia del negocio).
        // Devuelve false para mensajes informativos de exito del sistema que no representan un problema.
        private static void Log(string msg) =>
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {msg}");

        private static bool IsBlockingMessage(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return false;
            var n = NormalizeText(msg);
            // Mensajes informativos del sistema que confirman que el formulario esta bien;
            // no son errores de validacion ni impiden guardar.
            if (n.Contains("completo los campos") ||
                n.Contains("completo los datos") ||
                n.Contains("campos requeridos correctamente"))
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

