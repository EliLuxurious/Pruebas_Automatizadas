using SIGES3_0.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SIGES3_0.Pages
{
    public class AccessPage
    {
        private readonly IWebDriver driver;
        private readonly Utilities utilities;
        private readonly WebDriverWait wait;

        public AccessPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        private static readonly By UsernameField = By.Id("floatingInput");
        private static readonly By PasswordField = By.Id("floatingInputPassword");
        private static readonly By LoginButton = By.XPath("//button[normalize-space()='Ingresar']");

        public void OpenToAplicattion(string url)
        {
            driver.Navigate().GoToUrl(url);
            Thread.Sleep(3000);
        }

        public void LoginToApplication(string username, string password)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(UsernameField));
            utilities.EnterText(UsernameField, username);
            Thread.Sleep(500);
            utilities.EnterText(PasswordField, password);
            Thread.Sleep(500);
            utilities.ClickButton(LoginButton);

            // Esperar login exitoso: menú Ventas, logo, o que ya no estemos en /auth/login
            var successLocators = new[]
            {
                By.XPath("//span[contains(.,'Venta') or contains(.,'Ventas')]"),
                By.XPath("//a[contains(.,'Nueva Venta') or contains(.,'Ventas')]"),
                By.XPath("//img[@alt='Logo']"),
                By.CssSelector("img[alt*='Logo']"),
                By.CssSelector("img[alt*='logo']")
            };

            var found = wait.Until(_ =>
            {
                foreach (var loc in successLocators)
                {
                    var el = driver.FindElements(loc).FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
                    if (el != null) return true;
                }
                return driver.Url != null && !driver.Url.Contains("/auth/login");
            });

            Assert.IsTrue(found, "Login: no se detectó el dashboard. Verifica usuario/contraseña o que la página cargó correctamente.");
        }
    }
}
