using FluentAssertions.Equivalency;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.ComponentModel;
using Assert = NUnit.Framework.Assert;


namespace RESTAURANTE.Hoks.Pages.Facturacion
{
    public class FacturacionSimplePage
    {
        private IWebDriver driver;
        Utilities utilities;
        FacturacionPage facturacionPage;

        public FacturacionSimplePage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
            this.facturacionPage = new FacturacionPage(driver);

        }

        private By overlayLocator = By.ClassName("block-ui-overlay");
        private By _modalFacturacion = By.Id("facturacionVenta-0");
        private By fieldLocator;

        //CLIENTE
        private By docIdentidadField = By.XPath("//input[@id='DocumentoIdentidad']");
        private By aliasField = By.XPath("//input[@ng-model='$ctrl.facturacion.Orden.Cliente.Alias']");

        // COMPROBANTE
        private By comprobanteSelect = By.Name("TipoComprobante");

        //OBSERVACION
        private By observacionField = By.Id("observacion");
       
        // MODO DE PAGO
        By dpcuButton = By.XPath("//label[@id='labelMedioPago-0-14']");
        By tranfonButton = By.XPath("//label[@id='labelMedioPago-0-16']");
        By tdebButton = By.XPath("//label[@id='labelMedioPago-0-18']");
        By tcreButton = By.XPath("//label[@id='labelMedioPago-0-19']");
        By efButton = By.XPath("//label[@id='labelMedioPago-0-281']");

        // DEPCU
        By _bankAccountDEPCU = By.XPath("");
        By _infoDEPCU = By.Id("informacion");

        // TRANFON
        By _bankAccountTRANFON = By.XPath("");
        By _infoTRANFON = By.Id("informacion");

        // TDEB 
        // By _bankTDEB = By.Id("idEntidadFinancera");
        By _bankTDEB = By.XPath("//select[@id='idEntidadFinancera']");

        By _cardTDEB = By.XPath("//body/div[5]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[3]/div[1]/div[1]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[7]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[6]/div[1]/span[2]/span[1]/span[1]");

        By _infoTDEB = By.XPath("//body/div[5]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[3]/div[1]/div[1]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[7]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[6]/div[1]/textarea[1]");

        // TCRED
        By _bankTCRED = By.XPath("");
        By _cardTCRED = By.XPath("");
        By _infoTCRED = By.XPath("");

        //  EF
        By _recibido = By.XPath("");
        By _observation = By.XPath("");

        

        public IWebElement inicioModal()
        {
            // Encontrar el modal FACTURACION
            IWebElement modalFacturacion = driver.FindElement(_modalFacturacion);
            return modalFacturacion;
        }


