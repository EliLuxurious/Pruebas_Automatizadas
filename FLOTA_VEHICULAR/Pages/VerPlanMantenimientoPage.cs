using FLOTA_VEHICULAR.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;

namespace FLOTA_VEHICULAR.Pages
{
    public class VerPlanMantenimientoPage
    {
        private IWebDriver driver;
        Utilities utilities;
        private string ultimoMensajeSistema = "";

        public VerPlanMantenimientoPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        WebDriverWait Wait(int seconds = 20)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }

        // =============================
        // XPATHS - NAVEGACIÓN
        // =============================

        // Reutilizamos el desplegable principal de Mantenimiento
        private By moduloMantenimiento = By.XPath("//span[@class='mat-expansion-indicator ng-tns-c243-9 ng-trigger ng-trigger-indicatorRotate ng-star-inserted'] | //mat-panel-title[contains(., 'Mantenimiento')]");

        // XPath indestructible por texto para el nuevo submódulo
        private By submoduloVerPlanMantenimientos = By.XPath("//div[normalize-space()='Ver Plan Mantenimientos'] | //span[contains(normalize-space(), 'Ver Plan Mantenimientos')] | //a[contains(., 'Ver Plan Mantenimientos')]");

        // =============================
        // MÉTODOS - NAVEGACIÓN
        // =============================

        public void IngresarSubmoduloVerPlanMantenimientos()
        {
            var wait = Wait();
            System.Threading.Thread.Sleep(2000); // Esperar que cargue el menú lateral

            // 1. Clic en el acordeón principal 'Mantenimiento' (Si está cerrado)
            try
            {
                IWebElement modulo = wait.Until(ExpectedConditions.ElementExists(moduloMantenimiento));
                // Verificamos si el submódulo ya es visible para no cerrar el acordeón por error
                var subMenus = driver.FindElements(submoduloVerPlanMantenimientos);
                if (subMenus.Count == 0 || !subMenus[0].Displayed)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", modulo);
                    System.Threading.Thread.Sleep(1500); // Animación de despliegue
                }
            }
            catch (Exception) { /* Ignoramos si ya estaba abierto o interactuable */ }

