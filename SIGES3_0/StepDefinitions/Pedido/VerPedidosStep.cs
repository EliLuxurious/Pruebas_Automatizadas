using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.Adquisicion;
using SIGES3_0.Pages.Componentes;
using SIGES3_0.Pages.PedidoPage;
using System;
using System.Threading;

namespace SIGES3_0.StepDefinitions.PedidoStep
{
    [Binding]
    public class VerPedidosStep
    {
        private readonly VerPedidosPage verPedidosPage;
        private readonly GuiaRemisionPage guiaRemisionPage;
        private readonly IWebDriver driver;

        public VerPedidosStep(IWebDriver driver)
        {
            this.driver = driver;
            verPedidosPage = new VerPedidosPage(driver);
            guiaRemisionPage = new GuiaRemisionPage(driver);
        }

        [When(@"el usuario prepara producto para pedido con familia '(.*)' concepto '(.*)' cantidad '(.*)' resultado '(.*)'")]
        public void WhenElUsuarioPreparaProductoParaPedido(string familia, string concepto, string cantidad, string resultadoEsperado)
        {
            if (familia.Equals("ninguno", StringComparison.OrdinalIgnoreCase) ||
                concepto.Equals("ninguno", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Caso sin producto: se deja continuar sin seleccionar familia/concepto/cantidad.");
                return;
            }

            bool esperaErrorStock = resultadoEsperado.Contains(
                "Cantidad debe ser menor al stock",
                StringComparison.OrdinalIgnoreCase
            );

            bool productoPreparado = verPedidosPage.IntentarSeleccionarProductoYCantidad(
                familia, concepto, cantidad, permitirStockInsuficiente: esperaErrorStock
            );

            if (productoPreparado) return;

            //string cantidadAComprar = esperaErrorStock ? "100" : CalcularCantidadCompra(cantidad);
            string cantidadAComprar = CalcularCantidadCompra(concepto);

            verPedidosPage.VolverAVerPedidos();

            ComprarProductoEnAdquisicion(concepto, cantidadAComprar);

            verPedidosPage.SeleccionarOpcion("Nuevo Pedido");

            bool productoPreparadoLuegoDeCompra = verPedidosPage.IntentarSeleccionarProductoYCantidad(
                familia, concepto, cantidad, permitirStockInsuficiente: esperaErrorStock
            );

            Assert.That(
                productoPreparadoLuegoDeCompra,
                Is.True,
                $"Después de la adquisición no se pudo preparar el producto {concepto} con cantidad {cantidad}."
            );
        }

        //private string CalcularCantidadCompra(string cantidadPedido)
        //{
        //    if (!int.TryParse(cantidadPedido, out int cantidad)) return "100";
        //    return (cantidad + 30).ToString();
        //}
        private string CalcularCantidadCompra(string concepto)
        {
            return concepto switch
            {
                "7753234003320" => "500", // Coca-Cola
                "7753234003313" => "500", // Inca Kola
                "7751234001115" => "400", // Azúcar Rubia
                _ => "500" // Cantidad por defecto si es otro producto
            };
        }

        private void ComprarProductoEnAdquisicion(string concepto, string cantidad)
        {
            var nuevaAdquisicionPage = new NuevaAdquisicionPage(driver);

            string productoAdquisicion = ObtenerProductoParaAdquisicion(concepto);
            string valorUnitario = ObtenerValorUnitarioParaAdquisicion(concepto);
            string correlativo = DateTime.Now.ToString("HHmmss");

            nuevaAdquisicionPage.NavegarANuevaAdquisicion();

            nuevaAdquisicionPage.ConfigurarDatosFacturacion(
                documento: "FACTURA ELECTRONICA",
                serie: "F001",
                correlativo: correlativo,
                fechaEmision: DateTime.Now.ToString("dd/MM/yyyy"), // CORRECCIÓN: Fecha dinámica para evitar errores
                proveedor: "10759012017",
                infoAdicional: "Stock automático para pedido"
            );

            // CORRECCIÓN: Truco para forzar a Angular a reconocer el proveedor
            try
            {
                var inputProv = driver.FindElement(By.XPath("//input[contains(@placeholder, 'Proveedor') or @id='proveedor']"));
                inputProv.SendKeys(Keys.Enter);
                Thread.Sleep(1500);
            }
            catch { }

            nuevaAdquisicionPage.SeleccionarTipoEntrega("Inmediata");
            nuevaAdquisicionPage.ConfigurarDatosEntrega(rol: "Item Comercial", establecimiento: "RECSA - CENTRAL", almacen: "CENTRO COMERCIAL CENTRAL");
            nuevaAdquisicionPage.AgregarProducto(producto: productoAdquisicion, cantidad: cantidad, valorUnitario: valorUnitario);
            nuevaAdquisicionPage.AbrirSeccionPago();
            nuevaAdquisicionPage.SeleccionarTipoPago("Contado");
            nuevaAdquisicionPage.ConfigurarMedioPago("Efectivo", "NINGUNO");

            nuevaAdquisicionPage.ClicGuardarAdquisicion("SavePurchase");

            string mensaje = "";
            try { mensaje = nuevaAdquisicionPage.ObtenerMensajeConfirmacion(); } catch { }

            Assert.That(
                mensaje,
                Does.Contain("Se registró correctamente.").IgnoreCase,
                $"No se pudo registrar adquisición para {concepto}. Mensaje obtenido: {mensaje}"
            );

            verPedidosPage.SeleccionarOpcion("Pedidos");
            verPedidosPage.SeleccionarOpcion("Ver Pedidos");
        }

        private string ObtenerProductoParaAdquisicion(string concepto)
        {
            return concepto switch
            {
                "7753234003320" => "7753234003320|Coca-Cola Gaseosa Botella 1.5L",
                "7753234003313" => "7753234003313|Inca Kola Gaseosa Botella 1.5L",
                "7751234001115" => "7751234001115|Azúcar Rubia",
                _ => throw new ArgumentException($"No se configuró el producto para adquisición: {concepto}")
            };
        }

        private string ObtenerValorUnitarioParaAdquisicion(string concepto)
        {
            return concepto switch
            {
                "7753234003320" => "6.9",
                "7753234003313" => "7.1",
                "7751234001115" => "3.2",
                _ => "6.9"
            };
        }

        [When(@"el usuario accede al módulo '(.*)'")]
        public void WhenElUsuarioAccedeAlModulo(string modulo) => verPedidosPage.SeleccionarOpcion(modulo);

        [When(@"el usuario accede al submodulo '(.*)'")]
        public void WhenElUsuarioAccedeAlSubmodulo(string submodulo) => verPedidosPage.SeleccionarOpcion(submodulo);

        [When(@"el usuario selecciona la opciOn '(.*)'")]
        public void WhenElUsuarioSeleccionaLaOpcion(string opcion)
        {
            string opt = opcion.Trim().ToLower();
            if (opt == "editar pedido") verPedidosPage.SeleccionarEditarPedido();
            else if (opt == "invalidar pedido") verPedidosPage.SeleccionarInvalidarPedido();
            else if (opt == "confirmar pedido") verPedidosPage.SeleccionarConfirmarPedido();
            else verPedidosPage.SeleccionarOpcion(opcion);
        }

        [When(@"el usuario selecciona la familia '(.*)'")]
        public void WhenElUsuarioSeleccionaLaFamilia(string familia) => verPedidosPage.SeleccionarFamilia(familia);

        [When(@"usuario selecciona el concepto '(.*)'")]
        public void WhenElUsuarioSeleccionaElConcepto(string concepto) => verPedidosPage.SeleccionarConcepto(concepto);

        [When(@"usuario ingresa la cantidad '(.*)'")]
        public void WhenElUsuarioIngresaLaCantidad(string cantidad) => verPedidosPage.IngresarCantidad(cantidad);

        [When(@"el usuario activa IGV '(.*)'")]
        public void WhenElUsuarioActivaIGV(string igv) => verPedidosPage.ActivarIGV(igv);

        [When(@"el usuario activa DET.UNIF '(.*)'")]
        public void WhenElUsuarioActivaDETUNIF(string detUnif) => verPedidosPage.ActivarDetUnif(detUnif);

        [When(@"el usuario configura descuento '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraDescuento(string descuento, string tipoDescuento, string modoDescuento, string valorDescuento)
            => verPedidosPage.ConfigurarDescuento(descuento, tipoDescuento, modoDescuento, valorDescuento);

        [When("el usuario abre la sección {string}")]
        public void WhenElUsuarioAbreLaSeccion(string seccion) => verPedidosPage.AbrirSeccion(seccion);

        [When(@"el usuario busca el cliente '(.*)'")]
        public void WhenElUsuarioBuscaElCliente(string cliente) => verPedidosPage.BuscarCliente(cliente);

        [When(@"el usuario selecciona tipo de entrega '(.*)'")]
        public void WhenElUsuarioSeleccionaTipoDeEntrega(string tipoEntrega) => verPedidosPage.SeleccionarEntrega(tipoEntrega);

        [When(@"el usuario registra el pedido")]
        public void WhenElUsuarioRegistraElPedido() => verPedidosPage.RegistrarPedido();

        [Given(@"existe un pedido en estado registrado para editar")]
        public void GivenExisteUnPedidoEnEstadoRegistradoParaEditar() => verPedidosPage.AsegurarPedidoRegistradoParaEditar();

        [When(@"el usuario actualiza el pedido con familia '(.*)' concepto '(.*)' cantidad '(.*)' igv '(.*)' detUnif '(.*)' descuento '(.*)' tipoDescuento '(.*)' modoDescuento '(.*)' valorDescuento '(.*)' cliente '(.*)' entrega '(.*)'")]
        public void WhenElUsuarioActualizaElPedidoCon(string familia, string concepto, string cantidad, string igv, string detUnif, string descuentoActivo, string tipoDescuento, string modoDescuento, string valorDescuento, string cliente, string tipoEntrega)
            => verPedidosPage.ActualizarPedido(familia, concepto, cantidad, igv, detUnif, descuentoActivo, tipoDescuento, modoDescuento, valorDescuento, cliente, tipoEntrega);

        [When(@"el usuario guarda la edición del pedido")]
        public void WhenElUsuarioGuardaLaEdicionDelPedido() => verPedidosPage.GuardarEdicionPedido();


        // ══════════════════════════════════════════════════════════════════════════════════════════
        // GIVEN: GENERADORES DE PEDIDOS CON GARANTÍA DE STOCK (Para Invalidar y Confirmar)
        // ══════════════════════════════════════════════════════════════════════════════════════════

        [Given(@"existe un pedido en estado registrado para invalidar con familia '(.*)' concepto '(.*)' cantidad '(.*)' cliente '(.*)' entrega '(.*)'")]
        public void GivenExisteUnPedidoParaInvalidar(string familia, string concepto, string cantidad, string cliente, string entrega)
        {
            verPedidosPage.FiltrarPedidosRegistrados();
            if (!verPedidosPage.ExistePedidoRegistradoFiltrado())
            {
                GenerarPedidoGarantizandoStock(familia, concepto, cantidad, cliente, entrega);
                verPedidosPage.FiltrarPedidosRegistrados();
            }
        }

        [Given(@"existe un pedido base registrado para confirmar con total mayor a 700 '(.*)' familia '(.*)' concepto '(.*)' cantidad '(.*)' cliente '(.*)' entrega '(.*)'")]
        public void GivenExisteUnPedidoBaseParaConfirmar(string totalMayor700, string familia, string concepto, string cantidad, string cliente, string entrega)
        {
            bool esMayor700 = totalMayor700.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
            verPedidosPage.FiltrarPedidoBaseParaConfirmar(esMayor700);

            if (!verPedidosPage.ExistePedidoBaseParaConfirmar(esMayor700))
            {
                GenerarPedidoGarantizandoStock(familia, concepto, cantidad, cliente, entrega);
                verPedidosPage.FiltrarPedidoBaseParaConfirmar(esMayor700);
            }
        }

        private void GenerarPedidoGarantizandoStock(string familia, string concepto, string cantidad, string cliente, string entrega)
        {
            verPedidosPage.SeleccionarOpcion("Nuevo Pedido");
            verPedidosPage.ActualizarPedido(familia, concepto, cantidad, "false", "false", "false", "NA", "NA", "0", cliente, entrega);
            verPedidosPage.RegistrarPedido();

            string resultado = verPedidosPage.ObtenerResultadoSistema();

            if (resultado.Contains("menor al stock", StringComparison.OrdinalIgnoreCase) ||
                resultado.Contains("inconsistencia", StringComparison.OrdinalIgnoreCase))
            {
                try { verPedidosPage.ConfirmarMensaje(); } catch { }

                //int cantidadSegura = int.Parse(cantidad) + 50;
                //ComprarProductoEnAdquisicion(concepto, cantidadSegura.ToString());
                string cantidadSegura = CalcularCantidadCompra(concepto);
                ComprarProductoEnAdquisicion(concepto, cantidadSegura);

                verPedidosPage.SeleccionarOpcion("Nuevo Pedido");
                verPedidosPage.ActualizarPedido(familia, concepto, cantidad, "false", "false", "false", "NA", "NA", "0", cliente, entrega);
                verPedidosPage.RegistrarPedido();
            }

            try { verPedidosPage.ConfirmarMensaje(); } catch { }
            try { verPedidosPage.VolverAVerPedidos(); } catch { }
        }

        // ══════════════════════════════════════════════════════════════════════════════════════════

        [When(@"el usuario ingresa el motivo '(.*)'")]
        public void WhenElUsuarioIngresaElMotivo(string motivo) => verPedidosPage.IngresarMotivoInvalidacion(motivo);

        [When(@"el usuario confirma '(.*)'")]
        public void WhenElUsuarioConfirma(string accion) => verPedidosPage.ConfirmarInvalidacion(accion);

        [When(@"el usuario configura la facturacion '(.*)' '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraLaFacturacion(string tipoComprobante, string serie, string cliente)
            => verPedidosPage.ConfigurarFacturacionConfirmacion(tipoComprobante, serie, cliente);

        [When(@"el usuario configura la entrega '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraLaEntrega(string tipoEntrega, string guiaRemision)
        {
            if (!verPedidosPage.HayErrorCapturado()) verPedidosPage.ConfigurarEntregaConfirmacion(tipoEntrega, guiaRemision);
        }

        [When(@"el usuario completa la guia de remision '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void WhenElUsuarioCompletaLaGuiaDeRemision(string guiaRemision, string fechaTraslado, string pesoBruto, string cantidadBultos, string tipoTransporte, string transportistaRuc, string numeroLicencia, string numeroPlaca, string direccionOrigen, string detalleOrigen, string direccionDestino, string detalleDestino)
        {
            if (verPedidosPage.HayErrorCapturado() || !guiaRemision.Trim().Equals("true", StringComparison.OrdinalIgnoreCase)) return;

            guiaRemisionPage.ExpandirDatosGenerales();
            guiaRemisionPage.ValidarDestinatarioAutocompletado();
            guiaRemisionPage.IngresarFechaTraslado(fechaTraslado);
            guiaRemisionPage.IngresarPesoBruto(pesoBruto);
            guiaRemisionPage.IngresarNumeroBultos(cantidadBultos);
            guiaRemisionPage.ExpandirDatosTransporte();
            guiaRemisionPage.SeleccionarTipoTransporte(tipoTransporte);
            guiaRemisionPage.IngresarTransportistaPublico(transportistaRuc);
            guiaRemisionPage.IngresarNumeroLicencia(numeroLicencia);
            guiaRemisionPage.IngresarNumeroPlaca(numeroPlaca);
            guiaRemisionPage.SeleccionarDireccionOrigen(direccionOrigen);
            guiaRemisionPage.IngresarDetalleOrigen(detalleOrigen);
            guiaRemisionPage.SeleccionarDireccionDestino(direccionDestino);
            guiaRemisionPage.IngresarDetalleDestino(detalleDestino);
            guiaRemisionPage.GuardarGuia();
        }

        [When(@"el usuario configura los medios de pago '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)'")]
        public void WhenElUsuarioConfiguraLosMediosDePago(string tipoPago, string multipago, string medioPago, string banco, string tarjeta, string cuentaBancaria, string nroOperacion, string montoPorMedio, string nroCuotas, string montoInicialCredito)
        {
            if (!verPedidosPage.HayErrorCapturado())
                verPedidosPage.ConfigurarMediosDePagoConfirmacion(tipoPago, multipago, medioPago, banco, tarjeta, cuentaBancaria, nroOperacion, montoPorMedio, nroCuotas, montoInicialCredito);
        }

        [When(@"el usuario configura el pago '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraElPago(string tipoPago, string montoCubreTotal)
        {
            if (!verPedidosPage.HayErrorCapturado()) verPedidosPage.ConfigurarPagoConfirmacion(tipoPago, montoCubreTotal);
        }

        [When(@"el usuario confirma el pedido preparado")]
        public void WhenElUsuarioConfirmaElPedidoPreparado()
        {
            if (!verPedidosPage.HayErrorCapturado()) verPedidosPage.ConfirmarPedidoPreparado();
        }

        [Then(@"el sistema valida el resultado del pedido '(.*)'")]
        public void ThenElSistemaValidaElResultadoDelPedido(string resultadoEsperado)
        {
            string resultado = verPedidosPage.ObtenerResultadoSistema()?.Trim() ?? string.Empty;
            string esperado = resultadoEsperado?.Trim() ?? string.Empty;

            Assert.That(resultado, Is.Not.Empty, "El sistema no devolvió ningún mensaje o resultado visible.");
            Assert.That(resultado, Does.Contain(esperado).IgnoreCase, $"Resultado esperado: {resultadoEsperado}. Resultado obtenido: {resultado}");
        }
    }
}