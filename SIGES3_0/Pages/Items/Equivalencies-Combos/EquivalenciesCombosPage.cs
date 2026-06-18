using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V137.Network;
using OpenQA.Selenium.Support.UI;
using SIGES3_0.Pages.Helpers;
using System.Text.RegularExpressions;

namespace SIGES3_0.Pages.Items.NewItem
{
    public class EquivalenciesCombosPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public EquivalenciesCombosPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // MENU CONCEPTOS
        private By conceptosMenu = By.XPath("//span[normalize-space()='Conceptos']");

        // SUBMENU EQUIVALENCIA ENTRE PRODUCTO Y COMBOS
        private By EquivalenciaEntreProductoyCombos = By.XPath("//span[normalize-space()='Equivalencia entre Productos y Combos']");


        //-------------------------------------------------EQUIVALENCIA ENTRE PRODUCTOS-------------------------------------------------------------

        // OPCION EQUIVALENCIA ENTRE PRODUCTOS
        private By OpcionEquivalenciaEntreProductos = By.XPath("//button[normalize-space()='Equivalencia Entre Productos']");


        // DROPDOWN DE "ESTE PRODUCTO"
        private By DropdownEsteProducto = By.XPath("(//select[.//option[normalize-space()='Seleccione un producto']])[1]");


        // CAMPO CONTIENE (CANTIDAD)
        private By CampoCantidadEquivalencia = By.XPath("//app-register-business-item-equivalence//input[@placeholder='Cantidad' and not(@disabled)]");


        // DROPDOWN DE "DE ESTE PRODUCTO"
        private By DropdownDeEsteProducto = By.XPath("(//select[.//option[normalize-space()='Seleccione un producto']])[2]");


        //-------------------------------------------------------COMBOS----------------------------------------------------------------------------

        // OPCION COMBOS
        private By OpcionCombos = By.XPath("//button[@type='button' and normalize-space()='Combos']");


        // DROPDOWN SELECCIONAR UN CONCEPTO
        private By DropdownSeleccionarunConcepto = By.XPath("//select[.//option[normalize-space(.)='Seleccione un concepto']]");


        // CAMPO CANTIDAD
        private By CampoCantidadCombo = By.XPath("//app-register-business-item-combo//input[@placeholder='Cantidad']");


        // DROPDOWN SELECCIONAR UN PRODUCTO
        private By DropdownSeleccionarProducto = By.XPath("//app-register-business-item-combo//select[.//option[normalize-space()='Seleccione un producto']]");


        // METODOS
        public void AbrirModuloConceptos()
        {
            utilities.ClickButton(conceptosMenu);
        }


        public void SeleccionarEquivalenciaEntreProductoYCombos()
        {
            utilities.ClickButton(EquivalenciaEntreProductoyCombos);
        }

        //-------------------------------------------------------------------EQUIVALENCIA ENTRE PRODUCTOS----------------------------------------------------

        //----------------------------------------------------------REGISTRO EQUIVALENCIA ENTRE PRODUCTOS----------------------------------------------------


        public void SeleccionarEquivalenciaEntreProductos()
        {
            utilities.ClickButton(OpcionEquivalenciaEntreProductos);
            By DropdownRegistroEquivalencia = By.XPath("//button[.//span[normalize-space()='Registro de Equivalencias']]");
            utilities.ClickButton(DropdownRegistroEquivalencia);
        }

        public void SeleccionarEsteProducto(string valor)
        {
            utilities.ClickButton(DropdownEsteProducto);

            By opcionValorEsteProducto = By.XPath($"(//select[.//option[normalize-space()='Seleccione un producto']])[1]//option[normalize-space()='{valor}']");
            utilities.ClickButton(opcionValorEsteProducto);
        }

        public void IngresarCantidadEquivalencia(string cantidad)
        {
            utilities.ClearAndEnterText(CampoCantidadEquivalencia, cantidad);
        }

        public void SeleccionarDeEsteProducto(string valor)
        {
            utilities.ClickButton(DropdownDeEsteProducto);

            By opcionValorDeEsteProducto = By.XPath($"(//select[.//option[normalize-space()='Seleccione un producto']])[2]//option[normalize-space()='{valor}']");
            utilities.ClickButton(opcionValorDeEsteProducto);
        }

        public void AgregarEquivalencia()
        {
            By BotonAgregarEquivalencia = By.XPath("//app-register-business-item-equivalence//button[normalize-space()='Agregar']");
            utilities.ClickButton(BotonAgregarEquivalencia);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------


        //----------------------------------------------------------EDITAR EQUIVALENCIA ENTRE PRODUCTOS----------------------------------------------------

        public void EditarEquivalenciaEntreProductos(string producto)
        {
            By botonEditar = By.XPath($"//tbody//tr[td[normalize-space()='{producto}']]//td[last()]//button[1]");
            utilities.ClickButton(botonEditar);

            By DropdownRegistroEquivalencia = By.XPath("//button[.//span[normalize-space()='Registro de Equivalencias']]");
            utilities.ClickButton(DropdownRegistroEquivalencia);
        }

        public void EliminarEquivalenciaEntreProductos(string producto)
        {
            By botonEliminarEquivalencia = By.XPath($"//tbody//tr[td[normalize-space()='{producto}']]//td[last()]//button[1]");
            utilities.ClickButton(botonEliminarEquivalencia);
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------COMBOS------------------------------------------------------------------

        public void SeleccionarCombos()
        {
            utilities.ClickButton(OpcionCombos);

            By DropdownCombos = By.XPath("//span[normalize-space()='Registro de Combos']/ancestor::button");
            utilities.ClickButton(DropdownCombos);
        }

        public void SeleccionarConcepto(string valor)
        {
            utilities.ClickButton(DropdownSeleccionarunConcepto);

            By opcionConcepto = By.XPath($"//select[.//option[normalize-space(.)='Seleccione un concepto']]//option[normalize-space()='{valor}']");
            utilities.ClickButton(opcionConcepto);
        }

        public void IngresarCantidadCombos(string cantidad)
        {
            utilities.ClearAndEnterText(CampoCantidadCombo, cantidad);
        }

        public void AgregarCombo()
        {
            By AgregarCombo = By.XPath("//button[@class='btn btn-success']");
            utilities.ClickButton(AgregarCombo);
        }

        public void SeleccionarProducto(string valor)
        {
            utilities.ClickButton(DropdownSeleccionarProducto);

            By opcionProducto = By.XPath($"//app-register-business-item-combo//select[.//option[normalize-space()='Seleccione un producto']]//option[normalize-space()='{valor}']");
            utilities.ClickButton(opcionProducto);
        }

        public void GuardarCombos()
        {
            By GuardarCombos = By.XPath("//button[normalize-space()='Guardar']");
            utilities.ClickButton(GuardarCombos);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------

        public void MensajeOK()
        {
            By MensajeOK = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(MensajeOK);
        }
    }
}