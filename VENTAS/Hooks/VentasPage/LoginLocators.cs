using OpenQA.Selenium;

namespace SIGESLogin.Locators
{
    public static class LoginLocators
    {
        public static readonly By txtEmail = By.XPath("//input[@id='Email']");
        public static readonly By txtPassword = By.XPath("//input[@id='Password']");
        public static readonly By btnSignIn = By.XPath("//button[contains(text(),'Iniciar')]");
        public static readonly By btnConfirm = By.XPath("//button[contains(text(),'Aceptar')]");
    }
}
