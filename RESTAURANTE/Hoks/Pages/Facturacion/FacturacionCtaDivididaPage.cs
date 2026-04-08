using MongoDB.Bson.Serialization.Serializers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V130.Debugger;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTAURANTE.Hoks.Pages.Facturacion
{
    public class FacturacionCtaDivididaPage
    {
        private IWebDriver driver;
        Utilities utilities;
        FacturacionPage facturacionPage;

        public FacturacionCtaDivididaPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
            this.facturacionPage = new FacturacionPage(driver);

        }

        private By _modalFacturacion = By.Id("modal-facturador-restaurante");
        private By overlayLocator = By.ClassName("block-ui-overlay");
        private By fieldLocator;

        //CLIENTE
        private By docIdentidadField = By.Id("DocumentoIdentidad");
        // private By aliasField = By.XPath("//input[@ng-model='$ctrl.facturacion.Orden.Cliente.Alias']");
        private By aliasField = By.XPath("//label[contains(text(), 'ALIAS')]/following-sibling::input");


        // COMPROBANTE
        private By comprobanteSelect = By.Name("TipoComprobante");

        //OBSERVACION
        private By observacionField = By.Id("observacion");

        // AGREGAR CARD PAGOS
        private By addCardPagos = By.ClassName("agregar-card-pagos");
        private By deleteCardPagos = By.ClassName("close");
        private By dividirMonto = By.XPath("//button[@title='Dividir Monto']");

        public void addCard(int _ncuenta)
        {
            // ENCUENTRA MODAL FACTURACION RESTAURANTE
            IWebElement modalFacturacion = driver.FindElement(_modalFacturacion);
            Console.WriteLine("MODAL FACTURACION RESTAURANTE ENCONTRADO");
            //utilities.ClickButtonInModal(modalFacturacion, addCardPagos);

            for (int i = 2; i < _ncuenta; i++)
            {
                utilities.ClickButtonInModal(modalFacturacion, addCardPagos);
                utilities.elementExists(docIdentidadField);
                utilities.WaitForOverlayToDisappear();

                // Encontrar la ultima carta
                IWebElement ultimaCarta = modalFacturacion.FindElement(By.Id($"facturacionVenta-{i}"));

            }


            IWebElement ncard = driver.FindElement(By.Id("elementId")); // Encuentra el elemento
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", ncard); // Desplázate hasta el elemento



            /*
            // Desplazarse hacia abajo
            ((IJavaScriptExecutor)modalFacturacion).ExecuteScript("window.scrollBy(0, 1000);");

            // Desplazarse hacia la derecha
            ((IJavaScriptExecutor)modalFacturacion).ExecuteScript("window.scrollBy(1000, 0);");
            */
            /*
            // Si _ncuenta es mayor a 2, presionar el botón las veces necesarias
            for (int i = 0; i < vecesAPresionar; i++)
            {
                utilities.ClickButtonInModal(modalFacturacion, addCardPagos);
                utilities.buttonClickeable(addCardPagos);
            }
            */
            Thread.Sleep(4000);
        }

    }
}