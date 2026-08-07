using FLOTA_VEHICULAR.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace FLOTA_VEHICULAR.Pages.Soat
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
            Thread.Sleep(2000);
            IWebElement modulo = wait.Until(ExpectedConditions.ElementExists(moduloSoat));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", modulo);
            Thread.Sleep(2000);
        }

        public void ClicNuevoSoat()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnNuevo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(2500);
        }

        // =============================
        // FORMULARIO SOAT
        // =============================
        private By txtPlaca = By.XPath("(//mat-form-field[not(ancestor::table) and not(ancestor::p-table)][contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'placa')]//input)[1] | (//form//input)[1]");
        private By btnLupaPlaca = By.XPath("(//mat-icon[normalize-space()='search' and not(ancestor::table) and not(ancestor::p-table) and not(ancestor::td)])[1]");
        private By selectProveedor = By.XPath("//mat-form-field[not(ancestor::table) and not(ancestor::p-table)][contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'proveedor')]//mat-select");
        private By txtPoliza = By.XPath("//input[@formcontrolname='policyNumber']");

        // Botones de calendario
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
            var wait = Wait(20);
            Thread.Sleep(1500);

            IWebElement inputPlacaFresco = wait.Until(ExpectedConditions.ElementExists(txtPlaca));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", inputPlacaFresco);
            Thread.Sleep(500);

            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(inputPlacaFresco));
                inputPlacaFresco.Click();
                inputPlacaFresco.SendKeys(Keys.Control + "a");
                inputPlacaFresco.SendKeys(Keys.Backspace);
                inputPlacaFresco.SendKeys(placa);
            }
            catch (StaleElementReferenceException)
            {
                inputPlacaFresco = wait.Until(ExpectedConditions.ElementExists(txtPlaca));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='';", inputPlacaFresco);
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].value=arguments[1]; arguments[0].dispatchEvent(new Event('input', { bubbles: true })); arguments[0].dispatchEvent(new Event('change', { bubbles: true }));",
                    inputPlacaFresco,
                    placa
                );
            }

            Thread.Sleep(800);

            IWebElement botonBusqueda = ObtenerBotonBusquedaCercanoAInput(inputPlacaFresco, "PLACA");
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", botonBusqueda);
            Thread.Sleep(300);

            try
            {
                botonBusqueda.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonBusqueda);
            }

            Thread.Sleep(3000);
        }



        private IWebElement ObtenerBotonBusquedaCercanoAInput(IWebElement input, string nombreCampo)
        {
            var wait = Wait(10);

            var candidatos = input.FindElements(By.XPath(
                "./ancestor::mat-form-field[1]/following::button[1]" +
                " | ./ancestor::mat-form-field[1]//button" +
                " | ./ancestor::*[contains(@class,'row') or contains(@class,'form') or contains(@class,'col')][1]//button[.//mat-icon[normalize-space()='search']]" +
                " | ./following::button[.//mat-icon[normalize-space()='search']][1]" +
                " | ./following::button[contains(@class,'search')][1]" +
                " | ./following::*[self::button or self::span or self::mat-icon][normalize-space()='search'][1]"
            ));

            foreach (var candidato in candidatos)
            {
                try
                {
                    if (candidato.Displayed && candidato.Enabled)
                    {
                        return candidato;
                    }
                }
                catch
                {
                    // Ignorar elementos obsoletos o no visibles.
                }
            }

            // Plan B: buscar cualquier botón visible de búsqueda dentro del formulario, evitando tabla/grilla.
            var botonesGlobales = driver.FindElements(By.XPath(
                "//form//*[self::button or self::mat-icon or self::span]" +
                "[" +
                "contains(@class,'search') or " +
                "normalize-space()='search' or " +
                ".//mat-icon[normalize-space()='search']" +
                "]" +
                "[not(ancestor::table) and not(ancestor::p-table) and not(ancestor::td)]"
            ));

            foreach (var boton in botonesGlobales)
            {
                try
                {
                    if (boton.Displayed && boton.Enabled)
                    {
                        return boton;
                    }
                }
                catch
                {
                    // Ignorar.
                }
            }

            throw new Exception($"Fallo de QA: No se encontró el botón/lupa de búsqueda cercano al campo {nombreCampo}.");
        }



        public void SeleccionarProveedor(string proveedor)
        {
            var wait = Wait(20);
            string proveedorBuscado = proveedor.Trim();

            IWebElement dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(selectProveedor));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", dropdown);
            Thread.Sleep(500);

            try
            {
                dropdown.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dropdown);
            }

            Thread.Sleep(1000);

            wait.Until(ExpectedConditions.ElementExists(
                By.XPath("//div[contains(@class,'cdk-overlay-pane')] | //mat-option")
            ));

            string proveedorNormalizado = proveedorBuscado
                .ToUpper()
                .Replace("Á", "A")
                .Replace("É", "E")
                .Replace("Í", "I")
                .Replace("Ó", "O")
                .Replace("Ú", "U")
                .Replace(" ", "");

            bool encontrado = false;
            int intentos = 0;

            while (!encontrado && intentos < 20)
            {
                var opciones = driver.FindElements(By.XPath(
                    "//mat-option | " +
                    "//div[contains(@class,'mat-option')] | " +
                    "//span[contains(@class,'mat-option-text')] | " +
                    "//span[contains(@class,'mdc-list-item__primary-text')]"
                ));

                foreach (var opcion in opciones)
                {
                    string texto = "";

                    try
                    {
                        texto = opcion.GetAttribute("textContent") ?? "";
                    }
                    catch
                    {
                        continue;
                    }

                    string textoNormalizado = texto
                        .Trim()
                        .ToUpper()
                        .Replace("Á", "A")
                        .Replace("É", "E")
                        .Replace("Í", "I")
                        .Replace("Ó", "O")
                        .Replace("Ú", "U")
                        .Replace(" ", "");

                    if (textoNormalizado.Contains(proveedorNormalizado))
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", opcion);
                        Thread.Sleep(300);

                        try
                        {
                            opcion.Click();
                        }
                        catch
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opcion);
                        }

                        encontrado = true;
                        Thread.Sleep(1000);
                        break;
                    }
                }

                if (!encontrado)
                {
                    new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.ArrowDown).Perform();
                    Thread.Sleep(300);
                    intentos++;
                }
            }

            if (!encontrado)
            {
                string opcionesDisponibles = "";
                var opcionesDebug = driver.FindElements(By.XPath("//mat-option | //span[contains(@class,'mat-option-text')] | //span[contains(@class,'mdc-list-item__primary-text')]"));

                foreach (var opt in opcionesDebug)
                {
                    try
                    {
                        opcionesDisponibles += $"[{opt.GetAttribute("textContent")?.Trim()}] ";
                    }
                    catch { }
                }

                new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();

                throw new Exception($"Fallo de QA: No se encontró el proveedor '{proveedorBuscado}'. Opciones visibles: {opcionesDisponibles}");
            }
        }

        public void IngresarPoliza(string poliza)
        {
            utilities.EnterText(txtPoliza, poliza);
        }

        // =========================================================================
        // MÉTODO CAZA-FANTASMAS DEFINITIVO (CALCULADORA DE MESES Y CLIC AUTOMÁTICO)
        // =========================================================================
        private void SeleccionarFechaDinamicaPorBoton(IWebElement btnCal, DateTime fechaObjetivo, DateTime? fechaAperturaEsperada = null)
        {
            var wait = Wait(10);

            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(500);

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCal);
            Thread.Sleep(500);

            try
            {
                btnCal.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal);
            }

            wait.Until(ExpectedConditions.ElementExists(By.XPath("(//mat-datepicker-content)[last()]")));
            Thread.Sleep(800);

            DateTime fechaApertura = fechaAperturaEsperada ?? DateTime.Today;

            int mesesDiferencia =
                ((fechaObjetivo.Year - fechaApertura.Year) * 12)
                + fechaObjetivo.Month
                - fechaApertura.Month;

            if (mesesDiferencia > 0)
            {
                By btnNextMonth = By.XPath("(//mat-datepicker-content)[last()]//button[contains(@class, 'mat-calendar-next-button')]");

                for (int i = 0; i < mesesDiferencia; i++)
                {
                    IWebElement nextBtn = wait.Until(ExpectedConditions.ElementToBeClickable(btnNextMonth));

                    try
                    {
                        nextBtn.Click();
                    }
                    catch
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", nextBtn);
                    }

                    Thread.Sleep(250);
                }
            }
            else if (mesesDiferencia < 0)
            {
                By btnPrevMonth = By.XPath("(//mat-datepicker-content)[last()]//button[contains(@class, 'mat-calendar-previous-button')]");

                for (int i = 0; i < Math.Abs(mesesDiferencia); i++)
                {
                    IWebElement prevBtn = wait.Until(ExpectedConditions.ElementToBeClickable(btnPrevMonth));

                    try
                    {
                        prevBtn.Click();
                    }
                    catch
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", prevBtn);
                    }

                    Thread.Sleep(250);
                }
            }

            Thread.Sleep(500);

            string dia = fechaObjetivo.Day.ToString();

            string xpathDia = $@"(//mat-datepicker-content)[last()]
        //*[contains(@class, 'mat-calendar-body-cell') 
        and not(contains(@class, 'mat-calendar-body-disabled'))]
        [.//div[contains(@class, 'mat-calendar-body-cell-content') 
        and normalize-space()='{dia}']]";

            IWebElement celdaDia = wait.Until(ExpectedConditions.ElementExists(By.XPath(xpathDia)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", celdaDia);
            Thread.Sleep(200);

            try
            {
                celdaDia.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", celdaDia);
            }

            Thread.Sleep(800);
        }

        public void SeleccionarFechaDinamicaPorCalendario(By btnCalendario, DateTime fechaObjetivo, DateTime? fechaAperturaEsperada = null)
        {
            var wait = Wait(10);

            IWebElement btnCal = wait.Until(ExpectedConditions.ElementToBeClickable(btnCalendario));
            SeleccionarFechaDinamicaPorBoton(btnCal, fechaObjetivo, fechaAperturaEsperada);
        }

        public void SeleccionarFechasVigencia(DateTime fechaDesde, DateTime fechaHasta)
        {
            SeleccionarFechaDinamicaPorCalendario(btnCalDesde, fechaDesde, DateTime.Now);
            SeleccionarFechaDinamicaPorCalendario(btnCalHasta, fechaHasta, fechaDesde);
        }



        public void SeleccionarSoloFechaDesde(DateTime fechaObjetivo)
        {
            fechaDesdeSeleccionadaParaCalendario = fechaObjetivo;
            SeleccionarFechaDinamicaPorCalendario(btnCalDesde, fechaObjetivo, DateTime.Today);
        }


        private DateTime? fechaDesdeSeleccionadaParaCalendario;
        public void VerificarFechaHastaDeshabilitada(DateTime fechaBloqueada)
        {
            var wait = Wait(10);

            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(500);

            IWebElement btnCal = wait.Until(ExpectedConditions.ElementToBeClickable(btnCalHasta));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCal);
            Thread.Sleep(500);

            try
            {
                btnCal.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal);
            }

            wait.Until(ExpectedConditions.ElementExists(By.XPath("(//mat-datepicker-content)[last()]")));
            Thread.Sleep(800);

            DateTime fechaBase = fechaDesdeSeleccionadaParaCalendario ?? DateTime.Today;

            int mesesDiferencia =
                ((fechaBloqueada.Year - fechaBase.Year) * 12)
                + fechaBloqueada.Month
                - fechaBase.Month;

            if (mesesDiferencia > 0)
            {
                By btnNextMonth = By.XPath("(//mat-datepicker-content)[last()]//button[contains(@class, 'mat-calendar-next-button')]");

                for (int i = 0; i < mesesDiferencia; i++)
                {
                    IWebElement nextBtn = wait.Until(ExpectedConditions.ElementToBeClickable(btnNextMonth));
                    try { nextBtn.Click(); }
                    catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", nextBtn); }
                    Thread.Sleep(250);
                }
            }

            string dia = fechaBloqueada.Day.ToString();

            string xpathDia = $@"(//mat-datepicker-content)[last()]
        //*[contains(@class, 'mat-calendar-body-cell')]
        [.//div[contains(@class, 'mat-calendar-body-cell-content')
        and normalize-space()='{dia}']]";

            var celdas = driver.FindElements(By.XPath(xpathDia));

            if (celdas.Count == 0)
            {
                throw new Exception($"Fallo de QA: No se encontró la fecha {fechaBloqueada:dd/MM/yyyy} en el calendario HASTA.");
            }

            bool estaDeshabilitado = false;

            foreach (var celda in celdas)
            {
                string clase = celda.GetAttribute("class") ?? "";
                string ariaDisabled = celda.GetAttribute("aria-disabled") ?? "";
                string disabled = celda.GetAttribute("disabled") ?? "";

                if (clase.Contains("mat-calendar-body-disabled") ||
                    ariaDisabled.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                    disabled.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                    !celda.Enabled)
                {
                    estaDeshabilitado = true;
                    break;
                }
            }

            if (!estaDeshabilitado)
            {
                throw new Exception($"Fallo de QA: La fecha {fechaBloqueada:dd/MM/yyyy} debería estar bloqueada en el calendario HASTA.");
            }

            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(500);
        }




        private void VerificarFechaDeshabilitadaEnCalendario(By btnCalendario, DateTime fechaBloqueada, DateTime fechaBase, string nombreCalendario)
        {
            var wait = Wait(10);

            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(500);

            IWebElement btnCal = wait.Until(ExpectedConditions.ElementToBeClickable(btnCalendario));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCal);
            Thread.Sleep(500);

            try
            {
                btnCal.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal);
            }

            wait.Until(ExpectedConditions.ElementExists(By.XPath("(//mat-datepicker-content)[last()]")));
            Thread.Sleep(800);

            int mesesDiferencia =
                ((fechaBloqueada.Year - fechaBase.Year) * 12)
                + fechaBloqueada.Month
                - fechaBase.Month;

            if (mesesDiferencia > 0)
            {
                By btnNextMonth = By.XPath("(//mat-datepicker-content)[last()]//button[contains(@class, 'mat-calendar-next-button')]");

                for (int i = 0; i < mesesDiferencia; i++)
                {
                    IWebElement nextBtn = wait.Until(ExpectedConditions.ElementToBeClickable(btnNextMonth));
                    try { nextBtn.Click(); }
                    catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", nextBtn); }
                    Thread.Sleep(250);
                }
            }
            else if (mesesDiferencia < 0)
            {
                By btnPrevMonth = By.XPath("(//mat-datepicker-content)[last()]//button[contains(@class, 'mat-calendar-previous-button')]");

                for (int i = 0; i < Math.Abs(mesesDiferencia); i++)
                {
                    IWebElement prevBtn = wait.Until(ExpectedConditions.ElementToBeClickable(btnPrevMonth));
                    try { prevBtn.Click(); }
                    catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", prevBtn); }
                    Thread.Sleep(250);
                }
            }

            string dia = fechaBloqueada.Day.ToString();

            string xpathDia = $@"(//mat-datepicker-content)[last()]
        //*[contains(@class, 'mat-calendar-body-cell')]
        [.//div[contains(@class, 'mat-calendar-body-cell-content')
        and normalize-space()='{dia}']]";

            var celdas = driver.FindElements(By.XPath(xpathDia));

            if (celdas.Count == 0)
            {
                throw new Exception($"Fallo de QA: No se encontró la fecha {fechaBloqueada:dd/MM/yyyy} en el calendario {nombreCalendario}.");
            }

            bool estaDeshabilitado = false;

            foreach (var celda in celdas)
            {
                string clase = celda.GetAttribute("class") ?? "";
                string ariaDisabled = celda.GetAttribute("aria-disabled") ?? "";
                string disabled = celda.GetAttribute("disabled") ?? "";

                if (clase.Contains("mat-calendar-body-disabled") ||
                    ariaDisabled.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                    disabled.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                    !celda.Enabled)
                {
                    estaDeshabilitado = true;
                    break;
                }
            }

            if (!estaDeshabilitado)
            {
                throw new Exception($"Fallo de QA: La fecha {fechaBloqueada:dd/MM/yyyy} debería estar bloqueada en el calendario {nombreCalendario}.");
            }

            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(500);
        }

        public void VerificarFechaContratanteDeshabilitada(DateTime fechaBloqueada, DateTime fechaBase)
        {
            VerificarFechaDeshabilitadaEnCalendario(btnCalContratante, fechaBloqueada, fechaBase, "CONTRATANTE");
        }





        public void SeleccionarFechaContratante(DateTime fecha)
        {
            SeleccionarFechaDinamicaPorCalendario(btnCalContratante, fecha, DateTime.Now);
        }

        public void SeleccionarSoloFechaDesde(string dia)
        {
            int day = int.Parse(dia);
            DateTime fechaObjetivo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
            SeleccionarFechaDinamicaPorCalendario(btnCalDesde, fechaObjetivo, DateTime.Now);
        }

        public void IngresarRucYBuscar(string ruc)
        {
            var wait = Wait();
            utilities.EnterText(txtRuc, ruc);
            Thread.Sleep(1000);
            IWebElement lupa = wait.Until(ExpectedConditions.ElementToBeClickable(btnLupaRuc));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupa);
            Thread.Sleep(3000);
        }

        public void IngresarHoraEImporte(string hora, string importe)
        {
            IWebElement campoHora = driver.FindElement(txtHora);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", campoHora);
            campoHora.SendKeys(hora);
            Thread.Sleep(1000);
            utilities.EnterText(txtImporte, importe);
        }

        public void AdjuntarDocumento(string rutaArchivo)
        {
            var wait = Wait();
            IWebElement fileInput = wait.Until(ExpectedConditions.ElementExists(inputFile));
            fileInput.SendKeys(rutaArchivo);
            Thread.Sleep(4000);
        }

        public void GuardarSoat()
        {
            var wait = Wait();
            Thread.Sleep(2000);

            try
            {
                IWebElement btn = wait.Until(ExpectedConditions.ElementExists(btnGuardar));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);
                Thread.Sleep(500);

                string disabledAttr = btn.GetAttribute("disabled");
                string classAttr = btn.GetAttribute("class");

                if (disabledAttr == "true" || (classAttr != null && classAttr.Contains("disabled")))
                {
                    throw new Exception("El botón Guardar está bloqueado por falta de datos requeridos.");
                }

                try { wait.Until(ExpectedConditions.ElementToBeClickable(btn)).Click(); }
                catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn); }

                Thread.Sleep(4000);
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("Timeout: No se pudo hacer clic en Guardar.");
            }
        }

        public void IngresarPlacaSinBuscar(string placa)
        {
            utilities.EnterText(txtPlaca, placa);
            Thread.Sleep(500);
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Tab).Perform();
            Thread.Sleep(500);
        }

        public void IngresarRucSinBuscar(string ruc)
        {
            utilities.EnterText(txtRuc, ruc);
            Thread.Sleep(500);
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Tab).Perform();
            Thread.Sleep(500);
        }

        public void VerificarBotonGuardarDeshabilitado()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementExists(btnGuardar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);
            Thread.Sleep(500);

            string disabledAttr = btn.GetAttribute("disabled");
            string classAttr = btn.GetAttribute("class");

            if (disabledAttr != "true" && (classAttr == null || !classAttr.Contains("disabled")))
            {
                throw new Exception("Fallo de QA: El botón 'Guardar' debería estar bloqueado (deshabilitado).");
            }
        }


        public void VerificarSoatBloqueadoPorPlacaSinBuscar()
        {
            Thread.Sleep(1000);

            bool proveedorVisible = false;
            bool guardarBloqueado = false;

            // 1. Validar si el campo Proveedor aparece.
            var proveedores = driver.FindElements(selectProveedor);

            foreach (var proveedor in proveedores)
            {
                try
                {
                    if (proveedor.Displayed)
                    {
                        proveedorVisible = true;
                        break;
                    }
                }
                catch
                {
                    // Ignoramos elementos stale o no interactuables.
                }
            }

            // 2. Validar si el botón Guardar existe y está bloqueado.
            var botonesGuardar = driver.FindElements(btnGuardar);

            if (botonesGuardar.Count == 0)
            {
                guardarBloqueado = true;
            }
            else
            {
                foreach (var btn in botonesGuardar)
                {
                    try
                    {
                        string disabledAttr = btn.GetAttribute("disabled");
                        string ariaDisabled = btn.GetAttribute("aria-disabled");
                        string classAttr = btn.GetAttribute("class") ?? "";

                        if (disabledAttr == "true" ||
                            ariaDisabled == "true" ||
                            classAttr.ToLower().Contains("disabled") ||
                            !btn.Enabled)
                        {
                            guardarBloqueado = true;
                            break;
                        }
                    }
                    catch
                    {
                        guardarBloqueado = true;
                    }
                }
            }

            /*
             * La prueba es correcta si:
             * - El proveedor NO aparece porque no se buscó la placa.
             *   O
             * - El botón Guardar aparece, pero está bloqueado.
             */
            if (!proveedorVisible || guardarBloqueado)
            {
                Console.WriteLine("QA OK: El SOAT no permite continuar correctamente porque la placa no fue buscada con la lupa.");
                return;
            }

            throw new Exception("Fallo de QA: El sistema permitió continuar con el registro SOAT sin buscar la placa con la lupa.");
        }






        public void VerificarDiaHastaDeshabilitado(string dia)
        {
            var wait = Wait(10);
            IWebElement btnCal = wait.Until(ExpectedConditions.ElementToBeClickable(btnCalHasta));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCal);
            Thread.Sleep(500);
            try { btnCal.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal); }
            Thread.Sleep(1500);

            string xpathDiaDeshabilitado = $"(//mat-datepicker-content)[last()]//*[contains(@class, 'mat-calendar-body-disabled')]//div[normalize-space()='{dia}']";
            var elementos = driver.FindElements(By.XPath(xpathDiaDeshabilitado));

            if (elementos.Count == 0) throw new Exception($"Fallo de QA: El día {dia} debería estar bloqueado.");

            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(500);
        }

        public void AbrirCalendarioDesdeParaValidacion()
        {
            var wait = Wait(10);
            IWebElement btnCal = wait.Until(ExpectedConditions.ElementToBeClickable(btnCalDesde));
            try { btnCal.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal); }
            Thread.Sleep(1000);
        }

        public void VerificarMensajeErrorSoat(string mensajeEsperado)
        {
            var wait = Wait(10);
            By localizadorMensaje = By.XPath($"//*[contains(text(), '{mensajeEsperado}')]");
            try { wait.Until(ExpectedConditions.ElementIsVisible(localizadorMensaje)); }
            catch (WebDriverTimeoutException) { throw new Exception($"Fallo: Se esperaba el error '{mensajeEsperado}'."); }
            Thread.Sleep(1000);
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
        }

        // =============================
        // MÉTODOS DE GRILLA Y EDICIÓN
        // =============================
        private By btnVerSoat = By.XPath("(//mat-icon[normalize-space()='search' or normalize-space()='visibility'])[1] | //button[contains(@class, 'button-view')]");
        private By btnEditarSoat = By.XPath("//button[contains(@class, 'button-edit')] | //mat-icon[normalize-space()='edit']");

        public void BuscarSoatEnGrillaPorPlaca(string placa)
        {
            var wait = Wait();
            Thread.Sleep(2000);
            By txtFiltroPlaca = By.XPath("//th[7]//input[1]");
            IWebElement filtro = wait.Until(ExpectedConditions.ElementExists(txtFiltroPlaca));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", filtro);
            Thread.Sleep(500);

            try { filtro.Click(); filtro.Clear(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='';", filtro); }
            filtro.SendKeys(placa);
            Thread.Sleep(500);

            try { driver.FindElement(By.XPath("//button[contains(@class, 'mat-raised-button') and contains(., 'Buscar')]")).Click(); }
            catch { filtro.SendKeys(Keys.Enter); }
            Thread.Sleep(2000);
        }

        public void ClicVerSoat()
        {
            var wait = Wait();
            IWebElement btnVer = wait.Until(ExpectedConditions.ElementExists(btnVerSoat));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnVer);
            Thread.Sleep(2000);
        }

        public void ClicEditarSoat()
        {
            var wait = Wait();
            IWebElement btnEdit = wait.Until(ExpectedConditions.ElementExists(btnEditarSoat));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnEdit);
            Thread.Sleep(2000);
        }

        private By chkDocumentoAdjunto = By.XPath("(//div[contains(@class, 'p-checkbox-box')])[1] | //span[contains(@class, 'p-checkbox-icon')]");
        private By btnEliminarDocAdjunto = By.XPath("//button[.//span[contains(@class, 'pi-trash')]] | //button[contains(@ng-reflect-message, 'Eliminar Documento')]");

        public void EliminarDocumentoAdjunto()
        {
            var wait = Wait();
            Thread.Sleep(2000);
            IWebElement checkbox = wait.Until(ExpectedConditions.ElementExists(chkDocumentoAdjunto));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", checkbox);
            Thread.Sleep(500);

            try { wait.Until(ExpectedConditions.ElementToBeClickable(checkbox)).Click(); }
            catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox); }
            Thread.Sleep(1000);

            IWebElement btnBasurero = wait.Until(ExpectedConditions.ElementExists(btnEliminarDocAdjunto));
            try { wait.Until(ExpectedConditions.ElementToBeClickable(btnBasurero)).Click(); }
            catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBasurero); }
            Thread.Sleep(1500);
        }

        private By btnBuscarFiltros = By.XPath("//button[contains(., 'BUSCAR') or contains(., 'Buscar') or contains(@class, 'search')]");

        public void ClicBuscarFiltros()
        {
            var wait = Wait();
            IWebElement btnBuscar = wait.Until(ExpectedConditions.ElementToBeClickable(btnBuscarFiltros));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnBuscar);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBuscar);
            Thread.Sleep(3000);
        }

        public void VerificarGrillaConResultados()
        {
            Thread.Sleep(2000);
            var filas = driver.FindElements(By.XPath("//tbody/tr | //div[contains(@class, 'p-datatable-tbody')]//tr"));

            if (filas.Count == 0) throw new Exception("Fallo: Se esperaban resultados, pero la tabla está completamente vacía.");
            if (filas.Count == 1)
            {
                string textoFila = filas[0].Text.ToLower();
                if (textoFila.Contains("no se encontraron") || textoFila.Contains("disponible") || textoFila.Contains("empty"))
                {
                    throw new Exception("Fallo: Apareció el mensaje de tabla vacía cuando se esperaban datos.");
                }
            }
        }




        public void VerificarSoatRegistradoPorPlaca(string placa)
        {
            var wait = Wait(25);

            // Volvemos al módulo SOAT para asegurar que estamos en la grilla/listado
            try
            {
                IngresarModuloSoat();
            }
            catch
            {
                // Si ya está en SOAT, continuamos normal
            }

            Thread.Sleep(2000);

            // Buscar por placa en la grilla
            BuscarSoatEnGrillaPorPlaca(placa);

            Thread.Sleep(3000);

            string placaMayus = placa.Trim().ToUpper();

            var filas = driver.FindElements(By.XPath(
                "//tbody/tr | //div[contains(@class, 'p-datatable-tbody')]//tr"
            ));

            bool encontrado = false;
            string textoGrilla = "";

            foreach (var fila in filas)
            {
                try
                {
                    string textoFila = fila.Text.Trim().ToUpper();
                    textoGrilla += textoFila + "\n";

                    if (textoFila.Contains(placaMayus))
                    {
                        encontrado = true;
                        break;
                    }
                }
                catch
                {
                    // Ignorar filas obsoletas
                }
            }

            if (!encontrado)
            {
                throw new Exception(
                    $"Fallo de QA: El SOAT de la placa '{placa}' no se visualiza en la grilla después de guardar. " +
                    $"Esto indica que el registro no se guardó correctamente o la búsqueda no lo está mostrando.\n" +
                    $"Contenido encontrado en la grilla:\n{textoGrilla}"
                );
            }

            Console.WriteLine($"QA OK: El SOAT de la placa '{placa}' fue registrado y se visualiza en la grilla.");
        }



        // =============================
        // FILTROS AVANZADOS SOAT
        // =============================
        private By comboAseguradoras = By.XPath("//mat-select[contains(@placeholder, 'Aseguradora') or @formcontrolname='aseguradoras'] | (//mat-select)[1]");
        private By comboAreas = By.XPath("//mat-select[contains(@placeholder, 'rea') or @formcontrolname='areas'] | (//mat-select)[2]");
        private By comboEstado = By.XPath("//mat-select[contains(@placeholder, 'Estado') or @formcontrolname='estado'] | //th[contains(translate(., 'ESTADO', 'estado'), 'estado')]//input | //th[contains(translate(., 'ESTADO', 'estado'), 'estado')]//*[contains(@class, 'p-column-filter')] | (//input[contains(@class, 'p-column-filter')])[1]");

        public void AbrirFiltro(string nombreFiltro)
        {
            var wait = Wait();
            By locator = null;
            if (nombreFiltro.ToUpper().Contains("ASEGURADORA")) locator = comboAseguradoras;
            else if (nombreFiltro.ToUpper().Contains("AREA") || nombreFiltro.ToUpper().Contains("ÁREA")) locator = comboAreas;
            else if (nombreFiltro.ToUpper().Contains("ESTADO")) locator = comboEstado;

            IWebElement combo = wait.Until(ExpectedConditions.ElementExists(locator));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", combo);
            Thread.Sleep(500);
            try { combo.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", combo); }
            Thread.Sleep(1500);
        }

        public void DesmarcarOpcionTodas()
        {
            Thread.Sleep(1500);
            By locatorTodas = By.XPath("//span[contains(@class, 'mat-checkbox-inner-container-no-side-margin')] | //div[contains(@class, 'cdk-overlay-pane')]//mat-checkbox[contains(., 'TODAS')]");
            var elementosTodas = driver.FindElements(locatorTodas);

            if (elementosTodas.Count > 0)
            {
                IWebElement checkboxMaestro = elementosTodas[elementosTodas.Count - 1];
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", checkboxMaestro);
                Thread.Sleep(500);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkboxMaestro);
                Thread.Sleep(1500);
            }
            else throw new Exception("Fallo: No se encontró el checkbox maestro para desmarcar.");
        }

        public void SeleccionarOpcionEnFiltro(string opcion)
        {
            string opcionBuscada = opcion.Trim().ToUpper().Replace("Ó", "O").Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ú", "U").Replace(" ", "").Replace("-", "");
            string primeraLetra = opcion.Trim().Substring(0, 1);
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(primeraLetra).Perform();
            Thread.Sleep(800);

            bool opcionEncontrada = false;
            int intentosScroll = 0;

            while (!opcionEncontrada && intentosScroll < 40)
            {
                var opciones = driver.FindElements(By.XPath("//mat-checkbox | //label[contains(@class, 'mat-checkbox-layout')] | //mat-option | //div[contains(@class, 'mat-list-item-content')]"));

                foreach (var opt in opciones)
                {
                    string textoReal = opt.GetAttribute("textContent") ?? "";
                    textoReal = textoReal.ToUpper().Replace("Ó", "O").Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ú", "U").Replace(" ", "").Replace("-", "");

                    if (textoReal.Contains(opcionBuscada))
                    {
                        IWebElement elementoClic = opt;
                        try
                        {
                            var cuadritos = opt.FindElements(By.XPath(".//span[contains(@class, 'mat-checkbox-inner-container')] | .//span[contains(@class, 'mat-pseudo-checkbox')]"));
                            if (cuadritos.Count > 0) elementoClic = cuadritos[0];
                        }
                        catch { }

                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", elementoClic);
                        Thread.Sleep(500);
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elementoClic);

                        opcionEncontrada = true;
                        Thread.Sleep(1000);
                        break;
                    }
                }

                if (!opcionEncontrada)
                {
                    new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.ArrowDown).Perform();
                    new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.ArrowDown).Perform();
                    Thread.Sleep(200);
                    intentosScroll++;
                }
            }

            if (!opcionEncontrada) throw new Exception($"Fallo de QA: No se pudo encontrar la opción '{opcion}'");
        }

        public void CerrarComboFiltro()
        {
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(1000);
        }

        private By txtFiltroFechaDesde = By.XPath("(//input[contains(@placeholder, 'Desde') or contains(@formcontrolname, 'fechaInicio')])[1]");
        private By txtFiltroFechaHasta = By.XPath("(//input[contains(@placeholder, 'Hasta') or contains(@formcontrolname, 'fechaFin')])[1]");

        private IWebElement ObtenerBotonCalendarioCercanoAInput(By inputLocator)
        {
            var wait = Wait(10);

            IWebElement input = wait.Until(ExpectedConditions.ElementExists(inputLocator));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", input);
            Thread.Sleep(300);

            var botonesMatDatepicker = input.FindElements(By.XPath("./ancestor::mat-form-field[1]//mat-datepicker-toggle//button"));

            if (botonesMatDatepicker.Count > 0)
            {
                return botonesMatDatepicker[0];
            }

            var botonesGenericos = input.FindElements(By.XPath("./ancestor::mat-form-field[1]//button"));

            if (botonesGenericos.Count > 0)
            {
                return botonesGenericos[botonesGenericos.Count - 1];
            }

            var botonSiguiente = input.FindElements(By.XPath("./following::button[1]"));

            if (botonSiguiente.Count > 0)
            {
                return botonSiguiente[0];
            }

            throw new Exception("Fallo de QA: No se encontró el botón de calendario cercano al campo de fecha del filtro.");
        }

        public void SeleccionarRangoFechasFiltro(DateTime fechaDesde, DateTime fechaHasta)
        {
            IWebElement btnDesde = ObtenerBotonCalendarioCercanoAInput(txtFiltroFechaDesde);
            SeleccionarFechaDinamicaPorBoton(btnDesde, fechaDesde, DateTime.Today);

            IWebElement btnHasta = ObtenerBotonCalendarioCercanoAInput(txtFiltroFechaHasta);
            SeleccionarFechaDinamicaPorBoton(btnHasta, fechaHasta, fechaDesde);
        }

        public void IngresarRangoFechasFiltro(string fechaDesde, string fechaHasta)
        {
            DateTime dDesde = DateTime.ParseExact(
                fechaDesde,
                "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture
            );

            DateTime dHasta = DateTime.ParseExact(
                fechaHasta,
                "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture
            );

            SeleccionarRangoFechasFiltro(dDesde, dHasta);
        }

        // =============================
        // HISTORIAL Y FILTRO DE DÍAS
        // =============================
        private By txtDiasParaVencer = By.XPath("//th[3]//input[1]");
        private By btnHistorial = By.XPath("//button[@ng-reflect-message='Ver Historial']//span[@class='mat-button-wrapper'] | //button[@ng-reflect-message='Ver Historial']");
        private By btnCerrarHistorial = By.XPath("//button[contains(@class, 'tsp-button-delete')]//span[@class='mat-button-wrapper'] | //button[contains(@class, 'tsp-button-delete')]");

        public void IngresarDiasParaVencer(string dias)
        {
            var wait = Wait();
            IWebElement inputDias = wait.Until(ExpectedConditions.ElementExists(txtDiasParaVencer));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", inputDias);
            Thread.Sleep(500);

            try { inputDias.Clear(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='';", inputDias); }
            inputDias.SendKeys(dias);
            Thread.Sleep(500);
        }

        public void ClicHistorial()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnHistorial));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(2500);
        }

        public void CerrarHistorial()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnCerrarHistorial));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(1000);
        }
    }
}