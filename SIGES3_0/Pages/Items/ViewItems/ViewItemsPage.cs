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

        public void MostrarTodosLosConceptos()
        {
            try
            {
                By desplegablePaginacion = By.XPath("//app-table-row-filter//select[@class='form-select custom-input']");
                By opcionCien = By.XPath("//app-table-row-filter//select[@class='form-select custom-input']/option[@value='100']");

                utilities.ClickButton(desplegablePaginacion);
                Thread.Sleep(300);
                utilities.ClickButton(opcionCien);
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se encontró o no fue necesario ajustar el paginador de conceptos: " + ex.Message);
            }
        }

        public void EditarConcepto(string nombreConcepto)
        {
            try
            {
                // 1. Mostrar todos los registros
                MostrarTodosLosConceptos();

                // 2. Botón editar dinámico
                By botonEditar = By.XPath($"//tbody//tr[td[contains(normalize-space(),'{nombreConcepto}')]]//app-button-actions//button[1]");

                var elementos = driver.FindElements(botonEditar);

                if (elementos.Count > 0)
                {
                    utilities.ClickButton(botonEditar);
                    Thread.Sleep(500);
                }
                else
                {
                    throw new Exception($"No se encontró el concepto '{nombreConcepto}' en la tabla.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar editar el concepto: " + ex.Message);
                throw;
            }
        }

        public void EliminarConcepto(string nombreConcepto)
        {
            try
            {
                // 1. Mostrar todos los registros
                MostrarTodosLosConceptos();

                // 2. Botón eliminar dinámico
                By botonEliminar = By.XPath($"//tbody//tr[td[contains(normalize-space(),'{nombreConcepto}')]]//app-button-actions//button[2]");

                var elementos = driver.FindElements(botonEliminar);

                if (elementos.Count > 0)
                {
                    utilities.ClickButton(botonEliminar);
                    Thread.Sleep(500);

                    // 3. Confirmar eliminación
                    By botonConfirmar = By.XPath("//button[normalize-space()='Sí, ¡elimínalo!']");
                    utilities.ClickButton(botonConfirmar);
                }
                else
                {
                    throw new Exception($"No se encontró el concepto '{nombreConcepto}' para eliminar.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar el concepto: " + ex.Message);
                throw;
            }
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
