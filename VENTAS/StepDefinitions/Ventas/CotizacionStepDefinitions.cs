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
    public class CotizacionStepDefinitions
    {
        private IWebDriver driver;
        CotizacionPage qouta;
 
        public CotizacionStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            this.qouta = new CotizacionPage(driver); ;
        }

        [When("Seleccionar Cotización")]
        public void WhenSeleccionarCotizacion()
        {
            qouta.ClickQouta();
        }

        [When("Click en nueva cotización")]
        public void WhenClickEnNuevaCotizacion()
        {
            qouta.ClickNewQouta();
        }

        [When("Agregar concepto para cotización {string}")]
        public void WhenAgregarConceptoParaCotizacion(string value)
        {
            qouta.ConceptQuota(value);
        }

        [When("Agregar conceptos para cotización:")]
        public void WhenAgregarConceptosParaCotizacion(DataTable table)
        {
            foreach (var row in table.Rows)
            {
                string value = row["value"];

                qouta.ConceptQuota(value);
            }
        }

        [When("Agregar la cantidad {string}")]
        public void WhenAgregarLaCantidad(string value)
        {
            qouta.QuantityconceptQuota(value);
        }

        [When("Ingresar el precio unitario {string}")]
        public void WhenIngresarElPrecioUnitario(string value)
        {
            qouta.UnitPriceConceptQuota(value);
        }

        [When("Ingresar IGV {string}")]
        public void WhenIngresarIGV(string option)
        {
            qouta.IGVQuota(option);
        }

        [When("Agregar tipo de cliente para cotización {string} {string}")]
        public void WhenAgregarTipoDeClienteParaCotizacion(string option, string value)
        {
            qouta.CustomerTypeQuota(option, value);
        }

        [When("Ingresar la fecha de vencimiento {string}")]
        public void WhenIngresarLaFechaDeVencimiento(string value)
        {
            qouta.ExpirationDateQouta(value);
        }

        [When("Click en pregenerar pedido")]
        public void WhenClickEnPregenerarPedido()
        {
            qouta.ClickPregenerateOrder();
        }

        [When("Click en pregenerar venta")]
        public void WhenClickEnPregenerarVenta()
        {
            qouta.ClickPregenerateSale();
        }

        [When("Identificar cliente {string} {string}")]
        public void WhenIdentificarCliente(string option, string value)
        {
            qouta.CustomerTypeOrderModal(option, value);
        }

        [When("Seleccionar el DET.UNIF. {string}")]
        public void WhenSeleccionarElDET_UNIF_(string option)
        {
            qouta.UnifiedDetailOrderModal(option);
        }

        [Then("Guardar venta pregenerada")]
        public void ThenGuardarVentaPregenerada()
        {
            qouta.SavePregenerateSale();
        }

        [Then("Guardar pedido pregenerado")]
        public void ThenGuardarPedidoPregenerado()
        {
            qouta.SaveOrderFromQuote();
        }
    }
}
