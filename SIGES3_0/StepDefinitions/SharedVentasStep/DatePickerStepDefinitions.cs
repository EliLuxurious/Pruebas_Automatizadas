using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.SharedVentasPage;

namespace SIGES3_0.StepDefinitions.SharedVentasStep
{
    [Binding]
    public class DatePickerStepDefinitions
    {
        public const string FechaFinalValorAntesKey = "SharedVentas.DatePicker.FechaFinal.ValorAntes";
        public const string FechaFinalValorDespuesKey = "SharedVentas.DatePicker.FechaFinal.ValorDespues";

        private const string CampoInicialPorDefecto = "Fecha y Hora Inicial";
        private const string CampoFinalPorDefecto = "Fecha y Hora Final";

        private readonly DatePickerPage _datePickerPage;
        private readonly ScenarioContext _scenarioContext;

        public DatePickerStepDefinitions(IWebDriver driver, ScenarioContext scenarioContext)
        {
            _datePickerPage = new DatePickerPage(driver);
            _scenarioContext = scenarioContext;
        }

        [When("el usuario selecciona la fecha y hora inicial {string} en el campo {string}")]
        public void SeleccionarFechaHoraInicial(string fechaHoraInicial, string labelCampo)
        {
            _datePickerPage.SeleccionarFechaHoraInicial(fechaHoraInicial, labelCampo);
        }

        [When("el usuario selecciona la fecha y hora final {string} en el campo {string}")]
        public void SeleccionarFechaHoraFinal(string fechaHoraFinal, string labelCampo)
        {
            _scenarioContext[FechaFinalValorAntesKey] = _datePickerPage.ObtenerValorCampo(labelCampo);
            _datePickerPage.SeleccionarFechaHoraFinal(fechaHoraFinal, labelCampo);
            _scenarioContext[FechaFinalValorDespuesKey] = _datePickerPage.ObtenerValorCampo(labelCampo);
        }

        [When("el usuario ingresa la fecha y hora inicial {string}")]
        [Scope(Feature = "VerVentas")]
        public void IngresarFechaHoraInicial(string fechaHoraInicial)
        {
            SeleccionarFechaHoraInicial(fechaHoraInicial, CampoInicialPorDefecto);
        }

        [When("el usuario ingresa la fecha y hora final {string}")]
        [Scope(Feature = "VerVentas")]
        public void IngresarFechaHoraFinal(string fechaHoraFinal)
        {
            SeleccionarFechaHoraFinal(fechaHoraFinal, CampoFinalPorDefecto);
        }
    }
}
