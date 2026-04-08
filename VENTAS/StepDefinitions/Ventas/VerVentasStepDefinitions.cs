using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SigesCore.Hooks.LoginPage;
using SigesCore.Hooks.VentasPage;
using System.Net;
using SigesCore.Hooks.Utility;
using OpenQA.Selenium;

namespace SigesCore.StepDefinitions.Ventas
{
    [Binding]
    public class VerVentasStepDefinitions
    {
        private IWebDriver driver;
        VerVentasPage viewSale;

        public VerVentasStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            this.viewSale = new VerVentasPage(driver);
        }

        [When(@"Ingresar fecha inicial ""([^""]*)""")]
        public void WhenIngresarFechaInicial(string value)
        {
            viewSale.SetInitialDate(value);
        }

        [When(@"Ingresar fecha final ""([^""]*)""")]
        public void WhenIngresarFechaFinal(string value)
        {
            viewSale.SetFinalDate(value);   
        }

        [When(@"Click en consultar ventas")]
        public void WhenClickEnConsultarVentas()
        {
            viewSale.SetSalesQuery();
        }

        [When(@"Buscar venta '([^']*)'")]
        public void WhenBuscarVenta(string value)
        {
            viewSale.SearchSaleField(value);    
        }

        [When(@"Activar canje")]
        public void WhenActivarCanje()
        {
            viewSale.ActivateRedeem();
        }

        [When(@"Seleccionar venta")]
        public void WhenSeleccionarVenta()
        {
            viewSale.SelectSale();
        }

        [When(@"Click en el botón canjear")]
        public void WhenClickEnElBotonCanjear()
        {
            viewSale.ClickRedeemButton();
        }

        [When(@"Seleccionar el tipo de comprobante ""([^""]*)""")]
        public void WhenSeleccionarElTipoDeComprobante(string option)
        {
            viewSale.SetVoucherType(option);
        }

        [When(@"Click en el botón aceptar")]
        public void WhenClickEnElBotonAceptar()
        {
            viewSale.ClickAcceptRedeemButton();
        }

        [Then("Ver comprobante")]
        public void ThenVerComprobante()
        {
            viewSale.SeeSale();
        }

        [When("Ver venta buscada")]
        public void WhenVerVentaBuscada()
        {
            viewSale.SeeSale();
        }

        [When("Elegir tipo de nota {string}")]
        public void WhenElegirTipoDeNota(string option)
        {
            viewSale.ClickTypeNote(option);
        }

        [When("Seleccionar el tipo de nota {string}")]
        public void WhenSeleccionarElTipoDeNotaDeDebito(string option)
        {
            viewSale.TypeDebitNote(option);
        }

        [When("Seleccionar el documento {string}")]
        public void WhenSeleccionarElDocumento(string option)
        {
            viewSale.DocumentType(option);
        }

        [When("Escribir el motivo de la nota {string}")]
        public void WhenEscribirElMotivoDeLaNota(string value)
        {
            viewSale.ReasonDebitNote(value);
        }

        [When("Seleccionar el tipo de entrega {string}")]
        public void WhenSeleccionarElTipoDeEntrega(string option)
        {
            viewSale.DeliveryTypeNoteCredit(option);
        }

        [When(@"Ingresar el interés total '([^']*)'")]
        public void WhenIngresarElInteresTotal(string value)
        {
            viewSale.NoteAmount(value);
        }

        [When(@"Ingresar el aumento de valor de la nota '([^']*)'")]
        public void WhenIngresarElAumentoDeValorDeLaNota(string value)
        {
            viewSale.TotalAmount(value);
        }

        [When(@"Guardar nota")]
        public void WhenGuardarNotaDeDebito()
        {
            viewSale.SaveNote();
        }

        [When(@"Ingresar el descuento global '([^']*)'")]
        public void WhenIngresarElDescuentoGlobal(string value)
        {
            viewSale.NoteAmount(value);
        }

        [When(@"Ingresar el total de descuento '([^']*)'")]
        public void WhenIngresarElTotalDeDescuento(string value)
        {
            viewSale.TotalAmount(value);
        }

        [When("Ingresar la cantidad {string}")]
        public void WhenIngresarLaCantidad(string value)
        {
            viewSale.QuantityCreditNote(value);
        }

        [When(@"Click en el botón invalidar")]
        public void WhenClickEnElBotonInvalidar()
        {
            viewSale.ClickInvalidateDocument();
        }

        [When("Ingresar la observación {string}")]
        public void WhenIngresarLaObservacion(string value)
        {
            viewSale.Observation(value);
        }

        [When("Click en opción sí para invalidar documento")]
        public void WhenClickEnOpcionSiParaInvalidarDocumento()
        {
            viewSale.AcceptInvalidation();
        }

        [When(@"Click en el botón clonar")]
        public void WhenClickEnElBotonClonar()
        {
            viewSale.CloneSale();
        }

        [Then(@"Click en el botón imprimir")]
        public void ThenClickEnElBotonImprimir()
        {
            viewSale.PrintDocument();
        }

        [Then("Seleccionar el tipo de descarga {string}")]
        public void ThenSeleccionarElTipoDeDescarga(string option)
        {
            viewSale.DownloadType(option);
        }

        [When(@"Click en el botón enviar")]
        public void WhenClickEnElBotonEnviar()
        {
            viewSale.SendDocument();
        }

        [When(@"Ingresar correo '([^']*)'")]
        public void WhenIngresarCorreo(string value)
        {
            viewSale.EmailField(value);
        }

        [When("Click en el botón agregar el correo")]
        public void WhenClickEnElBotonAgregarElCorreo()
        {
            viewSale.AddEmail();
        }

        [Then("Enviar comprobante de venta")]
        public void ThenEnviarComprobanteDeVenta()
        {
            viewSale.Send();
        }
    }
}
