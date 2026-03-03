using SIGES3_0.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SIGES3_0.Pages
{
    public class PlanServicioPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public PlanServicioPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // --- Selectores para Login ---
        private By usernameField = By.Id("floatingInput");
        private By passwordField = By.Id("floatingInputPassword");
        private By loginButton = By.XPath("//button[normalize-space()='Ingresar']");
        private By logo = By.XPath("//img[@alt='Logo']");

        // --- Selectores para Navegación ---
        private By moduloFacturacionCiclica = By.XPath("//span[normalize-space()='Facturación Cíclica']/ancestor::a"); //con IA 
        private By submoduloPlanServicio = By.XPath("//span[normalize-space()='Plan de Servicio']/ancestor::a"); //con IA

        // --- Selectores para Detalles del Plan (Límites) ---
        private By btnDetallesPlan = By.XPath("//span[normalize-space()='Detalles del Plan']/ancestor::button");
        private By txtMinComprobantes = By.Id("min-78"); // ID de f12
        private By txtMaxComprobantes = By.Id("max-78"); // ID de f12
        private By txtMinLocales = By.Id("min-79");
        private By txtMaxLocales = By.Id("max-79");
        private By txtMinUsuarios = By.Id("min-80");
        private By txtMaxUsuarios = By.Id("max-80");

        // --- Selectores para Datos Generales ---
        private By tabDatosGenerales = By.XPath("//span[normalize-space()='Datos generales']/ancestor::button");
        private By txtNombrePlan =  By.XPath("//input[@placeholder='Nombre del plan']");
        private By txtDescripcionPlan = By.XPath("//textarea[@placeholder='Descripción']");
        private By selectCicloFacturacion = By.CssSelector("select[formcontrolname='billingCycleId']");
        private By txtPrecioPlan = By.XPath("//input[@placeholder='0.00']");
        private By btnGuardar = By.XPath("//button[normalize-space()='Guardar']");

        // --- Modal de OK ---
        private By btnOkModal = By.XPath("//button[normalize-space()='OK']");

        public void OpenToApplication(string url)
        {
            driver.Navigate().GoToUrl(url);
            Thread.Sleep(4000);
        }

        public void LoginToApplication(string _username, string _password)
        {
            utilities.EnterText(usernameField, _username);
            Thread.Sleep(1000);
            utilities.EnterText(passwordField, _password);
            Thread.Sleep(1000);
            utilities.ClickButton(loginButton);
            Thread.Sleep(4000);

            var succesElement = driver.FindElement(logo);
            Assert.IsNotNull(succesElement, "No se inició sesión correctamente.");
        }

        public void NavegarAPlanDeServicio()
        {
            utilities.ClickButton(moduloFacturacionCiclica);
            Thread.Sleep(1000);
            utilities.ClickButton(submoduloPlanServicio);
            Thread.Sleep(2000);
        }

        public void ConfigurarLimitesComprobantes(string min, string max)
        {
            utilities.ClickButton(btnDetallesPlan);
            utilities.EnterText(txtMinComprobantes, min);
            utilities.EnterText(txtMaxComprobantes, max);
        }

        public void ConfigurarLimitesLocalesYUsuarios(string entidad, string min, string max)
        {
            if (entidad.ToLower().Contains("locales"))
            {
                utilities.EnterText(txtMinLocales, min);
                utilities.EnterText(txtMaxLocales, max);
            }
            else if (entidad.ToLower().Contains("usuarios"))
            {
                utilities.EnterText(txtMinUsuarios, min);
                utilities.EnterText(txtMaxUsuarios, max);
            }
        }

        public void CompletarDatosGenerales(string nombre, string descripcion, string ciclo, string precio)
        {
            // 1. Asegurar pestaña
            utilities.ClickButton(tabDatosGenerales);
            Thread.Sleep(800); // Angular tabs delay real

            if (!string.IsNullOrEmpty(nombre))
                utilities.EnterText(txtNombrePlan, nombre);

            if (!string.IsNullOrEmpty(descripcion))
                utilities.EnterText(txtDescripcionPlan, descripcion);

            if (!string.IsNullOrEmpty(ciclo))
            {
                var select = driver.FindElement(selectCicloFacturacion);

                // 2. Scroll forzado
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", select);

                Thread.Sleep(500);

                // 3. Seteo Angular-safe
                ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].value = arguments[1];
                arguments[0].dispatchEvent(new Event('change'));
                ", select, ObtenerValorCiclo(ciclo));
            }

            if (!string.IsNullOrEmpty(precio))
                utilities.EnterText(txtPrecioPlan, precio);
        }

        private string ObtenerValorCiclo(string ciclo)
        {
            return ciclo.Trim().ToUpper() switch
            {
                "ANUAL" => "1001",
                "SEMESTRAL" => "1002",
                "TRIMESTRAL" => "1003",
                "BIMESTRAL" => "1004",
                "MENSUAL" => "1005",
                _ => throw new ArgumentException($"Ciclo de facturación no válido: {ciclo}")
            };
        }

        public void ClickGuardar()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardar)).Click();
        }

        public void ConfirmarRegistroCorrecto()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Esperar a que aparezca el botón OK del modal
            var btnOk = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(btnOkModal)
            );

            // Asegurar que sea clickeable
            wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(btnOk)
            ).Click();
        }
    }
}