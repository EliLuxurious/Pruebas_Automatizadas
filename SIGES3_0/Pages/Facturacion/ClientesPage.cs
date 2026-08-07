using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SIGES3_0.Pages.Facturacion
{
    public class ClientesPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public ClientesPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // ================================
        // SELECTORES LOGIN
        // ================================

        private By usernameField = By.Id("floatingInput");
        private By passwordField = By.Id("floatingInputPassword");
        private By loginButton = By.XPath("//button[normalize-space()='Ingresar']");

        // ================================
        // NAVEGACIÓN
        // ================================

        private By moduloFacturacionCiclica = By.XPath("//span[normalize-space()='Facturación Cíclica']/ancestor::a");
        private By submoduloNuevoCliente = By.XPath("//span[normalize-space()='Nuevo Cliente']");
        // ================================
        // DATOS GENERALES
        // ================================

        private By sectionDatosGenerales = By.XPath("//span[normalize-space()='Datos Generales']/ancestor::button");
        private By selectTipoDocumento = By.Id("documentTypeId");

        private By txtNumeroDocumento = By.Id("documentNumber");
        private By btnBuscarRUC = By.XPath("//button[.//i[contains(@class,'bi-search')]]");

        private By txtNombres = By.Id("names");
        private By txtApellidoPaterno = By.Id("paternalSurname");
        private By txtApellidoMaterno = By.Id("maternalSurname");
        private By txtNombreComercial = By.Id("tradeName");

        private By selectPageSize = By.Id("pageSizeSelect");

        private By selectPais = By.Id("country");
        private By selectUbigeo = By.Id("ubigeo");

        private By txtDireccion = By.Id("address");
        private By txtCorreo = By.Id("email");
        private By txtTelefono = By.Id("phoneNumber");

        // ================================
        // FACTURACIÓN
        // ================================

        private By sectionFacturacion = By.XPath("//button[@aria-controls='collapse-facturación']");
        private By dropdownTipoComprobante = By.XPath(
                "//label[contains(text(),'Tipo de Comprobante')]/following::div[contains(@class,'select-trigger')][1]");
        // ================================
        // CICLO FACTURACIÓN
        // ================================

        private By selectCicloFacturacion = By.Id("billingCycleId");

        // ================================
        // FORMA PAGO
        // ================================

        private By radioPagoVencido = By.Id("paymentMethod_250");

        // ================================
        // FECHA
        // ================================

        private By inputFechaInicio = By.XPath("//input[@formcontrolname='serviceStartDate']");

        // ================================
        // BOTONES
        // ================================

        private By btnGuardar = By.XPath("//button[contains(text(),'Guardar')]");
        private By modalOk = By.XPath("//button[normalize-space()='OK']");

        // Registro Completo
        private By seccionCredencialesSol = By.XPath("//h6[normalize-space()='Credenciales SOL']");
        private By seccionGuiasYOse = By.XPath("//h6[contains(.,'Guías de remisión y OSE')]");
        private By seccionConfiguracionAdicional = By.XPath("//h6[contains(.,'Configuración Adicional')]");


        private By mensajeAdvertenciaFactura = By.XPath("//p[contains(@class,'message')]");
        private By botonOkAdvertencia = By.XPath("//button[contains(@class,'ok-button') and normalize-space()='OK']");


        // =====================================================
        // MÉTODOS GENERALES
        // =====================================================

        public void IngresarTexto(By elemento, string texto)
        {
            IWebElement campo = wait.Until(ExpectedConditions.ElementIsVisible(elemento));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", campo);

            campo.Clear();
            campo.SendKeys(texto);

            // Refuerzo para que el sistema reconozca el valor
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].value = arguments[1];", campo, texto);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('input', { bubbles: true }));", campo);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('blur', { bubbles: true }));", campo);
        }

        public void ClickBoton(By elemento)
        {
            CerrarModalSiExiste();

            IWebElement boton = wait.Until(ExpectedConditions.ElementToBeClickable(elemento));
            boton.Click();
        }

        // =====================================================
        // LOGIN
        // =====================================================

        public void Login(string usuario, string password)
        {
            IngresarTexto(usernameField, usuario);
            IngresarTexto(passwordField, password);
            ClickBoton(loginButton);
        }

        // =====================================================
        // NAVEGACIÓN
        // =====================================================

        public void IrFacturacionCiclica()
        {
            ClickBoton(moduloFacturacionCiclica);
        }

        public void ClickNuevoCliente()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(submoduloNuevoCliente)).Click();
        }

        // =====================================================
        // DATOS GENERALES
        // =====================================================

        //public void ExpandirDatosGenerales()
        //{
        //    ClickBoton(sectionDatosGenerales);
        //}

        public void ExpandirDatosGenerales()
        {
            CerrarModalSiExiste(); // 🔥 antes

            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(sectionDatosGenerales));

            try
            {
                btn.Click();
            }
            catch (ElementClickInterceptedException)
            {
                CerrarModalSiExiste(); // 🔥 si aparece justo ahí
                btn.Click();
            }

            CerrarModalSiExiste(); // 🔥 después (CLAVE)
        }

        //public void SeleccionarTipoDoc(string tipoDocumento)
        //{
        //    // Limpiamos el texto que viene del Feature (ej: "el Pasaporte" -> "PASAPORTE")
        //    string tipoLimpio = tipoDocumento
        //        .Replace("el ", "", StringComparison.OrdinalIgnoreCase)
        //        .Replace("la ", "", StringComparison.OrdinalIgnoreCase)
        //        .Trim()
        //        .ToUpper();

        //    var selectElement = wait.Until(ExpectedConditions.ElementToBeClickable(selectTipoDocumento));
        //    var select = new SelectElement(selectElement);

        //    var mapaDocumentos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        //    {
        //        { "DNI", "DOC. NACIONAL DE IDENTIDAD" },
        //        { "RUC", "REG. UNICO DE CONTRIBUYENTES" },
        //        { "PASAPORTE", "PASAPORTE" },
        //        { "CARNET EXTRANJERIA", "CARNET DE EXTRANJERIA" },
        //        { "CEDULA DIPLOMATICA", "CED. DIPLOMATICA DE IDENTIDAD" },
        //        { "RESIDENCIA", "DOC.IDENT.PAIS.RESIDENCIA-NO.D" },
        //        { "SIN RUC", "DOC.TRIB.NO.DOM.SIN.RUC" },
        //        { "PPJJ", "IDENTIFICATION NUMBER - IN – DOC TRIB PP. JJ" },
        //        { "PTP", "PERMISO TEMPORAL DE PERMANENCIA - PTP" },
        //        { "SALVOCONDUCTO", "SALVOCONDUCTO" },
        //        { "TAM", "TAM - TARJETA ANDINA DE MIGRACIÓN" },
        //        { "PPNN", "TAX IDENTIFICATION NUMBER - TIN – DOC TRIB PP.NN" }
        //    };

        //    string textoABuscar = mapaDocumentos.ContainsKey(tipoLimpio) ? mapaDocumentos[tipoLimpio] : tipoLimpio;

        //    foreach (var option in select.Options)
        //    {
        //        if (option.Text.Trim().Contains(textoABuscar, StringComparison.OrdinalIgnoreCase))
        //        {
        //            option.Click();
        //            break;
        //        }
        //    }

        //    // Forzar eventos de Angular para que el formulario sepa que cambió el tipo
        //    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        //    js.ExecuteScript("arguments[0].dispatchEvent(new Event('change', { bubbles: true }));", selectElement);
        //}

        public void SeleccionarTipoDoc(string tipoDocumento)
        {
            string tipoLimpio = tipoDocumento
                .Replace("el ", "", StringComparison.OrdinalIgnoreCase)
                .Replace("la ", "", StringComparison.OrdinalIgnoreCase)
                .Trim()
                .ToUpper();

            if (tipoLimpio.Contains("RESIDENCIA"))
                tipoLimpio = "RESIDENCIA";

            IWebElement selectElement = wait.Until(ExpectedConditions.ElementExists(selectTipoDocumento));
            var select = new SelectElement(selectElement);

            var mapaDocumentos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "DNI", "DOC. NACIONAL DE IDENTIDAD" },
                { "RUC", "REG. UNICO DE CONTRIBUYENTES" },
                { "PASAPORTE", "PASAPORTE" },
                { "CARNET EXTRANJERIA", "CARNET DE EXTRANJERIA" },
                { "CEDULA DIPLOMATICA", "CED. DIPLOMATICA DE IDENTIDAD" },
                { "RESIDENCIA", "DOC.IDENT.PAIS.RESIDENCIA-NO.D" },
                { "SIN RUC", "DOC.TRIB.NO.DOM.SIN.RUC" },
                { "PPJJ", "IDENTIFICATION NUMBER - IN – DOC TRIB PP. JJ" },
                { "PTP", "PERMISO TEMPORAL DE PERMANENCIA - PTP" },
                { "SALVOCONDUCTO", "SALVOCONDUCTO" },
                { "TAM", "TAM - TARJETA ANDINA DE MIGRACIÓN" },
                { "PPNN", "TAX IDENTIFICATION NUMBER - TIN – DOC TRIB PP.NN" }
            };

            string textoABuscar = mapaDocumentos.ContainsKey(tipoLimpio)
                ? mapaDocumentos[tipoLimpio]
                : tipoLimpio;

            foreach (var opt in select.Options)
            {
                Console.WriteLine("👉 OPTION REAL: [" + opt.Text + "]");
            }

            // 🔥 BUSCAR OPCIÓN
            var opcion = select.Options
                .FirstOrDefault(o => o.Text.Trim()
                .Contains(textoABuscar, StringComparison.OrdinalIgnoreCase));

            if (opcion == null)
                throw new Exception($"❌ No se encontró el tipo de documento: {textoABuscar}");

            string value = opcion.GetAttribute("value");

            // 🔥 CLAVE: FORZAR A ANGULAR
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(@"
        const select = arguments[0];
        const value = arguments[1];

        select.value = value;

        select.dispatchEvent(new Event('input', { bubbles: true }));
        select.dispatchEvent(new Event('change', { bubbles: true }));
        select.dispatchEvent(new Event('blur', { bubbles: true }));
    ", selectElement, value);

            // 🔥 VALIDAR QUE CAMBIÓ
            wait.Until(d =>
            {
                var s = new SelectElement(d.FindElement(selectTipoDocumento));
                return s.SelectedOption.Text
                    .Contains(textoABuscar, StringComparison.OrdinalIgnoreCase);
            });

            Console.WriteLine("✅ Tipo documento seleccionado: " + textoABuscar);
        }

        public void BuscarPorRUC(string documento)
        {
            IngresarTexto(txtNumeroDocumento, documento);
            ClickBoton(btnBuscarRUC);

            // 🔥 esperar que el combo UBIGEO esté REALMENTE listo
            wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(selectUbigeo);

                    // 🔥 validar que esté habilitado
                    if (!element.Enabled)
                        return false;

                    var select = new SelectElement(element);

                    // 🔥 validar que tenga opciones reales
                    return select.Options.Count > 1;
                }
                catch (StaleElementReferenceException)
                {
                    return false; // Angular lo recreó
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }

        public void SeleccionarPais()
        {
            var selectPaisCombo = new SelectElement(
                wait.Until(ExpectedConditions.ElementIsVisible(selectPais))
            );

            selectPaisCombo.SelectByValue("144: 517");
        }

        public void SeleccionarUbigeo(string ubigeo)
        {
            wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(selectUbigeo);

                    if (!element.Enabled)
                        return false;

                    var select = new SelectElement(element);

                    if (select.Options.Count <= 1)
                        return false;

                    // 🔥 buscar opción SIN foreach persistente
                    var opcion = select.Options
                        .FirstOrDefault(o => o.Text.Trim()
                        .Contains(ubigeo, StringComparison.OrdinalIgnoreCase));

                    if (opcion == null)
                        return false;

                    opcion.Click();
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false; // 🔥 reintenta automáticamente
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }

        public void IngresarDireccion(string direccion)
        {
            IWebElement campo = wait.Until(ExpectedConditions.ElementToBeClickable(txtDireccion));
            campo.Clear();
            campo.SendKeys(direccion);
            campo.SendKeys(Keys.Tab);
        }

        public void IngresarCorreo(string correo)
        {
            IWebElement campo = wait.Until(ExpectedConditions.ElementIsVisible(txtCorreo));
            campo.Clear();
            campo.SendKeys(correo);
            campo.SendKeys(Keys.Tab);
        }

        public void IngresarTelefono(string telefono)
        {
            IWebElement campo = wait.Until(ExpectedConditions.ElementIsVisible(txtTelefono));
            campo.Clear();
            campo.SendKeys(telefono);
            campo.SendKeys(Keys.Tab);
        }



        // =====================================================
        // FACTURACIÓN
        // =====================================================

        //public void AbrirFacturacion()
        //{
        //    IWebElement btn = wait.Until(ExpectedConditions.ElementIsVisible(sectionFacturacion));

        //    if (btn.GetAttribute("aria-expanded") == "false")
        //        btn.Click();
        //}

        public void AbrirFacturacion()
        {
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(sectionFacturacion));

            if (btn.GetAttribute("aria-expanded") == "false")
                btn.Click();

            // 🔥 Esperar el select REAL
            wait.Until(driver =>
            {
                var elementos = driver.FindElements(
                    By.XPath("//label[contains(text(),'Tipo de Comprobante')]/following::select[1]")
                );
                return elementos.Count > 0 && elementos[0].Displayed;
            });

            Console.WriteLine("✅ Facturación cargada correctamente");
        }

        //public void SeleccionarTipoComprobante(string comprobante)
        //{
        //    var select = new SelectElement(
        //        wait.Until(ExpectedConditions.ElementIsVisible(selectTipoComprobante))
        //    );

        //    foreach (var option in select.Options)
        //    {
        //        if (option.Text.Trim().Equals(comprobante))
        //        {
        //            option.Click();
        //            break;
        //        }
        //    }
        //}
        public void SeleccionarTipoComprobante(string comprobante)
        {
            // 1️⃣ Abrir dropdown
            IWebElement dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(dropdownTipoComprobante));
            dropdown.Click();

            // 2️⃣ Esperar opciones (dinámico Angular)
            By opcionesLocator = By.XPath("//div[contains(@class,'select-item') or contains(@class,'option')]");

            var opciones = wait.Until(driver =>
            {
                var elems = driver.FindElements(opcionesLocator);
                return elems.Count > 0 ? elems : null;
            });

            // 3️⃣ Buscar opción
            var opcion = opciones.FirstOrDefault(o =>
                o.Text.Trim().Equals(comprobante, StringComparison.OrdinalIgnoreCase)
            );

            if (opcion == null)
                throw new Exception($"❌ No se encontró el comprobante: {comprobante}");

            // 4️⃣ Click seguro
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", opcion);

            try
            {
                opcion.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opcion);
            }

            Console.WriteLine("✅ Comprobante seleccionado: " + comprobante);
        }
        // =====================================================
        // CICLO FACTURACIÓN
        // =====================================================

        public void SeleccionarCicloFacturacion(string ciclo)
        {
            By combo = By.Id("billingCycleId");

            // 🔥 1. Esperar que el select exista y esté habilitado
            IWebElement element = wait.Until(driver =>
            {
                try
                {
                    var el = driver.FindElement(combo);
                    return el.Displayed && el.Enabled ? el : null;
                }
                catch
                {
                    return null;
                }
            });

            // 🔥 2. Esperar que tenga opciones reales
            wait.Until(driver =>
            {
                try
                {
                    var select = new SelectElement(driver.FindElement(combo));
                    return select.Options.Count > 1;
                }
                catch
                {
                    return false;
                }
            });

            // 🔥 3. Seleccionar directamente (SIN depender de ANUAL)
            var selectFinal = new SelectElement(driver.FindElement(combo));

            var opcion = selectFinal.Options
                .FirstOrDefault(o => o.Text.Trim()
                .Contains(ciclo, StringComparison.OrdinalIgnoreCase));

            if (opcion == null)
                throw new Exception($"No existe el ciclo: {ciclo}");

            string value = opcion.GetAttribute("value");

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript(@"
        const select = arguments[0];
        const value = arguments[1];

        select.value = value;
        select.dispatchEvent(new Event('input', { bubbles: true }));
        select.dispatchEvent(new Event('change', { bubbles: true }));
        select.dispatchEvent(new Event('blur', { bubbles: true }));
    ", element, value);

            // 🔥 4. Validar que sí cambió
            wait.Until(driver =>
            {
                try
                {
                    var select = new SelectElement(driver.FindElement(combo));
                    return select.SelectedOption.Text
                        .Contains(ciclo, StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            });

            Console.WriteLine("✅ Ciclo seleccionado: " + ciclo);
        }



        // =====================================================
        // PAGO
        // =====================================================

        public void SeleccionarFormaPago(string formaPago)
        {
            // 🔹 Esperar que los radios estén cargados
            wait.Until(driver =>
            {
                var radios = driver.FindElements(By.XPath("//input[@formcontrolname='paymentMethodId']"));
                return radios.Count >= 2; // esperamos que estén ambos
            });

            // 🔹 Buscar el label que contiene el texto
            var label = wait.Until(driver =>
            {
                try
                {
                    var labels = driver.FindElements(By.XPath("//label[contains(@class,'form-check-label')]"));
                    return labels.FirstOrDefault(l => l.Text.Trim().Equals(formaPago, StringComparison.OrdinalIgnoreCase) && l.Displayed);
                }
                catch
                {
                    return null;
                }
            });

            if (label == null)
                throw new Exception($"No se encontró la forma de pago: {formaPago}");

            // 🔹 Scroll y clic seguro
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", label);
            Thread.Sleep(200); // pequeña pausa
            label.Click();

            Console.WriteLine("✅ Forma de pago seleccionada: " + formaPago);
        }

        // =====================================================
        // CALENDARIO
        // =====================================================

        public void ExpandeCalendario()
        {
            // Localizamos el input que actúa como activador del calendario
            By selectorCalendario = By.XPath("//input[@formcontrolname='serviceStartDate']");

            // Esperamos a que sea cliqueable y hacemos clic para desplegar los "cuadrados"
            IWebElement elemento = wait.Until(ExpectedConditions.ElementToBeClickable(selectorCalendario));
            elemento.Click();
        }

        public void SeleccionarFechaCalendario(string fecha)
        {
            IWebElement campoFecha = wait.Until(ExpectedConditions.ElementIsVisible(inputFechaInicio));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", campoFecha);
            Thread.Sleep(200);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(@"
        const input = arguments[0];
        const value = arguments[1];
        input.value = value;
        input.dispatchEvent(new Event('input', { bubbles: true }));
        input.dispatchEvent(new Event('change', { bubbles: true }));
        input.dispatchEvent(new Event('blur', { bubbles: true }));
    ", campoFecha, fecha);

            // Esperar que el valor realmente se haya seteado
            wait.Until(drv =>
            {
                string val = campoFecha.GetAttribute("value");
                return val != null && val.Equals(fecha);
            });

            Console.WriteLine("✅ Fecha ingresada: " + fecha);
        }

        public void SeleccionarFecha(string fecha)
        {
            // 1. Abrir el calendario haciendo clic en el input
            IWebElement input = wait.Until(ExpectedConditions.ElementToBeClickable(inputFechaInicio));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);
            input.Click();
            Thread.Sleep(200); // pequeña pausa para que se despliegue el widget

            // 2. Seleccionar la fecha con JS (Angular-friendly)
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(@"
                const input = arguments[0];
                const value = arguments[1];
                input.value = value;
                input.dispatchEvent(new Event('input', { bubbles: true }));
                input.dispatchEvent(new Event('change', { bubbles: true }));
                input.dispatchEvent(new Event('blur', { bubbles: true }));
            ", input, fecha);

                    // 3. Disparar eventos Angular si existen
                    js.ExecuteScript(@"
                const input = arguments[0];
                if (window.angular) {
                    const el = angular.element(input);
                    el.triggerHandler('input');
                    el.triggerHandler('change');
                }
            ", input);

            // 4. Esperar que Angular registre el valor
            wait.Until(drv =>
            {
                string val = input.GetAttribute("value");
                return val != null && val.Equals(fecha);
            });

            Console.WriteLine("✅ Fecha ingresada: " + fecha);
        }

        // =====================================================
        // GUARDAR
        // =====================================================

        //public string GuardarYConfirmar()
        //{
        //    IWebElement boton = wait.Until(ExpectedConditions.ElementExists(btnGuardar));

        //    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        //    js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", boton);

        //    wait.Until(d =>
        //    {
        //        var el = d.FindElement(btnGuardar);
        //        return el.Enabled && !el.GetAttribute("class").Contains("disabled");
        //    });

        //    try
        //    {
        //        boton.Click();
        //    }
        //    catch (ElementClickInterceptedException)
        //    {
        //        js.ExecuteScript("arguments[0].click();", boton);
        //    }

        //    // 🔥 CAPTURAR MODAL RÁPIDO
        //    try
        //    {
        //        WebDriverWait waitModal = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        //        var modal = waitModal.Until(d =>
        //        {
        //            var m = d.FindElements(By.XPath("//div[contains(@class,'modal-container')]"));
        //            return m.Count > 0 ? m[0] : null;
        //        });

        //        string mensaje = modal.Text;
        //        Console.WriteLine("✅ Modal detectado: " + mensaje);

        //        // cerrar
        //        var btnOk = waitModal.Until(ExpectedConditions.ElementToBeClickable(
        //            By.XPath("//button[normalize-space()='OK']")
        //        ));
        //        btnOk.Click();

        //        return mensaje;
        //    }
        //    catch
        //    {
        //        Console.WriteLine("⚠️ No apareció modal, intentando toast...");

        //        try
        //        {
        //            WebDriverWait waitToast = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        //            var toast = waitToast.Until(d =>
        //            {
        //                var t = d.FindElements(By.XPath("//div[contains(@class,'toast')]"));
        //                return t.Count > 0 ? t[0] : null;
        //            });

        //            string mensaje = toast.Text;
        //            Console.WriteLine("✅ Toast detectado: " + mensaje);

        //            return mensaje;
        //        }
        //        catch
        //        {
        //            return "SIN MENSAJE";
        //        }
        //    }
        //}

        public void Guardar()
        {
            IWebElement boton = wait.Until(ExpectedConditions.ElementExists(btnGuardar));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", boton);

            wait.Until(driver => {
                var element = driver.FindElement(btnGuardar);
                return element.Enabled && !element.GetAttribute("class").Contains("disabled");
            });

            try
            {
                boton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                js.ExecuteScript("arguments[0].click();", boton);
            }
        }

        public string CapturarMensajeModal()
        {
            try
            {
                WebDriverWait waitModal = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                var modal = waitModal.Until(d =>
                {
                    var m = d.FindElements(By.XPath("//div[contains(@class,'modal-container')]"));
                    return m.Count > 0 ? m[0] : null;
                });

                string texto = modal.Text;
                Console.WriteLine("✅ Modal detectado: " + texto);

                return texto;
            }
            catch
            {
                return "SIN MODAL";
            }
        }

        public void CerrarModal()
        {
            try
            {
                var btnOk = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[normalize-space()='OK']")
                ));

                btnOk.Click();
            }
            catch
            {
                // no pasa nada si no existe
            }
        }

        public void SeleccionarPlanSeguro(string nombrePlan)
        {
            // 1️⃣ Intentar encontrar un combo <select>
            By comboPlan = By.Id("planId");
            try
            {
                IWebElement combo = new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(drv =>
                {
                    try
                    {
                        var el = drv.FindElement(comboPlan);
                        if (!el.Displayed || !el.Enabled) return null;

                        var select = new SelectElement(el);
                        return select.Options.Any(o => o.Text.Contains(nombrePlan)) ? el : null;
                    }
                    catch
                    {
                        return null;
                    }
                });

                if (combo != null)
                {
                    var selectElement = new SelectElement(combo);
                    var opcion = selectElement.Options.First(o => o.Text.Contains(nombrePlan));

                    string value = opcion.GetAttribute("value");
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript(@"
                const select = arguments[0];
                const value = arguments[1];
                select.value = value;
                select.dispatchEvent(new Event('input', { bubbles: true }));
                select.dispatchEvent(new Event('change', { bubbles: true }));
                select.dispatchEvent(new Event('blur', { bubbles: true }));
            ", combo, value);

                    // Esperar que Angular registre el cambio
                    new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(drv =>
                    {
                        var select = new SelectElement(drv.FindElement(comboPlan));
                        return select.SelectedOption.Text.Contains(nombrePlan);
                    });

                    Console.WriteLine("✅ Plan seleccionado en combo: " + nombrePlan);
                    return;
                }
            }
            catch
            {
                // No existe combo, seguimos a tabla dinámica
            }

            // 2️⃣ Intentar encontrar en tabla dinámica
            By selectorPlan = By.XPath($"//tr[contains(@class,'selectable-row')]//div[normalize-space()='{nombrePlan}']");
            int maxScrolls = 20; // máximo intentos
            int scrollHeight = 200; // píxeles por scroll

            for (int i = 0; i < maxScrolls; i++)
            {
                var fila = driver.FindElements(selectorPlan).FirstOrDefault();
                if (fila != null && fila.Displayed)
                {
                    try { fila.Click(); }
                    catch (ElementClickInterceptedException)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", fila);
                    }
                    Console.WriteLine("✅ Plan seleccionado en tabla: " + nombrePlan);
                    return;
                }

                // Scroll incremental
                ((IJavaScriptExecutor)driver).ExecuteScript($"document.querySelector('.tabla-container').scrollBy(0, {scrollHeight});");
                Thread.Sleep(300); // pausa para que Angular renderice
            }

            throw new WebDriverTimeoutException($"❌ No se encontró el plan '{nombrePlan}' después de scroll {maxScrolls} veces.");
        }

        // Método para ingresar solo el número sin darle a la lupa
        public void IngresarNumeroDocumentoManual(string numero)
        {
            IngresarTexto(txtNumeroDocumento, numero);
        }

        // Método para llenar nombres y apellidos (usado en los 10 tipos manuales)
        public void IngresarNombreCompletoManual(string nombres, string paterno, string materno)
        {
            IngresarTexto(txtNombres, nombres);
            IngresarTexto(txtApellidoPaterno, paterno);
            IngresarTexto(txtApellidoMaterno, materno);
        }

        public void ConfigurarPaginacion(string cantidad)
        {
            IWebElement comboPaginas = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("pageSizeSelect")));
            var select = new SelectElement(comboPaginas);
            select.SelectByValue(cantidad);

            // 🔥 IMPORTANTE: Esperar a que la tabla se "limpie" y vuelva a cargar
            // O una pausa fija un poco más larga si el ambiente de SIGES es lento
            Thread.Sleep(2500);
            Console.WriteLine($"✅ Paginación ajustada a {cantidad} filas y espera de refresco terminada.");
        }

        // =====================================================
        // 🚀 MÉTODO REUTILIZABLE PARA CREAR CLIENTE
        // =====================================================
        public string CrearClienteBasico(string nombrePlan)
        {
            Console.WriteLine("Creando cliente automático...");

            CerrarModalSiExiste();

            // 1️⃣ Ir a Nuevo Cliente
            ClickNuevoCliente();

            CerrarModalSiExiste();

            // 2️⃣ Datos Generales
            ExpandirDatosGenerales();

            SeleccionarTipoDoc("RUC");

            // 🔥 RUC dinámico para evitar duplicados
            string ruc = "10" + DateTime.Now.ToString("HHmmssfff");
            IngresarNumeroDocumentoManual(ruc);

            // Simular datos manuales (evita dependencia SUNAT)
            IngresarNombreCompletoManual("CLIENTE AUTO", "TEST", "QA");
            IngresarNombreComercial("COMERCIAL AUTO QA");

            // 3️⃣ Ubigeo + datos
            SeleccionarPais();
            SeleccionarUbigeo("HUANUCO - LEONCIO PRADO");

            IngresarDireccion("Jr. Test Automatización 123");
            IngresarCorreo("test_auto@gmail.com");
            IngresarTelefono("999999999");

            // 4️⃣ Facturación
            AbrirFacturacion();

            SeleccionarTipoComprobante("FACTURA ELECTRONICA");
            SeleccionarCicloFacturacion("MENSUAL");
            SeleccionarFormaPago("VENCIDO");

            // Fecha dinámica
            string fecha = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            SeleccionarFechaCalendario(fecha);

            // 5️⃣ Seleccionar Plan (dinámico)
            ConfigurarPaginacion("100");
            SeleccionarPlanSeguro(nombrePlan);

            // 6️⃣ Guardar
            Guardar();
            CerrarModal();

            Console.WriteLine($"Cliente creado con RUC: {ruc}");

            return ruc;
        }

        public void IngresarNombreComercial(string nombreComercial)
        {
            IngresarTexto(txtNombreComercial, nombreComercial);
        }

        public void CerrarModalSiExiste()
        {
            try
            {
                var modal = driver.FindElements(By.XPath("//div[contains(@class,'modal-container')]")).FirstOrDefault();

                if (modal != null && modal.Displayed)
                {
                    Console.WriteLine("⚠️ Modal detectado, cerrando...");

                    var btnOk = wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("//button[normalize-space()='OK']")
                    ));

                    btnOk.Click();

                    // esperar que desaparezca
                    wait.Until(driver =>
                    {
                        return driver.FindElements(By.XPath("//div[contains(@class,'modal-container')]")).Count == 0;
                    });

                    Console.WriteLine("✅ Modal cerrado");
                }
            }
            catch
            {
                // no hay modal → normal
            }
        }

        public void EsperarPlanDisponible(string nombrePlan)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            wait.Until(driver =>
            {
                try
                {
                    var planes = driver.FindElements(By.XPath("//tr//td[contains(text(),'" + nombrePlan + "')]"));
                    return planes.Count > 0;
                }
                catch
                {
                    return false;
                }
            });

            Console.WriteLine("✅ Plan disponible en sistema: " + nombrePlan);
        }

        public void ImprimirErroresConsola()
        {
            var logs = driver.Manage().Logs.GetLog("browser");

            foreach (var log in logs)
            {
                Console.WriteLine($"[BROWSER LOG] {log.Level}: {log.Message}");
            }
        }

        public string ObtenerMensajeFinal()
        {
            try
            {
                // 🔥 Esperar hasta 10 segundos a que aparezca algún mensaje
                WebDriverWait waitMensaje = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement mensajeElemento = waitMensaje.Until(driver =>
                {
                    var elementos = driver.FindElements(
                        By.XPath("//div[contains(@class,'toast') or contains(@class,'alert')]")
                    );

                    return elementos.Count > 0 ? elementos[0] : null;
                });

                return mensajeElemento.Text;
            }
            catch
            {
                return "SIN MENSAJE";
            }
        }

        public string ObtenerMensajeModal()
        {
            try
            {
                var modal = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//div[contains(@class,'modal-container')]")
                ));

                return modal.Text;
            }
            catch
            {
                return "SIN MODAL";
            }
        }

        public void EsperarPlanEnCombo(string nombrePlan)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            wait.Until(driver =>
            {
                try
                {
                    var select = new SelectElement(driver.FindElement(By.Id("planId")));

                    return select.Options.Any(o =>
                        o.Text.Contains(nombrePlan, StringComparison.OrdinalIgnoreCase));
                }
                catch
                {
                    return false;
                }
            });

            Console.WriteLine("✅ Plan disponible en combo de clientes: " + nombrePlan);
        }
        public bool ExistePlanEnClientes(string nombrePlan)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                // 🔥 1. Ubicar el combo
                var combo = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("planId")));

                // 🔥 2. Scroll al combo (por si está fuera de vista)
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", combo);

                // 🔥 3. Intentar como SELECT normal
                try
                {
                    var select = new SelectElement(combo);

                    foreach (var option in select.Options)
                    {
                        string texto = option.Text.Trim();

                        Console.WriteLine("🔍 [SELECT] " + texto);

                        if (texto.Contains(nombrePlan.Trim(), StringComparison.OrdinalIgnoreCase))
                            return true;
                    }
                }
                catch
                {
                    Console.WriteLine("⚠️ No es un SELECT clásico, probando scroll dinámico...");
                }

                // 🔥 4. FORZAR SCROLL (caso Angular / listas largas)
                combo.Click();
                Thread.Sleep(500);

                int maxScrolls = 20;

                for (int i = 0; i < maxScrolls; i++)
                {
                    var opciones = driver.FindElements(By.XPath("//div[contains(@class,'option') or contains(@class,'ng-option')]"));

                    foreach (var opcion in opciones)
                    {
                        string texto = opcion.Text.Trim();

                        Console.WriteLine("🔍 [SCROLL] " + texto);

                        if (texto.Contains(nombrePlan, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("✅ Plan encontrado con scroll");
                            return true;
                        }
                    }

                    // 🔥 SCROLL hacia abajo
                    ((IJavaScriptExecutor)driver).ExecuteScript(@"
                let panel = document.querySelector('.ng-dropdown-panel-items');
                if(panel) panel.scrollTop += 300;
            ");

                    Thread.Sleep(300);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public string EsperarResultadoFinalRegistro()
        {
            WebDriverWait waitLargo = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            try
            {
                return waitLargo.Until(driver =>
                {
                    try
                    {
                        // 1. Buscar modal visible
                        var modal = driver.FindElements(By.XPath("//div[contains(@class,'modal-container')]"))
                            .FirstOrDefault(x => x.Displayed);

                        if (modal != null)
                        {
                            string textoModal = modal.Text.Trim();

                            // Ignorar modal intermedio
                            if (!string.IsNullOrWhiteSpace(textoModal) &&
                                !textoModal.Contains("Procesando solicitud", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("✅ Modal final detectado: " + textoModal);
                                return textoModal;
                            }
                        }

                        // 2. Buscar toast/alert final
                        var mensaje = driver.FindElements(
                            By.XPath("//div[contains(@class,'toast') or contains(@class,'alert')]"))
                            .FirstOrDefault(x => x.Displayed && !string.IsNullOrWhiteSpace(x.Text));

                        if (mensaje != null)
                        {
                            string texto = mensaje.Text.Trim();
                            Console.WriteLine("✅ Toast/alert final detectado: " + texto);
                            return texto;
                        }

                        return null;
                    }
                    catch
                    {
                        return null;
                    }
                });
            }
            catch
            {
                return "SIN RESULTADO FINAL";
            }
        }

        public void AbrirSeccionPorHeader(By headerLocator, string nombreSeccion)
        {
            IWebElement header = wait.Until(ExpectedConditions.ElementIsVisible(headerLocator));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", header);

            Thread.Sleep(300);

            try
            {
                header.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", header);
            }

            Console.WriteLine($"✅ Sección abierta: {nombreSeccion}");
        }

        private IWebElement ObtenerInputDentroDeSeccion(By seccionLocator, string placeholder)
        {
            IWebElement seccion = wait.Until(ExpectedConditions.ElementIsVisible(seccionLocator));

            IWebElement contenedor = seccion.FindElement(By.XPath("./ancestor::div[contains(@class,'card') or contains(@class,'accordion') or contains(@class,'col')][1] | ./ancestor::div[1]"));

            return wait.Until(driver =>
            {
                try
                {
                    var input = contenedor.FindElements(By.XPath($".//input[@placeholder='{placeholder}']")).FirstOrDefault();
                    return input != null && input.Displayed ? input : null;
                }
                catch
                {
                    return null;
                }
            });
        }



        public void AbrirCredencialesSol()
        {
            AbrirSeccionPorHeader(seccionCredencialesSol, "Credenciales SOL");
        }

        public void IngresarCredencialesSolPrimarias(string usuario, string clave)
        {
            var txtUsuario = ObtenerInputDentroDeSeccion(seccionCredencialesSol, "Usuario SOL");
            var txtClave = ObtenerInputDentroDeSeccion(seccionCredencialesSol, "Clave SOL");

            IngresarTextoElemento(txtUsuario, usuario);
            IngresarTextoElemento(txtClave, clave);

            Console.WriteLine("✅ Credenciales SOL primarias ingresadas");
        }

        public void IngresarCredencialesSolSecundarias(string usuario, string clave)
        {
            var txtUsuario = ObtenerInputDentroDeSeccion(seccionCredencialesSol, "Usuario Secundario");
            var txtClave = ObtenerInputDentroDeSeccion(seccionCredencialesSol, "Clave Secundaria");

            IngresarTextoElemento(txtUsuario, usuario);
            IngresarTextoElemento(txtClave, clave);

            Console.WriteLine("✅ Credenciales SOL secundarias ingresadas");
        }

        public void AbrirGuiasYOse()
        {
            AbrirSeccionPorHeader(seccionGuiasYOse, "Guías de remisión y OSE");
        }

        public void IngresarCredencialesGuias(string usuario, string clave)
        {
            var txtUsuario = ObtenerInputDentroDeSeccion(seccionGuiasYOse, "Usuario Guías");
            var txtClave = ObtenerInputDentroDeSeccion(seccionGuiasYOse, "Clave Guías");

            IngresarTextoElemento(txtUsuario, usuario);
            IngresarTextoElemento(txtClave, clave);

            Console.WriteLine("✅ Credenciales de Guías ingresadas");
        }

        public void IngresarCredencialesOse(string usuario, string clave)
        {
            var txtUsuario = ObtenerInputDentroDeSeccion(seccionGuiasYOse, "Usuario OSE");
            var txtClave = ObtenerInputDentroDeSeccion(seccionGuiasYOse, "Clave OSE");

            IngresarTextoElemento(txtUsuario, usuario);
            IngresarTextoElemento(txtClave, clave);

            Console.WriteLine("✅ Credenciales OSE ingresadas");
        }

        public void IngresarTextoElemento(IWebElement campo, string texto)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", campo);

            campo.Clear();
            campo.SendKeys(texto);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].value = arguments[1];", campo, texto);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('input', { bubbles: true }));", campo);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change', { bubbles: true }));", campo);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('blur', { bubbles: true }));", campo);
        }

        public void AbrirConfiguracionAdicional()
        {
            AbrirSeccionPorHeader(seccionConfiguracionAdicional, "Configuración Adicional");
        }

        public void IngresarDatosAnyDesk(string usuario, string clave)
        {
            var txtUsuario = ObtenerInputDentroDeSeccion(seccionConfiguracionAdicional, "ID/Usuario");
            var txtClave = ObtenerInputDentroDeSeccion(seccionConfiguracionAdicional, "Contraseña");

            IngresarTextoElemento(txtUsuario, usuario);
            IngresarTextoElemento(txtClave, clave);

            Console.WriteLine("✅ Datos AnyDesk ingresados");
        }

        public void IngresarTenantId(string tenantId)
        {
            var txtTenant = ObtenerInputDentroDeSeccion(seccionConfiguracionAdicional, "Identificador del Tenant");

            IngresarTextoElemento(txtTenant, tenantId);

            Console.WriteLine("✅ Tenant ID ingresado");
        }

        public string ObtenerMensajeAdvertenciaFactura()
        {
            IWebElement mensaje = wait.Until(ExpectedConditions.ElementIsVisible(mensajeAdvertenciaFactura));
            string texto = mensaje.Text.Trim();

            Console.WriteLine("✅ Advertencia detectada: " + texto);

            return texto;
        }

        public void CerrarAdvertenciaFactura()
        {
            IWebElement btnOk = wait.Until(ExpectedConditions.ElementToBeClickable(botonOkAdvertencia));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnOk);

            Thread.Sleep(300);

            try
            {
                btnOk.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnOk);
            }

            Console.WriteLine("✅ Advertencia cerrada con OK");
        }
    }
}