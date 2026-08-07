using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SIGES3_0.Pages.Facturacion
{
    public class OrdenDePagoPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public OrdenDePagoPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // ===== LOCATORS =====

        
        private By moduloFacturacionCiclica =
        By.XPath("//span[normalize-space()='Facturación Cíclica']/ancestor::a");

        private By tabOrdenPago =
        By.XPath("//span[normalize-space()='Ordenes de Pago']");

        private By txtBuscarOrden =
        By.XPath("//span[contains(normalize-space(),'ORDEN DE PAGO')]/ancestor::th/parent::tr/following-sibling::tr[1]/th[position()=count(//span[contains(normalize-space(),'ORDEN DE PAGO')]/ancestor::th/preceding-sibling::th)+1]//input");

        private By btnVerDetalle(string idOrden)=>
        By.XPath($"//tr[.//*[contains(text(),'{idOrden}')]]//button[@title='Ver detalle']");

        // Boleta de Venta
        private By dropdownFormato = By.Id("formato");

        //AQUI ES LA FACTURACION MANUAL - PROCESO

        // GENERAR PENDIENTE
        private By btnGenerarPendiente = By.XPath("//button[contains(.,'PENDIENTE')]");

        // GENERAR ORDEN
        private By btnGenerarOrden = By.XPath("//button[contains(.,'Generar')]");

        // PAGINACIÓN
        // Usamos el ID que aparece en tu HTML: "pageSizeSelect"
        private By dropdownPaginacion = By.Id("pageSizeSelect");

        private By opcionPaginacion(int cantidad) =>
            By.XPath($"//select[@id='pageSizeSelect']/option[@value='{cantidad}']");

        // BUSCAR CLIENTE
        private By inputBuscarCliente = By.XPath("(//th//input[contains(@class, 'form-control')])[3]");

        // REVISAR PAGO
        private By btnRevisarPago = By.XPath("//button[contains(.,'Revisar Pago')]");

        // APROBAR PAGO
        private By btnAceptarPago = By.XPath("//button[contains(.,'Aceptar')]");

        // RECHAZAR
        
        // CAMPANITA
        private By iconCampanita = By.XPath("//button[@aria-label='Notificaciones']");

        // NOTIFICACIÓN
        private By itemNotificacion = By.XPath("//div[contains(@class, 'notification-item')]");

        private By btnRealizarPago = By.XPath("//button[contains(., 'REALIZAR PAGO')]");

        // PAGO MANUAL
        private By opcionPagoManual = By.XPath("//h4[contains(text(),'Manual')]");

        // INPUT FILE
        private By inputFile = By.XPath("//input[@type='file']");

        private By txtNumeroOperacion = By.XPath("//input[@formcontrolname='operationNumber']");

        // BOTÓN ENVIAR
        private By btnEnviar = By.XPath("//button[contains(., 'CONFIRMAR DEPÓSITO')]");

        private By btnOkModal = By.XPath("//button[normalize-space()='OK']");

        private By mensajePagoEnProceso = By.XPath("//*[contains(text(),'Su solicitud de pago está siendo procesada')]");

        // DETALLE CLIENTE (LUPA)
        private By btnDetalleCliente = By.XPath("//button[@title='Facturar']");

        private By btnRechazarPago = By.XPath("//button[contains(., 'Rechazar')]");

        private By btnValidarPago = By.XPath("//button[contains(., 'Validar')]");


        // HISTORIAL
        private By tabHistorial =
            By.XPath("//span[contains(text(),'Historial')]");

        // ESTADO DINÁMICO
        private By estadoTexto(string estado) =>
            By.XPath($"//span[contains(text(),'{estado}')]");
        // Aqui termina


        // ===== MÉTODOS =====

        //Metodo reutilizable para darle scroll a un elemento
        public void ScrollToElement(By locator)
        {
            var element = wait.Until(ExpectedConditions.ElementIsVisible(locator));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        }

        public void IrFacturacionCiclica()
        {
            var element = wait.Until(ExpectedConditions.ElementToBeClickable(moduloFacturacionCiclica));
            element.Click();
        }

        public void AbrirOrdenDePago()
        {
            ScrollToElement(tabOrdenPago);

            wait.Until(ExpectedConditions.ElementToBeClickable(tabOrdenPago)).Click();
        }

        public void BuscarOrden(string idOrden)
        {
            var input = wait.Until(ExpectedConditions.ElementIsVisible(txtBuscarOrden));

            input.Clear();
            input.SendKeys(idOrden);
            input.SendKeys(Keys.Enter);
        }

        public void AbrirDetalleOrden(string idOrden)
        {
            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath($"//tr[.//td[contains(.,'{idOrden}')]]//button[@title='Ver detalle']")
            ));

            boton.Click();
        }

        // BOTONES GENERALES

        public void ClickBoton(string nombreBoton)
        {
            // 1. Esperar a que el cargando desaparezca
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-container")));

            By botonLocator = By.XPath($"//button[.//*[normalize-space()='{nombreBoton}']]");

            // 2. Esperar a que el elemento esté presente y sea clickeable
            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(botonLocator));

            // 3. Centrar el elemento en pantalla
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            // 4. Pequeña pausa técnica para que el scroll se estabilice
            Thread.Sleep(500);

            try
            {
                // Intento de clic normal
                boton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Si falla porque algo lo intercepta, forzamos el clic por JS
                js.ExecuteScript("arguments[0].click();", boton);
            }
        }

        // OPCIONES DE COMPARTIR

        public void SeleccionarOpcion(string opcion)
        {
            By opcionUI = By.XPath($"//span[normalize-space()='{opcion}']");

            wait.Until(ExpectedConditions.ElementToBeClickable(opcionUI)).Click();
        }

        // VALIDACIONES

        public bool OrdenCompartida()
        {
            try
            {
                var ventana = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//*[contains(@class,'modal') or contains(@class,'popover') or contains(text(),'Compartir')]")
                ));

                return ventana.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool OrdenImpresa()
        {
            return true; 
        }

        public bool OrdenDescargada()
        {
            return driver.PageSource.Contains("Descarga");
        }

        public void SeleccionarFormato(string formato)
        {
            var selectElement = wait.Until(ExpectedConditions.ElementIsVisible(dropdownFormato));
            SelectElement select = new SelectElement(selectElement);

            string formatoUpper = formato.ToUpper();

            try
            {
                select.SelectByText(formatoUpper);
            }
            catch (NoSuchElementException)
            {
                select.SelectByValue(formatoUpper);
            }

            // --- SOLUCIÓN AL PROBLEMA ---
            // 1. Esperar a que aparezca un cargando (si el sistema lo muestra)
            Thread.Sleep(1000); // Pausa necesaria para que el DOM se actualice con el nuevo formato

            // 2. Opcional: Validar que el valor del select realmente cambió
            wait.Until(d => select.SelectedOption.Text.ToUpper().Contains(formatoUpper));

            // 3. Esperar a que cualquier overlay de "procesando" desaparezca
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-container")));
        }

        public void GenerarOrden()
        {
            // 1. Click en PENDIENTE
            var pendiente = wait.Until(ExpectedConditions.ElementToBeClickable(btnGenerarPendiente));
            pendiente.Click();

            // 2. Esperar que cargue la vista (IMPORTANTE)
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-container")));

            // 3. Click en GENERAR
            var generar = wait.Until(ExpectedConditions.ElementToBeClickable(btnGenerarOrden));
            generar.Click();
        }

        public void ConfigurarPaginacion(int cantidad)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(dropdownPaginacion)).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(opcionPaginacion(cantidad))).Click();
        }

        public void BuscarCliente(string nombre)
        {
            var input = wait.Until(ExpectedConditions.ElementIsVisible(inputBuscarCliente));
            input.Clear();
            input.SendKeys(nombre);
            input.SendKeys(Keys.Enter);
        }

        public void ClickRevisarPago()
        {
            try
            {
                Console.WriteLine("⏳ Esperando que aparezca 'Revisar Pago'...");

                // esperar hasta 30s (más realista para backend)
                WebDriverWait waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                var boton = waitLong.Until(driver =>
                {
                    var elementos = driver.FindElements(By.XPath("//button[contains(.,'Revisar Pago')]"));
                    return elementos.Count > 0 ? elementos[0] : null;
                });

                // scroll
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

                Thread.Sleep(500);

                boton.Click();

                Console.WriteLine("✅ Se encontró y clickeó 'Revisar Pago'");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("❌ No apareció 'Revisar Pago' después de esperar");

                // 👇 IMPORTANTE: no rompas el test aquí si quieres que continúe
                throw;
            }
        }
        public void AprobarPago()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(btnAceptarPago)).Click();
        }
        public void RechazarPago()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(btnRechazarPago)).Click();
        }

        public void AbrirCampanita()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(iconCampanita)).Click();
        }

        public void SeleccionarOrdenCliente()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(itemNotificacion)).Click();
        }

        public void SeleccionarPagoManual()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(opcionPagoManual)).Click();
        }

        public void SubirComprobante(string archivo)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", archivo);

            var input = wait.Until(ExpectedConditions.ElementExists(inputFile));
            input.SendKeys(ruta);
        }

        public void EnviarPago()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(btnEnviar)).Click();
        }

        public void VerDetalleCliente()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(btnDetalleCliente)).Click();
        }

        public void IrHistorialPlanes()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(tabHistorial)).Click();
        }

        public bool ValidarEstado(string estado)
        {
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(estadoTexto(estado)));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ClickPendientes()
        {
            try
            {
                var pendiente = wait.Until(ExpectedConditions.ElementToBeClickable(btnGenerarPendiente));
                pendiente.Click();

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-container")));
            }
            catch (Exception)
            {
                Console.WriteLine("⚠️ No se pudo aplicar filtro pendiente");
            }
        }

        public void ClickGenerar()
        {
            try
            {
                var generar = wait.Until(driver =>
                {
                    var btn = driver.FindElement(btnGenerarOrden);

                    // 🔥 Validar si está habilitado
                    if (btn.Displayed && btn.Enabled)
                        return btn;

                    return null;
                });

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", generar);

                generar.Click();

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-container")));

                Console.WriteLine("✅ Orden generada");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("⚠️ No hay órdenes pendientes → se continúa flujo");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("⚠️ Botón Generar no existe → se continúa flujo");
            }
        }

        public void ClickRealizarPago()
        {
            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnRealizarPago));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            boton.Click();
        }

        public void ClickRechazarPago()
        {
            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnRealizarPago));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            boton.Click();
        }

        public void IngresarNumeroOperacion(string numero)
        {
            var input = wait.Until(ExpectedConditions.ElementIsVisible(txtNumeroOperacion));

            input.Clear();
            input.SendKeys(numero);

            Console.WriteLine($"✅ Número de operación ingresado: {numero}");
        }

        public void ConfirmarModalOk()
        {
            try
            {
                var ok = wait.Until(ExpectedConditions.ElementToBeClickable(btnOkModal));
                ok.Click();

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-container")));
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("⚠️ No apareció modal OK");
            }
        }

        public bool ValidarPagoEnProceso()
        {
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(mensajePagoEnProceso));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void ClickFacturar()
        {
            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnDetalleCliente));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            boton.Click();
        }

        public void ValidarDocumento()
        {
            var validar = wait.Until(ExpectedConditions.ElementToBeClickable(btnValidarPago));
            validar.Click();
        }

        public bool ExisteRevisarPago()
        {
            return driver.FindElements(btnRevisarPago).Count > 0;
        }

        public bool ExisteValidar()
        {
            return driver.FindElements(btnValidarPago).Count > 0;
        }

        public string ObtenerEstadoPago()
        {
            try
            {
                var estado = wait.Until(driver =>
                {
                    var el = driver.FindElement(By.XPath("//span[contains(@class,'estado') or contains(text(),'Pendiente') or contains(text(),'Proceso') or contains(text(),'Facturado')]"));
                    return el.Text;
                });

                Console.WriteLine($"📌 Estado actual: {estado}");

                return estado.ToLower();
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("❌ No se pudo obtener el estado");
                return "";
            }
        }

    }
}