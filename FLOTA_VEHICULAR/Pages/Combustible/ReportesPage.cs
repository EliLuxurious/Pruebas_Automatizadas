using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace FLOTA_VEHICULAR.Pages.Combustible
{
    public class ReportesPage
    {
        private IWebDriver driver;

        public ReportesPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        WebDriverWait Wait(int seconds = 15)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }

        public void SeleccionarTipoReporte(string tipoReporte)
        {
            var wait = Wait();
            Console.WriteLine($"⏳ Seleccionando el reporte tipo: {tipoReporte}");
            Thread.Sleep(3000);

            if (tipoReporte.ToUpper().Contains("CONTROL"))
            {
                By locRadioControl = By.XPath("//mat-radio-button[contains(translate(., 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), 'CONTROL PARA FIRMA')]//span[contains(@class, 'mat-radio-outer-circle')]");
                IWebElement radioBtn = wait.Until(ExpectedConditions.ElementToBeClickable(locRadioControl));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", radioBtn);
                Thread.Sleep(500);
                try { radioBtn.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radioBtn); }
                Console.WriteLine("✅ Opción CONTROL PARA FIRMA seleccionada.");
            }
            else
            {
                By locRadioVal = By.XPath("//mat-radio-button[contains(translate(., 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), 'VALORIZACIONES')]//span[contains(@class, 'mat-radio-outer-circle')]");
                IWebElement radioBtn = wait.Until(ExpectedConditions.ElementToBeClickable(locRadioVal));
                try { radioBtn.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radioBtn); }
                Console.WriteLine("✅ Opción VALORIZACIONES seleccionada.");
            }
            Thread.Sleep(2000);
        }

        public void FiltrarFechasReporte(string fechaDesdeStr, string fechaHastaStr)
        {
            Console.WriteLine($"⏳ Usando la interfaz gráfica del Calendario de Angular para las fechas...");

            // Extraemos solo el día de tus variables del feature, quitando el cero a la izquierda
            // "01012026" se convierte en "1"  |  "24032026" se convierte en "24"
            string diaDesde = int.Parse(fechaDesdeStr.Substring(0, 2)).ToString();
            string diaHasta = int.Parse(fechaHastaStr.Substring(0, 2)).ToString();

            // 1. Calendario DESDE (Le pasamos el índice 1 para el primer ícono)
            SeleccionarFechaCalendario(1, diaDesde);

            // 2. Calendario HASTA (Le pasamos el índice 2 para el segundo ícono)
            SeleccionarFechaCalendario(2, diaHasta);

            Console.WriteLine("✅ Fechas seleccionadas con éxito usando clics visuales. ¡Adiós máscaras y recuadros rojos!");
        }

        // 🔥 Tu método maestro adaptado a esta pantalla
        private void SeleccionarFechaCalendario(int indiceToggle, string dia)
        {
            var wait = Wait();
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(500);

            // Ubicamos el botón del calendario (1 para Desde, 2 para Hasta) usando tu descubrimiento
            By locBtnCalendario = By.XPath($"(//mat-datepicker-toggle)[{indiceToggle}]//button");
            IWebElement btnCal = wait.Until(ExpectedConditions.ElementToBeClickable(locBtnCalendario));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCal);
            Thread.Sleep(500);

            try { btnCal.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal); }
            Thread.Sleep(1500); // Esperamos a que la ventanita del calendario termine de abrirse

            // Usamos tu XPath exacto para ubicar el div con el número del día
            string xpathDia = $"(//mat-datepicker-content)[last()]//*[contains(@class, 'mat-calendar-body-cell') and not(contains(@class, 'mat-calendar-body-disabled'))]//div[contains(@class, 'mat-calendar-body-cell-content') and normalize-space()='{dia}']";

            IWebElement divNumero = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpathDia)));

            // Clic directo al número
            try { divNumero.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", divNumero); }
            Thread.Sleep(1000); // Pausa para que Angular cierre el calendario y guarde el valor
        }

        public void SeleccionarAreaReporte(string area)
        {
            var wait = Wait();
            Console.WriteLine($"⏳ Seleccionando Área: {area}...");

            By locArea = By.XPath("(//mat-select)[1]");
            IWebElement comboArea = wait.Until(ExpectedConditions.ElementToBeClickable(locArea));

            try { comboArea.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", comboArea); }
            Thread.Sleep(1500);

            By locOpcion = By.XPath($"//mat-option//span[contains(@class, 'mat-option-text') and contains(text(), '{area}')]");
            IWebElement opcion = wait.Until(ExpectedConditions.ElementToBeClickable(locOpcion));

            // Clic humano para disparar eventos de Angular
            new OpenQA.Selenium.Interactions.Actions(driver).MoveToElement(opcion).Click().Perform();
            Thread.Sleep(1000);
        }

        public void SeleccionarContratoReporte(string contrato)
        {
            var wait = Wait();
            Console.WriteLine($"⏳ Seleccionando Contrato: {contrato}...");

            By locContrato = By.XPath("(//mat-select)[2]");
            IWebElement comboContrato = wait.Until(ExpectedConditions.ElementToBeClickable(locContrato));

            try { comboContrato.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", comboContrato); }
            Thread.Sleep(1500);

            By locOpcion = By.XPath($"//mat-option//span[contains(@class, 'mat-option-text') and contains(text(), '{contrato}')]");
            IWebElement opcion = wait.Until(ExpectedConditions.ElementToBeClickable(locOpcion));

            new OpenQA.Selenium.Interactions.Actions(driver).MoveToElement(opcion).Click().Perform();
            Thread.Sleep(1000);
        }

        public void ClicVerReporte()
        {
            var wait = Wait();
            Console.WriteLine("⏳ Dando clic en Ver Reporte...");

            // 1. Presionamos ESCAPE para cerrar cualquier menú desplegable (Área/Contrato) que esté tapando el botón
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(1000);

            // 2. Buscamos tu span exacto
            By locBtnVer = By.XPath("//span[contains(@class, 'mat-button-wrapper') and contains(text(), 'Ver')] | //button[contains(normalize-space(.), 'Ver')]");
            IWebElement btnVer = wait.Until(ExpectedConditions.ElementToBeClickable(locBtnVer));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnVer);
            Thread.Sleep(500);

            // 3. Clic humano
            new OpenQA.Selenium.Interactions.Actions(driver).MoveToElement(btnVer).Click().Perform();

            // 4. Doble tap ninja con JavaScript (por si Angular bloqueó el clic normal)
            try { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnVer); } catch { }

            Console.WriteLine("⏳ Clic ejecutado. Esperando reacción del sistema...");
        }

        public void ValidarResultado(string resultadoEsperado, string tipoReporte)
        {
            Console.WriteLine($"\n⏳ Validando tabla para el reporte: {tipoReporte}...");

            bool dataEncontrada = false;
            bool vacioEncontrado = false;
            IWebElement btnSalirEncontrado = null;

            // Escaneamos la pantalla cada segundo durante 25 segundos
            for (int i = 0; i < 25; i++)
            {
                Thread.Sleep(1000);

                // 1. Buscamos el mensaje de "No hay datos"
                var mensajesVacio = driver.FindElements(By.XPath("//td[contains(text(), 'No se encontró ningún registro')] | //div[contains(text(), 'No se encontró ningún registro')]"));
                if (mensajesVacio.Count > 0)
                {
                    vacioEncontrado = true;
                    break;
                }

                // 2. Buscamos el botón "Salir" que indica que el reporte se abrió exitosamente
                // Usamos tu XPath exacto, buscando el span o el botón que lo contiene
                var botonesSalir = driver.FindElements(By.XPath("//button[.//span[contains(normalize-space(text()), 'Salir')]] | //span[contains(text(), 'Salir')]/ancestor::button"));
                if (botonesSalir.Count > 0)
                {
                    dataEncontrada = true;
                    btnSalirEncontrado = botonesSalir[0];
                    break; // Rompemos el ciclo porque ya lo encontramos
                }
            }

            // Evaluamos qué encontró el robot
            if (vacioEncontrado)
            {
                if (resultadoEsperado.ToUpper() == "REPORTE_VACIO")
                {
                    Console.WriteLine("✅ OK: El sistema indicó que no hay datos ('No se encontró ningún registro').");
                }
                else
                {
                    throw new Exception("🚨 FALLO QA: El botón reaccionó, pero devolvió 'No se encontró ningún registro'.");
                }
            }
            else if (dataEncontrada)
            {
                Console.WriteLine($"✅ OK VISUAL: ¡El reporte de {tipoReporte} se generó y abrió correctamente en pantalla!");

                // Le damos clic al botón Salir tal como pediste
                Console.WriteLine("⏳ Cerrando la vista del reporte (Clic en Salir)...");
                Thread.Sleep(1000);
                try
                {
                    btnSalirEncontrado.Click();
                }
                catch
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnSalirEncontrado);
                }
                Thread.Sleep(2000); // Esperamos a que la animación de cierre termine
            }
            else
            {
                throw new Exception("🚨 FALLO QA: Pasaron 25 segundos y no apareció ni el reporte (botón Salir) ni el mensaje de 'No hay datos'.");
            }
        }
    }
}