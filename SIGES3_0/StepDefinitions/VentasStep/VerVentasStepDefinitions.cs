using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.VentasPage;

namespace SIGES3_0.StepDefinitions.VentasStep
{
    [Binding]
    public class VerVentasStepDefinitions
    {
        private readonly VerVentasPage verVentasPage;

        public VerVentasStepDefinitions(IWebDriver driver)
        {
            verVentasPage = new VerVentasPage(driver);
        }

        [When("hace clic en consultar ventas")]
        public void WhenHaceClicEnConsultarVentas()
        {
            verVentasPage.QuerySales();
        }

        [When("activa el modo canje")]
        public void WhenActivaElModoCanje()
        {
            verVentasPage.ActivarModoCanje();
        }

        [When(@"selecciona las notas de venta ""([^""]*)""")]
        public void WhenSeleccionaLasNotasDeVenta(string nvList)
        {
            verVentasPage.SeleccionarNVsPorSerie(nvList);
        }

        [When(@"selecciona (\d+) notas de venta")]
        public void WhenSeleccionaNotasDeVenta(int cantidad)
        {
            verVentasPage.SeleccionarNVs(cantidad);
        }

        [When("hace clic en el boton Canjear")]
        public void WhenHaceClicEnElBotonCanjear()
        {
            verVentasPage.ClickCanjear();
        }

        [When(@"selecciona el comprobante ""([^""]*)"" en el modal de canje")]
        public void WhenSeleccionaElComprobanteEnModal(string tipo)
        {
            verVentasPage.SeleccionarComprobanteEnModal(tipo);
        }

        [When(@"selecciona la serie ""([^""]*)"" en el modal de canje")]
        public void WhenSeleccionaLaSerieEnModal(string serie)
        {
            verVentasPage.SeleccionarSerieEnModal(serie);
        }

        [When("confirma el canje")]
        public void WhenConfirmaElCanje()
        {
            verVentasPage.ConfirmarCanje();
        }

        [Then("el sistema genera el canje exitosamente")]
        public void ThenElSistemaGeneraElCanjeExitosamente()
        {
            verVentasPage.VerificarCanjeExitoso();
        }

        [StepDefinition("filtra ventas del dia de hoy")]
        public void FiltrarVentasDiaDeHoy()
        {
            verVentasPage.FiltrarVentasDiaDeHoy();
        }

        [StepDefinition("filtra por tipo de documento {string}")]
        public void FiltrarPorTipoDoc(string tipoDoc)
        {
            verVentasPage.FiltrarPorTipoDoc(tipoDoc);
        }

        [StepDefinition("selecciona las primeras {int} notas de venta")]
        public void SeleccionarPrimerasNVs(int cantidad)
        {
            verVentasPage.SeleccionarNVs(cantidad);
        }

        [Then("el boton Canjear permanece deshabilitado")]
        public void ThenElBotonCanjearPermanaceDeshabilitado()
        {
            verVentasPage.VerificarBotonCanjearDeshabilitado();
        }

        [Then("el sistema muestra una advertencia de inconsistencia")]
        public void ThenElSistemaMuestraUnaAdvertenciaDeInconsistencia()
        {
            verVentasPage.VerificarMensajeInconsistencia();
        }

        [Then("el boton Aceptar permanece deshabilitado")]
        public void ThenElBotonAceptarPermanaceDeshabilitado()
        {
            verVentasPage.VerificarBotonAceptarDeshabilitado();
        }
    }
}
