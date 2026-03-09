using FLOTA_VEHICULAR.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace FLOTA_VEHICULAR.StepDefinitions
{
    [Binding]
    public class SoatStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly SoatPage soatPage;

        public SoatStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            soatPage = new SoatPage(driver);
        }

        [When("Se ingresa al módulo SOAT")]
        public void WhenSeIngresaAlModuloSoat()
        {
            soatPage.IngresarModuloSoat();
        }

        [When("Se selecciona Nuevo SOAT")]
        public void WhenSeSeleccionaNuevoSoat()
        {
            soatPage.ClicNuevoSoat();
        }

        [When("Se ingresa la placa {string} y se busca en SOAT")]
        public void WhenSeIngresaLaPlacaYSeBuscaEnSoat(string placa)
        {
            soatPage.IngresarPlacaYBuscar(placa);
        }

        [When("Se selecciona el proveedor {string}")]
        public void WhenSeSeleccionaElProveedor(string proveedor)
        {
            soatPage.SeleccionarProveedor(proveedor);
        }

        [When("Se ingresa la póliza {string}")]
        public void WhenSeIngresaLaPoliza(string poliza)
        {
            soatPage.IngresarPoliza(poliza);
        }

        [When("Se selecciona la fecha DESDE el día {string} y HASTA el día {string}")]
        public void WhenSeSeleccionaLaFechaDesdeElDiaYHastaElDia(string diaDesde, string diaHasta)
        {
            soatPage.SeleccionarFechaDesdeYHasta(diaDesde, diaHasta);
        }

        [When("Se ingresa el RUC {string} y se busca")]
        public void WhenSeIngresaElRucYSeBusca(string ruc)
        {
            soatPage.IngresarRucYBuscar(ruc);
        }

        [When("Se selecciona la fecha del contratante el día {string}")]
        public void WhenSeSeleccionaLaFechaDelContratanteElDia(string dia)
        {
            soatPage.SeleccionarFechaContratante(dia);
        }

        [When("Se ingresa la hora de emisión {string} y el importe {string}")]
        public void WhenSeIngresaLaHoraDeEmisionYElImporte(string hora, string importe)
        {
            soatPage.IngresarHoraEImporte(hora, importe);
        }

        [When("Se adjunta el documento {string}")]
        public void WhenSeAdjuntaElDocumento(string rutaArchivo)
        {
            soatPage.AdjuntarDocumento(rutaArchivo);
        }

        [Then("Se guarda el SOAT")]
        public void ThenSeGuardaElSOAT()
        {
            // Ya no usamos ValidarFormularioCompleto, vamos directo al guardado validado
            soatPage.GuardarSoat();
        }







    }
}