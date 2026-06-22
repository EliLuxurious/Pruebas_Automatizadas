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
            var wait = Wait(10);

            // 1. Buscamos el botón de "+ NUEVO" de forma robusta por su texto o ícono
            By btnNuevo = By.XPath("//button[contains(translate(., 'nuevo', 'NUEVO'), 'NUEVO')] | //button[.//mat-icon[normalize-space()='add']]");

            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnNuevo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);

            // 2. En lugar de buscar un input con ID dinámico que rompe la prueba, 
            // simplemente le damos 2 segundos a Angular para que termine de abrir el panel lateral o modal.
            System.Threading.Thread.Sleep(2000);
        }

        // =============================
        // INPUTS
        // =============================
        // Usamos una combinación: el texto "CERRAR" o el botón que contiene el ícono 'close'
        private By btnCerrarModal = By.XPath("//button[contains(@class, 'button-close')] | //button//span[contains(text(), 'CERRAR')]/parent::button | //mat-icon[text()='close']/ancestor::button");
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
        // XPath Registrar reparación limpio basado en el tuyo. Usamos mat-tooltip-trigger y tsp-button-tool para asegurarnos de que es el botón de la barra de acciones.
        private By btnCambiarAOperativo = By.XPath("//button[contains(@class, 'tsp-button-tool') and contains(@class, 'mat-primary')]");

        
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

        /*  public void ClicDarDeBaja()
          {
              var wait = Wait();
              IWebElement btnBaja = wait.Until(ExpectedConditions.ElementExists(btnDarDeBaja));
              ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBaja);
              System.Threading.Thread.Sleep(1000); // Esperar que abra el modal
          }*/


        // =============================
        // DAR DE BAJA (ACTUALIZADO PARA CAPTURAR BUG CP016)
        // =============================
        public void ClicDarDeBaja()
        {
            // Le damos 10 segundos máximos para buscar el botón
            var wait = Wait(10);
            try
            {
                IWebElement btnBaja = wait.Until(ExpectedConditions.ElementExists(btnDarDeBaja));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBaja);
                System.Threading.Thread.Sleep(1000); // Esperar que abra el modal
            }
            catch (WebDriverTimeoutException)
            {
                // ¡AQUÍ ATRAPAMOS EL BUG DEL CP016!
                throw new Exception("[BUG DETECTADO ]: El botón DAR DE BAJA (papelera) NO está disponible. ");
            }
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
            // Usamos un tiempo de espera moderado (10 seg) para no hacer el test eterno si el botón no está
            var wait = Wait(10);

            try
            {
                // Esperamos a que el botón exista en el DOM
                IWebElement btnEdit = wait.Until(ExpectedConditions.ElementExists(btnEditarVehiculo));

                // Usamos JS Click por si hay animaciones bloqueando
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnEdit);

                // Le damos 2 segundos para que el formulario cargue con todos los datos llenos
                System.Threading.Thread.Sleep(2000);
            }
            catch (WebDriverTimeoutException)
            {
                // Si Selenium no encuentra el botón después de 10 segundos, cae aquí.
                // Como este método solo se usa cuando SE SUPONE que debemos poder editar, 
                // lanzamos el BUG directamente.
                throw new Exception("[BUG DETECTADO - REGLA DE NEGOCIO]: El botón EDITAR no está disponible en la pantalla. El sistema está bloqueando incorrectamente la edición para este estado del vehículo.");
            }
        }


        public bool ValidarAvisoPlacaDuplicada()
        {
            var wait = Wait(10); // Le damos tiempo a la validación de red
            try
            {
                // Usamos el XPath que sacaste de SelectorsHub
                By avisoPlaca = By.XPath("//div[@role='alert']");

                IWebElement alerta = wait.Until(ExpectedConditions.ElementIsVisible(avisoPlaca));

                // Si el texto contiene "Placa", confirmamos que es el error que buscamos
                return alerta.Text.ToUpper().Contains("PLACA");
            }
            catch
            {
                return false;
            }
        }


        // Para @regitrar vehiculo con motor de baja
        public bool ValidarEstadoCampoMotor()
        {
            // Esperamos a que el sistema procese la validación en tiempo real (Paso 14)
            System.Threading.Thread.Sleep(2000);

            try
            {
                // Buscamos el contenedor del input usando el ID EngineNumber para ver si Angular lo marcó como inválido
                // Si el campo tiene la clase 'mat-form-field-invalid', el sistema NO aceptó el motor.
                By contenedorError = By.XPath("//input[@id='EngineNumber']/ancestor::mat-form-field[contains(@class, 'mat-form-field-invalid')]");

                bool tieneError = driver.FindElements(contenedorError).Count > 0;

                // Retornamos TRUE si NO tiene error (es decir, fue aceptado)
                return !tieneError;
            }
            catch
            {
                // Si no encuentra el ID, por defecto asumimos que no hay error visual bloqueante
                return true;
            }
        }


        //PARA CERRAR MODAL TRAS CONFIRMAR BAJA
            public void CerrarVentanaDetalles()
            {
                var wait = Wait(15);
                try
                {
                    // 1. Esperamos a que el botón exista en el DOM
                    IWebElement btn = wait.Until(ExpectedConditions.ElementExists(btnCerrarModal));

                    // 2. Aseguramos que el botón sea visible haciendo scroll hacia él
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
                    System.Threading.Thread.Sleep(1000);

                    // 3. Clic con JavaScript (esto ignora si hay overlays o capas transparentes bloqueando)
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);

                    // 4. Verificamos que el modal realmente desaparezca
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.TagName("mat-dialog-container")));

                    // Pausa extra para que la grilla principal recupere el foco
                    System.Threading.Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al cerrar ventana: " + ex.Message);
                    // Si falla el clic suave, intentamos un clic de emergencia por coordenadas o enviando Escape
                    new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
                }
            }

        // Método para verificar que el botón Guardar está inhabilitado
        public bool ValidarBotonGuardarInhabilitado()
        {
            IWebElement btn = driver.FindElement(btnGuardar);
            // Verificamos si tiene el atributo 'disabled' o si la clase contiene 'disabled'
            string disabledAttr = btn.GetAttribute("disabled");
            string classAttr = btn.GetAttribute("class");

            return (disabledAttr != null && (disabledAttr == "true" || disabledAttr == "disabled"))
                   || classAttr.Contains("disabled")
                   || classAttr.Contains("mat-button-disabled");
        }


        public bool ValidarBotonGuardarHabilitado()
        {
            // Damos un segundo para que Angular procese las validaciones de los inputs
            System.Threading.Thread.Sleep(1500);
            IWebElement btn = driver.FindElement(btnGuardar);

            // Un botón está habilitado si NO tiene el atributo 'disabled' 
            // y NO tiene clases de 'mat-button-disabled'
            bool tieneAtributoDisabled = btn.GetAttribute("disabled") != null;
            bool tieneClaseDisabled = btn.GetAttribute("class").Contains("disabled");

            return !tieneAtributoDisabled && !tieneClaseDisabled;
        }

        public bool ValidarAvisoMotorDuplicado()
        {
            var wait = Wait(8);
            try
            {
                // Buscamos el aviso de alerta que aparece al ingresar el motor
                IWebElement alerta = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@role='alert']")));

                // Verificamos que el mensaje mencione algo de "Motor" para no confundirlo con placa
                return alerta.Text.ToUpper().Contains("MOTOR");
            }
            catch
            {
                return false; // Si no hay alerta, el sistema dejó pasar el motor (Bug)
            }
        }

        public bool ExisteBotonEditar()
        {
            // Damos un tiempo pequeño para que cargue la vista de detalles
            System.Threading.Thread.Sleep(2000);

            // Si la lista está vacía, es porque el botón no existe (Correcto para DE BAJA)
            return driver.FindElements(btnEditarVehiculo).Count > 0;
        }


        // Selector del botón morado de herramientas
        private By btnReportarAveria = By.XPath("//button[contains(@class, 'mat-primary') and contains(@class, 'tsp-button-tool')] | //mat-icon[normalize-space()='build']/ancestor::button");

        public void ClicReportarAveria()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnReportarAveria));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(1500); // Pausa para que el modal termine de abrir
        }

        // Método que busca directamente en la bandeja (grilla)
        public bool ValidarEstadoVehiculoEnGrilla(string placa, string estadoEsperado)
        {
            var wait = Wait(10);
            try
            {
                // XPath inteligente: Busca la fila (tr) de la placa, y dentro verifica si existe el estado esperado
                By celdaEstado = By.XPath($"//tr[td[contains(normalize-space(), '{placa}')]]//td[contains(normalize-space(), '{estadoEsperado.ToUpper()}')]");

                return wait.Until(ExpectedConditions.ElementIsVisible(celdaEstado)).Displayed;
            }
            catch
            {
                return false;
            }
        }



  


        public void ClicCambiarAOperativo()
        {
            var wait = Wait();
            // Esperamos a que el botón de reparación esté disponible
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnCambiarAOperativo));

            // JS Click para evitar problemas visuales
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(1500); // Pausa para que el modal termine de abrir
        }

        // =============================
        // VALIDACIÓN DE SEGURIDAD (MANTENIMIENTO BLOQUEADO)
        // =============================

        public bool ExistenBotonesDeMantenimiento()
        {
            // Esperamos 2 segundos para que el modal de detalle cargue completamente
            System.Threading.Thread.Sleep(2000);

            // Contamos si existe el botón de Avería (llave inglesa) o Reparación
            int cantAveria = driver.FindElements(btnReportarAveria).Count;
            int cantReparacion = driver.FindElements(btnCambiarAOperativo).Count;

            // Retorna TRUE si encuentra al menos uno de los dos botones
            return (cantAveria > 0 || cantReparacion > 0);
        }

        // =============================
        // VALIDACIÓN DE LÓGICA DE BOTONES (VERDADERA VISIBILIDAD CON XPATH AISLADO)
        // =============================

        public bool ExisteBotonRegistrarReparacion()
        {
            System.Threading.Thread.Sleep(2000);

            // 1. Declaramos el XPath AQUÍ ADENTRO. 
            // Esto asegura que no afecte a ningún otro método de la clase.
            By btnReparacionEspecifico = By.XPath("//button[.//mat-icon[normalize-space()='check_circle']]");

            // 2. Buscamos usando este XPath específico
            var botones = driver.FindElements(btnReparacionEspecifico);

            // 3. Verificamos que sea visible a los ojos (ignora los ocultos por código)
            foreach (var btn in botones)
            {
                if (btn.Displayed)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ExisteBotonReportarAveria()
        {
            System.Threading.Thread.Sleep(2000);

            // 1. XPath aislado solo para la validación visual de Avería
            By btnAveriaEspecifico = By.XPath("//button[.//mat-icon[normalize-space()='construction']]");

            var botones = driver.FindElements(btnAveriaEspecifico);

            foreach (var btn in botones)
            {
                if (btn.Displayed)
                {
                    return true;
                }
            }
            return false;
        }



        // =============================
        // FILTROS DE BANDEJA
        // =============================

        // Selectores de los Checkboxes
        private By chkTodoSoat = By.XPath("//label[@for='mat-checkbox-5-input']//span[@class='mat-checkbox-inner-container mat-checkbox-inner-container-no-side-margin']");
        private By chkTodoRevTecnica = By.XPath("//label[@for='mat-checkbox-6-input']//span[@class='mat-checkbox-inner-container mat-checkbox-inner-container-no-side-margin']");
        private By chkTodoRegistro = By.XPath("//label[@for='mat-checkbox-7-input']//span[@class='mat-checkbox-inner-container mat-checkbox-inner-container-no-side-margin']");
        private By chkAveriado = By.XPath("//label[@for='mat-checkbox-9-input']//span[@class='mat-checkbox-inner-container']");

        // Selector del botón Buscar
        private By btnBuscarFiltro = By.XPath("//button[@class='mat-focus-indicator mat-tooltip-trigger button-add mr-2 tsp-font-size-buttons mat-raised-button mat-button-base']");

        public void DesmarcarFiltrosParaDejarSoloOperativo()
        {
            var wait = Wait(10);

            // Usamos JavaScript Click porque los checkboxes de Angular a veces ocultan el input real y Selenium nativo no los puede clickear
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkTodoSoat)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkTodoRevTecnica)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkTodoRegistro)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkAveriado)));
        }

        public void ClicBotonBuscarFiltros()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnBuscarFiltro));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);

            // Le damos un tiempo prudencial de 3 segundos para que la grilla recargue la data del servidor
            System.Threading.Thread.Sleep(3000);
        }

        public bool ValidarColumnaEstadoVehiculo(string estadoEsperado)
        {
            // Buscamos todas las celdas de la columna 11 (Estado del Vehículo) en la tabla actual
            By celdasEstadoVehiculo = By.XPath("//tbody/tr/td[11]");
            var celdas = driver.FindElements(celdasEstadoVehiculo);

            // Si la tabla está vacía, no podemos validar
            if (celdas.Count == 0)
            {
                Console.WriteLine("Advertencia: La búsqueda no arrojó ningún resultado.");
                return false;
            }

            // Recorremos fila por fila
            foreach (var celda in celdas)
            {
                // Si encontramos al menos una celda que NO contenga el estado esperado, el filtro falló
                if (!celda.Text.ToUpper().Contains(estadoEsperado.ToUpper()))
                {
                    Console.WriteLine($"Error: Se encontró un registro con estado {celda.Text} cuando se esperaba {estadoEsperado}");
                    return false;
                }
            }

            // Si recorrió toda la tabla y no encontró errores, el filtro es perfecto
            return true;
        }

        // Selector del checkbox OPERATIVO que me acabas de pasar
        private By chkOperativo = By.XPath("//label[@for='mat-checkbox-8-input']//span[@class='mat-checkbox-inner-container']");

        public void DesmarcarFiltrosParaDejarSoloAveriado()
        {
            var wait = Wait(10);

            // Desmarcamos los mismos 3 de arriba (SOAT, Rev Técnica y Registro)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkTodoSoat)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkTodoRevTecnica)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkTodoRegistro)));

            // Y aquí la diferencia: desmarcamos OPERATIVO para que quede solo AVERIADO
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkOperativo)));
        }

        // =============================
        // FILTROS DE ESTADO DE REGISTRO
        // =============================

        // Selectores específicos de la sección "Estado de Registro"
        private By chkRegistroActivo = By.XPath("//label[@for='mat-checkbox-16-input']//span[@class='mat-checkbox-inner-container']");
        private By chkRegistroDeBaja = By.XPath("//label[@for='mat-checkbox-17-input']//span[@class='mat-checkbox-inner-container']");

        public void DesmarcarFiltrosParaDejarSoloRegistroActivo()
        {
            var wait = Wait(10);

            // Desmarcamos las otras categorías generales que vienen marcadas por defecto
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkTodoSoat)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkTodoRevTecnica)));

            // IMPORTANTE: En la sección "Estado de Registro", desmarcamos DE BAJA para que solo quede ACTIVO
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkRegistroDeBaja)));
        }

        public void DesmarcarFiltrosParaDejarSoloRegistroDeBaja()
        {
            var wait = Wait(10);

            // Desmarcamos las categorías generales
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkTodoSoat)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkTodoRevTecnica)));

            // IMPORTANTE: En la sección "Estado de Registro", desmarcamos ACTIVO para que solo quede DE BAJA
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", wait.Until(ExpectedConditions.ElementExists(chkRegistroActivo)));
        }

        // Nuevo validador dinámico para la columna "Estado de Registro" (Columna 10)
        public bool ValidarColumnaEstadoRegistro(string estadoEsperado)
        {
            // Buscamos todas las celdas de la columna 10 (Estado de Registro)
            By celdasEstadoRegistro = By.XPath("//tbody/tr/td[10]");
            var celdas = driver.FindElements(celdasEstadoRegistro);

            if (celdas.Count == 0) return false;

            foreach (var celda in celdas)
            {
                if (!celda.Text.ToUpper().Contains(estadoEsperado.ToUpper()))
                {
                    Console.WriteLine($"Error de filtrado: Se encontró {celda.Text} cuando se esperaba {estadoEsperado}");
                    return false;
                }
            }
            return true;
        }


        // =============================
        // EXPORTAR EXCEL
        // =============================

        private By btnExportarExcel = By.XPath("//mat-icon[normalize-space()='assignment_returned']/ancestor::button | //button[.//mat-icon[normalize-space()='assignment_returned']]");

        public void ClicExportarExcel()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnExportarExcel));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
        }

        public bool ValidarArchivoDescargado()
        {
            // 1. Obtenemos la ruta de tu carpeta "Descargas" de Windows
            string rutaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // 2. Le damos tiempo al navegador para descargar el archivo (10 segundos máximo)
            bool descargado = false;
            int intentos = 0;

            while (intentos < 10)
            {
                System.Threading.Thread.Sleep(1000); // Pausa de 1 segundo

                // 3. Buscamos el archivo más reciente en la carpeta de descargas
                // Ajusta el "*.xlsx" si el sistema descarga un ".csv" o ".xls"
                var directorio = new DirectoryInfo(rutaDescargas);
                var archivoReciente = directorio.GetFiles("*.xlsx")
                                                .OrderByDescending(f => f.LastWriteTime)
                                                .FirstOrDefault();

                // 4. Si hay un archivo y fue modificado/creado en los últimos 2 minutos, ¡es nuestro!
                if (archivoReciente != null && archivoReciente.LastWriteTime > DateTime.Now.AddMinutes(-2))
                {
                    Console.WriteLine($"¡Éxito! Archivo descargado: {archivoReciente.Name}");
                    descargado = true;
                    break; // Salimos del bucle
                }
                intentos++;
            }

            return descargado;
        }
    }


}