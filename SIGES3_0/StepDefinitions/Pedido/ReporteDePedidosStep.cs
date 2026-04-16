using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.PedidoPages;

namespace SIGES3_0.StepDefinitions.PedidoStep
{
    [Binding]
    public class ReporteDePedidosStep
    {
        private readonly IWebDriver driver;
        private readonly ReporteDePedidosPage reportePage;

        public ReporteDePedidosStep(IWebDriver driver)
        {
            this.driver = driver;
            reportePage = new ReporteDePedidosPage(driver);
        }

        // =========================
        // NAVEGACIÓN
        // =========================

        [When("el usuario accede al submodulo de reportes")]
        public void WhenElUsuarioAccedeAlSubmoduloDeReportes()
        {
            reportePage.AccederASubmoduloReportes();
        }

        // =========================
        // FILTROS
        // =========================

        [When("el usuario selecciona el establecimiento {string}")]
        public void WhenElUsuarioSeleccionaElEstablecimiento(string establecimiento)
        {
            reportePage.SeleccionarEstablecimiento(establecimiento);
        }

        [When("el usuario selecciona el punto de venta {string}")]
        public void WhenElUsuarioSeleccionaElPuntoDeVenta(string puntoDeVenta)
        {
            reportePage.SeleccionarPuntoDeVenta(puntoDeVenta);
        }

        [When("el usuario ingresa la fecha y hora inicial {string}")]
        public void WhenElUsuarioIngresaLaFechaYHoraInicial(string fechaHoraInicial)
        {
            reportePage.IngresarFechaHoraInicial(fechaHoraInicial);
        }

        [When("el usuario ingresa la fecha y hora final {string}")]
        public void WhenElUsuarioIngresaLaFechaYHoraFinal(string fechaHoraFinal)
        {
            reportePage.IngresarFechaHoraFinal(fechaHoraFinal);
        }

        // =========================
        // ACCIÓN
        // =========================

        [When("el usuario hace clic en ver reporte {string}")]
        public void WhenElUsuarioHaceClicEnVerReporte(string tipoReporte)
        {
            reportePage.ClickVerReporte(tipoReporte);
        }

        // =========================
        // VALIDACIÓN
        // =========================

        [Then("el sistema muestra el resultado esperado del reporte {string}")]
        public void ThenElSistemaMuestraElResultadoEsperadoDelReporte(string resultadoEsperado)
        {
            reportePage.ValidarResultadoEsperado(resultadoEsperado);
        }
    }
}