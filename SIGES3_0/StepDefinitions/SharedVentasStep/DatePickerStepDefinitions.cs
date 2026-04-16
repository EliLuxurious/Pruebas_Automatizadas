using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.SharedVentasStep;

namespace SIGES3_0.StepDefinitions.SharedStep
{
    [Binding]
    public class DatePickerStepDefinitions
    {
        private readonly DatePicker _datePicker;

        public DatePickerStepDefinitions(IWebDriver driver)
        {
            _datePicker = new DatePicker(driver);
        }

        [When(@"el usuario ingresa la fecha y hora {string} en el campo {string}")]
        public void IngresarFechaEnCampo(string fechaHora, string labelCampo)
        {
            _datePicker.IngresarFechaHora(LocatorPorLabel(labelCampo), fechaHora);
        }

        private static By LocatorPorLabel(params string[] labels)
        {
            var condicion = string.Join(" or ", System.Linq.Enumerable.Select(labels, l => $"contains(.,'{l}')"));
            return By.XPath($"(//label[{condicion}])[1]/following::input[@readonly][1]");
        }
    }
}