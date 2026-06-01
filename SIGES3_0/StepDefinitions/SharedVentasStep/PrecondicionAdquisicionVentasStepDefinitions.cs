using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.SharedVentasPage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIGES3_0.StepDefinitions.SharedVentasStep
{
    [Binding]
    public class PrecondicionAdquisicionVentasStepDefinitions
    {
        private readonly PrecondicionAdquisicionVentasPage _page;

        public PrecondicionAdquisicionVentasStepDefinitions(IWebDriver driver)
        {
            _page = new PrecondicionAdquisicionVentasPage(driver);
        }

        [When("Se selecciona y configura el producto de ventas a adquirir:")]
        public void WhenSeSeleccionaYConfiguraElProductoDeVentasAAdquirir(Table table)
        {
            foreach (var row in table.Rows)
            {
                _page.SeleccionarYConfigurarProductoVentas(
                    row["Producto"],
                    row["Cantidad"],
                    row["V. U"]);
            }
        }

        [When("Se configuran los datos de Entrega de Ventas:")]
        public void WhenSeConfiguranLosDatosDeEntregaDeVentas(Table table)
        {
            var datos = table.Rows.ToDictionary(
                row => row["Campo"].Trim(),
                row => row["Valor"].Trim(),
                StringComparer.OrdinalIgnoreCase);

            _page.ConfigurarEntregaVentas(
                Valor(datos, "Rol"),
                Valor(datos, "Establecimiento"),
                Valor(datos, "Almacén", "Almacen")
            );
        }

        private static string Valor(IDictionary<string, string> datos, params string[] llaves)
        {
            foreach (var llave in llaves)
            {
                if (datos.TryGetValue(llave, out var valor))
                    return valor;
            }

            return string.Empty;
        }
    }
}