        // EXTRAE EL MODO DE PAGO SELECCIONADO
        public string VerModoPago()
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
            return string.Empty;// Si no se selecciona nada, retorna una cadena vacía o lanza una excepción si es necesario
        }

        public void EnterBillingDetails(string _clientType, string _clientValue, string _comprobante, string _observacion, string _moodPago)
        {
            // Encontrar el modal FACTURACION
            IWebElement modalFacturacion = driver.FindElement(By.Id("facturacionVenta-0"));

            // TIPO CLIENTE
            switch (_clientType)
            {
                case "VARIOS":
                    return;

                case "DNI":
                case "RUC":

                    fieldLocator = docIdentidadField; // CAMPO DOC INDENTIDAD
                    break;

                case "ALIAS":

                    fieldLocator = aliasField; // CAMPO ALIAS
                    break;

                default:
                    throw new ArgumentException($"El {_clientType} no es válido");
            }

            utilities.InputTextoModal(modalFacturacion, fieldLocator, _clientValue);
            Console.WriteLine("CLIENTE INGRESADO");
            Thread.Sleep(4000);

            // COMPROBANTE
            var dropdown1 = new SelectElement(modalFacturacion.FindElement(comprobanteSelect));
            dropdown1.SelectByText(_comprobante);
            Assert.That(dropdown1.SelectedOption.Text, Is.EqualTo(_comprobante));

            Console.WriteLine("COMPROBANTE SELECCIONADO");
            Thread.Sleep(4000);

            // OBSERVACION
            utilities.elementExists(observacionField);
            utilities.WaitForOverlayToDisappear();

            utilities.addFieldModal(modalFacturacion, observacionField, _observacion);
            Console.WriteLine("OBSERVACION AGREGADA");
            Thread.Sleep(4000);

            // Diccionario que mapea el modo de pago al botón correspondiente
            var moodPagoButtons = new Dictionary<string, By>
            {
                { "DEPCU", dpcuButton },
                { "TRANFON", tranfonButton },
                { "TDEB", tdebButton },
                { "TCRE", tcreButton },
                { "EF", efButton }
            };

            // Verificar si el modo de pago existe en el diccionario
            if (moodPagoButtons.ContainsKey(_moodPago))
            {
                // Llamar al método moodPay con el botón correspondiente
                utilities.ClickButtonInModal(modalFacturacion, moodPagoButtons[_moodPago]);
            }
            else
            {
                // Lanzar excepción si el modo de pago no es válido
                throw new ArgumentException($"El {_moodPago} no es válido");
            }
            Console.WriteLine("MODO DE PAGO SELECCIONADA");
            Thread.Sleep(4000);
        }

        // TIPO COMPROBANTE DE PAGO
        public void typeComprobante(string _comprobante)
        {

            Thread.Sleep(4000);
            // Encontrar el modal FACTURACION
            IWebElement modalFacturacion = driver.FindElement(By.Id("facturacionVenta-0"));

            /*
            IWebElement dropdown = modalFacturacion.FindElement(By.Name("TipoComprobante"));
            Console.WriteLine("Campo seleccion encontrada");
            IReadOnlyCollection<IWebElement> dropdownOptions = dropdown.FindElements(By.TagName("option"));
            Console.WriteLine("Slecciones extraidas");
            foreach (IWebElement option in dropdownOptions)
            {
                if (option.Text.Equals(_comprobante))
                    option.Click();
            }
            string selectedOption = "";
            foreach (IWebElement option in dropdownOptions)
            {
                if (option.Selected)
                    selectedOption = option.Text;
            }
            Assert.That(selectedOption, Is.EqualTo(_comprobante));
            */

            var dropdown1 = new SelectElement(modalFacturacion.FindElement(By.Name("TipoComprobante")));
            dropdown1.SelectByText(_comprobante);
            Assert.That(dropdown1.SelectedOption.Text, Is.EqualTo(_comprobante));

            Console.WriteLine("Seleccion realizada");
            Thread.Sleep(4000);
        
        }


        // MODO DE PAGO
        public void moodPay(string _moodPago)
        {
            // Diccionario que mapea el modo de pago al botón correspondiente
            var moodPagoButtons = new Dictionary<string, By>
            {
                { "DEPCU", dpcuButton },
                { "TRANFON", tranfonButton },
                { "TDEB", tdebButton },
                { "TCRE", tcreButton },
                { "EF", efButton }
            };

            // Verificar si el modo de pago existe en el diccionario
            if (moodPagoButtons.ContainsKey(_moodPago))
            {
                // Llamar al método moodPay con el botón correspondiente
                // facturacionPage.moodPay(moodPagoButtons[_moodPago]);
            }
            else
            {
                // Lanzar excepción si el modo de pago no es válido
                throw new ArgumentException($"El {_moodPago} no es válido");
            }

            Thread.Sleep(4000);
        }

        // DATOS DE PAGO BANCO - 2 CAMPOS
        public void PaymentBank(string _cuentaBancaria, string _info)
        {
            string modoPagoSeleccionado = VerModoPago();

            Console.WriteLine($"{modoPagoSeleccionado}");

            IWebElement modalFacturacion = inicioModal();

            switch (modoPagoSeleccionado)
            {
                case "DEPCU":

                    // Llenar los datos bancarios del formulario de facturación
                    // facturacionPage.datosBanco(modalFacturacion, _bankAccountDEPCU, _cuentaBancaria, _infoDEPCU, _info);

                    


                    Thread.Sleep(4000);
                    break;

                case "TRANFON":
                    facturacionPage.datosBanco(modalFacturacion, _bankAccountTRANFON, _cuentaBancaria, _infoDEPCU, _info);
                    break;
                default:
                    throw new ArgumentException($"El modo de pago {modoPagoSeleccionado} no es válido.");
            }
            Thread.Sleep(4000);
        }

        // DATOS DE PAGO TARJETA - 3 CAMPOS
        public void PaymentCard(string _bank, string _card, string _info)
        {
            string modoPagoSeleccionado = VerModoPago();

            Console.WriteLine($"EL MODO DE PAGO SELECCIONADO EN EL PASO ANTERIOR ES {modoPagoSeleccionado}");

            // IWebElement modalFacturacion = inicioModal();
            // Encontrar el modal FACTURACION

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            /*
            IWebElement modalFacturacion = driver.FindElement(By.Id("facturacionVenta-0"));
            IWebElement modalPago = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("EditorPago")));
            */
            IWebElement modalFacturacion = driver.FindElement(By.Id("facturacionVenta-0"));
            // IWebElement modalPago = modalFacturacion.FindElement(By.Id("EditorPago"));
            IWebElement modalPago = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("EditorPago")));
            Console.WriteLine("MODAL Pago");
            Thread.Sleep(4000);
            // Esperar hasta que el elemento sea interactuable
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idEntidadFinancera")));

            Console.WriteLine("ELEMENTO CLICKEABLE");
            Thread.Sleep(2000);
            switch (modoPagoSeleccionado)
            {
                case "TDEB":

                    var dropdown1 = new SelectElement(modalPago.FindElement(By.Id("idEntidadFinancera")));
                    Console.WriteLine("ENCONTRADO");
                    Thread.Sleep(4000);
                    dropdown1.SelectByValue("385");
                    Assert.That(dropdown1.SelectedOption.Text, Is.EqualTo("385"));


                    // utilities.SelecOption(modalFacturacion, comprobanteField1, _comprobante);
                    Console.WriteLine("Seleccion realizada");
                    Thread.Sleep(4000);


                    /*
                    IWebElement select2Container = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".select2-selection")));
                    select2Container.Click(); // Abre el dropdown


                    IWebElement option = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//li[contains(text(), '{_bank}')]")));
                    option.Click();
                    */

                    /*
                    utilities.SelecOption();
                    //facturacionPage.datosCard(modalFacturacion, _bankTDEB, _bank, _cardTDEB, _card, _infoTDEB, _info);
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    IWebElement dropdownElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idEntidadFinancera")));
                    Console.WriteLine("Es visible");
                    var dropdown2 = new SelectElement(modalPago.FindElement(By.Id("idEntidadFinancera")));
                    Console.WriteLine("Campo selección encontrada");
                    Thread.Sleep(4000);
                    dropdown2.SelectByText(_bank);
                    Assert.That(dropdown2.SelectedOption.Text, Is.EqualTo(_bank));
                    Thread.Sleep(4000);
                    */
                    /*
                    IWebElement dropdown = modalPago.FindElement(By.Id("idEntidadFinancera"));
                    Console.WriteLine("Campo selección encontrada");
                    Thread.Sleep(4000);
                    /*
                    // Espera explícita para que el dropdown esté interactuable
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(dropdown));
                    */
                    /*
                    SelectElement select = new SelectElement(dropdown);
                    Console.WriteLine($"Valor de _bank: '{_bank}' (longitud: {_bank.Length})");
                    Thread.Sleep(4000);
                    // Intentar seleccionar por texto
                    try
                    {
                        select.SelectByText(_bank.Trim());
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine("No se encontró la opción por texto, intentando por valor...");
                        select.SelectByValue("385"); // Ajusta el valor correcto
                    }

                    // Si `SelectElement` no funciona, intentar con JavaScript
                    if (select.SelectedOption.Text != _bank.Trim())
                    {
                        Console.WriteLine("Intentando con JavaScript...");
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("arguments[0].value = '385';", dropdown);
                        js.ExecuteScript("arguments[0].dispatchEvent(new Event('change'))", dropdown);
                    }

                    // Confirmar la selección
                    string selectedOption = select.SelectedOption.Text;
                    Console.WriteLine($"Opción seleccionada: {selectedOption}");
                    Assert.That(selectedOption, Is.EqualTo(_bank.Trim()));
                    */

                    /*
                    foreach (IWebElement option in dropdownOptions)
                    {
                        if (option.Text.Equals(_bank))
                            option.Click();
                        Console.WriteLine("CLICK");
                    }*/


                    break;
                case "TCRE":
                    facturacionPage.datosCard(modalFacturacion, _bankTCRED, _bank, _cardTCRED, _card, _infoTCRED, _info);
                    break;
                default:
                    throw new ArgumentException($"El modo de pago {modoPagoSeleccionado} no es válido.");
            }
            Thread.Sleep(4000);
        }

        
        /*
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(dropdown));
                    Console.WriteLine("CARGADAS");
                    */
    }
}
