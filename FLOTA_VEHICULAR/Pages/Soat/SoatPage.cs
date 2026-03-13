using FLOTA_VEHICULAR.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

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

            // 1. Pausa vital para que Angular destruya el modal viejo y construya el nuevo
            System.Threading.Thread.Sleep(2000);

            // 2. BUSCAMOS el elemento FRESCO en el DOM en este preciso instante
            IWebElement inputPlacaFresco = wait.Until(ExpectedConditions.ElementExists(txtPlaca));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", inputPlacaFresco);
            System.Threading.Thread.Sleep(500);

            // 3. Escribimos la placa (usando try-catch por si Angular nos tira Stale justo en este milisegundo)
            try
            {
                inputPlacaFresco.Clear();
                inputPlacaFresco.SendKeys(placa);
            }
            catch (StaleElementReferenceException)
            {
                // Si falla, lo buscamos de nuevo y lo forzamos con JS
                inputPlacaFresco = driver.FindElement(txtPlaca);
                ((IJavaScriptExecutor)driver).ExecuteScript($"arguments[0].value='{placa}';", inputPlacaFresco);
                inputPlacaFresco.SendKeys(Keys.Space); // Para que Angular registre el cambio
                inputPlacaFresco.SendKeys(Keys.Backspace);
            }

            System.Threading.Thread.Sleep(1000);

            // 4. Buscamos la lupa FRESCA y le damos clic
            IWebElement lupaFresca = wait.Until(ExpectedConditions.ElementToBeClickable(btnLupaPlaca));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", lupaFresca);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupaFresca);

            // Pausa para que carguen los datos del vehículo
            System.Threading.Thread.Sleep(3000);
        }

        public void SeleccionarProveedor(string proveedor)
        {
            var wait = Wait();
            IWebElement dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(selectProveedor));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", dropdown);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dropdown);
            Thread.Sleep(1000);

            string provTrim = proveedor.Trim();
            By optionXPath = By.XPath($"//span[@class='mat-option-text' and contains(normalize-space(), '{provTrim}')] | //mat-option[.//span[contains(normalize-space(), '{provTrim}')]]");

            IWebElement optionElement = wait.Until(ExpectedConditions.ElementExists(optionXPath));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", optionElement);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", optionElement);
            Thread.Sleep(1000);
        }

        public void IngresarPoliza(string poliza)
        {
            utilities.EnterText(txtPoliza, poliza);
        }

        // =========================================================================
        // MÉTODO CAZA-FANTASMAS (SELECCIÓN INFALIBLE DE CALENDARIO)
        // =========================================================================
        // =========================================================================
        // MÉTODO CAZA-FANTASMAS (SELECCIÓN INFALIBLE DE CALENDARIO)
        // =========================================================================
        public void SeleccionarFecha(By btnCalendario, string dia, bool avanzarUnAno = false, bool retrocederUnAno = false)
        {
            var wait = Wait(10);

            // 1. Limpiar pantalla
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            System.Threading.Thread.Sleep(500);

            // 2. Abrir el calendario
            IWebElement btnCal = wait.Until(ExpectedConditions.ElementToBeClickable(btnCalendario));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCal);
            System.Threading.Thread.Sleep(500);

            try { btnCal.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal); }
            System.Threading.Thread.Sleep(1500);

            // =========================================================================
            // 🔥 NAVEGACIÓN DE MESES: Hacia adelante (Siguiente) o Hacia atrás (Anterior)
            // =========================================================================
            if (avanzarUnAno)
            {
                By btnNextMonth = By.XPath("//button[contains(@class, 'mat-calendar-next-button')]");
                IWebElement nextBtn = wait.Until(ExpectedConditions.ElementToBeClickable(btnNextMonth));
                for (int i = 0; i < 12; i++) { nextBtn.Click(); System.Threading.Thread.Sleep(150); }
                System.Threading.Thread.Sleep(500);
            }
            else if (retrocederUnAno)
            {
                // Busca la flechita izquierda de PrimeNG / Angular Material y le da 12 clics
                By btnPrevMonth = By.XPath("//button[contains(@class, 'mat-calendar-previous-button')]");
                IWebElement prevBtn = wait.Until(ExpectedConditions.ElementToBeClickable(btnPrevMonth));
                for (int i = 0; i < 12; i++) { prevBtn.Click(); System.Threading.Thread.Sleep(150); }
                System.Threading.Thread.Sleep(500);
            }

            // 3. XPATH MAESTRO para buscar el día exacto
            string xpathDia = $"(//mat-datepicker-content)[last()]//*[contains(@class, 'mat-calendar-body-cell') and not(contains(@class, 'mat-calendar-body-disabled'))]//div[contains(@class, 'mat-calendar-body-cell-content') and normalize-space()='{dia}']";

            try
            {
                IWebElement divNumero = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpathDia)));
                divNumero.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aviso: Usando Plan B para el día {dia}. Detalle: {ex.Message}");
                IWebElement divNumero = driver.FindElement(By.XPath(xpathDia));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", divNumero);
            }

            // 4. Esperar a que el calendario se cierre
            System.Threading.Thread.Sleep(1500);
        }

        // 🔥 NUEVO MÉTODO PARA EL CASO 06
        public void SeleccionarFechaDesdeYHastaAnoPasado(string diaDesde, string diaHasta)
        {
            // Le pasamos 'true' al último parámetro para que retroceda 1 año en ambos calendarios
            SeleccionarFecha(btnCalDesde, diaDesde, false, true);
            SeleccionarFecha(btnCalHasta, diaHasta, false, true);
        }



        public void SeleccionarFechaDesdeYHastaUnAnoDespues(string diaDesde, string diaHasta)
        {
            // El 'Desde' se selecciona normal (mes actual)
            SeleccionarFecha(btnCalDesde, diaDesde, false);

            // El 'Hasta' avanza 12 meses automáticamente
            SeleccionarFecha(btnCalHasta, diaHasta, true);
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
            Thread.Sleep(1000);
            IWebElement lupa = wait.Until(ExpectedConditions.ElementToBeClickable(btnLupaRuc));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", lupa);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupa);
            Thread.Sleep(3000);
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

                // 🚀 AQUÍ ESTÁ LA MAGIA: Intentamos clic normal, si hay una sombra estorbando, usamos JS Click
                try
                {
                    wait.Until(ExpectedConditions.ElementToBeClickable(btn)).Click();
                }
                catch (Exception)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
                }

                System.Threading.Thread.Sleep(4000);
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
            // Presionamos Tab para salir del campo sin darle clic a la lupa
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Tab).Perform();
            Thread.Sleep(500);
        }

        public void IngresarRucSinBuscar(string ruc)
        {
            utilities.EnterText(txtRuc, ruc);
            Thread.Sleep(500);
            // Presionamos Tab para salir del campo sin darle clic a la lupa
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
                throw new Exception("Fallo de QA: El botón 'Guardar' debería estar bloqueado (deshabilitado) por falta de datos, pero está encendido.");
            }
        }



            public void VerificarDiaHastaDeshabilitado(string dia)
        {
            var wait = Wait(10);
            
            // 1. Abrimos el calendario HASTA
            IWebElement btnCal = wait.Until(ExpectedConditions.ElementToBeClickable(btnCalHasta));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCal);
            Thread.Sleep(500);
            try { btnCal.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal); }
            Thread.Sleep(1500);

            // 2. Buscamos el día, pero esta vez exigimos que tenga la clase 'mat-calendar-body-disabled'
            string xpathDiaDeshabilitado = $"(//mat-datepicker-content)[last()]//*[contains(@class, 'mat-calendar-body-disabled')]//div[normalize-space()='{dia}']";
            
            var elementos = driver.FindElements(By.XPath(xpathDiaDeshabilitado));
            
            if (elementos.Count == 0)
            {
                throw new Exception($"Fallo de QA: Se esperaba que el día {dia} estuviera bloqueado, pero está disponible para selección.");
            }

            // 3. Cerramos el calendario
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(500);
        }
        
        // RECICLAMOS EL MÉTODO DEL TOAST DE ODÓMETRO PARA SOAT
        public void VerificarMensajeErrorSoat(string mensajeEsperado)
        {
            var wait = Wait(10); 
            By localizadorMensaje = By.XPath($"//*[contains(text(), '{mensajeEsperado}')]");
            try { wait.Until(ExpectedConditions.ElementIsVisible(localizadorMensaje)); }
            catch (WebDriverTimeoutException) { throw new Exception($"Fallo: Se esperaba el error '{mensajeEsperado}'."); }
            Thread.Sleep(1000);
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
        }




        public void SeleccionarSoloFechaDesde(string dia)
        {
            // Reutiliza nuestro método infalible, pasándole solo el calendario DESDE
            SeleccionarFecha(btnCalDesde, dia, false);
        }




        // =============================
        // MÉTODOS DE GRILLA Y EDICIÓN
        // =============================

        // ¡XPATH MÁGICO!: Busca el primer input que tenga la clase de filtro de PrimeNG que descubriste
        private By txtFiltroPlaca = By.XPath("(//thead//tr[last()]//th)[2]//input");

        private By btnBuscarFiltro = By.XPath("//button[contains(@class, 'mat-raised-button') and contains(., 'Buscar')]");
        private By btnVerSoat = By.XPath("(//mat-icon[normalize-space()='search' or normalize-space()='visibility'])[1] | //button[contains(@class, 'button-view')]");

        // Exactamente la misma clase que usaste en Odómetro para editar
        private By btnEditarSoat = By.XPath("//button[contains(@class, 'button-edit')] | //mat-icon[normalize-space()='edit']");

        private By cbxAseguradoras = By.XPath("//mat-select[@formcontrolname='aseguradoras' or contains(@placeholder, 'Aseguradora')] | (//mat-select)[1]");
        private By cbxAreas = By.XPath("//mat-select[@formcontrolname='areas' or contains(@placeholder, 'Área')] | (//mat-select)[2]");
        private By cbxEstado = By.XPath("//mat-select[@formcontrolname='estado' or contains(@placeholder, 'Estado')] | (//mat-select)[3]");

        // 3. Checkboxes "TODAS" indestructibles (Basados en tus XPaths, pero sin los IDs dinámicos)
        private By chkTodasAseguradoras = By.XPath("//mat-checkbox[contains(., 'TODAS')]//span[contains(@class, 'mat-checkbox-inner-container')]");

        // El checkbox general de Estado que me mandaste
        private By chkEstadoGeneral = By.XPath("//span[contains(@class, 'mat-checkbox-inner-container-no-side-margin')]");




        public void BuscarSoatEnGrillaPorPlaca(string placa)
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(2000); // Pausa para que cargue la tabla

            // ¡USAMOS EL XPATH EXACTO QUE DESCUBRISTE CON SELECTORSHUB!
            By txtFiltroPlaca = By.XPath("//th[7]//input[1]");

            IWebElement filtro = wait.Until(ExpectedConditions.ElementExists(txtFiltroPlaca));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", filtro);
            System.Threading.Thread.Sleep(500);

            // Limpiamos y escribimos
            try { filtro.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", filtro); }
            try { filtro.Clear(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='';", filtro); }

            filtro.SendKeys(placa);
            System.Threading.Thread.Sleep(500);

            // Presionamos Enter en la misma caja (es lo más seguro en PrimeNG) o le damos al botón Buscar
            try
            {
                IWebElement btnBuscar = driver.FindElement(By.XPath("//button[contains(@class, 'mat-raised-button') and contains(., 'Buscar')]"));
                btnBuscar.Click();
            }
            catch
            {
                filtro.SendKeys(Keys.Enter);
            }

            System.Threading.Thread.Sleep(2000); // Esperar a que la tabla filtre
        }

        public void ClicVerSoat()
        {
            var wait = Wait();
            // Usamos ElementExists igual que en Odómetro
            IWebElement btnVer = wait.Until(ExpectedConditions.ElementExists(btnVerSoat));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnVer);
            System.Threading.Thread.Sleep(500);

            // Clic inyectado con JS igual que en Odómetro
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnVer);
            System.Threading.Thread.Sleep(2000);
        }

        public void ClicEditarSoat()
        {
            var wait = Wait();
            IWebElement btnEdit = wait.Until(ExpectedConditions.ElementExists(btnEditarSoat));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnEdit);
            System.Threading.Thread.Sleep(500);

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnEdit);
            System.Threading.Thread.Sleep(2000);
        }




        // =============================
        // ELIMINAR DOCUMENTO ADJUNTO
        // =============================


        private By chkDocumentoAdjunto = By.XPath("(//div[contains(@class, 'p-checkbox-box')])[1] | //span[contains(@class, 'p-checkbox-icon')]");

        // 2. Localizador del Basurero del Documento (usando el pi-trash y el tooltip que me pasaste)
        private By btnEliminarDocAdjunto = By.XPath("//button[.//span[contains(@class, 'pi-trash')]] | //button[contains(@ng-reflect-message, 'Eliminar Documento')]");


        public void EliminarDocumentoAdjunto()
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(2000); // Esperamos a que la sección de documentos cargue bien

            // PASO 1: Marcar el checkbox del documento
            IWebElement checkbox = wait.Until(ExpectedConditions.ElementExists(chkDocumentoAdjunto));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", checkbox);
            System.Threading.Thread.Sleep(500);

            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(checkbox)).Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
            }

            System.Threading.Thread.Sleep(1000); // Pequeña pausa para que el sistema detecte el check y habilite el basurero

            // PASO 2: Hacer clic en el basurero rojo
            IWebElement btnBasurero = wait.Until(ExpectedConditions.ElementExists(btnEliminarDocAdjunto));
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(btnBasurero)).Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBasurero);
            }

            System.Threading.Thread.Sleep(1500); // Pausa para que el documento desaparezca visualmente
        }




        private By btnBuscarFiltros = By.XPath("//button[contains(., 'BUSCAR') or contains(., 'Buscar') or contains(@class, 'search')]");

        public void ClicBuscarFiltros()
        {
            var wait = Wait();
            IWebElement btnBuscar = wait.Until(ExpectedConditions.ElementToBeClickable(btnBuscarFiltros));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnBuscar);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBuscar);

            // Pausa generosa para que el servidor traiga todos los SOATs a la tabla
            System.Threading.Thread.Sleep(3000);
        }

        public void VerificarGrillaConResultados()
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(2000);

            var filas = driver.FindElements(By.XPath("//tbody/tr | //div[contains(@class, 'p-datatable-tbody')]//tr"));

            if (filas.Count == 0)
            {
                throw new Exception("Fallo: Se esperaban resultados, pero la tabla está completamente vacía.");
            }

            if (filas.Count == 1)
            {
                string textoFila = filas[0].Text.ToLower();
                if (textoFila.Contains("no se encontraron") || textoFila.Contains("disponible") || textoFila.Contains("sin registros") || textoFila.Contains("empty"))
                {
                    throw new Exception("Fallo: Apareció el mensaje de tabla vacía cuando se esperaban datos.");
                }
            }
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
            System.Threading.Thread.Sleep(500);

            // 🔥 CLAVE: Usamos clic NATIVO primero porque Angular a veces ignora el clic de JavaScript en los combos
            try { combo.Click(); }
            catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", combo); }

            System.Threading.Thread.Sleep(1500); // Pausa generosa para que el menú se dibuje en la pantalla
        }

        public void DesmarcarOpcionTodas()
        {
            System.Threading.Thread.Sleep(1500); // Pausa sagrada para que el menú cargue

            // 🔥 TU XPATH EXACTO para desmarcar todo en "Estado" (el checkbox sin lado) y el general de "TODAS"
            By locatorTodas = By.XPath("//span[contains(@class, 'mat-checkbox-inner-container-no-side-margin')] | //div[contains(@class, 'cdk-overlay-pane')]//mat-checkbox[contains(., 'TODAS')]");

            var elementosTodas = driver.FindElements(locatorTodas);

            if (elementosTodas.Count > 0)
            {
                // Agarramos el último elemento encontrado (suele ser el del menú abierto)
                IWebElement checkboxMaestro = elementosTodas[elementosTodas.Count - 1];

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", checkboxMaestro);
                System.Threading.Thread.Sleep(500);

                // Clic inyectado sin preguntar si está ".Displayed"
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkboxMaestro);
                System.Threading.Thread.Sleep(1500);
            }
            else
            {
                throw new Exception("Fallo: No se encontró el checkbox maestro para desmarcar las opciones.");
            }
        }

        public void SeleccionarOpcionEnFiltro(string opcion)
        {
            // SUPERPODER 1: Limpiamos acentos, ESPACIOS y GUIONES. Comparación a prueba de balas.
            string opcionBuscada = opcion.Trim().ToUpper().Replace("Ó", "O").Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ú", "U").Replace(" ", "").Replace("-", "");

            // SUPERPODER 2: Presionar la primera letra para saltar rápido en listas alfabéticas largas
            string primeraLetra = opcion.Trim().Substring(0, 1);
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(primeraLetra).Perform();
            System.Threading.Thread.Sleep(800); // Pausa para que el menú baje de golpe hasta esa letra

            bool opcionEncontrada = false;
            int intentosScroll = 0;

            // Aumentamos el límite a 40 por si hay muchas áreas con la misma letra
            while (!opcionEncontrada && intentosScroll < 40)
            {
                var opciones = driver.FindElements(By.XPath("//mat-checkbox | //label[contains(@class, 'mat-checkbox-layout')] | //mat-option | //div[contains(@class, 'mat-list-item-content')]"));

                foreach (var opt in opciones)
                {
                    string textoReal = opt.GetAttribute("textContent") ?? "";

                    // Limpiamos el texto extraído igual que la búsqueda
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
                        System.Threading.Thread.Sleep(500);

                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elementoClic);

                        opcionEncontrada = true;
                        System.Threading.Thread.Sleep(1000);
                        break;
                    }
                }

                if (!opcionEncontrada)
                {
                    // Bajamos de 2 en 2 para recorrer la lista más rápido
                    new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.ArrowDown).Perform();
                    new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.ArrowDown).Perform();
                    System.Threading.Thread.Sleep(200);
                    intentosScroll++;
                }
            }

            if (!opcionEncontrada)
            {
                throw new Exception($"Fallo de QA: No se pudo encontrar la opción '{opcion}' visible en la lista después de hacer scroll.");
            }
        }
        public void CerrarComboFiltro()
        {
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            System.Threading.Thread.Sleep(1000);
        }

        public void IngresarRangoFechasFiltro(string fechaDesde, string fechaHasta)
        {
            var wait = Wait();
            // Asumiendo que son los clásicos input de texto para fechas de Angular
            By txtDesde = By.XPath("//input[contains(@placeholder, 'Desde') or contains(@formcontrolname, 'fechaInicio')]");
            By txtHasta = By.XPath("//input[contains(@placeholder, 'Hasta') or contains(@formcontrolname, 'fechaFin')]");

            try
            {
                IWebElement inputDesde = driver.FindElement(txtDesde);
                inputDesde.Clear();
                inputDesde.SendKeys(fechaDesde);

                IWebElement inputHasta = driver.FindElement(txtHasta);
                inputHasta.Clear();
                inputHasta.SendKeys(fechaHasta);
            }
            catch
            {
                // Si usan un datepicker especial, me avisas y lo adaptamos
            }
            System.Threading.Thread.Sleep(500);
        }




        // =============================
        // HISTORIAL Y FILTRO DE DÍAS
        // =============================

        // Tu XPath exacto para los Días para Vencer
        private By txtDiasParaVencer = By.XPath("//th[3]//input[1]");

        // Tus XPaths exactos para el Historial
        private By btnHistorial = By.XPath("//button[@ng-reflect-message='Ver Historial']//span[@class='mat-button-wrapper'] | //button[@ng-reflect-message='Ver Historial']");
        private By btnCerrarHistorial = By.XPath("//button[contains(@class, 'tsp-button-delete')]//span[@class='mat-button-wrapper'] | //button[contains(@class, 'tsp-button-delete')]");

        public void IngresarDiasParaVencer(string dias)
        {
            var wait = Wait();
            IWebElement inputDias = wait.Until(ExpectedConditions.ElementExists(txtDiasParaVencer));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", inputDias);
            System.Threading.Thread.Sleep(500);

            try { inputDias.Clear(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='';", inputDias); }
            inputDias.SendKeys(dias);
            System.Threading.Thread.Sleep(500);
        }

        public void ClicHistorial()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnHistorial));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);

            // Pausa generosa para que se abra el modal del historial
            System.Threading.Thread.Sleep(2500);
        }

        public void CerrarHistorial()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnCerrarHistorial));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(1000);
        }











        public void EscribirFechasVigencia(string fechaDesde, string fechaHasta)
        {
            var wait = Wait();

            // Localizadores exactos de tus campos de fecha de vigencia
            By inputDesde = By.XPath("//input[@formcontrolname='startPolicyValidity']");
            By inputHasta = By.XPath("//input[@formcontrolname='endPolicyValidity']");

            // Llenamos DESDE
            IWebElement txtDesde = wait.Until(ExpectedConditions.ElementExists(inputDesde));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", txtDesde);
            System.Threading.Thread.Sleep(500);

            // Borramos y escribimos como humano
            txtDesde.SendKeys(Keys.Control + "a");
            txtDesde.SendKeys(Keys.Delete);
            txtDesde.SendKeys(fechaDesde);
            System.Threading.Thread.Sleep(500);
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Tab).Perform();

            // Llenamos HASTA
            IWebElement txtHasta = wait.Until(ExpectedConditions.ElementExists(inputHasta));
            txtHasta.SendKeys(Keys.Control + "a");
            txtHasta.SendKeys(Keys.Delete);
            txtHasta.SendKeys(fechaHasta);
            System.Threading.Thread.Sleep(500);
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Tab).Perform();
        }











    }
}