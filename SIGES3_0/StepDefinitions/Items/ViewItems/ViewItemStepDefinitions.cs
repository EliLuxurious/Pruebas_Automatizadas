using AventStack.ExtentReports.Gherkin.Model;
using NUnit.Framework;
using OpenQA.Selenium;
using SIGES3_0.Pages.Helpers;
using SIGES3_0.Pages.Items.ViewItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGES3_0.StepDefinitions.Items.ViewItems
{
    [Binding]
    public class ViewItemStepDefinitions
    {
        private IWebDriver driver;
        ViewItemsPage conceptosPage;

        public ViewItemStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            conceptosPage = new ViewItemsPage(driver);
        }

        [When("el usuario selecciona Ver Conceptos")]
        public void WhenElUsuarioSeleccionaVerConceptos()
        {
            conceptosPage.SeleccionarVerConcepto();
        }

        [When("el usuario selecciona el filtro Familia {string}")]
        public void WhenElUsuarioSeleccionaElFiltroFamilia(string familia)
        {
            conceptosPage.SeleccionarFamilia(familia);
        }

        [When("el usuario selecciona el filtro Categoria {string}")]
        public void WhenElUsuarioSeleccionaElFiltroCategoria(string categoria)
        {
            conceptosPage.SeleccionarCategoria(categoria);
        }

        [When("el usuario ingresa la palabra clave {string}")]
        public void WhenElUsuarioIngresaLaPalabraClave(string clave)
        {
            conceptosPage.IngresarPalabraClave(clave);
        }

        [When("el usuario presiona el botón Buscar")]
        public void WhenElUsuarioPresionaElBotonBuscar()
        {
            conceptosPage.HacerBusqueda();
        }

        [When("el usuario restablece los filtros")]
        public void WhenElUsuarioRestableceLosFiltros()
        {
            conceptosPage.LimpiarBusqueda();
        }

        [When("el usuario edita el concepto {string}")]
        public void WhenElUsuarioEditaElConcepto(string nombreConcepto)
        {
            conceptosPage.EditarConcepto(nombreConcepto);
        }

        [When("el usuario elimina el concepto {string}")]
        public void WhenElUsuarioEliminaElConcepto(string nombreConcepto)
        {
            conceptosPage.EliminarConcepto(nombreConcepto);
        }

        [Then("el sistema muestra conceptos asociados")]
        public void ThenElSistemaMuestraConceptosAsociados()
        {
            Assert.True(conceptosPage.HayResultados());
        }

        [Then("el sistema no muestra conceptos asociados")]
        public void ThenElSistemaNoMuestraConceptosAsociados()
        {
            Assert.True(conceptosPage.NoHayResultados());
        }

        
    }
}
