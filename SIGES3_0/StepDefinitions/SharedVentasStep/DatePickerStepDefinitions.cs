using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.VentasPage;

namespace SIGES3_0.StepDefinitions.SharedStep
{
    [Binding]
    public class DatePickerStepDefinitions
    {
        private readonly ReportesPage _reportesPage;

        public DatePickerStepDefinitions(IWebDriver driver)
        {
            _reportesPage = new ReportesPage(driver);
        }

        [When(@"el usuario ingresa la fecha y hora {string} en el campo {string}")]
        public void IngresarFechaEnCampo(string fechaHora, string labelCampo)
        {
            _reportesPage.IngresarFechaHora(LocatorPorLabel(labelCampo), fechaHora);
        }

        private static By LocatorPorLabel(params string[] labels)
        {
            var condicion = string.Join(" or ", System.Linq.Enumerable.Select(labels, l => $"contains(.,'{l}')"));
            return By.XPath($"(//label[{condicion}])[1]/following::input[@readonly][1]");
        }
    }
}