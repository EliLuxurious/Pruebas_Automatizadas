using FluentAssertions.Equivalency;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Assert = NUnit.Framework.Assert;


namespace RESTAURANTE.Hoks.Pages.Facturacion
{
    public class FacturacionPage
    {

        private IWebDriver driver;
        Utilities utilities;
        WebDriverWait wait;

        public FacturacionPage(IWebDriver driver, int timeoutInSeconds = 20)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver), "El WebDriver no puede ser null.");
            }
            this.driver = driver;
            this.utilities = new Utilities(driver);
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        }

        // OVERLAY
        private By overlayLocator = By.ClassName("block-ui-overlay");

        // BOTON FACTURAR
        private By facturarAtencionButton = By.XPath("//tbody/tr[1]/td[9]/a[1]/span[1]");

        // TIPO DE FACTURACION
        private By _buttonSimple = By.Id("inlineRadio1");
        private By _buttonCtaDividida = By.Id("inlineRadio2");
        private By _buttonDiferenciado = By.Id("inlineRadio3");
        private By _buttonLocator;

        // MODAL - REGISTRAR FACTURACION

        private By _modalFacturacion = By.Id("modal-facturador-restaurante");
        private By fieldLocator;

        private By _modal = By.Id("modal-facturador-restaurante");

        //CLIENTE
        private By docIdentidadField = By.Id("DocumentoIdentidad");
        //private By docIdentidadField = By.XPath("//input[@id='DocumentoIdentidad']");

        // private By aliasField = By.XPath("//input[@ng-model='$ctrl.facturacion.Orden.Cliente.Alias']");
        private By aliasField = By.XPath("//label[contains(text(), 'ALIAS')]/following-sibling::input");


        // COMPROBANTE
        private By comprobanteSelect = By.Name("TipoComprobante");

        //OBSERVACION
        private By observacionField = By.Id("observacion");

        private By SelecOptions = By.CssSelector(".select2-results__options");

        // BOTON FACTURAR
        By _buttonFacturar = By.XPath("//button[contains(text(),'FACTURAR')]");


        public void SwitchToModal(IWebDriver driver)
        {
            // Almacena el identificador de la ventana principal
            string parentWindowHandler = driver.CurrentWindowHandle;

            // Obtén todos los identificadores de ventanas abiertas
            var handles = driver.WindowHandles;

            // Cambia a la ventana que no sea la principal
            foreach (string handle in handles)
            {
                if (handle != parentWindowHandler)
                {
                    driver.SwitchTo().Window(handle);
                    break;
                }
            }

            // Aquí puedes interactuar con el contenido del modal
            // Por ejemplo, rellenar un campo de texto:
            //driver.FindElement(By.Id("tuCampoID")).SendKeys("Información del modal");

            // Si necesitas volver a la ventana principal:
            //driver.SwitchTo().Window(parentWindowHandler);
        }

        public void typeInvoice(string _typeFactura) 
        {
            // BOTON FACTURAR - ELECCION DEL PRIMERO
            utilities.elementExists(facturarAtencionButton);
            utilities.WaitForOverlayToDisappear();
            utilities.buttonClickeable(facturarAtencionButton);

            // ENCUENTRA MODAL FACTURACION RESTAURANTE
            IWebElement modalRestaurante = driver.FindElement(_modal);
            Console.WriteLine("MODAL FACTURACION RESTAURANTE ENCONTRADO");

            // ESPERA Y OVERLAY
            utilities.elementExists(docIdentidadField);
            utilities.WaitForOverlayToDisappear();

            switch (_typeFactura)
            {
                case "SIMPLE":
                    _buttonLocator = _buttonSimple;
                    break;
                case "CUENTA DIVIDIDA":
                    _buttonLocator = _buttonCtaDividida;
                    break;
                case "DIFERENCIADO":
                    _buttonLocator = _buttonDiferenciado;
                    break;
                default:
                    throw new ArgumentException($"Tipo de facturacion {_typeFactura} no válido");
            }

            utilities.ClickButtonInModal(modalRestaurante, _buttonLocator);
            Console.WriteLine($"TIPO DE FACTURACION {_typeFactura} SELECCIONADO");

            // ESPERA PARA RELLENAR CAMPOS
            utilities.elementExists(docIdentidadField);
            utilities.WaitForOverlayToDisappear();
            /*
            List<IWebElement> iframes = driver.FindElements(By.TagName("iframe")).ToList();
            driver.SwitchTo().Frame(iframes[0]);  // Prueba con el primer iframe
            Console.WriteLine("CAMBIANDO IFRAME");
            */
        }

        
        public void EnterBillingDetails(int _ncuenta, string _clientType, string _clientValue, string _comprobante, string _observacion, string _moodPago)
        {
            // Encontrar el modal FACTURACION
            IWebElement modalFacturacion = driver.FindElement(By.Id($"facturacionVenta-{_ncuenta}"));

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
            Console.WriteLine($"CLIENTE CUENTA {_ncuenta} INGRESADO");
            Thread.Sleep(4000);

            // COMPROBANTE
            var dropdown = new SelectElement(modalFacturacion.FindElement(comprobanteSelect));
            dropdown.SelectByText(_comprobante);
            Assert.That(dropdown.SelectedOption.Text, Is.EqualTo(_comprobante));

            Console.WriteLine($"COMPROBANTE CUENTA {_ncuenta} INGRESADO");
            Thread.Sleep(4000);

            // OBSERVACION
            utilities.elementExists(observacionField);
            utilities.WaitForOverlayToDisappear();

            utilities.addFieldModal(modalFacturacion, observacionField, _observacion);
            Console.WriteLine($"OBSERVACION CUENTA {_ncuenta} INGRESADO");
            Thread.Sleep(4000);

            Dictionary<string, By> moodPagoButtons;

            if (_ncuenta == 0)
            {
                moodPagoButtons = new Dictionary<string, By>
                {
                    { "DEPCU", By.XPath($"//label[@id='labelMedioPago-{_ncuenta}-14']") },
                    { "TRANFON", By.XPath($"//label[@id='labelMedioPago-{_ncuenta}-16']") },
                    { "TDEB", By.XPath($"//label[@id='labelMedioPago-{_ncuenta}-18']") },
                    { "TCRE", By.XPath($"//label[@id='labelMedioPago-{_ncuenta}-19']") },
                    { "EF", By.XPath($"//label[@id='labelMedioPago-{_ncuenta}-281']") }
                };
            }
            else
            {
                // Diccionario que mapea el modo de pago al botón correspondiente
                moodPagoButtons = new Dictionary<string, By>
                {
                    { "DEPCU", By.XPath($"//label[@id='labelMedioPago--{_ncuenta}-14']") },
                    { "TRANFON", By.XPath($"//label[@id='labelMedioPago--{_ncuenta}-16']") },
                    { "TDEB", By.XPath($"//label[@id='labelMedioPago--{_ncuenta}-18']") },
                    { "TCRE", By.XPath($"//label[@id='labelMedioPago--{_ncuenta}-19']") },
                    { "EF", By.XPath($"//label[@id='labelMedioPago--{_ncuenta}-281']") }
                };
            }

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
            Console.WriteLine($"METODO DE PAGO {_ncuenta} INGRESADO");
            Thread.Sleep(4000);

            Console.WriteLine($"***DETALLES CUENTA {_ncuenta} INGRESADO***");
            Thread.Sleep(4000);

        }

        /*
        // MODO DE PAGO
        public void moodPay(IWebElement _modal, By _path)
        {
            if (driver.FindElements(_path).Count == 0)
            {
                throw new NoSuchElementException($"El elemento con el localizador {_path} no se encontró.");
            }

            utilities.ClickButtonInModal(_path);
        }
        */

        //MODO DE PAGO TARJETA TDEB TCRE
        public void datosBanco(IWebElement _modal, By _bankAccountDEPCU, string _cuentaBancaria, By _infoField, string _info)
        {

            // utilityPage.SelecOption(_modal, _bankAccountDEPCU, _cuentaBancaria); // SELECCION BANCO

            utilities.addFieldModal(_modal, _infoField, _info);

        }

        //MODO DE PAGO TARJETA TDEB TCRE
        public void datosCard(IWebElement _modal, By _bankField, string _bank, By _cardField, string _card, By _infoField, string _info)
        {

            // utilityPage.SelecOption(_modal, _bankField, _bank); // SELECCION BANCO
            // utilityPage.SelecOption(_modal, _cardField, _card); // SELECCION TARJETA
            utilities.addFieldModal(_modal, _infoField, _info); ; // AGREGA INFO
            
        }

        public void realizarFacturaracion()
        {
            utilities.buttonClickeable(_buttonFacturar); //  CLIC EN FACTURAR
            Thread.Sleep(4000);
        }

    }
}
