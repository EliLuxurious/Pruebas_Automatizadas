using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Reqnroll;
using System.Runtime.InteropServices;

namespace SIGES3_0.StepDefinitions.VentasStep
{
    [Binding]
    public class ChromePopupVentasStepDefinitions
    {
        private readonly IWebDriver driver;

        public ChromePopupVentasStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
        }

        [StepDefinition("se descarta aviso de contrasena de Chrome si aparece")]
        public void SeDescartaAvisoDeContrasenaDeChromeSiAparece()
        {
            // Este aviso pertenece a la UI de Chrome, no al DOM de SIGES.
            // La prevencion real esta en ChromeOptions; esto queda como respaldo.
            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            Thread.Sleep(1200);
            TrySendEnterWithSelenium();
            PressKey(VirtualKeyEnter);
            Thread.Sleep(700);
        }

        private void TrySendEnterWithSelenium()
        {
            try
            {
                new Actions(driver).SendKeys(Keys.Enter).Perform();
            }
            catch (WebDriverException)
            {
            }
        }

        private const byte VirtualKeyEnter = 0x0D;
        private const uint KeyEventKeyUp = 0x0002;

        private static void PressKey(byte virtualKey)
        {
            keybd_event(virtualKey, 0, 0, UIntPtr.Zero);
            keybd_event(virtualKey, 0, KeyEventKeyUp, UIntPtr.Zero);
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
    }
}
