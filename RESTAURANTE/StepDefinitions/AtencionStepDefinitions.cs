using OpenQA.Selenium;
using RESTAURANTE.Hoks.Pages;
using RESTAURANTE.Hoks.Pages.Atencion;
using RESTAURANTE.Hoks.Pages.Preparacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RESTAURANTE.StepDefinitions
{
    [Binding]
    public class AtencionStepDefinitions
    {
        private IWebDriver driver;
        AtencionPage atencionPage;
        AtencionConMesaPage atencionConMesaPage;
        AtencionSinMesaPage atencionSinMesaPage;

        public AtencionStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            atencionPage = new AtencionPage(driver);
            atencionConMesaPage = new AtencionConMesaPage(driver);
            atencionSinMesaPage = new AtencionSinMesaPage(driver);
        }

        // TIPO DE ATENCION

        [Given("Se seleciona el tipo de atencion {string}")]
        public void GivenSeSelecionaElTipoDeAtencion(string _tipoAtencion)
        {
            atencionPage.tipoAtencion(_tipoAtencion);
        }






        //ATENCION SIN MESA

        [Given("Se seleciona el modo {string}")]
        public void GivenSeSelecionaElModo(string _modoAtencion)
        {
            atencionSinMesaPage.SeleccionModoAtencion(_modoAtencion);
        }

        [Given("Se ingresa el cliente {string}")]
        public void GivenSeIngresaElCliente(string _cliente)
        {
            atencionSinMesaPage.IngresarCliente(_cliente);
        }

        [Given("Se selecciona el mozo {string}")]
        public void GivenSeSeleccionaElMozo(string _mozo)
        {
            atencionSinMesaPage.SeleccionMozo(_mozo);
        }

        [When("Se ingresa las siguientes ordenes:")]
        public void WhenSeIngresaLasSiguientesOrdenes(DataTable dataTable)
        {
            Console.WriteLine($"Procesando orden {dataTable.Rows}");

            foreach (var row in dataTable.Rows)
            {
                string _orden = row["Orden"];
                string _concepto = row["Concepto"];
                string _cantidad= row["Cantidad"];
                string _anotacion = row["Anotacion"];

                Console.WriteLine($"Procesando orden {_orden}: {_concepto}");

                switch (_orden)
                {
                    case "CODIGO":
                        atencionSinMesaPage.CodigoItem(atencionSinMesaPage.Formato("numero", _concepto));
                        break;

                    case "ITEM":
                        atencionSinMesaPage.SeleccionItem(_concepto, _cantidad);
                        break;

                    default:
                        throw new ArgumentException($"ORDEN NO INGRESADA: {_orden}");
                }
                
                if (!string.IsNullOrWhiteSpace(_anotacion))
                {
                    atencionSinMesaPage.AccionItem("Agregar anotacion", atencionSinMesaPage.Formato("nombre", _concepto), _cantidad, _anotacion);
                }
                Thread.Sleep(4000);
            }
        }

        [When("Se realiza la siguiente modificacion a la orden:")]
        public void WhenSeRealizaLaSiguienteModificacionALaOrden(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                string _accion = row["Accion"];
                string _item = row["Concepto"];
                string _cantidad = row["Cantidad"];
                string _anotacion = row["Anotacion"];
                atencionSinMesaPage.AccionItem(_accion, _item, _cantidad, _anotacion);
            }
        }

        [Given("Se selecciona el siguiente pedido:")]
        public void GivenSeSeleccionaElSiguientePedido(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                string _alias = row["Alias"];
                string _modo = row["Modo"];
                string _total = row["Total"];
                atencionSinMesaPage.BusquedaOrden(_alias, _modo, _total);
            }
        }

        [Given("Se realiza la accion de {string}")]
        public void GivenSeRealizaLaAccionDe(string _accion)
        {
            atencionSinMesaPage.AccionTabla(_accion, "");

            // atencionSinMesaPage.AccionPedido(_accion);
        }

        [Then("Se procede a {string} la orden")]
        public void ThenOutcome(string _accion)
        {
            atencionSinMesaPage.AccionOrden(_accion);
        }
    }
}
