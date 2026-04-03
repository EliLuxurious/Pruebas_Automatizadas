using OpenQA.Selenium;
using Reqnroll;
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

        [StepDefinition("selecciona familia {string} y concepto {string}")]
        public void WhenSeleccionaFamiliaYConcepto(string familia, string concepto)
        {
            nuevaVentaPage.SelectProductFlow(familia, concepto);
        }

        [StepDefinition("actualiza la cantidad {string}")]
        public void WhenActualizaLaCantidad(string cantidad)
        {
            nuevaVentaPage.UpdateQuantityFlow(cantidad);
        }

        [StepDefinition("ingresa el documento del cliente {string}")]
        public void WhenIngresaDocumentoCliente(string documento)
        {
            nuevaVentaPage.EnterDocumentAndSearch(documento);
        }

        [StepDefinition("selecciona comprobante {string} con serie {string}")]
        public void WhenSeleccionaComprobante(string comprobante, string serie)
        {
            nuevaVentaPage.SelectVoucherFlow(comprobante, serie);
        }

        [StepDefinition("selecciona tipo de entrega {string}")]
        public void WhenSeleccionaTipoEntrega(string entrega)
        {
            nuevaVentaPage.SelectDeliveryFlow(entrega);
        }

        [StepDefinition("configura el pago {string}")]
        public void WhenConfiguraPago(string pago)
        {
            nuevaVentaPage.ConfigurePaymentFlow(pago);
        }

        [StepDefinition("hace clic en Guardar")]
        public void WhenHaceClicEnGuardar()
        {
            nuevaVentaPage.GuardarVentaFlow();
        }

        [StepDefinition("crea {int} notas de venta con familia {string}, concepto {string}, cantidad {string} y documento {string}")]
        public void CreaNotasDeVenta(int n, string familia, string concepto, string cantidad, string documento)
        {
            nuevaVentaPage.CrearNotasDeVenta(n, familia, concepto, cantidad, documento);
        }

        [Then("el sistema muestra el mensaje {string}")]
        public void ThenElSistemaMuestraElMensaje(string mensaje)
        {
            nuevaVentaPage.VerifyConfirmationMessage(mensaje);
        }
    }
}
