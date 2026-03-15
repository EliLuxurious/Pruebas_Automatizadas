using OpenQA.Selenium;
using SIGES3_0.Pages.VentasPage;

namespace SIGES3_0.StepDefinitions.VentasStep
{
    [Binding]
    public class ReportesStepDefinitions
    {
        private readonly ReportesPage reportesPage;

        public ReportesStepDefinitions(IWebDriver driver)
        {
            reportesPage = new ReportesPage(driver);
        }

        [When("Ingresar a Compras - Reportes")]
        public void WhenIngresarAComprasReportes()
        {
            reportesPage.OpenReports();
        }

        [When("Reporte por Tipo, Tipo de comprobante {string} Fecha Inicial {string} Fecha Final {string}")]
        public void WhenReportePorTipoTipoDeComprobanteFechaInicialFechaFinal(string option, string fromDate, string toDate)
        {
            reportesPage.ConfigureReportByType(option, fromDate, toDate);
        }

        [When("Reporte por {string} Fecha Inicial {string} Fecha Final {string}")]
        public void WhenReportePorFechaInicialFechaFinal(string reportType, string fromDate, string toDate)
        {
            reportesPage.ConfigureReport(reportType, fromDate, toDate);
        }

        [Then("Generar reporte por {string}")]
        public void ThenGenerarReportePor(string reportType)
        {
            reportesPage.Generate(reportType);
        }
    }
}
