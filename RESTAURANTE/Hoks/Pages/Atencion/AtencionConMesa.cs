using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RESTAURANTE.Hoks.Pages.Atencion
{
    public class AtencionConMesaPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public AtencionConMesaPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
        }

        /*
        // ESTADO DE LAS MESAS
        private By _disponibles = By.ClassName("btn-mesa-disponible"); 
        private By _ocupadas = By.ClassName("btn btn-mesa-ocupada");
        */
        // MOZO
        private By _selectMozo = By.Id("mozo");

        // ITEM
        private By _codeBarraItem = By.XPath("//input[@id='codigoBarraItem']");
        private By _selectItem = By.Id("itemSeleccionado");
        private By _itemCantidad = By.Id("cantidadItem");
        private By _agregarItem = By.XPath("//button[@title='Agregar Item']");
        private By _anotacionItem = By.XPath("//button[@title='Agregar una Anotacion']");
        private By _eliminarItem = By.XPath("//button[@title='Eliminar Item de Orden']");

        // AMBIENTE
        private By _ambientePrincipal = By.Id("ambienteconmesa-0");
        private By _ambienteReservaciones = By.Id("ambienteconmesa-1");
        private By fieldLocator;

        public void SeleccionAmbiente(string _ambiente)
        {
            switch (_ambiente)
            {
                case "PRINCIPAL":
                    fieldLocator = _ambientePrincipal;
                    return;

                case "RESERVACIONES":

                    fieldLocator = _ambienteReservaciones;
                    break;

                default:
                    throw new ArgumentException($"El {_ambiente} no es válido");
            }

            utilities.elementExists(fieldLocator);
            Console.WriteLine("BOTON ENCONTRADO");
            utilities.buttonClickeable(fieldLocator);
        }

        public void seleccionCode(string _concepto)
        {
            utilities.SubmitTexto(_codeBarraItem, _concepto);
        }

        public void seleccionItem(string _item, string _cantidad)
        {
            // ITEM POR SELECT
            var dropdown = new SelectElement(driver.FindElement(_selectItem));
            dropdown.SelectByText(_item);
            Assert.That(dropdown.SelectedOption.Text, Is.EqualTo(_item));
            Console.WriteLine($"{_item} SELECCIONADO");
            // CANTIDAD
            utilities.InputTexto(_itemCantidad, _cantidad);
            // CHECK
            utilities.buttonClickeable(_agregarItem);
        }

        /*
        public void esperaPreparacion()
        {
            /*
            utilities.elementExists(By.Id("ambienteconmesa-0"));
            Console.WriteLine("BOTON ENCONTRADO");
            utilities.WaitForOverlayToDisappear();
            
            }
        */

        public void seleccionMesa(string _mesa)
        {
            //body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/button[1]
            Console.WriteLine("SELECCION DE MESA INGRESADO");

            Thread.Sleep(10000);

            utilities.buttonClickeable(By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/button[1]"));
            // Seleccionar todos los botones de las mesas disponibles
            // Esperar hasta que los botones de mesa estén visibles
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var mesas = wait.Until(d => d.FindElements(By.XPath("//button[contains(@class, 'btn-mesa-disponible') and span]")));


            if (mesas.Count == 0)
            {
                Console.WriteLine("No se encontraron mesas disponibles.");
                return;
            }

            // Buscar la mesa con el número 6
            foreach (var mesa in mesas)
            {
                try
                {
                    var span = mesa.FindElement(By.TagName("span"));
                    var textoMesa = span.Text.Trim();

                    Console.WriteLine($"Mesa encontrada con número: {textoMesa}");

                    if (textoMesa == "6") // Verificar si el texto coincide
                    {
                        mesa.Click();
                        Console.WriteLine("✅ MESA 6 SELECCIONADA");
                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("No se encontró el número dentro de la mesa.");
                }
            }

        }

        public void TomarOrden(int _nMesa, string _mozo)
        {
            Thread.Sleep(10000);
            Console.WriteLine("TOMAR ORDEN");
            /*
            // Seleccionar todos los botones de las mesas disponibles
            var mesas = driver.FindElements(_disponibles);
            foreach (var mesa in mesas)
            {
                if (mesa.Text.Contains($"{_nMesa}"))
                {
                    mesa.Click();
                    break;
                }
            }
            */
            Thread.Sleep(4000);

            // SELECCION MOZO
            var dropdown = new SelectElement(driver.FindElement(_selectMozo));
            dropdown.SelectByText(_mozo);
            Assert.That(dropdown.SelectedOption.Text, Is.EqualTo(_mozo));
            Console.WriteLine($"{_mozo} SELECCIONADO");
            Thread.Sleep(5000);

            // IWebElement modalFacturacion = driver.FindElement(By.Id($"facturacionVenta-{_ncuenta}"));
        }

        public void agregarOrden( string _orden, string _concepto, string _cantidad, string _anotacion)
        {
            // TIPO ORDEN
            switch (_orden)
            {
                case "CODIGO":
                    seleccionCode( _concepto);
                    return;

                case "ITEM":

                    seleccionItem(_concepto, _cantidad);
                    break;

                default:
                    throw new ArgumentException($"La tipo de orden por {_orden} no es válido");
            }

            Thread.Sleep(2000);
        }


    }
}
