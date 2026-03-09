using FLOTA_VEHICULAR.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace FLOTA_VEHICULAR.Pages
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
            System.Threading.Thread.Sleep(2000);
            IWebElement modulo = wait.Until(ExpectedConditions.ElementExists(moduloOdometro));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", modulo);
            System.Threading.Thread.Sleep(2000);
        }

        public void ClickNuevoOdometro()
        {
            Wait().Until(ExpectedConditions.ElementToBeClickable(btnNuevo)).Click();
            System.Threading.Thread.Sleep(1500);
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
            System.Threading.Thread.Sleep(1000);

            // 2. Ingresamos la placa
            utilities.EnterText(txtPlaca, placa);
            System.Threading.Thread.Sleep(500);

            // 3. Clic en la lupa (ahora sí, la del modal)
            IWebElement lupa = wait.Until(ExpectedConditions.ElementToBeClickable(btnLupa));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupa);

            // 4. Pausa obligatoria para que el sistema traiga los datos del vehículo
            System.Threading.Thread.Sleep(3000);
        }

        public void IngresarLectura(string lectura)
        {
            utilities.EnterText(txtLectura, lectura);
        }

        public void SeleccionarFecha(string dia)
        {
            var wait = Wait();

            // 1. Abrir el calendario
            IWebElement btnCal = wait.Until(ExpectedConditions.ElementExists(btnCalendario));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal);
            System.Threading.Thread.Sleep(1000);

            // 2. Seleccionar el día dinámicamente
            By selectorDia = By.XPath($"//div[normalize-space()='{dia}']");
            IWebElement diaElement = wait.Until(ExpectedConditions.ElementExists(selectorDia));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", diaElement);
            System.Threading.Thread.Sleep(1000);
        }

        public void GuardarOdometro()
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(1000);
            IWebElement btn = wait.Until(ExpectedConditions.ElementExists(btnGuardar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);
            System.Threading.Thread.Sleep(500);

            try { wait.Until(ExpectedConditions.ElementToBeClickable(btn)).Click(); }
            catch (Exception) { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn); }

            System.Threading.Thread.Sleep(4000); // Esperar que guarde
        }

        // =============================
        // GRID Y EDICIÓN DE ODÓMETRO
        // =============================

        // VARIABLES (Asegúrate de que estén aquí y con su "By.XPath" asignado)
        private By txtBuscarPlacaGrid = By.XPath("//th[2]//input[1] | (//input[contains(@class, 'p-column-filter')])[2]");
        private By btnVerOdometroGrid = By.XPath("(//mat-icon[normalize-space()='search'])[1]");
        private By btnEditarOdometro = By.XPath("//button[contains(@class, 'button-edit')]");

        // ¡ESTA ES LA VARIABLE QUE ESTABA DANDO ERROR "NULL"!
        private By btnGuardarEdicion = By.XPath("//button[contains(@class, 'button-editing')]");

        public void BuscarOdometroPorPlacaGrid(string placa)
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(2000); // Esperar que la tabla cargue

            IWebElement searchBox = wait.Until(ExpectedConditions.ElementToBeClickable(txtBuscarPlacaGrid));
            searchBox.Click();
            searchBox.Clear();
            searchBox.SendKeys(placa);

            System.Threading.Thread.Sleep(2000); // Esperar que filtre automáticamente
        }

        public void ClicVerOdometroGrid()
        {
            var wait = Wait();
            IWebElement btnVer = wait.Until(ExpectedConditions.ElementExists(btnVerOdometroGrid));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnVer);
            System.Threading.Thread.Sleep(2000); // Esperar que abra el detalle
        }

        public void ClicEditarOdometro()
        {
            var wait = Wait();
            IWebElement btnEdit = wait.Until(ExpectedConditions.ElementExists(btnEditarOdometro));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnEdit);
            System.Threading.Thread.Sleep(2000); // Esperar que habilite los campos
        }

        public void GuardarEdicionOdometro()
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(1000);

            // Ahora Selenium sí encontrará el XPath en la variable btnGuardarEdicion
            IWebElement btnSave = wait.Until(ExpectedConditions.ElementExists(btnGuardarEdicion));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnSave);
            System.Threading.Thread.Sleep(500);

            try { wait.Until(ExpectedConditions.ElementToBeClickable(btnSave)).Click(); }
            catch (Exception) { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnSave); }

            System.Threading.Thread.Sleep(3000); // Esperar que guarde la edición
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
            System.Threading.Thread.Sleep(1500);
        }

        public void ConfirmarBajaOdometro()
        {
            var wait = Wait();
            IWebElement btnConf = wait.Until(ExpectedConditions.ElementExists(btnConfirmarBajaOdometro));

            // Clic con JS directo al corazón del botón
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnConf);

            // Pausa para que se cierre el modal y el sistema procese la baja
            System.Threading.Thread.Sleep(3000);
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
            // Usamos un wait corto internamente para el escáner
            var waitCorto = Wait(5);

            // 1. Abrir el combo de Áreas
            IWebElement dropdown = Wait().Until(ExpectedConditions.ElementToBeClickable(selectFiltroArea));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", dropdown);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dropdown);
            System.Threading.Thread.Sleep(1500); // Damos tiempo al panel de abrirse

            // 2. Iterar por cada área
            string[] listaAreas = areasSeparadasPorComa.Split(',');
            foreach (var area in listaAreas)
            {
                string areaTrim = area.Trim();

                // XPATH EXACTO: Usando la clase que me proporcionaste
                By optionXPath = By.XPath($"//span[@class='mat-option-text' and normalize-space()='{areaTrim}'] | //mat-option[.//span[normalize-space()='{areaTrim}']]");

                bool opcionEncontrada = false;
                int intentosScroll = 0;

                // TÁCTICA ESCÁNER: Buscamos, si no está, flecha abajo y repetimos
                while (!opcionEncontrada && intentosScroll < 15)
                {
                    try
                    {
                        // Intentamos encontrar el elemento
                        IWebElement optionElement = driver.FindElement(optionXPath);

                        // Si lo encuentra, lo centramos y hacemos clic
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", optionElement);
                        System.Threading.Thread.Sleep(500);
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", optionElement);

                        opcionEncontrada = true;
                        System.Threading.Thread.Sleep(800);
                    }
                    catch (NoSuchElementException)
                    {
                        // Si no lo encuentra, hacemos scroll hacia abajo obligando a Angular a cargarlo
                        new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(OpenQA.Selenium.Keys.ArrowDown).Perform();
                        new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(OpenQA.Selenium.Keys.ArrowDown).Perform();
                        new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(OpenQA.Selenium.Keys.ArrowDown).Perform();
                        System.Threading.Thread.Sleep(500);
                        intentosScroll++;
                    }
                }
            }

            // 3. Cerrar el combo con Escape
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(OpenQA.Selenium.Keys.Escape).Perform();
            System.Threading.Thread.Sleep(1000);
        }

        public void SeleccionarOrigenFiltro(string origen)
        {
            var wait = Wait();

            // 1. Abrir el combo de Origen
            IWebElement dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(selectFiltroOrigen));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", dropdown);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dropdown);
            System.Threading.Thread.Sleep(1500);

            // 2. Seleccionar la opción usando el XPath exacto basado en tu HTML
            string origenTrim = origen.Trim();

            // ACTUALIZACIÓN CRÍTICA: Quitamos el "normalize-space()" para evitar problemas de mayúsculas/minúsculas
            // y permitimos que busque directamente usando la función text() de forma más permisiva
            By optionXPath = By.XPath($"//span[@class='mat-option-text' and contains(text(), '{origenTrim}')] | //mat-option[.//span[contains(text(), '{origenTrim}')]]");

            IWebElement optionElement = wait.Until(ExpectedConditions.ElementExists(optionXPath));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", optionElement);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", optionElement);
            System.Threading.Thread.Sleep(1000);

            // 3. Cerrar con Escape
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(OpenQA.Selenium.Keys.Escape).Perform();
            System.Threading.Thread.Sleep(1000);
        }

        public void HacerClicEnTodas(string tipoFiltro)
        {
            var wait = Wait();
            IWebElement checkbox;

            if (tipoFiltro.ToUpper() == "ÁREAS" || tipoFiltro.ToUpper() == "AREAS")
            {
                checkbox = wait.Until(ExpectedConditions.ElementExists(chkTodasAreas));
            }
            else
            {
                checkbox = wait.Until(ExpectedConditions.ElementExists(chkTodasOrigen));
            }

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
            System.Threading.Thread.Sleep(1000);
        }

        public void ClicBuscarFiltros()
        {
            var wait = Wait();
            IWebElement btnBuscar = wait.Until(ExpectedConditions.ElementToBeClickable(btnBuscarFiltros));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBuscar);

            // Pausa generosa para que el servidor busque y traiga los resultados a la tabla
            System.Threading.Thread.Sleep(3000);
        }
    }
}