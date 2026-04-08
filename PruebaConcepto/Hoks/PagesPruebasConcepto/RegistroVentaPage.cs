using FluentAssertions.Equivalency;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Xml;
using NUnit.Framework;

namespace PruebaConcepto.Hoks.PagesPruebaConcepto
{
    public class RegistroVentaPage
    {
        private IWebDriver driver;

        public RegistroVentaPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        By SelecOptions = By.CssSelector(".select2-results__options");
        
        // Element locators
        By salesButton = By.XPath("//span[contains(text(),'Venta')]");
        By newSaleButton = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[1]/ul[1]/li[1]/a[1]");
        By codeBarraField = By.XPath("//input[@id='idCodigoBarra']");
        By dniField = By.XPath("//input[@id='DocumentoIdentidad']");
        By comprobanteField= By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[6]/selector-comprobante[1]/div[1]/ng-form[1]/div[1]/div[1]/span[1]/span[1]/span[1]");

        //METODO 1

        // TDEB
        By bankFieldTdeb = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[6]/div[1]/span[1]/span[1]/span[1]");

        By cardFieldTdeb = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[6]/div[1]/span[2]/span[1]/span[1]");

        By infoMethod1FieldTdeb = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[6]/div[1]/textarea[1]");

        // TCRED
        By bankFieldTcred = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[5]/div[1]/span[1]/span[1]/span[1]");

        By cardFieldTcred = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[5]/div[1]/span[2]/span[1]/span[1]");

        By infoMethod1FieldTcred = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[5]/div[1]/textarea[1]");


        //METODO 2

        //DEPCU
        By bankAccountFieldDepcu = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[8]/div[1]/span[1]/span[1]/span[1]");

        By infoMethod2FieldDepcu = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[8]/div[1]/textarea[1]");

        //TRANFON
        By bankAccountFieldTranfon = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[7]/div[1]/span[1]/span[1]/span[1]");

        //METODO 2

        //EFECTIVO
        By infoMethod2FieldTranfon = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[7]/div[1]/textarea[1]");

        By observacionFieldEf = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[2]/div[1]/textarea[1]");


        By saveSaleButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]");

        public void EnterField(By _path, string _field)
        {
            driver.FindElement(_path).Clear();
            driver.FindElement(_path).SendKeys(_field);
            driver.FindElement(_path).SendKeys(Keys.Enter);
        }

        public void ClickButton(By _button)
        {
            driver.FindElement(_button).Click();
        }

        By Almacen = By.XPath("//span[contains(text(),'Almacén')]");
        By guiasderemision = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[7]/ul[1]/li[4]/a[1]");
        By nuevo = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[6]/button[1]");
        By documento = By.Id("DocumentoIdentidad");


        public void nuevaguia(string dni)
        {

            ClickButton(Almacen);
            Thread.Sleep(4000);

            ClickButton(guiasderemision);
            Thread.Sleep(4000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            ClickButton(nuevo);
            Thread.Sleep(4000);

            // Encontrar el modal FACTURACION
            IWebElement modalFacturacion = driver.FindElement(By.Id("modal-registro-guia-remision"));

            EnterField(documento, dni);
            Thread.Sleep(4000);

        }
        // Método para realizar el inicio de sesión completo
        public void CompleteFields(string codeBarra, string dni)
        {
            ClickButton(salesButton);
            Thread.Sleep(2000);

            ClickButton(newSaleButton);
            Thread.Sleep(4000);

            EnterField(codeBarraField, codeBarra);
            Thread.Sleep(4000);

            EnterField(dniField, dni);
            Thread.Sleep(4000);
        }

        public string VerMedioPago()
        {
            // Encuentra el contenedor con los botones de radio
            var medioPagoContainer = driver.FindElement(By.Id("medioPago0"));

            // Encuentra todos los botones de radio dentro del contenedor
            var radioButtons = medioPagoContainer.FindElements(By.CssSelector("input[type='radio']"));

            // Itera por cada botón de radio para verificar cuál está seleccionado
            foreach (var radioButton in radioButtons)
            {
                if (radioButton.Selected) // Verifica si el botón está seleccionado
                {
                    // Encuentra el label asociado al botón de radio seleccionado
                    var label = driver.FindElement(By.CssSelector($"label[for='{radioButton.GetAttribute("id")}']"));

                    // Retorna el texto del label (DEPCU, TRANFON, etc.)
                    return label.Text;
                }
            }

            // Si no se selecciona nada, retorna una cadena vacía o lanza una excepción si es necesario
            return string.Empty;
        }

        public void SelecOption(By _path, string option)
        {
            try
            {
                // Esperar que el menú desplegable sea visible
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementIsVisible(_path));

                // Abre el menú desplegable
                IWebElement dropdown = driver.FindElement(_path);
                dropdown.Click();

                // Espera explícita para que las opciones sean visibles
                wait.Until(ExpectedConditions.ElementIsVisible(SelecOptions));

                // Selecciona la opción deseada
                IWebElement optionElement = driver.FindElement(By.XPath($"//li[contains(text(), '{option}')]"));
                optionElement.Click();
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Error: No se encontró la opción '{option}' en el menú desplegable. Detalle: {ex.Message}");
            }
        }

