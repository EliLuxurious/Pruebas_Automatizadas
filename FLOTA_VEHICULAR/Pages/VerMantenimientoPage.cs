using FLOTA_VEHICULAR.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace FLOTA_VEHICULAR.Pages
{
    public class VerMantenimientoPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public VerMantenimientoPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        WebDriverWait Wait(int seconds = 20)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }

        // =============================
        // NAVEGACIÓN A VER MANTENIMIENTOS
        // =============================

        // Reutilizamos el desplegable principal de Mantenimiento
        private By moduloMantenimiento = By.XPath("//span[@class='mat-expansion-indicator ng-tns-c243-9 ng-trigger ng-trigger-indicatorRotate ng-star-inserted'] | //mat-panel-title[contains(., 'Mantenimiento')]");

        // XPath indestructible por texto (evitamos el is-active)
        private By submoduloVerMantenimientos = By.XPath("//div[normalize-space()='Ver Mantenimientos'] | //span[contains(normalize-space(), 'Ver Mantenimientos')]");

        public void IngresarSubmoduloVerMantenimientos()
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(2000); // Esperar que cargue el menú lateral

            // 1. Clic en el acordeón principal 'Mantenimiento'
            IWebElement modulo = wait.Until(ExpectedConditions.ElementExists(moduloMantenimiento));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", modulo);
            System.Threading.Thread.Sleep(1500); // Animación de despliegue

            // 2. Clic en 'Ver Mantenimientos'
            IWebElement submodulo = wait.Until(ExpectedConditions.ElementExists(submoduloVerMantenimientos));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submodulo);
            System.Threading.Thread.Sleep(2000); // Pausa para que cargue la nueva grilla
        }



        private By btnNuevo = By.XPath("//button[contains(@class, 'tsp-button-success')]//span[contains(text(), 'NUEVO') or contains(., 'Nuevo')] | //button[.//mat-icon[normalize-space()='add']]");

        // XPath Supremo: Busca por placeholder, luego por texto visual, y si todo falla, agarra el primer input del modal
        private By txtPlaca = By.XPath("//mat-dialog-container//input[contains(@placeholder, 'PLACA') or contains(@data-placeholder, 'PLACA')] | //mat-dialog-container//mat-form-field[contains(., 'PLACA')]//input | (//mat-dialog-container//input)[1]");
        // 1. La lupa: Ahora el robot buscará la lupa estrictamente DENTRO del modal
        private By btnLupaPlaca = By.XPath("//mat-dialog-container//button[.//mat-icon[normalize-space()='search']] | //div[contains(@class, 'mat-dialog')]//button[.//mat-icon[normalize-space()='search']]");

        // 2. KM y Monto (Aprovechamos para hacerlos indestructibles con el mat-dialog-container y mat-label)
        private By txtKm = By.XPath("//mat-dialog-container//mat-label[contains(translate(., 'km', 'KM'), 'KM')]/ancestor::mat-form-field//input | //mat-dialog-container//mat-form-field[contains(., 'KM')]//input");

        private By txtMonto = By.XPath("//mat-dialog-container//mat-label[contains(translate(., 'monto', 'MONTO'), 'MONTO')]/ancestor::mat-form-field//input | //mat-dialog-container//mat-form-field[contains(., 'MONTO')]//input");

        // Calendario (El toggle infalible)
        // Calendario (El toggle infalible encerrado en el modal)
        private By btnCalendarioUnico = By.XPath("//mat-dialog-container//mat-datepicker-toggle//button");






        // Botones "+" usando tu descubrimiento de ng-reflect-message para que sean súper precisos
        private By btnMasActividad = By.XPath("//button[contains(@ng-reflect-message, 'actividad')]//mat-icon[normalize-space()='add'] | (//mat-dialog-container//button[.//mat-icon[normalize-space()='add' or contains(text(), 'add')]])[1]");

        private By btnMasRepuesto = By.XPath("//button[contains(@ng-reflect-message, 'repuesto')]//mat-icon[normalize-space()='add'] | (//mat-dialog-container//button[.//mat-icon[normalize-space()='add' or contains(text(), 'add')]])[2]");




        // Botón Cerrar apuntando ESTRICTAMENTE al último modal abierto (para que no cierre el principal)
        //private By btnCerrarModal = By.XPath("(//mat-dialog-container)[last()]//button[contains(@class, 'tsp-button-delete') or contains(translate(., 'cerrar', 'CERRAR'), 'CERRAR')]");

        // Botón Cerrar exclusivo para el modal chiquito (evita cerrar la ventana principal por error)
        //private By btnCerrarModalPeq = By.XPath("//mat-dialog-container[contains(., 'MAESTRO') or contains(., 'maestro')]//button[contains(translate(., 'cerrar', 'CERRAR'), 'CERRAR')]");

        // Botón Cerrar buscando ESTRICTAMENTE el último que aparezca en el código (para ignorar ventanas fantasmas cerradas previamente)
        private By btnCerrarModalPeq = By.XPath("(//mat-dialog-container//button[contains(translate(., 'cerrar', 'CERRAR'), 'CERRAR')])[last()]");


        private By inputFileDocumento = By.XPath("//input[@type='file']");

        //private By btnGuardar = By.XPath("//button[contains(@class, 'tsp-button-success') and contains(., 'Guardar')]");
        // XPath Híbrido: Encuentra el botón Guardar en modo Creación O en modo Edición
        private By btnGuardar = By.XPath("//button[contains(@class, 'tsp-button-success') or contains(@class, 'button-editing')][contains(., 'Guardar')] | //button[.//span[contains(text(), 'Guardar')]]");







        // ==========================================
        // XPATHS - HISTORIAL DE MANTENIMIENTO
        // ==========================================
        private By btnHistorial = By.XPath("//button[contains(., 'HISTORIAL') or contains(., 'history')]");

        // Input y lupa dentro del modal de historial
        private By txtPlacaHistorial = By.XPath("//mat-dialog-container//mat-label[contains(translate(., 'placa', 'PLACA'), 'PLACA')]/ancestor::mat-form-field//input | (//mat-dialog-container//input)[1]");
        private By btnLupaHistorial = By.XPath("//mat-dialog-container//button[.//mat-icon[normalize-space()='search']] | //button[contains(., 'Buscar por placa')]");

        // Elementos de la tabla de resultados
        private By txtTablaVacia = By.XPath("//mat-dialog-container//*[contains(translate(., 'vehículo', 'VEHÍCULO'), 'NO HAY VEHÍCULO')] | //td[contains(., 'No hay Vehículo')]");
        private By filasDeTablaHistorial = By.XPath("//mat-dialog-container//div[contains(@class, 'p-datatable-wrapper')]//tbody//tr | //mat-dialog-container//tbody//tr");


        // ==========================================
        // XPATHS - FILTROS Y ELIMINACIÓN
        // ==========================================
        // Usamos tus XPaths exactos, pero con un respaldo por si las columnas cambian de orden
        private By filtroPlaca = By.XPath("//th[4]//input[1] | //thead//input[contains(@placeholder, 'PLACA') or contains(@placeholder, 'Placa')]");
        private By filtroMonto = By.XPath("//th[6]//input[1] | //thead//input[contains(@placeholder, 'MONTO') or contains(@placeholder, 'Monto')]");

        // Botón de lupa de la tabla principal
        private By btnVerRegistroTabla = By.XPath("//tbody/tr[1]/td[7]/div[1]/button[1] | //tbody/tr[1]//button[.//mat-icon[normalize-space()='search']]");

        // Botones dentro del modal de detalle
        private By btnTachoEliminar = By.XPath("//mat-dialog-container//button[.//mat-icon[normalize-space()='delete']] | //button[.//mat-icon[normalize-space()='delete']]");

        // Botón rojo de confirmación
       // private By btnConfirmarEliminar = By.XPath("//mat-dialog-container//button[.//span[normalize-space()='Eliminar']] | //button[.//span[normalize-space()='Eliminar']]");

        // Elemento para validar que la grilla está vacía
        private By msjSinRegistros = By.XPath("//td[contains(translate(., 'registros', 'REGISTROS'), 'NO SE ENCONTRARON REGISTROS')] | //div[contains(@class, 'p-datatable-emptymessage')]");

        // Botón rojo de confirmación
        private By btnConfirmarEliminar = By.XPath("//mat-dialog-container//button[.//span[normalize-space()='Eliminar']] | //button[.//span[normalize-space()='Eliminar']]");

        // Botón Editar (Lápiz amarillo) dentro del modal de detalle
        private By btnEditarMantenimiento = By.XPath("//mat-dialog-container//button[contains(@class, 'btn-warning') or .//mat-icon[normalize-space()='edit']] | //button[.//mat-icon[normalize-space()='edit']]");


        // ==========================================
        // XPATHS - FILTROS DE BÚSQUEDA
        // ==========================================
        // Buscamos el desplegable basándonos en su etiqueta (label) para evadir los ID dinámicos
        private By cbxFiltroTipoMantenimiento = By.XPath("//mat-label[contains(translate(., 'tipo de mantenimiento', 'TIPO DE MANTENIMIENTO'), 'TIPO DE MANTENIMIENTO')]/ancestor::mat-form-field//mat-select");

        // Botón azul oscuro de BUSCAR
        private By btnBuscarPrincipal = By.XPath("//button[contains(@class, 'tsp-button-primary') and contains(., 'BUSCAR')] | //button[.//span[normalize-space()='BUSCAR']]");

        
        // ==========================================
        // MÉTODOS DE ACCIÓN
        // ==========================================

        public void ClicNuevoMantenimiento()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnNuevo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(2000);
        }

        public void BuscarPlaca(string placa)
        {
            var wait = Wait(10);

            // 1. Buscamos el input de Placa (esperamos hasta que realmente se pueda clickear)
            IWebElement input = wait.Until(ExpectedConditions.ElementToBeClickable(txtPlaca));

            // 2. Le damos clic con JS para forzar el foco
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", input);
            System.Threading.Thread.Sleep(500);

            // 3. Limpieza destructiva: Usamos .Clear() y luego teclado para que Angular no sobreviva
            input.Clear();
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            System.Threading.Thread.Sleep(500);

            // 4. Escribimos la placa
            input.SendKeys(placa);
            System.Threading.Thread.Sleep(500);

            // 5. Buscamos y damos clic en la lupita azul
            IWebElement lupa = wait.Until(ExpectedConditions.ElementToBeClickable(btnLupaPlaca));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupa);

            // Pausa obligatoria para que la base de datos devuelva los datos del vehículo
            System.Threading.Thread.Sleep(3000);
        }



        public void IngresarKm(string km)
        {
            var wait = Wait();
            IWebElement input = wait.Until(ExpectedConditions.ElementToBeClickable(txtKm));
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            input.SendKeys(km);
        }

        public void IngresarMonto(string monto)
        {
            var wait = Wait();
            IWebElement input = wait.Until(ExpectedConditions.ElementToBeClickable(txtMonto));
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            input.SendKeys(monto);
        }

        // ==========================================
        // LÓGICA DE CALENDARIOS RECICLADA
        // ==========================================
        private string ObtenerMesAngular(string mesNumero)
        {
            switch (mesNumero)
            {
                case "01": case "1": return "ENE";
                case "02": case "2": return "FEB";
                case "03": case "3": return "MAR";
                case "04": case "4": return "ABR";
                case "05": case "5": return "MAY";
                case "06": case "6": return "JUN";
                case "07": case "7": return "JUL";
                case "08": case "8": return "AGO";
                case "09": case "9": return "SEP";
                case "10": return "OCT";
                case "11": return "NOV";
                case "12": return "DIC";
                default: return "ENE";
            }
        }

        private void SeleccionarEnCalendario(By btnCalendario, string dia, string mes, string anio)
        {
            var wait = Wait(10);

            // Cerramos cualquier cosa abierta por si acaso
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            System.Threading.Thread.Sleep(500);

            // PASO 1: Abrir el calendario
            IWebElement btnCal = wait.Until(ExpectedConditions.ElementToBeClickable(btnCalendario));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCal);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal);
            System.Threading.Thread.Sleep(1000);

            // PASO 2: Clic en el botón superior (el triangulito) 
            By btnTriangulo = By.CssSelector("button.mat-calendar-period-button");
            IWebElement triangulo = wait.Until(ExpectedConditions.ElementToBeClickable(btnTriangulo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", triangulo);
            System.Threading.Thread.Sleep(1000);

            // CLASE MAESTRA DE ANGULAR PARA CALENDARIOS
            string claseCalendario = "mat-calendar-body-cell-content";

            // PASO 3: Clic en el Año 
            By celdaAnio = By.XPath($"//div[contains(@class, '{claseCalendario}') and normalize-space()='{anio}']");
            int targetYear = int.Parse(anio);
            int currentYear = DateTime.Now.Year;
            int intentos = 0;

            while (driver.FindElements(celdaAnio).Count == 0 && intentos < 5)
            {
                string selectorFlecha = targetYear > currentYear ? "button.mat-calendar-next-button" : "button.mat-calendar-previous-button";
                IWebElement btnFlecha = driver.FindElement(By.CssSelector(selectorFlecha));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnFlecha);
                System.Threading.Thread.Sleep(800);
                intentos++;
            }

            IWebElement elementAnio = wait.Until(ExpectedConditions.ElementExists(celdaAnio));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elementAnio);
            System.Threading.Thread.Sleep(1000);

            // PASO 4: Clic en el Mes (Búsqueda inteligente ignorando mayúsculas, minúsculas y puntos)
            By celdaMes = By.XPath($"//div[contains(@class, '{claseCalendario}') and contains(translate(., 'abcdefghijklmnopqrstuvwxyz.', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), '{mes}')]");
            IWebElement elementMes = wait.Until(ExpectedConditions.ElementExists(celdaMes));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elementMes);
            System.Threading.Thread.Sleep(1000);

            // PASO 5: Clic en el Día 
            By celdaDia = By.XPath($"//div[contains(@class, '{claseCalendario}') and normalize-space()='{dia}']");
            IWebElement elementDia = wait.Until(ExpectedConditions.ElementExists(celdaDia));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elementDia);
            System.Threading.Thread.Sleep(1000);
        }

        public void IngresarFechaMantenimiento(string fecha)
        {
            string[] partes = fecha.Split('/');
            string dia = int.Parse(partes[0]).ToString();
            string mes = ObtenerMesAngular(partes[1]);
            string anio = partes[2];

            SeleccionarEnCalendario(btnCalendarioUnico, dia, mes, anio);
        }


        // ==========================================
        // LÓGICA DE LISTAS BLINDADA (SIN ESCAPE)
        // ==========================================
        public void SeleccionarDeLista(string etiquetaCampo, string valorASeleccionar)
        {
            var wait = Wait();

            // 1. Clic en la lista desplegable correspondiente
            By desplegable = By.XPath($"//mat-label[contains(translate(text(), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), '{etiquetaCampo.ToUpper()}')]/ancestor::mat-form-field//mat-select");
            IWebElement select = wait.Until(ExpectedConditions.ElementToBeClickable(desplegable));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", select);
            System.Threading.Thread.Sleep(1000);

            // 2. Magia anti-mayúsculas
            string valorBuscado = valorASeleccionar.ToUpper();
            By opcion = By.XPath($"//mat-option[contains(translate(., 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), '{valorBuscado}')]");

            IWebElement opt = wait.Until(ExpectedConditions.ElementToBeClickable(opcion));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opt);

            // Pausa para que Angular cierre la lista por sí solo tras hacer clic
            System.Threading.Thread.Sleep(1000);
        }

       
        public void AgregarActividad(string actividad)
        {
            var wait = Wait(10);

            // 1. Clic en el botón "+" (Abre el modal para desbugear Angular)
            IWebElement btnMas = wait.Until(ExpectedConditions.ElementToBeClickable(btnMasActividad));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnMas);
            System.Threading.Thread.Sleep(1500);

            // 2. Clic en el último "Cerrar" que exista
            IWebElement btnCerrar = wait.Until(ExpectedConditions.ElementToBeClickable(btnCerrarModalPeq));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCerrar);
            System.Threading.Thread.Sleep(1000);

            // 3. Seleccionamos de la lista (Al hacer clic en la opción, Angular ya lo agrega a la tabla solo)
            SeleccionarDeLista("ACTIVIDADES", actividad);
            System.Threading.Thread.Sleep(1000); // Pausa para que se refleje en la grilla inferior
        }

        public void AgregarRepuesto(string repuesto)
        {
            var wait = Wait(10);

            // 1. Clic en el botón "+" (Abre el modal)
            IWebElement btnMas = wait.Until(ExpectedConditions.ElementToBeClickable(btnMasRepuesto));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnMas);
            System.Threading.Thread.Sleep(1500);

            // 2. Clic en el último "Cerrar" que exista
            IWebElement btnCerrar = wait.Until(ExpectedConditions.ElementToBeClickable(btnCerrarModalPeq));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCerrar);
            System.Threading.Thread.Sleep(1000);

            // 3. Seleccionamos de la lista
            SeleccionarDeLista("REPUESTOS", repuesto);
            System.Threading.Thread.Sleep(1000);
        }

        // ==========================================
        // ARCHIVOS Y GUARDADO
        // ==========================================
        public void AdjuntarDocumento(string rutaArchivo)
        {
            var wait = Wait();
            // Truco QA: Enviamos la ruta directamente al input invisible type='file'
            IWebElement input = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(inputFileDocumento)).First();
            input.SendKeys(rutaArchivo);
            System.Threading.Thread.Sleep(2000); // Esperar que el archivo se cargue
        }

        public void GuardarMantenimiento()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(3000);
        }

        // ==========================================
        // MÉTODOS - HISTORIAL DE MANTENIMIENTO
        // ==========================================
        public void ClicBotonHistorial()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnHistorial));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(2000); // Esperamos a que abra el modal
        }

        public void BuscarPlacaEnHistorial(string placa)
        {
            var wait = Wait(10);

            // 1. Buscamos la cajita de placa dentro del modal de historial
            IWebElement input = wait.Until(ExpectedConditions.ElementToBeClickable(txtPlacaHistorial));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", input);
            System.Threading.Thread.Sleep(500);

            // 2. Limpiamos y escribimos
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            input.SendKeys(placa);
            System.Threading.Thread.Sleep(500);

            // 3. Clic en la lupa
            IWebElement lupa = wait.Until(ExpectedConditions.ElementToBeClickable(btnLupaHistorial));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupa);

            // Le damos 3 segundos para que la tabla cargue los registros
            System.Threading.Thread.Sleep(3000);
        }

        public bool ValidarHistorialTieneRegistros()
        {
            // 1. Verificamos si aparece el texto rojo triste "No hay Vehículo"
            var elementosVacios = driver.FindElements(txtTablaVacia);
            if (elementosVacios.Count > 0 && elementosVacios[0].Displayed)
            {
                return false; // La tabla está vacía
            }

            // 2. Si no está vacío, contamos las filas de la tabla de Angular/PrimeNG
            var filas = driver.FindElements(filasDeTablaHistorial);
            return filas.Count > 0; // Devolverá true si encuentra al menos 1 fila de mantenimiento
        }





        public bool ValidarMensajeExito(string mensajeEsperado)
        {
            var wait = Wait(10);
            try
            {
                By alertaGlobal = By.XPath("//div[@role='alert'] | //snack-bar-container | //div[contains(@class, 'toast-message')] | //mat-snack-bar-container");
                IWebElement alerta = wait.Until(ExpectedConditions.ElementIsVisible(alertaGlobal));
                return alerta.Text.ToLower().Contains(mensajeEsperado.ToLower());
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }


        public bool ValidarMensajeError(string mensajeEsperado)
        {
            var wait = Wait(10);
            try
            {
                // Buscamos cualquier alerta en pantalla (sea verde, roja o SweetAlert)
                By alertaGlobal = By.XPath("//div[@role='alert'] | //snack-bar-container | //div[contains(@class, 'toast-message')] | //mat-snack-bar-container");
                IWebElement alerta = wait.Until(ExpectedConditions.ElementIsVisible(alertaGlobal));

                string textoAlerta = alerta.Text.ToLower();
                Console.WriteLine($"[INFO SISTEMA]: El sistema arrojó la siguiente alerta -> '{alerta.Text}'");

                // Verificamos si la alerta contiene la frase que estamos esperando
                return textoAlerta.Contains(mensajeEsperado.ToLower());
            }
            catch (WebDriverTimeoutException)
            {
                // Si pasan 10 segundos y no sale el error, devolvemos false (lo que hará fallar la prueba)
                return false;
            }
        }




        // ==========================================
        // MÉTODOS - FILTROS Y ELIMINACIÓN
        // ==========================================
        /* public void FiltrarMantenimiento(string placa, string monto)
         {
             var wait = Wait(10);

             // 1. Filtro Placa
             IWebElement inputPlaca = wait.Until(ExpectedConditions.ElementToBeClickable(filtroPlaca));
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", inputPlaca);
             inputPlaca.SendKeys(Keys.Control + "a");
             inputPlaca.SendKeys(Keys.Delete);
             inputPlaca.SendKeys(placa);
             inputPlaca.SendKeys(Keys.Enter); // Forzamos la búsqueda de Angular

             // 2. Filtro Monto
             IWebElement inputMonto = wait.Until(ExpectedConditions.ElementToBeClickable(filtroMonto));
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", inputMonto);
             inputMonto.SendKeys(Keys.Control + "a");
             inputMonto.SendKeys(Keys.Delete);
             inputMonto.SendKeys(monto);
             inputMonto.SendKeys(Keys.Enter);

             // 3. Magia: Esperamos 3 segundos para que Angular procese los filtros y actualice la tabla
             System.Threading.Thread.Sleep(3000);
         }
        */

        /*public void FiltrarMantenimiento(string placa, string monto)
        {
            var wait = Wait(10);

            // 1. Freno inicial: Esperamos que la grilla cargue sus datos al entrar al módulo
            System.Threading.Thread.Sleep(2500);

            // 2. Filtro Placa
            IWebElement inputPlaca = wait.Until(ExpectedConditions.ElementToBeClickable(filtroPlaca));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", inputPlaca);
            inputPlaca.SendKeys(Keys.Control + "a");
            inputPlaca.SendKeys(Keys.Delete);
            inputPlaca.SendKeys(placa);
            inputPlaca.SendKeys(Keys.Enter); // Esto dispara la búsqueda de Angular

            // 3. Freno intermedio: ¡Vital! Angular destruye y rearma la tabla aquí. Le damos tiempo.
            System.Threading.Thread.Sleep(2000);

            // 4. Filtro Monto (Volvemos a buscar el elemento porque la tabla ahora es nueva)
            IWebElement inputMonto = wait.Until(ExpectedConditions.ElementToBeClickable(filtroMonto));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", inputMonto);
            inputMonto.SendKeys(Keys.Control + "a");
            inputMonto.SendKeys(Keys.Delete);
            inputMonto.SendKeys(monto);
            inputMonto.SendKeys(Keys.Enter);

            // 5. Espera final para que la tabla muestre el resultado definitivo
            System.Threading.Thread.Sleep(3000);
        }*/

        public void FiltrarMantenimiento(string placa, string monto)
        {
            var wait = Wait(10);
            System.Threading.Thread.Sleep(2500);

            // 1. Filtro Placa
            IWebElement inputPlaca = wait.Until(ExpectedConditions.ElementToBeClickable(filtroPlaca));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", inputPlaca);
            inputPlaca.SendKeys(Keys.Control + "a");
            inputPlaca.SendKeys(Keys.Delete);

            // ¡MAGIA ANTI-CORTES! Inyectamos toda la palabra de golpe en la memoria del navegador
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = arguments[1];", inputPlaca, placa);

            // Truco QA: Damos un espacio y lo borramos para obligar a Angular a darse cuenta de que el texto cambió
            inputPlaca.SendKeys(Keys.Space);
            inputPlaca.SendKeys(Keys.Backspace);
            inputPlaca.SendKeys(Keys.Enter);

            System.Threading.Thread.Sleep(2000);

            // 2. Filtro Monto
            IWebElement inputMonto = wait.Until(ExpectedConditions.ElementToBeClickable(filtroMonto));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", inputMonto);
            inputMonto.SendKeys(Keys.Control + "a");
            inputMonto.SendKeys(Keys.Delete);

            // Inyectamos el monto de golpe
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = arguments[1];", inputMonto, monto);
            inputMonto.SendKeys(Keys.Space);
            inputMonto.SendKeys(Keys.Backspace);
            inputMonto.SendKeys(Keys.Enter);

            System.Threading.Thread.Sleep(3000);
        }



        public void AbrirDetallePrimerRegistro()
        {
            var wait = Wait(10);
            IWebElement btnVer = wait.Until(ExpectedConditions.ElementToBeClickable(btnVerRegistroTabla));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnVer);
            System.Threading.Thread.Sleep(2000); // Esperamos a que abra el modal gigante
        }

        public void ClicEliminarMantenimiento()
        {
            var wait = Wait(10);
            IWebElement btnTacho = wait.Until(ExpectedConditions.ElementToBeClickable(btnTachoEliminar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnTacho);
            // Esperamos 2 segundos para que el sistema procese la eliminación directa y lance el toast verde
            System.Threading.Thread.Sleep(2000);
        }


        public bool ValidarRegistroNoExisteEnGrilla()
        {
            try
            {
                var wait = Wait(5); // Solo esperamos 5 segundos

                // XPath indestructible: Busca cualquier elemento que contenga "NO SE ENCONTRARON REGISTROS", ignorando minúsculas.
                By mensajeVacio = By.XPath("//*[contains(translate(., 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), 'NO SE ENCONTRARON REGISTROS')]");

                wait.Until(ExpectedConditions.ElementIsVisible(mensajeVacio));

                Console.WriteLine("[INFO QA]: ¡Confirmado! Se detectó el texto 'NO SE ENCONTRARON REGISTROS DISPONIBLES' en la pantalla.");
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                // Si el mensaje no apareció, significa que el registro fantasma sigue ahí
                return false;
            }
        }


        public void ConfirmarEliminacion()
        {
            var wait = Wait(10);

            By btnConfirmar = By.XPath("//mat-dialog-container//button[contains(translate(., 'eliminar', 'ELIMINAR'), 'ELIMINAR')] | //button[.//span[contains(., 'Eliminar')]]");
            IWebElement btnConfirma = wait.Until(ExpectedConditions.ElementToBeClickable(btnConfirmar));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnConfirma);

            // ¡EL FRENO DE MANO! Esperamos 3 segundos para que la ventanita se cierre 
            // y la base de datos termine de eliminar el registro.
            System.Threading.Thread.Sleep(3000);
        }

        public void ClicEditarMantenimiento()
        {
            var wait = Wait(10);
            IWebElement btnEdit = wait.Until(ExpectedConditions.ElementToBeClickable(btnEditarMantenimiento));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnEdit);

            // Pausa vital: Esperamos 2 segundos para que Angular habilite todos los campos que estaban bloqueados
            System.Threading.Thread.Sleep(2000);
        }



        // ==========================================
        // MÉTODOS EXCLUSIVOS PARA EDICIÓN (SIN TOCAR EL "+")
        // ==========================================
        public void AgregarActividadEnEdicion(string actividad)
        {
            // Vamos directo a la lista desplegable
            SeleccionarDeLista("ACTIVIDADES", actividad);
            System.Threading.Thread.Sleep(1000); // Pausa para que se refleje en la interfaz
        }

        public void AgregarRepuestoEnEdicion(string repuesto)
        {
            // Vamos directo a la lista desplegable
            SeleccionarDeLista("REPUESTOS", repuesto);
            System.Threading.Thread.Sleep(1000);
        }


        // ==========================================
        // MÉTODOS - FILTROS DE BÚSQUEDA
        // ==========================================
        public void SeleccionarFiltroTipoMantenimiento(string tipoEsperado)
        {
            var wait = Wait(10);
            System.Threading.Thread.Sleep(2000); // Pausa para que carguen los filtros al entrar

            // 1. Clic en el desplegable para abrir las opciones
            IWebElement select = wait.Until(ExpectedConditions.ElementToBeClickable(cbxFiltroTipoMantenimiento));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", select);
            System.Threading.Thread.Sleep(1000);

            // 2. Buscamos la opción exacta que nos pasen (Preventivo o Correctivo) y le damos clic
            By opcion = By.XPath($"//mat-option[contains(translate(., 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), '{tipoEsperado.ToUpper()}')]");
            IWebElement opt = wait.Until(ExpectedConditions.ElementToBeClickable(opcion));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opt);
            System.Threading.Thread.Sleep(500);

            // 3. Como es un combo con checkboxes, apretamos ESCAPE para cerrarlo sin cancelar la selección
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            System.Threading.Thread.Sleep(1000);
        }

        public void ClicBotonBuscarPrincipal()
        {
            var wait = Wait(10);
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnBuscarPrincipal));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);

            // Pausa obligatoria para que Angular consulte a la BD y repinte la grilla inferior
            System.Threading.Thread.Sleep(3000);
        }

        public void ValidarColumnaTipoMantenimiento(string valorEsperado)
        {
            // Según el video, la columna "TIPO DE MANTENIMIENTO" es la 5ta (N°, Fecha, Prox, Vehiculo, Tipo...)
            By celdasTipo = By.XPath("//tbody//tr/td[5]");

            var elementos = driver.FindElements(celdasTipo);

            if (elementos.Count == 0)
            {
                // Si la tabla está vacía, no hay nada que validar
                Console.WriteLine("INFO: La búsqueda no arrojó resultados para validar.");
                return;
            }

            // Recorremos todas las filas visibles para asegurarnos que NINGUNA sea diferente
            foreach (var celda in elementos)
            {
                string textoCelda = celda.Text.Trim().ToUpper();
                if (!textoCelda.Contains(valorEsperado.ToUpper()))
                {
                    // Si encuentra un intruso (ej. sale un 'Preventivo' buscando 'Correctivo'), estalla la prueba
                    throw new Exception($"[BUG DETECTADO - FALLO DE FILTRO]: Se filtró por '{valorEsperado}', pero se filtró incorrectamente un registro de tipo '{textoCelda}'.");
                }
            }
        }













    }
}