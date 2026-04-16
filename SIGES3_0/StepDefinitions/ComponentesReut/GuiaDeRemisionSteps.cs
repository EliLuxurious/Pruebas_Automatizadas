using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.Componentes;
using SIGES3_0.Pages.PedidoPage;

namespace SIGES3_0.StepDefinitions.GuiaRemisionStep
{
    [Binding]
    public class GuiaRemisionStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly VerPedidosPage verPedidosPage;
        private readonly GuiaRemisionPage guiaRemisionPage;

        public GuiaRemisionStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            verPedidosPage = new VerPedidosPage(driver);
            guiaRemisionPage = new GuiaRemisionPage(driver);
        }

        [Given(@"el usuario accede al modulo correspondiente")]
        public void GivenElUsuarioAccedeAlModuloCorrespondiente()
        {
            driver.Navigate().GoToUrl("https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login");
        }

        [Given(@"existe un pedido base para emitir guia con familia '(.*)' concepto '(.*)' cantidad '(.*)' cliente '(.*)' entrega '(.*)'")]
        public void GivenExisteUnPedidoBaseParaEmitirGuia(
            string familia, string concepto, string cantidad,
            string cliente, string entrega)
        {
            verPedidosPage.FiltrarPedidoBaseParaConfirmar(false);

            if (verPedidosPage.ExistePedidoBaseParaConfirmar(false))
                return;

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
            verPedidosPage.FiltrarPedidoBaseParaConfirmar(false);
        }

        [When(@"el usuario abre el flujo de guia de remision con comprobante '(.*)' serie '(.*)' cliente '(.*)' entrega '(.*)'")]
        public void WhenElUsuarioAbreElFlujoDeGuiaDeRemision(
            string tipoComprobante, string serie,
            string cliente, string tipoEntrega)
        {
            verPedidosPage.SeleccionarConfirmarPedido();
            verPedidosPage.ConfigurarFacturacionConfirmacion(tipoComprobante, serie, cliente);
            verPedidosPage.ConfigurarEntregaConfirmacion(tipoEntrega, "true");
            guiaRemisionPage.EsperarModalGuia();
        }

        [When(@"el usuario valida el destinatario autocompletado")]
        public void WhenElUsuarioValidaElDestinatarioAutocompletado()
        {
            guiaRemisionPage.ValidarDestinatarioAutocompletado();
        }

        [When(@"el usuario ingresa fecha de traslado '(.*)'")]
        public void WhenElUsuarioIngresaFechaDeTraslado(string fecha)
        {
            guiaRemisionPage.IngresarFechaTraslado(fecha);
        }

        [When(@"el usuario ingresa peso bruto '(.*)'")]
        public void WhenElUsuarioIngresaPesoBruto(string pesoBruto)
        {
            guiaRemisionPage.IngresarPesoBruto(pesoBruto);
        }

        [When(@"el usuario ingresa numero de bultos '(.*)'")]
        public void WhenElUsuarioIngresaNumeroDeBultos(string cantidadBultos)
        {
            guiaRemisionPage.IngresarNumeroBultos(cantidadBultos);
        }

        [When(@"el usuario selecciona transporte '(.*)'")]
        public void WhenElUsuarioSeleccionaTransporte(string tipoTransporte)
        {
            guiaRemisionPage.SeleccionarTipoTransporte(tipoTransporte);
        }

        [When(@"el usuario ingresa transportista privado '(.*)'")]
        public void WhenElUsuarioIngresaTransportistaPrivado(string transportista)
        {
            guiaRemisionPage.IngresarTransportistaPrivado(transportista);
        }

        [When(@"el usuario ingresa RUC transportista '(.*)'")]
        public void WhenElUsuarioIngresaRUCTransportista(string ruc)
        {
            guiaRemisionPage.IngresarTransportistaPublico(ruc);
        }

        [When(@"el usuario ingresa licencia '(.*)'")]
        public void WhenElUsuarioIngresaLicencia(string licencia)
        {
            guiaRemisionPage.IngresarNumeroLicencia(licencia);
        }

        [When(@"el usuario ingresa placa '(.*)'")]
        public void WhenElUsuarioIngresaPlaca(string placa)
        {
            guiaRemisionPage.IngresarNumeroPlaca(placa);
        }

        [When(@"el usuario selecciona direccion de origen '(.*)'")]
        public void WhenElUsuarioSeleccionaDireccionDeOrigen(string direccion)
        {
            guiaRemisionPage.SeleccionarDireccionOrigen(direccion);
        }

        [When(@"el usuario selecciona detalle de direccion de origen '(.*)'")]
        public void WhenElUsuarioSeleccionaDetalleDeOrigen(string detalle)
        {
            guiaRemisionPage.IngresarDetalleOrigen(detalle);
        }

        [When(@"el usuario selecciona direccion de destino '(.*)'")]
        public void WhenElUsuarioSeleccionaDireccionDeDestino(string direccion)
        {
            guiaRemisionPage.SeleccionarDireccionDestino(direccion);
        }

        [When(@"el usuario selecciona detalle de direccion de destino '(.*)'")]
        public void WhenElUsuarioSeleccionaDetalleDeDestino(string detalle)
        {
            guiaRemisionPage.IngresarDetalleDestino(detalle);
        }

        [When(@"el usuario emite la guia")]
        public void WhenElUsuarioEmiteLaGuia()
        {
            guiaRemisionPage.GuardarGuia();
        }

        [Then(@"el sistema valida el resultado de la guia '(.*)'")]
        public void ThenElSistemaValidaElResultadoDeLaGuia(string resultadoEsperado)
        {
            string resultado = guiaRemisionPage.ObtenerResultadoGuia()?.Trim() ?? string.Empty;

            Assert.That(resultado, Is.Not.Empty,
                "La guía no devolvió ningún resultado visible.");

            Assert.That(resultado,
                Does.Contain(resultadoEsperado).IgnoreCase,
                $"Resultado esperado: '{resultadoEsperado}'. Resultado obtenido: '{resultado}'");
        }
    }
}