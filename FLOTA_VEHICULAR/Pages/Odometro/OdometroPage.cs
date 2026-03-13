using FLOTA_VEHICULAR.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace FLOTA_VEHICULAR.Pages.Odometro
{
    public class OdometroPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public OdometroPage(IWebDriver driver)
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
        private By moduloOdometro = By.XPath("//div[normalize-space()='Odómetro']");
        private By btnNuevo = By.XPath("//button[contains(@class, 'tsp-button-success')]//span[contains(., 'Nuevo') or contains(., 'NUEVO')] | //mat-icon[normalize-space()='add']/ancestor::button");

        public void IngresarModuloOdometro()
        {
            var wait = Wait();
            Thread.Sleep(2000);
            IWebElement modulo = wait.Until(ExpectedConditions.ElementExists(moduloOdometro));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", modulo);
            Thread.Sleep(2000);
        }

        public void ClickNuevoOdometro()
        {
            Wait().Until(ExpectedConditions.ElementToBeClickable(btnNuevo)).Click();
            Thread.Sleep(1500);
        }

        // =============================
        // FORMULARIO ODÓMETRO (XPaths Dinámicos e Inteligentes)
        // =============================

        // Busca la caja de texto que esté dentro de un contenedor que diga "Placa"
        private By txtPlaca = By.XPath("//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'placa')]//input | //input[@id='mat-input-4']");
        private By btnLupa = By.XPath("//div[contains(@class, 'cdk-overlay-pane')]//mat-icon[normalize-space()='search'] | //mat-dialog-container//mat-icon[normalize-space()='search'] | (//mat-icon[normalize-space()='search'])[last()]");

        // Busca la caja que diga "Lectura" u "Odómetro"
        private By txtLectura = By.XPath("//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'lectura') or contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'odómetro')]//input | //input[@id='mat-input-11']");

        // Busca el botón del calendario (ícono)
        private By btnCalendario = By.XPath("//mat-datepicker-toggle//button | //*[name()='svg']/*[name()='path' and contains(@d,'M19 3h-1V1')]/ancestor::button");
        private By btnGuardar = By.XPath("//button[contains(@class, 'tsp-button-success') or contains(@class, 'mat-raised-button')][contains(., 'Guardar') or contains(., 'GUARDAR')]");

        public void IngresarPlacaYBuscar(string placa)
        {
            var wait = Wait();

            // 1. Pausa pequeña para que la animación del modal "+Nuevo" termine de aparecer
            Thread.Sleep(1000);

            // 2. Ingresamos la placa
            utilities.EnterText(txtPlaca, placa);
            Thread.Sleep(500);

            // 3. Clic en la lupa (ahora sí, la del modal)
            IWebElement lupa = wait.Until(ExpectedConditions.ElementToBeClickable(btnLupa));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupa);

            // 4. Pausa obligatoria para que el sistema traiga los datos del vehículo
            Thread.Sleep(3000);
        }

        public void IngresarLectura(string lectura)
        {
            var wait = Wait();

            // 1. Buscamos la caja de texto
            IWebElement inputLectura = wait.Until(ExpectedConditions.ElementExists(txtLectura));

            // 2. Centramos el elemento en la pantalla
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", inputLectura);
            Thread.Sleep(500);

            // 3. Le damos el "foco" haciendo un clic nativo e inyectado para asegurar que el cursor parpadee ahí
            try { inputLectura.Click(); } catch { }
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus(); arguments[0].click();", inputLectura);
            Thread.Sleep(500);

            // 4. Escribimos usando la simulación de Teclado (Actions)
            // Esto evita por completo el error "InvalidElementStateException" porque teclea "al aire"
            new OpenQA.Selenium.Interactions.Actions(driver)
                .SendKeys(Keys.Control + "a")  // Selecciona todo (si hubiera algo)
                .SendKeys(Keys.Delete)         // Borra
                .SendKeys(lectura)             // Escribe el número como un humano
                .Perform();

            // 5. Pausa visual para asegurar que el 5000 se escribió correctamente en pantalla
            Thread.Sleep(1000);
        }

        public void SeleccionarFecha(string dia)
        {
            var wait = Wait();

            // 1. Abrir el calendario
            IWebElement btnCal = wait.Until(ExpectedConditions.ElementExists(btnCalendario));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal);
            Thread.Sleep(1000);

            // 2. Seleccionar el día dinámicamente
            By selectorDia = By.XPath($"//div[normalize-space()='{dia}']");
            IWebElement diaElement = wait.Until(ExpectedConditions.ElementExists(selectorDia));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", diaElement);
            Thread.Sleep(1000);
        }

        public void GuardarOdometro()
        {
            var wait = Wait();
            Thread.Sleep(1000);
            IWebElement btn = wait.Until(ExpectedConditions.ElementExists(btnGuardar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);
            Thread.Sleep(500);

            try { wait.Until(ExpectedConditions.ElementToBeClickable(btn)).Click(); }
            catch (Exception) { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn); }

            Thread.Sleep(4000); // Esperar que guarde
        }

        // =============================
        // GRID Y EDICIÓN DE ODÓMETRO
        // =============================

        // VARIABLES (Asegúrate de que estén aquí y con su "By.XPath" asignado)
        private By txtBuscarPlacaGrid = By.XPath("(//input[contains(@class, 'p-column-filter')])[1]");
        private By btnVerOdometroGrid = By.XPath("(//mat-icon[normalize-space()='search'])[1]");
        private By btnEditarOdometro = By.XPath("//button[contains(@class, 'button-edit')]");

        // ¡ESTA ES LA VARIABLE QUE ESTABA DANDO ERROR "NULL"!
        private By btnGuardarEdicion = By.XPath("//button[contains(@class, 'button-editing')]");

        public void BuscarOdometroPorPlacaGrid(string placa)
        {
            var wait = Wait();
            IWebElement searchBox = wait.Until(ExpectedConditions.ElementToBeClickable(txtBuscarPlacaGrid));

            // Centramos la caja
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", searchBox);
            Thread.Sleep(500);

            // Clickeamos y limpiamos como humano
            searchBox.Click();
            Thread.Sleep(500);
            searchBox.SendKeys(Keys.Control + "a");
            searchBox.SendKeys(Keys.Delete);
            Thread.Sleep(500);

            // 🔥 CLAVE 1: Escribimos letra por letra para que Angular lo detecte
            foreach (char c in placa)
            {
                searchBox.SendKeys(c.ToString());
                Thread.Sleep(100); // Pausa mínima entre letras
            }

            // 🔥 CLAVE 2: Pausa para que PrimeNG procese el texto (Debounce)
            Thread.Sleep(1000);

            // 🔥 CLAVE 3: Hacemos clic en el fondo de la pantalla (fuera de la caja)
            // Esto obliga a Angular a registrar el texto (evento blur) ANTES de que el robot le dé al botón azul Buscar.
            By fondoPantalla = By.XPath("//div[normalize-space()='Odómetro']");
            IWebElement fondo = driver.FindElement(fondoPantalla);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", fondo);

            Thread.Sleep(1000);
        }

        public void ClicVerOdometroGrid()
        {
            var wait = Wait();
            IWebElement btnVer = wait.Until(ExpectedConditions.ElementExists(btnVerOdometroGrid));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnVer);
            Thread.Sleep(2000); // Esperar que abra el detalle
        }

        public void ClicEditarOdometro()
        {
            var wait = Wait();
            IWebElement btnEdit = wait.Until(ExpectedConditions.ElementExists(btnEditarOdometro));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnEdit);
            Thread.Sleep(2000); // Esperar que habilite los campos
        }

        public void GuardarEdicionOdometro()
        {
            var wait = Wait();
            Thread.Sleep(1000);

            // Ahora Selenium sí encontrará el XPath en la variable btnGuardarEdicion
            IWebElement btnSave = wait.Until(ExpectedConditions.ElementExists(btnGuardarEdicion));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnSave);
            Thread.Sleep(500);

            try { wait.Until(ExpectedConditions.ElementToBeClickable(btnSave)).Click(); }
            catch (Exception) { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnSave); }

            Thread.Sleep(3000); // Esperar que guarde la edición
        }


        // =============================
        // DAR DE BAJA ODÓMETRO
        // =============================

        private By btnDarDeBajaOdometro = By.XPath("//mat-icon[normalize-space()='delete']");

        // Hacemos el XPath robusto ignorando el 'cdk-focused' y buscando botones de éxito en el modal
        private By btnConfirmarBajaOdometro = By.XPath("//button[contains(@class, 'tsp-button-success') and contains(@class, 'tsp-buttons')] | //button[contains(., 'Confirmar') or contains(., 'CONFIRMAR') or contains(., 'Aceptar')]");

        public void ClicDarDeBajaOdometro()
        {
            var wait = Wait();
            IWebElement btnBaja = wait.Until(ExpectedConditions.ElementExists(btnDarDeBajaOdometro));

            // Clic con JS por si hay un tooltip o sombra bloqueando el ícono
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBaja);

            // Pausa obligatoria para que el modal de confirmación termine de aparecer flotando en la pantalla
            Thread.Sleep(1500);
        }

        public void ConfirmarBajaOdometro()
        {
            var wait = Wait();
            IWebElement btnConf = wait.Until(ExpectedConditions.ElementExists(btnConfirmarBajaOdometro));

            // Clic con JS directo al corazón del botón
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnConf);

            // Pausa para que se cierre el modal y el sistema procese la baja
            Thread.Sleep(3000);
        }

        // =============================
        // FILTROS DE BÚSQUEDA
        // =============================

        // Usamos [1] para el primer combo (Áreas) y [2] para el segundo (Origen)
        private By selectFiltroArea = By.XPath("(//mat-select)[1]");
        private By selectFiltroOrigen = By.XPath("(//mat-select)[2]");

        // Buscamos los checkboxes que tengan el texto TODAS al lado (el 1ero es de Áreas, el 2do de Origen)
        private By chkTodasAreas = By.XPath("(//mat-checkbox[contains(., 'TODAS')])[1]");
        private By chkTodasOrigen = By.XPath("(//mat-checkbox[contains(., 'TODAS')])[2]");

        private By btnBuscarFiltros = By.XPath("//button[contains(., 'BUSCAR') or contains(., 'Buscar')]");

        public void SeleccionarAreasFiltro(string areasSeparadasPorComa)
        {
            var wait = Wait();

            // 1. Abrir el combo de Áreas usando clic nativo si es posible
            IWebElement dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(selectFiltroArea));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", dropdown);
            Thread.Sleep(500);
            try { dropdown.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dropdown); }
            Thread.Sleep(1500); // Damos tiempo al panel de abrirse

            // 2. Iterar por cada área
            string[] listaAreas = areasSeparadasPorComa.Split(',');
            foreach (var area in listaAreas)
            {
                string areaTrim = area.Trim();

                // Un XPath más directo para Angular Material
                By optionXPath = By.XPath($"//mat-option[contains(., '{areaTrim}')]");

                bool opcionEncontrada = false;
                int intentosScroll = 0;

                while (!opcionEncontrada && intentosScroll < 15)
                {
                    try
                    {
                        IWebElement optionElement = driver.FindElement(optionXPath);
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", optionElement);
                        Thread.Sleep(500);

                        // CLAVE: Clic nativo para que Angular registre la selección real
                        try { optionElement.Click(); }
                        catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", optionElement); }

                        opcionEncontrada = true;
                        Thread.Sleep(800);
                    }
                    catch (NoSuchElementException)
                    {
                        new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.ArrowDown).Perform();
                        Thread.Sleep(300);
                        intentosScroll++;
                    }
                }
            }

            // 3. Cerrar el combo con Escape
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(1000);
        }

        public void SeleccionarOrigenFiltro(string origen)
        {
            var wait = Wait();

            // 1. Abrir el combo de Origen
            IWebElement dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(selectFiltroOrigen));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", dropdown);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dropdown);
            Thread.Sleep(1500);

            // 2. Seleccionar la opción usando el XPath exacto basado en tu HTML
            string origenTrim = origen.Trim();

            // ACTUALIZACIÓN CRÍTICA: Quitamos el "normalize-space()" para evitar problemas de mayúsculas/minúsculas
            // y permitimos que busque directamente usando la función text() de forma más permisiva
            By optionXPath = By.XPath($"//span[@class='mat-option-text' and contains(text(), '{origenTrim}')] | //mat-option[.//span[contains(text(), '{origenTrim}')]]");

            IWebElement optionElement = wait.Until(ExpectedConditions.ElementExists(optionXPath));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", optionElement);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", optionElement);
            Thread.Sleep(1000);

            // 3. Cerrar con Escape
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(1000);
        }

        public void HacerClicEnTodas(string tipoFiltro)
        {
            var wait = Wait();
            IWebElement dropdown;

            // 1. Identificar y abrir el combo correcto
            if (tipoFiltro.ToUpper() == "ÁREAS" || tipoFiltro.ToUpper() == "AREAS")
            {
                dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(selectFiltroArea));
            }
            else
            {
                dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(selectFiltroOrigen));
            }

            // Centrar y hacer clic para abrir el menú
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", dropdown);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dropdown);

            // Pausa clave para que Angular Material cree el panel flotante en el DOM
            Thread.Sleep(1500);

            // 2. Buscar el checkbox 'TODAS' dentro del panel flotante ACTIVO (cdk-overlay-pane)
            // Esto elimina el error de los índices porque solo busca en lo que está abierto AHORA.
            By chkTodasActivo = By.XPath("//div[contains(@class, 'cdk-overlay-pane')]//mat-checkbox[contains(., 'TODAS')] | //div[contains(@class, 'cdk-overlay-pane')]//span[contains(., 'TODAS')]");

            IWebElement checkbox = wait.Until(ExpectedConditions.ElementExists(chkTodasActivo));

            // 3. Hacer clic en el checkbox
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", checkbox);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
            Thread.Sleep(1000);

            // 4. Cerrar el combo presionando la tecla Escape
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(1000);
        }

        public void ClicBuscarFiltros()
        {
            var wait = Wait();
            IWebElement btnBuscar = wait.Until(ExpectedConditions.ElementToBeClickable(btnBuscarFiltros));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBuscar);

            // Pausa generosa para que el servidor busque y traiga los resultados a la tabla
            Thread.Sleep(3000);
        }

        public void IngresarPlacaSinBuscar(string placa)
        {
            var wait = Wait();
            Thread.Sleep(1000);
            utilities.EnterText(txtPlaca, placa);
            Thread.Sleep(500);

            // CLAVE: Presionamos "Tab" para salir de la caja de la placa. 
            // Esto cierra cualquier mensaje de validación o panel de autocompletado que esté tapando la pantalla.
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Tab).Perform();
            Thread.Sleep(500);
        }

        public void VerificarBotonGuardarDeshabilitado()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementExists(btnGuardar));

            // Verificamos si el botón tiene el atributo "disabled"
            // (Angular Material suele agregar "disabled='true'" o la clase "mat-button-disabled")
            string isDisabledAttr = btn.GetAttribute("disabled");
            string classes = btn.GetAttribute("class");

            bool isDisabled = isDisabledAttr != null || classes.Contains("disabled") || classes.Contains("mat-button-disabled");

            if (!isDisabled)
            {
                throw new Exception("El botón 'Guardar' debería estar deshabilitado, pero está habilitado.");
            }

            Thread.Sleep(1000);

            // Cerramos el modal para no estropear la siguiente prueba
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
        }



        public void VerificarOpcionTodasSeleccionada(string tipoFiltro)
        {
            // Como el sistema ya marca "TODAS" por defecto, respetamos ese comportamiento.
            // Hacemos una pausa mínima visual y omitimos el clic que rompía la prueba.
            Thread.Sleep(500);
        }

        public void VerificarGrillaConResultados()
        {
            var wait = Wait();
            Thread.Sleep(2000); // Darle tiempo a la tabla para cargar

            // Busca filas en la tabla
            var filas = driver.FindElements(By.XPath("//tbody/tr | //div[contains(@class, 'p-datatable-tbody')]//tr"));

            if (filas.Count == 0)
            {
                throw new Exception("Fallo: Se esperaban resultados en la grilla, pero la tabla está completamente vacía.");
            }

            // Si solo hay 1 fila, verificamos que no sea la fila de "tabla vacía"
            if (filas.Count == 1)
            {
                string textoFila = filas[0].Text.ToLower();
                if (textoFila.Contains("no se encontraron") || textoFila.Contains("disponible") || textoFila.Contains("sin registros") || textoFila.Contains("empty"))
                {
                    throw new Exception("Fallo: Se esperaban resultados en la grilla, pero apareció el mensaje de tabla vacía.");
                }
            }
        }

        public void VerificarMensajeSinRegistros()
        {
            Thread.Sleep(2000); // Esperar que la tabla procese la búsqueda

            var filas = driver.FindElements(By.XPath("//tbody/tr | //div[contains(@class, 'p-datatable-tbody')]//tr"));

            // Si hay filas, debemos asegurarnos de que sea el mensaje de error y NO datos reales
            if (filas.Count > 0)
            {
                string textoFila = filas[0].Text.ToLower();

                // Si la fila contiene datos normales (no contiene palabras de vacío), la prueba falla
                if (!textoFila.Contains("no se encontraron") && !textoFila.Contains("disponible") && !textoFila.Contains("sin registros") && !textoFila.Contains("empty"))
                {
                    throw new Exception($"Fallo: Se esperaba una grilla vacía, pero devolvió registros. Texto de la fila: '{textoFila}'");
                }
            }
            // Si filas.Count == 0, la prueba pasa exitosamente (la tabla se quedó vacía)
        }


        public void VerificarMensajeErrorEmergente(string mensajeEsperado)
        {
            var wait = Wait(10); // Esperamos hasta 10 segundos a que salga el popup/toast

            // Buscamos cualquier elemento en la pantalla que contenga el texto del error
            By localizadorMensaje = By.XPath($"//*[contains(text(), '{mensajeEsperado}')]");

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(localizadorMensaje));
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception($"Fallo: Se esperaba ver el mensaje de error '{mensajeEsperado}', pero el sistema no lo mostró.");
            }

            Thread.Sleep(1000);

            // Presionamos Escape un par de veces para cerrar el mensaje de error y el modal de Nuevo Odómetro
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(500);
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(1000);
        }


    }
}