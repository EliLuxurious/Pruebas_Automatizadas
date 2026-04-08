using OpenQA.Selenium;


namespace RESTAURANTE.Hoks.Pages
{
    public class AccessPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public AccessPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
        }

        // LOGIN
        private By usernameField = By.XPath("//input[@id='Email']"); // campo de usuario
        private By passwordField = By.XPath("//input[@id='Password']"); // campo de contraseña
        private By loginButton = By.XPath("//button[contains(text(),'Iniciar')]"); // botón de inicio de sesión 
        private By acceptButton = By.XPath("//button[contains(text(),'Aceptar')]"); // botón de aceptar

        // INGRESO MODULO RESTAURANTE
        private By restaurantField = By.XPath("//a[@class='menu-lista-cabecera']/span[text()='Restaurante']");
        private By AttentionField = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[2]/ul[1]/li[1]/a[1]");
        private By preparationField = By.CssSelector("a[href='/Restaurante/Preparacion']");
        // private By preparationField = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[2]/ul[1]/li[2]/a[1]");
        private By boxField = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[2]/ul[1]/li[3]/a[1]");
        private By complementsField = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[2]/ul[1]/li[3]/a[1]");
        private By reportsField = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[2]/ul[1]/li[5]/a[1]");


        // INICIO DE SESION
        public void LoginToApplication(string _user, string _password)
        {
            driver.Url = "https://tintoymadero-qa.sigesonline.com/";
            utilities.InputTexto(usernameField, _user); // campo user
            utilities.InputTexto(passwordField, _password); // campo contraseña

            utilities.buttonClickeable(loginButton); // boton login
            utilities.buttonClickeable(acceptButton); // boton aceptar
        }


        public void enterModulo(string _modulo)
        {
            utilities.buttonClickeable(restaurantField);
            utilities.WaitForOverlayToDisappear();
            Thread.Sleep(2000);

            switch (_modulo)
            {
                case "Atencion":
                    Console.WriteLine("MODULO ATENCION");
                    utilities.VisibilidadElement(AttentionField);
                    utilities.buttonClickeable(AttentionField);

                    break;

                case "Preparacion":
                    utilities.VisibilidadElement(preparationField);
                    utilities.buttonClickeable(preparationField);
   
                    break;

                case "Caja":

                    utilities.buttonClickeable(boxField);
                    break;

                case "Complementos":

                    utilities.buttonClickeable(complementsField);
                    break;

                case "Reportes":

                    utilities.buttonClickeable(reportsField);
                    break;

                default:
                    throw new ArgumentException($"El {_modulo} no es válido.");
            }

            Thread.Sleep(4000);
        }
    }
}
