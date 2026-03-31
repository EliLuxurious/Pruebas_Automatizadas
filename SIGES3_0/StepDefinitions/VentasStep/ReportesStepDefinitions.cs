using OpenQA.Selenium;
using SIGES3_0.Pages.VentasPage;
using NUnit.Framework;

namespace SIGES3_0.StepDefinitions.VentasStep
{
    [Binding]
    public class ReportesStepDefinitions
    {
        private readonly ReportesPage _reportesPage;

        public ReportesStepDefinitions(IWebDriver driver)
        {
            _reportesPage = new ReportesPage(driver);
        }

        // ── Compartido (todas las vistas) ────────────────────────────────────────

        [When(@"selecciona la vista ""(.*)""")]
        public void WhenSeleccionaLaVista(string tabName)
        {
            _reportesPage.SeleccionarVista(tabName);
        }

        [When(@"hace clic en ""(.*)"" en la tarjeta ""(.*)""")]
        public void WhenHaceClicEnEnLaTarjeta(string btn, string tarjeta)
        {
            _reportesPage.ClickVerReporte(tarjeta);
        }

        // ── Tab: Comprobantes ────────────────────────────────────────────────────

        [When(@"selecciona el tipo de comprobante ""(.*)""")]
        public void WhenSeleccionaElTipoDeComprobante(string tipoComprobante)
        {
            _reportesPage.SeleccionarTipoComprobante(tipoComprobante);
        }

        [When(@"selecciona la serie ""(.*)""")]
        public void WhenSeleccionaLaSerie(string serie)
        {
            _reportesPage.SeleccionarSerie(serie);
        }

        // ── Tab: Series ──────────────────────────────────────────────────────────

        [When(@"selecciona el comprobante y serie ""(.*)""")]
        public void WhenSeleccionaElComprobanteSerie(string valor)
        {
            _reportesPage.SeleccionarComprobanteSerie(valor);
        }

        // ── Tab: Conceptos ───────────────────────────────────────────────────────

        [When(@"selecciona el punto de venta ""(.*)""")]
        public void WhenSeleccionaElPuntoDeVenta(string puntoVenta)
        {
            _reportesPage.SeleccionarPuntoVenta(puntoVenta);
        }

        [When(@"selecciona la familia ""(.*)""")]
        public void WhenSeleccionaLaFamilia(string familia)
        {
            _reportesPage.SeleccionarFamilia(familia);
        }

        [When(@"selecciona la característica ""(.*)"" en la tarjeta ""(.*)""")]
        public void WhenSeleccionaLaCaracteristicaEnLaTarjeta(string caracteristica, string tarjeta)
        {
            _reportesPage.SeleccionarCaracteristica(caracteristica, tarjeta);
        }

        // ── Tab: Vendedor ────────────────────────────────────────────────────────

        [When(@"selecciona el vendedor ""(.*)""")]
        public void WhenSeleccionaElVendedor(string vendedor)
        {
            _reportesPage.SeleccionarVendedor(vendedor);
        }

        [When(@"selecciona ""(.*)"" en el filtro ""(.*)"" de la tarjeta ""(.*)""")]
        public void WhenSeleccionaEnElFiltroDeLaTarjeta(string valor, string filtro, string tarjeta)
        {
            _reportesPage.SeleccionarFiltroEnTarjeta(valor, filtro, tarjeta);
        }

        // ── Tab: Grupos ──────────────────────────────────────────────────────────

        [When(@"selecciona el establecimiento ""(.*)""")]
        public void WhenSeleccionaElEstablecimiento(string establecimiento)
        {
            _reportesPage.SeleccionarEstablecimiento(establecimiento);
        }



        // Usado por: @FiltroFechas — valida si el reporte se generó o el sistema bloqueó las fechas inválidas

        [Then(@"el sistema muestra el resultado esperado del reporte ""(.*)""")]
        public void ThenElSistemaMuestraElResultadoEsperadoDelReporte(string resultadoEsperado)
        {
            _reportesPage.ValidarResultadoReporte(resultadoEsperado);
        }

        // Usado por: @PorComprobante, @PorSerie, @PorConceptos, @PorFamilia, @PorCaracteristica
        [Then(@"el sistema genera el reporte exitosamente")]
        public void ThenElSistemaGeneraElReporteExitosamente()
        {
            Assert.IsTrue(_reportesPage.VerificarReporteGenerado(), "El reporte no se generó exitosamente.");
        }
    }
}
