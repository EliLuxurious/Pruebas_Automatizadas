using SIGES3_0.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace SIGES3_0.Pages.Adquisicion
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
        //private By panelFacturacion = By.Id("heading-collapse-facturacion");
        private By panelFacturacion = By.Id("heading-collapse-facturación");
        private By cmbDocumento = By.XPath("//select[@id='documentTypeId']");
        private By txtSerie = By.XPath("//input[@id='serieName']");
        private By txtCorrelativo = By.XPath("//input[@id='correlative']");
        private By txtFechaEmision = By.XPath("//input[@id='date']");
        private By txtProveedor = By.XPath("//input[starts-with(@id, 'actor-search-')]");
        //private By txtProveedor = By.XPath("//input[@id='client-search']");
        private By txtInfoAdicional = By.XPath("//textarea[@id='additionalInfo']");

        // --- 2. Entrega ---
        //private By panelEntrega = By.Id("heading-collapse-entrega");
        private By panelEntrega = By.Id("heading-collapse-entrega");
        private By btnEntregaInmediata = By.XPath("//input[@id='tipoBien']");
        private By btnEntregaDiferida = By.XPath("//input[@id='tipoServicio']");
        private By chkSeleccionarAlmacen = By.XPath("//input[@id='slectWarehouse']");
        // En la sección de selectores de Entrega
        private By chkVariosAlmacenes = By.XPath("//input[@id='severalWarehouses']");
        private By cmbRol = By.XPath("//span[contains(text(), 'Rol') or contains(text(), 'Role')]/following-sibling::app-dropdown-search//div[contains(@class, 'select-trigger')]");
        private By cmbEstablecimiento = By.XPath("//span[contains(text(), 'Establecimiento') or contains(text(), 'Establishment')]/following-sibling::app-dropdown-search//div[contains(@class, 'select-trigger')]");
        private By cmbAlmacen = By.XPath("//span[contains(text(), 'Almacén') or contains(text(), 'Warehouse')]/following-sibling::app-dropdown-search//div[contains(@class, 'select-trigger')]");
        private By txtBusquedaDropdown = By.XPath("//input[@placeholder='Buscar...'] | //input[contains(@class, 'select-search')]");

        // --- 2.1. Tipo dee Compra 

        // --- 2.1. Tipo de Compra ---
        private By rbCompraG = By.XPath("//label[normalize-space()='G']/preceding-sibling::input");
        private By rbCompraNG = By.XPath("//label[normalize-space()='NG']/preceding-sibling::input");
        private By rbCompraGyNG = By.XPath("//label[normalize-space()='G Y NG']/preceding-sibling::input");

        // --- 3. Productos ---
        private By cmbBuscarConcepto = By.XPath("//label[@for='conceptSelect']/following-sibling::div//div[contains(@class, 'select-trigger')]");
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
        private By txtMontoMultipago = By.XPath("//input[contains(@formcontrolname, 'amount')] | //input[@type='number' and not(@disabled)]");
        private By btnAgregarPago = By.XPath("//button[contains(text(), 'Agregar Pago')]");

        // --- Medios de Pago (Pestañas) ---

        //Montos en general despues de darle a Multipaggo 
        private By txtMontoEfectivo = By.XPath("//div[@id='item1']//input[contains(@formcontrolname, 'amount') or @type='number']");
        private By txtMontoBilletera = By.XPath("//div[@id='item2']//input[contains(@formcontrolname, 'amount') or @type='number']");
        private By txtMontoDeposito = By.XPath("//div[@id='item3']//input[contains(@formcontrolname, 'amount') or @type='number']");
        private By txtMontoTransferencia = By.XPath("//div[@id='item4']//input[contains(@formcontrolname, 'amount') or @type='number']");
        private By txtMontoTarjetaDebito = By.XPath("//div[@id='item5']//input[contains(@formcontrolname, 'amount') or @type='number']");
        private By txtMontoTarjetaCredito = By.XPath("//div[@id='item6']//input[contains(@formcontrolname, 'amount') or @type='number']");

        // Efectivo 
        private By tabEfectivo = By.XPath("//button[@aria-controls='item1']");
        private By txtObservacionEfectivo = By.XPath("//input[@id='observacion']");

        // Billetera Digital
        private By tabBilleteraDigital = By.XPath("//button[@aria-controls='item2']");
        private By dropdownBilletera = By.XPath("//div[@id='item2']//select");
        private By inputTransactionCode = By.XPath("//div[@id='item2']//input[@type='text' or not(@type='checkbox')]");
        private By txtObservacionBilletera = By.XPath("//div[@id='item2']//textarea | //div[@id='item2']//input[contains(@id, 'observa') or contains(@formcontrolname, 'descrip')]");

        //Transferencia en cuenta
        private By tabTransferencia = By.XPath("//button[@aria-controls='item4']");
        private By cmbCuentaPropia = By.XPath("//div[@id='item4']//*[@formcontrolname='ownBankAccountId']"); 
        private By txtCuentaProvTransferencia = By.XPath("//div[@id='item4']//*[@id='supplierBankAccount']");
        private By txtInfoTransferencia = By.XPath("//div[@id='item4']//*[@id='information']");

        //Deposito en cuenta
        private By tabDeposito = By.XPath("//button[@aria-controls='item3']");
        private By cmbCaja = By.XPath("//div[@id='item3']//*[@formcontrolname='checkoutId']");
        private By txtCuentaProvDeposito = By.XPath("//div[@id='item3']//*[@id='numberBankAccount']");
        private By txtInfoDeposito = By.XPath("//div[@id='item3']//*[@id='information']");


        // Tarjeta de Credito
        private By tabTarjetaCredito = By.XPath("//button[@aria-controls='item6']");
        //Tarjeta de Debito
        private By tabTarjetaDebito = By.XPath("//button[@aria-controls='item5']");
        private By cmbSeleccionarTarjeta = By.Id("bankingCard");
        private By txtInfoTarjeta = By.Id("information");

        // VALIDACION
        private By alertaError = By.XPath("//div[contains(@class, 'toast-error')] | //div[contains(@class, 'alert-danger')] | //div[contains(@class, 'invalid-feedback')] | //div[@role='alert']");

        //--- 5. Guardar
        private By btnGuardarAdquisicion = By.XPath("//button[contains(text(), 'Guardar Adquisición')]");
        private By btnOkModal = By.XPath("//button[contains(@class, 'ok-button')]");

        private By modalContenedor = By.XPath("//div[contains(@class, 'modal-container')]");
        private By textoMensaje = By.XPath("//p[contains(@class, 'message')]");
        private By btnOk = By.XPath("//button[contains(@class, 'ok-button')]");

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
                IWebElement errorVisual = driver.FindElement(By.XPath("//app-business-actor-search//input[contains(@class, 'is-invalid')] | //div[contains(text(), 'Debe seleccionar un proveedor')]"));
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
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var camposDescuento = wait.Until(d => d.FindElements(txtDescuentoItemTabla));

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

            Thread.Sleep(3500);

            var roles = driver.FindElements(variosRol);
            IWebElement selRol = roles[roles.Count - 1];
            selRol.Click();
            selRol.SendKeys(rol);
            Thread.Sleep(800);
            selRol.SendKeys(Keys.Enter);
            Thread.Sleep(2000);

            var almacenes = driver.FindElements(variosAlmacen);
            IWebElement selAlmacen = almacenes[almacenes.Count - 1]; 
            selAlmacen.Click();
            selAlmacen.SendKeys(almacen);
            Thread.Sleep(800);
            selAlmacen.SendKeys(Keys.Enter);

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
            var existeSeccion = driver.FindElements(panelPago).Count > 0;

            if (!existeSeccion)
            {
                Console.WriteLine("ℹ️ La sección de pago no está presente (probablemente por importe 0). Saltando...");
                return;
            }

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
                Thread.Sleep(1500);
            }
        }
        public void SeleccionarTipoPago(string tipo, string montoInicial = "0", string cuotas = "", string frecuencia = "", bool esMultipago = false)
        {
            //esto es pa proteger por si el importe es igual a 0
            var radios = driver.FindElements(rbPagoContado);
            if (radios.Count == 0 || !radios[0].Displayed) return;

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            By locator = tipo.Equals("Contado", StringComparison.OrdinalIgnoreCase) ? rbPagoContado : rbPagoCredito;
            IWebElement radioBtn = wait.Until(ExpectedConditions.ElementExists(locator));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radioBtn);
            Thread.Sleep(1000);
            

            if (tipo.Equals("Crédito", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // --- Monto Inicial ---
                    IWebElement inputMonto = wait.Until(ExpectedConditions.ElementIsVisible(txtMontoInicial));
                    inputMonto.Clear();
                    inputMonto.SendKeys(montoInicial + Keys.Tab);
                    Thread.Sleep(1500); 

                    if (montoInicial == "0")
                    {
                        var pestañasPagoVisible = driver.FindElements(tabEfectivo);

                        bool pestañaEsVisible = pestañasPagoVisible.Count > 0 && pestañasPagoVisible[0].Displayed;

                        if (!pestañaEsVisible)
                        {
                            Assert.Fail("❌ BUG CRÍTICO DE UI DETECTADO: Al ingresar un Monto Inicial de '0', la sección de Medios de Pago (Efectivo, Tarjeta, etc.) se oculta, bloqueando el formulario y evitando que se pueda guardar la adquisición porque el sistema sigue requiriendo seleccionar un método.");
                        }
                    }

                    IWebElement inputCuotas = wait.Until(ExpectedConditions.ElementIsVisible(txtNumeroCuotas));
                    inputCuotas.Clear();
                    inputCuotas.SendKeys(cuotas + Keys.Tab);

                    // --- Frecuencia (Días) ---
                    IWebElement inputFreq = wait.Until(ExpectedConditions.ElementIsVisible(txtFrecuenciaDias));
                    inputFreq.Clear();
                    inputFreq.SendKeys(frecuencia + Keys.Tab);

                    Thread.Sleep(1500); 
                }
                catch (Exception ex)
                {
                    if (ex is AssertionException)
                    {
                        throw; 
                    }
                    Assert.Fail($"❌ Error al configurar los campos de Crédito: {ex.Message}");
                }
            }
        }

        public void ActivarMultipago(bool activar)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement checkbox = wait.Until(ExpectedConditions.ElementExists(btnMultiPago));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", checkbox);

            bool estaMarcado = checkbox.Selected;

            if (activar && !estaMarcado)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
                Thread.Sleep(1500); // Dar tiempo a que Angular habilite todos los inputs de monto
            }
            else if (!activar && estaMarcado)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
            }
        }

        public void AgregarPagoGrid()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement btnAdd = wait.Until(ExpectedConditions.ElementToBeClickable(btnAgregarPago));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnAdd);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnAdd);

            Thread.Sleep(1500);
        }
        public void ConfigurarMedioPago(string medio, string observacion, string monto = "", string codigo = "", string billetera = "", string tarjetaName = "", string cuentaPropia = "", string cuentaProveedor = "", string caja = "")
        {
            //esto es pa proteger por si el importe es iguala a 0
            var radios = driver.FindElements(rbPagoContado);
            if (radios.Count == 0 || !radios[0].Displayed) return;


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
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
            Thread.Sleep(2000); // Espera a que la pestaña cargue

            switch (medioLimpio)
            {
                case "efectivo":
                    // Si enviamos un monto, es porque es Multipago y lo escribimos directamente
                    if (!string.IsNullOrEmpty(monto))
                    {
                        IWebElement inputMonto = wait.Until(ExpectedConditions.ElementIsVisible(txtMontoEfectivo));
                        inputMonto.Clear();
                        inputMonto.SendKeys(monto + Keys.Tab);
                        Thread.Sleep(500);
                    }
                    else // Si no hay monto, validamos si necesita el truco ninja (pago único bloqueado)
                    {
                        try
                        {
                            IWebElement inputMonto = wait.Until(ExpectedConditions.ElementExists(txtMontoEfectivo));
                            if (inputMonto.GetAttribute("disabled") != null)
                            {
                                js.ExecuteScript(
                                    "arguments[0].removeAttribute('disabled');" +
                                    "arguments[0].dispatchEvent(new Event('input', { bubbles: true }));" +
                                    "arguments[0].dispatchEvent(new Event('change', { bubbles: true }));" +
                                    "arguments[0].setAttribute('disabled', 'true');",
                                    inputMonto);
                                Thread.Sleep(800);
                            }
                        }
                        catch (NoSuchElementException) { }
                    }

                    if (!string.IsNullOrEmpty(observacion))
                    {
                        IWebElement inputObs = wait.Until(ExpectedConditions.ElementIsVisible(txtObservacionEfectivo));
                        inputObs.SendKeys(Keys.Control + "a" + Keys.Backspace);
                        Thread.Sleep(500);
                        inputObs.SendKeys(observacion + Keys.Tab);
                    }
                    break;

                case "billetera digital":
                    if (!string.IsNullOrEmpty(monto))
                    {
                        IWebElement inputMonto = wait.Until(ExpectedConditions.ElementIsVisible(txtMontoBilletera));
                        inputMonto.Clear();
                        inputMonto.SendKeys(monto + Keys.Tab);
                    }

                    if (!string.IsNullOrEmpty(billetera))
                    {
                        IWebElement combo = wait.Until(ExpectedConditions.ElementIsVisible(dropdownBilletera));
                        js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", combo);
                        Thread.Sleep(1000);
                        SelectElement select = new SelectElement(combo);

                        if (select.Options.Count == 0 || (select.Options.Count == 1 && select.Options[0].Text.Contains("Seleccione")))
                        {
                            Assert.Fail($"❌ BUG DE DATOS: El combo de 'Billetera digital' está vacío. No se cargaron opciones como Yape o Plin.");
                        }

                        try
                        {
                            select.SelectByText(billetera);
                        }
                        catch (NoSuchElementException)
                        {
                            Assert.Fail($"❌ ERROR: No se encontró la billetera '{billetera}' en el listado del sistema.");
                        }

                        Thread.Sleep(500);
                    }
                    if (!string.IsNullOrEmpty(codigo)) wait.Until(ExpectedConditions.ElementIsVisible(inputTransactionCode)).SendKeys(codigo);
                    if (!string.IsNullOrEmpty(observacion))
                    {
                        IWebElement inputObsBil = wait.Until(ExpectedConditions.ElementIsVisible(txtObservacionBilletera));
                        inputObsBil.SendKeys(Keys.Control + "a" + Keys.Backspace);
                        inputObsBil.SendKeys(observacion + Keys.Tab);
                    }
                    break;

                case string m when m.Contains("transferencia") || m.Contains("transferecia"):
                case string d when d.Contains("sito"):
                    bool esDeposito = medioLimpio.Contains("sito");

                    if (!string.IsNullOrEmpty(monto))
                    {
                        By selectorMonto = esDeposito ? txtMontoDeposito : txtMontoTransferencia;
                        IWebElement inputMonto = wait.Until(ExpectedConditions.ElementIsVisible(selectorMonto));
                        inputMonto.Clear();
                        inputMonto.SendKeys(monto + Keys.Tab);
                        Thread.Sleep(500);
                    }

                    try
                    {
                        By selectorCombo = esDeposito ? cmbCaja : cmbCuentaPropia;
                        string valorABuscar = esDeposito ? caja : cuentaPropia;
                        string nombreCampo = esDeposito ? "Caja" : "Cuenta Bancaria Propia";

                        IWebElement combo = wait.Until(ExpectedConditions.ElementIsVisible(selectorCombo));
                        SelectElement select = new SelectElement(combo);
                        if (!string.IsNullOrEmpty(valorABuscar))
                        {
                            // Validamos si el combo está literalmente vacío o si solo tiene la opción "Seleccione..." deshabilitada
                            if (select.Options.Count == 0 || (select.Options.Count == 1 && select.Options[0].GetAttribute("disabled") != null))
                            {
                                Assert.Fail($"❌ BUG DE DATOS DETECTADO: El combo desplegable de '{nombreCampo}' está vacío. El sistema no ha cargado las opciones.");
                            }

                            try
                            {
                                select.SelectByText(valorABuscar);
                            }
                            catch (NoSuchElementException)
                            {
                                Assert.Fail($"❌ ERROR DE VALIDACIÓN: Se intentó seleccionar '{valorABuscar}' en '{nombreCampo}', pero la opción no existe en el sistema.");
                            }
                        }
                        if (!string.IsNullOrEmpty(cuentaProveedor))
                        {
                            By selectorProv = esDeposito ? txtCuentaProvDeposito : txtCuentaProvTransferencia;
                            IWebElement inputProv = wait.Until(ExpectedConditions.ElementIsVisible(selectorProv));
                            inputProv.Clear();
                            inputProv.SendKeys(cuentaProveedor);
                        }

                        if (!string.IsNullOrEmpty(observacion))
                        {

                            By selectorInfo = esDeposito ? txtInfoDeposito : txtInfoTransferencia;
                            IWebElement inputInfo = wait.Until(ExpectedConditions.ElementIsVisible(selectorInfo));
                            inputInfo.Clear();
                            inputInfo.SendKeys(observacion + Keys.Tab);
                        }
                    }

                    catch (Exception ex) { Assert.Fail($"❌ Error: {ex.Message}"); }
                    break;

                case string m when m.Contains("tarjeta"):
                    bool esCredito = medioLimpio.Contains("cr") || medioLimpio.Contains("cre");

                    if (!string.IsNullOrEmpty(monto))
                    {
                        By selectorMonto = esCredito ? txtMontoTarjetaCredito : txtMontoTarjetaDebito;
                        IWebElement inputMonto = wait.Until(ExpectedConditions.ElementIsVisible(selectorMonto));
                        inputMonto.Clear();
                        inputMonto.SendKeys(monto + Keys.Tab);
                        Thread.Sleep(500);
                    }

                    try
                    {
                        Thread.Sleep(2000);
                        IWebElement comboT = wait.Until(ExpectedConditions.ElementIsVisible(cmbSeleccionarTarjeta));
                        js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", comboT);
                        Thread.Sleep(1000);

                        if (!string.IsNullOrEmpty(tarjetaName))
                        {
                            SelectElement select = new SelectElement(comboT);
                            string tarjetaBuscada = tarjetaName.Trim().ToUpper();
                            try { select.SelectByText(tarjetaBuscada); }
                            catch (NoSuchElementException)
                            {
                                bool encontrado = false;
                                foreach (var option in select.Options) { if (option.Text.ToUpper().Contains(tarjetaBuscada)) { select.SelectByText(option.Text); encontrado = true; break; } }
                                if (!encontrado) Assert.Fail($"❌ No se encontró la opción '{tarjetaBuscada}'.");
                            }
                            Thread.Sleep(1500);
                        }
                        if (!string.IsNullOrEmpty(observacion))
                        {
                            IWebElement inputInfo = wait.Until(ExpectedConditions.ElementIsVisible(txtInfoTarjeta));
                            inputInfo.Clear();
                            inputInfo.SendKeys(observacion + Keys.Tab);
                        }
                    }
                    catch (Exception ex) { Assert.Fail($"❌ Error en Tarjeta: {ex.Message}"); }
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
                IWebElement boton = wait.Until(ExpectedConditions.ElementExists(btnGuardarAdquisicion));

                js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", boton);
                Thread.Sleep(1500);

                js.ExecuteScript("arguments[0].click();", boton);

                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Assert.Fail($"❌ No se pudo completar el guardado: {ex.Message}");
            }
        }

        public string ObtenerMensajeConfirmacion()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(modalContenedor));

                IWebElement mensajeElemento = wait.Until(ExpectedConditions.ElementIsVisible(textoMensaje));
                string mensajeFinal = mensajeElemento.Text.Trim();

                try
                {
                    IWebElement botonOk = driver.FindElement(btnOk);
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonOk);
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(modalContenedor));
                }
                catch { /* Si no se puede cerrar, igual ya tenemos el mensaje */ }

                return mensajeFinal;
            }
            catch (WebDriverTimeoutException)
            {
                var errores = driver.FindElements(By.XPath("//div[contains(@class, 'invalid-feedback')] | //*[contains(text(), 'Complete los campos')]"));

                if (errores.Count > 0)
                    return "ERROR_SISTEMA: " + errores[0].Text;

                return "NO_SE_DETECTO_MODAL_DE_EXITO";
            }
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