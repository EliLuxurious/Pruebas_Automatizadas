using SIGES3_0.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace SIGES3_0.Pages
{
    public class NuevaAdquisicionPage
    {
        private IWebDriver driver;
        private Utilities utilities;

        public NuevaAdquisicionPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // 1. ZONA DE SELECTORES (Locators)
        private By usernameField = By.Id("floatingInput");
        private By passwordField = By.Id("floatingInputPassword");
        private By loginButton = By.XPath("//button[normalize-space()='Ingresar']");
        private By logo = By.XPath("//img[@alt='Logo']");

        private By moduloAdquisicion = By.XPath("//span[normalize-space()='Adquisición']");
        private By submoduloNuevaAdquisicion = By.XPath("//span[normalize-space()='Nueva Adquisición']");

        // --- 1. Facturación ---
        private By panelFacturacion = By.Id("heading-collapse-facturación");
        private By cmbDocumento = By.XPath("//select[@id='documentTypeId']");
        private By txtSerie = By.XPath("//input[@id='serieName']");
        private By txtCorrelativo = By.XPath("//input[@id='correlative']");
        private By txtFechaEmision = By.XPath("//input[@id='date']");
        private By txtProveedor = By.XPath("//input[@id='client-search']");
        private By txtInfoAdicional = By.XPath("//textarea[@id='additionalInfo']");

        // --- 2. Entrega ---
        private By panelEntrega = By.Id("heading-collapse-entrega");
        private By btnEntregaInmediata = By.XPath("//input[@id='tipoBien']");
        private By btnEntregaDiferida = By.XPath("//input[@id='tipoServicio']");
        private By chkSeleccionarAlmacen = By.XPath("//input[@id='slectWarehouse']");
        // En la sección de selectores de Entrega
        private By chkVariosAlmacenes = By.XPath("//input[@id='severalWarehouses']");
        private By cmbRol = By.XPath("//span[text()='Rol']/following-sibling::app-dropdown-search//div[contains(@class, 'select-trigger')]");
        private By cmbEstablecimiento = By.XPath("//span[text()='Establecimiento']/following-sibling::app-dropdown-search//div[contains(@class, 'select-trigger')]");
        private By cmbAlmacen = By.XPath("//span[text()='Almacén']/following-sibling::app-dropdown-search//div[contains(@class, 'select-trigger')]");
        private By txtBusquedaDropdown = By.XPath("//input[@placeholder='Buscar...'] | //input[contains(@class, 'select-search')]");

        // --- 2.1. Tipo dee Compra 

        // --- 2.1. Tipo de Compra ---
        private By rbCompraG = By.XPath("//label[normalize-space()='G']/preceding-sibling::input");
        private By rbCompraNG = By.XPath("//label[normalize-space()='NG']/preceding-sibling::input");
        private By rbCompraGyNG = By.XPath("//label[normalize-space()='G Y NG']/preceding-sibling::input");

        // --- 3. Productos ---
        private By cmbBuscarConcepto = By.XPath("//span[contains(text(), 'Seleccionar un concepto')]");
        private By opcionProductoLista = By.XPath("//span[normalize-space()='7751234001115|Azúcar Rubia']");
        private By txtCantidad = By.XPath("//tbody/tr[@class='ng-star-inserted']/td[3]/div[1]/input[1]");
        private By txtValorUnitario = By.XPath("//tbody/tr[@class='ng-star-inserted']/td[4]/div[1]/input[1]");

        // --- 3.1. Producto cuando es con Varios Almacenes 
        private By opcion1ProductoLista = By.XPath("//span[normalize-space()='7751234001122|Azúcar Blanca']");
        // Estos reemplazan a tus actuales variosRol y variosAlmacen
        private By variosRol = By.XPath("//tbody/tr[contains(@class,'ng-star-inserted')]//td[3]//select");
        private By variosAlmacen = By.XPath("//tbody/tr[contains(@class,'ng-star-inserted')]//td[4]//select");
        private By txtCantidadTabla = By.XPath("//tbody/tr[contains(@class,'ng-star-inserted')]//td[5]//input");
        private By txtValorUnitarioTabla = By.XPath("//tbody/tr[contains(@class,'ng-star-inserted')]//td[6]//input");

        // --- 3.2. Botones de Activación ---
        private By chkHabilitarDescuento = By.XPath("//input[@id='discount']"); // El que marcas al inicio del video
        private By txtDescuentoItemTabla = By.XPath("//tbody/tr[contains(@class,'ng-star-inserted')]/td[6]//input");
        private By btnDescuentoGlobal = By.XPath("//button[normalize-space()='Global']");
        private By txtInputDescuentoGlobal = By.XPath("//tbody/tr[@class='ng-star-inserted']/td[2]/div[1]/input[1]");

        // --- 4. SECCIÓN DE PAGO ---
        private By panelPago = By.XPath("//button[contains(@class, 'accordion-button') and contains(., 'PaymentMethod.Title')]");

        private By rbPagoContado = By.XPath("//input[@id='contado']");

        private By rbPagoCredito = By.XPath("//input[@id='credito']");
        private By txtMontoInicial = By.XPath("//input[@formcontrolname='initialPayment']");
        private By txtNumeroCuotas = By.XPath("//input[@formcontrolname='numberOfInstallments']");
        private By txtFrecuenciaDias = By.XPath("//input[@formcontrolname='frequencyDays']");

        private By btnMultiPago = By.XPath("//input[@id='checkTypePaymentMethod']");

        // --- Medios de Pago (Pestañas) ---
        // Efectivo 
        private By tabEfectivo = By.XPath("//button[normalize-space()='Efectivo']");
        private By txtObservacionEfectivo = By.XPath("//input[@id='observacion']");

        // Billetera Digital
        private By tabBilleteraDigital = By.XPath("//button[normalize-space()='Billetera digital']");
        private By dropdownBilletera = By.XPath("//select[contains(@class, 'form-select')]");
        private By inputTransactionCode = By.XPath("//label[contains(normalize-space(), 'TransactionCode')]/following-sibling::input | //label[contains(normalize-space(), 'TransactionCode')]/following-sibling::div//input");
        private By txtObservacionBilletera = By.XPath("//label[contains(normalize-space(), 'Observation')]/following-sibling::textarea | //label[contains(normalize-space(), 'Observation')]/following-sibling::div//textarea");

        //Transferencia en cuenta
        private By tabTransferencia = By.XPath("//button[normalize-space()='Transferencia en cuenta']");
        //Deposito en cuenta
        private By tabDeposito = By.XPath("//button[contains(text(),'Dep��sito en cuenta')]");
        private By cmbCaja = By.Id("checkout"); 
        private By cmbCuentaPropia = By.Id("ownBankAccount"); 
        private By txtCuentaProveedor = By.Id("numberBankAccount"); 
        private By txtInfoPagoGeneral = By.Id("information"); 

        // Tarjeta de Credito
        private By tabTarjetaCredito = By.XPath("//button[contains(text(),'Tarjeta de crǸdito')]");
        //Tarjeta de Debito
        private By tabTarjetaDebito = By.XPath("//button[contains(text(),'Tarjeta de dǸbito')]");
        private By cmbSeleccionarTarjeta = By.Id("bankingCard");
        private By txtInfoTarjeta = By.Id("information");

        // VALIDACION
        private By alertaError = By.XPath("//div[contains(@class, 'toast-error')] | //div[contains(@class, 'alert-danger')] | //div[contains(@class, 'invalid-feedback')] | //div[@role='alert']");

        //--- 5. Guardar
        private By btnGuardarAdquisicion = By.XPath("//button[contains(text(), 'Guardar Adquisición')]");
        private By btnOkModal = By.XPath("//button[contains(@class, 'ok-button')]");

        // 2. ZONA DE MÉTODOS

        public void OpenToApplication(string url)
        {
            driver.Navigate().GoToUrl(url);
            Thread.Sleep(4000);
        }

        public void LoginToApplication(string _username, string _password)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45)); // Espera larga por si acaso

            utilities.EnterText(usernameField, _username);
            utilities.EnterText(passwordField, _password);
            utilities.ClickButton(loginButton);

            // ESTO es lo que evita el error del logo
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(logo));
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("❌ El logo no apareció después del login. La página no cargó a tiempo.");
            }
        }

        public void NavegarANuevaAdquisicion()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement btnModulo = wait.Until(ExpectedConditions.ElementExists(moduloAdquisicion));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click(); arguments[0].parentNode.click();", btnModulo);
            Thread.Sleep(2000);
            IWebElement btnSubmodulo = wait.Until(ExpectedConditions.ElementExists(submoduloNuevaAdquisicion));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click(); arguments[0].parentNode.click();", btnSubmodulo);
            Thread.Sleep(3000);
        }

        public void ConfigurarDatosFacturacion(string documento, string serie, string correlativo, string fechaEmision, string proveedor, string infoAdicional)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            IWebElement btnFacturacion = wait.Until(ExpectedConditions.ElementExists(panelFacturacion));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnFacturacion);
            Thread.Sleep(1500);

            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(panelFacturacion)).Click();
            }
            catch (Exception)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnFacturacion);
            }

            IWebElement comboDoc = wait.Until(ExpectedConditions.ElementIsVisible(cmbDocumento));

            utilities.EnterText(cmbDocumento, documento);
            Thread.Sleep(1000);
            comboDoc.SendKeys(Keys.Enter);

            if (!string.IsNullOrEmpty(serie))
            {
                wait.Until(ExpectedConditions.ElementIsVisible(txtSerie));
                utilities.EnterText(txtSerie, serie);
            }

            if (!string.IsNullOrEmpty(correlativo))
            {
                utilities.EnterText(txtCorrelativo, correlativo);
            }

            utilities.EnterText(txtFechaEmision, fechaEmision);

            // 7. Proveedor (Manejo de búsqueda/autocompletado)
            IWebElement campoProveedor = wait.Until(ExpectedConditions.ElementIsVisible(txtProveedor));
            utilities.ClearAndEnterText(txtProveedor, proveedor);
            Thread.Sleep(2000);
            campoProveedor.SendKeys(Keys.Enter);

            try
            {
                IWebElement errorVisual = driver.FindElement(By.XPath("//input[@id='client-search'][contains(@class, 'is-invalid')] | //small[contains(@class, 'text-danger')]"));
                Console.WriteLine("⚠️ El sistema detectó un problema con el proveedor: " + errorVisual.Text);
            }
            catch (NoSuchElementException) { /* Si no hay error, todo bien, seguimos */ }

            utilities.EnterText(txtInfoAdicional, infoAdicional);
        }
        public void SeleccionarTipoEntrega(string tipoEntrega)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            // 1. Scroll y apertura del acordeón
            IWebElement panelBtn = wait.Until(ExpectedConditions.ElementExists(panelEntrega));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", panelBtn);
            Thread.Sleep(1000);

            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(panelEntrega)).Click();
            }
            catch (Exception)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", panelBtn);
            }
            Thread.Sleep(2000);

            // 2. Selección del Radio Button
            if (tipoEntrega.Equals("Inmediata", StringComparison.OrdinalIgnoreCase))
            {
                IWebElement radioInmediata = wait.Until(ExpectedConditions.ElementExists(btnEntregaInmediata));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radioInmediata);
            }
            else if (tipoEntrega.Equals("Diferida", StringComparison.OrdinalIgnoreCase))
            {
                IWebElement radioDiferida = wait.Until(ExpectedConditions.ElementExists(btnEntregaDiferida));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radioDiferida);

                // --- ¡ESTO ES LO QUE FALTABA! ---
                // Esperamos un momento a que aparezca el checkbox "Seleccionar Almacén" y le damos clic
                Thread.Sleep(1500);
                IWebElement checkboxAlmacen = wait.Until(ExpectedConditions.ElementExists(chkSeleccionarAlmacen));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkboxAlmacen);
            }

            Thread.Sleep(2000); // Pausa necesaria para que carguen los combos de Rol y Establecimiento
        }

        public void ConfigurarDatosEntrega(string rol, string establecimiento, string almacen)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(25));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // --- 1. SELECCIONAR ROL ---
            SeleccionarEnDropdownNuevo(cmbRol, rol);

            // --- 2. SELECCIONAR ESTABLECIMIENTO ---
            if (!string.IsNullOrEmpty(establecimiento))
            {
                SeleccionarEnDropdownNuevo(cmbEstablecimiento, establecimiento);
            }

            // --- 3. SELECCIONAR ALMACÉN ---
            if (!string.IsNullOrEmpty(almacen))
            {
                SeleccionarEnDropdownNuevo(cmbAlmacen, almacen);
            }
        }

        // Método auxiliar para reutilizar la lógica de clic en el resultado
        private void SeleccionarEnDropdownNuevo(By locatorTrigger, string valorABuscar)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // 1. Clic para abrir el combo
            IWebElement trigger = wait.Until(ExpectedConditions.ElementExists(locatorTrigger));
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", trigger);
            Thread.Sleep(1000);
            js.ExecuteScript("arguments[0].click();", trigger);

            // 2. Escribir en el buscador
            IWebElement input = wait.Until(ExpectedConditions.ElementIsVisible(txtBusquedaDropdown));
            input.Clear();
            input.SendKeys(valorABuscar);

            // 3. ESPERA A QUE APAREZCA EL RESULTADO (Crucial para el video)
            Thread.Sleep(3000);

            // 4. CLIC REAL EN LA OPCIÓN FILTRADA
            // Buscamos el elemento que contiene el texto exacto dentro de la lista de resultados
            By locatorOpcion = By.XPath($"//div[contains(@class, 'select-results')]//*[contains(text(), '{valorABuscar}')] | //div[contains(@class, 'options')]//*[contains(text(), '{valorABuscar}')]");

            try
            {
                IWebElement opcion = wait.Until(ExpectedConditions.ElementToBeClickable(locatorOpcion));
                js.ExecuteScript("arguments[0].click();", opcion);
                Thread.Sleep(1500);
            }
            catch (Exception)
            {
                // Si el clic falla, intentamos el Enter como último recurso
                input.SendKeys(Keys.Enter);
                Thread.Sleep(1500);
            }
        }
        public void ActivarVariosAlmacenes(bool activar)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement checkbox = wait.Until(ExpectedConditions.ElementExists(chkVariosAlmacenes));
            bool estaMarcado = checkbox.Selected;

            if (activar && !estaMarcado)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
                Thread.Sleep(1000); // Tiempo para que la tabla de productos se actualice
            }
            else if (!activar && estaMarcado)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
            }
        }

        public void SeleccionarTipoCompra(string tipo)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            By locator;

            By rbCompraG = By.XPath("//label[normalize-space()='G']/preceding-sibling::input");
            By rbCompraNG = By.XPath("//label[normalize-space()='NG']/preceding-sibling::input");
            By rbCompraGyNG = By.XPath("//label[normalize-space()='G Y NG']/preceding-sibling::input");

            switch (tipo.Trim().ToUpper())
            {
                case "G": locator = rbCompraG; break;
                case "NG": locator = rbCompraNG; break;
                case "G Y NG": locator = rbCompraGyNG; break;
                default: throw new ArgumentException($"Tipo de compra '{tipo}' no reconocido.");
            }

            IWebElement radio = wait.Until(ExpectedConditions.ElementExists(locator));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", radio);
            Thread.Sleep(1000);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radio);

            Thread.Sleep(1500);
        }

        public void AgregarProducto(string producto, string cantidad, string valorUnitario)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            // 1. Abrimos el buscador de conceptos
            IWebElement comboConcepto = wait.Until(ExpectedConditions.ElementExists(cmbBuscarConcepto));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", comboConcepto);
            Thread.Sleep(1000);

            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(cmbBuscarConcepto)).Click();
            }
            catch (Exception)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", comboConcepto);
            }

            Thread.Sleep(1500);

            // 2. SELECCIÓN DINÁMICA: Creamos el localizador en el momento con el nombre exacto del producto
            By selectorProductoDinamico = By.XPath($"//span[normalize-space()='{producto}']");
            IWebElement opcion = wait.Until(ExpectedConditions.ElementExists(selectorProductoDinamico));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opcion);

            // Damos tiempo a que se dibuje la nueva fila en la tabla
            Thread.Sleep(2000);

            // 3. LLENADO EN LA ÚLTIMA FILA: Buscamos todos los inputs y tomamos los de la fila recién agregada
            var camposCantidad = driver.FindElements(txtCantidad);
            if (camposCantidad.Count > 0)
            {
                IWebElement ultimaCantidad = camposCantidad[camposCantidad.Count - 1]; // Toma el último
                ultimaCantidad.Clear();
                ultimaCantidad.SendKeys(cantidad);
            }

            Thread.Sleep(500);

            var camposPrecio = driver.FindElements(txtValorUnitario);
            if (camposPrecio.Count > 0)
            {
                IWebElement ultimoPrecio = camposPrecio[camposPrecio.Count - 1]; // Toma el último
                ultimoPrecio.Clear();
                ultimoPrecio.SendKeys(valorUnitario);
            }

            Thread.Sleep(1000); // Pequeña pausa antes de que el bucle intente agregar el siguiente producto
        }
        public void HabilitarDescuentoPorItem(bool habilitar)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement checkbox = wait.Until(ExpectedConditions.ElementExists(chkHabilitarDescuento));

            if (habilitar != checkbox.Selected)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
                Thread.Sleep(1000); // Espera a que la columna se renderice
            }
        }
        public void ConfigurarDescuentoEnFila(string montoDescuento)
        {
            // 1. Tienes que declarar el wait dentro del método o usar driver directamente
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. CORRECCIÓN 'method group': Agregamos los paréntesis a Count() 
            // y aseguramos que el selector 'txtDescuentoItemTabla' esté bien definido.
            var camposDescuento = wait.Until(d => d.FindElements(txtDescuentoItemTabla));

            // Usamos Count - 1 (sin paréntesis si es IReadOnlyCollection) o Count()
            IWebElement ultimoCampo = camposDescuento[camposDescuento.Count - 1];

            ultimoCampo.Clear();
            ultimoCampo.SendKeys(montoDescuento);
            ultimoCampo.SendKeys(Keys.Tab);
            Thread.Sleep(800);
        }
        public void AplicarDescuentoGlobal(string monto)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement btnGlobal = wait.Until(ExpectedConditions.ElementExists(btnDescuentoGlobal));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnGlobal);
            Thread.Sleep(1000); // Pequeña pausa para que termine el scroll

            wait.Until(ExpectedConditions.ElementToBeClickable(btnGlobal)).Click();
            Thread.Sleep(2000); // Pausa para que se despliegue el input

            IWebElement inputDescuento = wait.Until(ExpectedConditions.ElementIsVisible(txtInputDescuentoGlobal));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(
                "arguments[0].value = arguments[1]; " +
                "arguments[0].dispatchEvent(new Event('input', { bubbles: true })); " +
                "arguments[0].dispatchEvent(new Event('change', { bubbles: true }));",
                inputDescuento, monto);

            Thread.Sleep(1000);

            // 5. Volvemos a buscar el elemento y damos Tab
            IWebElement inputActualizado = wait.Until(ExpectedConditions.ElementIsVisible(txtInputDescuentoGlobal));
            inputActualizado.SendKeys(Keys.Tab);

            Thread.Sleep(1500);
        }

        public void AgregarProductoConAlmacenVarios(string producto, string rol, string almacen, string cantidad, string valorUnitario)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            IWebElement comboConcepto = wait.Until(ExpectedConditions.ElementExists(cmbBuscarConcepto));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", comboConcepto);
            Thread.Sleep(1000);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", comboConcepto);

            By selectorProducto = producto.Contains("Azúcar Rubia") ? opcionProductoLista : opcion1ProductoLista;
            wait.Until(ExpectedConditions.ElementIsVisible(selectorProducto)).Click();

            // 2. Espera a que la nueva fila aparezca y el DOM se estabilice
            Thread.Sleep(3500);

            // 3. Llenar ROL (Siempre en la última fila)
            var roles = driver.FindElements(variosRol);
            IWebElement selRol = roles[roles.Count - 1]; // Tomamos el último select de Rol
            selRol.Click();
            selRol.SendKeys(rol);
            Thread.Sleep(800);
            selRol.SendKeys(Keys.Enter);
            // 4. Esperar a que el combo de Almacén se cargue tras elegir el Rol
            Thread.Sleep(2000);

            // 5. Llenar ALMACÉN (Siempre en la última fila)
            var almacenes = driver.FindElements(variosAlmacen);
            IWebElement selAlmacen = almacenes[almacenes.Count - 1]; // Tomamos el último select de Almacén
            selAlmacen.Click();
            selAlmacen.SendKeys(almacen);
            Thread.Sleep(800);
            selAlmacen.SendKeys(Keys.Enter);

            // 6. Llenar Cantidad y Valor Unitario (En la última fila)
            Thread.Sleep(1000);

            var cantidades = driver.FindElements(txtCantidadTabla);
            IWebElement campoCantidad = cantidades[cantidades.Count - 1];
            campoCantidad.Clear(); // Limpiamos directo
            campoCantidad.SendKeys(cantidad); // Escribimos directo

            Thread.Sleep(800);

            var valores = driver.FindElements(txtValorUnitarioTabla);
            IWebElement campoPrecio = valores[valores.Count - 1];
            campoPrecio.Clear(); // Limpiamos directo
            campoPrecio.SendKeys(valorUnitario); // Escribimos directo
        }
        public void AbrirSeccionPago()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement btnPanel = wait.Until(ExpectedConditions.ElementExists(panelPago));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", btnPanel);

            string ariaExpanded = btnPanel.GetAttribute("aria-expanded");

            if (ariaExpanded == "false" || ariaExpanded == null)
            {
                try
                {
                    wait.Until(ExpectedConditions.ElementToBeClickable(btnPanel)).Click();
                }
                catch (Exception)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnPanel);
                }
                Thread.Sleep(1500); // Pausa obligatoria para que termine la animación de despliegue
            }
        }
        public void SeleccionarTipoPago(string tipo)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            By locator = tipo.Equals("Contado", StringComparison.OrdinalIgnoreCase) ? rbPagoContado : rbPagoCredito;

            IWebElement radioBtn = wait.Until(ExpectedConditions.ElementExists(locator));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radioBtn);
            Thread.Sleep(1000);
        }
        public void SeleccionarTipoPago(string tipo, string montoInicial = "0", string cuotas = "", string frecuencia = "")
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            By locator = tipo.Equals("Contado", StringComparison.OrdinalIgnoreCase) ? rbPagoContado : rbPagoCredito;
            IWebElement radioBtn = wait.Until(ExpectedConditions.ElementExists(locator));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radioBtn);
            Thread.Sleep(1000);

            // 3. SI ES CRÉDITO, LLENAMOS LOS CAMPOS AQUÍ MISMO
            if (tipo.Equals("Crédito", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // --- Monto Inicial ---
                    IWebElement inputMonto = wait.Until(ExpectedConditions.ElementIsVisible(txtMontoInicial));
                    inputMonto.Clear();
                    inputMonto.SendKeys(montoInicial + Keys.Tab);
                    Thread.Sleep(800);

                    // Validación del BUG del '0' que viste en el video
                    if (montoInicial == "0" && string.IsNullOrEmpty(inputMonto.GetAttribute("value")))
                    {
                        Assert.Fail("❌ BUG: El sistema no permite ingresar '0' en el Monto Inicial.");
                    }

                    // --- Número de Cuotas ---
                    IWebElement inputCuotas = wait.Until(ExpectedConditions.ElementIsVisible(txtNumeroCuotas));
                    inputCuotas.Clear();
                    inputCuotas.SendKeys(cuotas + Keys.Tab);

                    // --- Frecuencia (Días) ---
                    IWebElement inputFreq = wait.Until(ExpectedConditions.ElementIsVisible(txtFrecuenciaDias));
                    inputFreq.Clear();
                    inputFreq.SendKeys(frecuencia + Keys.Tab);

                    Thread.Sleep(1500); // Para que se genere el cronograma visualmente
                }
                catch (Exception ex)
                {
                    Assert.Fail($"❌ Error al configurar los campos de Crédito: {ex.Message}");
                }
            }
        }

        public void ConfigurarMedioPago(string medio, string observacion, string codigo = "", string billetera = "", string tarjetaName = "", string cuentaPropia = "", string cuentaProveedor ="", string caja = "")
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver; // ✅ Esto arregla el error de 'js'
            string medioLimpio = medio.ToLower().Trim();

            By locatorTab;
            if (medioLimpio.Contains("transferencia") || medioLimpio.Contains("transferecia")) locatorTab = tabTransferencia;
            else if (medioLimpio.Contains("sito")) locatorTab = tabDeposito;
            else if (medioLimpio.Contains("billetera")) locatorTab = tabBilleteraDigital;
            else if (medioLimpio.Contains("tarjeta") && (medioLimpio.Contains("cr") || medioLimpio.Contains("cre"))) locatorTab = tabTarjetaCredito;
            else if (medioLimpio.Contains("tarjeta") && (medioLimpio.Contains("de") || medioLimpio.Contains("bi"))) locatorTab = tabTarjetaDebito;
            else locatorTab = tabEfectivo;

            IWebElement tab = wait.Until(ExpectedConditions.ElementExists(locatorTab));
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", tab);
            Thread.Sleep(1000);
            js.ExecuteScript("arguments[0].click();", tab);

            // Espera para que cargue el contenido interno
            Thread.Sleep(2500);

            switch (medioLimpio)
            {
                case "efectivo":
                    if (!string.IsNullOrEmpty(observacion))
                    {
                        IWebElement inputObs = wait.Until(ExpectedConditions.ElementIsVisible(txtObservacionEfectivo));
                        inputObs.SendKeys(Keys.Control + "a" + Keys.Backspace);
                        Thread.Sleep(500);
                        inputObs.SendKeys(observacion);
                        inputObs.SendKeys(Keys.Tab);
                    }
                    break;

                case "billetera digital":
                    if (!string.IsNullOrEmpty(billetera))
                    {
                        IWebElement combo = wait.Until(ExpectedConditions.ElementIsVisible(dropdownBilletera));
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", combo);
                        Thread.Sleep(1000);

                        SelectElement select = new SelectElement(combo);

                        try
                        {
                            select.SelectByText(billetera);
                        }
                        catch (NoSuchElementException)
                        {

                            NUnit.Framework.Assert.Fail($"❌ BUG DE UI DETECTADO: El sistema no cargó las opciones de Billetera Digital. Se buscó '{billetera}' pero el combo está vacío o no tiene esa opción.");
                        }
                        Thread.Sleep(500);
                    }

                    if (!string.IsNullOrEmpty(codigo))
                    {
                        IWebElement inputCode = wait.Until(ExpectedConditions.ElementIsVisible(inputTransactionCode));
                        inputCode.SendKeys(codigo);
                    }

                    if (!string.IsNullOrEmpty(observacion))
                    {
                        IWebElement inputObsBil = wait.Until(ExpectedConditions.ElementIsVisible(txtObservacionBilletera));
                        inputObsBil.SendKeys(Keys.Control + "a" + Keys.Backspace);
                        Thread.Sleep(500);
                        inputObsBil.SendKeys(observacion);
                        inputObsBil.SendKeys(Keys.Tab);
                    }
                    break;

                case string m when m.Contains("transferencia") || m.Contains("transferecia"):
                case string d when d.Contains("sito"):
                    try
                    {
                        // 1. Identificar si es Depósito (Caja) o Transferencia (Cuenta Propia)
                        bool esDeposito = medioLimpio.Contains("sito");
                        By selectorCombo = esDeposito ? cmbCaja : cmbCuentaPropia;
                        string valorABuscar = esDeposito ? caja : cuentaPropia;
                        string nombreCampo = esDeposito ? "Caja" : "Cuenta Bancaria Propia";

                        // 2. Intentar encontrar el combo con un tiempo de espera razonable
                        IWebElement combo;
                        try
                        {
                            combo = wait.Until(ExpectedConditions.ElementIsVisible(selectorCombo));
                        }
                        catch (WebDriverTimeoutException)
                        {
                            // AQUÍ DETENEMOS EL FALSO POSITIVO
                            Assert.Fail($"❌ BUG CRÍTICO: El campo '{nombreCampo}' no existe en la interfaz o no cargó. No se puede completar el pago por {medio}.");
                            return;
                        }

                        // 3. Intentar seleccionar la opción
                        SelectElement select = new SelectElement(combo);
                        try
                        {
                            select.SelectByText(valorABuscar);
                        }
                        catch (NoSuchElementException)
                        {
                            // AQUÍ DETENEMOS EL FALSO POSITIVO SI EL COMBO ESTÁ VACÍO
                            Assert.Fail($"❌ BUG DE DATOS: El combo '{nombreCampo}' está presente pero no contiene la opción '{valorABuscar}' (posiblemente cargó vacío).");
                        }

                        // 4. Llenar campos de texto comunes (Cuenta Proveedor e Información)
                        if (!string.IsNullOrEmpty(cuentaProveedor))
                        {
                            IWebElement inputProv = wait.Until(ExpectedConditions.ElementIsVisible(txtCuentaProveedor));
                            inputProv.Clear();
                            inputProv.SendKeys(cuentaProveedor);
                        }

                        if (!string.IsNullOrEmpty(observacion))
                        {
                            IWebElement inputInfo = wait.Until(ExpectedConditions.ElementIsVisible(txtInfoPagoGeneral));
                            inputInfo.Clear();
                            inputInfo.SendKeys(observacion + Keys.Tab);
                        }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail($"❌ Error no controlado en {medio}: {ex.Message}");
                    }
                    break;

                case string m when m.Contains("tarjeta"):
                    try
                    {
                        Thread.Sleep(4000);
                        IWebElement comboT = wait.Until(ExpectedConditions.ElementIsVisible(cmbSeleccionarTarjeta));

                        js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", comboT);
                        Thread.Sleep(1000);

                        if (!string.IsNullOrEmpty(tarjetaName))
                        {
                            SelectElement select = new SelectElement(comboT);
                            string tarjetaBuscada = tarjetaName.Trim().ToUpper();

                            try
                            {
                                select.SelectByText(tarjetaBuscada);
                            }
                            catch (NoSuchElementException)
                            {
                                bool encontrado = false;
                                foreach (var option in select.Options)
                                {
                                    if (option.Text.ToUpper().Contains(tarjetaBuscada))
                                    {
                                        select.SelectByText(option.Text);
                                        encontrado = true;
                                        break;
                                    }
                                }

                                if (!encontrado)
                                {
                                    Assert.Fail($"❌ No se encontró la opción '{tarjetaBuscada}' en el combo de tarjetas.");
                                }
                            }
                            Thread.Sleep(2000);
                        }
                        if (!string.IsNullOrEmpty(observacion))
                        {
                            IWebElement inputInfo = wait.Until(ExpectedConditions.ElementIsVisible(txtInfoTarjeta));
                            inputInfo.Clear();
                            inputInfo.SendKeys(observacion);
                            inputInfo.SendKeys(Keys.Tab);
                        }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail($"❌ Error en Tarjeta: {ex.Message}");
                    }
                    break;

            }
            Thread.Sleep(1000);
        }
        public void ClicGuardarAdquisicion(string accionGuardar)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(25));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            try
            {
                // 1. Esperar a que el botón aparezca en el DOM
                IWebElement boton = wait.Until(ExpectedConditions.ElementExists(btnGuardarAdquisicion));

                // 2. Mover la pantalla hacia el botón (Scroll)
                js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", boton);
                Thread.Sleep(1500);

                // 3. Clic con JavaScript (evita que otros elementos lo tapen)
                js.ExecuteScript("arguments[0].click();", boton);

                // 4. Pausa para que el servidor procese y aparezca el modal de éxito
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Assert.Fail($"❌ No se pudo completar el guardado: {ex.Message}");
            }
        }

        public string ObtenerMensajeConfirmacion()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Apuntamos al modal de error o de éxito que sale en el centro
            By modalPopUp = By.XPath("//div[contains(@class, 'swal2-popup')] | //div[@role='dialog'] | //div[contains(@class, 'modal-content')]");
            string textoMensaje = "";

            try
            {
                IWebElement modal = wait.Until(ExpectedConditions.ElementIsVisible(modalPopUp));
                textoMensaje = modal.Text;
            }
            catch (WebDriverTimeoutException)
            {
                textoMensaje = driver.FindElement(By.TagName("body")).Text;
            }

            var btnOk = wait.Until(ExpectedConditions.ElementExists(btnOkModal));
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(btnOkModal)).Click();
            }
            catch (Exception)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnOk);
            }
            return textoMensaje.Replace("\r", " ").Replace("\n", " ").Trim();
        }
        public string ObtenerMensajeDeValidacion()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            try
            {
                IWebElement alerta = wait.Until(ExpectedConditions.ElementIsVisible(alertaError));
                return alerta.Text;
            }
            catch (WebDriverTimeoutException)
            {
                return "SIN_ALERTA";
            }
        }
    }
}