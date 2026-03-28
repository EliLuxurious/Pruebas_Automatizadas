using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SeleniumExtras.WaitHelpers;
using System;

namespace SIGES3_0.StepDefinitions.SharedStep
{ 
    /// Steps de navegación compartidos PARA NO duplicar.

    [Binding]
    public class NavigationStepDefinitions
    {
        private readonly IWebDriver driver;

        public NavigationStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
        }

        [When(@"el usuario accede al módulo '(.*)'")]
        public void WhenElUsuarioAccedeAlModulo(string modulo)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var elemento = wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//span[normalize-space()='{modulo}']/ancestor::a[1]")
                )
            );
            elemento.Click();
        }

        [When(@"el usuario accede al submodulo '(.*)'")]
        public void WhenElUsuarioAccedeAlSubmodulo(string submodulo)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var elemento = wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//span[contains(text(),'{submodulo}')]")
                )
            );
            elemento.Click();
        }
    }
}