using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SIGES3_0.Pages.Componentes;
using SIGES3_0.Pages.PedidoPage;
using System;

namespace SIGES3_0.StepDefinitions.PedidoStep
{
    [Binding]
    public class VerPedidosStep
    {
        private readonly IWebDriver driver;
        private readonly VerPedidosPage verPedidosPage;
        private readonly GuiaRemisionPage guiaRemisionPage;



        public VerPedidosStep(IWebDriver driver)
        {
            this.driver = driver;
            verPedidosPage = new VerPedidosPage(driver);
            guiaRemisionPage = new GuiaRemisionPage(driver); // ← agregar
        }

        // -------------------------
        // NAVEGACIÓN
        // ------------------------

        [When(@"el usuario accede al módulo '(.*)'")]
        public void WhenElUsuarioAccedeAlModulo(string modulo)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            var elemento = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//span[normalize-space()='{modulo}']/ancestor::a")
                )
            );

            elemento.Click();
        }

        [When(@"el usuario accede al submodulo '(.*)'")]
        public void WhenElUsuarioAccedeAlSubmodulo(string submodulo)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var elemento = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//span[contains(text(),'{submodulo}')]")
                )
            );

            elemento.Click();
        }

        [When(@"el usuario selecciona la opci[oó]n '(.*)'")]
        public void WhenElUsuarioSeleccionaLaOpcion(string opcion)
        {
            if (opcion.Trim().Equals("Editar pedido", StringComparison.OrdinalIgnoreCase))
            {
                verPedidosPage.SeleccionarEditarPedido();
                return;
            }

            if (opcion.Trim().Equals("Invalidar pedido", StringComparison.OrdinalIgnoreCase))
            {
                verPedidosPage.SeleccionarInvalidarPedido();
                return;
            }

            if (opcion.Trim().Equals("Confirmar pedido", StringComparison.OrdinalIgnoreCase))
            {
                verPedidosPage.SeleccionarConfirmarPedido();
                return;
            }

            verPedidosPage.SeleccionarOpcion(opcion);
        }



        // -------------------------
        // PRODUCTO
        // -------------------------
        [When(@"el usuario selecciona la familia '(.*)'")]
        public void WhenElUsuarioSeleccionaLaFamilia(string familia)
        {
            if (familia == "NO_CAMBIO")
            {
                Console.WriteLine("[EditarCotizacion] Familia = NO_CAMBIO, no se modifica.");
                return;
            }

            verPedidosPage.SeleccionarFamilia(familia);
        }

        [When(@"el usuario selecciona el concepto '(.*)'")]
        public void WhenElUsuarioSeleccionaElConcepto(string concepto)
        {
            if (concepto == "NO_CAMBIO")
            {
                Console.WriteLine("[EditarCotizacion] Concepto = NO_CAMBIO, no se modifica.");
                return;
            }

            verPedidosPage.SeleccionarConcepto(concepto);
        }

        [When(@"el usuario ingresa la cantidad '(.*)'")]
        public void WhenElUsuarioIngresaLaCantidad(string cantidad)
        {
            if (cantidad == "NO_CAMBIO")
            {
                Console.WriteLine("[EditarCotizacion] Cantidad = NO_CAMBIO, no se modifica.");
                return;
            }

            verPedidosPage.IngresarCantidad(cantidad);
        }

        [When(@"el usuario activa IGV '(.*)'")]
        public void WhenElUsuarioActivaIGV(string igv)
        {
            if (igv == "NO_CAMBIO")
            {
                Console.WriteLine("[EditarCotizacion] IGV = NO_CAMBIO, no se modifica.");
                return;
            }

            verPedidosPage.ActivarIGV(igv);
        }


        //[When(@"el usuario selecciona la familia '(.*)'")]
        //public void WhenElUsuarioSeleccionaLaFamilia(string familia)
        //{
        //    verPedidosPage.SeleccionarFamilia(familia);
        //}

        //[When(@"el usuario selecciona el concepto '(.*)'")]
        //public void WhenElUsuarioSeleccionaElConcepto(string concepto)
        //{
        //    verPedidosPage.SeleccionarConcepto(concepto);
        //}

        //[When(@"el usuario ingresa la cantidad '(.*)'")]
        //public void WhenElUsuarioIngresaLaCantidad(string cantidad)
        //{
        //    verPedidosPage.IngresarCantidad(cantidad);
        //}

        //// -------------------------
        //// OPCIONES
        //// -------------------------

        //[When(@"el usuario activa IGV '(.*)'")]
        //public void WhenElUsuarioActivaIGV(string igv)
        //{
        //    verPedidosPage.ActivarIGV(igv);
        //}

        [When(@"el usuario activa DET.UNIF '(.*)'")]
        public void WhenElUsuarioActivaDETUNIF(string detUnif)
        {
            verPedidosPage.ActivarDetUnif(detUnif);
        }

        // -------------------------
        // DESCUENTO
        // -------------------------

        //[When(@"el usuario configura descuento '(.*)' '(.*)' '(.*)' '(.*)'")]
        //public void WhenElUsuarioConfiguraDescuento(string activo, string tipo, string modo, string valor)
        //{
        //    verPedidosPage.ConfigurarDescuento(activo, tipo, modo, valor);
        //}
        [When(@"el usuario configura descuento '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraDescuento(string descuento, string tipoDescuento, string modoDescuento, string valorDescuento)
        {
            if (descuento == "NO_CAMBIO")
            {
                Console.WriteLine("[EditarCotizacion] Descuento = NO_CAMBIO, no se modifica.");
                return;
            }

            verPedidosPage.ConfigurarDescuento(descuento, tipoDescuento, modoDescuento, valorDescuento);
        }

        [When("el usuario abre la sección {string}")]
        public void WhenElUsuarioAbreLaSeccion(string seccion)
        {
            verPedidosPage.AbrirSeccion(seccion);
        }


        [When(@"el usuario busca el cliente '(.*)'")]
        public void WhenElUsuarioBuscaElCliente(string cliente)
        {
            verPedidosPage.BuscarCliente(cliente);
        }

        // -------------------------
        // ENTREGA
        // -------------------------

        [When(@"el usuario selecciona tipo de entrega '(.*)'")]
        public void WhenElUsuarioSeleccionaTipoDeEntrega(string tipoEntrega)
        {
            verPedidosPage.SeleccionarEntrega(tipoEntrega);
        }

        // -------------------------
        // REGISTRO
        // -------------------------

        [When(@"el usuario registra el pedido")]
        public void WhenElUsuarioRegistraElPedido()
        {
            verPedidosPage.RegistrarPedido();
        }

        //EDITAR PEDIDO
        [Given(@"existe un pedido en estado registrado para editar")]
        public void GivenExisteUnPedidoEnEstadoRegistradoParaEditar()
        {
            verPedidosPage.AsegurarPedidoRegistradoParaEditar();
        }

        [When(@"el usuario actualiza el pedido con familia '(.*)' concepto '(.*)' cantidad '(.*)' igv '(.*)' detUnif '(.*)' descuento '(.*)' tipoDescuento '(.*)' modoDescuento '(.*)' valorDescuento '(.*)' cliente '(.*)' entrega '(.*)'")]
        public void WhenElUsuarioActualizaElPedidoCon(
            string familia,
            string concepto,
            string cantidad,
            string igv,
            string detUnif,
            string descuentoActivo,
            string tipoDescuento,
            string modoDescuento,
            string valorDescuento,
            string cliente,
            string tipoEntrega)
        {
            verPedidosPage.ActualizarPedido(
                familia, concepto, cantidad, igv, detUnif,
                descuentoActivo, tipoDescuento, modoDescuento, valorDescuento,
                cliente, tipoEntrega
            );
        }

        [When(@"el usuario guarda la edición del pedido")]
        public void WhenElUsuarioGuardaLaEdicionDelPedido()
        {
            verPedidosPage.GuardarEdicionPedido();
        }


        // -------------------------
        // INVALIDACIÓN
        // -------------------------
        [Given(@"existe un pedido en estado registrado para invalidar con familia '(.*)' concepto '(.*)' cantidad '(.*)' cliente '(.*)' entrega '(.*)'")]
        public void GivenExisteUnPedidoParaInvalidar(
        string familia, string concepto, string cantidad, string cliente, string entrega)
        {
            // 1. Buscar primero
            verPedidosPage.FiltrarPedidosRegistrados();
            if (verPedidosPage.ExistePedidoRegistradoFiltrado()) return;

            // 2. Solo si no existe, registrar usando los métodos públicos del page
            verPedidosPage.SeleccionarOpcion("Nuevo Pedido");
            verPedidosPage.SeleccionarFamilia(familia);
            verPedidosPage.SeleccionarConcepto(concepto);
            verPedidosPage.IngresarCantidad(cantidad);
            verPedidosPage.ActivarIGV("false");
            verPedidosPage.ActivarDetUnif("false");
            verPedidosPage.ConfigurarDescuento("false", "NA", "NA", "0");
            verPedidosPage.AbrirSeccion("Facturación");
            verPedidosPage.BuscarCliente(cliente);
            verPedidosPage.AbrirSeccion("Entrega");
            verPedidosPage.SeleccionarEntrega(entrega);
            verPedidosPage.RegistrarPedido();
            verPedidosPage.ConfirmarMensaje();
            verPedidosPage.VolverAVerPedidos();
            verPedidosPage.FiltrarPedidosRegistrados();
        }


        [When(@"el usuario ingresa el motivo '(.*)'")]
        public void WhenElUsuarioIngresaElMotivo(string motivo)
        {
            verPedidosPage.IngresarMotivoInvalidacion(motivo);
        }

        [When(@"el usuario confirma '(.*)'")]
        public void WhenElUsuarioConfirma(string accion)
        {
            verPedidosPage.ConfirmarInvalidacion(accion);
        }

        //CONFIRMAR PEDIDO
        [Given(@"existe un pedido base registrado para confirmar con total mayor a 700 '(.*)' familia '(.*)' concepto '(.*)' cantidad '(.*)' cliente '(.*)' entrega '(.*)'")]
        public void GivenExisteUnPedidoBaseParaConfirmar(
    string totalMayor700, string familia, string concepto,
    string cantidad, string cliente, string entrega)
        {
            bool esMayor700 = totalMayor700.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);

            // 1. Buscar primero
            verPedidosPage.FiltrarPedidoBaseParaConfirmar(esMayor700);
            if (verPedidosPage.ExistePedidoBaseParaConfirmar(esMayor700)) return;

            // 2. Solo si no existe, registrar usando los métodos públicos del page
            verPedidosPage.SeleccionarOpcion("Nuevo Pedido");
            verPedidosPage.SeleccionarFamilia(familia);
            verPedidosPage.SeleccionarConcepto(concepto);
            verPedidosPage.IngresarCantidad(cantidad);
            verPedidosPage.ActivarIGV("false");
            verPedidosPage.ActivarDetUnif("false");
            verPedidosPage.ConfigurarDescuento("false", "NA", "NA", "0");
            verPedidosPage.AbrirSeccion("Facturación");
            verPedidosPage.BuscarCliente(cliente);
            verPedidosPage.AbrirSeccion("Entrega");
            verPedidosPage.SeleccionarEntrega(entrega);
            verPedidosPage.RegistrarPedido();
            verPedidosPage.ConfirmarMensaje();
            verPedidosPage.VolverAVerPedidos();
            verPedidosPage.FiltrarPedidoBaseParaConfirmar(esMayor700);
        }

        //[When(@"el usuario configura la facturacion '(.*)' '(.*)' '(.*)'")]
        //public void WhenElUsuarioConfiguraLaFacturacion(string tipoComprobante, string serie, string cliente)
        //{
        //    verPedidosPage.ConfigurarFacturacionConfirmacion(tipoComprobante, serie, cliente);
        //}

        [When(@"el usuario configura la facturacion '(.*)' '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraLaFacturacion(string tipoComprobante, string serie, string cliente)
        {
            verPedidosPage.ConfigurarFacturacionConfirmacion(tipoComprobante, serie, cliente);
        }

        //[When(@"el usuario configura la entrega '(.*)' '(.*)'")]
        //public void WhenElUsuarioConfiguraLaEntrega(string tipoEntrega, string guiaRemision)
        //{
        //    verPedidosPage.ConfigurarEntregaConfirmacion(tipoEntrega, guiaRemision);
        //}
        [When(@"el usuario configura la entrega '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraLaEntrega(string tipoEntrega, string guiaRemision)
        {
            if (verPedidosPage.HayErrorCapturado()) return;
            verPedidosPage.ConfigurarEntregaConfirmacion(tipoEntrega, guiaRemision);
        }


        //GUIA DE REMISION
        [When(@"el usuario completa la guia de remision '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void WhenElUsuarioCompletaLaGuiaDeRemision(
        string guiaRemision,
        string fechaTraslado,
        string pesoBruto,
        string cantidadBultos,
        string tipoTransporte,
        string transportistaRuc,
        string dniConductor,
        string numeroLicencia,
        string numeroPlaca,
        string direccionOrigen,
        string direccionDestino)
            
        {
            if (verPedidosPage.HayErrorCapturado()) return;

            // Si no aplica guía, no hacer nada
            if (!guiaRemision.Trim().Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("[GuiaRemision] No aplica, se omite.");
                return;
            }
        

            guiaRemisionPage.ExpandirDatosGenerales();
            guiaRemisionPage.ValidarDestinatarioAutocompletado();
            guiaRemisionPage.IngresarFechaTraslado(fechaTraslado);
            guiaRemisionPage.IngresarPesoBruto(pesoBruto);
            guiaRemisionPage.IngresarNumeroBultos(cantidadBultos);

            guiaRemisionPage.ExpandirDatosTransporte();
            guiaRemisionPage.SeleccionarTipoTransporte(tipoTransporte);
            guiaRemisionPage.IngresarTransportistaPublico(transportistaRuc);
            guiaRemisionPage.IngresarConductorPrivado(dniConductor);
            guiaRemisionPage.IngresarNumeroLicencia(numeroLicencia);
            guiaRemisionPage.IngresarNumeroPlaca(numeroPlaca);
            guiaRemisionPage.SeleccionarDireccionOrigen(direccionOrigen);
            guiaRemisionPage.SeleccionarDireccionDestino(direccionDestino);

            guiaRemisionPage.GuardarGuia();
        }

   

        // MEDIOS DE PAGO
        [When(@"el usuario configura los medios de pago '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraLosMediosDePago(
        string tipoPago,
        string multipago,
        string medioPago,
        string banco,
        string tarjeta,
        string cuentaBancaria,
        string nroOperacion,
        string monto,
        string nroCuotas)
        {
            if (verPedidosPage.HayErrorCapturado()) return;
            verPedidosPage.ConfigurarMediosDePagoConfirmacion(
                tipoPago,
                multipago,
                medioPago,
                banco,
                tarjeta,
                cuentaBancaria,
                nroOperacion,
                monto,
                nroCuotas
            );
        }

        [When(@"el usuario configura el pago '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraElPago(string tipoPago, string montoCubreTotal)
        {
            if (verPedidosPage.HayErrorCapturado()) return;
            verPedidosPage.ConfigurarPagoConfirmacion(tipoPago, montoCubreTotal);
        }

        [When(@"el usuario confirma el pedido preparado")]
        public void WhenElUsuarioConfirmaElPedidoPreparado()
        {
            if (verPedidosPage.HayErrorCapturado()) return;
            verPedidosPage.ConfirmarPedidoPreparado();
        }


        [Then(@"el sistema valida el resultado del pedido '(.*)'")]
        public void ThenElSistemaValidaElResultadoDelPedido(string resultadoEsperado)
        {
            string resultado = verPedidosPage.ObtenerResultadoSistema()?.Trim() ?? string.Empty;
            string esperado = resultadoEsperado?.Trim() ?? string.Empty;

            Assert.That(resultado, Is.Not.Empty, "El sistema no devolvió ningún mensaje o resultado visible.");

            Assert.That(
                resultado,
                Does.Contain(esperado).IgnoreCase,
                $"Resultado esperado: {resultadoEsperado}. Resultado obtenido: {resultado}"
            );
        }


    }
}
