using OpenQA.Selenium;
using SIGES3_0.Pages.VentasPage;

namespace SIGES3_0.StepDefinitions.VentasStep
{
    [Binding]
    public class NuevaVentaStepDefinitions
    {
        private readonly NuevaVentaPage nuevaVentaPage;

        public NuevaVentaStepDefinitions(IWebDriver driver)
        {
            nuevaVentaPage = new NuevaVentaPage(driver);
        }

        [When("abre el flujo de ventas {string}")]
        public void WhenAbreElFlujoDeVentas(string salesFlow)
        {
            nuevaVentaPage.OpenSalesFlow(salesFlow);
        }

        [When("ejecuta el flujo de nueva venta con familia {string}, concepto {string}, cantidad {string}, documento {string}, comprobante {string}, serie {string}, entrega {string} y pago {string}")]
        public void WhenEjecutaElFlujoDeNuevaVentaDynamic(string familia, string concepto, string cantidad, string documento, string comprobante, string serie, string entrega, string pago)
        {
            nuevaVentaPage.ExecuteFlowDynamic(familia, concepto, cantidad, documento, comprobante, serie, entrega, pago);
        }

        [Then("valida que Guardar habilitado sea {string}")]
        public void ThenValidaQueGuardarHabilitadoSea(string habilitado)
        {
            var isEnabled = habilitado.Trim().ToUpperInvariant() is "SI" or "YES" or "TRUE";
            var expectation = new VentaExpectation
            {
                SaveShouldBeEnabled = isEnabled
            };
            nuevaVentaPage.ValidateSale(expectation);
        }

        [Then("valida que Ejecutar guardado sea {string}")]
        public void ThenValidaQueEjecutarGuardadoSea(string ejecutar)
        {
            var isExecuted = ejecutar.Trim().ToUpperInvariant() is "SI" or "YES" or "TRUE";
            var expectation = new VentaExpectation
            {
                SaveShouldBeExecuted = isExecuted
            };
            nuevaVentaPage.ValidateSale(expectation);
        }

        [Then("verifica el mensaje de confirmacion {string}")]
        public void ThenVerificaElMensajeDeConfirmacion(string mensaje)
        {
            var expectation = new VentaExpectation
            {
                ExpectedMessage = mensaje
            };
            nuevaVentaPage.ValidateSale(expectation);
        }
    }
}
