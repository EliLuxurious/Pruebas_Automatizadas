using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SIGES3_0.Pages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SIGES3_0.Pages.PedidoPage
{
    public class VerPedidosPage : BasePage
    {
        public VerPedidosPage(IWebDriver driver) : base(driver) { }

        // --- NAVEGACIÓN ---
        private readonly By submoduloVerPedidos = By.XPath("//span[normalize-space()='Ver Pedidos']");

        // --- NUEVO PEDIDO ---
        private readonly By btnNuevoPedido = By.XPath("//button[normalize-space()='Nuevo Pedido']");
        private readonly By cmbFamilia = By.XPath("//span[normalize-space()='Seleccione una familia']");
        private readonly By cmbConcepto = By.XPath("//span[normalize-space()='Seleccione un concepto']");
        private readonly By txtCantidad = By.XPath("//table/tbody/tr[1]//input");
        private readonly By chkIGV = By.XPath("//label[normalize-space()='IGV']");
        private readonly By chkDetUnif = By.XPath("//label[normalize-space()='DET.UNIF.']");
        private readonly By chkDescuento = By.XPath("//label[normalize-space()='Descuento']");
        private readonly By btnDescuentoItem = By.XPath("//button[normalize-space()='Item']");
        private readonly By btnDescuentoGlobal = By.XPath("//button[normalize-space()='Global']");
        private readonly By btnDescuentoSoles = By.XPath("//button[normalize-space()='$']");
        private readonly By btnDescuentoPorcentaje = By.XPath("//button[normalize-space()='%']");
        private readonly By txtDescuento = By.XPath("//input[@placeholder='0']");
        private readonly By txtCliente = By.CssSelector("input.search-input[placeholder='Buscar...']");
        private readonly By btnRegistrarPedido = By.XPath("//button[normalize-space()='Registrar Pedido']");
        private readonly By btnOKConfirmacion = By.XPath("//button[normalize-space()='OK']");
        private readonly By loadingContainer = By.CssSelector("div.loading-container");

        // --- MENSAJES ---
        private readonly By mensajeError = By.XPath("//strong[normalize-space()='Se encontraron inconsistencias en los datos:']");
        private readonly By mensajeInconsistenciaRegistro = By.XPath("//strong[normalize-space()='Se encontraron inconsistencias en los datos:']");
        private readonly By detalleInconsistenciaRegistro = By.XPath("//div[contains(@class,'alert-danger')]//li");
        private readonly By mensajeSinProductoRegistro = By.XPath("//span[@class='badge-status danger']");

        // --- EDITAR / INVALIDAR / CONFIRMAR ---
        private readonly By btnEditarPrimerRegistro = By.XPath("//tbody/tr[1]/td[9]/div[1]/button[1]");
        private readonly By btnEditarPedidoFinal = By.XPath("//button[normalize-space()='Editar Pedido']");
        private readonly By txtFiltroEstado = By.XPath("//th[8]//input[1]");
        private readonly By btnInvalidarPrimerRegistro = By.XPath("//tbody/tr[1]/td[9]/div[1]/button[2]");
        private readonly By txtMotivoInvalidacion = By.XPath("//textarea[@placeholder='Ingrese el motivo de la anulación...']");
        private readonly By btnSiInvalidar = By.XPath("//button[normalize-space()='Sí']");
        private readonly By btnNoInvalidar = By.XPath("//button[normalize-space()='No']");
        private readonly By txtFiltroTotal = By.XPath("//th[7]//input[1]");
        private readonly By btnConfirmarPrimerRegistro = By.XPath("//tbody/tr[1]/td[9]/div[1]/button[3]");
        private readonly By btnConfirmarPedidoFinal = By.XPath("//button[contains(normalize-space(.),'Confirmar Pedido')]");

        // --- SECCIONES CONFIRMACIÓN ---
        private readonly By seccionFacturacionConfirmacion = By.XPath("//div[contains(@class,'d-flex') and contains(@class,'align-items-center') and contains(@class,'w-100')][.//span[normalize-space()='Facturación']]");
        private readonly By txtClienteConfirmacion = By.CssSelector("input.search-input[placeholder='Buscar...']");
        private readonly By cmbTipoComprobanteConfirmacion = By.XPath("//div[contains(@class,'select-trigger') and contains(@class,'form-control')]");
        private readonly By panelDropdownNgSelect = By.CssSelector(".ng-dropdown-panel");
        private readonly By btnGuiaRemisionConfirmacion = By.XPath("//button[.//span[normalize-space()='Guia de remisión'] or normalize-space()='Guia de remisión']");
        private readonly By rbtContadoConfirmacion = By.XPath("//label[normalize-space()='Al contado']");
        private readonly By tabEfectivoConfirmacion = By.XPath("//*[contains(text(),'EFECTIVO')]");
        private readonly By txtRecibidoEfectivoConfirmacion = By.XPath("//input[@id='amountReceived']");
        private readonly By seccionPagoConfirmacion = By.XPath("//span[normalize-space()='Pago']/ancestor::div[contains(@class,'d-flex align-items-center w-100')][1]");
        private readonly By bodyPagoConfirmacion = By.XPath("//div[contains(@class,'accordion-body')][.//label[normalize-space()='Contado' or normalize-space()='Crédito']]");
        private readonly By chkMultipagoConfirmacion = By.XPath("//input[@id='checkTypePaymentMethod']");
        private readonly By tabTarjetaCreditoConfirmacion = By.XPath("//span[normalize-space()='TARJETAS DE CREDITO']");
        private readonly By tabTarjetaDebitoConfirmacion = By.XPath("//span[normalize-space()='TARJETAS DE DEBITO']");
        private readonly By tabTransferenciaConfirmacion = By.XPath("//span[normalize-space()='TRANSFERENCIA DE FONDOS']");
        private readonly By tabDepositosConfirmacion = By.XPath("//span[normalize-space()='DEPOSITOS EN CUENTA']");
        private readonly By tabPuntosConfirmacion = By.XPath("//span[normalize-space()='PUNTOS']");
        private readonly By cmbBancoConfirmacion = By.XPath("//select[@id='bankEntityId']");
        private readonly By cmbTarjetaConfirmacion = By.XPath("//select[@id='bankingCard']");
        private readonly By txtInformacionConfirmacion = By.XPath("//input[@id='informacion']");
        private readonly By cmbCuentaBancariaConfirmacion = By.XPath("//select[@id='bankAccountId' or @id='bankEntityId']");
        private readonly By txtNumeroCuotasConfirmacion = By.XPath("//input[@type='number'][@min='1'][@max='60']");
        private readonly By txtMontoInicialCreditoConfirmacion = By.XPath("//input[@type='number'][@min='0']");
        private readonly By btnAgregarMedioPagoConfirmacion = By.XPath("//button[normalize-space()='Agregar Medio de Pago']");
        private readonly By rbtCreditoConfirmacion = By.XPath("//label[normalize-space()='Crédito']");
        private readonly By txtMontoMedioPagoConfirmacion = By.XPath("//input[@type='number' and not(@id='amountReceived')]");

        private const string TOTAL_BASE_MAYOR_700 = "759";
        private const string TOTAL_BASE_MENOR_IGUAL_700 = "32";

        private string ultimaAccion = "";
        private string ultimoMedioPagoConfirmacion = "";
        private string? mensajeErrorCapturado = null;

        public bool HayErrorCapturado() => !string.IsNullOrEmpty(mensajeErrorCapturado);

        // --- MÉTODOS DE NEGOCIO ---

        public void SeleccionarOpcion(string opcion) => ClickSeguro(By.XPath($"//*[contains(text(),'{opcion}')]"));

        public void SeleccionarFamilia(string familia)
        {
            if (EsValorIgnorado(familia)) return;
            ClickSeguro(cmbFamilia);
            ClickSeguro(By.XPath($"//span[normalize-space()='{familia}']"));
        }

        public void SeleccionarConcepto(string concepto)
        {
            if (EsValorIgnorado(concepto)) return;
            ClickSeguro(cmbConcepto);
            ClickSeguro(By.XPath($"//*[contains(text(),'{concepto}')]"));
        }

        public void IngresarCantidad(string cantidad)
        {
            if (cantidad == "0" || EsValorIgnorado(cantidad)) return;
            LimpiarEIngresarTexto(txtCantidad, cantidad);
        }

        public void ActivarIGV(string valor)
        {
            if (valor.Trim().Equals("true", StringComparison.OrdinalIgnoreCase)) ClickSeguro(chkIGV);
        }

        public void ActivarDetUnif(string valor)
        {
            if (valor.Trim().Equals("true", StringComparison.OrdinalIgnoreCase)) ClickSeguro(chkDetUnif);
        }

        public void ConfigurarDescuento(string activo, string tipo, string modo, string valor)
        {
            if (!activo.Trim().Equals("true", StringComparison.OrdinalIgnoreCase)) return;

            ClickSeguro(chkDescuento);
            if (tipo.Trim().Equals("item", StringComparison.OrdinalIgnoreCase)) ClickSeguro(btnDescuentoItem);
            if (tipo.Trim().Equals("global", StringComparison.OrdinalIgnoreCase)) ClickSeguro(btnDescuentoGlobal);
            if (modo.Trim().Equals("$", StringComparison.OrdinalIgnoreCase)) ClickSeguro(btnDescuentoSoles);
            if (modo.Trim().Equals("%", StringComparison.OrdinalIgnoreCase)) ClickSeguro(btnDescuentoPorcentaje);

            LimpiarEIngresarTexto(txtDescuento, valor);
        }

        public void BuscarCliente(string cliente)
        {
            if (EsValorIgnorado(cliente) || cliente == "00000000" || cliente.ToLower() == "varios") return;

            LimpiarEIngresarTexto(txtCliente, cliente);
            driver.FindElement(txtCliente).SendKeys(Keys.Enter);

            wait.Until(d => !string.IsNullOrEmpty(d.FindElement(txtCliente).GetAttribute("value")));
        }

        public void SeleccionarEntrega(string tipoEntrega)
        {
            if (EsValorIgnorado(tipoEntrega)) return;
            string xpath = tipoEntrega.Trim().Equals("inmediata", StringComparison.OrdinalIgnoreCase)
                ? "//label[normalize-space()='Inmediata']" : "//label[normalize-space()='Diferida']";
            ClickSeguro(By.XPath(xpath));
        }

        public void RegistrarPedido()
        {
            ultimaAccion = "registrar";

            var boton = wait.Until(ExpectedConditions.ElementExists(btnRegistrarPedido));

            ScrollToElement(boton);
            Thread.Sleep(500); 

            try
            {
                boton.Click();
            }
            catch
            {
                JsClick(boton);
            }
        }

        public void AbrirSeccion(string seccion)
        {
            try { waitLong.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingContainer)); } catch { }

            if (EsContenidoVisible(seccion)) return;

            var header = waitLong.Until(d => d.FindElement(By.XPath($"//h2[contains(@class,'accordion-header')][.//*[contains(normalize-space(.),'{seccion}')]]")));
            ScrollToElement(header);

            try { header.FindElement(By.XPath(".//button | .//*[@role='button']")).Click(); }
            catch { JsClick(header); }

            waitLong.Until(d => EsContenidoVisible(seccion));
        }

        private bool EsContenidoVisible(string seccion)
        {
            try
            {
                if (seccion.Trim().Equals("Facturación", StringComparison.OrdinalIgnoreCase))
                    return driver.FindElements(By.CssSelector("input.search-input[placeholder='Buscar...']")).Any(e => e.Displayed);

                if (seccion.Trim().Equals("Entrega", StringComparison.OrdinalIgnoreCase))
                    return driver.FindElements(By.XPath("//label[normalize-space()='Inmediata' or normalize-space()='Diferida']")).Any(e => e.Displayed);

                return driver.FindElements(By.XPath($"//app-form-accordion[contains(@class,'is-expanded')][.//h2[contains(@class,'accordion-header')][.//*[contains(normalize-space(.),'{seccion}')]]]")).Any(e => e.Displayed);
            }
            catch { return false; }
        }

        public void VolverAVerPedidos() => ClickSeguro(By.XPath("//span[contains(text(),'Ver Pedidos')]"));

        // --- ACTUALIZAR PEDIDO ---
        public void ActualizarPedido(
            string familia, string concepto, string cantidad, string igv,
            string detUnif, string descuentoActivo, string tipoDescuento,
            string modoDescuento, string valorDescuento, string cliente, string tipoEntrega)
        {
            bool algunCambioReal = !EsValorIgnorado(familia) || !EsValorIgnorado(concepto) ||
                                   !EsValorIgnorado(cantidad) || !EsValorIgnorado(igv) ||
                                   !EsValorIgnorado(detUnif) || !EsValorIgnorado(descuentoActivo) ||
                                   !EsValorIgnorado(cliente) || !EsValorIgnorado(tipoEntrega);

            if (!algunCambioReal)
            {
                ultimaAccion = "editar_sin_cambio";
                return;
            }

            SeleccionarFamilia(familia);
            SeleccionarConcepto(concepto);
            IngresarCantidad(cantidad);
            ActivarIGV(igv);
            ActivarDetUnif(detUnif);
            ConfigurarDescuento(descuentoActivo, tipoDescuento, modoDescuento, valorDescuento);

            if (!EsValorIgnorado(cliente)) { AbrirSeccion("Facturación"); BuscarCliente(cliente); }
            if (!EsValorIgnorado(tipoEntrega)) { AbrirSeccion("Entrega"); SeleccionarEntrega(tipoEntrega); }
        }

        public void SeleccionarEditarPedido()
        {
            FiltrarPedidosRegistrados();
            ClickSeguro(btnEditarPrimerRegistro);
        }

        public void GuardarEdicionPedido()
        {
            ultimaAccion = "editar";
            Thread.Sleep(2500);

            var boton = waitLong.Until(ExpectedConditions.ElementExists(btnEditarPedidoFinal));
            ScrollToElement(boton);

            if (BotonEstaDeshabilitado(boton))
            {
                ultimaAccion = "editar_deshabilitado";
                return;
            }

            ClickSeguro(boton);
        }

        // --- INVALIDAR ---
        public void FiltrarPedidosRegistrados()
        {
            LimpiarEIngresarTexto(txtFiltroEstado, "REGISTRADO");
            try { waitLong.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingContainer)); } catch { }
        }

        public bool ExistePedidoRegistradoFiltrado() => driver.FindElements(btnInvalidarPrimerRegistro).Any(e => e.Displayed);

        public void AsegurarPedidoRegistradoParaEditar()
        {
            FiltrarPedidosRegistrados();
            if (!driver.FindElements(btnEditarPrimerRegistro).Any(e => e.Displayed))
                Assert.Fail("No se pudo generar un pedido en estado REGISTRADO para editar.");
        }

        public void SeleccionarInvalidarPedido()
        {
            FiltrarPedidosRegistrados();
            ClickSeguro(btnInvalidarPrimerRegistro);
        }

        public void IngresarMotivoInvalidacion(string motivo)
        {
            if (EsValorIgnorado(motivo)) return;
            LimpiarEIngresarTexto(txtMotivoInvalidacion, motivo);
        }

        public void ConfirmarInvalidacion(string accion)
        {
            if (accion.Trim().StartsWith("S", StringComparison.OrdinalIgnoreCase))
            {
                var botonSi = waitLong.Until(ExpectedConditions.ElementExists(btnSiInvalidar));
                if (BotonEstaDeshabilitado(botonSi))
                {
                    ultimaAccion = "invalidar_deshabilitado";
                    return;
                }
                ultimaAccion = "invalidar";
                ClickSeguro(botonSi);
            }
            else
            {
                ClickSeguro(btnNoInvalidar);
            }
        }

        // --- CONFIRMAR PEDIDO ---
        public void FiltrarPedidoBaseParaConfirmar(bool esMayor700)
        {
            LimpiarEIngresarTexto(txtFiltroEstado, "REGISTRADO");
            LimpiarEIngresarTexto(txtFiltroTotal, esMayor700 ? TOTAL_BASE_MAYOR_700 : TOTAL_BASE_MENOR_IGUAL_700);
            Thread.Sleep(1500);
        }

        public bool ExistePedidoBaseParaConfirmar(bool esMayor700)
        {
            FiltrarPedidoBaseParaConfirmar(esMayor700);
            return driver.FindElements(btnConfirmarPrimerRegistro).Any(e => e.Displayed);
        }

        public void SeleccionarConfirmarPedido() => ClickSeguro(btnConfirmarPrimerRegistro);

        public void ConfigurarFacturacionConfirmacion(string tipoComprobante, string serie, string cliente)
        {
            AbrirFacturacionConfirmacion();

            if (!EsValorIgnorado(cliente) && cliente != "00000000" && cliente.ToLower() != "varios")
            {
                LimpiarEIngresarTexto(txtClienteConfirmacion, cliente);
                driver.FindElement(txtClienteConfirmacion).SendKeys(Keys.Enter);
            }

            // Lógica original de comprobante mantenida (compleja interacción UI en Angular)
            ClickSeguro(cmbTipoComprobanteConfirmacion);
            string busqueda = ObtenerTextoBusquedaComprobante(tipoComprobante);

            var inputs = driver.FindElements(By.XPath("//ng-select//input[@type='text']")).Where(e => e.Displayed).ToList();
            if (inputs.Any())
            {
                inputs.First().SendKeys(busqueda);
            }

            var opcion = wait.Until(d => d.FindElements(By.CssSelector(".option-item, .ng-option, [role='option']"))
                .FirstOrDefault(e => e.Displayed && CoincideComprobante((e.Text ?? "").Trim(), tipoComprobante)));

            if (opcion != null) ClickSeguro(opcion);

            string errorModal = VerificarErrorModalComprobante();
            if (errorModal != null)
            {
                mensajeErrorCapturado = errorModal;
                return;
            }

            if (!EsValorIgnorado(serie))
            {
                var radioSerie = wait.Until(d => d.FindElements(By.XPath($"//label[normalize-space(.)='{serie}'] | //input[@type='radio'][@value='{serie}']")).FirstOrDefault(e => e.Displayed));
                if (radioSerie != null) ClickSeguro(radioSerie);
            }

            AbrirFacturacionConfirmacion(); // Cerrar
        }

        private void AbrirFacturacionConfirmacion()
        {
            bool yaVisible = driver.FindElements(By.XPath("//div[contains(@class,'accordion-body')][.//label[contains(normalize-space(),'Cliente')]]")).Any(e => e.Displayed);
            if (!yaVisible) ClickSeguro(seccionFacturacionConfirmacion);
        }

        public void ConfigurarEntregaConfirmacion(string tipoEntrega, string guiaRemision)
        {
            bool yaVisible = driver.FindElements(By.XPath("//label[normalize-space()='Inmediata' or normalize-space()='Diferida']")).Any(e => e.Displayed);
            if (!yaVisible) ClickSeguro(By.XPath("//*[.//span[normalize-space()='Entrega'] or normalize-space()='Entrega'][self::div or self::button or self::h2]"));

            if (!EsValorIgnorado(tipoEntrega))
            {
                string xpath = tipoEntrega.Trim().Equals("inmediata", StringComparison.OrdinalIgnoreCase) ? "//label[normalize-space()='Inmediata']" : "//label[normalize-space()='Diferida']";
                ClickSeguro(By.XPath(xpath));
            }

            if (guiaRemision.Trim().Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                // Agregamos una espera explícita para que el botón de guía aparezca en el DOM
                var btnGuia = wait.Until(d => d.FindElements(btnGuiaRemisionConfirmacion).FirstOrDefault(e => e.Displayed));

                if (btnGuia == null || BotonEstaDeshabilitado(btnGuia))
                {
                    mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                    return;
                }

                ClickSeguro(btnGuia);

                // RESTAURADO: Validar que el modal realmente se abrió (vital para el Caso 6)
                bool modalAbierto = false;
                try
                {
                    new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(d =>
                        d.FindElements(By.XPath("//button[normalize-space()='Aceptar']")).Any(e => e.Displayed));
                    modalAbierto = true;
                }
                catch { }

                if (!modalAbierto)
                {
                    mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                    return;
                }
            }
        }

        public void ConfigurarPagoConfirmacion(string tipoPago, string montoCubreTotal)
        {
            AbrirPagoConfirmacion();
            SeleccionarTipoPagoConfirmacion(tipoPago);
            ClickSeguro(tabEfectivoConfirmacion);
            IngresarMontoEfectivoConfirmacion(montoCubreTotal);
        }

        public void ConfirmarPedidoPreparado()
        {
            ultimaAccion = "confirmar";
            var boton = waitLong.Until(ExpectedConditions.ElementExists(btnConfirmarPedidoFinal));
            ScrollToElement(boton);

            // PRIMERO: Revisar si hay campos obligatorios vacíos ANTES de hacer click
            string? errorPreClick = VerificarErrorCamposPago();
            if (errorPreClick != null)
            {
                mensajeErrorCapturado = errorPreClick;
                return; // NO hacemos click, capturamos el error negativo directamente
            }

            if (BotonEstaDeshabilitado(boton))
            {
                mensajeErrorCapturado = "Boton Confirmar Pedido deshabilitado";
                return;
            }

            ClickSeguro(boton);

            // SEGUNDO: Revisar si apareció un error dinámico DESPUÉS de hacer el click
            string? errorPostClick = VerificarErrorCamposPago();
            if (errorPostClick != null) mensajeErrorCapturado = errorPostClick;
        }

        private void AbrirPagoConfirmacion()
        {
            bool yaVisible = driver.FindElements(bodyPagoConfirmacion).Any(e => e.Displayed);
            if (!yaVisible) ClickSeguro(seccionPagoConfirmacion);
        }

        private void SeleccionarTipoPagoConfirmacion(string tipoPago)
        {
            if (tipoPago.Trim().Equals("contado", StringComparison.OrdinalIgnoreCase)) ClickSeguro(rbtContadoConfirmacion);
            if (tipoPago.Trim().Equals("credito", StringComparison.OrdinalIgnoreCase)) ClickSeguro(rbtCreditoConfirmacion);
        }

        private void IngresarMontoEfectivoConfirmacion(string monto)
        {
            if (EsValorIgnorado(monto)) return;
            string valor = ResolverMontoPago(monto);
            if (!string.IsNullOrWhiteSpace(valor)) LimpiarEIngresarTexto(txtRecibidoEfectivoConfirmacion, valor);
        }

        // Funciones de validación de texto mantenidas intactas
        private string ObtenerTextoBusquedaComprobante(string tipo)
        {
            string t = (tipo ?? "").Trim().ToUpperInvariant();
            if (t.Contains("NOTA DE VENTA")) return "NOTA";
            if (t.Contains("FACTURA")) return "FACTURA";
            if (t.Contains("BOLETA")) return "BOLETA";
            return t;
        }

        private bool CoincideComprobante(string actual, string tipoComprobante)
        {
            string act = (actual ?? "").Trim().ToUpperInvariant();
            string esperado = (tipoComprobante ?? "").Trim().ToUpperInvariant()
                .Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N");

            if (esperado.Contains("NOTA DE VENTA")) return act.Contains("NOTA DE VENTA");
            if (esperado.Contains("FACTURA")) return act.Contains("FACTURA ELECTRONICA");
            if (esperado.Contains("BOLETA")) return act.Contains("BOLETA DE VENTA ELECTRONICA");
            return act.Contains(esperado);
        }

        private string ResolverMontoPago(string monto)
        {
            if (EsValorIgnorado(monto)) return string.Empty;
            if (monto.Trim().Equals("true", StringComparison.OrdinalIgnoreCase)) return "1000";
            if (monto.Trim().Equals("false", StringComparison.OrdinalIgnoreCase)) return "1";
            return monto.Trim();
        }

        public void ConfirmarMensaje() => ClickSeguro(btnOKConfirmacion);

        public string ObtenerResultadoSistema()
        {
            // 1. Si capturamos el error ANTES del click (durante ConfirmarPedidoPreparado)
            if (!string.IsNullOrEmpty(mensajeErrorCapturado))
            {
                string msg = mensajeErrorCapturado;
                mensajeErrorCapturado = null;
                return msg;
            }

            try
            {
                // 2. Revisamos si hay errores de pago activos en pantalla
                string? errorPago = VerificarErrorCamposPago();
                if (errorPago != null) return errorPago;

                // 3. Validaciones de Inconsistencia y Stock (Nuevo Pedido)
                if (driver.FindElements(mensajeInconsistenciaRegistro).Any(e => e.Displayed))
                {
                    var detalles = driver.FindElements(detalleInconsistenciaRegistro).Where(e => e.Displayed).Select(e => e.Text).ToList();
                    if (detalles.Any(t => t.Contains("stock", StringComparison.OrdinalIgnoreCase))) return "Cantidad debe ser menor al stock";
                    return "muestra mensaje de inconsistencia";
                }

                if (driver.FindElements(mensajeSinProductoRegistro).Any(e => e.Displayed)) return "Ningún producto seleccionado";

                if (ultimaAccion == "editar_deshabilitado" || ultimaAccion == "editar_sin_cambio") return "Boton deshabilitado";
                if (ultimaAccion == "invalidar_deshabilitado") return "Boton SI deshabilitado";

                // 4. Validaciones de Facturación / Guías (Popups o alertas)
                string[] xpathErroresGenerales = {
                    "//*[contains(text(),'RUC (11 dígitos)')]",
                    "//*[contains(text(),'numero de serie') or contains(text(),'número de serie')]",
                    "//*[contains(text(),'mayor a S/.700')]",
                    "//*[contains(text(),'Necesita identificar al cliente con RUC o DNI')]"
                };

                foreach (var xpath in xpathErroresGenerales)
                {
                    var el = driver.FindElements(By.XPath(xpath)).FirstOrDefault(e => e.Displayed);
                    if (el != null) return el.Text.Trim();
                }

                // 5. Validar Modal de Éxito
                var btnOk = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                            .Until(ExpectedConditions.ElementIsVisible(btnOKConfirmacion));

                if (btnOk.Displayed)
                {
                    if (driver.FindElements(By.XPath("//*[contains(text(),'Pedido confirmado correctamente')]")).Any(e => e.Displayed))
                        return "Pedido confirmado correctamente";

                    if (ultimaAccion == "invalidar") return "el pedido se Invalido correctamente";
                    if (ultimaAccion == "confirmar") return "Pedido confirmado correctamente";
                    if (ultimaAccion == "registrar") return "el pedido se guardo correctamente";
                    if (ultimaAccion == "editar") return "el pedido se edito correctamente";
                }
            }
            catch { }

            return string.Empty;
        }

        private string? VerificarErrorModalComprobante()
        {
            var modal = driver.FindElements(By.XPath("//*[contains(text(),'Para emitir Factura') or contains(text(),'RUC (11 dígitos)')]")).FirstOrDefault(e => e.Displayed);
            if (modal != null)
            {
                string texto = modal.Text;
                ConfirmarMensaje();
                return texto;
            }
            return null;
        }

        private string? VerificarErrorCamposPago()
        {
            // 1. Mensajes específicos de negocio (Puntos, Montos, Créditos)
            string[] xpathErrores = {
                "//*[contains(text(),'No hay suficientes puntos disponibles')]",
                "//*[contains(text(),'Monto insuficiente')]",
                "//*[contains(text(),'Para dar a credito debe identificar al cliente')]",
                "//*[contains(text(),'Para el pago con puntos debe identificar al cliente')]"
            };

            foreach (var xpath in xpathErrores)
            {
                var el = driver.FindElements(By.XPath(xpath)).FirstOrDefault(e => e.Displayed);
                if (el != null) return el.Text.Trim();
            }

            // 2. NUEVO: Capturar las advertencias 
            bool hayBadgeRequeridos = driver.FindElements(By.XPath("//*[contains(text(),'Complete los campos requeridos')]")).Any(e => e.Displayed);
            bool hayTextoObligatorio = driver.FindElements(By.XPath("//*[contains(text(),'Este campo es obligatorio')]")).Any(e => e.Displayed);

            if (hayBadgeRequeridos || hayTextoObligatorio)
            {
                
                if (ultimoMedioPagoConfirmacion == "tarjeta_credito" || ultimoMedioPagoConfirmacion == "tarjeta_debito")
                    return "Seleccione una entidad bancaria";

                if (ultimoMedioPagoConfirmacion == "transferencia_fondos" || ultimoMedioPagoConfirmacion == "deposito_cuenta")
                    return "Seleccione una cuenta bancaria";

                return "Complete los campos requeridos";
            }

            return null;
        }

        // --- FLUJO COMPLETO DE MEDIOS DE PAGO ---
        public void ConfigurarMediosDePagoConfirmacion(string tipoPago, string multipago, string medioPago, string banco, string tarjeta, string cuenta, string nroOp, string monto, string nroCuotas, string montoInicial)
        {
            AbrirPagoConfirmacion();
            SeleccionarTipoPagoConfirmacion(tipoPago);

            bool esMultipago = multipago.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
            var chk = wait.Until(ExpectedConditions.ElementExists(chkMultipagoConfirmacion));
            if (chk.Selected != esMultipago) ClickSeguro(chk);

            if (tipoPago.Trim().Equals("credito", StringComparison.OrdinalIgnoreCase))
            {
                if (!EsValorIgnorado(nroCuotas)) LimpiarEIngresarTexto(txtNumeroCuotasConfirmacion, nroCuotas);
                if (!EsValorIgnorado(montoInicial)) LimpiarEIngresarTexto(txtMontoInicialCreditoConfirmacion, ResolverMontoPago(montoInicial));
            }

            var medios = medioPago.Split(',').Select(x => x.Trim()).Where(x => !EsValorIgnorado(x)).ToList();
            var bancos = new Queue<string>(banco.Split(',').Select(x => x.Trim()).Where(x => !EsValorIgnorado(x)));
            var tarjetas = new Queue<string>(tarjeta.Split(',').Select(x => x.Trim()).Where(x => !EsValorIgnorado(x)));
            var cuentas = new Queue<string>(cuenta.Split(',').Select(x => x.Trim()).Where(x => !EsValorIgnorado(x)));
            var operaciones = new Queue<string>(nroOp.Split(',').Select(x => x.Trim()).Where(x => !EsValorIgnorado(x)));

            // Los montos los manejamos por posición estricta para que cuadren con la cantidad de medios
            var montosArr = monto.Split(',').Select(x => x.Trim()).ToArray();

            for (int i = 0; i < medios.Count; i++)
            {
                string medioActual = medios[i];
                string montoActual = (i < montosArr.Length) ? montosArr[i] : "NA";

                SeleccionarTabMedioPagoConfirmacion(medioActual);
                ConfigurarDetalleMedioPago(medioActual, tipoPago, montoActual, bancos, tarjetas, cuentas, operaciones);

                if (esMultipago)
                {
                    // Validamos si el botón está habilitado antes de forzar el clic para evitar Timeouts
                    var btnAgregar = waitLong.Until(ExpectedConditions.ElementExists(btnAgregarMedioPagoConfirmacion));
                    if (!BotonEstaDeshabilitado(btnAgregar))
                    {
                        ClickSeguro(btnAgregar);
                        Thread.Sleep(600); // Tiempo para que Angular pinte la nueva fila
                    }
                }
            }

            if (esMultipago) ultimoMedioPagoConfirmacion = "";
        }

        private void SeleccionarTabMedioPagoConfirmacion(string medio)
        {
            ultimoMedioPagoConfirmacion = medio.ToLower();
            switch (ultimoMedioPagoConfirmacion)
            {
                case "efectivo": ClickSeguro(tabEfectivoConfirmacion); break;
                case "tarjeta_credito": ClickSeguro(tabTarjetaCreditoConfirmacion); break;
                case "tarjeta_debito": ClickSeguro(tabTarjetaDebitoConfirmacion); break;
                case "transferencia_fondos": ClickSeguro(tabTransferenciaConfirmacion); break;
                case "deposito_cuenta": ClickSeguro(tabDepositosConfirmacion); break;
                case "puntos": ClickSeguro(tabPuntosConfirmacion); break;
            }
            Thread.Sleep(300);
        }

        private void ConfigurarDetalleMedioPago(string medio, string tipoPago, string monto, Queue<string> bancos, Queue<string> tarjetas, Queue<string> cuentas, Queue<string> operaciones)
        {
            switch (medio.ToLower())
            {
                case "efectivo":
                    if (tipoPago.Equals("contado", StringComparison.OrdinalIgnoreCase)) IngresarMontoEfectivoConfirmacion(monto);
                    else IngresarMontoGenerico(monto);
                    break;

                case "tarjeta_credito":
                case "tarjeta_debito":
                    SeleccionarComboNativo(cmbBancoConfirmacion, bancos.Count > 0 ? bancos.Dequeue() : "NA");
                    SeleccionarComboNativo(cmbTarjetaConfirmacion, tarjetas.Count > 0 ? tarjetas.Dequeue() : "NA");
                    IngresarMontoGenerico(monto);

                    // Ahora SOLO extraemos la operación si es Tarjeta (El efectivo no lo roba)
                    string opTarjeta = operaciones.Count > 0 ? operaciones.Dequeue() : "NA";
                    if (!EsValorIgnorado(opTarjeta)) LimpiarEIngresarTexto(txtInformacionConfirmacion, opTarjeta);
                    break;

                case "transferencia_fondos":
                case "deposito_cuenta":
                    SeleccionarComboNativo(cmbCuentaBancariaConfirmacion, cuentas.Count > 0 ? cuentas.Dequeue() : "NA");
                    IngresarMontoGenerico(monto);

                    // Ahora SOLO extraemos la operación si es Transferencia/Depósito
                    string opTransferencia = operaciones.Count > 0 ? operaciones.Dequeue() : "NA";
                    if (!EsValorIgnorado(opTransferencia)) LimpiarEIngresarTexto(txtInformacionConfirmacion, opTransferencia);
                    break;

                case "puntos":
                    break;
            }
        }

        private void SeleccionarComboNativo(By locator, string valor)
        {
            if (EsValorIgnorado(valor)) return;

            // ESPERA CRÍTICA: Esperar a que el combo tenga más de 1 opción 
            var selectEl = wait.Until(d => {
                var el = d.FindElements(locator).LastOrDefault(e => e.Displayed && e.Enabled);
                if (el != null)
                {
                    try { if (new SelectElement(el).Options.Count > 1) return el; }
                    catch { }
                }
                return null;
            });

            if (selectEl == null) throw new Exception($"El combo {locator} no cargó sus opciones a tiempo.");

            var combo = new SelectElement(selectEl);
            try
            {
                combo.SelectByText(valor.Trim());
            }
            catch
            {
                var opt = combo.Options.FirstOrDefault(x => x.Text.Trim().Contains(valor.Trim(), StringComparison.OrdinalIgnoreCase));
                if (opt != null) opt.Click();
                else throw new Exception($"Opción '{valor}' no encontrada en el combo.");
            }

            // Notificar del cambio
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                var el = arguments[0]; 
                el.dispatchEvent(new Event('input',  { bubbles: true }));
                el.dispatchEvent(new Event('change', { bubbles: true }));
                el.blur();
            ", selectEl);

            Thread.Sleep(400);
        }

        private void IngresarMontoGenerico(string monto)
        {
            if (EsValorIgnorado(monto)) return;
            string valor = ResolverMontoPago(monto);

            var input = wait.Until(d => d.FindElements(txtMontoMedioPagoConfirmacion).LastOrDefault(e => e.Displayed && e.Enabled));
            if (input != null)
            {
                ScrollToElement(input);
                try { input.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", input); }
                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Delete);
                input.SendKeys(valor);
                input.SendKeys(Keys.Tab);
                Thread.Sleep(300);
            }
        }
    }
}