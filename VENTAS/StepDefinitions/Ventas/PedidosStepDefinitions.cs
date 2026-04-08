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
using Reqnroll;

namespace SigesCore.StepDefinitions.Ventas
{
    [Binding]
    public class PedidosStepDefinitions
    {
        private IWebDriver driver;
        PedidosPage orders;

        public PedidosStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            this.orders = new PedidosPage(driver);
        }

        [When("Seleccionar Pedido")]
        public void WhenSeleccionarPedido()
        {
            orders.ClickOrder();
        }

        [When("Seleccionar ver pedido")]
        public void WhenSeleccionarVerPedido()
        {
            orders.ClickViewOrder();
        }

        [When("Click en nuevo pedido")]
        public void WhenClickEnNuevoPedido()
        {
            orders.ClickNewOrder();
        }

        [When("Agregar concepto {string}")]
        public void WhenAgregarConcepto(string value)
        {
            orders.ConceptOrder(value);
        }

        [When("Agregar conceptos para pedido:")]
        public void WhenAgregarConceptosParaPedido(DataTable table)
        {
            foreach (var row in table.Rows)
            {
                string value = row["value"];

                orders.ConceptOrder(value);
            }
        }

        [When("Agregar cantidad {string}")]
        public void WhenAgregarCantidad(string value)
        {
            orders.QuantityconceptOrder(value);
        }

        [When("Agregar precio unitario {string}")]
        public void WhenAgregarPrecioUnitario(string value)
        {
            orders.UnitPriceConceptOrder(value);
        }

        [When("Seleccionar IGV {string}")]
        public void WhenSeleccionarIGV(string option)
        {
            orders.IGVOrder(option);
        }

        [When("Seleccionar DET.UNIF. {string}")]
        public void WhenSeleccionarDET_UNIF_(string option)
        {
            orders.UnifiedDetailOrder(option);
        }

        [When("Agregar tipo de cliente {string} {string}")]
        public void WhenAgregarElTipoDeCliente(string option, string value)
        {
            orders.CustomerTypeOrder(option, value);
        }

        [When("Seleccionar tipo de comprobante {string}")]
        public void WhenSeleccionarElComprobante(string option)
        {
            orders.SelectInvoiceType(option);
        }

        [When("Elegir tipo de entrega {string}")]
        public void WhenSeleccionarElTipoDeEntrega(string option)
        {
            orders.SelectDeliveryType(option);
        }

        [When("Digitar fecha inicial {string}")]
        public void WhenDigitarFechaInicial(string value)
        {
            orders.InitialDate(value);
        }

        [When("Digitar fecha final {string}")]
        public void WhenDigitarFechaFinal(string value)
        {
            orders.FinalDate(value);
        }

        [When("Click en consultar pedidos")]
        public void WhenClickEnConsultarPedidos()
        {
            orders.OrderQuery();
        }

        [When("Click en confirmar pedido")]
        public void WhenClickEnConfirmarPedido()
        {
            orders.ConfirmOrder();
        }

        [When("Seleccionar comprobante {string}")]
        public void WhenSeleccionarComprobante(string value)
        {
            orders.InvoiceType(value);
        }

        [When("Click en invalidar pedido")]
        public void WhenClickEnInvalidarPedido()
        {
            orders.InvalidateOrder();
        }

        [When("Ingresar la observación de invalidación {string}")]
        public void WhenIngresarLaObservacionDeInvalidacion(string value)
        {
            orders.AddObservation(value);
        }

        [Then("Click en aceptar invalidación")]
        public void ThenClickEnAceptarInvalidacion()
        {
            orders.ClickAcceptInvalidation();
        }

        [Then("Guardar pedido o cotización")]
        public void ThenGuardarPedido()
        {
            orders.SaveOrder();
        }
    }
}
