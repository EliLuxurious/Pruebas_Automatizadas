using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using System.Threading;

namespace SIGES3_0.Pages.Base
{
    public abstract class BasePage
    {
        protected readonly IWebDriver driver;
        protected readonly WebDriverWait wait;

        protected readonly WebDriverWait waitLong;

        protected BasePage(IWebDriver driver, int timeoutSeconds = 15)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            this.waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(25))
            {
                PollingInterval = TimeSpan.FromMilliseconds(200)
            };
        }

        protected void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
            Thread.Sleep(200);
        }

        protected void JsClick(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
        }

        // --- SOBRECARGAS DE CLICK SEGURO ---

        // Opción 1: Recibe un Localizador (By)
        protected void ClickSeguro(By locator)
        {
            var element = waitLong.Until(d =>
            {
                var elements = d.FindElements(locator).Where(e => e.Displayed && e.Enabled).ToList();
                return elements.Any() ? elements.First() : null;
            });

            if (element == null) throw new Exception($"Elemento no clickeable: {locator}");

            ScrollToElement(element);
            try { element.Click(); }
            catch { JsClick(element); }
            Thread.Sleep(300);
        }

        // Opción 2: Recibe un elemento web directo (IWebElement)
        protected void ClickSeguro(IWebElement element)
        {
            ScrollToElement(element);
            try { element.Click(); }
            catch { JsClick(element); }
            Thread.Sleep(300);
        }

        protected void LimpiarEIngresarTexto(By locator, string texto)
        {
            var input = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            ScrollToElement(input);
            try { input.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", input); }
            Thread.Sleep(150);
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(150);
            input.SendKeys(texto);
            input.SendKeys(Keys.Tab);
            Thread.Sleep(300);
        }

        protected bool EsValorIgnorado(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ||
                   valor.Trim().Equals("NA", StringComparison.OrdinalIgnoreCase) ||
                   valor.Trim().Equals("NO_CAMBIO", StringComparison.OrdinalIgnoreCase) ||
                   valor.Trim().Equals("sin_cambio", StringComparison.OrdinalIgnoreCase) ||
                   valor.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase);
        }

        // ESTO SOLUCIONA LOS ERRORES  ("BotonEstaDeshabilitado" no existe)
        protected bool BotonEstaDeshabilitado(IWebElement boton)
        {
            try
            {
                string disabled = (boton.GetAttribute("disabled") ?? "").Trim().ToLower();
                string ariaDisabled = (boton.GetAttribute("aria-disabled") ?? "").Trim().ToLower();
                string clases = (boton.GetAttribute("class") ?? "").Trim().ToLower();
                string pointerEvents = (boton.GetCssValue("pointer-events") ?? "").Trim().ToLower();

                return !boton.Enabled || disabled == "true" || disabled == "disabled" ||
                       ariaDisabled == "true" || clases.Contains("disabled") || pointerEvents == "none";
            }
            catch { return false; }
        }
    }
}