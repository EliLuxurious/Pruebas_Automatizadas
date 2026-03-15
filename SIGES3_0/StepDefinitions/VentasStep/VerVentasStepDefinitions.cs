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

        [When(@"Ingresar fecha inicial ""([^""]*)""")]
        public void WhenIngresarFechaInicial(string value)
        {
            verVentasPage.SetInitialDate(value);
        }

        [When(@"Ingresar fecha final ""([^""]*)""")]
        public void WhenIngresarFechaFinal(string value)
        {
            verVentasPage.SetFinalDate(value);
        }

        [When("Click en consultar ventas")]
        public void WhenClickEnConsultarVentas()
        {
            verVentasPage.QuerySales();
        }

        [When(@"Buscar venta '([^']*)'")]
        public void WhenBuscarVenta(string value)
        {
            verVentasPage.SearchSale(value);
        }

        [When("Activar canje")]
        public void WhenActivarCanje()
        {
            verVentasPage.ActivateRedeem();
        }

        [When("Seleccionar venta")]
        public void WhenSeleccionarVenta()
        {
            verVentasPage.SelectFirstSale();
        }

        [When("Click en el boton canjear")]
        public void WhenClickEnElBotonCanjear()
        {
            verVentasPage.ClickRedeem();
        }

        [When(@"Seleccionar el tipo de comprobante ""([^""]*)""")]
        public void WhenSeleccionarElTipoDeComprobante(string option)
        {
            verVentasPage.SetVoucherType(option);
        }

        [When("Click en el boton aceptar")]
        public void WhenClickEnElBotonAceptar()
        {
            verVentasPage.AcceptRedeem();
        }

        [Then("Ver comprobante")]
        public void ThenVerComprobante()
        {
            verVentasPage.OpenSale();
        }

        [When("Ver venta buscada")]
        public void WhenVerVentaBuscada()
        {
            verVentasPage.OpenSale();
        }

        [When("Elegir tipo de nota {string}")]
        public void WhenElegirTipoDeNota(string option)
        {
            verVentasPage.ChooseNoteType(option);
        }

        [When("Seleccionar el tipo de nota {string}")]
        public void WhenSeleccionarElTipoDeNota(string option)
        {
            verVentasPage.SelectNoteCategory(option);
        }

        [When("Seleccionar el documento {string}")]
        public void WhenSeleccionarElDocumento(string option)
        {
            verVentasPage.SelectNoteDocument(option);
        }

        [When("Escribir el motivo de la nota {string}")]
        public void WhenEscribirElMotivoDeLaNota(string value)
        {
            verVentasPage.EnterReason(value);
        }

        [When(@"Ingresar el interes total '([^']*)'")]
        public void WhenIngresarElInteresTotal(string value)
        {
            verVentasPage.EnterNoteAmount(value);
        }

        [When(@"Ingresar el aumento de valor de la nota '([^']*)'")]
        public void WhenIngresarElAumentoDeValorDeLaNota(string value)
        {
            verVentasPage.EnterRowAmount(value);
        }

        [When("Seleccionar el tipo de entrega {string}")]
        public void WhenSeleccionarElTipoDeEntrega(string option)
        {
            verVentasPage.SelectCreditDelivery(option);
        }

        [When("Guardar nota")]
        public void WhenGuardarNota()
        {
            verVentasPage.SaveNote();
        }

        [When(@"Ingresar el descuento global '([^']*)'")]
        public void WhenIngresarElDescuentoGlobal(string value)
        {
            verVentasPage.EnterNoteAmount(value);
        }

        [When(@"Ingresar el total de descuento '([^']*)'")]
        public void WhenIngresarElTotalDeDescuento(string value)
        {
            verVentasPage.EnterRowAmount(value);
        }

        [When("Ingresar la cantidad {string}")]
        public void WhenIngresarLaCantidad(string value)
        {
            verVentasPage.EnterQuantity(value);
        }

        [When("Click en el boton invalidar")]
        public void WhenClickEnElBotonInvalidar()
        {
            verVentasPage.InvalidateDocument();
        }

        [When("Ingresar la observacion {string}")]
        public void WhenIngresarLaObservacion(string value)
        {
            verVentasPage.EnterObservation(value);
        }

        [When("Click en opcion si para invalidar documento")]
        public void WhenClickEnOpcionSiParaInvalidarDocumento()
        {
            verVentasPage.AcceptInvalidation();
        }

        [When("Click en el boton clonar")]
        public void WhenClickEnElBotonClonar()
        {
            verVentasPage.CloneSale();
        }

        [Then("Click en el boton imprimir")]
        public void ThenClickEnElBotonImprimir()
        {
            verVentasPage.PrintDocument();
        }

        [Then("Seleccionar el tipo de descarga {string}")]
        public void ThenSeleccionarElTipoDeDescarga(string option)
        {
            verVentasPage.DownloadDocument(option);
        }

        [When("Click en el boton enviar")]
        public void WhenClickEnElBotonEnviar()
        {
            verVentasPage.OpenSendModal();
        }

        [When(@"Ingresar correo '([^']*)'")]
        public void WhenIngresarCorreo(string value)
        {
            verVentasPage.EnterEmail(value);
        }

        [When("Click en el boton agregar el correo")]
        public void WhenClickEnElBotonAgregarElCorreo()
        {
            verVentasPage.AddEmail();
        }

        [Then("Enviar comprobante de venta")]
        public void ThenEnviarComprobanteDeVenta()
        {
            verVentasPage.SendMail();
        }
    }
}