            // 2. Clic en 'Ver Plan Mantenimientos'
            IWebElement submodulo = wait.Until(ExpectedConditions.ElementExists(submoduloVerPlanMantenimientos));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submodulo);

            System.Threading.Thread.Sleep(2000); // Pausa para que cargue la nueva grilla
        }

        // ==========================================
        // XPATHS - REGISTRO DE PLAN
        // ==========================================
        private By btnNuevoPlan = By.XPath("//button[.//mat-icon[normalize-space()='add'] or contains(., '+ NUEVO')]");

        // Buscamos las cajas de texto a través de sus etiquetas visuales (labels)
        private By txtRUC = By.XPath("//mat-label[contains(translate(., 'ruc', 'RUC'), 'RUC')]/ancestor::mat-form-field//input | //input[contains(@placeholder, 'RUC')]");
        //private By btnLupaRUC = By.XPath("//mat-label[contains(translate(., 'ruc', 'RUC'), 'RUC')]/ancestor::mat-form-field//button[.//mat-icon[normalize-space()='search']] | //button[.//mat-icon[normalize-space()='search']]");
        // XPath infalible usando la etiqueta personalizada de Angular
        //private By btnLupaRUC = By.XPath("//cad-search-print//button | //button[.//mat-icon[contains(text(), 'search')]]");
        // XPath blindado que obliga al robot a buscar SOLO dentro del modal
        private By btnLupaRUC = By.XPath("//mat-dialog-container//cad-search-print//button | //mat-dialog-container//button[.//mat-icon[normalize-space()='search']]");

        private By txtDireccion = By.XPath("//mat-label[contains(translate(., 'dirección', 'DIRECCIÓN'), 'DIRECCIÓN')]/ancestor::mat-form-field//input | //input[contains(@placeholder, 'DIRECCIÓN')]");
        private By txtNumContrato = By.XPath("//mat-label[contains(translate(., 'contrato', 'CONTRATO'), 'CONTRATO')]/ancestor::mat-form-field//input | //input[contains(@placeholder, 'CONTRATO')]");

        private By btnCalendarioDesde = By.XPath("(//mat-dialog-container//mat-datepicker-toggle//button)[1]");
        private By btnCalendarioHasta = By.XPath("(//mat-dialog-container//mat-datepicker-toggle//button)[2]");

        private By txtFechaDesde = By.XPath("(//mat-dialog-container//input[contains(@class, 'mat-datepicker-input')])[1]");
        private By txtFechaHasta = By.XPath("(//mat-dialog-container//input[contains(@class, 'mat-datepicker-input')])[2]");

        // Elementos para el archivo y guardado
        private By btnAgregarDocumento = By.XPath("//button[contains(., 'Agregar documento')] | //button[contains(@class, 'button-Upload')]");
        private By inputSubirArchivo = By.XPath("//input[@type='file']"); // Oculto en el DOM de Angular, se usa para subir archivos directamente
                                                                          // private By btnGuardarPlan = By.XPath("//button[contains(@class, 'tsp-button-success') and contains(., 'Guardar')] | //button[.//span[contains(text(), 'Guardar')]]");
                                                                          // XPath blindado que busca el botón Guardar dentro del modal, sin importar de qué color sea
        private By btnGuardarPlan = By.XPath("//mat-dialog-container//button[contains(translate(., 'guardar', 'GUARDAR'), 'GUARDAR')]");
        // Elemento clave para validar que entramos a la pantalla correcta
        private By tituloPantalla = By.XPath("//div[contains(translate(., 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), 'PLANES DE MANTENIMIENTO')] | //h1[contains(., 'PLANES DE MANTENIMIENTO')]");


        // =============================
        // XPATHS - APROBAR PLAN
        // =============================

        private By filtroNumeroContrato = By.XPath("//th[4]//input[1]");

        private By btnLupaResultadoPlan = By.XPath("//tbody/tr[1]/td[6]//button");

        private By btnAgregarVehiculo = By.XPath("//button[@mattooltip='Agregar vehículos' or contains(., 'VEHICULOS') or contains(., 'VEHÍCULOS')]");

        private By chkVehiculo1 = By.XPath("//tbody/tr[3]/td[1]//p-tablecheckbox//div[contains(@class,'p-checkbox-box')]");
        private By chkVehiculo2 = By.XPath("//tbody/tr[6]/td[1]//p-tablecheckbox//div[contains(@class,'p-checkbox-box')]");

       

        private By btnAprobarPlan = By.XPath("//button[contains(@class,'button-aprovedPlan') and contains(.,'Aprobar')] | //button[contains(.,'Aprobar')]");

        private By btnConfirmarAprobacion = By.XPath("//mat-dialog-container//button[contains(.,'Confirmar')] | //button[contains(.,'Confirmar')]");

        private By btnCerrarDetallePlan = By.XPath("//button[contains(.,'Cerrar')] | //mat-icon[normalize-space()='close']");

        private By estadoPrimerResultado = By.XPath("//tbody/tr[1]/td[5]");

        private By btnCerrarVistaDetalle = By.XPath("(//button[contains(., 'Cerrar')])[last()]");


        // =============================
        // XPATHS - CREAR MANTENIMIENTO PREVENTIVO
        // =============================


        private By toastMantenimientoRegistrado = By.XPath(
            "//div[@role='alert'] | " +
            "//p-toastitem | " +
            "//div[contains(@class,'p-toast')] | " +
            "//div[contains(@class,'p-toast-summary')] | " +
            "//div[contains(@class,'p-toast-detail')] | " +
            "//*[contains(text(),'MANTENIMIENTO') and contains(text(),'REGISTRADO')] | " +
            "//*[contains(text(),'Mantenimiento') and contains(text(),'registrado')]"
        );


        // =============================
        // MODALES / OVERLAYS
        // =============================
        private By cualquierModal = By.CssSelector("mat-dialog-container");

        private By modalVehiculos = By.XPath(
            "//mat-dialog-container[.//*[contains(translate(normalize-space(.), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), 'VEHICULOS') " +
            "or contains(translate(normalize-space(.), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), 'VEHÍCULOS')]]"
        );

        private By modalMantenimientoPreventivo = By.XPath(
            "//mat-dialog-container[.//mat-label[contains(translate(normalize-space(.), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), 'CLASE DE MANTENIMIENTO')] " +
            "or .//*[contains(translate(normalize-space(.), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), 'MANTENIMIENTO PREVENTIVO')]]"
        );

        // Botón guardar SOLO dentro del modal de vehículos
        private By btnGuardarVehiculos = By.XPath(
    "(//mat-dialog-container//button[contains(normalize-space(.), 'Guardar')])[last()]"
);
        // Checkboxes visibles dentro del modal de vehículos
        private By checkboxesVehiculosModal = By.XPath(
            "//mat-dialog-container//tbody//p-tablecheckbox//div[contains(@class,'p-checkbox-box')]"
        );

        // Botón crear mantenimiento: mejor buscarlo dentro del detalle, no cualquier add final
        private By btnCrearMantenimientoPreventivo = By.XPath(
            "(//button[contains(translate(normalize-space(.), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), 'CREAR MANTENIMIENTO')]" +
            " | //button[.//mat-icon[normalize-space()='add'] and ancestor::*[contains(translate(normalize-space(.), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), 'MANTENIMIENTO')]])[last()]"
        );

        // Combo clase SOLO dentro del modal correcto
        private By comboClaseMantenimiento = By.XPath(
            "//mat-dialog-container//mat-label[contains(translate(normalize-space(.), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), 'CLASE DE MANTENIMIENTO')]" +
            "/ancestor::mat-form-field//mat-select"
        );

        private By txtFechaEjecucionPlanificada = By.XPath(
            "//mat-dialog-container//mat-label[contains(translate(normalize-space(.), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), 'FECHA EJECUCIÓN PLANIFICADA')]" +
            "/ancestor::mat-form-field//input"
        );

        private By btnGuardarMantenimientoPreventivo = By.XPath(
            "//mat-dialog-container[.//mat-label[contains(translate(normalize-space(.), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), 'CLASE DE MANTENIMIENTO')]]" +
            "//button[contains(translate(normalize-space(.), 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), 'GUARDAR')]"
        );




















        // ==========================================
        // LÓGICA DE CALENDARIOS RECICLADA Y MEJORADA
        // ==========================================
        /* private string ObtenerMesAngular(string mesNumero)
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
         }*/




        private string ObtenerMesAngular(string mesNumero)
         {
             switch (mesNumero.Trim())
             {
                 case "01": case "1": return "ENE";
                 case "02": case "2": return "FEB";
                 case "03": case "3": return "MAR";
                 case "04": case "4": return "ABR";
                 case "05": case "5": return "MAY";
                 case "06": case "6": return "JUN";
                 case "07": case "7": return "JUL";
                 case "08": case "8": return "AGO";
                 case "09": case "9": return "SET"; // Ojo aquí: a veces Angular usa SET en lugar de SEP
                 case "10": return "OCT";
                 case "11": return "NOV";
                 case "12": return "DIC";
                 default: return "ENE";
             }
         }












        /* private void SeleccionarEnCalendario(By btnCalendario, string dia, string mes, string anio)
         {
             var wait = Wait(10);
             new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
             System.Threading.Thread.Sleep(500);

             // PASO 1: Abrir el calendario específico (Desde o Hasta)
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

             // PASO 4: Clic en el Mes 
             By celdaMes = By.XPath($"//div[contains(@class, '{claseCalendario}') and contains(translate(., 'abcdefghijklmnopqrstuvwxyz.', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), '{mes}')]");
             IWebElement elementMes = wait.Until(ExpectedConditions.ElementExists(celdaMes));
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elementMes);
             System.Threading.Thread.Sleep(1000);

             // PASO 5: Clic en el Día 
             By celdaDia = By.XPath($"//div[contains(@class, '{claseCalendario}') and normalize-space()='{dia}']");
             IWebElement elementDia = wait.Until(ExpectedConditions.ElementExists(celdaDia));
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elementDia);
             System.Threading.Thread.Sleep(1000);
         }*/


        private void SeleccionarEnCalendario(By btnCalendario, By inputFecha, string fecha)
        {
            var wait = Wait(15);

            string[] partes = fecha.Split('/');
            string dia = int.Parse(partes[0]).ToString();
            string mes = ObtenerMesAngular(partes[1]);
            string anio = partes[2];

            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            System.Threading.Thread.Sleep(500);

            IWebElement btnCal = wait.Until(ExpectedConditions.ElementToBeClickable(btnCalendario));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCal);
            System.Threading.Thread.Sleep(500);
            btnCal.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector(".cdk-overlay-container mat-datepicker-content")
            ));

            IWebElement btnPeriodo = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector(".cdk-overlay-container button.mat-calendar-period-button")
            ));
            btnPeriodo.Click();

            string claseCelda = "mat-calendar-body-cell-content";

            By celdaAnio = By.XPath($"//div[contains(@class,'cdk-overlay-container')]//div[contains(@class,'{claseCelda}') and normalize-space()='{anio}']");

            int intentos = 0;
            while (driver.FindElements(celdaAnio).Count == 0 && intentos < 10)
            {
                IWebElement btnSiguiente = driver.FindElement(By.CssSelector(".cdk-overlay-container button.mat-calendar-next-button"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnSiguiente);
                System.Threading.Thread.Sleep(500);
                intentos++;
            }

            IWebElement elementAnio = wait.Until(ExpectedConditions.ElementToBeClickable(celdaAnio));
            elementAnio.Click();

            By celdaMes = By.XPath($"//div[contains(@class,'cdk-overlay-container')]//div[contains(@class,'{claseCelda}') and contains(translate(normalize-space(.), 'abcdefghijklmnopqrstuvwxyz.', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), '{mes}')]");

            IWebElement elementMes = wait.Until(ExpectedConditions.ElementToBeClickable(celdaMes));
            elementMes.Click();

            By celdaDia = By.XPath(
                $"//div[contains(@class,'cdk-overlay-container')]//td[not(contains(@class,'mat-calendar-body-disabled'))]//div[contains(@class,'{claseCelda}') and normalize-space()='{dia}']"
            );

            IWebElement elementDia = wait.Until(ExpectedConditions.ElementToBeClickable(celdaDia));

            try
            {
                elementDia.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elementDia);
            }

            IWebElement input = wait.Until(ExpectedConditions.ElementIsVisible(inputFecha));

            wait.Until(d =>
            {
                string valor = input.GetAttribute("value");
                return !string.IsNullOrWhiteSpace(valor);
            });

            Console.WriteLine($"[INFO]: Fecha seleccionada correctamente: {input.GetAttribute("value")}");
        }









        // ==========================================
        // MÉTODOS - ACCIONES DEL FORMULARIO
        // ==========================================
        /* public void ClicNuevoPlan()
         {
             var wait = Wait();
             IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnNuevoPlan));
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
             System.Threading.Thread.Sleep(2000);
         }*/


        public void ClicNuevoPlan()
        {
            var wait = Wait(15); // Le damos hasta 15 segundos para que la navegación termine

            // ¡EL FRENO DE MANO! Obligamos al robot a esperar hasta ver el título de la pantalla correcta
            // Si la pantalla de Vehículos sigue ahí, el robot esperará pacientemente.
            wait.Until(ExpectedConditions.ElementIsVisible(tituloPantalla));

            // Una vez que confirma que está en la página correcta, recién busca el botón
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnNuevoPlan));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            System.Threading.Thread.Sleep(2000); // Pausa para que el modal se abra completamente
        }

















        public void IngresarRUCYBuscar(string ruc)
        {
            var wait = Wait(10);

            // 1. Encontramos la caja y la limpiamos
            IWebElement input = wait.Until(ExpectedConditions.ElementToBeClickable(txtRUC));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", input);
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);

            // 2. Escribimos el RUC
            input.SendKeys(ruc);

            // TRUCO VITAL: Apretamos TAB para quitar el foco de la cajita. 
            // Esto obliga a Angular a registrar que ya terminamos de escribir los 11 dígitos.
            input.SendKeys(Keys.Tab);
            System.Threading.Thread.Sleep(1500); // Pausa breve para que Angular active el botón

            // 3. Clic en la Lupa
            IWebElement lupa = wait.Until(ExpectedConditions.ElementToBeClickable(btnLupaRUC));

            // Intentamos un clic natural de Selenium primero (suele llevarse mejor con Angular aquí)
            try
            {
                lupa.Click();
            }
            catch (Exception)
            {
                // Si algo lo bloquea, lo forzamos con JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", lupa);
            }

            // Esperamos 3 segundos a que el sistema consulte la Base de Datos/SUNAT y rellene la Razón Social
            System.Threading.Thread.Sleep(3000);
        }

        public void IngresarDireccion(string direccion)
        {
            var wait = Wait();
            IWebElement input = wait.Until(ExpectedConditions.ElementToBeClickable(txtDireccion));
            input.Clear();
            input.SendKeys(direccion);
        }

        public void IngresarNumeroContrato(string contrato)
        {
            var wait = Wait();
            IWebElement input = wait.Until(ExpectedConditions.ElementToBeClickable(txtNumContrato));
            input.Clear();
            input.SendKeys(contrato);
        }

        /*public void IngresarFechaDesde(string fecha)
         {
             string[] partes = fecha.Split('/');
             string dia = int.Parse(partes[0]).ToString();
             string mes = ObtenerMesAngular(partes[1]);
             string anio = partes[2];
             SeleccionarEnCalendario(btnCalendarioDesde, dia, mes, anio);
         }

         public void IngresarFechaHasta(string fecha)
         {
             string[] partes = fecha.Split('/');
             string dia = int.Parse(partes[0]).ToString();
             string mes = ObtenerMesAngular(partes[1]);
             string anio = partes[2];
             SeleccionarEnCalendario(btnCalendarioHasta, dia, mes, anio);
         }
        */


        public void IngresarFechaDesde(string fecha)
        {
            SeleccionarEnCalendario(btnCalendarioDesde, txtFechaDesde, fecha);
        }

        public void IngresarFechaHasta(string fecha)
        {
            SeleccionarEnCalendario(btnCalendarioHasta, txtFechaHasta, fecha);
        }







        public void SubirDocumento(string nombreArchivo)
        {
            // Ubicamos el archivo en la carpeta TestData
            string rutaCompleta = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\TestData", nombreArchivo));

            var wait = Wait(5);
            try
            {
                // TRUCO QA MAESTRO: Obligamos al navegador a hacer visible todos los inputs de archivos ocultos
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "document.querySelectorAll('input[type=file]').forEach(function(el) { " +
                    "el.style.display = 'block'; " +
                    "el.style.opacity = '1'; " +
                    "el.style.width = '100px'; " +
                    "el.style.height = '100px'; " +
                    "});"
                );
                System.Threading.Thread.Sleep(1000); // Pausa para que el código surta efecto

                // Ahora que es visible, le disparamos el archivo
                IWebElement inputUpload = driver.FindElement(By.XPath("//input[@type='file']"));
                inputUpload.SendKeys(rutaCompleta);

                System.Threading.Thread.Sleep(2000); // Esperamos a que la barrita de carga (si la hay) termine
                Console.WriteLine($"[EXITO]: Se adjuntó el documento: {nombreArchivo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ADVERTENCIA]: No se encontró el input de archivos oculto. Detalle: " + ex.Message);
            }
        }

        /* public void GuardarPlanMantenimiento()
         {
             var wait = Wait();
             IWebElement btn = wait.Until(ExpectedConditions.ElementExists(btnGuardarPlan));
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);
             System.Threading.Thread.Sleep(1000);

             IWebElement btnClickable = wait.Until(ExpectedConditions.ElementToBeClickable(btn));
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnClickable);
             System.Threading.Thread.Sleep(3000);
         }*/





        public void GuardarPlanMantenimiento()
        {
            var wait = Wait(10);
            ultimoMensajeSistema = "";

            IWebElement btn = wait.Until(ExpectedConditions.ElementExists(btnGuardarPlan));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);
            System.Threading.Thread.Sleep(500);

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);

            try
            {
                WebDriverWait waitToast = Wait(8);

                string textoToast = waitToast.Until(d =>
                {
                    var elementos = d.FindElements(By.XPath(
                        "//div[@role='alert']" +
                        " | //p-toastitem" +
                        " | //div[contains(@class,'p-toast-message')]" +
                        " | //div[contains(@class,'p-toast-summary')]" +
                        " | //div[contains(@class,'p-toast-detail')]"
                    ));

                    foreach (var elemento in elementos)
                    {
                        try
                        {
                            if (elemento.Displayed && !string.IsNullOrWhiteSpace(elemento.Text))
                            {
                                return elemento.Text.Trim();
                            }
                        }
                        catch
                        {
                            // Ignora elementos que desaparecen rápido
                        }
                    }

                    return null;
                });

                ultimoMensajeSistema = textoToast;
                Console.WriteLine($"[INFO SISTEMA]: Mensaje capturado al guardar -> '{ultimoMensajeSistema}'");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("[ERROR QA]: Se hizo clic en Guardar, pero no se logró capturar ningún toast.");
                ultimoMensajeSistema = "";
            }
        }


        public bool ValidarMensajeExito(string mensajeEsperado)
        {
            Console.WriteLine($"[INFO QA]: Mensaje esperado -> '{mensajeEsperado}'");
            Console.WriteLine($"[INFO QA]: Último mensaje capturado -> '{ultimoMensajeSistema}'");

            if (string.IsNullOrWhiteSpace(ultimoMensajeSistema))
            {
                return false;
            }

            string textoReal = ultimoMensajeSistema
                .ToLower()
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u");

            mensajeEsperado = mensajeEsperado
                .ToLower()
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u");

            return textoReal.Contains(mensajeEsperado)
                || textoReal.Contains("plan de mantenimiento registrado")
                || textoReal.Contains("se agrego correctamente el plan de mantenimiento");
        }



        public bool ValidarMensajeError(string mensajeEsperado)
        {
            Console.WriteLine($"[INFO QA]: Mensaje de error esperado -> '{mensajeEsperado}'");
            Console.WriteLine($"[INFO QA]: Último mensaje capturado -> '{ultimoMensajeSistema}'");

            if (string.IsNullOrWhiteSpace(ultimoMensajeSistema))
            {
                return false;
            }

            string textoReal = ultimoMensajeSistema
                .ToLower()
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u");

            mensajeEsperado = mensajeEsperado
                .ToLower()
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u");

            return textoReal.Contains(mensajeEsperado)
                || textoReal.Contains("plan de mantenimiento no registrado")
                || textoReal.Contains("numero de contrato existente");
        }




        public void BuscarPlanPorNumeroContrato(string contrato)
        {
            var wait = Wait(15);

            IWebElement input = wait.Until(ExpectedConditions.ElementToBeClickable(filtroNumeroContrato));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);
            input.Click();
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            input.SendKeys(contrato);

            // En tablas PrimeNG normalmente filtra al escribir, pero le damos un pequeño tiempo.
            System.Threading.Thread.Sleep(1500);

            Console.WriteLine($"[INFO]: Se buscó el plan con contrato: {contrato}");
        }

        public void AbrirDetallePlanEncontrado()
        {
            var wait = Wait(20);

            IWebElement btnLupa = wait.Until(ExpectedConditions.ElementToBeClickable(btnLupaResultadoPlan));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnLupa);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnLupa);

            // El detalle NO es mat-dialog-container.
            // Esperamos algo propio del detalle: botón + VEHICULOS.
            wait.Until(ExpectedConditions.ElementIsVisible(btnAgregarVehiculo));

            Console.WriteLine("[INFO]: Se abrió el detalle del plan correctamente.");
        }
        private void ClickHumano(IWebElement elemento)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView({block:'center', inline:'center'});",
                elemento
            );

            System.Threading.Thread.Sleep(500);

            try
            {
                new OpenQA.Selenium.Interactions.Actions(driver)
                    .MoveToElement(elemento)
                    .Pause(TimeSpan.FromMilliseconds(300))
                    .Click()
                    .Perform();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].dispatchEvent(new MouseEvent('mouseover', { bubbles: true }));" +
                    "arguments[0].dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));" +
                    "arguments[0].dispatchEvent(new MouseEvent('mouseup', { bubbles: true }));" +
                    "arguments[0].dispatchEvent(new MouseEvent('click', { bubbles: true }));",
                    elemento
                );
            }
        }
        public void AgregarVehiculosPorPlaca(string placa1, string placa2)
        {
            var wait = Wait(25);

            // 1. Abrir modal de vehículos
            IWebElement btnAgregar = wait.Until(ExpectedConditions.ElementToBeClickable(btnAgregarVehiculo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnAgregar);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnAgregar);

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("mat-dialog-container")));
            Console.WriteLine("[INFO]: Se abrió el modal de vehículos.");

            // 2. Seleccionar vehículo por placa 1
            SeleccionarVehiculoPorPlaca(placa1);

            // 3. Seleccionar vehículo por placa 2
            SeleccionarVehiculoPorPlaca(placa2);

            // 4. Guardar vehículos
            By btnGuardarVehiculos = By.XPath("(//mat-dialog-container//button[contains(normalize-space(.), 'Guardar')])[last()]");
            IWebElement btnGuardar = wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardarVehiculos));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnGuardar);
            System.Threading.Thread.Sleep(500);

            try
            {
                btnGuardar.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuardar);
            }

            Console.WriteLine("[INFO]: Se hizo clic en Guardar vehículos.");

            // 5. Esperar que cierre el modal
            try
            {
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("mat-dialog-container")));
                Console.WriteLine("[INFO]: Se guardaron los vehículos y se cerró el modal.");
            }
            catch
            {
                Console.WriteLine("[ADVERTENCIA QA]: El modal no se cerró automáticamente. Se intentará cerrar manualmente.");

                By btnCerrar = By.XPath("(//mat-dialog-container//button[contains(normalize-space(.), 'Cerrar')])[last()]");
                IWebElement cerrar = wait.Until(ExpectedConditions.ElementToBeClickable(btnCerrar));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", cerrar);

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("mat-dialog-container")));
                Console.WriteLine("[INFO]: Se cerró manualmente el modal de vehículos.");
            }

            System.Threading.Thread.Sleep(1500);
        }

        private void SeleccionarVehiculoPorPlaca(string placa)
        {
            var wait = Wait(20);

            By filaVehiculo = By.XPath(
                $"//mat-dialog-container//tbody/tr[.//td[contains(normalize-space(), '{placa}')]]"
            );

            IWebElement fila = wait.Until(ExpectedConditions.ElementIsVisible(filaVehiculo));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", fila);
            System.Threading.Thread.Sleep(500);

            By checkboxVehiculo = By.XPath(
                $"//mat-dialog-container//tbody/tr[.//td[contains(normalize-space(), '{placa}')]]//p-tablecheckbox//div[contains(@class,'p-checkbox-box')]"
            );

            IWebElement check = wait.Until(ExpectedConditions.ElementToBeClickable(checkboxVehiculo));

            try
            {
                check.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", check);
            }

            System.Threading.Thread.Sleep(500);

            Console.WriteLine($"[INFO]: Se seleccionó el vehículo con placa: {placa}");
        }
        public void AgregarVehiculosAlPlan()
        {
            var wait = Wait(25);

            // 1. Abrir modal de vehículos
            IWebElement btnAgregar = wait.Until(ExpectedConditions.ElementToBeClickable(btnAgregarVehiculo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnAgregar);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnAgregar);

            wait.Until(ExpectedConditions.ElementIsVisible(modalVehiculos));
            Console.WriteLine("[INFO]: Se abrió el modal de vehículos.");

            // 2. Seleccionar vehículos visibles
            var checks = wait.Until(d =>
            {
                var elementos = d.FindElements(checkboxesVehiculosModal);
                return elementos.Count >= 1 ? elementos : null;
            });

            int seleccionados = 0;

            foreach (var check in checks)
            {
                if (seleccionados == 2)
                    break;

                try
                {
                    if (check.Displayed && check.Enabled)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", check);
                        System.Threading.Thread.Sleep(300);
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", check);
                        seleccionados++;
                        System.Threading.Thread.Sleep(500);
                    }
                }
                catch
                {
                    // Ignoramos si algún checkbox cambia o desaparece.
                }
            }

            if (seleccionados == 0)
            {
                throw new Exception("[ERROR QA]: No se pudo seleccionar ningún vehículo visible en el modal.");
            }

            Console.WriteLine($"[INFO]: Vehículos seleccionados: {seleccionados}");

            // 3. Ubicar botón Guardar
            IWebElement btnGuardar = wait.Until(ExpectedConditions.ElementExists(btnGuardarVehiculos));

            string disabled = btnGuardar.GetAttribute("disabled");
            string ariaDisabled = btnGuardar.GetAttribute("aria-disabled");
            string claseBoton = btnGuardar.GetAttribute("class") ?? "";

            Console.WriteLine($"[DEBUG]: Botón Guardar vehículos - disabled: {disabled}, aria-disabled: {ariaDisabled}, class: {claseBoton}");

            if (disabled != null || ariaDisabled == "true" || claseBoton.ToLower().Contains("disabled"))
            {
                throw new Exception("[ERROR QA]: El botón Guardar vehículos está deshabilitado. Los vehículos parecen seleccionados, pero el sistema no habilitó el guardado.");
            }

            // 4. Clic más humano en Guardar
            ClickHumano(btnGuardar);

            Console.WriteLine("[INFO]: Se intentó hacer clic en Guardar vehículos.");

            // 5. Esperar respuesta del sistema
            try
            {
                wait.Until(d =>
                {
                    bool modalCerrado = d.FindElements(By.CssSelector("mat-dialog-container")).Count == 0;

                    bool apareceToast = d.FindElements(By.XPath(
                        "//div[@role='alert'] | //p-toastitem | //div[contains(@class,'p-toast')]"
                    )).Any(e =>
                    {
                        try
                        {
                            return e.Displayed && !string.IsNullOrWhiteSpace(e.Text);
                        }
                        catch
                        {
                            return false;
                        }
                    });

                    return modalCerrado || apareceToast;
                });

                Console.WriteLine("[INFO]: Luego de guardar vehículos, el sistema respondió correctamente.");
            }
            catch
            {
                throw new Exception("[ERROR QA]: Se hizo clic en Guardar vehículos, pero el sistema no cerró el modal ni mostró mensaje.");
            }

            // 6. Si apareció toast pero el modal sigue abierto, lo cerramos para continuar el flujo
            try
            {
                var modales = driver.FindElements(By.CssSelector("mat-dialog-container"));

                if (modales.Count > 0)
                {
                    Console.WriteLine("[ADVERTENCIA QA]: El sistema respondió, pero el modal de vehículos sigue abierto. Se cerrará manualmente.");

                    By btnCerrarVehiculos = By.XPath("(//mat-dialog-container//button[contains(normalize-space(.), 'Cerrar')])[last()]");
                    IWebElement btnCerrar = wait.Until(ExpectedConditions.ElementToBeClickable(btnCerrarVehiculos));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCerrar);

                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("mat-dialog-container")));

                    Console.WriteLine("[INFO]: Se cerró manualmente el modal de vehículos.");
                }
            }
            catch
            {
                throw new Exception("[ERROR QA]: El sistema respondió, pero no se pudo cerrar el modal de vehículos.");
            }

            System.Threading.Thread.Sleep(1500);

            Console.WriteLine("[INFO]: Se seleccionaron, guardaron y cerraron vehículos para el plan.");
        }

        /* public void AprobarPlanMantenimiento()
         {
             var wait = Wait(25);

             // Subimos arriba del detalle para ver el botón Aprobar
             ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);");
             System.Threading.Thread.Sleep(1000);

             IWebElement btnAprobar = wait.Until(ExpectedConditions.ElementToBeClickable(btnAprobarPlan));
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnAprobar);
             System.Threading.Thread.Sleep(500);
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnAprobar);

             // Aquí sí aparece modal de confirmación
             wait.Until(ExpectedConditions.ElementIsVisible(
                 By.XPath("//mat-dialog-container | //div[contains(.,'APROBAR PLAN')]")
             ));

             IWebElement btnConfirmar = wait.Until(ExpectedConditions.ElementToBeClickable(btnConfirmarAprobacion));
             System.Threading.Thread.Sleep(500);
             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnConfirmar);

             System.Threading.Thread.Sleep(2500);

             Console.WriteLine("[INFO]: Se confirmó la aprobación del plan.");

             // Cerramos el detalle del plan
             try
             {
                 IWebElement cerrar = wait.Until(ExpectedConditions.ElementToBeClickable(btnCerrarDetallePlan));
                 ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", cerrar);
                 System.Threading.Thread.Sleep(500);
                 ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", cerrar);
                 System.Threading.Thread.Sleep(1500);
             }
             catch
             {
                 Console.WriteLine("[INFO]: No se pudo cerrar con botón, quizá ya estaba cerrado.");
             }
         }*/

        public void AprobarPlanMantenimiento()
        {
            var wait = Wait(25);

            // Subimos arriba del detalle para ver el botón Aprobar
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);");
            System.Threading.Thread.Sleep(1000);

            IWebElement btnAprobar = wait.Until(ExpectedConditions.ElementToBeClickable(btnAprobarPlan));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnAprobar);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnAprobar);

            // Modal de confirmación
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//mat-dialog-container | //div[contains(.,'APROBAR PLAN')]")
            ));

            IWebElement btnConfirmar = wait.Until(ExpectedConditions.ElementToBeClickable(btnConfirmarAprobacion));
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnConfirmar);

            System.Threading.Thread.Sleep(2500);

            Console.WriteLine("[INFO]: Se confirmó la aprobación del plan.");

            // Cerrar con el botón rojo de abajo a la derecha
            CerrarVistaDetalleConBotonRojo();
        }

        public bool ValidarEstadoPlan(string estadoEsperado)
        {
            var wait = Wait(20);

            try
            {
                IWebElement estado = wait.Until(ExpectedConditions.ElementIsVisible(estadoPrimerResultado));

                string textoReal = estado.Text.Trim().ToUpper();
                string esperado = estadoEsperado.Trim().ToUpper();

                Console.WriteLine($"[INFO QA]: Estado real -> '{textoReal}'");
                Console.WriteLine($"[INFO QA]: Estado esperado -> '{esperado}'");

                return textoReal.Contains(esperado);
            }
            catch
            {
                Console.WriteLine("[ERROR QA]: No se pudo validar el estado del plan.");
                return false;
            }
        }


        public void CerrarVistaDetalleConBotonRojo()
        {
            var wait = Wait(20);

            try
            {
                // Bajamos hasta el final porque el botón Cerrar está abajo a la derecha
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                System.Threading.Thread.Sleep(1000);

                By btnCerrarRojo = By.XPath("(//button[contains(., 'Cerrar')])[last()]");

                IWebElement cerrar = wait.Until(ExpectedConditions.ElementToBeClickable(btnCerrarRojo));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", cerrar);
                System.Threading.Thread.Sleep(500);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", cerrar);

                // Esperamos regresar a la grilla principal
                wait.Until(ExpectedConditions.ElementIsVisible(filtroNumeroContrato));

                Console.WriteLine("[INFO]: Se cerró la vista aprobada con el botón rojo Cerrar y se volvió a la grilla principal.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR QA]: No se pudo cerrar la vista con el botón rojo Cerrar. Detalle: " + ex.Message);
                throw;
            }
        }


        public void IntentarAprobarPlanSinVehiculos()
        {
            var wait = Wait(25);

            // Subimos arriba porque el botón Aprobar está en la parte superior derecha
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);");
            System.Threading.Thread.Sleep(1000);

            IWebElement btnAprobar = wait.Until(ExpectedConditions.ElementToBeClickable(btnAprobarPlan));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnAprobar);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnAprobar);

            System.Threading.Thread.Sleep(1000);

            // Si aparece modal de confirmación, confirmamos.
            // Si el sistema valida antes y muestra toast directo, igual continuamos.
            try
            {
                By modalConfirmacion = By.XPath("//mat-dialog-container | //div[contains(.,'APROBAR PLAN')]");
                wait.Until(ExpectedConditions.ElementIsVisible(modalConfirmacion));

                IWebElement btnConfirmar = wait.Until(ExpectedConditions.ElementToBeClickable(btnConfirmarAprobacion));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnConfirmar);

                Console.WriteLine("[INFO]: Se confirmó intento de aprobación sin vehículos.");
            }
            catch
            {
                Console.WriteLine("[INFO]: No apareció modal de confirmación. Posiblemente el sistema mostró validación directa.");
            }

            System.Threading.Thread.Sleep(1500);
        }

        public bool ValidarMensajePlanSinVehiculos()
        {
            var wait = Wait(15);

            try
            {
                By alerta = By.XPath(
                    "//div[@role='alert']" +
                    " | //p-toastitem" +
                    " | //div[contains(@class,'p-toast')]" +
                    " | //div[contains(@class,'p-toast-summary')]" +
                    " | //div[contains(@class,'p-toast-detail')]" +
                    " | //div[contains(@class,'toast')]" +
                    " | //div[contains(text(),'vehiculo')]" +
                    " | //div[contains(text(),'vehículo')]" +
                    " | //div[contains(text(),'mantenimiento')]" +
                    " | //div[contains(text(),'asociado')]"
                );

                IWebElement mensaje = wait.Until(ExpectedConditions.ElementIsVisible(alerta));
                string textoReal = mensaje.Text.Trim();

                Console.WriteLine($"[INFO SISTEMA]: Mensaje mostrado al aprobar sin vehículos -> '{textoReal}'");

                string texto = textoReal
                    .ToLower()
                    .Replace("á", "a")
                    .Replace("é", "e")
                    .Replace("í", "i")
                    .Replace("ó", "o")
                    .Replace("ú", "u");

                return texto.Contains("vehiculo")
                    || texto.Contains("mantenimiento")
                    || texto.Contains("asociado")
                    || texto.Contains("debe")
                    || texto.Contains("no se puede")
                    || texto.Contains("no registrado")
                    || texto.Contains("no aprobado");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("[ERROR QA]: No se encontró mensaje de validación para plan sin vehículos.");
                return false;
            }
        }

        public void CrearMantenimientoPreventivo(string clase, string fechaEjecucion)
        {
            var wait = Wait(25);

            // Seguridad: si quedó algún overlay o modal viejo, no seguimos a ciegas.
            EsperarQueNoHayaOverlay(10);

            IWebElement btnCrear = wait.Until(ExpectedConditions.ElementToBeClickable(btnCrearMantenimientoPreventivo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnCrear);
            System.Threading.Thread.Sleep(700);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCrear);

            // Ahora sí esperamos el modal correcto, no cualquier mat-dialog-container.
            wait.Until(ExpectedConditions.ElementIsVisible(modalMantenimientoPreventivo));
            wait.Until(ExpectedConditions.ElementIsVisible(comboClaseMantenimiento));

            Console.WriteLine("[INFO]: Se abrió correctamente el modal de mantenimiento preventivo.");

            SeleccionarClaseMantenimiento(clase);

            IngresarFechaEjecucionMantenimiento(fechaEjecucion);

            IWebElement btnGuardar = wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardarMantenimientoPreventivo));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnGuardar);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuardar);

            System.Threading.Thread.Sleep(1500);

            Console.WriteLine("[INFO]: Se hizo clic en Guardar mantenimiento preventivo.");
        }

        private void SeleccionarClaseMantenimiento(string clase)
        {
            var wait = Wait(20);

            IWebElement combo = wait.Until(ExpectedConditions.ElementToBeClickable(comboClaseMantenimiento));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", combo);
            System.Threading.Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", combo);

            By opcionClase = By.XPath(
                $"//div[contains(@class,'cdk-overlay-container')]//mat-option//span[normalize-space()='{clase}'] | " +
                $"//div[contains(@class,'cdk-overlay-container')]//mat-option[contains(normalize-space(), '{clase}')]"
            );

            IWebElement opcion = wait.Until(ExpectedConditions.ElementToBeClickable(opcionClase));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opcion);

            System.Threading.Thread.Sleep(700);

            Console.WriteLine($"[INFO]: Se seleccionó la clase de mantenimiento: {clase}");
        }




        private void IngresarFechaEjecucionMantenimiento(string fecha)
        {
            var wait = Wait(15);

            IWebElement inputFecha = wait.Until(ExpectedConditions.ElementToBeClickable(txtFechaEjecucionPlanificada));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", inputFecha);
            System.Threading.Thread.Sleep(500);

            inputFecha.Click();
            inputFecha.SendKeys(Keys.Control + "a");
            inputFecha.SendKeys(Keys.Delete);
            inputFecha.SendKeys(fecha);
            inputFecha.SendKeys(Keys.Tab);

            // Disparamos eventos para que Angular reconozca la fecha
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].dispatchEvent(new Event('input', { bubbles: true }));" +
                "arguments[0].dispatchEvent(new Event('change', { bubbles: true }));" +
                "arguments[0].dispatchEvent(new Event('blur', { bubbles: true }));",
                inputFecha
            );

            System.Threading.Thread.Sleep(800);

            Console.WriteLine($"[INFO]: Se ingresó la fecha de ejecución del mantenimiento: {fecha}");
        }


        public bool ValidarMensajeMantenimientoRegistrado()
        {
            var wait = Wait(20);

            try
            {
                IWebElement alerta = wait.Until(ExpectedConditions.ElementIsVisible(toastMantenimientoRegistrado));
                string textoReal = alerta.Text.Trim();

                Console.WriteLine($"[INFO SISTEMA]: Mensaje al registrar mantenimiento -> '{textoReal}'");

                string texto = Normalizar(textoReal);

                bool mensajeCorrecto = texto.Contains("mantenimiento")
                    && (
                        texto.Contains("registrado")
                        || texto.Contains("registrados")
                        || texto.Contains("exitosamente")
                        || texto.Contains("exitosa")
                    );

                if (!mensajeCorrecto)
                    return false;

                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(modalMantenimientoPreventivo));
                    Console.WriteLine("[INFO]: El modal de mantenimiento preventivo se cerró correctamente.");
                }
                catch
                {
                    Console.WriteLine("[ADVERTENCIA QA]: El mantenimiento se registró, pero el modal quedó abierto.");
                }

                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("[ERROR QA]: No se encontró mensaje de mantenimiento registrado.");
                return false;
            }
        }

        private void EsperarCierreDeModal(int segundos = 15)
        {
            var wait = Wait(segundos);

            wait.Until(d =>
            {
                try
                {
                    var modales = d.FindElements(cualquierModal);
                    foreach (var modal in modales)
                    {
                        if (modal.Displayed)
                            return false;
                    }
                    return true;
                }
                catch
                {
                    return true;
                }
            });
        }

        private void EsperarQueNoHayaOverlay(int segundos = 15)
        {
            var wait = Wait(segundos);

            wait.Until(d =>
            {
                var overlays = d.FindElements(By.CssSelector(".cdk-overlay-backdrop, .cdk-overlay-pane"));
                foreach (var overlay in overlays)
                {
                    try
                    {
                        if (overlay.Displayed)
                            return false;
                    }
                    catch { }
                }
                return true;
            });
        }

        private string Normalizar(string texto)
        {
            if (texto == null) return "";

            return texto.Trim()
                .ToLower()
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u");
        }















    }

}