using FLOTA_VEHICULAR.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FLOTA_VEHICULAR.Pages
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
        private By usernameField = By.XPath("//input[@id='loginEmail']"); 
        private By passwordField = By.Id("loginPassword");
        private By loginButton = By.XPath("//button[@id='submitBtn']");
        private By logo = By.CssSelector("img[src*='LogoSIGES']");

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