        public void tipoComprobante(string _comprobante)
        {
            SelecOption(comprobanteField, _comprobante); // SELECCION COMPROBANTE
            Thread.Sleep(4000);
        }


        // METODO DE PAGO TARJETA
        public void PaymentMethod1( string typeBank, string typeCard, string info)
        {
            string medioPagoSeleccionado = VerMedioPago();

            switch (medioPagoSeleccionado)
            {
                case "TDEB":

                    SelecOption(bankFieldTdeb, typeBank); // SELECCION BANCO
                    SelecOption(cardFieldTdeb, typeCard); // SELECCION TARJETA

                    EnterField(infoMethod1FieldTdeb, info); // AGREGA INFO
                    Thread.Sleep(4000);
              
                    break;

                case "TCRE":

                    SelecOption(bankFieldTcred, typeBank); // SELECCION BANCO
                    SelecOption(cardFieldTcred, typeCard); // SELECCION TARJETA

                    EnterField(infoMethod1FieldTcred, info); // AGREGA INFO
                    Thread.Sleep(4000);

                    break;
                default:
                    throw new ArgumentException($"El tipo de pago {medioPagoSeleccionado} no es válido.");
            }
        }

        //METODO PAGO BANCO
        public void PaymentMethod2(string _bank, string info)
        {
            string medioPagoSeleccionado = VerMedioPago();

            switch (medioPagoSeleccionado)
            {
                case "DEPCU":

                    SelecOption(bankAccountFieldDepcu, _bank); // CUENTA

                    EnterField(infoMethod2FieldDepcu, info); // AGREGA INFO
                    Thread.Sleep(4000);

                    break;

                case "TRANFON":

                    SelecOption(bankAccountFieldTranfon, _bank); // CUENTA

                    EnterField(infoMethod2FieldTranfon, info); // AGREGA INFO
                    Thread.Sleep(4000);

                    break;
                default:
                    throw new ArgumentException($"El tipo de pago {medioPagoSeleccionado} no es válido.");
            }
        }

        // METODO DE PAGO EFECTIVO
        public void PaymentMethod3(string _observacion)
        {
            string medioPagoSeleccionado = VerMedioPago();

            if (medioPagoSeleccionado == "EF")
            {
                EnterField(observacionFieldEf, _observacion); // AGREGA OBSERVACION
                Thread.Sleep(4000);
            }
            else
            {
                throw new ArgumentException($"El tipo de pago {medioPagoSeleccionado} no es válido.");
            }
        }

        public void SaveSale()
        {
            ClickButton(saveSaleButton); // GUARDAR VENTA
            Thread.Sleep(2000);
        }

        /*
        public void SelecOption(By _path, string option)
        {
            // Esperar que el menú desplegable sea visible
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(_path));
            IWebElement dropdownPath = driver.FindElement(_path);

            // Encuentra el elemento del menú desplegable
            IWebElement dropdownOptions = driver.FindElement(SelecOptions);

            Thread.Sleep(4000);
            EnterField(infoTdebField, info);
            Thread.Sleep(4000);
            ClickButton(saveSaleButton);
            Thread.Sleep(2000);

            // Haz clic en la opción deseada
            optionElement.Click();

        }
        */
    }
}
