using OpenQA.Selenium;
using SIGESLogin.Locators;
using SIGESLogin.Locators;

namespace SIGESLogin.Pages
{
    public class LoginPage
    {
        private IWebDriver driver;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        // Métodos de interacción
        public void EnterEmail(string email)
        {
            driver.FindElement(LoginLocators.txtEmail).SendKeys(email);
        }

        public void EnterPassword(string password)
        {
            driver.FindElement(LoginLocators.txtPassword).SendKeys(password);
        }

        public void ClickSignIn()
        {
            driver.FindElement(LoginLocators.btnSignIn).Click();
        }

        public void ConfirmLogin()
        {
            driver.FindElement(LoginLocators.btnConfirm).Click();
        }

        // Método compuesto (flujo completo)
        public void Login(string email, string password)
        {
            EnterEmail(email);
            EnterPassword(password);
            ClickSignIn();
            ConfirmLogin();
        }
    }
}
