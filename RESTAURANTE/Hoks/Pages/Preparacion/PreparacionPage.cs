using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RESTAURANTE.Hoks.Pages.Facturacion;
using static OpenQA.Selenium.BiDi.Modules.Input.Wheel;
using OpenQA.Selenium.Interactions;

namespace RESTAURANTE.Hoks.Pages.Preparacion
{
    public class PreparacionPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public PreparacionPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
        }

        // CONTENEDOR CARTILLAS ORDENES
        private By contenedorCartillas = By.ClassName("contenedor-cartillas");
        private By detallesOrdenes = By.ClassName("boton-detalle-orden");

        // AMBIENTES
        private By ambienteTodos = By.Id("ambiente0");
        private By ambientePrincipal = By.Id("ambiente3554");
        private By ambienteReservaciones = By.Id("ambiente24168");

        // ACCIONES
        private By prepararButton = By.Id("boton-preparar");
        private By servirButton = By.Id("boton-servir");
        private By refrescarButton = By.XPath("//button[@title='Refrescar']");
        private By fieldLocator;

        public IList<IWebElement> scrollCartilla(string _nOrden)
        {
            IWebElement scrollableElement = driver.FindElement(contenedorCartillas);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Encuentra la cartilla específica
            IWebElement cartillaEspecifica = scrollableElement.FindElement(By.XPath($"//div[contains(@class, 'box-primary') and .//h3[contains(text(),'{_nOrden}')]]"));

            // Desplaza la cartilla hasta la vista
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'nearest', inline: 'center'});", cartillaEspecifica);


            // Encuentra y devuelve las órdenes dentro de la cartilla
            return cartillaEspecifica.FindElements(detallesOrdenes);
        }

        public void seleccionAmbiente(string _ambiente)
        {
            switch (_ambiente)
            {
                case "PRINCIPAL":
                    fieldLocator = ambientePrincipal;
                    return;

                case "RESERVACIONES":

                    fieldLocator = ambienteReservaciones;
                    break;

                default:
                    throw new ArgumentException($"El {_ambiente} no es válido");
            }

            utilities.elementExists(fieldLocator);
            Console.WriteLine("BOTON ENCONTRADO");
            utilities.WaitForOverlayToDisappear(); // OVERLAY
            utilities.buttonClickeable(fieldLocator);
        }

        public void esperaPreparacion()
        {
            utilities.elementExists(ambientePrincipal);
            Console.WriteLine("BOTON ENCONTRADO");
            utilities.WaitForOverlayToDisappear(); // OVERLAY
        }
        public void seleccionOrdenes(string _nOrden)
        {
            //seleccionAmbiente("");

            utilities.elementExists(ambientePrincipal);
            Console.WriteLine("BOTON ENCONTRADO");
            utilities.WaitForOverlayToDisappear(); // OVERLAY

            IList<IWebElement> ordenes = scrollCartilla(_nOrden);

            // ordenes[0].Click(); // SOLO EN UNO
            foreach (var orden in ordenes)
            {
                orden.Click();
                Thread.Sleep(2000);
            }
        }

        public void seleccionOrden (string _nOrden, string _item)
        {
            utilities.elementExists(ambientePrincipal);
            Console.WriteLine("BOTON ENCONTRADO");
            utilities.WaitForOverlayToDisappear(); // OVERLAY

            // BUSCAR ORDEN EN LA CARTILLA
            IList<IWebElement> ordenes = scrollCartilla(_nOrden);
            IWebElement orden = ordenes.FirstOrDefault(o => o.GetAttribute("textContent").Trim().Contains(_item));
            orden.Click();

            // IWebElement ordenPolloMani = ordenes.FirstOrDefault(o => o.Text.Trim().Contains("MENU POLLO CON MANI"));

            // BUSCAR TODAS LAS CARTILLAS
            // IList<IWebElement> cartillas = scrollableElement.FindElements(By.ClassName("box-primary"));

            // IWebElement cartillaOrden = scrollableElement.FindElement(By.XPath("//div[h3[contains(text(),'C001 - 125339')]]"));
            // IWebElement cartillaEspecifica = scrollableElement.FindElements(By.ClassName("box-primary")).FirstOrDefault(cartilla => cartilla.Text.Contains("C001 - 125339"));

            /*
            // Ejecutar JavaScript para hacer scroll dentro del contenedor
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollLeft += 500;", scrollableElement); // Baja 300px
            */
            // js.ExecuteScript("arguments[0].scrollIntoView(true);", elemento);
        }

        public void accionRealizar(string _accion)
        {
            switch (_accion)
            {
                case "Preparar":
                    fieldLocator = prepararButton;
                    break;
                case "Servir":
                    fieldLocator = servirButton;
                    break;
                default:
                    throw new ArgumentException($"Accion {_accion} no válido");
            }

            utilities.buttonClickeable(fieldLocator);
            Console.WriteLine($"Accion {_accion} realizada");
            Thread.Sleep(2000);

            /*
            //CLICK EN REFRESCAR
            utilities.elementExists(refrescarButton);
            utilities.WaitForOverlayToDisappear();
            utilities.buttonClickeable(refrescarButton);
            Thread.Sleep(4000);
            */
        }
    }
}
