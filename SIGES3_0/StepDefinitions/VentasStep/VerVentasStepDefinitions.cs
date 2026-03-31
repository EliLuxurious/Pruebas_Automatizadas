using OpenQA.Selenium;
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
