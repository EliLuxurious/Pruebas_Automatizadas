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

        [When("ejecuta el flujo de nueva venta {string}")]
        public void WhenEjecutaElFlujoDeNuevaVenta(string caseId)
        {
            nuevaVentaPage.ExecuteFlow(caseId);
        }

        [Then("valida el resultado esperado de la venta:")]
        public void ThenValidaElResultadoEsperadoDeLaVenta(Table table)
        {
            var data = ToDictionary(table);
            var expectation = new SaleExpectation
            {
                SaveShouldBeEnabled = ReadBool(data, "SaveEnabled"),
                SaveShouldBeExecuted = ReadBool(data, "ExecuteSave"),
                ExpectedMessage = Read(data, "Mensaje")
            };
            nuevaVentaPage.ValidateSale(expectation);
        }

        private static Dictionary<string, string> ToDictionary(Table table)
        {
            var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in table.Rows)
            {
                var key = row.Values.FirstOrDefault()?.Trim() ?? "";
                var value = row.Values.Skip(1).FirstOrDefault()?.Trim() ?? "";
                if (!string.IsNullOrWhiteSpace(key))
                    d[key.Replace(" ", "").Replace(".", "")] = value;
            }
            return d;
        }

        private static string Read(Dictionary<string, string> data, string key)
        {
            return data.TryGetValue(key.Replace(" ", "").Replace(".", ""), out var v) ? v : "";
        }

        private static bool? ReadBool(Dictionary<string, string> data, string key)
        {
            if (!data.TryGetValue(key.Replace(" ", "").Replace(".", ""), out var v) || string.IsNullOrWhiteSpace(v))
                return null;
            var u = v.Trim().ToUpperInvariant();
            return u is "SI" or "YES" or "TRUE" ? true : u is "NO" or "FALSE" ? false : null;
        }
    }
}
