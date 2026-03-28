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
    public class VerPedidosStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly VerPedidosPage verPedidosPage;
        private readonly GuiaRemisionPage guiaRemisionPage;



        public VerPedidosStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            verPedidosPage = new VerPedidosPage(driver);
            guiaRemisionPage = new GuiaRemisionPage(driver); // ← agregar
        }

        [When(@"el usuario selecciona la opci[oó]n '(.*)'")]
        public void WhenElUsuarioSeleccionaLaOpcion(string opcion)
        {
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
            verPedidosPage.SeleccionarFamilia(familia);
        }

        [When(@"el usuario selecciona el concepto '(.*)'")]
        public void WhenElUsuarioSeleccionaElConcepto(string concepto)
        {
            verPedidosPage.SeleccionarConcepto(concepto);
        }

        [When(@"el usuario ingresa la cantidad '(.*)'")]
        public void WhenElUsuarioIngresaLaCantidad(string cantidad)
        {
            verPedidosPage.IngresarCantidad(cantidad);
        }

        // -------------------------
        // OPCIONES
        // -------------------------

        [When(@"el usuario activa IGV '(.*)'")]
        public void WhenElUsuarioActivaIGV(string igv)
        {
            verPedidosPage.ActivarIGV(igv);
        }

        [When(@"el usuario activa DET.UNIF '(.*)'")]
        public void WhenElUsuarioActivaDETUNIF(string detUnif)
        {
            verPedidosPage.ActivarDetUnif(detUnif);
        }

        // -------------------------
        // DESCUENTO
        // -------------------------

        [When(@"el usuario configura descuento '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraDescuento(string activo, string tipo, string modo, string valor)
        {
            verPedidosPage.ConfigurarDescuento(activo, tipo, modo, valor);
        }

        // -------------------------
        // CLIENTE
        // -------------------------

        /*[When(@"el usuario abre la sección '(.*)'")]
        public void WhenElUsuarioAbreLaSeccion(string seccion)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            var elemento = wait.Until(d =>
                d.FindElement(By.XPath($"//*[contains(text(),'{seccion}')]/ancestor::button"))
            );

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", elemento);

            Thread.Sleep(500);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", elemento);
        }*/

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

        // Validar q exista un pedido en estado registrado 
        [Given(@"existe un pedido en estado registrado para invalidar")]
        public void GivenExisteUnPedidoEnEstadoRegistradoParaInvalidar()
        {
            verPedidosPage.AsegurarPedidoRegistradoParaInvalidar();
        }


        // -------------------------
        // INVALIDACIÓN
        // -------------------------

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
        [Given(@"existe un pedido base registrado para confirmar con total mayor a 700 '(.*)'")]
        public void GivenExisteUnPedidoBaseRegistradoParaConfirmarConTotalMayorA700(string totalMayor700)
        {
            verPedidosPage.AsegurarPedidoBaseParaConfirmar(totalMayor700);
        }

        [When(@"el usuario configura la facturacion '(.*)' '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraLaFacturacion(string tipoComprobante, string serie, string cliente)
        {
            verPedidosPage.ConfigurarFacturacionConfirmacion(tipoComprobante, serie, cliente);
        }

        [When(@"el usuario configura la entrega '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraLaEntrega(string tipoEntrega, string guiaRemision)
        {
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

        [When(@"el usuario configura el pago '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraElPago(string tipoPago, string montoCubreTotal)
        {
            verPedidosPage.ConfigurarPagoConfirmacion(tipoPago, montoCubreTotal);
        }

        [When(@"el usuario confirma el pedido preparado")]
        public void WhenElUsuarioConfirmaElPedidoPreparado()
        {
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
