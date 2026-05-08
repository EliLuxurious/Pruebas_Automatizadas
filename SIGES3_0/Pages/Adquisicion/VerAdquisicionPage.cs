using System.IO;
using System.Linq;

using SIGES3_0.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using NUnit.Framework;

namespace SIGES3_0.Pages.Adquisicion
{
    public class VerAdquisicionPage
    {
        private IWebDriver driver;
        private Utilities utilities;

        public VerAdquisicionPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
        }

        // --- 1. ZONA DE SELECTORES (Locators de Ver Adquisición) ---
        private By submoduloVerAdquisicion = By.XPath("//span[normalize-space()='Ver Adquisición']");

        //Para hacer una nueva adquisicion aqui
        private By btnNuevaCompra = By.XPath("//button[contains(normalize-space(), 'Nueva Compra')]");

        // Filtros Generales
        private By txtFechaInicial = By.XPath("//input[@formcontrolname='startDate']");
        private By txtFechaFinal = By.XPath("//input[@formcontrolname='endDate']");
        private By cmbProveedor = By.XPath("//app-dropdown-search[@formcontrolname='supplierId']//div[contains(@class, 'select-trigger')]");
        private By txtBusquedaDropdown = By.XPath("//input[contains(@class, 'select-search')] | //input[@placeholder='Buscar...']");
        private By btnBuscar = By.XPath("//button[.//i[contains(@class, 'bi-search')]]");
        private By celdasProveedorEnTabla = By.XPath("//table/tbody/tr/td[5]");

        // Ver Adquisicion Especifica 
        private By btnVerPrimerRegistro = By.XPath("//table/tbody/tr[1]//button[.//i[contains(@class, 'bi-search')]]");
        private By modalContenedor = By.XPath("//div[contains(@class, 'modal-header')]");

        // Botón de Cerrar (la 'X' en la esquina superior derecha)
        //private By btnCerrarVisor = By.XPath("//button[contains(@class, 'btn-close')]");

        private By cmbFormatoNativo = By.Id("formato");
        private By btnImprimir = By.XPath("//button[contains(@class, 'btn-print')]");
        private By btnDescargar = By.XPath("//button[contains(@class, 'btn-download')]");

        private By btnNotaCredito = By.XPath("//button[contains(., 'Nota de Credito')]");
        private By btnNotaDebito = By.XPath("//button[contains(., 'Nota de Debito')]");

        // --- 2. ZONA DE MÉTODOS ---

