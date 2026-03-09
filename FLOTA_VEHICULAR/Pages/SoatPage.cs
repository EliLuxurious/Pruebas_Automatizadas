using FLOTA_VEHICULAR.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace FLOTA_VEHICULAR.Pages
{
    public class SoatPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public SoatPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        WebDriverWait Wait(int seconds = 20)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }

        // =============================
        // NAVEGACIÓN Y BOTONES
        // =============================
        private By moduloSoat = By.XPath("//div[normalize-space()='SOAT']");
        private By btnNuevo = By.XPath("//button[contains(., '+Nuevo') or contains(., 'Nuevo') or contains(., 'NUEVO')] | //div[contains(@class, 'filter')]//button[3]");

        public void IngresarModuloSoat()
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(2000);
            IWebElement modulo = wait.Until(ExpectedConditions.ElementExists(moduloSoat));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", modulo);
            System.Threading.Thread.Sleep(2000);
        }

        public void ClicNuevoSoat()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnNuevo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(2500);
        }

        // =============================
        // FORMULARIO SOAT
        // =============================
        private By txtPlaca = By.XPath("(//mat-form-field[not(ancestor::table) and not(ancestor::p-table)][contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'placa')]//input)[1] | (//form//input)[1]");
        private By btnLupaPlaca = By.XPath("(//mat-icon[normalize-space()='search' and not(ancestor::table) and not(ancestor::p-table) and not(ancestor::td)])[1]");

        private By selectProveedor = By.XPath("//mat-form-field[not(ancestor::table) and not(ancestor::p-table)][contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'proveedor')]//mat-select");
        private By txtPoliza = By.XPath("//input[@formcontrolname='policyNumber']");

        // Íconos de Calendario - XPATHS INDESTRUCTIBLES basados en el HTML que me diste
        private By btnCalDesde = By.XPath("//mat-form-field[.//input[@formcontrolname='startPolicyValidity']]//button");
        private By btnCalHasta = By.XPath("//mat-form-field[.//input[@formcontrolname='endPolicyValidity']]//button");
        private By btnCalContratante = By.XPath("(//mat-datepicker-toggle//button)[last()]");

        private By txtRuc = By.XPath("//input[@formcontrolname='documentIdentity']");
        private By btnLupaRuc = By.XPath("(//mat-icon[normalize-space()='search' and not(ancestor::table) and not(ancestor::p-table) and not(ancestor::td)])[2]");

        private By txtHora = By.XPath("//input[@formcontrolname='hour']");
        private By txtImporte = By.XPath("//input[@formcontrolname='amount']");
        private By inputFile = By.XPath("//input[@type='file']");
        private By btnGuardar = By.XPath("//button[not(ancestor::table)][contains(@class, 'tsp-button-success') or contains(@class, 'mat-raised-button')][contains(., 'Guardar') or contains(., 'GUARDAR')]");

        // =============================
        // MÉTODOS DE INTERACCIÓN
        // =============================
        public void IngresarPlacaYBuscar(string placa)
        {
            var wait = Wait();
            utilities.EnterText(txtPlaca, placa);
            System.Threading.Thread.Sleep(1000);

            IWebElement lupa = wait.Until(ExpectedConditions.ElementToBeClickable(btnLupaPlaca));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", lupa);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupa);
            System.Threading.Thread.Sleep(3000);
        }

        public void SeleccionarProveedor(string proveedor)
        {
            var wait = Wait();
            IWebElement dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(selectProveedor));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", dropdown);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dropdown);
            System.Threading.Thread.Sleep(1000);

            string provTrim = proveedor.Trim();
            By optionXPath = By.XPath($"//span[@class='mat-option-text' and contains(normalize-space(), '{provTrim}')] | //mat-option[.//span[contains(normalize-space(), '{provTrim}')]]");

            IWebElement optionElement = wait.Until(ExpectedConditions.ElementExists(optionXPath));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", optionElement);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", optionElement);
            System.Threading.Thread.Sleep(1000);
        }

        public void IngresarPoliza(string poliza)
        {
            utilities.EnterText(txtPoliza, poliza);
        }

        // =========================================================================
        // MÉTODO CAZA-FANTASMAS (SELECCIÓN INFALIBLE DE CALENDARIO)
        // =========================================================================
        public void SeleccionarFecha(By btnCalendario, string dia)
        {
            var wait = Wait(10);

            // 1. Limpiar pantalla cerrando cualquier cosa abierta
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            System.Threading.Thread.Sleep(500);

            // 2. Abrir el calendario
            IWebElement btnCal = wait.Until(ExpectedConditions.ElementToBeClickable(btnCalendario));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCal);
            System.Threading.Thread.Sleep(500);

            try { btnCal.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal); }
            System.Threading.Thread.Sleep(1500); // Pausa obligatoria para animación

            // 3. XPATH MAESTRO:
            // Busca DENTRO DEL ÚLTIMO PANEL ABIERTO, la celda que NO esté deshabilitada y agarra su div numérico.
            string xpathDia = $"(//mat-datepicker-content)[last()]//*[contains(@class, 'mat-calendar-body-cell') and not(contains(@class, 'mat-calendar-body-disabled'))]//div[contains(@class, 'mat-calendar-body-cell-content') and normalize-space()='{dia}']";

            try
            {
                // Espera a que el div del número sea VISIBLE
                IWebElement divNumero = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpathDia)));

                // Clic Nativo (El que hace que Angular se active y rellene las demás cajas)
                divNumero.Click();
            }
            catch (Exception ex)
            {
                // Plan B de rescate
                Console.WriteLine($"Aviso: Usando Plan B para el día {dia}. Detalle: {ex.Message}");
                IWebElement divNumero = driver.FindElement(By.XPath(xpathDia));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", divNumero);
            }

            // 4. Esperar a que el calendario se cierre solo
            System.Threading.Thread.Sleep(1500);

            // 5. Cierre de seguridad por si Angular se trabó
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            System.Threading.Thread.Sleep(500);
        }

        public void SeleccionarFechaDesdeYHasta(string diaDesde, string diaHasta)
        {
            SeleccionarFecha(btnCalDesde, diaDesde);
            SeleccionarFecha(btnCalHasta, diaHasta);
        }

        public void IngresarRucYBuscar(string ruc)
        {
            var wait = Wait();
            utilities.EnterText(txtRuc, ruc);
            System.Threading.Thread.Sleep(1000);
            IWebElement lupa = wait.Until(ExpectedConditions.ElementToBeClickable(btnLupaRuc));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", lupa);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupa);
            System.Threading.Thread.Sleep(3000);
        }

        public void SeleccionarFechaContratante(string dia)
        {
            SeleccionarFecha(btnCalContratante, dia);
        }

        public void IngresarHoraEImporte(string hora, string importe)
        {
            IWebElement campoHora = driver.FindElement(txtHora);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", campoHora);
            campoHora.SendKeys(hora);
            System.Threading.Thread.Sleep(1000);

            utilities.EnterText(txtImporte, importe);
        }

        public void AdjuntarDocumento(string rutaArchivo)
        {
            var wait = Wait();
            IWebElement fileInput = wait.Until(ExpectedConditions.ElementExists(inputFile));
            fileInput.SendKeys(rutaArchivo);
            System.Threading.Thread.Sleep(4000);
        }

        public void GuardarSoat()
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(2000);

            try
            {
                IWebElement btn = wait.Until(ExpectedConditions.ElementExists(btnGuardar));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);
                System.Threading.Thread.Sleep(500);

                string disabledAttr = btn.GetAttribute("disabled");
                string classAttr = btn.GetAttribute("class");

                if (disabledAttr == "true" || (classAttr != null && classAttr.Contains("disabled")))
                {
                    string validationScript = @"
                        var invalidFields = [];
                        var inputs = document.querySelectorAll('input');
                        inputs.forEach(function(input) {
                            if (input.hasAttribute('required') && !input.value) {
                                var label = input.closest('mat-form-field') ? input.closest('mat-form-field').querySelector('mat-label') : null;
                                var name = label ? label.textContent.trim() : input.getAttribute('formcontrolname');
                                invalidFields.push(name);
                            }
                        });
                        return invalidFields.join(', ');
                    ";
                    string camposVacios = (string)((IJavaScriptExecutor)driver).ExecuteScript(validationScript);
                    throw new Exception($"El botón Guardar está bloqueado. Campos obligatorios vacíos: {camposVacios}");
                }

                wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardar)).Click();
                System.Threading.Thread.Sleep(4000);
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("Timeout: No se pudo hacer clic en Guardar.");
            }
        }







    }
}