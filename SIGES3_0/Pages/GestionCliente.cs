using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SIGES3_0.Pages
{
    internal class GestionCliente
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public GestionCliente(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // =====================================================
        // SELECTORES
        // =====================================================

        // 🔹 Tabla de clientes
        private By filasClientes = By.XPath("//tr[contains(@class,'table-row')]");

        // 🔹 Botón lupa
        private By botonLupa = By.XPath("//button[@title='Buscar']");

        // 🔹 Opción dar de baja
        private By opcionDarDeBaja = By.XPath("//button[contains(.,'DAR DE BAJA')]");

        // 🔹 Modal
        private By modalConfirmacion = By.XPath("//div[contains(@class, 'modal-body') and .//h5[contains(., 'Confirmación')]]");

        private By btnSi = By.XPath("//div[contains(@class, 'modal-footer')]//button[text()='Sí']");
        private By btnNo = By.XPath("//div[contains(@class, 'modal-footer')]//button[text()='No']");
        // Modal de confirmacion
        private By botonOkExito = By.XPath("//button[contains(@class, 'ok-button') and text()='OK']");

        // 🔹 Estado del cliente
        //private By estadoCliente = By.XPath("//td[contains(@class,'estado')]");
        private By ObtenerEstadoPorTexto(string estado) =>
            By.XPath($"//div[contains(@class,'min-height-40') and normalize-space()='{estado}']");

        private By menuClientes = By.XPath("//span[text()='Clientes']");

        private By columnasFila = By.XPath(".//td");


        private By inputBuscarCliente = By.XPath("//th/input[@type='text' and contains(@class, 'form-control')]");
        private By btnBuscarCliente = By.XPath("//button[contains(@class, 'btn-primary') and @title='Buscar']");

        private By estadosActivos = By.XPath("//div[contains(@class,'min-height-40') and normalize-space()='Activo']");

        private By botonDescargarContrato = By.XPath("//button[contains(@class,'btn-pdf') or .//i[contains(@class,'bi-file-earmark-pdf-fill')]]");
        // =====================================================
        // MÉTODOS
        // =====================================================

        // 🔍 Verificar si existe cliente
        public bool ExisteClienteActivo()
        {
            try
            {
                wait.Until(driver =>
                {
                    return driver.FindElements(By.XPath("//table//tr")).Count > 0
                        || driver.FindElements(estadosActivos).Count > 0;
                });

                bool existe = driver.FindElements(estadosActivos).Any(e => e.Displayed);

                Console.WriteLine(existe
                    ? "✅ Se encontró al menos un cliente en estado Activo"
                    : "⚠️ No se encontró cliente en estado Activo");

                return existe;
            }
            catch
            {
                return false;
            }
        }

        public void IrAModuloClientes()
        {
            var menu = wait.Until(ExpectedConditions.ElementToBeClickable(menuClientes));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", menu);

            Thread.Sleep(300);

            try
            {
                menu.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", menu);
            }

            // Esperar que al menos cargue algo de la pantalla de clientes
            wait.Until(driver =>
            {
                try
                {
                    return driver.FindElements(By.Id("pageSizeSelect")).Count > 0
                        || driver.FindElements(By.XPath("//th/input[@type='text']")).Count > 0
                        || driver.FindElements(By.XPath("//table")).Count > 0;
                }
                catch
                {
                    return false;
                }
            });

            Console.WriteLine("✅ Módulo Clientes cargado");
        }

        // 🔍 Seleccionar cliente (primero disponible)
        public IWebElement ObtenerFilaClienteActivo()
        {
            return wait.Until(driver =>
            {
                try
                {
                    var estadoActivo = driver.FindElements(
                        By.XPath("//tr[.//div[contains(@class,'min-height-40') and normalize-space()='Activo']]")
                    ).FirstOrDefault();

                    return estadoActivo;
                }
                catch
                {
                    return null;
                }
            });
        }



        //public void BuscarClientePorRuc(string ruc)
        //{
        //    var input = wait.Until(ExpectedConditions.ElementIsVisible(inputBuscarCliente));
        //    input.Clear();
        //    input.SendKeys(ruc);

        //    wait.Until(ExpectedConditions.ElementToBeClickable(btnBuscarCliente)).Click();

        //    // 🔥 esperar refresco de tabla
        //    wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(filasClientes));
        //}

        public void BuscarClientePorRuc(string ruc)
        {
            Thread.Sleep(2000); // 🔥 deja respirar a Angular

            var input = wait.Until(ExpectedConditions.ElementIsVisible(inputBuscarCliente));
            input.Clear();
            input.SendKeys(ruc);

            var btn = wait.Until(ExpectedConditions.ElementToBeClickable(btnBuscarCliente));
            btn.Click();

            // 🔥 ESPERA INTELIGENTE (NO EXACTA)
            wait.Until(driver =>
            {
                try
                {
                    var filas = driver.FindElements(filasClientes);

                    // Solo verifica que haya filas (no que contenga el RUC)
                    return filas.Count > 0;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });

            Console.WriteLine("✅ Tabla filtrada (sin validar texto exacto)");
        }

        // 🔎 Click en lupa de la fila
        public void ClickBotonLupa(string ruc)
        {
            IWebElement fila = wait.Until(driver =>
            {
                try
                {
                    var filas = driver.FindElements(filasClientes);

                    return filas.FirstOrDefault(f => f.Text.Contains(ruc));
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });

            if (fila == null)
                throw new Exception("❌ No se encontró la fila del cliente");

            var lupa = fila.FindElement(botonLupa);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", lupa);

            Thread.Sleep(500);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", lupa);

            Console.WriteLine("✅ Click en lupa del cliente");
        }

        // ⬇️ Click "Dar de Baja"
        public void ClickDarDeBaja()
        {
            // 1. Espera a que el botón sea CLICKEABLE (no solo que exista)
            // Usamos un XPath más directo y limpio
            By selectorBaja = By.XPath("//button[contains(normalize-space(),'DAR DE BAJA')]");

            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(selectorBaja));

            // 2. Scroll al centro para que no lo tape el header/footer
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btn);
            Thread.Sleep(500); // Pausa visual para estabilidad

            // 3. INTENTO DOBLE: Clic normal y si falla, clic por JS
            try
            {
                btn.Click();
            }
            catch
            {
                Console.WriteLine("⚠️ Clic normal bloqueado, usando JavaScript en DAR DE BAJA");
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            }

            Console.WriteLine("✅ Click exitoso en DAR DE BAJA");
        }

        // ✅ Confirmar SI
        public void ConfirmarModalSi()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(modalConfirmacion));

            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnSi));
            boton.Click();
        }

        // ❌ Confirmar NO
        public void ConfirmarModalNo()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(modalConfirmacion));

            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnNo));
            boton.Click();
        }

        // 📊 Obtener estado
        public string ObtenerEstadoCliente(string estadoEsperado)
        {
            var estado = wait.Until(ExpectedConditions.ElementIsVisible(
                ObtenerEstadoPorTexto(estadoEsperado)
            ));

            return estado.Text.Trim();
        }

        public void CambiarPaginacionA100()
        {
            int reintentos = 3;

            for (int i = 0; i < reintentos; i++)
            {
                try
                {
                    IWebElement combo = wait.Until(driver =>
                    {
                        try
                        {
                            var el = driver.FindElement(By.Id("pageSizeSelect"));
                            return (el.Displayed && el.Enabled) ? el : null;
                        }
                        catch
                        {
                            return null;
                        }
                    });

                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", combo);

                    Thread.Sleep(300);

                    // 🔥 volver a ubicar antes de cambiar, por si Angular refrescó el DOM
                    combo = driver.FindElement(By.Id("pageSizeSelect"));

                    ((IJavaScriptExecutor)driver).ExecuteScript(@"
                const select = arguments[0];
                select.value = '100';
                select.dispatchEvent(new Event('input', { bubbles: true }));
                select.dispatchEvent(new Event('change', { bubbles: true }));
                select.dispatchEvent(new Event('blur', { bubbles: true }));
            ", combo);

                    Thread.Sleep(2000);

                    Console.WriteLine("✅ Paginación a 100 en clientes");
                    return;
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine($"⚠️ Combo de paginación quedó stale. Reintento {i + 1}/{reintentos}");
                    Thread.Sleep(500);
                }
            }

            throw new Exception("❌ No se pudo cambiar la paginación a 100 por stale element.");
        }

        public string ObtenerRucDeFila(IWebElement fila)
        {
            var celdas = fila.FindElements(columnasFila);

            // Ajusta el índice si el RUC está en otra columna
            // Aquí asumo que alguna celda contiene un número largo identificable
            var textoRuc = celdas
                .Select(td => td.Text.Trim())
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t) && t.All(char.IsDigit) && t.Length >= 8);

            if (string.IsNullOrWhiteSpace(textoRuc))
                throw new Exception("❌ No se pudo obtener el RUC de la fila activa");

            return textoRuc;
        }

        public string ObtenerRucPrimerClienteActivo()
        {
            var fila = ObtenerFilaClienteActivo();

            if (fila == null)
                throw new Exception("❌ No se encontró cliente activo");

            string ruc = ObtenerRucDeFila(fila);

            Console.WriteLine($"✅ Cliente activo encontrado con RUC: {ruc}");
            return ruc;
        }

        public void ClickBotonLupaClienteActivo()
        {
            IWebElement fila = ObtenerFilaClienteActivo();

            if (fila == null)
                throw new Exception("❌ No se encontró cliente activo para abrir");

            var lupa = fila.FindElement(botonLupa);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", lupa);

            Thread.Sleep(500);

            try
            {
                lupa.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", lupa);
            }

            Console.WriteLine("✅ Click en lupa del primer cliente activo");
        }

        public bool EsperarEstadoCliente(string estadoEsperado)
        {
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(
                    ObtenerEstadoPorTexto(estadoEsperado)
                ));

                Console.WriteLine($"✅ Estado visible encontrado: {estadoEsperado}");
                return true;
            }
            catch
            {
                Console.WriteLine($"❌ No se encontró el estado: {estadoEsperado}");
                return false;
            }
        }

        public void CerrarModalOkSiExiste()
        {
            try
            {
                WebDriverWait waitCorto = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

                var btnOk = waitCorto.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[normalize-space()='OK']")
                ));

                btnOk.Click();
                Console.WriteLine("✅ Modal OK cerrado");
            }
            catch
            {
                Console.WriteLine("ℹ️ No apareció modal OK");
            }
        }

        public void ClickDescargarContrato()
        {
            IWebElement btn = wait.Until(ExpectedConditions.ElementToBeClickable(botonDescargarContrato));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btn);

            Thread.Sleep(500);

            try
            {
                btn.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            }

            Console.WriteLine("✅ Click en botón PDF del contrato");
        }

        public bool SeAbrioVistaDeContrato()
        {
            try
            {
                Thread.Sleep(3000);

                // Caso 1: nueva pestaña
                if (driver.WindowHandles.Count > 1)
                {
                    Console.WriteLine("✅ Se abrió una nueva pestaña para el contrato");
                    return true;
                }

                // Caso 2: misma pestaña con visor PDF / print / blob
                string url = driver.Url.ToLowerInvariant();

                if (url.Contains("pdf") || url.Contains("print") || url.Contains("blob:"))
                {
                    Console.WriteLine("✅ Se abrió vista de contrato en la misma pestaña");
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public void PrepararDeteccionImpresion()
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
        window.__printInvocado = false;

        const originalPrint = window.print;
        window.print = function () {
            window.__printInvocado = true;
            if (originalPrint) {
                return originalPrint.apply(window, arguments);
            }
        };
    ");

            Console.WriteLine("✅ Hook de impresión preparado");
        }

        public bool SeInvocoImpresion()
        {
            try
            {
                WebDriverWait waitPrint = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                return waitPrint.Until(d =>
                {
                    try
                    {
                        var resultado = ((IJavaScriptExecutor)d)
                            .ExecuteScript("return window.__printInvocado === true;");
                        return resultado is bool b && b;
                    }
                    catch
                    {
                        return false;
                    }
                });
            }
            catch
            {
                return false;
            }
        }
    }
}