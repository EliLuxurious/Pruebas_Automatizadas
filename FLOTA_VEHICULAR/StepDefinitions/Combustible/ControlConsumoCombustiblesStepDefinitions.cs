using FLOTA_VEHICULAR.Pages.Combustible;
using OpenQA.Selenium;
using Reqnroll;

namespace FLOTA_VEHICULAR.StepDefinitions.Combustible
{
    [Binding]
    public class ControlConsumoCombustiblesStepDefinitions
    {
        private readonly ControlConsumoCombustiblesPage consumoPage;

        public ControlConsumoCombustiblesStepDefinitions(IWebDriver driver)
        {
            consumoPage = new ControlConsumoCombustiblesPage(driver);
        }

        [When(@"Se filtran las fechas desde el año ""(.*)"" hasta hoy")]
        public void WhenSeFiltranLasFechasDesdeElAnoHastaHoy(string anoDesde)
        {
            consumoPage.FiltrarFechas(anoDesde);
        }

        [When(@"Se filtra por placa ""(.*)"" en control de consumo")]
        public void WhenSeFiltraPorPlacaEnControlDeConsumo(string placa)
        {
            consumoPage.FiltrarPorPlaca(placa);
        }

        [When(@"Se hace clic en el boton Buscar en la pantalla de control")]
        public void WhenSeHaceClicEnElBotonBuscarEnLaPantallaDeControl()
        {
            consumoPage.ClicBuscar();
        }

        [When(@"Se hace clic en el icono de la Lupa del primer registro")]
        public void WhenSeHaceClicEnElIconoDeLaLupaDelPrimerRegistro()
        {
            consumoPage.ClicLupa();
        }

        // Reemplaza el [Then] anterior por este:
        [Then(@"Se verifican los calculos y la regla de tolerancia del consumo")]
        public void ThenSeVerificanLosCalculosYLaReglaDeToleranciaDelConsumo()
        {
            consumoPage.ValidarReglaDeNegocioAutonoma();
        }

        [Then(@"Se cierra el modal de detalle de consumo")]
        public void ThenSeCierraElModalDeDetalleDeConsumo()
        {
            consumoPage.CerrarModal();
        }
    }
}