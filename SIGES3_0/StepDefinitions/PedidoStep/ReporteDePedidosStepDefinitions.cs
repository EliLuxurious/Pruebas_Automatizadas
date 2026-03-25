using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.PedidoPages;

namespace SIGES3_0.StepDefinitions.PedidoStep
{
    [Binding]
    public class ReporteDePedidosStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly ReporteDePedidosPage reporteDePedidosPage;

        public ReporteDePedidosStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            reporteDePedidosPage = new ReporteDePedidosPage(driver);
        }

        [When("el usuario accede al submodulo de reportes")]
        public void WhenElUsuarioAccedeAlSubmoduloDeReportes()
        {
            reporteDePedidosPage.AccederASubmoduloReportes();
        }

        [When("el usuario selecciona el establecimiento {string}")]
        public void WhenElUsuarioSeleccionaElEstablecimiento(string establecimiento)
        {
            reporteDePedidosPage.SeleccionarEstablecimiento(establecimiento);
        }

        [When("el usuario selecciona el punto de venta {string}")]
        public void WhenElUsuarioSeleccionaElPuntoDeVenta(string puntoDeVenta)
        {
            reporteDePedidosPage.SeleccionarPuntoDeVenta(puntoDeVenta);
        }

        [When("el usuario ingresa la fecha y hora inicial {string}")]
        public void WhenElUsuarioIngresaLaFechaYHoraInicial(string fechaHoraInicial)
        {
            reporteDePedidosPage.IngresarFechaHoraInicial(fechaHoraInicial);
        }

        [When("el usuario ingresa la fecha y hora final {string}")]
        public void WhenElUsuarioIngresaLaFechaYHoraFinal(string fechaHoraFinal)
        {
            reporteDePedidosPage.IngresarFechaHoraFinal(fechaHoraFinal);
        }

        [When("el usuario hace clic en ver reporte {string}")]
        public void WhenElUsuarioHaceClicEnVerReporte(string tipoReporte)
        {
            reporteDePedidosPage.ClickVerReporte(tipoReporte);
        }

        [Then("el sistema muestra el resultado esperado del reporte {string}")]
        public void ThenElSistemaMuestraElResultadoEsperadoDelReporte(string resultadoEsperado)
        {
            reporteDePedidosPage.ValidarResultadoEsperado(resultadoEsperado);
        }
    }
}