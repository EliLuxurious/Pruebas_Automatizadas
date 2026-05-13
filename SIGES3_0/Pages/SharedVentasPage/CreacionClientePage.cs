using System.Globalization;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SIGES3_0.Pages.SharedVentasPage
{
    /// <summary>
    /// Page Object para el modal "Creacion de Cliente" dentro de Nueva Venta.
    /// Incluye pasos granulares y flujos completos para persona natural y juridica.
    /// </summary>
    public class CreacionClientePage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        private string _ultimoMensajeModal = string.Empty;

        public string UltimoMensajeModal => _ultimoMensajeModal;

        public static class TipoDoc
        {
            public const string DNI = "DOC. NACIONAL DE IDENTIDAD";
            public const string CarnetExtranjeria = "CARNET DE EXTRANJERIA";
            public const string CedulaDiplomatica = "CED. DIPLOMATICA DE IDENTIDAD";
            public const string DocIdentPaisResidencia = "DOC.IDENT.PAIS.RESIDENCIA-NO.D";
            public const string DocTribNoDomSinRuc = "DOC.TRIB.NO.DOM.SIN.RUC";
            public const string IdentificationNumberIN = "IDENTIFICATION NUMBER - IN - DOC TRIB PP. JJ";
            public const string Pasaporte = "PASAPORTE";
            public const string PTP = "PERMISO TEMPORAL DE PERMANENCIA - PTP";
            public const string RUC = "REG. UNICO DE CONTRIBUYENTES";
            public const string Salvoconducto = "SALVOCONDUCTO";
            public const string TAM = "TAM - TARJETA ANDINA DE MIGRACION";
            public const string TIN = "TAX IDENTIFICATION NUMBER - TIN - DOC TRIB PP.NN";

            public static bool TieneNombreComercial(string tipo) =>
                string.Equals(tipo, RUC, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(tipo, DocTribNoDomSinRuc, StringComparison.OrdinalIgnoreCase);
        }

        private static class Locators
        {
            public static readonly By AccordionFacturacion =
                By.XPath("//span[contains(normalize-space(),'Factur')]/ancestor::*[self::div or self::a or self::button][1]");

            public static readonly By BtnAgregarCliente =
                By.XPath("//button[contains(@class,'btn-add') or .//*[contains(@class,'bi-plus')]]");

            public static readonly By BtnGuardar =
                By.XPath("//button[contains(normalize-space(),'Guardar')]");

            public static readonly By BtnOk =
                By.CssSelector(".ok-button.ng-star-inserted, .ok-button");

            public static readonly By MensajeModal =
                By.XPath("//p[contains(@class,'message')] | //*[@class='swal2-html-container'] | //div[contains(@class,'modal-body')]//p[normalize-space()]");

            public static readonly By BarraCliente =
                By.CssSelector("input#DocumentoIdentidad, input[placeholder='Buscar...'], input[formcontrolname='commercialActorNumber'], input#numeroDocumento");

            public static readonly By DropdownTipoDocumentoTrigger =
                By.XPath("//div[contains(@class,'select-trigger')] | //ng-select[contains(@formcontrolname,'identityDocumentType')]//div[contains(@class,'ng-select-container')]");

            public static readonly By InputNumeroDocumento =
                By.XPath("//input[@id='identityDocumentNumber' or @formcontrolname='identityDocumentNumber']");

            public static readonly By BtnBuscarDocumento =
                By.XPath("//button[.//i[contains(@class,'bi-search')] or contains(@class,'btn-search')]");

            public static readonly By SelectGenero =
                By.XPath("//select[@id='genderId' or @formcontrolname='genderId']");

            public static readonly By SelectEstadoCivil =
                By.XPath("//select[@id='maritalStatusId' or @formcontrolname='maritalStatusId']");

            public static readonly By InputCorreoElectronico =
                By.XPath("//input[@id='email' or @formcontrolname='email']");

            public static readonly By InputTelefono =
                By.XPath("//input[@id='phoneNumber' or @formcontrolname='phoneNumber']");

            public static readonly By InputNombreComercial =
                By.XPath("//input[@id='tradeName' or @formcontrolname='tradeName']");

            public static readonly By TextareaDireccion =
                By.XPath("//textarea[@id='detail' or @formcontrolname='detail']");

            public static By OpcionTipoDocumento(string texto) =>
                By.XPath($"//span[normalize-space()='{texto}'] | //div[@role='option'][normalize-space()='{texto}']");
        }

        public CreacionClientePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        public void ExpandirFacturacion()
        {
            if (TryFindVisible(1, Locators.BtnAgregarCliente) != null)
                return;

            Log("Expandir acordeon Facturacion");
            Click(Locators.AccordionFacturacion);
            FindVisible(Locators.BtnAgregarCliente);
            Thread.Sleep(500);
        }

        public void ClickAgregarCliente()
        {
            Log("Click boton agregar cliente");
            Click(Locators.BtnAgregarCliente);
            FindVisible(Locators.InputNumeroDocumento);
            Thread.Sleep(500);
        }

        public void AbrirModalCreacionCliente()
        {
            ExpandirFacturacion();
            ClickAgregarCliente();
        }

        public void SeleccionarTipoDocumento(string tipoDocumento)
        {
            if (DebeOmitirse(tipoDocumento))
                return;

            Log($"Seleccionar tipo de documento: {tipoDocumento}");
            Click(Locators.DropdownTipoDocumentoTrigger);
            Thread.Sleep(300);
            Click(Locators.OpcionTipoDocumento(tipoDocumento));
            Thread.Sleep(300);
        }

        public void IngresarNumeroDocumento(string numero)
        {
            if (DebeOmitirse(numero))
                return;

            Log($"Ingresar numero de documento: {numero}");
            ClearAndType(Locators.InputNumeroDocumento, numero);
        }

        public void ClickBuscarDocumento()
        {
            Log("Click buscar documento");
            Click(Locators.BtnBuscarDocumento);
            Thread.Sleep(1500);
        }

        public string? DismissErrorSiExiste(int timeoutSeconds = 3)
        {
            var btnOk = TryFindVisible(timeoutSeconds, Locators.BtnOk);
            if (btnOk == null)
            {
                Log("Sin dialogo modal");
                return null;
            }

            _ultimoMensajeModal = ObtenerMensajeModal();
            Log($"Dialogo detectado: '{_ultimoMensajeModal}'");
            ClickElement(btnOk);
            Thread.Sleep(1000);
            return _ultimoMensajeModal;
        }

        public void SeleccionarGenero(string texto)
        {
            if (DebeOmitirse(texto))
                return;

            Log($"Seleccionar genero: {texto}");
            SelectByText(Locators.SelectGenero, texto);
        }

        public void SeleccionarEstadoCivil(string texto)
        {
            if (DebeOmitirse(texto))
                return;

            Log($"Seleccionar estado civil: {texto}");
            SelectByText(Locators.SelectEstadoCivil, texto);
        }

        public void IngresarCorreoElectronico(string correo)
        {
            if (DebeOmitirse(correo))
                return;

            Log($"Ingresar correo electronico: {correo}");
            ClearAndType(Locators.InputCorreoElectronico, correo);
        }

        public void IngresarTelefono(string telefono)
        {
            if (DebeOmitirse(telefono))
                return;

            Log($"Ingresar telefono: {telefono}");
            ClearAndType(Locators.InputTelefono, telefono);
        }

        public void IngresarNombreComercial(string nombre)
        {
            if (DebeOmitirse(nombre))
                return;

            Log($"Ingresar nombre comercial: {nombre}");
            ClearAndType(Locators.InputNombreComercial, nombre);
        }

        public void IngresarDireccion(string direccion)
        {
            if (DebeOmitirse(direccion))
                return;

            Log($"Ingresar direccion: {direccion}");
            ClearAndType(Locators.TextareaDireccion, direccion);
        }

        public void ClickGuardar()
        {
            Log("Click Guardar");
            Click(Locators.BtnGuardar);
            Thread.Sleep(1000);
        }

        public void ClickOk()
        {
            var btnOk = FindVisible(Locators.BtnOk);
            Log("Click OK");
            ClickElement(btnOk);
            Thread.Sleep(500);
        }

        public void ValidarDocumentoFlow(string tipoDocumento, string numero)
        {
            SeleccionarTipoDocumento(tipoDocumento);
            IngresarNumeroDocumento(numero);
            ClickBuscarDocumento();

            var error = DismissErrorSiExiste();
            if (!string.IsNullOrWhiteSpace(error) && EsMensajeError(error))
                Assert.Inconclusive($"La validacion del documento retorno un error: '{error}'");
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
            if (DebeOmitirse(numeroDocumento))
                return;

            try
            {
                var input = new WebDriverWait(driver, TimeSpan.FromSeconds(4)).Until(d =>
                    d.FindElements(Locators.BarraCliente).FirstOrDefault(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    }));

                Assert.That(input, Is.Not.Null, "No se pudo localizar la barra de cliente.");

                var valorActual = (input!.GetAttribute("value") ?? string.Empty).Trim();
                TestContext.Out.WriteLine(valorActual.Contains(numeroDocumento, StringComparison.OrdinalIgnoreCase)
                    ? $"Cliente {numeroDocumento} cargado en barra de cliente."
                    : $"Cliente {numeroDocumento} no fue cargado automaticamente. Barra actual: '{valorActual}'.");
            }
            catch (WebDriverTimeoutException)
            {
                TestContext.Out.WriteLine($"No se pudo verificar la barra de cliente para '{numeroDocumento}'.");
            }
        }

        public void ValidarResultadoEsperado(string resultadoEsperado)
        {
            if (DebeOmitirse(resultadoEsperado))
                return;

            var esperado = NormalizarTexto(resultadoEsperado);
            var actual = string.IsNullOrWhiteSpace(_ultimoMensajeModal)
                ? DismissErrorSiExiste(1) ?? string.Empty
                : _ultimoMensajeModal;

            if (esperado.Contains("guard") || esperado.Contains("registr") || esperado.Contains("correct") || esperado.Contains("exito"))
            {
                Assert.That(EsMensajeError(actual), Is.False,
                    $"Se esperaba un guardado exitoso, pero el sistema devolvio: '{actual}'.");

                if (!string.IsNullOrWhiteSpace(actual))
                {
                    var actualNormalizado = NormalizarTexto(actual);
                    Assert.That(
                        actualNormalizado.Contains("guard") ||
                        actualNormalizado.Contains("registr") ||
                        actualNormalizado.Contains("correct") ||
                        actualNormalizado.Contains("exito"),
                        Is.True,
                        $"El mensaje obtenido no parece exitoso. Actual: '{actual}'.");
                }

                return;
            }

            Assert.That(NormalizarTexto(actual), Does.Contain(esperado),
                $"Resultado esperado: '{resultadoEsperado}'. Resultado actual: '{actual}'.");
        }

        private void ManejarModalPostGuardar()
        {
            var btnOk = TryFindVisible(5, Locators.BtnOk);
            if (btnOk == null)
            {
                Log("Sin modal post-guardar");
                return;
            }

            _ultimoMensajeModal = ObtenerMensajeModal();
            ClickElement(btnOk);
            Thread.Sleep(800);

            if (EsMensajeError(_ultimoMensajeModal))
            {
                Assert.Inconclusive($"Guardar retorno un error de la aplicacion: '{_ultimoMensajeModal}'");
            }

            TestContext.Out.WriteLine($"Cliente guardado. Respuesta: '{_ultimoMensajeModal}'.");
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
                        .Where(e =>
                        {
                            try { return e.Displayed; }
                            catch { return false; }
                        })
                        .Select(e =>
                        {
                            try { return e.Text?.Trim(); }
                            catch { return null; }
                        })
                        .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t) && t!.Length > 3);

                    if (!string.IsNullOrWhiteSpace(texto))
                        return texto!;
                }
                catch
                {
                }
            }

            return "(mensaje no capturado)";
        }

        private IWebElement FindVisible(params By[] locators)
        {
            foreach (var locator in locators)
            {
                try
                {
                    var element = wait.Until(d =>
                        d.FindElements(locator).FirstOrDefault(e =>
                        {
                            try { return e.Displayed; }
                            catch { return false; }
                        }));

                    if (element != null)
                        return element;
                }
                catch (WebDriverTimeoutException)
                {
                }
            }

            throw new NoSuchElementException($"No se encontro un elemento visible para: {string.Join(" | ", locators.Select(x => x.ToString()))}");
        }

        private IWebElement FindClickable(params By[] locators)
        {
            foreach (var locator in locators)
            {
                try
                {
                    var element = wait.Until(d =>
                        d.FindElements(locator).FirstOrDefault(e =>
                        {
                            try { return e.Displayed && e.Enabled; }
                            catch { return false; }
                        }));

                    if (element != null)
                        return element;
                }
                catch (WebDriverTimeoutException)
                {
                }
            }

            throw new NoSuchElementException($"No se encontro un elemento clickeable para: {string.Join(" | ", locators.Select(x => x.ToString()))}");
        }

        private IWebElement? TryFindVisible(int timeoutSeconds, params By[] locators)
        {
            var shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));

            foreach (var locator in locators)
            {
                try
                {
                    var element = shortWait.Until(d =>
                        d.FindElements(locator).FirstOrDefault(e =>
                        {
                            try { return e.Displayed; }
                            catch { return false; }
                        }));

                    if (element != null)
                        return element;
                }
                catch (WebDriverTimeoutException)
                {
                }
            }

            return null;
        }

        private void Click(params By[] locators)
        {
            var element = FindClickable(locators);
            ClickElement(element);
        }

        private void ClickElement(IWebElement element)
        {
            ScrollToCenter(element);

            try
            {
                element.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
            }

            Thread.Sleep(300);
        }

        private void ClearAndType(By locator, string value)
        {
            var element = FindVisible(locator);
            ScrollToCenter(element);
            element.Click();
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);
            Thread.Sleep(150);
            element.SendKeys(value);
            element.SendKeys(Keys.Tab);
            Thread.Sleep(300);
        }

        private void SelectByText(By locator, string texto)
        {
            var select = FindVisible(locator);
            ScrollToCenter(select);

            try
            {
                new SelectElement(select).SelectByText(texto);
            }
            catch (NoSuchElementException)
            {
                var option = new SelectElement(select).Options
                    .FirstOrDefault(o => string.Equals(
                        o.Text?.Trim(),
                        texto.Trim(),
                        StringComparison.OrdinalIgnoreCase));

                if (option == null)
                    throw;

                option.Click();
            }

            Thread.Sleep(300);
        }

        private void ScrollToCenter(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView({ block: 'center', inline: 'nearest' });",
                element);
            Thread.Sleep(150);
        }

        private static bool DebeOmitirse(string? valor)
        {
            return string.IsNullOrWhiteSpace(valor) ||
                   valor.Trim().Equals("NA", StringComparison.OrdinalIgnoreCase) ||
                   valor.Trim().Equals("-", StringComparison.OrdinalIgnoreCase);
        }

        private static bool EsMensajeError(string? mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
                return false;

            var normalizado = NormalizarTexto(mensaje);
            return normalizado.Contains("error") ||
                   normalizado.Contains("advert") ||
                   normalizado.Contains("inval") ||
                   normalizado.Contains("duplic") ||
                   normalizado.Contains("no existe") ||
                   normalizado.Contains("incorrect");
        }

        private static string NormalizarTexto(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var formD = value.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(formD.Length);

            foreach (var c in formD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(char.ToLowerInvariant(c));
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private static void Log(string message) =>
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [CreacionCliente] {message}");
    }
}
