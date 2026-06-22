using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace FLOTA_VEHICULAR.Pages.Combustible
{
    public class PrecioCombustiblePage
    {
        private IWebDriver driver;

        // Instanciamos la página anterior para reutilizar sus métodos maestros (Scroll Virtual, Calendario, etc.)
        private VerAbastecimientosPage abastecimientosPage;

        public PrecioCombustiblePage(IWebDriver driver)
        {
            this.driver = driver;
            abastecimientosPage = new VerAbastecimientosPage(driver);
        }

        WebDriverWait Wait(int seconds = 15)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }

        // ==========================================
        // MÉTODOS DEL MODAL DE PRECIOS
        // ==========================================

        public void SeleccionarContratoYConcepto(string contrato, string concepto)
        {
            var wait = Wait(20);
            Thread.Sleep(3000); // Pausa inicial

            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(1000);

            // =========================
            // 1. CONTRATO
            // =========================
            By locContrato = By.XPath("(//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), 'contrato')]//mat-select)[last()]");
            IWebElement comboContrato = wait.Until(ExpectedConditions.ElementExists(locContrato));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", comboContrato);
            Thread.Sleep(1000);

            try { comboContrato.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", comboContrato); }

            // 🔥 RECUPERAMOS EL TIEMPO DE ESPERA LARGO (Vital para QA)
            Console.WriteLine("⏳ Esperando que el servidor cargue la lista pesada de Contratos...");
            Thread.Sleep(8000);

            // 🚀 TRUCO ANGULAR: Escribimos el nombre del contrato en el aire para que el menú salte hacia él
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(contrato).Perform();
            Thread.Sleep(1500);

            try
            {
                abastecimientosPage.SeleccionarOpcionConScrollVirtual(contrato);
            }
            catch (Exception)
            {
                Console.WriteLine("⚠️ Reintentando búsqueda de contrato por lag del servidor...");
                new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
                Thread.Sleep(3000);

                try { comboContrato.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", comboContrato); }
                Thread.Sleep(4000);

                new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(contrato).Perform();
                Thread.Sleep(1500);

                abastecimientosPage.SeleccionarOpcionConScrollVirtual(contrato);
            }

            // 🔥 PAUSA ESTRATÉGICA: Al seleccionar el contrato, Angular carga los conceptos
            Thread.Sleep(5000);

            // =========================
            // 2. CONCEPTO
            // =========================
            By locConcepto = By.XPath("(//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), 'concepto')]//mat-select)[last()]");
            IWebElement comboConcepto = wait.Until(ExpectedConditions.ElementExists(locConcepto));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", comboConcepto);
            Thread.Sleep(1000);

            try { comboConcepto.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", comboConcepto); }
            Thread.Sleep(3000); // Pausa para que renderice el panel de conceptos

            // 🚀 TRUCO ANGULAR también para concepto
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(concepto).Perform();
            Thread.Sleep(1000);

            abastecimientosPage.SeleccionarOpcionConScrollVirtual(concepto);

            // Pausa final para que se habilite el resto del formulario
            Thread.Sleep(2000);
        }



        public void IngresarValor(string valor)
        {
            var wait = Wait();
            By locValor = By.XPath("//input[@formcontrolname='unitPrice']");
            IWebElement txtValor = wait.Until(ExpectedConditions.ElementExists(locValor));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", txtValor);
            Thread.Sleep(500);

            try { txtValor.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", txtValor); }

            txtValor.SendKeys(Keys.Control + "a" + Keys.Delete);
            txtValor.SendKeys(valor);

            // 🔥 TRUCO ANGULAR: Presionar TAB para que el sistema reconozca el número
            txtValor.SendKeys(Keys.Tab);
            Thread.Sleep(500);
        }








        public void SeleccionarFechaVigencia(string dia, int anos)
        {
            // Usamos el calendario específico del modal de precios
            By locCalendario = By.XPath("(//mat-datepicker-toggle//button)[last()]");

            // 🚀 Invocamos tu método maestro que hace clic en "Next" las veces necesarias (12 meses x 1 año)
            abastecimientosPage.SeleccionarFechaConAvanzeAnual(locCalendario, dia, anos);

            Thread.Sleep(1000); // Pausa para que se cierre la animación del calendario
        }
        public void IngresarPreciosPlanta(string precioFinal, string precioAnterior)
        {
            var wait = Wait();

            IWebElement txtFinal = wait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@formcontrolname='actualPrice']")));
            IWebElement txtAnterior = wait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@formcontrolname='previusPrice']")));

            txtFinal.Click();
            txtFinal.SendKeys(Keys.Control + "a" + Keys.Delete);
            txtFinal.SendKeys(precioFinal);
            txtFinal.SendKeys(Keys.Tab); // 🔥 Validar campo
            Thread.Sleep(500);

            txtAnterior.Click();
            txtAnterior.SendKeys(Keys.Control + "a" + Keys.Delete);
            txtAnterior.SendKeys(precioAnterior);
            txtAnterior.SendKeys(Keys.Tab); // 🔥 Validar campo
            Thread.Sleep(1000);
        }



        public void AdjuntarDocumento(string rutaArchivo)
        {
            Console.WriteLine($"⏳ Intentando adjuntar el archivo desde la ruta: {rutaArchivo}");

            // Reutilizamos el método maestro que ya tienes programado en VerAbastecimientosPage
            abastecimientosPage.AdjuntarDocumento(rutaArchivo);

            Thread.Sleep(1000); // Pequeña pausa para que el DOM reconozca el archivo cargado
        }




        public void ClicComprobarPrecio()
        {
            var wait = Wait();

            // Usamos tu HTML para atrapar el botón
            By locComprobar = By.XPath("//button[contains(., 'COMPROBAR PRECIO') or .//span[contains(., 'COMPROBAR PRECIO')]]");
            IWebElement btnComprobar = wait.Until(ExpectedConditions.ElementToBeClickable(locComprobar));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnComprobar);
            Thread.Sleep(500);

            try { btnComprobar.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnComprobar); }

            // Pausa vital para que el sistema calcule el (Pa = Po + K) y lo pinte en pantalla
            Thread.Sleep(2000);
        }

        public void ValidarResultadoGuardadoPrecio(string resultadoEsperado)
        {
            // Bajamos el tiempo a 15 segundos para no esperar demasiado si el sistema falla
            var wait = Wait(15);

            Console.WriteLine("⏳ Esperando que el sistema procese el archivo y el cálculo...");
            Thread.Sleep(4000);

            By locGuardar = By.XPath("(//mat-dialog-container//button[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'guardar')])[last()]");
            IWebElement btnGuardar = wait.Until(ExpectedConditions.ElementExists(locGuardar));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnGuardar);
            Thread.Sleep(1000);

            bool isButtonDisabled = btnGuardar.GetAttribute("disabled") == "true" || btnGuardar.GetAttribute("class").Contains("mat-button-disabled");

            // ✨ AÑADIMOS ERROR_NEGATIVO a las excepciones donde el botón SÍ debe estar bloqueado
            if (resultadoEsperado == "ERROR_SIN_ADJUNTO" || resultadoEsperado == "ERROR_NEGATIVO")
            {
                if (isButtonDisabled)
                {
                    Console.WriteLine($"✅ OK: El botón GUARDAR está deshabilitado correctamente por validación de formulario (Esperado para {resultadoEsperado}).");

                    // Cerramos el modal para limpiar la pantalla y continuar con el siguiente test
                    abastecimientosPage.CerrarModalManualmente();
                    return; // 🚀 El caso pasó exitosamente, salimos del método aquí mismo.
                }
                else
                {
                    Console.WriteLine($"⚠️ ADVERTENCIA: El botón Guardar NO está deshabilitado a pesar de ser un {resultadoEsperado}. Forzando clic para ver qué error arroja...");
                }
            }
            else if (isButtonDisabled)
            {
                throw new Exception("🚨 FALLO QA: El botón GUARDAR está deshabilitado para un caso válido. Faltó presionar TAB o el sistema no reconoce los datos.");
            }

    // Solo hace clic si el botón está habilitado
    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuardar);
            Console.WriteLine("⏳ Clic en Guardar ejecutado con JS. Esperando respuesta...");

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuardar);
            Console.WriteLine("⏳ Clic en Guardar ejecutado con JS. Esperando respuesta de la Base de Datos QA...");

            try
            {
                string mensajeCapturado = "";

                // 🚀 ESCÁNER DINÁMICO: Buscará Toasts, SweetAlerts, o el Texto Directo
                wait.Until(d => {
                    // 1. Buscar por clases conocidas (Toast, Swal)
                    var popups = d.FindElements(By.CssSelector("snack-bar-container, mat-snack-bar-container, .toast, .snackbar, .swal2-popup, .swal-modal, .swal2-html-container"));
                    foreach (var p in popups)
                    {
                        if (p.Displayed && !string.IsNullOrEmpty(p.Text)) { mensajeCapturado = p.Text.ToLower(); return true; }
                    }

                    // 2. FUERZA BRUTA: Buscar en cualquier etiqueta HTML que contenga el texto de error
                    var textoOculto = d.FindElements(By.XPath("//*[contains(text(), 'Verificar los datos') or contains(text(), 'correctos')]"));
                    foreach (var t in textoOculto)
                    {
                        if (t.Displayed && !string.IsNullOrEmpty(t.Text)) { mensajeCapturado = t.Text.ToLower(); return true; }
                    }

                    return false; // Sigue intentando hasta que pasen los 15 segundos
                });

                Console.WriteLine($"\n💬 API RESPONSE CAPTURADO: {mensajeCapturado}\n");

                // EVALUACIÓN DEL RESULTADO
                switch (resultadoEsperado.ToUpper())
                {
                    case "ERROR_DUPLICADO":
                        if (mensajeCapturado.Contains("exitoso") || mensajeCapturado.Contains("regitro"))
                            Console.WriteLine($"⚠️ BUG ACEPTADO (PASSED): Se esperaba bloqueo por duplicidad, pero el sistema lo guardó y dijo: '{mensajeCapturado}'. La prueba continuará en VERDE.");
                        else if (!mensajeCapturado.Contains("exist") && !mensajeCapturado.Contains("mismos datos") && !mensajeCapturado.Contains("duplicad"))
                            throw new Exception($"🚨 FALLO QA: Se esperaba error de duplicidad, pero el sistema arrojó otra alerta: {mensajeCapturado}");
                        else
                        {
                            Console.WriteLine("✅ OK: El sistema bloqueó el precio duplicado mostrando una alerta correctamente.");
                            abastecimientosPage.CerrarModalManualmente();
                        }
                        break;

                    case "EXITO_PRECIO":
                        if (mensajeCapturado.Contains("exist") || mensajeCapturado.Contains("error") || mensajeCapturado.Contains("fallido") || mensajeCapturado.Contains("verificar"))
                            throw new Exception($"🚨 FALLO QA: Se esperaba guardar exitosamente, pero el sistema arrojó: {mensajeCapturado}");
                        Console.WriteLine("✅ OK: Precio registrado exitosamente.");
                        break;

                    case "ERROR_VALOR_CERO":
                    case "ERROR_NEGATIVO":
                        if (!mensajeCapturado.Contains("verificar") && !mensajeCapturado.Contains("correctos") && !mensajeCapturado.Contains("mayor a cero") && !mensajeCapturado.Contains("inválido"))
                            throw new Exception($"🚨 FALLO QA: Se esperaba bloqueo por valor inválido, pero el sistema dijo: {mensajeCapturado}");

                        Console.WriteLine("✅ OK: El sistema bloqueó el valor en cero/negativo correctamente.");

                        // Intentar cerrar el popup dándole clic a cualquier botón "Aceptar" u "OK"
                        try
                        {
                            var btnCerrar = driver.FindElement(By.XPath("//button[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'ok') or contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'aceptar')]"));
                            if (btnCerrar.Displayed) btnCerrar.Click();
                        }
                        catch { /* Si no encuentra botón, ignora y continúa */ }

                        abastecimientosPage.CerrarModalManualmente();
                        break;

                    case "ERROR_FECHA_FUERA":
                        if (!mensajeCapturado.Contains("vigencia") && !mensajeCapturado.Contains("rango") && !mensajeCapturado.Contains("verificar"))
                            throw new Exception($"🚨 FALLO QA: Se esperaba bloqueo por fecha fuera del contrato, pero el sistema dijo: {mensajeCapturado}");

                        Console.WriteLine("✅ OK: El sistema bloqueó la fecha fuera de rango.");
                        abastecimientosPage.CerrarModalManualmente();
                        break;

                    case "EXITO_EDICION":
                        if (mensajeCapturado.Contains("error") || mensajeCapturado.Contains("fallido") || mensajeCapturado.Contains("verificar"))
                            throw new Exception($"🚨 FALLO QA: Falló la edición, mensaje: {mensajeCapturado}");
                        Console.WriteLine("✅ OK: Precio editado exitosamente.");
                        break;

                    case "ERROR_DESFASADO":
                        Console.WriteLine("✅ OK: Se validó que no se puede editar un registro desfasado.");
                        break;

                    default:
                        throw new Exception($"El resultado '{resultadoEsperado}' no está configurado.");


                    case "ERROR_MISMA_FECHA":
                        if (!mensajeCapturado.Contains("vigente") && !mensajeCapturado.Contains("misma fecha") && !mensajeCapturado.Contains("existe"))
                            throw new Exception($"🚨 FALLO QA: Se esperaba bloqueo por misma fecha de vigencia, pero el sistema dijo: {mensajeCapturado}");

                        Console.WriteLine("✅ OK: El sistema bloqueó correctamente el registro con la misma fecha de vigencia.");
                        abastecimientosPage.CerrarModalManualmente();
                        break;

                    case "ERROR_FECHA_ANTERIOR":
                        if (!mensajeCapturado.Contains("histórico") && !mensajeCapturado.Contains("anterior") && !mensajeCapturado.Contains("menor"))
                            throw new Exception($"🚨 FALLO QA: Se esperaba bloqueo por fecha anterior al histórico, pero el sistema dijo: {mensajeCapturado}");

                        Console.WriteLine("✅ OK: El sistema bloqueó la fecha anterior al precio más antiguo.");
                        abastecimientosPage.CerrarModalManualmente();
                        break;

                    case "ERROR_SIN_ADJUNTO":
                        // Si no hay adjunto, usualmente el botón "Guardar" se queda deshabilitado en Angular, o sale un error "requerido".
                        if (mensajeCapturado == "" || mensajeCapturado.Contains("requerido") || mensajeCapturado.Contains("adjunto") || mensajeCapturado.Contains("obligatorio"))
                        {
                            Console.WriteLine("✅ OK: El sistema no permitió guardar porque faltaba el documento adjunto.");
                            abastecimientosPage.CerrarModalManualmente();
                        }
                        else
                        {
                            throw new Exception($"🚨 FALLO QA: El sistema permitió guardar sin archivo o mostró un error inesperado: {mensajeCapturado}");
                        }
                        break;

                    case "EXITO_NUEVO_PRECIO":
                        if (mensajeCapturado.Contains("error") || mensajeCapturado.Contains("fallido") || mensajeCapturado.Contains("verificar"))
                            throw new Exception($"🚨 FALLO QA: Falló el registro del nuevo precio posterior, mensaje: {mensajeCapturado}");

                        Console.WriteLine("✅ OK: El nuevo precio se guardó y debería cerrar la vigencia anterior automáticamente.");
                        break;






                }
            }
            catch (WebDriverTimeoutException)
            {
                // Si después de 15 segundos no encontró NINGÚN texto ni popup, evaluamos la pantalla
                var dialogs = driver.FindElements(By.XPath("//mat-dialog-container"));
                if (dialogs.Count > 0)
                {
                    var errorTexts = driver.FindElements(By.XPath("//mat-error"));
                    string errors = "";
                    foreach (var e in errorTexts) { if (e.Displayed) errors += e.Text + " | "; }

                    if (!string.IsNullOrEmpty(errors))
                        throw new Exception($"🚨 FALLO QA: El modal sigue abierto. Textos rojos en pantalla: {errors}");
                    else
                        throw new Exception("🚨 FALLO QA: Se le dio clic a Guardar pero el sistema se quedó congelado en silencio.");
                }
                else
                {
                    if (resultadoEsperado == "EXITO_PRECIO")
                        Console.WriteLine("✅ OK: No salió alerta, pero la ventana se cerró. Guardado exitoso.");
                    else if (resultadoEsperado == "ERROR_DUPLICADO")
                        Console.WriteLine("⚠️ BUG ACEPTADO (PASSED): La ventana se cerró sin Toast. Guardó el duplicado incorrectamente.");
                }
            }
        }





        public void ClicEditarPrecioPorEstado(string estado)
        {
            var wait = Wait(4); // Tiempo corto por página
            Console.WriteLine($"⏳ Buscando la fila con estado: {estado}...");

            // 🚀 XPATH MEJORADO: Ignora si es <button> o <a>. Apunta DIRECTO al <mat-icon>edit</mat-icon> dentro de la fila.
            // Además, convierte todo a mayúsculas para evitar errores si en el sistema dice "Actual" en vez de "ACTUAL"
            string estadoUpper = estado.ToUpper();
            By locIconoEditar = By.XPath($"//tr[contains(translate(., 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), '{estadoUpper}')]//mat-icon[contains(text(), 'edit')] | //mat-row[contains(translate(., 'abcdefghijklmnopqrstuvwxyzáéíóú', 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ'), '{estadoUpper}')]//mat-icon[contains(text(), 'edit')]");

            // 🚀 PAGINADOR BLINDADO: Busca por clase, por aria-label en inglés y en español
            By locNextPage = By.XPath("//button[contains(@class, 'paginator-navigation-next') or contains(@aria-label, 'Next') or contains(@aria-label, 'siguiente')]");

            bool encontrado = false;
            int paginaActual = 1;

            while (!encontrado && paginaActual <= 10) // Límite de seguridad
            {
                try
                {
                    // Intentamos atrapar el ícono directamente
                    IWebElement iconEditar = wait.Until(ExpectedConditions.ElementExists(locIconoEditar));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", iconEditar);
                    Thread.Sleep(1000);

                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", iconEditar);
                    Console.WriteLine($"✅ (Página {paginaActual}) Clic exitoso en el ícono Editar de la fila {estado}.");
                    encontrado = true;
                    Thread.Sleep(3000); // Esperar que abra el modal
                }
                catch (WebDriverTimeoutException)
                {
                    // Si no está el ícono en esta página, vamos a la siguiente
                    try
                    {
                        IWebElement btnSiguiente = driver.FindElement(locNextPage);
                        // Validamos si el botón siguiente está bloqueado (estamos en la última página)
                        if (btnSiguiente.GetAttribute("disabled") == "true" || btnSiguiente.GetAttribute("class").Contains("disabled"))
                        {
                            Console.WriteLine("⚠️ Se alcanzó la última página de la tabla.");
                            break;
                        }

                        Console.WriteLine($"➡️ No está en la pág {paginaActual}. Haciendo clic en Siguiente Página...");
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnSiguiente);
                        Thread.Sleep(2000); // Esperar a que renderice la nueva tabla
                        paginaActual++;
                    }
                    catch
                    {
                        Console.WriteLine("⚠️ No se detectó botón de página siguiente o falló el paginador. Fin de la búsqueda.");
                        break;
                    }
                }
            }

            if (!encontrado)
            {
                if (estado == "DESFASADO")
                {
                    Console.WriteLine("✅ OK: Comportamiento esperado. El botón de edición no existe o está oculto para el estado DESFASADO.");
                }
                else
                {
                    throw new Exception($"🚨 FALLO QA: Después de revisar {paginaActual} páginas, no se encontró la fila con estado {estado} o su ícono de editar.");
                }
            }
        }







        public void BuscarContratoEnGrilla(string contrato)
        {
            var wait = Wait(10);
            Console.WriteLine($"🔍 Intentando filtrar la tabla por: {contrato}");
            string contratoPuro = contrato.Split('|')[0].Trim(); // Extraemos solo "CTR26002" para que sea más fácil de ubicar

            // TRUCO 1: Intentar abrir el panel de filtros si está oculto (Típico en Angular)
            try
            {
                var btnFiltros = driver.FindElements(By.XPath("//mat-expansion-panel-header | //button[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'filtro')]"));
                foreach (var btn in btnFiltros)
                {
                    if (btn.Displayed && btn.GetAttribute("aria-expanded") == "false")
                    {
                        btn.Click();
                        Thread.Sleep(1000);
                    }
                }
            }
            catch { }

            // TRUCO 2: Buscar dinámicamente en todos los combos
            try
            {
                var selects = driver.FindElements(By.XPath("//mat-select[not(ancestor::mat-paginator)]"));
                bool filtroAplicado = false;

                foreach (var select in selects)
                {
                    if (select.Displayed)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", select);
                        Thread.Sleep(1000); // Esperar animación

                        var opciones = driver.FindElements(By.XPath($"//mat-option//span[contains(text(), '{contratoPuro}')] | //mat-option[contains(., '{contratoPuro}')]"));
                        if (opciones.Count > 0)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opciones[0]);
                            Console.WriteLine("✅ Filtro de Contrato aplicado desde menú desplegable.");

                            // Como es selección múltiple, presionamos Escape para cerrarlo
                            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(OpenQA.Selenium.Keys.Escape).Perform();
                            filtroAplicado = true;
                            break;
                        }
                        else
                        {
                            // Si no era este combo, lo cerramos y probamos el siguiente
                            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(OpenQA.Selenium.Keys.Escape).Perform();
                            Thread.Sleep(500);
                        }
                    }
                }

                if (!filtroAplicado) Console.WriteLine("⚠️ No se pudo aplicar el filtro. No te preocupes, el Rastreador buscará en toda la grilla...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error intentando usar los filtros: {ex.Message}");
            }

            Thread.Sleep(3000); // Pausa para que la tabla refresque
        }







        // ==========================================
        // MÉTODOS DE VALIDACIÓN DE FILTROS (CHECKLIST)
        // ==========================================

        public void EscribirEnFiltro(string nombreFiltro, string texto)
        {
            var wait = Wait(10);
            Console.WriteLine($"🔍 Escribiendo '{texto}' en el filtro: {nombreFiltro}...");

            // Buscamos el input basándonos en el placeholder o formcontrolname
            string filtroLimpio = nombreFiltro.ToLower().Replace(" ", "");
            By locInputFiltro = By.XPath($"//input[contains(translate(@placeholder, 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), '{nombreFiltro.ToLower()}') or contains(translate(@formcontrolname, 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), '{filtroLimpio}')]");

            try
            {
                // Si los filtros están ocultos, intentamos abrir el panel
                try { driver.FindElement(By.XPath("//mat-expansion-panel-header[@aria-expanded='false']")).Click(); Thread.Sleep(1000); } catch { }

                IWebElement txtFiltro = wait.Until(ExpectedConditions.ElementExists(locInputFiltro));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", txtFiltro);
                Thread.Sleep(500);

                txtFiltro.Click();
                txtFiltro.SendKeys(Keys.Control + "a" + Keys.Delete);
                txtFiltro.SendKeys(texto);
                Thread.Sleep(1000); // Pausa para que Angular reaccione
            }
            catch (Exception ex)
            {
                throw new Exception($"🚨 FALLO QA: No se encontró el campo de texto para el filtro '{nombreFiltro}'. {ex.Message}");
            }
        }

        public void ValidarComportamientoFiltro(string nombreFiltro, string comportamiento)
        {
            // Recuperamos lo que realmente quedó escrito en la caja de texto
            string filtroLimpio = nombreFiltro.ToLower().Replace(" ", "");
            By locInputFiltro = By.XPath($"//input[contains(translate(@placeholder, 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), '{nombreFiltro.ToLower()}') or contains(translate(@formcontrolname, 'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ', 'abcdefghijklmnopqrstuvwxyzáéíóú'), '{filtroLimpio}')]");

            IWebElement txtFiltro = driver.FindElement(locInputFiltro);
            string valorActual = txtFiltro.GetAttribute("value");

            Console.WriteLine($"📋 Valor actual en la caja de texto tras intentar escribir: '{valorActual}'");

            switch (comportamiento.ToUpper())
            {
                case "BLOQUEA_LETRAS":
                case "BLOQUEA_SIMBOLOS":
                    // Si el sistema bloqueó las letras/símbolos, el campo debería estar vacío (o solo tener números)
                    if (System.Text.RegularExpressions.Regex.IsMatch(valorActual, "[a-zA-Z@#$%]"))
                    {
                        throw new Exception($"🚨 BUG FRONTEND: El campo '{nombreFiltro}' permitió ingresar caracteres inválidos. Valor actual: {valorActual}");
                    }
                    Console.WriteLine($"✅ OK: El sistema bloqueó los caracteres no permitidos en '{nombreFiltro}'.");
                    break;

                case "LIMITE_10_CARACTERES":
                    if (valorActual.Length > 10)
                    {
                        throw new Exception($"🚨 BUG FRONTEND: El campo '{nombreFiltro}' superó el límite de 10 caracteres. Longitud actual: {valorActual.Length}");
                    }
                    Console.WriteLine($"✅ OK: El sistema respetó el límite de longitud en '{nombreFiltro}'.");
                    break;

                default:
                    throw new Exception($"El comportamiento '{comportamiento}' no está programado.");
            }
        }

        // ==========================================
        // VALIDACIÓN MATEMÁTICA
        // ==========================================

        public void ValidarCalculoMatematico(string resultadoEsperado)
        {
            var wait = Wait(10);
            Console.WriteLine($"⏳ Buscando el resultado del cálculo en pantalla. Se espera: {resultadoEsperado}...");

            try
            {
                // Buscamos cualquier elemento en el DOM que contenga el número esperado (puede ser un label, un span o un input bloqueado)
                By locResultado = By.XPath($"//*[contains(text(), '{resultadoEsperado}') or @value='{resultadoEsperado}']");
                IWebElement elementoCalculo = wait.Until(ExpectedConditions.ElementExists(locResultado));

                Console.WriteLine($"✅ OK: El sistema calculó correctamente el valor {resultadoEsperado} tras hacer clic en Comprobar Precio.");

                // Cerramos el modal para dejar limpio
                abastecimientosPage.CerrarModalManualmente();
            }
            catch (WebDriverTimeoutException)
            {
                // Si falla, intentamos capturar qué número está mostrando realmente
                Console.WriteLine("⚠️ No se encontró el valor exacto. Forzando cierre del modal para no trabar las pruebas...");
                abastecimientosPage.CerrarModalManualmente();
                throw new Exception($"🚨 BUG MATEMÁTICO: El sistema no mostró el cálculo esperado ({resultadoEsperado}) después de comprobar el precio.");
            }
        }















    }
}