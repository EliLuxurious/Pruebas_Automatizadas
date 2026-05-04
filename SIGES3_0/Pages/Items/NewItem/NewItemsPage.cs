using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V137.Network;
using OpenQA.Selenium.Support.UI;
using SIGES3_0.Pages.Helpers;
using System.Text.RegularExpressions;

namespace SIGES3_0.Pages.Items.NewItem
{
    public class NewItemsPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public NewItemsPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // MENU CONCEPTOS
        private By conceptosMenu = By.XPath("//span[text()='Conceptos']/following::input[1]");

        // SUBMENU
        private By nuevoConcepto = By.XPath("//span[normalize-space()='Nuevo Concepto']");

        // FAMILIA
        private By DropdownFamilia = By.XPath("//div[contains(@class,'select-trigger')]");

        //SUFIJO
        private By IngresarSufijo = By.XPath("//input[@placeholder='Sufijo']");

        //SELECCION CHECKBOX AUTO CODIGO DE BARRA
        private By CheckAuto = By.XPath("//input[@id='autoBarcodeChk']");

        //INGRESAR CODIGO DE BARRA
        private By CampoCodigoBarra = By.XPath("//input[@placeholder='Código de Barra']");

        //U.M.COMERCIAL
        private By DropdownUMComercial = By.XPath("//app-dropdown-search[@formcontrolname='umComercial']//div[@class='select-trigger form-control']");

        //U.Medida
        private By DropdownUMedida = By.XPath("//app-dropdown-search[@formcontrolname='uMedida']//div[@class='select-trigger form-control']");

        //ELIMINAR ROL POR DEFECTO
        private By EliminarRol = By.XPath("//span[normalize-space()='Item Comercial']//i[@class='bi bi-x']");

        //ROLES
        private By DropdownRoles = By.XPath("//span[normalize-space()='Seleccione el rol']");

        //ELIMINAR MODULO A MOSTRAR POR DEFECTO
        private By EliminarModulo = By.XPath("//span[normalize-space()='MOD0006']//i[@class='bi bi-x']");

        //MODULOS A MOSTRAR
        private By DropdownModulos = By.XPath("//span[normalize-space()='Seleccione el modulo']");

        //MARCA
        private By DropdownMarca = By.XPath("//select[contains(@class,'custom-select')]");

        //PRESENTACIÓN
        private By DropdownPresentacion = By.XPath("//app-dropdown-search[@formcontrolname='nombrePresentation']//i[@class='bi bi-chevron-down']");

        //CANTIDAD
        private By CampoCantidad = By.XPath("//input[@placeholder='0.00']");

        //UNIDAD DE MEDIDA
        private By DropdownUnidadMedida = By.XPath("//div[contains(@class,'select-trigger')][.//span[normalize-space()='UN']]");

        //BOTÓN GUARDAR
        private By BotonGuardar = By.XPath("//button[normalize-space()='Guardar']");

        // METODOS
        public void AbrirModuloConceptos()
        {
            utilities.ClickButton(conceptosMenu);
        }

        public void SeleccionarNuevoConcepto()
        {
            utilities.ClickButton(nuevoConcepto);
        }

        public void SeleccionarFamilia(string familia)
        {
            utilities.ClickButton(DropdownFamilia);
            By buscadorFamilia = By.XPath("//input[contains(@class,'search-input')]");
            utilities.EnterText(buscadorFamilia, familia);
            By opcionFamilia = By.XPath($"//span[contains(@class,'option-label') and text()='{familia}']");
            utilities.ClickButton(opcionFamilia);
        }

        public void SeleccionarAutoCodigoDeBarra()
        {
            utilities.ClickButton(CheckAuto);
        }

        public void IngresarCodigoDeBarra(string codigoBarra)
        {
            utilities.EnterText(CampoCodigoBarra, codigoBarra);
        }

        public void AgregarSufijo(string sufijo)
        {
            utilities.EnterText(IngresarSufijo, sufijo);
        }

