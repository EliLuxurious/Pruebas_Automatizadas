using SIGES3_0.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SIGES3_0.Pages
{
    public class AccessPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public AccessPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // LOGIN
        private By usernameField = By.Id("floatingInput");
        private By passwordField = By.Id("floatingInputPassword");
        private By loginButton = By.XPath("//button[normalize-space()='Ingresar']");
        private By logo = By.XPath("//img[@alt='Logo']");

        public void OpenToAplicattion(string url)
        {
            driver.Navigate().GoToUrl(url);
            Thread.Sleep(4000);
        }

        public void LoginToApplication(string _username, string _password)
        {
            utilities.EnterText(usernameField, _username);
            Thread.Sleep(2000);

            utilities.EnterText(passwordField, _password);
            Thread.Sleep(2000);

            utilities.ClickButton(loginButton);
            Thread.Sleep(4000);

            // Comprobar que el login fue exitoso
            var succesElement = driver.FindElement(logo);
            Assert.IsNotNull(succesElement, "No se encontró el elemento de éxito después del login.");
        }
    }
}
