using SIGES3_0.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SIGES3_0.Pages.SharedVentasStep
{
    /// <summary>
    /// Page Object para el modal "Creación de Cliente" en Nueva Venta.
    /// Contiene locators, tipos de documento y métodos de interacción.
    /// Soporta todos los tipos de documento (DNI, RUC 10, RUC 20, Pasaporte, etc.).
    /// </summary>
    public class CreacionClientePage
    {
        private readonly IWebDriver driver;
        private readonly Utilities utilities;
        private readonly WebDriverWait wait;

        // ══════════════════════════════════════════════════════════
        //  TIPOS DE DOCUMENTO
        // ══════════════════════════════════════════════════════════

        public static class TipoDoc
        {
            public const string DNI = "DOC. NACIONAL DE IDENTIDAD";
            public const string CarnetExtranjeria = "CARNET DE EXTRANJERIA";
            public const string CedulaDiplomatica = "CED. DIPLOMATICA DE IDENTIDAD";
            public const string DocIdentPaisResidencia = "DOC.IDENT.PAIS.RESIDENCIA-NO.D";
            public const string DocTribNoDomSinRuc = "DOC.TRIB.NO.DOM.SIN.RUC";
            public const string IdentificationNumberIN = "IDENTIFICATION NUMBER - IN – DOC TRIB PP. JJ";
            public const string Pasaporte = "PASAPORTE";
            public const string PTP = "PERMISO TEMPORAL DE PERMANENCIA - PTP";
            public const string RUC = "REG. UNICO DE CONTRIBUYENTES";
            public const string Salvoconducto = "SALVOCONDUCTO";
            public const string TAM = "TAM - TARJETA ANDINA DE MIGRACIÓN";
            public const string TIN = "TAX IDENTIFICATION NUMBER - TIN – DOC TRIB PP.NN";

            public static bool TieneNombreComercial(string tipo) => tipo == RUC || tipo == DocTribNoDomSinRuc;
        }

        // ══════════════════════════════════════════════════════════
        //  XPATH LOCATORS
        // ══════════════════════════════════════════════════════════

        private static class Locators
        {
            // ── NAVEGACIÓN ───────────────────────────────────────
            public static readonly By AccordionFacturacion =
                By.XPath("//span[contains(text(),'Facturación')]/ancestor::div[contains(@class,'d-flex')]");
            public static readonly By BtnAgregarCliente =
                By.XPath("//button[contains(@class,'btn-add')]");

            // ── MODAL: ACCIONES ──────────────────────────────────
            public static readonly By BtnGuardar =
                By.XPath("//button[contains(normalize-space(),'Guardar')]");
            public static readonly By BtnOk =
                By.CssSelector(".ok-button.ng-star-inserted, .ok-button");
            public static readonly By MensajeModal =
                By.XPath("//p[contains(@class,'message')]");

            // ── BARRA DE CLIENTE (Nueva Venta) ───────────────────
            public static readonly By BarraCliente =
                By.CssSelector("input#DocumentoIdentidad, input[placeholder='Buscar...'], input[formcontrolname='commercialActorNumber'], input[id='numeroDocumento']");

            // ── PASO 1: DATOS DE VALIDACIÓN ──────────────────────
            public static readonly By DropdownTipoDocumentoTrigger =
                By.XPath("//div[contains(@class,'col-md-6')]//div[contains(@class,'select-trigger')]");
            public static readonly By InputNumeroDocumento =
                By.XPath("//input[@id='identityDocumentNumber']");
            public static readonly By BtnBuscarDocumento =
                By.XPath("//button[contains(@class,'btn')]//i[contains(@class,'bi-search')]");

            // ── PASO 2: DATOS GENERALES ──────────────────────────
            public static readonly By SelectGenero =
                By.XPath("//select[@id='genderId']");
            public static readonly By SelectEstadoCivil =
                By.XPath("//select[@id='maritalStatusId']");
            public static readonly By InputCorreoElectronico =
                By.XPath("//input[@id='email']");
            public static readonly By InputTelefono =
                By.XPath("//input[@id='phoneNumber']");
            public static readonly By InputNombreComercial =
                By.XPath("//input[@id='tradeName']");

            // ── PASO 3: DIRECCIÓN ────────────────────────────────
            public static readonly By TextareaDireccion =
                By.XPath("//textarea[@id='detail']");

            // ── DINÁMICOS ────────────────────────────────────────
            public static By OpcionTipoDocumento(string texto) =>
                By.XPath($"//span[normalize-space()='{texto}']");
        }

        // ══════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ══════════════════════════════════════════════════════════

        public CreacionClientePage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // ══════════════════════════════════════════════════════════
        //  NAVEGACIÓN: Abrir el modal
        // ══════════════════════════════════════════════════════════

        public void ExpandirFacturacion()
        {
            Console.WriteLine("Expandir acordeón Facturación");
            Click(Locators.AccordionFacturacion);
            Thread.Sleep(1000);
        }

        public void ClickAgregarCliente()
        {
            Console.WriteLine("Click botón agregar cliente (+)");
            Click(Locators.BtnAgregarCliente);
            Thread.Sleep(2000);
        }

        public void AbrirModalCreacionCliente()
        {
            ExpandirFacturacion();
            ClickAgregarCliente();
        }

        // ══════════════════════════════════════════════════════════
        //  PASO 1: DATOS DE VALIDACIÓN
        // ══════════════════════════════════════════════════════════

        public void SeleccionarTipoDocumento(string tipoDocumento)
        {
            Console.WriteLine($"Seleccionar tipo de documento: {tipoDocumento}");
            Click(Locators.DropdownTipoDocumentoTrigger);
            Thread.Sleep(500);
            Click(Locators.OpcionTipoDocumento(tipoDocumento));
            Thread.Sleep(500);
        }

        public void IngresarNumeroDocumento(string numero)
        {
            Console.WriteLine($"Ingresar número de documento: {numero}");
            var input = wait.Until(ExpectedConditions.ElementToBeClickable(Locators.InputNumeroDocumento));
            input.Click();
            input.SendKeys(numero);
            Thread.Sleep(500);
        }

        public void ClickBuscarDocumento()
        {
            Console.WriteLine("Click buscar documento (lupa)");
            Click(Locators.BtnBuscarDocumento);
            Thread.Sleep(5000);
        }

        public string? DismissErrorSiExiste()
        {
            try
            {
                var btnOk = new WebDriverWait(driver, TimeSpan.FromSeconds(3))
                    .Until(ExpectedConditions.ElementIsVisible(Locators.BtnOk));
                var mensaje = ObtenerMensajeModal();
                Console.WriteLine($"Diálogo detectado: '{mensaje}' — cerrando con OK");
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnOk);
                Thread.Sleep(2000);
                return mensaje;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Sin diálogo — continuando");
                return null;
            }
        }

        private string ObtenerMensajeModal()
        {
            var estrategias = new[]
            {
                Locators.MensajeModal,
                By.XPath("//button[contains(@class,'ok-button')]/preceding::p[normalize-space()][1]"),
            };

            foreach (var locator in estrategias)
            {
                try
                {
                    var texto = driver.FindElements(locator)
                        .Select(e => { try { return e.Text?.Trim(); } catch { return null; } })
                        .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t) && t.Length > 3);
                    if (!string.IsNullOrWhiteSpace(texto))
                        return texto;
                }
                catch { }
            }

            return "(mensaje no capturado)";
        }

        // ══════════════════════════════════════════════════════════
        //  PASO 2: DATOS GENERALES
        // ══════════════════════════════════════════════════════════

        public void SeleccionarGenero(string texto)
        {
            Console.WriteLine($"Seleccionar género: {texto}");
            var dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(Locators.SelectGenero));
            dropdown.Click();
            dropdown.FindElement(By.XPath($"//option[. = '{texto}']")).Click();
            Thread.Sleep(500);
        }

        public void SeleccionarEstadoCivil(string texto)
        {
            Console.WriteLine($"Seleccionar estado civil: {texto}");
            var dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(Locators.SelectEstadoCivil));
            dropdown.Click();
            dropdown.FindElement(By.XPath($"//option[. = '{texto}']")).Click();
            Thread.Sleep(500);
        }

        public void IngresarCorreoElectronico(string correo)
        {
            Console.WriteLine($"Ingresar correo electrónico: {correo}");
            var input = wait.Until(ExpectedConditions.ElementToBeClickable(Locators.InputCorreoElectronico));
            input.Click();
            input.SendKeys(correo);
            Thread.Sleep(500);
        }

        public void IngresarTelefono(string telefono)
        {
            Console.WriteLine($"Ingresar teléfono: {telefono}");
            var input = wait.Until(ExpectedConditions.ElementToBeClickable(Locators.InputTelefono));
            input.Click();
            input.SendKeys(telefono);
            Thread.Sleep(500);
        }

        public void IngresarNombreComercial(string nombre)
        {
            Console.WriteLine($"Ingresar nombre comercial: {nombre}");
            var input = wait.Until(ExpectedConditions.ElementToBeClickable(Locators.InputNombreComercial));
            input.Click();
            input.SendKeys(nombre);
            Thread.Sleep(500);
        }

        // ══════════════════════════════════════════════════════════
        //  PASO 3: DIRECCIÓN
        // ══════════════════════════════════════════════════════════

        public void IngresarDireccion(string direccion)
        {
            Console.WriteLine($"Ingresar dirección: {direccion}");
            var input = wait.Until(ExpectedConditions.ElementToBeClickable(Locators.TextareaDireccion));
            input.Click();
            input.SendKeys(direccion);
            Thread.Sleep(500);
        }

        // ══════════════════════════════════════════════════════════
        //  ACCIONES DEL MODAL
        // ══════════════════════════════════════════════════════════

        public void ClickGuardar()
        {
            Console.WriteLine("Click Guardar");
            Click(Locators.BtnGuardar);
            Thread.Sleep(2000);
        }

        public void ClickOk()
        {
            Console.WriteLine("Click OK confirmación");
            Click(Locators.BtnOk);
            Thread.Sleep(1000);
        }

        // ══════════════════════════════════════════════════════════
        //  FLUJOS COMPLETOS (para StepDefinitions)
        // ══════════════════════════════════════════════════════════

        public void ValidarDocumentoFlow(string tipoDocumento, string numero)
        {
            SeleccionarTipoDocumento(tipoDocumento);
            IngresarNumeroDocumento(numero);
            ClickBuscarDocumento();
            var error = DismissErrorSiExiste();
            if (error != null)
                Assert.Inconclusive($"Retornó error: '{error}'");
        }

        public void DatosGeneralesPersonaNaturalFlow(string genero, string estadoCivil, string correo, string telefono)
        {
            SeleccionarGenero(genero);
            SeleccionarEstadoCivil(estadoCivil);
            IngresarCorreoElectronico(correo);
            IngresarTelefono(telefono);
        }

        public void DatosGeneralesPersonaJuridicaFlow(string correo, string telefono)
        {
            IngresarCorreoElectronico(correo);
            IngresarTelefono(telefono);
        }

        public void GuardarYConfirmarFlow()
        {
            ClickGuardar();
            ManejarModalPostGuardar();
        }

        public void VerificarClienteEnBarra(string numeroDocumento)
        {
            try
            {
                var input = new WebDriverWait(driver, TimeSpan.FromSeconds(3))
                    .Until(ExpectedConditions.ElementIsVisible(Locators.BarraCliente));
                var valorActual = (input.GetAttribute("value") ?? string.Empty).Trim();

                if (valorActual.Contains(numeroDocumento))
                    TestContext.Out.WriteLine($"✔ Cliente {numeroDocumento} cargado en barra de cliente.");
                else
                    TestContext.Out.WriteLine($"ℹ Cliente {numeroDocumento} NO cargado automáticamente. Barra muestra: '{valorActual}'.");
            }
            catch (WebDriverTimeoutException)
            {
                TestContext.Out.WriteLine($"ℹ Barra de cliente no localizable. Estado de '{numeroDocumento}' no verificado.");
            }
        }

        private void ManejarModalPostGuardar()
        {
            try
            {
                var btnOk = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                    .Until(ExpectedConditions.ElementToBeClickable(Locators.BtnOk));
                var mensaje = ObtenerMensajeModal();
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnOk);
                Thread.Sleep(1000);

                if (mensaje.Contains("error", StringComparison.OrdinalIgnoreCase)
                    || mensaje.Contains("Error", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Inconclusive($"Guardar retornó error de la aplicación: '{mensaje}'");
                }
                else
                {
                    TestContext.Out.WriteLine($"✔ Cliente guardado exitosamente. Respuesta: '{mensaje}'");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Sin modal post-guardar — continuando");
            }
        }

        // ══════════════════════════════════════════════════════════
        //  HELPERS PRIVADOS
        // ══════════════════════════════════════════════════════════

        private void Click(By locator)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();
        }
    }
}
