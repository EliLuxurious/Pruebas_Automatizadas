using FLOTA_VEHICULAR.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace FLOTA_VEHICULAR.StepDefinitions
{
    [Binding]
    public class OdometroStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly OdometroPage odometroPage;

        public OdometroStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            odometroPage = new OdometroPage(driver);
        }

        [When("Se ingresa al módulo Odómetro")]
        public void WhenSeIngresaAlModuloOdometro()
        {
            odometroPage.IngresarModuloOdometro();
        }

        [When("Se selecciona Nuevo Odómetro")]
        public void WhenSeSeleccionaNuevoOdometro()
        {
            odometroPage.ClickNuevoOdometro();
        }

        [When("Se ingresa la placa {string} y se cargan los datos")]
        public void WhenSeIngresaLaPlacaYSeCarganLosDatos(string placa)
        {
            odometroPage.IngresarPlacaYBuscar(placa);
        }

        [When("Se ingresa la lectura del odómetro {string}")]
        public void WhenSeIngresaLaLecturaDelOdometro(string lectura)
        {
            odometroPage.IngresarLectura(lectura);
        }

        [When("Se selecciona la fecha de lectura día {string}")]
        public void WhenSeSeleccionaLaFechaDeLecturaDia(string dia)
        {
            odometroPage.SeleccionarFecha(dia);
        }

        [Then("Se procede a Guardar el odómetro")]
        public void ThenSeProcedeaGuardarElOdometro()
        {
            odometroPage.GuardarOdometro();
        }

        // ==========================================
        // PASOS PARA EDITAR ODÓMETRO
        // ==========================================

        [When("Se busca el odómetro por placa {string}")]
        public void WhenSeBuscaElOdometroPorPlaca(string placa)
        {
            odometroPage.BuscarOdometroPorPlacaGrid(placa);
        }

        [When("Se hace clic en ver odómetro")]
        public void WhenSeHaceClicEnVerOdometro()
        {
            odometroPage.ClicVerOdometroGrid();
        }

        [When("Se hace clic en editar odómetro")]
        public void WhenSeHaceClicEnEditarOdometro()
        {
            odometroPage.ClicEditarOdometro();
        }

        [Then("Se guarda la edición del odómetro")]
        public void ThenSeGuardaLaEdicionDelOdometro()
        {
            odometroPage.GuardarEdicionOdometro();
        }

        // ==========================================
        // PASOS PARA DAR DE BAJA ODÓMETRO
        // ==========================================

        [When("Se hace clic en dar de baja odómetro")]
        public void WhenSeHaceClicEnDarDeBajaOdometro()
        {
            odometroPage.ClicDarDeBajaOdometro();
        }

        [Then("Se confirma la baja del odómetro")]
        public void ThenSeConfirmaLaBajaDelOdometro()
        {
            odometroPage.ConfirmarBajaOdometro();
        }


        // ==========================================
        // PASOS PARA FILTROS DE ODÓMETRO
        // ==========================================

        [When("Se seleccionan las áreas en el filtro {string}")]
        public void WhenSeSeleccionanLasAreasEnElFiltro(string areas)
        {
            odometroPage.SeleccionarAreasFiltro(areas);
        }

        [When("Se selecciona el origen en el filtro {string}")]
        public void WhenSeSeleccionaElOrigenEnElFiltro(string origen)
        {
            odometroPage.SeleccionarOrigenFiltro(origen);
        }

        [When("Se marca la opción TODAS en {string}")]
        public void WhenSeMarcaLaOpcionTODASEn(string tipoFiltro)
        {
            odometroPage.HacerClicEnTodas(tipoFiltro);
        }

        [When("Se hace clic en Buscar filtros")]
        public void WhenSeHaceClicEnBuscarFiltros()
        {
            odometroPage.ClicBuscarFiltros();
        }
    }
}