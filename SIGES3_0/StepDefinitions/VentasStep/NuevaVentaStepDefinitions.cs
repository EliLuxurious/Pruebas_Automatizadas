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

        // ─── MODO DE VENTA ────────────────────────────────────────────────────────────

        [StepDefinition("selecciona el modo de venta {string}")]
        public void WhenSeleccionaElModoDeVenta(string modo) =>
            nuevaVentaPage.SelectSaleModeFlow(modo);

        [StepDefinition("ingresa la fecha de emision {string}")]
        public void WhenIngresaLaFechaDeEmision(string fecha) =>
            nuevaVentaPage.SetFechaEmisionFlow(fecha);

        // ─── DETALLE ─────────────────────────────────────────────────────────────────

        [StepDefinition("configura IGV {string} y Detalle Unificado {string}")]
        public void WhenConfiguraIgvYDetalleUnificado(string igv, string detUnificado) =>
            nuevaVentaPage.ConfigurarIgvDetUnif(igv, detUnificado);

        // ─── FACTURACIÓN ─────────────────────────────────────────────────────────────

        [StepDefinition("selecciona el punto de venta {string}")]
        public void WhenSeleccionaElPuntoDeVenta(string puntoVenta) =>
            nuevaVentaPage.SelectPuntoVentaFlow(puntoVenta);

        [StepDefinition("selecciona el vendedor {string}")]
        public void WhenSeleccionaElVendedor(string vendedor) =>
            nuevaVentaPage.SelectVendorFlow(vendedor);

        [StepDefinition("configura la facturacion {string} {string} {string}")]
        public void WhenConfiguraLaFacturacion(string comprobante, string serie, string cliente) =>
            nuevaVentaPage.ConfigurarFacturacionNuevaVenta(comprobante, serie, cliente);

        // ─── ENTREGA ─────────────────────────────────────────────────────────────────

        [Scope(Tag = "NuevaVenta")]
        [When(@"el usuario configura la entrega '(.*)' '(.*)'")]
        public void WhenConfiguraEntregaNuevaVenta(string entrega, string guiaRemision) =>
            nuevaVentaPage.ConfigurarEntregaNuevaVenta(entrega, guiaRemision);

        // GuiaRemisionPage usa clases Bootstrap (g-2 mb-3) que no existen en el form de NuevaVenta.
        [Scope(Tag = "NuevaVenta")]
        [When(@"el usuario ingresa peso bruto '(.*)'")]
        public void WhenIngresaPesoBrutoNV(string peso) =>
            nuevaVentaPage.IngresarPesoBrutoNV(peso);

        [Scope(Tag = "NuevaVenta")]
        [When(@"el usuario ingresa numero de bultos '(.*)'")]
        public void WhenIngresaNumeroBultosNV(string bultos) =>
            nuevaVentaPage.IngresarNumeroBultosNV(bultos);

        // ─── PAGO ─────────────────────────────────────────────────────────────────────

        [StepDefinition("configura el pago {string}")]
        public void WhenConfiguraPago(string pago) =>
            nuevaVentaPage.ConfigurePaymentFlow(pago);

        // ─── GUARDAR Y VALIDAR ────────────────────────────────────────────────────────

        [StepDefinition("hace clic en Guardar")]
        public void WhenHaceClicEnGuardar() =>
            nuevaVentaPage.GuardarVentaFlow();

        [Then("el sistema valida el resultado de venta {string}")]
        public void ThenElSistemaValidaElResultadoDeVenta(string resultado) =>
            nuevaVentaPage.ValidarResultadoVenta(resultado);
    }
}

