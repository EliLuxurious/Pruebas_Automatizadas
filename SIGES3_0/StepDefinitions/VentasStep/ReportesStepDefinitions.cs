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

        [When(@"ingresa al modulo de ""(.*)"" y selecciona ""(.*)""")]
        public void WhenIngresaAlModuloDeYSelecciona(string modulo, string submenu)
        {
            _reportesPage.NavegarAReportes();
        }

        [When(@"selecciona la vista ""(.*)""")]
        public void WhenSeleccionaLaVista(string tabName)
        {
            _reportesPage.SeleccionarVista(tabName);
        }

        [When(@"ingresa la fecha y hora inicial ""(.*)"" y final ""(.*)""")]
        public void WhenIngresaLaFechaYHoraInicialYFinal(string fechaInicial, string fechaFinal)
        {
            _reportesPage.IngresarFechas(fechaInicial, fechaFinal);
        }

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

        [When(@"hace clic en ""(.*)"" en la tarjeta ""(.*)""")]
        public void WhenHaceClicEnEnLaTarjeta(string btn, string tarjeta)
        {
            _reportesPage.ClickVerReporte(tarjeta);
        }

        [Then(@"el sistema genera el reporte exitosamente")]
        public void ThenElSistemaGeneraElReporteExitosamente()
        {
            Assert.IsTrue(_reportesPage.VerificarReporteGenerado(), "El reporte no se generó exitosamente.");
        }

        [Then(@"valida que el boton ""(.*)"" en la tarjeta ""(.*)"" este deshabilitado")]
        public void ThenValidaQueElBotonEnLaTarjetaEsteDeshabilitado(string btn, string tarjeta)
        {
            Assert.IsFalse(_reportesPage.VerificarBotonHabilitado(tarjeta), $"El boton {btn} en la tarjeta {tarjeta} deberia estar deshabilitado.");
        }

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
    }
}
