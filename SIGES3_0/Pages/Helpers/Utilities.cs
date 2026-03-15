using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Runtime.InteropServices;

namespace SIGES3_0.Pages.Helpers
{
    public class Utilities
    {
        private const int ClickDelayMs = 1000;
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public Utilities(IWebDriver driver, int timeoutInSeconds = 20)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        }

        public IWebElement FindVisible(params By[] locators)
        {
            foreach (var locator in locators)
            {
                if (locator == null)
                {
                    continue;
                }

                var candidates = driver.FindElements(locator);
                var visible = candidates.FirstOrDefault(element => SafeDisplayed(element));
                if (visible != null)
                {
                    return visible;
                }
            }

            throw new NoSuchElementException($"No se encontro ningun elemento visible para los localizadores: {string.Join(", ", locators.Select(locator => locator?.ToString()))}");
        }

        public IReadOnlyCollection<IWebElement> FindAll(By locator)
        {
            wait.Until(_ => driver.FindElements(locator).Count > 0);
            return driver.FindElements(locator);
        }

        public bool Exists(By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }

        public bool IsVisible(By locator)
        {
            return driver.FindElements(locator).Any(SafeDisplayed);
        }

        public IWebElement WaitUntilVisible(By locator)
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public IWebElement WaitUntilClickable(By locator)
        {
            return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        public void ClickButton(By path)
        {
            var element = WaitUntilClickable(path);
            ScrollViewElement(element);
            element.Click();
            PauseAfterClick();
        }

        public void ClickButton(params By[] locators)
        {
            var element = FindVisible(locators);
            ScrollViewElement(element);
            wait.Until(_ => element.Displayed && element.Enabled);
            element.Click();
            PauseAfterClick();
        }

        public bool ClickIfPresent(params By[] locators)
        {
            foreach (var locator in locators)
            {
                var element = driver.FindElements(locator).FirstOrDefault(SafeDisplayed);
                if (element == null)
                {
                    continue;
                }

                ScrollViewElement(element);
                element.Click();
                PauseAfterClick();
                return true;
            }

            return false;
        }

        public bool ClickWithJavaScript(params By[] locators)
        {
            foreach (var locator in locators)
            {
                var element = driver.FindElements(locator).FirstOrDefault();
                if (element == null)
                {
                    continue;
                }

                try
                {
                    ScrollViewElement(element);
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
                    PauseAfterClick();
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        public void EnterText(By path, string value)
        {
            WaitUntilVisible(path).SendKeys(value);
        }

        public void ClearAndEnterText(By path, string value)
        {
            var element = WaitUntilVisible(path);
            ScrollViewElement(element);
            element.SendKeys(GetSelectAllShortcut());
            element.SendKeys(Keys.Delete);
            element.Clear();
            element.SendKeys(value);
        }

        public void InputAndEnter(By path, string value)
        {
            ClearAndEnterText(path, value);
            WaitUntilVisible(path).SendKeys(Keys.Enter);
        }

        public void Enter(By path)
        {
            WaitUntilVisible(path).SendKeys(Keys.Enter);
        }

        public void SelectOption(By pathComponent, string option)
        {
            SelectOption(option, pathComponent);
        }

        public void SelectOption(string option, params By[] pathComponents)
        {
            var dropdown = FindVisible(pathComponents);
            ScrollViewElement(dropdown);
            dropdown.Click();

            var searchInput = driver.FindElements(By.CssSelector("input.search-input"))
                .FirstOrDefault(SafeDisplayed);

            if (searchInput != null)
            {
                ScrollViewElement(searchInput);
                searchInput.SendKeys(GetSelectAllShortcut());
                searchInput.SendKeys(Keys.Delete);
                searchInput.Clear();
                searchInput.SendKeys(option);
            }

            var optionElement = wait.Until(_ =>
            {
                try
                {
                    foreach (var label in driver.FindElements(By.CssSelector(".option-label")))
                    {
                        if (!SafeDisplayed(label))
                        {
                            continue;
                        }

                        var text = SafeText(label);
                        if (text.Contains(option, StringComparison.OrdinalIgnoreCase))
                        {
                            return label;
                        }
                    }

                    var optionLocators = new[]
                    {
                        By.XPath($"//*[@role='option' and normalize-space()={ToXPathLiteral(option)}]"),
                        By.XPath($"//li[normalize-space()={ToXPathLiteral(option)}]"),
                        By.XPath($"//li[contains(normalize-space(), {ToXPathLiteral(option)})]"),
                        By.XPath($"//*[contains(@class,'option-label') and contains(normalize-space(), {ToXPathLiteral(option)})]"),
                        By.XPath($"//div[contains(@class,'option') and normalize-space()={ToXPathLiteral(option)}]"),
                        By.XPath($"//div[normalize-space()={ToXPathLiteral(option)}]"),
                        By.XPath($"//span[normalize-space()={ToXPathLiteral(option)}]")
                    };

                    foreach (var locator in optionLocators)
                    {
                        var found = driver.FindElements(locator).FirstOrDefault(SafeDisplayed);
                        if (found != null)
                        {
                            return found;
                        }
                    }

                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });

            ScrollViewElement(optionElement);
            optionElement.Click();
            PauseAfterClick();
        }

        public void SetCheckbox(By locator, bool expectedValue)
        {
            var checkbox = WaitUntilVisible(locator);
            ScrollViewElement(checkbox);

            if (checkbox.Selected != expectedValue)
            {
                checkbox.Click();
                PauseAfterClick();
            }
        }

        public bool IsElementEnabled(By locator)
        {
            var element = FindVisible(locator);
            var classes = element.GetAttribute("class") ?? string.Empty;
            var ariaDisabled = element.GetAttribute("aria-disabled") ?? string.Empty;
            var disabledAttr = element.GetAttribute("disabled") ?? string.Empty;

            return element.Enabled
                && !classes.Contains("disabled", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(ariaDisabled, "true", StringComparison.OrdinalIgnoreCase)
                && string.IsNullOrEmpty(disabledAttr);
        }

        public bool PageContainsText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            return driver.PageSource.Contains(text, StringComparison.OrdinalIgnoreCase)
                || driver.FindElement(By.TagName("body")).Text.Contains(text, StringComparison.OrdinalIgnoreCase);
        }

        public void WaitForText(string text, int timeoutInSeconds = 10)
        {
            var textWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            textWait.Until(_ => PageContainsText(text));
        }

        public string GetText(By locator)
        {
            return WaitUntilVisible(locator).Text;
        }

        public string GetAttribute(By locator, string attributeName)
        {
            return WaitUntilVisible(locator).GetAttribute(attributeName) ?? string.Empty;
        }

        public void ScrollViewElement(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});", element);
        }

        public void ScrollViewTop()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, 0);");
        }

        private static bool SafeDisplayed(IWebElement element)
        {
            try
            {
                return element.Displayed;
            }
            catch
            {
                return false;
            }
        }

        private static string SafeText(IWebElement element)
        {
            try
            {
                return element.Text ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static void PauseAfterClick()
        {
            Thread.Sleep(ClickDelayMs);
        }

        private static string GetSelectAllShortcut()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                ? Keys.Command + "a"
                : Keys.Control + "a";
        }

        private static string EscapeXPath(string value)
        {
            return value.Replace("\"", "\\\"");
        }

        private static string ToXPathLiteral(string value)
        {
            if (!value.Contains('\''))
            {
                return $"'{value}'";
            }

            if (!value.Contains('"'))
            {
                return $"\"{value}\"";
            }

            var parts = value.Split('\'');
            return "concat('" + string.Join("',\"'\",'", parts) + "')";
        }
    }
}
