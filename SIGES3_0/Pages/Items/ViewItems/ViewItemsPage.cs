using AventStack.ExtentReports.Model;
using OpenQA.Selenium;
using SIGES3_0.Pages.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGES3_0.Pages.Items.ViewItems
{
    public class ViewItemsPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public ViewItemsPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // MENU CONCEPTOS
        private By conceptosMenu = By.XPath("//span[text()='Conceptos']/following::input[1]");

        // SUBMENU
        private By VerConceptos = By.XPath("//a[@href='/business-item/ViewBusinessItem']");

        //DROPDOWN FAMILIA
        private By DropdownFamilia = By.XPath("//button[normalize-space()='Seleccione una familia']");

        //DROPDOWN CATEGORIA
        private By DropdownCategoria = By.XPath("//button[normalize-space()='Seleccione una categoría']");

        //PALABRA CLAVE
        private By PalabraClave = By.XPath("//input[@placeholder='Ingrese palabras claves']");

        //BOTON BUSCAR
        private By BotonBuscar = By.XPath("//i[@class='bi bi-search']");

        //BOTON LIMPIAR
        private By BotonLimpiar = By.XPath("//button[.//i[contains(@class,'bi-x')]]");

        // FILAS DE LA TABLA
        private By FilasTabla = By.XPath("//tbody/tr");



        public void AbrirModuloConceptos()
        {
            utilities.ClickButton(conceptosMenu);
        }

        public void SeleccionarVerConcepto()
        {
            utilities.ClickButton(VerConceptos);
        }

        public void SeleccionarFamilia(string familia)
        {
            utilities.ClickButton(DropdownFamilia);
            By opcionfamilia = By.XPath($"//a[normalize-space()='{familia}']");
            utilities.ClickButton(opcionfamilia);
        }

        public void SeleccionarCategoria(string categoria)
        {
            utilities.ClickButton(DropdownCategoria);
            By opcioncategoria = By.XPath($"//a[normalize-space()='{categoria}']");
            utilities.ClickButton(opcioncategoria);
        }

        public void IngresarPalabraClave(string clave)
        {
            utilities.EnterText(PalabraClave, clave);
        }

        public void HacerBusqueda()
        {
            utilities.ClickButton(BotonBuscar);
        }

        public void LimpiarBusqueda()
        {
            utilities.ClickButton(BotonLimpiar);
        }

        public bool HayResultados()
        {
            return driver.FindElements(FilasTabla).Count > 0;
        }

        public bool NoHayResultados()
        {
            return driver.FindElements(FilasTabla).Count == 0;
        }

    }
}