        public void SeleccionarUMComercial(string umComercial)
        {
            utilities.ClickButton(DropdownUMComercial);
            By buscador = By.XPath("(//input[@placeholder='Buscar...'])[last()]");
            utilities.EnterText(buscador, umComercial);
            By opcion = By.XPath($"//div[contains(@class,'option-item')][.//text()='{umComercial}']");
            utilities.ClickButton(opcion);
        }

        public void SeleccionarUMedida(string umMedida)
        {
            utilities.ClickButton(DropdownUMedida);
            By buscador = By.XPath("(//input[@placeholder='Buscar...'])[last()]");
            utilities.EnterText(buscador, umMedida);
            By opcion = By.XPath($"//div[contains(@class,'option-item')][.//text()='{umMedida}']");
            utilities.ClickButton(opcion);
        }

        public void EliminarRolPredefinido()
        {
            utilities.ClickButton(EliminarRol);
        }

        public void SeleccionarRol(string rol)
        {
            utilities.ClickButton(EliminarRol);
            utilities.ClickButton(DropdownRoles);
            By buscadorrol = By.XPath("(//input[@placeholder='Buscar...'])[last()]");
            utilities.EnterText(buscadorrol, rol);
            By opcionrol = By.XPath($"//span[normalize-space()='{rol}']");
            utilities.ClickButton(opcionrol);
            By cerrarrol = By.XPath("//span[@class='select-icon open']//i[@class='bi bi-chevron-down']");
            utilities.ClickButton(cerrarrol);
        }

        public void EliminarModuloPredefinido()
        {
            utilities.ClickButton(EliminarModulo);
        }

        public void SeleccionarModulo(string modulo)
        {
            utilities.ClickButton(EliminarModulo);
            utilities.ClickButton(DropdownModulos);
            By buscadorModulo = By.XPath("(//input[@placeholder='Buscar...'])[last()]");
            utilities.EnterText(buscadorModulo, modulo);
            By opcionModulo = By.XPath($"//span[contains(@class,'option-label') and normalize-space()='{modulo}']");
            utilities.ClickButton(opcionModulo);
            By cerrarmodulo = By.XPath("//span[@class='select-icon open']//i[@class='bi bi-chevron-down']");
            utilities.ClickButton(cerrarmodulo);
        }

        public void SeleccionarMarca(string marca)
        {
            utilities.ClickButton(DropdownMarca);
            By opcionMarca = By.XPath($"//option[normalize-space()='{marca}']");
            utilities.ClickButton(opcionMarca);
        }

        public void SeleccionarPresentacion(string presentacion)
        {
            utilities.ClickButton(DropdownPresentacion);
            By buscador = By.XPath("(//input[@placeholder='Buscar...'])[last()]");
            utilities.EnterText(buscador, presentacion);
            By opcion = By.XPath($"//span[@class='option-label' and normalize-space()='{presentacion}']");
            utilities.ClickButton(opcion);
        }

        public void IngresarCantidad(string cantidad)
        {
            utilities.ClearAndEnterText(CampoCantidad, cantidad);
        }

        public void SeleccionarUnidadMedida(string unidadMedida)
        {
            utilities.ClickButton(DropdownUnidadMedida);
            By buscador = By.XPath("(//input[@placeholder='Buscar...'])[last()]");
            utilities.EnterText(buscador, unidadMedida);
            By opcion = By.XPath($"//span[@class='option-label' and normalize-space()='{unidadMedida}']");
            utilities.ClickButton(opcion);
        }

        public void SeleccionarTarifa(string tarifa)
        {
            By opcionTarifa = By.XPath($"//label[contains(text(),'{tarifa}')]/preceding-sibling::input");
            utilities.ClickButton(opcionTarifa);
        }

        public void IngresarPrecio(string precio)
        {
            By campoPrecio = By.XPath("//table[contains(@class,'price-tier-table')]//input[@type='number' and not(@disabled)]");
            utilities.EnterText(campoPrecio, precio);
        }

        public void GuardarConcepto()
        {
            utilities.ClickButton(BotonGuardar);
            By botonok = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonok);
        }

        public void NoGuardarConcepto()
        {
            utilities.ClickButton(BotonGuardar);
            By botonok = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonok);
        }
    }
}
