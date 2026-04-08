using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaConcepto.Hoks.PagesPruebaConcepto
{
    public class LoginPage
    {
        private IWebDriver driver;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        // Element locators
        By usernameField = By.XPath("//input[@id='Email']"); // campo de usuario
        By passwordField = By.XPath("//input[@id='Password']"); // campo de contraseña
        By loginButton = By.XPath("//button[contains(text(),'Iniciar')]"); // botón de inicio de sesión 
        By aceptarButton = By.XPath("//button[contains(text(),'Aceptar')]"); // botón de aceptar

        // Métodos para interactuar con los elementos de la página
        public void EnterUsername(string username)
        {
            driver.FindElement(usernameField).SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            driver.FindElement(passwordField).SendKeys(password);
        }

        public void ClickLoginButton()
        {
            driver.FindElement(loginButton).Click();
        }

        public void ClickAceptarButton()
        {
            driver.FindElement(aceptarButton).Click();
        }

        // Método para realizar el inicio de sesión completo
        public void LoginToApplication(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLoginButton();
            Thread.Sleep(4000);
            ClickAceptarButton();
            Thread.Sleep(4000);
        }
    }
}
