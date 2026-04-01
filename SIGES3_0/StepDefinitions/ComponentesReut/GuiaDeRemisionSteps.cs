using System;
using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.Componentes;

namespace SIGES3_0.StepDefinitions.Componentes
{
    [Binding]
    public class GuiaRemisionStepDefinitions
    {
        private readonly GuiaRemisionPage guiaRemisionPage;

        public GuiaRemisionStepDefinitions(IWebDriver driver)
        {
            guiaRemisionPage = new GuiaRemisionPage(driver);
        }

        [Given(@"el usuario accede al modulo correspondiente")]
        public void GivenElUsuarioAccedeAlModuloCorrespondiente()
        {
            guiaRemisionPage.EsperarModalGuia();
        }

        [When(@"el usuario valida el destinatario autocompletado")]
        public void WhenElUsuarioValidaElDestinatarioAutocompletado()
        {
            guiaRemisionPage.ExpandirDatosGenerales();
            guiaRemisionPage.ValidarDestinatarioAutocompletado();
        }

        [When(@"el usuario ingresa fecha de traslado '(.*)'")]
        public void WhenElUsuarioIngresaFechaDeTraslado(string fechaDeInicioTraslado)
        {
            guiaRemisionPage.ExpandirDatosGenerales();
            guiaRemisionPage.IngresarFechaTraslado(fechaDeInicioTraslado);
        }

        [When(@"el usuario ingresa peso bruto '(.*)'")]
        public void WhenElUsuarioIngresaPesoBruto(string pesoBruto)
        {
            guiaRemisionPage.ExpandirDatosGenerales();
            guiaRemisionPage.IngresarPesoBruto(pesoBruto);
        }

        [When(@"el usuario ingresa numero de bultos '(.*)'")]
        public void WhenElUsuarioIngresaNumeroDeBultos(string cantidadBultos)
        {
            guiaRemisionPage.ExpandirDatosGenerales();
            guiaRemisionPage.IngresarNumeroBultos(cantidadBultos);
        }

        [When(@"el usuario selecciona transporte '(.*)'")]
        public void WhenElUsuarioSeleccionaTransporte(string tipoTransporte)
        {
            guiaRemisionPage.ExpandirDatosTransporte();
            guiaRemisionPage.SeleccionarTipoTransporte(tipoTransporte);
        }

        [When(@"el usuario ingresa RUC transportista '(.*)'")]
        public void WhenElUsuarioIngresaRUCTransportista(string transportistaRuc)
        {
            guiaRemisionPage.ExpandirDatosTransporte();
            guiaRemisionPage.IngresarTransportistaPublico(transportistaRuc);
        }

        [When(@"el usuario ingresa DNI conductor '(.*)'")]
        public void WhenElUsuarioIngresaDNIConductor(string dniConductor)
        {
            guiaRemisionPage.ExpandirDatosTransporte();
            guiaRemisionPage.IngresarConductorPrivado(dniConductor);
        }

        [When(@"el usuario ingresa licencia '(.*)'")]
        public void WhenElUsuarioIngresaLicencia(string numeroLicencia)
        {
            guiaRemisionPage.ExpandirDatosTransporte();
            guiaRemisionPage.IngresarNumeroLicencia(numeroLicencia);
        }

        [When(@"el usuario ingresa placa '(.*)'")]
        public void WhenElUsuarioIngresaPlaca(string numeroPlaca)
        {
            guiaRemisionPage.ExpandirDatosTransporte();
            guiaRemisionPage.IngresarNumeroPlaca(numeroPlaca);
        }

        [When(@"el usuario selecciona direccion de origen '(.*)'")]
        public void WhenElUsuarioValidaDireccionDeOrigen(string direccionOrigen)
        {
            guiaRemisionPage.ExpandirDatosTransporte();
            guiaRemisionPage.SeleccionarDireccionOrigen(direccionOrigen);
        }

        [When(@"el usuario valida direccion de destino '(.*)'")]
        public void WhenElUsuarioValidaDireccionDeDestino(string direccionDestino)
        {
            guiaRemisionPage.ExpandirDatosTransporte();
            guiaRemisionPage.SeleccionarDireccionDestino(direccionDestino);
        }

        [When(@"el usuario emite la guia")]
        public void WhenElUsuarioEmiteLaGuia()
        {
            guiaRemisionPage.GuardarGuia();
        }

        [Then(@"el sistema valida el resultado de la guia '(.*)'")]
        public void ThenElSistemaValidaElResultadoDeLaGuia(string resultadoEsperado)
        {
            guiaRemisionPage.ValidarResultado(resultadoEsperado);
        }
    }
}