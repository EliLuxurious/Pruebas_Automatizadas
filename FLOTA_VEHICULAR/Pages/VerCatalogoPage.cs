using FLOTA_VEHICULAR.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace FLOTA_VEHICULAR.Pages
{
    public class VerCatalogoPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public VerCatalogoPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        WebDriverWait Wait(int seconds = 20)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }

        // =============================
        // NAVEGACIÓN DEL MÓDULO MANTENIMIENTO
        // =============================

        // Usamos tu XPath exacto + un respaldo por si Angular cambia los números dinámicos (ng-tns...)
        private By moduloMantenimiento = By.XPath("//span[@class='mat-expansion-indicator ng-tns-c243-9 ng-trigger ng-trigger-indicatorRotate ng-star-inserted'] | //mat-panel-title[contains(., 'Mantenimiento')]");

        // Tu XPath exacto para Ver Catálogos
        private By submoduloVerCatalogo = By.XPath("//div[normalize-space()='Ver Catálogos']");

        public void IngresarSubmoduloVerCatalogo()
        {
            var wait = Wait();
            // Le damos 2 segundos para que el menú lateral termine de cargar después del login
            System.Threading.Thread.Sleep(2000);

            // 1. Clic en el acordeón de Mantenimiento para desplegarlo
            IWebElement modulo = wait.Until(ExpectedConditions.ElementExists(moduloMantenimiento));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", modulo);
            System.Threading.Thread.Sleep(1500); // Pausa necesaria para que la animación de Angular despliegue la lista

            // 2. Clic en la opción Ver Catálogos
            IWebElement submodulo = wait.Until(ExpectedConditions.ElementExists(submoduloVerCatalogo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submodulo);
            System.Threading.Thread.Sleep(2000); // Pausa para que cargue la bandeja principal del catálogo
        }

        // =============================
        // REGISTRO DE CATÁLOGO (NUEVO)
        // =============================

        // Botón +NUEVO (Usando una búsqueda robusta sin clases dinámicas)
        private By btnNuevoCatalogo = By.XPath("//button[contains(., 'NUEVO') or .//mat-icon[normalize-space()='add']]");

        // Botón Guardar
        private By btnGuardarCatalogo = By.XPath("//button[contains(., 'Guardar') and contains(@class, 'tsp-button-success')]");

        public void ClicNuevoCatalogo()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnNuevoCatalogo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(2000); // Esperar que abra el modal
        }

        // Método dinámico para los Radio Buttons (Clasificador y Clase de Mantenimiento)
        // Método dinámico MEJORADO para los Radio Buttons
        /* public void SeleccionarRadioButton(string valor)
         {
             var wait = Wait(10);

             // XPath ultra-flexible: Busca el mat-radio-button que contenga el texto en cualquier parte de su interior, 
             // y luego apunta a su contenedor o a su label para hacerle el clic.
             By rdoOpcion = By.XPath($"//mat-radio-button[contains(., '{valor}')]//div[contains(@class, 'mat-radio-container')] | //mat-radio-button[contains(., '{valor}')]//label");

             // Usamos ElementExists en lugar de Clickable porque los radios de Angular a veces ocultan el input real
             IWebElement rdo = wait.Until(ExpectedConditions.ElementExists(rdoOpcion));

             // Clic forzado con JavaScript (infalible para Angular Material)
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", rdo);

             System.Threading.Thread.Sleep(500); // Pequeña pausa para que se pinte el circulito
         }*/



        public void SeleccionarRadioButton(string valor)
        {
            var wait = Wait(10);

            // 1. Buscamos TODOS los radio buttons que existen en la ventana
            By radioButtonsLoc = By.TagName("mat-radio-button");
            var todosLosRadios = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(radioButtonsLoc));

            // 2. Los revisamos uno por uno con C#
            foreach (var radio in todosLosRadios)
            {
                // .Text extrae lo que el ojo humano ve. .Trim() le borra los espacios basura de los lados.
                string textoDelRadio = radio.Text.Trim();

                // 3. Comparación EXACTA de C# (Aquí "ALTA" jamás se confundirá con "A")
                if (textoDelRadio == valor)
                {
                    // Encontramos el correcto. Buscamos su parte clickeable (el label) y le damos clic forzado.
                    IWebElement parteClickeable = radio.FindElement(By.TagName("label"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", parteClickeable);

                    System.Threading.Thread.Sleep(500); // Pausa para que se pinte el círculo
                    return; // ¡Misión cumplida! Salimos del método.
                }
            }

            // 4. Si revisa todos y no encuentra el exacto, nos avisa claramente
            throw new Exception($"[ERROR DE AUTOMATIZACIÓN]: No se encontró ningún Radio Button que diga exactamente '{valor}'.");
        }
        /*
        public void IngresarFechas(string fechaInicio, string fechaFin)
        {
            var wait = Wait(10);

            // 1. Usamos el XPath seguro que SÍ encuentra los campos correctos del modal basándose en su etiqueta
            By txtInicio = By.XPath("//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), 'inicio')]//input");
            By txtFin = By.XPath("//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), 'fin')]//input");

            // Buscamos los elementos asegurándonos de que existan
            IWebElement inputInicio = wait.Until(ExpectedConditions.ElementExists(txtInicio));
            IWebElement inputFin = wait.Until(ExpectedConditions.ElementExists(txtFin));

            // 2. Script de inyección mejorado (agregamos 'blur' para que Angular marque el campo como válido/rojo si es necesario)
            string scriptInyeccion = @"
                arguments[0].value = arguments[1];
                arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('blur', { bubbles: true }));
             ";

            // 3. Inyectamos Fecha Inicio directamente en la memoria del input
            ((IJavaScriptExecutor)driver).ExecuteScript(scriptInyeccion, inputInicio, fechaInicio);
            System.Threading.Thread.Sleep(500);

            // 4. Inyectamos Fecha Fin
            ((IJavaScriptExecutor)driver).ExecuteScript(scriptInyeccion, inputFin, fechaFin);
            System.Threading.Thread.Sleep(500);
        }
        */

        // 1. Traductor de meses (Convierte "01" a "ENE.", etc.)
        private string ObtenerMesAngular(string mesNumero)
        {
            switch (mesNumero)
            {
                case "01": case "1": return "ENE.";
                case "02": case "2": return "FEB.";
                case "03": case "3": return "MAR.";
                case "04": case "4": return "ABR.";
                case "05": case "5": return "MAY.";
                case "06": case "6": return "JUN.";
                case "07": case "7": return "JUL.";
                case "08": case "8": return "AGO.";
                case "09": case "9": return "SEP.";
                case "10": return "OCT.";
                case "11": return "NOV.";
                case "12": return "DIC.";
                default: return "ENE.";
            }
        }

        // 2. Este es el método que recibe tus fechas del Excel (ej. 15/01/2026) y las separa
        public void IngresarFechas(string fechaInicio, string fechaFin)
        {
            // Procesamos Fecha Inicio
            string[] partesInicio = fechaInicio.Split('/');
            string diaInicio = int.Parse(partesInicio[0]).ToString();
            string mesInicio = ObtenerMesAngular(partesInicio[1]);
            string anioInicio = partesInicio[2];

            // Procesamos Fecha Fin
            string[] partesFin = fechaFin.Split('/');
            string diaFin = int.Parse(partesFin[0]).ToString();
            string mesFin = ObtenerMesAngular(partesFin[1]);
            string anioFin = partesFin[2];

            // ¡EL ARREGLO ESTÁ AQUÍ! XPaths exactos que buscan por la palabra y no por el orden en pantalla
            By btnCalInicio = By.XPath("//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), 'inicio')]//button");
            By btnCalFin = By.XPath("//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), 'fin')]//button");

            // Ahora el robot no se confundirá jamás de cajita
            SeleccionarEnCalendario(btnCalInicio, diaInicio, mesInicio, anioInicio);
            SeleccionarEnCalendario(btnCalFin, diaFin, mesFin, anioFin);
        }

        // Fíjate que ahora recibe un "By btnCalendario" en vez del número 1 o 2

        private void SeleccionarEnCalendario(By btnCalendario, string dia, string mes, string anio)
        {
            var wait = Wait(10);

            // Cerrar cualquier cosa que esté estorbando antes de abrir
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            System.Threading.Thread.Sleep(500);

            // PASO 1: Abrir el calendario específico que le pasamos
            IWebElement btnCal = wait.Until(ExpectedConditions.ElementExists(btnCalendario));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCal);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCal);
            System.Threading.Thread.Sleep(1000);

            // PASO 2: Clic en el botón superior (el triangulito) 
            By btnTriangulo = By.CssSelector("button.mat-calendar-period-button");
            IWebElement triangulo = wait.Until(ExpectedConditions.ElementExists(btnTriangulo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", triangulo);
            System.Threading.Thread.Sleep(1000);

            // PASO 3: Clic en el Año (Con Navegación Inteligente Futuro/Pasado)
            By celdaAnio = By.XPath($"//div[normalize-space()='{anio}']");

            int targetYear = int.Parse(anio);
            int currentYear = DateTime.Now.Year; // Toma el año en el que estamos ejecutando la prueba
            int intentos = 0;

            // Bucle Ninja: Si no ve el año, decide para qué lado ir
            while (driver.FindElements(celdaAnio).Count == 0 && intentos < 5)
            {
                // Si el año buscado es mayor, flecha derecha. Si es menor, flecha izquierda.
                string selectorFlecha = targetYear > currentYear ? "button.mat-calendar-next-button" : "button.mat-calendar-previous-button";

                IWebElement btnFlecha = driver.FindElement(By.CssSelector(selectorFlecha));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnFlecha);
                System.Threading.Thread.Sleep(800);
                intentos++;
            }

            IWebElement elementAnio = wait.Until(ExpectedConditions.ElementExists(celdaAnio));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elementAnio);
            System.Threading.Thread.Sleep(1000);

            // ==========================================
            // ¡ESTO ERA LO QUE TE FALTABA!
            // ==========================================

            // PASO 4: Clic en el Mes 
            By celdaMes = By.XPath($"//div[normalize-space()='{mes}']");
            IWebElement elementMes = wait.Until(ExpectedConditions.ElementExists(celdaMes));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elementMes);
            System.Threading.Thread.Sleep(1000);

            // PASO 5: Clic en el Día 
            By celdaDia = By.XPath($"//div[normalize-space()='{dia}']");
            IWebElement elementDia = wait.Until(ExpectedConditions.ElementExists(celdaDia));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elementDia);

            System.Threading.Thread.Sleep(1000);
        }

















        // Método para listas desplegables (Tipo de Motor y Actividades)
        public void SeleccionarDeLista(string etiquetaCampo, string valorASeleccionar)
        {
            var wait = Wait();
            // 1. Clic en la lista desplegable correspondiente
            By desplegable = By.XPath($"//mat-label[contains(translate(text(), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), '{etiquetaCampo.ToUpper()}')]/ancestor::mat-form-field//mat-select");
            IWebElement select = wait.Until(ExpectedConditions.ElementToBeClickable(desplegable));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", select);
            System.Threading.Thread.Sleep(1000);

            // 2. Clic en la opción deseada dentro del panel que se abre
            By opcion = By.XPath($"//mat-option[.//span[contains(normalize-space(), '{valorASeleccionar}')]]");
            IWebElement opt = wait.Until(ExpectedConditions.ElementToBeClickable(opcion));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opt);
            System.Threading.Thread.Sleep(500);

            // Presionamos Escape por si es un selector múltiple y necesita cerrarse
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
        }

        public void GuardarCatalogo()
        {
            var wait = Wait();
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardarCatalogo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(3000); // Esperar que guarde y cierre
        }



        //

        // =============================
        // VALIDACIONES DE ERRORES / ALERTAS
        // =============================

        /* public bool ValidarErrorFechasSolapadas()
         {
             var wait = Wait(10);
             try
             {
                 // En Angular, los errores suelen aparecer en un rol de 'alert', un snack-bar (toast), o en una etiqueta mat-error roja.
                 // Este XPath cubre todas esas posibilidades.
                 By alertaError = By.XPath("//div[@role='alert'] | //snack-bar-container | //div[contains(@class, 'toast-message')] | //mat-error");

                 IWebElement alerta = wait.Until(ExpectedConditions.ElementIsVisible(alertaError));

                 // Imprimimos el texto exacto del error en consola para que quede evidencia en el reporte
                 Console.WriteLine($"[INFO]: El sistema bloqueó la acción con el mensaje: '{alerta.Text}'");

                 return true; // Sí apareció el error
             }
             catch (WebDriverTimeoutException)
             {
                 return false; // No apareció ningún error tras 10 segundos
             }
         }*/

        public bool ValidarErrorFechasSolapadas()
        {
            var wait = Wait(10);
            try
            {
                // Buscamos cualquier notificación que salte (sea verde o roja)
                By alertaGlobal = By.XPath("//div[@role='alert'] | //snack-bar-container | //div[contains(@class, 'toast-message')] | //mat-error");

                IWebElement alerta = wait.Until(ExpectedConditions.ElementIsVisible(alertaGlobal));

                // Extraemos el texto y lo pasamos a minúsculas para que sea fácil de comparar
                string textoMensaje = alerta.Text.ToLower();
                Console.WriteLine($"[INFO SISTEMA]: El sistema mostró el mensaje -> '{alerta.Text}'");

                // ==========================================
                // DETECTOR DE MENTIRAS (Filtro de Éxito)
                // ==========================================
                // Si el mensaje tiene palabras felices, sabemos que es la cajita verde.
                if (textoMensaje.Contains("éxito") ||
                    textoMensaje.Contains("exitosamente") ||
                    textoMensaje.Contains("correctamente") ||
                    textoMensaje.Contains("guardado"))
                {
                    Console.WriteLine("[INFO QA]: Es un mensaje de éxito verde. Ignorando falsa alarma de error.");
                    // Le damos un segundo para que el mensaje verde desaparezca y no estorbe al siguiente paso
                    System.Threading.Thread.Sleep(1500);
                    return false; // Devolvemos FALSE porque NO hay error.
                }

                // Si no tenía palabras felices, entonces efectivamente es el error rojo de solapamiento
                Console.WriteLine("[INFO QA]: Se confirmó que es un mensaje de error rojo.");
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                // Si pasaron 10 segundos y no salió absolutamente nada, tampoco hay error
                return false;
            }
        }


        public void BuscarYDarDeBaja(string fInicio, string fFin)
        {
            var wait = Wait(10);

            // 1. Llenar Fecha Inicio y Fecha Fin en los filtros de la cabecera
            By inputInicio = By.XPath("(//thead//input)[1]");
            IWebElement filtroInicio = wait.Until(ExpectedConditions.ElementToBeClickable(inputInicio));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", filtroInicio);
            filtroInicio.SendKeys(Keys.Control + "a");
            filtroInicio.SendKeys(Keys.Delete);
            filtroInicio.SendKeys(fInicio);

            By inputFin = By.XPath("(//thead//input)[2]");
            IWebElement filtroFin = wait.Until(ExpectedConditions.ElementToBeClickable(inputFin));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", filtroFin);
            filtroFin.SendKeys(Keys.Control + "a");
            filtroFin.SendKeys(Keys.Delete);
            filtroFin.SendKeys(fFin);

            // 2. ¡La magia del Auto-Filtro! Esperamos 3 segundos a que Angular actualice la tabla
            System.Threading.Thread.Sleep(3000);

            // 3. Buscamos el botón de la Lupita (Ver Detalle) en la fila de resultados
            By btnLupita = By.XPath("//button[.//mat-icon[normalize-space()='search']] | //mat-icon[normalize-space()='search']");
            var elementosLupita = driver.FindElements(btnLupita);

            // 4. VALIDACIÓN DEL BUG FANTASMA
            if (elementosLupita.Count == 0)
            {
                // Si no hay lupita, es porque la tabla está vacía. ¡Freno de mano y error de QA!
                Assert.Fail($"[BUG DETECTADO - FILTRO ROTO]: Se guardó el catálogo, pero al filtrar automáticamente por '{fInicio}' y '{fFin}', el registro no aparece en la grilla.");
            }

            // 5. Si sí lo encontró, hacemos clic en la Lupita para abrir el detalle
            IWebElement lupita = elementosLupita[0];
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupita);
            System.Threading.Thread.Sleep(2000); // Esperamos a que abra la pantalla con el detalle

            // 6. Ahora que estamos adentro, buscamos el tacho de basura rojo y lo clickeamos
            By btnEliminar = By.XPath("//button[contains(@class, 'mat-warn') or contains(@class, 'btn-danger')] | //button[.//mat-icon[contains(text(), 'delete')]]");
            IWebElement btnTacho = wait.Until(ExpectedConditions.ElementToBeClickable(btnEliminar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnTacho);
            System.Threading.Thread.Sleep(1500);

            // 7. Confirmación (Si el sistema pregunta "¿Está seguro?")
            try
            {
                By btnConfirmar = By.XPath("//button[contains(normalize-space(), 'Aceptar') or contains(normalize-space(), 'Sí') or contains(normalize-space(), 'Si')]");
                IWebElement btnSi = wait.Until(ExpectedConditions.ElementToBeClickable(btnConfirmar));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnSi);
                System.Threading.Thread.Sleep(2000);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("No apareció mensaje de confirmación, el catálogo se dio de baja directamente.");
            }
        }

    

        public bool ValidarBotonGuardarDeshabilitado()
        {
            var wait = Wait(10);

            // 1. XPath a prueba de balas: Buscamos cualquier botón que diga "Guardar"
            By btnGuardarExacto = By.XPath("//button[contains(normalize-space(), 'Guardar')]");
            IWebElement btn = wait.Until(ExpectedConditions.ElementExists(btnGuardarExacto));

            // 2. Le preguntamos directamente al núcleo del navegador usando JavaScript (infalible en Angular)
            bool isDisabledViaJS = (bool)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].disabled || arguments[0].hasAttribute('disabled');", btn);

            // 3. Revisamos si Angular le pegó su clase especial de bloqueo
            string clases = btn.GetAttribute("class");
            bool isDisabledViaClass = clases != null && (clases.Contains("mat-button-disabled") || clases.Contains("disabled"));

            // 4. Verificación nativa clásica de Selenium
            bool isEnabledNatively = btn.Enabled;

            // Si CUALQUIERA de nuestros 3 detectores dice que está bloqueado, entonces es True.
            bool estaDeshabilitado = isDisabledViaJS || isDisabledViaClass || !isEnabledNatively;

            // Dejamos una pista en la consola por si necesitamos investigar después
            Console.WriteLine($"[INFO BOTÓN GUARDAR] JS dice bloqueado: {isDisabledViaJS} | Clase dice bloqueado: {isDisabledViaClass} | Selenium Nativo dice habilitado: {isEnabledNatively}");

            return estaDeshabilitado;
        }



        // 1. Método para buscar y SOLO abrir la lupa (sin eliminar)
        public void BuscarYAbrirDetalle(string fInicio, string fFin)
        {
            var wait = Wait(10);

            // Filtramos usando la misma lógica robusta
            By inputInicio = By.XPath("(//thead//input)[1]");
            IWebElement filtroInicio = wait.Until(ExpectedConditions.ElementToBeClickable(inputInicio));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", filtroInicio);
            filtroInicio.SendKeys(Keys.Control + "a");
            filtroInicio.SendKeys(Keys.Delete);
            filtroInicio.SendKeys(fInicio);

            By inputFin = By.XPath("(//thead//input)[2]");
            IWebElement filtroFin = wait.Until(ExpectedConditions.ElementToBeClickable(inputFin));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", filtroFin);
            filtroFin.SendKeys(Keys.Control + "a");
            filtroFin.SendKeys(Keys.Delete);
            filtroFin.SendKeys(fFin);

            System.Threading.Thread.Sleep(3000);

            // Buscamos y abrimos la lupa
            By btnLupita = By.XPath("//button[.//mat-icon[normalize-space()='search']] | //mat-icon[normalize-space()='search']");
            var elementosLupita = driver.FindElements(btnLupita);

            if (elementosLupita.Count == 0)
            {
                Assert.Fail($"[BUG DETECTADO]: No se encontró el catálogo con fechas '{fInicio}' - '{fFin}' para poder editarlo.");
            }

            IWebElement lupita = elementosLupita[0];
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupita);
            System.Threading.Thread.Sleep(2000);
        }

        // 2. Método para hacer clic en el lápiz amarillo (Editar)
        public void ClicEditarCatalogo()
        {
            var wait = Wait(10);
            // Buscamos el botón amarillo de edición por su clase o por su ícono 'edit'
            By btnEditar = By.XPath("//button[contains(@class, 'btn-warning') or .//mat-icon[normalize-space()='edit']]");

            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnEditar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(1500); // Esperamos a que los campos se habiliten
        }

        // 3. Método para borrar una actividad específica de la tablita interna
        public void EliminarActividadFila(int fila)
        {
            var wait = Wait(10);
            // Buscamos el tacho de basura que está específicamente dentro de la fila indicada
            By btnEliminarFila = By.XPath($"(//tbody//tr | //mat-row)[{fila}]//button[.//mat-icon[normalize-space()='delete'] or contains(@class, 'btn-danger')]");

            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnEliminarFila));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(1000);
        }


        public void BuscarPrimerCatalogoPorEstadoYEditar(string estadoBuscado)
        {
            var wait = Wait(15); // Le subimos a 15 segundos por si la grilla demora un poquito en cargar

            // 1. XPath Inmortal (Case-Insensitive)
            // Convertimos tu palabra a minúsculas y le decimos a XPath que convierta todo el HTML a minúsculas temporalmente para comparar.
            string estadoMin = estadoBuscado.ToLower();
            By filaEstado = By.XPath($"(//tbody//tr[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), '{estadoMin}')])[1]");

            IWebElement fila = wait.Until(ExpectedConditions.ElementExists(filaEstado));

            // 2. Buscamos la lupa DENTRO de esa fila específica (usando el punto inicial .//)
            By btnLupa = By.XPath(".//button[.//mat-icon[normalize-space()='search']] | .//mat-icon[normalize-space()='search']");
            IWebElement lupa = fila.FindElement(btnLupa);

            // Le damos clic con JS que nunca falla en Angular
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupa);
            System.Threading.Thread.Sleep(2000); // Esperar a que abra el modal de detalle

            // 3. Ya en el detalle, damos clic en el lápiz amarillo (Editar)
            By btnEditar = By.XPath("//button[contains(@class, 'btn-warning') or .//mat-icon[normalize-space()='edit']]");
            IWebElement btnEdit = wait.Until(ExpectedConditions.ElementToBeClickable(btnEditar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnEdit);

            System.Threading.Thread.Sleep(1500); // Esperar a que se habiliten los campos para editar
        }

        // Método para leer el estado de la primera fila después de guardar
        public string ObtenerEstadoPrimerRegistro()
        {
            var wait = Wait(10);
            System.Threading.Thread.Sleep(3000); // Pausa vital para que la grilla recargue tras guardar

            // Buscamos la columna de estado en la primera fila (ajustamos para que lea el texto visible)
            By celdaEstado = By.XPath("(//tbody//tr)[1]//td[contains(@class, 'cdk-column-estado') or position()=8]");

            try
            {
                IWebElement estado = wait.Until(ExpectedConditions.ElementIsVisible(celdaEstado));
                return estado.Text.Trim().ToUpper();
            }
            catch (Exception)
            {
                return "ESTADO NO ENCONTRADO";
            }
        }



        public void ClicBotonClonar()
        {
            var wait = Wait(10);
            // Buscamos el botón celeste de Clonar dentro del modal de detalle
            By btnClonar = By.XPath("//button[contains(normalize-space(), 'Clonar')] | //span[normalize-space()='Clonar']");
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnClonar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(1500); // Esperamos que cargue el modal de clonación
        }


        public void BuscarPrimerCatalogoPorEstadoYClonar(string estadoBuscado)
        {
            var wait = Wait(15);

            // 1. Convertimos la palabra a minúsculas para que el XPath no falle
            string estadoMin = estadoBuscado.ToLower();
            By filaEstado = By.XPath($"(//tbody//tr[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), '{estadoMin}')])[1]");

            IWebElement fila = wait.Until(ExpectedConditions.ElementExists(filaEstado));

            // 2. Buscamos la lupa DENTRO de esa fila específica
            By btnLupa = By.XPath(".//button[.//mat-icon[normalize-space()='search']] | .//mat-icon[normalize-space()='search']");
            IWebElement lupa = fila.FindElement(btnLupa);

            // Le damos clic con JS 
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupa);
            System.Threading.Thread.Sleep(2000); // Esperar a que abra el modal de detalle

            // 3. Ya en el detalle, damos clic en el botón celeste de Clonar
            By btnClonar = By.XPath("//button[contains(normalize-space(), 'Clonar')] | //span[normalize-space()='Clonar']");
            IWebElement btnClone = wait.Until(ExpectedConditions.ElementToBeClickable(btnClonar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnClone);

            System.Threading.Thread.Sleep(1500); // Esperamos a que cargue el modal de clonación
        }


        // =============================
        // BORRAR ACTIVIDADES (CASO CLONACIÓN NEGATIVA)
        // =============================
        public void EliminarTodasLasActividades()
        {
            var wait = Wait(10);

            // Utilizamos exactamente el XPath que me proporcionaste
            By tachoPrimerFila = By.XPath("//tbody/tr[1]/td[3]/div[1]/div[1]/button[1]/mat-icon[1]");

            // Mientras el sistema encuentre al menos 1 tachito de basura en la tabla...
            while (driver.FindElements(tachoPrimerFila).Count > 0)
            {
                IWebElement btnTacho = driver.FindElement(tachoPrimerFila);

                // Clic forzado con JS (ideal para los mat-icon de Angular)
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnTacho);

                // Le damos 1 segundo a Angular para que borre la fila de la interfaz antes de buscar la siguiente
                System.Threading.Thread.Sleep(1000);
            }

            // Hacemos un clic fuera de la tabla (ej. en el título) por si Angular necesita 
            // perder el foco para validar y bloquear el botón Guardar
            try
            {
                By tituloModal = By.XPath("//h2 | //mat-dialog-title");
                IWebElement titulo = driver.FindElement(tituloModal);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", titulo);
            }
            catch (Exception) { }

            System.Threading.Thread.Sleep(1000); // Pausa final para que se actualice el estado del botón Guardar
        }


        

        // =============================
        // PREPARACIÓN DE FILTROS DE ESTADO (CP074 - CP075)
        // =============================

        public void PrepararFiltrosYSeleccionarEstado(string estadoBuscado)
        {
            var wait = Wait(10);

            // Aseguramos que la página ya haya pintado los checkboxes
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.TagName("mat-checkbox")));

            // ==============================================================
            // PASO 1: APAGAR FILTROS GENERALES (Clasificadores y Estado)
            // ==============================================================
            // Aprovechamos tu descubrimiento: los checkboxes maestros de Angular 
            // no tienen texto, por lo que tienen la clase 'no-side-margin'.
            By checkboxesMaestros = By.XPath("//mat-checkbox[.//span[contains(@class, 'no-side-margin')]]");
            var maestros = driver.FindElements(checkboxesMaestros);

            foreach (var chk in maestros)
            {
                // Verificamos el estado: ¿Tiene Angular la clase 'mat-checkbox-checked'?
                bool estaMarcado = chk.GetAttribute("class").Contains("mat-checkbox-checked");

                // SOLO si está encendido, le damos clic para apagarlo
                if (estaMarcado)
                {
                    IWebElement clickeable = chk.FindElement(By.TagName("label"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", clickeable);
                    System.Threading.Thread.Sleep(500); // Pausa corta para la animación visual
                }
            }

            // ==============================================================
            // PASO 2: ENCENDER EL ESTADO ESPECÍFICO ("VIGENTE" o "CADUCADO")
            // ==============================================================
            // Estos sí tienen el texto internamente, así que los buscamos por su palabra
            var todosLosCheckboxes = driver.FindElements(By.TagName("mat-checkbox"));
            bool encontrado = false;

            foreach (var chk in todosLosCheckboxes)
            {
                string textoVisible = chk.Text.Trim().ToUpper();

                // Si encontramos el que dice "VIGENTE" o "CADUCADO"
                if (textoVisible == estadoBuscado.ToUpper())
                {
                    encontrado = true;
                    bool estaMarcado = chk.GetAttribute("class").Contains("mat-checkbox-checked");

                    // SOLO si está apagado, le damos clic para encenderlo
                    if (!estaMarcado)
                    {
                        IWebElement clickeable = chk.FindElement(By.TagName("label"));
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", clickeable);
                        System.Threading.Thread.Sleep(500);
                    }
                    break; // Ya hicimos lo que queríamos, salimos del bucle
                }
            }

            // Si el robot escanea todo y no encuentra la palabra exacta, avisa
            if (!encontrado)
            {
                throw new Exception($"[ERROR DE AUTOMATIZACIÓN]: No se encontró en pantalla ningún checkbox con el texto '{estadoBuscado}'.");
            }
        }

        public void ClicBotonBuscarPrincipal()
        {
            var wait = Wait(10);
            // Buscamos el botón azul de Buscar por su texto o por el ícono
            By btnBuscar = By.XPath("//button[contains(translate(., 'buscar', 'BUSCAR'), 'BUSCAR') or contains(@class, 'btn-primary')]");

            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnBuscar));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(3000); // Pausa OBLIGATORIA para que la grilla traiga los nuevos resultados
        }

        public void ValidarColumnaGrilla(string nombreColumna, string valorEsperado)
        {
            var wait = Wait(10);

            // 1. Buscamos en qué posición está la columna que queremos evaluar
            var cabeceras = driver.FindElements(By.XPath("//thead//th"));
            int indiceColumna = -1;

            for (int i = 0; i < cabeceras.Count; i++)
            {
                if (cabeceras[i].Text.Trim().ToUpper() == nombreColumna.ToUpper())
                {
                    indiceColumna = i + 1; // Los XPath empiezan en 1
                    break;
                }
            }

            if (indiceColumna == -1)
            {
                Assert.Fail($"[ERROR DE AUTOMATIZACIÓN]: No existe la columna '{nombreColumna}' en la tabla.");
            }

            // 2. Extraemos las filas del cuerpo de la tabla
            var filas = driver.FindElements(By.XPath("//tbody//tr"));

            if (filas.Count == 0)
            {
                Assert.Fail("[BUG DETECTADO]: La tabla se quedó totalmente en blanco (0 filas) después de hacer clic en Buscar.");
                return;
            }

            bool revisoAlMenosUnDato = false;

            // 3. Recorremos fila por fila de forma segura
            foreach (var fila in filas)
            {
                // Contamos cuántas celdas tiene esta fila en particular
                var celdas = fila.FindElements(By.XPath("./td"));

                // Si la fila tiene menos celdas que nuestra columna 8, es una fila de "Sin resultados" o una fila fantasma
                if (celdas.Count < indiceColumna)
                {
                    // Comprobamos si es el mensaje de tabla vacía
                    if (celdas.Count == 1 && celdas[0].Text.ToLower().Contains("no se encontra"))
                    {
                        Assert.Fail($"[BUG DETECTADO - FILTRO ROTO]: Se buscó por '{valorEsperado}', pero el sistema devolvió un mensaje de 'No se encontraron resultados'.");
                    }
                    // Si no es el mensaje de vacío, simplemente la ignoramos (filas raras de Angular)
                    continue;
                }

                // Extraemos el texto de la celda específica
                string textoCelda = celdas[indiceColumna - 1].Text.Trim().ToUpper();

                // Angular a veces pone filas extrañas vacías. Si está vacía, la ignoramos.
                if (string.IsNullOrEmpty(textoCelda)) continue;

                revisoAlMenosUnDato = true;

                // ¡AQUÍ ES DONDE ESTALLARÁ ATRAPANDO EL BUG SI MUESTRA DATOS INCORRECTOS!
                Assert.AreEqual(valorEsperado.ToUpper(), textoCelda,
                    $"[BUG DETECTADO - EL FILTRO NO FUNCIONA]: Se aplicó el filtro '{valorEsperado}', pero la tabla mostró un registro en estado '{textoCelda}'.");
            }

            // Si recorrió toda la tabla y no pudo leer ni un solo dato válido
            if (!revisoAlMenosUnDato)
            {
                Assert.Fail($"[BUG DETECTADO]: Tras buscar '{valorEsperado}', la tabla no mostró ningún dato válido en la columna '{nombreColumna}'.");
            }
        }









    }




}