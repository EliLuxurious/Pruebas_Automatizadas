using OpenQA.Selenium;
using SIGES3_0.Pages.Items.NewItem;
using SIGES3_0.Pages.Items.RegisterItemData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGES3_0.StepDefinitions.Items.RegisterItemData
{
    [Binding]
    public class EquivalenciesCombosStepDefinitions
    {
        private IWebDriver driver;
        EquivalenciesCombosPage conceptosPage;

        public EquivalenciesCombosStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            conceptosPage = new EquivalenciesCombosPage(driver);
        }

        [When("el usuario selecciona Equivalencia entre Productos y Combos")]
        public void WhenElUsuarioSeleccionaEquivalenciaEntreProductosYCombos()
        {
            conceptosPage.SeleccionarEquivalenciaEntreProductoYCombos();
        }

        //-------------------------------------------------------EQUIVALENCIA ENTRE PRODUCTOS--------------------------------------------------------------

        [When("el usuario selecciona la opcion Equivalencia entre Productos")]
        public void WhenElUsuarioSeleccionaLaOpcionEquivalenciaEntreProductos()
        {
            conceptosPage.SeleccionarEquivalenciaEntreProductos();
        }

        [When("el usuario selecciona este producto {string}")]
        public void WhenElUsuarioSeleccionaEsteProducto(string producto)
        {
            conceptosPage.SeleccionarEsteProducto(producto);
        }

        [When("el usuario ingresa la cantidad {string}")]
        public void WhenElUsuarioIngresaLaCantidad(string cantidad)
        {
            conceptosPage.IngresarCantidadEquivalencia(cantidad);
        }

        [When("el usuario selecciona de este producto {string}")]
        public void WhenElUsuarioSeleccionaDeEsteProducto(string producto)
        {
            conceptosPage.SeleccionarDeEsteProducto(producto);
        }

        [Then("el sistema agrega la equivalencia")]
        public void ThenElSistemaAgregaLaEquivalencia()
        {
            conceptosPage.AgregarEquivalencia();
        }

        [When("el usuario edita la equivalencia {string}")]
        public void WhenElUsuarioEditaLaEquivalencia(string producto)
        {
            conceptosPage.EditarEquivalenciaEntreProductos(producto);
        }

        [When("el usuario elimina la equivalencia {string}")]
        public void WhenElUsuarioEliminaLaEquivalencia(string producto)
        {
            conceptosPage.EliminarEquivalenciaEntreProductos(producto);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------COMBOS-----------------------------------------------------------------------------------

        [When("el usuario selecciona la opcion Combos")]
        public void WhenElUsuarioSeleccionaLaOpcionCombos()
        {
            conceptosPage.SeleccionarCombos();
        }

        [When("el usuario selecciona el concepto {string}")]
        public void WhenElUsuarioSeleccionaElConcepto(string concepto)
        {
            conceptosPage.SeleccionarConcepto(concepto);
        }

        [When("el usuario ingresa la cantidad del concepto {string}")]
        public void WhenElUsuarioIngresaLaCantidadDelConcepto(string cantidad)
        {
            conceptosPage.IngresarCantidadCombos(cantidad);
        }

        [When("el usuario agrega el concepto al combo")]
        public void WhenElUsuarioAgregaElConceptoAlCombo()
        {
            conceptosPage.AgregarCombo();
        }

        [When("el usuario selecciona el producto final del combo {string}")]
        public void WhenElUsuarioSeleccionaElProductoFinalDelCombo(string producto)
        {
            conceptosPage.SeleccionarProducto(producto);
        }

        [When("el usuario guarda el combo")]
        public void WhenElUsuarioGuardaElCombo()
        {
            conceptosPage.GuardarCombos();
        }

        [Then("el sistema muestra un mensaje de confirmacion exitosa")]
        public void ElSistemaMuestraUnMensajeDeConfirmacion()
        {
            conceptosPage.MensajeOK();
        }

    }
}


