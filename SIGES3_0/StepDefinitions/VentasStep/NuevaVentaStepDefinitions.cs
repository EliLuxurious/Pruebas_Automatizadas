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

        [Given("configura la precondicion del concepto vendible para nueva venta:")]
        [When("configura la precondicion del concepto vendible para nueva venta:")]
        public void GivenConfiguraLaPrecondicionDelConceptoVendibleParaNuevaVenta(Table table) =>
            nuevaVentaPage.ConfigurarPrecondicionConceptoVendibleParaNuevaVenta(
                table.Rows.ToDictionary(row => row["Campo"], row => row["Valor"]));

        [Given("existe un concepto vendible para nueva venta con familia {string} concepto {string} y stock minimo {string}")]
        [When("existe un concepto vendible para nueva venta con familia {string} concepto {string} y stock minimo {string}")]
        public void GivenExisteUnConceptoVendibleParaNuevaVenta(string familia, string concepto, string stockMinimo) =>
            nuevaVentaPage.AsegurarConceptoVendibleParaNuevaVenta(familia, concepto, stockMinimo);

        // ─── MODO DE VENTA ────────────────────────────────────────────────────────────

        [StepDefinition("selecciona el modo de venta {string}")]
        public void WhenSeleccionaElModoDeVenta(string modo) =>
            nuevaVentaPage.SelectSaleModeFlow(modo);

        [StepDefinition("ingresa la fecha de emision {string}")]
        public void WhenIngresaLaFechaDeEmision(string fecha) =>
            nuevaVentaPage.SetFechaEmisionFlow(fecha);

        [StepDefinition("ingresa la fecha de credito {string}")]
        public void WhenIngresaLaFechaDeCredito(string fecha) =>
            nuevaVentaPage.SetFechaCreditoFlow(fecha);

        // ─── DETALLE ─────────────────────────────────────────────────────────────────

        [StepDefinition("configura IGV {string} y Detalle Unificado {string}")]
        public void WhenConfiguraIgvYDetalleUnificado(string igv, string detUnificado) =>
            nuevaVentaPage.ConfigurarIgvDetUnif(igv, detUnificado);

        // ─── FACTURACIÓN ─────────────────────────────────────────────────────────────

        [Scope(Tag = "NuevaVenta")]
        [When("el usuario selecciona la familia preparada para nueva venta")]
        public void WhenElUsuarioSeleccionaLaFamiliaPreparadaParaNuevaVenta() =>
            nuevaVentaPage.SeleccionarFamiliaPreparadaParaNuevaVenta();

        [Scope(Tag = "NuevaVenta")]
        [When("el usuario selecciona el concepto preparado para nueva venta")]
        public void WhenElUsuarioSeleccionaElConceptoPreparadoParaNuevaVenta() =>
            nuevaVentaPage.SeleccionarConceptoPreparadoParaNuevaVenta();

        [Scope(Tag = "NuevaVenta")]
        [When(@"el usuario selecciona la familia '(.*)'")]
        public void WhenElUsuarioSeleccionaLaFamiliaNuevaVenta(string familia) =>
            nuevaVentaPage.SeleccionarFamiliaNuevaVenta(familia);

        [Scope(Tag = "NuevaVenta")]
        [When(@"el usuario selecciona el concepto '(.*)'")]
        [When(@"usuario selecciona el concepto '(.*)'")]
        public void WhenElUsuarioSeleccionaElConceptoNuevaVenta(string concepto) =>
            nuevaVentaPage.SeleccionarConceptoNuevaVenta(concepto);

        [Scope(Tag = "NuevaVenta")]
        [When(@"el usuario ingresa la cantidad '(.*)'")]
        [When(@"usuario ingresa la cantidad '(.*)'")]
        public void WhenElUsuarioIngresaLaCantidadNuevaVenta(string cantidad) =>
            nuevaVentaPage.IngresarCantidadNuevaVenta(cantidad);

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

        // Reutiliza el contrato declarativo de medios de pago sin tocar Pedido.
        [Scope(Tag = "NuevaVenta")]
        [When(@"el usuario configura los medios de pago '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)'")]
        public void WhenElUsuarioConfiguraLosMediosDePagoNuevaVenta(
            string tipoPago,
            string multipago,
            string medioPago,
            string banco,
            string tarjeta,
            string cuentaBancaria,
            string nroOperacion,
            string montoPorMedio,
            string nroCuotas,
            string montoInicialCredito) =>
            nuevaVentaPage.ConfigurarMediosDePagoNuevaVenta(
                tipoPago,
                multipago,
                medioPago,
                banco,
                tarjeta,
                cuentaBancaria,
                nroOperacion,
                montoPorMedio,
                nroCuotas,
                montoInicialCredito,
                string.Empty);

        [Scope(Tag = "NuevaVenta")]
        [When(@"el usuario configura los medios de pago '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)' '([^']*)'")]
        public void WhenElUsuarioConfiguraLosMediosDePagoNuevaVentaConObservacion(
            string tipoPago,
            string multipago,
            string medioPago,
            string banco,
            string tarjeta,
            string cuentaBancaria,
            string nroOperacion,
            string montoPorMedio,
            string nroCuotas,
            string montoInicialCredito,
            string observacionPago) =>
            nuevaVentaPage.ConfigurarMediosDePagoNuevaVenta(
                tipoPago,
                multipago,
                medioPago,
                banco,
                tarjeta,
                cuentaBancaria,
                nroOperacion,
                montoPorMedio,
                nroCuotas,
                montoInicialCredito,
                observacionPago);

        // Observacion es un paso extra propio de NuevaVenta.
        [When("el usuario ingresa la observacion del pago {string}")]
        public void WhenElUsuarioIngresaLaObservacionDelPagoNuevaVenta(string observacion) =>
            nuevaVentaPage.IngresarObservacionDelPagoNuevaVenta(observacion);

        // ─── GUARDAR Y VALIDAR ────────────────────────────────────────────────────────

        [StepDefinition("hace clic en Guardar")]
        public void WhenHaceClicEnGuardar() =>
            nuevaVentaPage.GuardarVentaFlow();

        [Scope(Tag = "NuevaVenta")]
        [Then("el sistema valida el resultado de venta {string}")]
        public void ThenElSistemaValidaElResultadoDeVenta(string resultado) =>
            nuevaVentaPage.ValidarResultadoVenta(resultado);

        [Then("el sistema valida el resultado del descuento en venta {string}")]
        public void ThenElSistemaValidaElResultadoDelDescuentoEnVenta(string resultado) =>
            nuevaVentaPage.ValidarResultadoDescuentoEnVenta(resultado);

        [Scope(Tag = "NuevaVenta")]
        [Then("el sistema valida el resultado del pago en nueva venta {string}")]
        public void ThenElSistemaValidaElResultadoDelPagoEnNuevaVenta(string resultado) =>
            nuevaVentaPage.ValidarResultadoPagoEnNuevaVenta(resultado);
    }
}

