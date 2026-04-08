using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace RESTAURANTE.Hoks.Pages
{
    public class Utilities
    {
        private IWebDriver driver;
        WebDriverWait wait;

        public Utilities(IWebDriver driver, int timeoutInSeconds = 30)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver), "El WebDriver no puede ser null.");
            }
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        }


        //By restauranteField = By.XPath("//span[contains(text(),'Restaurante')]");

       
        // MENU DESPEGABLE
        By SelecOptions = By.CssSelector(".select2-results__options");

        // OVERLAY
        private By overlayLocator = By.ClassName("block-ui-overlay");

        public void WaitForOverlayToDisappear()
        {
            // WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));
            wait.Until(driver =>
            {
                try
                {
                    IWebElement overlay = driver.FindElement(overlayLocator);
                    return !overlay.Displayed; // Espera hasta que el overlay no esté visible
                }
                catch (NoSuchElementException)
                {
                    return true; // Si no se encuentra, el overlay ya desapareció
                }
            });
        }

        public void InputTexto(By _path, string _field)
        {
            if (driver.FindElements(_path).Count == 0)
            {
                throw new NoSuchElementException($"El elemento con el localizador {_path} no se encontró.");
            }

            wait.Until(ExpectedConditions.ElementIsVisible(_path)); // Espera hasta que el elemento sea visible
            driver.FindElement(_path).Clear(); 
            driver.FindElement(_path).SendKeys(_field);
        }

        public void SubmitTexto(By _path, string _field)
        {
            if (driver.FindElements(_path).Count == 0)
            {
                throw new NoSuchElementException($"El elemento con el localizador {_path} no se encontró.");
            }

            wait.Until(ExpectedConditions.ElementIsVisible(_path)); // Espera hasta que el elemento sea visible
            driver.FindElement(_path).Clear();
            driver.FindElement(_path).SendKeys(_field);
            driver.FindElement(_path).SendKeys(Keys.Enter);
        }

        public void VisibilidadElement(By _path)
        {
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(_path)); 

                //buttonClickeable(_button);
            }
            catch (WebDriverTimeoutException)
            {
                throw new NoSuchElementException($"El elemento con el localizador {_path} visibilidad no se encontró dentro del tiempo esperado.");
            }
        }

        public void elementExists(By _button)
        {
            try
            {
                wait.Until(ExpectedConditions.ElementExists(_button)); // Espera hasta que el elemento exista en el DOM
                //buttonClickeable(_button);
            }
            catch (WebDriverTimeoutException)
            {
                throw new NoSuchElementException($"El elemento con el localizador {_button} no se encontró dentro del tiempo esperado.");
            }
        }

        public void buttonClickeable(By _button)
        {
            if (driver.FindElements(_button).Count == 0)
            {
                throw new NoSuchElementException($"El elemento con el localizador {_button} no se encontró.");
            }

            WaitForOverlayToDisappear(); // OVERLAY
            wait.Until(ExpectedConditions.ElementToBeClickable(_button)); // Espera hasta que el elemento sea clickeable
            driver.FindElement(_button).Click();
        }
        
        
        // MODALES
        public void ClickButtonInModal(IWebElement _element, By buttonLocator)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(buttonLocator));
            wait.Until(ExpectedConditions.ElementToBeClickable(buttonLocator)); // Espera hasta que el elemento sea clickeable
            _element.FindElement(buttonLocator).Click();
        }

        public void InputTextoModal(IWebElement _element, By _path, string _field)
        {
            if (_element.FindElements(_path).Count == 0)
            {
                throw new NoSuchElementException($"El elemento con el localizador {_path} no se encontró.");
            }

            wait.Until(ExpectedConditions.ElementIsVisible(_path)); // Espera hasta que el elemento sea visible
            _element.FindElement(_path).Clear();
            _element.FindElement(_path).SendKeys(_field);
            _element.FindElement(_path).SendKeys(Keys.Enter);
        }

        public void addFieldModal(IWebElement _element, By _path, string _field)
        {
            if (_element.FindElements(_path).Count == 0)
            {
                throw new NoSuchElementException($"El elemento con el localizador {_path} no se encontró.");
            }

            wait.Until(ExpectedConditions.ElementIsVisible(_path)); // Espera hasta que el elemento sea visible
            _element.FindElement(_path).Clear();
            _element.FindElement(_path).SendKeys(_field);
        }

        // SCROLL
        public void ScrollViewElement(IWebElement _path)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", _path);
            Thread.Sleep(2000);
        }

        public void ScrollViewTop()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, 0);");
            Thread.Sleep(2000);
        }

        public void SelecOption(IWebElement _element, By _path, string option)
        {
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(_path));
                wait.Until(ExpectedConditions.ElementToBeClickable(_path));

                _element.FindElement(_path).Click();

                /*
                // Abre el menú desplegable
                IWebElement dropdown = _element.FindElement(_path);
                //dropdown.Click();
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].click();", dropdown);
                */

                // Espera explícita para que las opciones sean visibles
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".select2-results__options")));

                // Selecciona la opción deseada
                IWebElement optionElement = _element.FindElement(By.XPath($"//li[contains(text(), '{option}')]"));
                optionElement.Click();
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Error: No se encontró la opción '{option}' en el menú desplegable. Detalle: {ex.Message}");
            }
        }

    }
}
