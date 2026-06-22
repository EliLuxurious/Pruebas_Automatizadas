using SIGES3_0.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace SIGES3_0.Pages
{
    public class PlanServicioPage
    {
        private IWebDriver driver;
        Utilities utilities;
        private readonly WebDriverWait wait; // esto se aauemnto al ultimo

        public PlanServicioPage(IWebDriver driver)
        {
            
            this.driver = driver;
            utilities = new Utilities(driver);
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15)); //esto de iguaal froma

        }
        
        // --- Selectores para Login ---
        //private By usernameField = By.Id("floatingInput");
        //private By passwordField = By.Id("floatingInputPassword");
        //private By loginButton = By.XPath("//button[normalize-space()='Ingresar']");
        //private By logo = By.XPath("//img[@alt='Logo']");

        // --- Selectores para Navegación ---
        private By moduloFacturacionCiclica = By.XPath("//span[normalize-space()='Facturación Cíclica']/ancestor::a");
        private By checkModulo = By.XPath("//span[normalize-space()='Facturación Cíclica']/ancestor::a//input[@type='checkbox']");
        private By submoduloPlan = By.XPath("//a[contains(@href,'service-plan')]");

        // --- Selectores para Detalles del Plan (Límites) ---
        private By btnDetallesPlan = By.XPath("//button[.//span[contains(text(),'Detalles del Plan')]]");
        private By txtMinComprobantes = By.Id("min-78"); // ID de f12
        private By txtMaxComprobantes = By.Id("max-78"); // ID de f12
        private By txtMinLocales = By.Id("min-79");
        private By txtMaxLocales = By.Id("max-79");
        private By txtMinUsuarios = By.Id("min-80");
        private By txtMaxUsuarios = By.Id("max-80");

        // --- Selectores para Datos Generales ---
        private By tabDatosGenerales = By.XPath("//span[normalize-space()='Datos generales']/ancestor::button");
        private By txtNombrePlan = By.XPath("//input[@placeholder='Nombre del plan']");
        private By txtDescripcionPlan = By.XPath("//textarea[@placeholder='Descripción']");
        private By selectCicloFacturacion = By.CssSelector("select[formcontrolname='billingCycleId']");
        private By txtPrecioPlan = By.XPath("//input[@placeholder='0.00']");
        private By btnGuardar = By.XPath("//button[normalize-space()='Guardar']");

        // --- Modal de OK ---
        private By btnOkModal = By.XPath("//button[normalize-space()='OK']");


        // --- LISTADO / BÚSQUEDA ---
        private By txtBuscarPlan = By.XPath("//th[contains(., 'Nombre')]//following::input[1]");
        private By tablaPlanes = By.XPath("//table[contains(@class, 'table')]");

        private By selectPaginacion = By.Id("pageSizeSelect");

        //private By ObtenerFilaPlan(string nombre) =>
        //    By.XPath($"//tr[.//div[contains(normalize-space(), '{nombre}')]]");

        private By ObtenerFilaPlan(string nombre) =>
            By.XPath($"//tr[.//*[contains(normalize-space(), '{nombre}')]]");

        // --- ESTADO DEL PLAN ---
        private By ObtenerEstadoPlan(string nombre) =>
            By.XPath($"//tr[td[contains(., '{nombre}')]]//input[@role='switch']");

        // --- BOTÓN SOLICITAR BAJA ---
        private By btnSolicitarBaja(string nombre) =>
            By.XPath($"//button[contains(@class, 'btn-danger') and .//i[contains(@class, 'bi-trash')]]");

        // --- MODAL ---
        //private By btnModalSi = By.XPath("//div[contains(@class, 'modal-footer')]//button[normalize-space()='Sí']");
        private By btnModalSi = By.XPath("//button[normalize-space()='Sí' or normalize-space()='Si']");
        private By btnModalNo = By.XPath("//div[contains(@class, 'modal-footer')]//button[normalize-space()='No']");

        // Para editar

        private By btnEditarPlan(string nombre) =>
            By.XPath($"//tr[td[normalize-space()='{nombre}']]//button[@title='Editar']");

        // Botón eliminar ciclo (basado en el ícono trash)
        private By btnEliminarCiclo = By.XPath("//button[contains(@class,'btn-outline-danger')]");
        private By btnGuardarCambio = By.XPath("//button[normalize-space()='Guardar Cambios']");

        private By ObtenerPlanPorNombre(string nombre) =>
            By.XPath($"//div[contains(@class,'custom-cursor-on-hover') and contains(normalize-space(), '{nombre}')]");

        public void OpenToApplication(string url)
        {
            driver.Navigate().GoToUrl(url);
            Thread.Sleep(4000);
        }

        //public void LoginToApplication(string _username, string _password)
        //{
        //    utilities.EnterText(usernameField, _username);
        //    Thread.Sleep(1000);
        //    utilities.EnterText(passwordField, _password);
        //    Thread.Sleep(1000);
        //    utilities.ClickButton(loginButton);
        //    Thread.Sleep(4000);

        //    var succesElement = driver.FindElement(logo);
        //    Assert.IsNotNull(succesElement, "No se inició sesión correctamente.");
        //}

        public void EsperarCargaDashboard()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//span[contains(text(),'Facturación Cíclica')]")
            ));
        }

        public void IrModuloFacturacionCiclica()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            var modulo = wait.Until(ExpectedConditions.ElementExists(moduloFacturacionCiclica));

            // 🔥 Scroll real
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", modulo);

            Thread.Sleep(500);

            // 🔥 Forzar visibilidad real
            wait.Until(ExpectedConditions.ElementToBeClickable(modulo));

            // 🔥 Click REAL (no simulado)
            modulo.Click();

            Thread.Sleep(1000);

            // 🔥 DEBUG: verificar si cambió algo en DOM
            var submodulos = driver.FindElements(submoduloPlan);
            Console.WriteLine("Submodulos encontrados: " + submodulos.Count);
        }

        public void NavegarAPlanDeServicio()
        {
            WebDriverWait waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            Console.WriteLine("Ingresando a Plan de Servicio...");

            // 1. Intentar ubicar directamente el submódulo Plan de Servicio
            var submodulo = waitLong.Until(driver =>
            {
                var elementos = driver.FindElements(By.XPath(
                    "//a[contains(@href,'service-plan') " +
                    "or .//span[contains(normalize-space(),'Plan de Servicio')] " +
                    "or contains(normalize-space(),'Plan de Servicio')]"
                ));

                foreach (var el in elementos)
                {
                    if (el.Displayed && el.Enabled)
                        return el;
                }

                return null;
            });

            js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", submodulo);
            Thread.Sleep(500);

            try
            {
                submodulo.Click();
            }
            catch (ElementClickInterceptedException)
            {
                js.ExecuteScript("arguments[0].click();", submodulo);
            }

            Console.WriteLine("✅ Se ingresó al submódulo Plan de Servicio");
        }

        public void ConfigurarLimitesComprobantes(string min, string max)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            var boton = wait.Until(ExpectedConditions.ElementExists(
                By.XPath("//button[.//span[contains(text(),'Detalles del Plan')]]")
            ));

            // 🔥 Verificar si está cerrado
            string expanded = boton.GetAttribute("aria-expanded");

            if (expanded == "false")
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(boton));

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", boton);
            }

            // 🔥 Esperar que se despliegue el contenido
            wait.Until(ExpectedConditions.ElementIsVisible(txtMinComprobantes));

            utilities.EnterText(txtMinComprobantes, min);
            utilities.EnterText(txtMaxComprobantes, max);
        }

        public void ConfigurarLimitesLocalesYUsuarios(string entidad, string min, string max)
        {
            if (entidad.ToLower().Contains("locales"))
            {
                utilities.EnterText(txtMinLocales, min);
                utilities.EnterText(txtMaxLocales, max);
            }
            else if (entidad.ToLower().Contains("usuarios"))
            {
                utilities.EnterText(txtMinUsuarios, min);
                utilities.EnterText(txtMaxUsuarios, max);
            }
        }

        public void CompletarDatosGenerales(string nombre, string descripcion, string ciclo, string precio)
        {
            // 1. Asegurar pestaña
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var tab = wait.Until(ExpectedConditions.ElementToBeClickable(tabDatosGenerales));
            tab.Click();

            Thread.Sleep(800); // Angular tabs delay real

            if (!string.IsNullOrEmpty(nombre))
                utilities.EnterText(txtNombrePlan, nombre);

            if (!string.IsNullOrEmpty(descripcion))
                utilities.EnterText(txtDescripcionPlan, descripcion);

            if (!string.IsNullOrEmpty(ciclo))
            {
                var select = driver.FindElement(selectCicloFacturacion);

                // 2. Scroll forzado
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", select);

                Thread.Sleep(500);

                // 3. Seteo Angular-safe
                ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].value = arguments[1];
                arguments[0].dispatchEvent(new Event('change'));
                ", select, ObtenerValorCiclo(ciclo));
            }

            //if (!string.IsNullOrEmpty(precio))
            //    utilities.EnterText(txtPrecioPlan, precio); Aqui cambio para limpiar campos
            if (!string.IsNullOrEmpty(precio))
                LimpiarYEscribir(txtPrecioPlan, precio);
        }

        private string ObtenerValorCiclo(string ciclo)
        {
            return ciclo.Trim().ToUpper() switch
            {
                "ANUAL" => "1001",
                "SEMESTRAL" => "1002",
                "TRIMESTRAL" => "1003",
                "BIMESTRAL" => "1004",
                "MENSUAL" => "1005",
                _ => throw new ArgumentException($"Ciclo de facturación no válido: {ciclo}")
            };
        }

        public void ClickGuardar()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardar)).Click();
        }

        public void ConfirmarRegistroCorrecto()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var btnOk = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(btnOkModal)
            );

            wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(btnOk)
            ).Click();
        }

        public void BuscarPlan(string nombre)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var input = wait.Until(ExpectedConditions.ElementIsVisible(txtBuscarPlan));
            input.Clear();
            input.SendKeys(nombre);

            Thread.Sleep(1500);
        }

        //public void SeleccionarPlan(string nombre)
        //{
        //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //    var fila = wait.Until(ExpectedConditions.ElementToBeClickable(ObtenerFilaPlan(nombre)));

        //    ((IJavaScriptExecutor)driver)
        //        .ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", fila);

        //    fila.Click();
        //}

        public void SeleccionarPlan(string nombrePlan)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            CambiarPaginacionA100();
            BuscarPlan(nombrePlan);

            var fila = wait.Until(drv =>
            {
                var elementos = drv.FindElements(By.XPath($"//tr[.//*[contains(normalize-space(), '{nombrePlan}')]]"));
                return elementos.Count > 0 ? elementos[0] : null;
            });

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", fila);
            Thread.Sleep(500);

            try
            {
                fila.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", fila);
            }

            Console.WriteLine("✅ Plan seleccionado: " + nombrePlan);
        }
        public void ClickSolicitarBaja(string nombre)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var btn = wait.Until(ExpectedConditions.ElementToBeClickable(
                btnSolicitarBaja(nombre)
            ));

            btn.Click();
        }

        public void ConfirmarModalSi()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(btnModalSi)).Click();
        }

        public void ConfirmarModalNo()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(btnModalNo)).Click();
        }

        public bool EstaActivo(string nombre)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var toggle = wait.Until(ExpectedConditions.ElementIsVisible(
                ObtenerEstadoPlan(nombre)
            ));

            return toggle.GetAttribute("checked") != null;
        }

        public void EsperarCambioEstado(string nombre, bool estadoEsperado)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            wait.Until(driver => EstaActivo(nombre) == estadoEsperado);
        }

        public bool ExistePlan(string nombrePlan)
        {
            try
            {
                Thread.Sleep(2000);

                var filas = driver.FindElements(By.XPath("//tr"));

                return filas.Any(f => f.Text.Contains(nombrePlan));
            }
            catch
            {
                return false;
            }
        }

        public bool BuscarPlanPorNombre(string nombrePlan)
        {
            try
            {
                // 🔥 1. Cambiar paginación primero
                CambiarPaginacionA100();

                // 🔥 2. Buscar
                var input = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//th//input[@type='text']")));

                input.Clear();
                input.SendKeys(nombrePlan);

                var btnBuscar = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(@class,'btn-primary')]")));

                btnBuscar.Click();

                // 🔥 3. Espera inteligente (NO Sleep)
                wait.Until(driver =>
                {
                    try
                    {
                        var filas = driver.FindElements(By.XPath("//tr"));
                        return filas.Count > 0;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                });

                // 🔥 4. Validar si aparece
                var filasFinal = driver.FindElements(By.XPath("//tr"));

                bool existe = filasFinal.Any(f => f.Text.Contains(nombrePlan));

                Console.WriteLine(existe
                    ? $"✅ Plan encontrado: {nombrePlan}"
                    : $"❌ Plan NO encontrado: {nombrePlan}");

                return existe;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error buscando plan: " + ex.Message);
                return false;
            }
        }

        public void CambiarPaginacionA100()
        {
            var combo = wait.Until(ExpectedConditions.ElementExists(
                By.Id("pageSizeSelect")));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", combo);

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                const select = arguments[0];
                select.value = '100';
                select.dispatchEvent(new Event('change', { bubbles: true }));
            ", combo);

            Thread.Sleep(2000);

            Console.WriteLine("✅ Paginación a 100 en planes");
        }

        public void EsperarQueDesaparezcaPlan(string nombre)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            wait.Until(driver =>
            {
                var elementos = driver.FindElements(ObtenerFilaPlan(nombre));
                return elementos.Count == 0;
            });
        }

        public void ManejarModalOkSiExiste()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

                var btnOk = wait.Until(ExpectedConditions.ElementExists(btnOkModal));

                if (btnOk.Displayed)
                {
                    wait.Until(ExpectedConditions.ElementToBeClickable(btnOkModal)).Click();
                    Console.WriteLine("DEBUG: Modal OK cerrado manualmente");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("DEBUG: Modal no apareció o se cerró automáticamente");
            }
        }

        public void ClickEditarPlan(string nombre)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var btn = wait.Until(ExpectedConditions.ElementToBeClickable(
                btnEditarPlan(nombre)
            ));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btn);

            Thread.Sleep(500);

            btn.Click();
        }

        public string BuscarOCrearPlan(string nombrePlan)
        {
            BuscarPlan(nombrePlan);

            if (!ExistePlan(nombrePlan))
            {
                Console.WriteLine("DEBUG: No existe plan, se creará uno nuevo");

                nombrePlan = "PlanQA_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                ConfigurarLimitesComprobantes("50", "500");
                ConfigurarLimitesLocalesYUsuarios("Locales", "1", "5");
                ConfigurarLimitesLocalesYUsuarios("Usuarios", "2", "15");

                CompletarDatosGenerales(nombrePlan, "Plan auto edit", "MENSUAL", "100");

                ClickGuardar();
                ConfirmarRegistroCorrecto();

                Thread.Sleep(2000);
                driver.Navigate().Refresh();

                BuscarPlan(nombrePlan);
            }

            return nombrePlan;
        }

        public void LimpiarYEscribir(By locator, string valor)
        {
            var element = driver.FindElement(locator);

            element.Click();
            element.Clear();

            Thread.Sleep(300);

            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);

            element.SendKeys(valor);
        }

        public void EliminarCicloSiExiste()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                var botones = driver.FindElements(btnEliminarCiclo);

                if (botones.Count > 0)
                {
                    Console.WriteLine("DEBUG: Eliminando ciclo anterior");

                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", botones[0]);

                    Thread.Sleep(500);

                    botones[0].Click();

                    // 🔥 manejar posible confirmación
                    ManejarModalOkSiExiste();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DEBUG: No se encontró ciclo para eliminar: " + ex.Message);
            }
        }

        public void ClickGuardarCambios()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardarCambio));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            Thread.Sleep(500);

            boton.Click();
        }

        public void ClickToggleEstado(string nombre)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            var toggle = wait.Until(ExpectedConditions.ElementExists(
                ObtenerEstadoPlan(nombre)
            ));

            // 🔥 1. Scroll vertical al elemento
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", toggle);

            Thread.Sleep(500);

            // 🔥 2. Scroll horizontal (CLAVE 🔥)
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].closest('table').parentElement.scrollLeft = arguments[0].offsetLeft;
            ", toggle);

            Thread.Sleep(500);

            // 🔥 3. Click forzado (evita overlays)
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", toggle);
        }

        public void ValidarYSeleccionarPlanEnClientes(string nombre)
        {
            Console.WriteLine($"⏳ Buscando plan en Nuevo Cliente con diagnóstico real: {nombre}");
            Thread.Sleep(1500);

            var contenedor = ObtenerContenedorScrollDerecho();

            if (contenedor == null)
                throw new WebDriverException("❌ No se encontró el contenedor de scroll derecho.");

            Console.WriteLine("DEBUG class contenedor: " + contenedor.GetAttribute("class"));

            var js = (IJavaScriptExecutor)driver;

            long maxScrolls = 30;
            long scrollStep = 350;

            for (int i = 0; i < maxScrolls; i++)
            {
                try
                {
                    var planesTotales = driver.FindElements(By.XPath("//div[contains(@class,'custom-cursor-on-hover')]"));
                    Console.WriteLine($"DEBUG planes renderizados: {planesTotales.Count}");

                    var planesCoincidentes = driver.FindElements(ObtenerPlanPorNombre(nombre));
                    Console.WriteLine($"DEBUG coincidencias con '{nombre}': {planesCoincidentes.Count}");

                    var plan = planesCoincidentes.FirstOrDefault(e => e.Displayed);

                    if (plan != null)
                    {
                        js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", plan);
                        Thread.Sleep(500);

                        try
                        {
                            plan.Click();
                        }
                        catch
                        {
                            js.ExecuteScript("arguments[0].click();", plan);
                        }

                        Console.WriteLine($"✅ Plan seleccionado correctamente: {nombre}");
                        return;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine("DEBUG: DOM refrescado por Angular");
                }

                long scrollTopAntes = Convert.ToInt64(js.ExecuteScript("return arguments[0].scrollTop;", contenedor));
                long scrollHeight = Convert.ToInt64(js.ExecuteScript("return arguments[0].scrollHeight;", contenedor));
                long clientHeight = Convert.ToInt64(js.ExecuteScript("return arguments[0].clientHeight;", contenedor));

                js.ExecuteScript("arguments[0].scrollTop = arguments[0].scrollTop + arguments[1];", contenedor, scrollStep);
                Thread.Sleep(400);

                long scrollTopDespues = Convert.ToInt64(js.ExecuteScript("return arguments[0].scrollTop;", contenedor));

                Console.WriteLine(
                    $"🔍 intento {i + 1}/{maxScrolls} | scrollTop antes={scrollTopAntes} después={scrollTopDespues} " +
                    $"| scrollHeight={scrollHeight} clientHeight={clientHeight}");

                if (scrollTopAntes == scrollTopDespues)
                {
                    Console.WriteLine("⚠️ El scroll NO se movió. Ese probablemente no es el contenedor correcto.");
                }
            }

            throw new WebDriverTimeoutException(
                $"❌ No se encontró el plan '{nombre}' en el panel derecho después de hacer scroll.");
        }

        public void ConfigurarPaginacion()
        {
            var combo = wait.Until(ExpectedConditions.ElementExists(selectPaginacion));

            // 🔥 Scroll
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", combo);

            // 🔥 FORZAR cambio real en Angular
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
        const select = arguments[0];

        select.value = '100';

        select.dispatchEvent(new Event('input', { bubbles: true }));
        select.dispatchEvent(new Event('change', { bubbles: true }));
        select.dispatchEvent(new Event('blur', { bubbles: true }));
    ", combo);

            // 🔥 pequeña espera (Angular necesita tiempo)
            Thread.Sleep(2000);

            Console.WriteLine("✅ Paginación intentada a 100");

            // 🔥 OPCIONAL: validar (pero sin bloquear)
            try
            {
                var nuevo = driver.FindElement(selectPaginacion);
                string valor = nuevo.GetAttribute("value");

                Console.WriteLine("🔍 Valor actual paginación: " + valor);
            }
            catch { }
        }


        public bool ExistePlanPorTextoDirecto(string nombrePlan)
        {
            try
            {
                By plan = By.XPath($"//div[contains(@class,'p-3') and normalize-space()='{nombrePlan}']");

                WebDriverWait waitCorto = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                waitCorto.Until(driver =>
                {
                    var elementos = driver.FindElements(plan);
                    return elementos.Count > 0;
                });

                Console.WriteLine($"✅ Plan encontrado en DOM: {nombrePlan}");
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"❌ Plan NO encontrado en DOM: {nombrePlan}");
                return false;
            }
        }

        private IWebElement ObtenerContenedorScrollDerecho()
        {
            var js = (IJavaScriptExecutor)driver;

            return (IWebElement)js.ExecuteScript(@"
        const elems = Array.from(document.querySelectorAll('div, section'));

        const scrollables = elems.filter(e => {
            const style = window.getComputedStyle(e);
            const overflowY = style.overflowY;
            const hasVerticalScroll = e.scrollHeight > e.clientHeight + 20;
            const visible = e.offsetWidth > 0 && e.offsetHeight > 0;

            return visible &&
                   hasVerticalScroll &&
                   (overflowY === 'auto' || overflowY === 'scroll');
        });

        if (scrollables.length === 0) return null;

        scrollables.sort((a, b) => {
            const rectA = a.getBoundingClientRect();
            const rectB = b.getBoundingClientRect();

            // Más a la derecha primero
            if (rectB.left !== rectA.left) return rectB.left - rectA.left;

            // Si empatan, preferir el más alto
            return rectB.height - rectA.height;
        });

        return scrollables[0];
    ");
        }
    }   
}