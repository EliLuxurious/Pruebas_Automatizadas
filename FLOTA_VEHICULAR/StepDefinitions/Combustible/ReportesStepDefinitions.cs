using FLOTA_VEHICULAR.Pages.Combustible;
using OpenQA.Selenium;
using Reqnroll;

namespace FLOTA_VEHICULAR.StepDefinitions.Combustible
{
    [Binding]
    public class ReportesStepDefinitions
    {
        private readonly ReportesPage reportesPage;

        // 🔥 Variable de clase para "recordar" qué reporte elegimos
        private string reporteSeleccionado = "";

        public ReportesStepDefinitions(IWebDriver driver)
        {
            reportesPage = new ReportesPage(driver);
        }

        [When(@"Se selecciona el tipo de reporte ""(.*)""")]
        public void WhenSeSeleccionaElTipoDeReporte(string tipoReporte)
        {
            reporteSeleccionado = tipoReporte; // Guardamos el nombre en la memoria
            reportesPage.SeleccionarTipoReporte(tipoReporte);
        }

        [When(@"Se filtran las fechas del reporte desde ""(.*)"" hasta ""(.*)""")]
        public void WhenSeFiltranLasFechasDelReporteDesdeHasta(string fechaDesde, string fechaHasta)
        {
            reportesPage.FiltrarFechasReporte(fechaDesde, fechaHasta);
        }

        [When(@"Se selecciona el area ""(.*)"" para el reporte")]
        public void WhenSeSeleccionaElAreaParaElReporte(string area)
        {
            reportesPage.SeleccionarAreaReporte(area);
        }





        [When(@"Se selecciona el contrato ""(.*)"" en reportes")]
        public void WhenSeSeleccionaElContratoEnReportes(string contrato)
        {
            if (contrato.ToUpper() != "N/A")
            {
                reportesPage.SeleccionarContratoReporte(contrato);
            }
            else
            {
                System.Console.WriteLine("✅ Contrato omitido porque es un reporte de Control para firma.");
            }
        }










        [When(@"Se hace clic en el boton Ver Reporte")]
        public void WhenSeHaceClicEnElBotonVerReporte()
        {
            reportesPage.ClicVerReporte();
        }

        [Then(@"Se valida que el sistema genere la accion esperada para el resultado ""(.*)""")]
        public void ThenSeValidaQueElSistemaGenereLaAccionEsperadaParaElResultado(string resultadoEsperado)
        {
            // 🔥 Le pasamos a tu Page Object el resultado Y el nombre del reporte que guardamos
            reportesPage.ValidarResultado(resultadoEsperado, reporteSeleccionado);
        }
    }
}