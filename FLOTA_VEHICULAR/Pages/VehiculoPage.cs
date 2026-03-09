using FLOTA_VEHICULAR.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace FLOTA_VEHICULAR.Pages
{
    public class VehiculoPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public VehiculoPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        WebDriverWait Wait(int seconds = 20)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }

        // =============================
        // MODULO
        // =============================

        private By moduloVehiculo = By.XPath("//div[normalize-space()='Vehículo']");

        public void IngresarModuloVehiculo()
        {
            var wait = Wait();
            // Le damos 2 segundos para que el dashboard y el menú lateral terminen de cargar después del login
            System.Threading.Thread.Sleep(2000);

            IWebElement modulo = wait.Until(ExpectedConditions.ElementExists(moduloVehiculo));

            // Clic con JavaScript para que no importe si hay un banner o un spinner de carga estorbando
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", modulo);

            System.Threading.Thread.Sleep(2000); // Pausa para que la pantalla de vehículos cargue
        }

        // =============================
        // BOTON NUEVO
        // =============================

        private By btnNuevo = By.XPath("//mat-icon[normalize-space()='add']/ancestor::button");

        public void ClickNuevoVehiculo()
        {
            Wait().Until(ExpectedConditions.ElementToBeClickable(btnNuevo)).Click();

            Wait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[contains(@id,'mat-input')]")));
        }

        // =============================
        // INPUTS
        // =============================

        private By txtPlaca = By.XPath("(//input[contains(@id,'mat-input')])[1]");
        private By txtColor = By.XPath("//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), 'color')]//input | (//input[contains(@id,'mat-input')])[2]");
        private By txtConsumo = By.XPath("//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), 'consumo')]//input | (//input[contains(@id,'mat-input')])[3]");
        private By txtSerie = By.XPath("//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), 'serie')]//input | (//input[contains(@id,'mat-input')])[4]");

        // EXCEPCIÓN: El Motor usa su ID original (para Registrar) o busca la etiqueta "MOTOR" (para Editar) SIN usar índices que choquen.
        private By txtMotor = By.XPath("//input[@id='EngineNumber'] | //mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), 'motor')]//input");

        public void IngresarPlaca(string placa)
        {
            utilities.EnterText(txtPlaca, placa);
        }

        public void IngresarColor(string color)
        {
            utilities.EnterText(txtColor, color);
        }

        public void IngresarMotor(string motor)
        {
            utilities.EnterText(txtMotor, motor);
        }

        public void IngresarConsumo(string consumo)
        {
            utilities.EnterText(txtConsumo, consumo);
        }

        public void IngresarNumeroSerie(string serie)
        {
            utilities.EnterText(txtSerie, serie);
        }

        // =============================
        // SELECTORES
        // =============================

        private By selectArea = By.XPath("(//mat-select[not(@multiple)])[1]");
        private By selectPropietario = By.XPath("(//mat-select[not(@multiple)])[2]");
        private By selectMarca = By.XPath("(//mat-select[not(@multiple)])[3]");
        private By selectModelo = By.XPath("(//mat-select[not(@multiple)])[4]");
        private By selectAnio = By.XPath("(//mat-select[not(@multiple)])[5]");
        private By selectTipoVehiculo = By.XPath("(//mat-select[not(@multiple)])[6]");
        private By selectClasificador = By.XPath("(//mat-select[not(@multiple)])[7]");
        private By selectCombustible = By.XPath("(//mat-select[not(@multiple)])[8]");
        private By selectTipoMotor = By.XPath("(//mat-select[not(@multiple)])[9]");

        void SeleccionarOpcion(By selector, string opcion)
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(1000);

            // 1. Centramos el elemento visualmente
            IWebElement dropdown = wait.Until(ExpectedConditions.ElementExists(selector));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", dropdown);
            System.Threading.Thread.Sleep(500);

            // 2. TÁCTICA TECLADO: Evitamos el ratón por completo. 
            // Enviamos un "Enter" directo al elemento, lo que obliga a Angular a desplegar la lista sin importar dónde esté en la pantalla.
            try
            {
                dropdown.SendKeys(Keys.Enter);
            }
            catch (Exception)
            {
                // Si el Enter falla, respaldamos con JS
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dropdown);
            }

            System.Threading.Thread.Sleep(1500); // Tiempo para que caiga la lista

            By optionXPath = By.XPath($"//mat-option[contains(normalize-space(), '{opcion}')]");

            // 3. Paracaídas de emergencia: Si la lista sigue sin abrirse, damos un clic clásico
            if (driver.FindElements(optionXPath).Count == 0)
            {
                try { dropdown.Click(); } catch { }
                System.Threading.Thread.Sleep(1500);
            }

            // 4. Seleccionamos la opción con JS (ignora problemas visuales)
            IWebElement optionElement = wait.Until(ExpectedConditions.ElementExists(optionXPath));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", optionElement);

            System.Threading.Thread.Sleep(500);

            // 5. Cerramos la lista con Escape
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(OpenQA.Selenium.Keys.Escape).Perform();
            System.Threading.Thread.Sleep(800);
        }

        public void SeleccionarArea(string area)
        {
            SeleccionarOpcion(selectArea, area);
        }

        public void SeleccionarPropietario(string propietario)
        {
            SeleccionarOpcion(selectPropietario, propietario);
        }

        public void SeleccionarMarca(string marca)
        {
            SeleccionarOpcion(selectMarca, marca);
        }

        public void SeleccionarModelo(string modelo)
        {
            SeleccionarOpcion(selectModelo, modelo);
        }

        public void SeleccionarAnio(string anio)
        {
            SeleccionarOpcion(selectAnio, anio);
        }

        public void SeleccionarTipoVehiculo(string tipo)
        {
            SeleccionarOpcion(selectTipoVehiculo, tipo);
        }

        public void SeleccionarClasificador(string clasificador)
        {
            SeleccionarOpcion(selectClasificador, clasificador);
        }

        public void SeleccionarCombustible(string combustible)
        {
            SeleccionarOpcion(selectCombustible, combustible);
        }

        public void SeleccionarTipoMotor(string tipoMotor)
        {
            SeleccionarOpcion(selectTipoMotor, tipoMotor);
        }

        // =============================
        // GUARDAR
        // =============================

        // Buscamos cualquier botón que contenga la palabra "Guardar" y sea de tipo success/azul, ignorando clases extra
        private By btnGuardar = By.XPath("//button[contains(@class, 'tsp-button-success') or contains(@class, 'mat-raised-button')][contains(., 'Guardar') or contains(., 'GUARDAR')]");

        public void GuardarVehiculo()
        {
            var wait = Wait();

            // 1. PAUSA VITAL: Le damos tiempo a Angular para validar el último campo y habilitar el botón
            System.Threading.Thread.Sleep(2000);

            // 2. Esperamos a que el botón exista en el HTML
            IWebElement btn = wait.Until(ExpectedConditions.ElementExists(btnGuardar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);
            System.Threading.Thread.Sleep(500);

            // 3. Intentamos hacer clic normal. Si hay una capa bloqueando, forzamos con JS.
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(btn)).Click();
            }
            catch (Exception)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            }

            // 4. PAUSA DE GUARDADO: Evita que Selenium cierre el navegador antes de que el servidor responda
            System.Threading.Thread.Sleep(4000);
        }


        // =============================
        // DAR DE BAJA
        // =============================
        // Usamos un XPath genérico para el buscador de la grilla (suele ser el primer input visible en la lista)
        private By txtBuscarPlaca = By.XPath("(//input[contains(@class, 'p-column-filter') or contains(@class, 'tsp-input-filter')])[4]");
        // Apuntamos al botón que contiene el ícono, no solo al ícono
        private By btnVerVehiculo = By.XPath("//mat-icon[normalize-space()='search']");
        private By btnDarDeBaja = By.XPath("//mat-icon[normalize-space()='delete']");
        // Buscamos el textarea dinámico
        private By txtObservaciones = By.XPath("//textarea[contains(@id,'mat-input')] | //textarea");
        // Buscamos el botón por su texto dentro del modal
        private By btnConfirmarBaja = By.XPath("//button[contains(., 'Confirmar') or contains(., 'CONFIRMAR') or contains(., 'Aceptar') or contains(., 'ACEPTAR')]");

        public void BuscarVehiculoPorPlaca(string placa)
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(2000); // Esperar que la grilla cargue completamente

            IWebElement searchBox = wait.Until(ExpectedConditions.ElementToBeClickable(txtBuscarPlaca));

            // Le damos un clic primero (PrimeNG a veces necesita foco antes de limpiar)
            searchBox.Click();
            searchBox.Clear();
            searchBox.SendKeys(placa);

            // Pausa para que la grilla filtre los resultados automáticamente después de escribir
            System.Threading.Thread.Sleep(2000);
        }

        public void ClicVerVehiculo()
        {
            var wait = Wait();
            IWebElement btnVer = wait.Until(ExpectedConditions.ElementExists(btnVerVehiculo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnVer);
            System.Threading.Thread.Sleep(2000); // Esperar que cargue la vista de detalle
        }

        public void ClicDarDeBaja()
        {
            var wait = Wait();
            IWebElement btnBaja = wait.Until(ExpectedConditions.ElementExists(btnDarDeBaja));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBaja);
            System.Threading.Thread.Sleep(1000); // Esperar que abra el modal
        }

        public void IngresarObservaciones(string observaciones)
        {
            var wait = Wait();
            IWebElement txtObs = wait.Until(ExpectedConditions.ElementIsVisible(txtObservaciones));
            txtObs.Clear();
            txtObs.SendKeys(observaciones);
        }

        public void ConfirmarBaja()
        {
            var wait = Wait();
            IWebElement btnConf = wait.Until(ExpectedConditions.ElementExists(btnConfirmarBaja));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnConf);
            System.Threading.Thread.Sleep(2000); // Esperar que cierre el modal y guarde
        }


        // =============================
        // EDITAR VEHÍCULO
        // =============================

        private By btnEditarVehiculo = By.XPath("//mat-icon[normalize-space()='edit']");

        public void ClicEditarVehiculo()
        {
            var wait = Wait();
            IWebElement btnEdit = wait.Until(ExpectedConditions.ElementExists(btnEditarVehiculo));

            // Usamos JS Click por si hay animaciones bloqueando
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnEdit);

            // Le damos 2 segundos para que el formulario cargue con todos los datos llenos
            System.Threading.Thread.Sleep(2000);
        }
    }
}