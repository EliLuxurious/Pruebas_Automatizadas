using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SIGES3_0.Pages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public bool IntentarSeleccionarProductoYCantidad(
    string familia,
    string concepto,
    string cantidad,
    bool permitirStockInsuficiente = false)
        {
            SeleccionarFamilia(familia);

            bool productoSeleccionado = IntentarSeleccionarConceptoLeyendoStock(
                concepto,
                cantidad,
                permitirStockInsuficiente
            );

            if (!productoSeleccionado)
            {
                return false;
            }

            IngresarCantidad(cantidad);
            return true;
        }

        private bool IntentarSeleccionarConceptoLeyendoStock(
    string concepto,
    string cantidad,
    bool permitirStockInsuficiente)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            try
            {
                AbrirComboConcepto();

                IWebElement inputBuscar = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//input[@placeholder='Buscar...' or @placeholder='Buscar']")
                ));

                inputBuscar.Clear();
                inputBuscar.SendKeys(concepto);

                Thread.Sleep(1000);

                By opcionProducto = By.XPath(
                    $"//span[contains(normalize-space(), '{concepto}') and contains(normalize-space(), 'Stock:')]"
                );

                IWebElement opcion = wait.Until(ExpectedConditions.ElementIsVisible(opcionProducto));
                string texto = opcion.Text.Trim();

                int stock = ExtraerStockDesdeTexto(texto);

                if (!permitirStockInsuficiente &&
                    int.TryParse(cantidad, out int cantidadSolicitada) &&
                    stock < cantidadSolicitada)
                {
                    Console.WriteLine($"Stock insuficiente para {concepto}. Stock: {stock}, cantidad: {cantidadSolicitada}.");
                    return false;
                }

                js.ExecuteScript("arguments[0].click();", opcion);
                Thread.Sleep(800);

                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"No se encontró el producto {concepto} en el combo Concepto.");
                return false;
            }
        }

        private int ExtraerStockDesdeTexto(string texto)
        {
            Match match = Regex.Match(texto, @"Stock:\s*(\d+)", RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                Console.WriteLine($"No se pudo leer stock desde el texto: {texto}");
                return 0;
            }

            return int.Parse(match.Groups[1].Value);
        }

        private void AbrirComboConcepto()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            By comboConcepto = By.XPath(
                "(//label[contains(normalize-space(),'Concepto')]/following::div[contains(@class,'select-trigger')])[1]"
            );

            IWebElement combo = wait.Until(ExpectedConditions.ElementExists(comboConcepto));
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", combo);
            Thread.Sleep(500);
            js.ExecuteScript("arguments[0].click();", combo);
            Thread.Sleep(500);
        }





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
            if (EsValorIgnorado(cliente)) return;

            // Si es "varios" o "00000000", escribimos el código genérico. 
            // Si es un RUC/DNI, escribimos el RUC/DNI.
            string valorAEscribir = (cliente.ToLower() == "varios") ? "00000000" : cliente;

            // Tu método LimpiarEIngresarTexto se encarga de borrar fantasmas si los hay
            LimpiarEIngresarTexto(txtCliente, valorAEscribir);
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

            try { boton.Click(); } catch { JsClick(boton); }
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
            if (!string.IsNullOrEmpty(mensajeErrorCapturado)) return;

            AbrirFacturacionConfirmacion();

            if (!EsValorIgnorado(cliente))
            {
                string valorAEscribir = (cliente.ToLower() == "varios") ? "00000000" : cliente;

                LimpiarEIngresarTexto(txtClienteConfirmacion, valorAEscribir);
                driver.FindElement(txtClienteConfirmacion).SendKeys(Keys.Enter);
                Thread.Sleep(500); // Pequeña pausa para que Angular traiga la razón social
            }

            ClickSeguro(cmbTipoComprobanteConfirmacion);
            string busqueda = ObtenerTextoBusquedaComprobante(tipoComprobante);

            var inputs = driver.FindElements(By.XPath("//ng-select//input[@type='text']")).Where(e => e.Displayed).ToList();
            if (inputs.Any()) inputs.First().SendKeys(busqueda);

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

        // AQUÍ ESTÁ LA CORRECCIÓN CLAVE
        //public void ConfigurarEntregaConfirmacion(string tipoEntrega, string guiaRemision)
        //{
        //    if (!string.IsNullOrEmpty(mensajeErrorCapturado)) return;

        //    bool yaVisible = driver.FindElements(By.XPath("//label[normalize-space()='Inmediata' or normalize-space()='Diferida']")).Any(e => e.Displayed);
        //    if (!yaVisible) ClickSeguro(By.XPath("//*[.//span[normalize-space()='Entrega'] or normalize-space()='Entrega'][self::div or self::button or self::h2]"));

        //    if (!EsValorIgnorado(tipoEntrega))
        //    {
        //        string xpath = tipoEntrega.Trim().Equals("inmediata", StringComparison.OrdinalIgnoreCase) ? "//label[normalize-space()='Inmediata']" : "//label[normalize-space()='Diferida']";
        //        ClickSeguro(By.XPath(xpath));
        //    }

        //    if (guiaRemision.Trim().Equals("true", StringComparison.OrdinalIgnoreCase))
        //    {
        //        var btnGuia = wait.Until(d => d.FindElements(btnGuiaRemisionConfirmacion).FirstOrDefault(e => e.Displayed));

        //        if (btnGuia == null)
        //        {
        //            mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
        //            return;
        //        }

        //        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnGuia);
        //        Thread.Sleep(300);

        //        // ¡PROHIBIDO FORZAR! Verificamos los atributos HTML para ver si Angular lo bloqueó visualmente
        //        bool estaDeshabilitadoHTML = !btnGuia.Enabled ||
        //                                     btnGuia.GetAttribute("disabled") != null ||
        //                                     (btnGuia.GetAttribute("class") ?? "").Contains("disabled");

        //        if (estaDeshabilitadoHTML)
        //        {
        //            mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
        //            return;
        //        }

        //        // Usamos Click NATIVO (.Click()). Si Angular bloqueó el botón pero no le puso "disabled",
        //        // el click rebotará y el catch atrapará el error. ¡Nada de JavaScript aquí!
        //        try
        //        {
        //            btnGuia.Click();
        //        }
        //        catch
        //        {
        //            mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
        //            return;
        //        }

        //        Thread.Sleep(1000);

        //        // Validamos la alerta INMEDIATAMENTE
        //        var alertaInmediata = driver.FindElements(By.XPath("//*[contains(text(),'Necesita identificar al cliente con RUC o DNI')]")).FirstOrDefault(e => e.Displayed);
        //        if (alertaInmediata != null)
        //        {
        //            mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
        //            return;
        //        }

        //        // Confirmamos si el modal de verdad se abrió
        //        bool modalAbierto = false;
        //        try
        //        {
        //            new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(d =>
        //                d.FindElements(By.XPath("//*[contains(text(),'Peso Bruto') or contains(text(),'Número de Bultos')]")).Any(e => e.Displayed));
        //            modalAbierto = true;
        //        }
        //        catch { }

        //        if (!modalAbierto)
        //        {
        //            mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
        //            return;
        //        }
        //    }
        //}

        // AQUÍ ESTÁ TU CÓDIGO ORIGINAL RESTAURADO Y ESTABILIZADO
        public void ConfigurarEntregaConfirmacion(string tipoEntrega, string guiaRemision)
        {
            // Escudo para no borrar errores previos
            if (!string.IsNullOrEmpty(mensajeErrorCapturado)) return;

            bool yaVisible = driver.FindElements(By.XPath("//label[normalize-space()='Inmediata' or normalize-space()='Diferida']")).Any(e => e.Displayed);
            if (!yaVisible) ClickSeguro(By.XPath("//*[.//span[normalize-space()='Entrega'] or normalize-space()='Entrega'][self::div or self::button or self::h2]"));

            if (!EsValorIgnorado(tipoEntrega))
            {
                string xpath = tipoEntrega.Trim().Equals("inmediata", StringComparison.OrdinalIgnoreCase) ? "//label[normalize-space()='Inmediata']" : "//label[normalize-space()='Diferida']";
                ClickSeguro(By.XPath(xpath));
            }

            if (guiaRemision.Trim().Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                // Espera vital de 1.5s para que Angular procese si el cliente ingresado tiene RUC o no
                Thread.Sleep(1500);

                var btnGuia = wait.Until(d => d.FindElements(btnGuiaRemisionConfirmacion).FirstOrDefault(e => e.Displayed));

                if (btnGuia == null)
                {
                    mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                    return;
                }

                // 1. FILTRO VISUAL Y HTML: Leemos las clases CSS del botón (como se ve en tu imagen 1)
                string clases = btnGuia.GetAttribute("class") ?? "";
                string attrDisabled = btnGuia.GetAttribute("disabled");
                bool bloqueadoHTML = !btnGuia.Enabled || clases.Contains("disabled") || (attrDisabled != null && attrDisabled != "false");

                if (bloqueadoHTML)
                {
                    // Si el botón está opaco/bloqueado, atrapamos el error inmediatamente y cortamos
                    mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                    return;
                }

                // 2. Intentamos el clic si supuestamente superó la validación
                ClickSeguro(btnGuia);
                Thread.Sleep(1000);

                // 3. EL FILTRO INFALIBLE: Validar si el modal VERDADERAMENTE se abrió.
                // NO BUSCAMOS "Aceptar", buscamos "Peso Bruto" o "Número de Bultos" que son únicos del modal.
                bool modalRealmenteAbierto = false;
                try
                {
                    new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(d =>
                        d.FindElements(By.XPath("//*[contains(text(),'Peso Bruto') or contains(text(),'Número de Bultos')]")).Any(e => e.Displayed));
                    modalRealmenteAbierto = true;
                }
                catch { }

                if (!modalRealmenteAbierto)
                {
                    // Si le dimos clic pero el modal con "Peso Bruto" nunca apareció, Angular lo bloqueó silenciosamente
                    mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                    return;
                }
            }
        }

        public void ConfigurarPagoConfirmacion(string tipoPago, string montoCubreTotal)
        {
            if (!string.IsNullOrEmpty(mensajeErrorCapturado)) return; // ESCUDO ACTIVADO

            AbrirPagoConfirmacion();
            SeleccionarTipoPagoConfirmacion(tipoPago);
            ClickSeguro(tabEfectivoConfirmacion);
            IngresarMontoEfectivoConfirmacion(montoCubreTotal);
        }

        public void ConfirmarPedidoPreparado()
        {
            if (!string.IsNullOrEmpty(mensajeErrorCapturado)) return; // ESCUDO ACTIVADO

            ultimaAccion = "confirmar";
            var boton = waitLong.Until(ExpectedConditions.ElementExists(btnConfirmarPedidoFinal));
            ScrollToElement(boton);

            string? errorPreClick = VerificarErrorCamposPago();
            if (errorPreClick != null)
            {
                mensajeErrorCapturado = errorPreClick;
                return;
            }

            if (BotonEstaDeshabilitado(boton))
            {
                mensajeErrorCapturado = "Boton Confirmar Pedido deshabilitado";
                return;
            }

            ClickSeguro(boton);

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
            // 1. Entregamos el error INTACTO de la Guía
            if (!string.IsNullOrEmpty(mensajeErrorCapturado))
            {
                string msg = mensajeErrorCapturado;
                mensajeErrorCapturado = null;
                return msg;
            }

            try
            {
                // 2. PRIORIDAD MÁXIMA PARA LAS ALERTAS
                string[] xpathErroresGenerales = {
                    "//*[contains(text(),'RUC (11 dígitos)')]",
                    "//*[contains(text(),'numero de serie') or contains(text(),'número de serie')]",
                    "//*[contains(text(),'mayor a S/.700')]",
                    "//*[contains(text(),'Necesita identificar al cliente con RUC o DNI')]"
                };

                foreach (var xpath in xpathErroresGenerales)
                {
                    var el = driver.FindElements(By.XPath(xpath)).FirstOrDefault(e => e.Displayed);
                    if (el != null)
                    {
                        if (el.Text.Contains("Necesita identificar al cliente con RUC o DNI"))
                            return "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                        return el.Text.Trim();
                    }
                }

                // 3. Revisamos los errores genéricos
                string? errorPago = VerificarErrorCamposPago();
                if (errorPago != null) return errorPago;

                if (driver.FindElements(mensajeInconsistenciaRegistro).Any(e => e.Displayed))
                {
                    var detalles = driver.FindElements(detalleInconsistenciaRegistro).Where(e => e.Displayed).Select(e => e.Text).ToList();
                    if (detalles.Any(t => t.Contains("stock", StringComparison.OrdinalIgnoreCase))) return "Cantidad debe ser menor al stock";
                    return "muestra mensaje de inconsistencia";
                }

                if (driver.FindElements(mensajeSinProductoRegistro).Any(e => e.Displayed)) return "Ningún producto seleccionado";

                if (ultimaAccion == "editar_deshabilitado" || ultimaAccion == "editar_sin_cambio") return "Boton deshabilitado";
                if (ultimaAccion == "invalidar_deshabilitado") return "Boton SI deshabilitado";

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

        public void ConfigurarMediosDePagoConfirmacion(string tipoPago, string multipago, string medioPago, string banco, string tarjeta, string cuenta, string nroOp, string monto, string nroCuotas, string montoInicial)
        {
            if (!string.IsNullOrEmpty(mensajeErrorCapturado)) return; // ESCUDO ACTIVADO

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

            var montosArr = monto.Split(',').Select(x => x.Trim()).ToArray();

            for (int i = 0; i < medios.Count; i++)
            {
                string medioActual = medios[i];
                string montoActual = (i < montosArr.Length) ? montosArr[i] : "NA";

                SeleccionarTabMedioPagoConfirmacion(medioActual);
                ConfigurarDetalleMedioPago(medioActual, tipoPago, montoActual, bancos, tarjetas, cuentas, operaciones);

                if (esMultipago)
                {
                    var btnAgregar = waitLong.Until(ExpectedConditions.ElementExists(btnAgregarMedioPagoConfirmacion));
                    if (!BotonEstaDeshabilitado(btnAgregar))
                    {
                        ClickSeguro(btnAgregar);
                        Thread.Sleep(600);
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

                    string opTarjeta = operaciones.Count > 0 ? operaciones.Dequeue() : "NA";
                    if (!EsValorIgnorado(opTarjeta)) LimpiarEIngresarTexto(txtInformacionConfirmacion, opTarjeta);
                    break;

                case "transferencia_fondos":
                case "deposito_cuenta":
                    SeleccionarComboNativo(cmbCuentaBancariaConfirmacion, cuentas.Count > 0 ? cuentas.Dequeue() : "NA");
                    IngresarMontoGenerico(monto);

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