        public void ClicEnNuevaCompra()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnNuevaCompra));
            boton.Click();

            Thread.Sleep(2000);
        }

        public void IngresarASubmoduloVerAdquisicion()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement btnSubmodulo = wait.Until(ExpectedConditions.ElementExists(submoduloVerAdquisicion));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnSubmodulo);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click(); arguments[0].parentNode.click();", btnSubmodulo);
            Thread.Sleep(3000); // Espera a que cargue la pantalla de listado
        }

        public void ConfigurarFiltros(string fechaInicial, string fechaFinal, string proveedor)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            if (!string.IsNullOrEmpty(fechaInicial))
            {
                IWebElement inputInicio = wait.Until(ExpectedConditions.ElementIsVisible(txtFechaInicial));
                inputInicio.SendKeys(Keys.Control + "a" + Keys.Backspace);
                inputInicio.SendKeys(fechaInicial);
            }

            if (!string.IsNullOrEmpty(fechaFinal))
            {
                IWebElement inputFin = wait.Until(ExpectedConditions.ElementIsVisible(txtFechaFinal));
                inputFin.SendKeys(Keys.Control + "a" + Keys.Backspace);
                inputFin.SendKeys(fechaFinal);
            }

            if (!string.IsNullOrEmpty(proveedor))
            {
                SeleccionarProveedorEnCombo(proveedor);
            }
        }

        public void ClicBuscar()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnBuscar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", boton);
            Thread.Sleep(500);

            try
            {
                boton.Click();
            }
            catch (Exception)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton);
            }
            Thread.Sleep(4000);
        }

        public bool ValidarRegistrosEnTabla(string proveedorEsperado)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                var celdas = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(celdasProveedorEnTabla));

                if (celdas.Count == 0) return false;

                string esperadoLimpio = proveedorEsperado.Replace("-", " ").Trim();

                foreach (var celda in celdas)
                {
                    string textoCrudo = celda.Text;
                    string textoTablaLimpio = textoCrudo.Replace("-", " ").Trim();

                    if (!textoTablaLimpio.Contains(esperadoLimpio, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Fila incorrecta. Se esperaba: {proveedorEsperado}, pero se encontró: {textoCrudo}");
                        return false;
                    }
                }
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("La tabla apareció vacía.");
                return false;
            }
        }

        private void SeleccionarProveedorEnCombo(string valorABuscar)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            IWebElement trigger = wait.Until(ExpectedConditions.ElementExists(cmbProveedor));
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", trigger);
            Thread.Sleep(1000);
            js.ExecuteScript("arguments[0].click();", trigger);

            IWebElement input = wait.Until(ExpectedConditions.ElementIsVisible(txtBusquedaDropdown));
            input.Clear();
            input.SendKeys(valorABuscar);
            Thread.Sleep(3000); // Crucial para que el back filtre

            By locatorOpcion = By.XPath($"//div[contains(@class, 'select-results')]//*[contains(text(), '{valorABuscar}')] | //div[contains(@class, 'options')]//*[contains(text(), '{valorABuscar}')]");

            try
            {
                IWebElement opcion = wait.Until(ExpectedConditions.ElementToBeClickable(locatorOpcion));
                js.ExecuteScript("arguments[0].click();", opcion);
            }
            catch (Exception)
            {
                input.SendKeys(Keys.Enter);
            }
            Thread.Sleep(1500);
        }

        public void CambiarFormatoDocumento(string formatoEsperado)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement comboFormato = wait.Until(ExpectedConditions.ElementIsVisible(cmbFormatoNativo));

            SelectElement select = new SelectElement(comboFormato);
            select.SelectByValue(formatoEsperado.ToUpper()); // Busca por el atributo 'value' que vimos en tu HTML

            Thread.Sleep(3000);
        }
        public bool EsBotonVisibleEnVisor(string accion)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                By selectorBoton = accion.Equals("Descargar", StringComparison.OrdinalIgnoreCase) ? btnDescargar : btnImprimir;

                IWebElement boton = wait.Until(ExpectedConditions.ElementIsVisible(selectorBoton));
                return boton.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        public void AbrirVisorPrimerRegistro()
        {
            WebDriverWait waitCorto = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            IWebElement btnVer = waitCorto.Until(ExpectedConditions.ElementExists(btnVerPrimerRegistro));
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnVer);
            Thread.Sleep(1000);

            try
            {
                waitCorto.Until(ExpectedConditions.ElementToBeClickable(btnVer)).Click();
            }
            catch (Exception)
            {
                js.ExecuteScript("arguments[0].click();", btnVer);
            }
            WebDriverWait waitLargo = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            By modalCabeceraFinal = By.XPath("//div[contains(@class, 'modal-header')] | //button[contains(@class, 'btn-print')]");

            try
            {
                waitLargo.Until(ExpectedConditions.ElementIsVisible(modalCabeceraFinal));
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("❌ El documento tardó más de 1 minuto en cargar o el sistema se quedó colgado en 'Cargando comprobante...'");
            }

            Thread.Sleep(1500);
        }
        public void EjecutarAccionDocumento(string accion)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            By selectorBoton;

            if (accion.Equals("Descargar", StringComparison.OrdinalIgnoreCase))
            {
                selectorBoton = By.XPath("//button[contains(@class, 'btn-download') or contains(., 'Descargar')]");
            }
            else if (accion.Equals("Imprimir", StringComparison.OrdinalIgnoreCase))
            {
                selectorBoton = By.XPath("//button[contains(@class, 'btn-print') or contains(., 'Imprimir')]");
            }
            else
            {
                Assert.Fail($"❌ La acción '{accion}' no es válida. Usa 'Imprimir' o 'Descargar'.");
                return;
            }

            try
            {
                IWebElement botonAccion = wait.Until(ExpectedConditions.ElementToBeClickable(selectorBoton));
                js.ExecuteScript("arguments[0].click();", botonAccion);

                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Assert.Fail($"❌ No se pudo hacer clic en el botón '{accion}'. Error: {ex.Message}");
            }
        }

        public bool ValidarAccionExitosa(string accion)
        {
            if (accion.Equals("Descargar", StringComparison.OrdinalIgnoreCase))
            {
                string rutaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

                for (int i = 0; i < 15; i++)
                {
                    var archivoReciente = new DirectoryInfo(rutaDescargas)
                        .GetFiles()
                        .OrderByDescending(f => f.LastWriteTime)
                        .FirstOrDefault();

                    if (archivoReciente != null &&
                       (DateTime.Now - archivoReciente.LastWriteTime).TotalMinutes < 1 &&
                       !archivoReciente.Extension.Contains("crdownload"))
                    {
                        Console.WriteLine($"✅ Archivo descargado exitosamente: {archivoReciente.Name}");
                        return true;
                    }
                    Thread.Sleep(1000);
                }
                return false;
            }
            else if (accion.Equals("Imprimir", StringComparison.OrdinalIgnoreCase))
            {
                if (driver.WindowHandles.Count > 1)
                {
                    driver.SwitchTo().Window(driver.WindowHandles.Last());
                    driver.Close();
                    driver.SwitchTo().Window(driver.WindowHandles.First());
                    return true;
                }

                Console.WriteLine("✅ Se hizo clic en imprimir. (Nota: No se puede interactuar con el diálogo nativo de Chrome).");
                return true;
            }

            return false;
        }

        public void ConfigurarNotaCredito()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnNotaCredito));

            // Usamos JS para evitar que algo tape el botón al darle clic
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", boton);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton);
        }

        public void ConfigurarNotaDebito()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnNotaDebito));

            // Usamos JS para evitar que algo tape el botón al darle clic
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", boton);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton);
        }
    }
}