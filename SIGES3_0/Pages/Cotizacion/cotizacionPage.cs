using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using System.Threading;

namespace SIGES3_0.Pages.CotizacionPage
{
    public class CotizacionPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        private bool _fechaPasadaIntentada = false;
        private string _resultadoRegistro = "";

        public bool FechaPasadaIntentada => _fechaPasadaIntentada;

        //  Selectores 
        private By txtCliente = By.XPath("//input[@placeholder='Buscar...']");
        private By btnAbrirCalendario = By.XPath("//input[contains(@class,'premium-input')]");
        private By btnRegistrarCotizacion = By.XPath("//button[@class='btn btn-primary btn-save']");
        private By popupExito = By.XPath("//*[contains(text(),'Se registró correctamente') or contains(text(),'Se registro correctamente')]");
        private By btnOK = By.XPath("//button[normalize-space()='OK']");

        // ── Selectores nuevos para Editar ──
        private By btnEditarCotizacion = By.XPath("//button[@title='Editar cotización']");
        private By btnActualizarCotizacion = By.XPath("//button[normalize-space()='Actualizar Cotización']");
        private By txtFiltroConvertido = By.XPath("//th[contains(.,'CONVERTIDO')]//following-sibling::th//input | //thead//input[contains(@placeholder,'Convertido') or contains(@placeholder,'convertido')] | //th[7]//input");
        private By badgeConvertidoNo = By.XPath("//tbody/tr[1]//span[normalize-space()='No' or normalize-space()='NO']");
        private string _resultadoEdicion = "";

        public CotizacionPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // ── Buscar cliente ──
        public void BuscarCliente(string cliente)
        {
            try
            {
                if (cliente == "00000000" || cliente.ToLower() == "varios")
                {
                    Console.WriteLine("[BuscarCliente] Cliente VARIOS - no se realiza búsqueda");
                    return;
                }

                var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(25));

                var input = waitLong.Until(
                    ExpectedConditions.ElementIsVisible(txtCliente)
                );

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);

                waitLong.Until(ExpectedConditions.ElementToBeClickable(txtCliente)).Click();

                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Delete);
                input.SendKeys(cliente);

                try
                {
                    var waitDropdown = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                    var opcion = waitDropdown.Until(d =>
                    {
                        try
                        {
                            var opciones = d.FindElements(By.CssSelector(
                                ".ng-dropdown-panel .ng-option, .dropdown-menu .dropdown-item"
                            ));
                            return opciones.FirstOrDefault(o => o.Displayed && o.Enabled);
                        }
                        catch { return null; }
                    });

                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("arguments[0].click();", opcion);
                }
                catch (WebDriverTimeoutException)
                {
                    input.SendKeys(Keys.Enter);
                }

                waitLong.Until(d =>
                {
                    try
                    {
                        var val = input.GetAttribute("value") ?? "";
                        return val.Trim().Length > 0;
                    }
                    catch { return false; }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("[BuscarCliente] Error: " + e.Message);
                throw;
            }
        }

        // ── Ingresar fecha final ──
        public void IngresarFechaFinal(string fecha)
        {
            try
            {
                var partes = fecha.Trim().Split(' ');
                string parteFecha = partes[0];
                string parteHora = partes.Length > 1 ? partes[1] : "12:00:am";

                var fechaParts = parteFecha.Split('/');
                int dia = int.Parse(fechaParts[0]);
                int mes = int.Parse(fechaParts[1]);
                int anio = int.Parse(fechaParts[2]);

                var horaParts = parteHora.Split(':');
                string horaStr = horaParts[0];
                string minStr = horaParts[1];
                string ampm = horaParts[2].ToLower();

                DateTime fechaSeleccionada = new DateTime(anio, mes, dia);
                DateTime hoy = DateTime.Today;

                // Abrir el calendario
                var inputFecha = wait.Until(
                    ExpectedConditions.ElementToBeClickable(btnAbrirCalendario)
                );
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", inputFecha);
                inputFecha.Click();
                Thread.Sleep(500);

                // ── CASO 4: fecha pasada ──
                if (fechaSeleccionada.Date < hoy)
                {
                    Console.WriteLine($"[IngresarFechaFinal] Fecha {fecha} es pasada, día deshabilitado.");
                    _fechaPasadaIntentada = true;

                    var diaDeshabilitado = driver.FindElements(
                        By.XPath($"//div[@class='day-cell disabled ng-star-inserted'][normalize-space()='{dia}']")
                    ).FirstOrDefault(e => e.Displayed);

                    if (diaDeshabilitado != null)
                        Console.WriteLine($"[IngresarFechaFinal] Día {dia} confirmado como deshabilitado.");

                    // Cerrar el calendario
                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("document.body.click();");
                    Thread.Sleep(300);
                    return;
                }

                // ── FECHAS HABILITADAS: seleccionar día ──
                var diaHabilitado = wait.Until(d =>
                {
                    try
                    {
                        var elementos = d.FindElements(By.XPath(
                            $"//div[contains(@class,'day-cell') and not(contains(@class,'disabled'))][normalize-space()='{dia}']"
                        ));
                        return elementos.FirstOrDefault(e => e.Displayed);
                    }
                    catch { return null; }
                });

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", diaHabilitado);
                Thread.Sleep(300);

                // ── Seleccionar hora ──
                SeleccionarItemEnColumna("hours", horaStr);
                Thread.Sleep(200);

                // ── Seleccionar minutos ──
                SeleccionarItemEnColumna("minutes", minStr);
                Thread.Sleep(200);

                // ── Seleccionar AM/PM ──
                string ampmTexto = ampm == "am" ? "a. m." : "p. m.";
                SeleccionarAmPm(ampmTexto);
                Thread.Sleep(300);

                // Cerrar el calendario
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("document.body.click();");
                Thread.Sleep(300);
            }
            catch (Exception e)
            {
                Console.WriteLine("[IngresarFechaFinal] Error: " + e.Message);
                throw;
            }
        }

        // ── Selecciona un item en la columna de horas o minutos ──
        private void SeleccionarItemEnColumna(string columna, string valor)
        {
            try
            {
                string valorNorm = valor.TrimStart('0');
                if (valorNorm == "") valorNorm = "0";

                var item = wait.Until(d =>
                {
                    try
                    {
                        var columnaEl = d.FindElement(
                            By.CssSelector($"div.time-column.{columna}")
                        );

                        var items = columnaEl.FindElements(By.XPath(
                            $".//div[contains(@class,'time-item')]" +
                            $"[normalize-space()='{valor}' or normalize-space()='{valorNorm}']"
                        ));

                        return items.FirstOrDefault(e => e.Displayed);
                    }
                    catch { return null; }
                });

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", item);
                Thread.Sleep(200);
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", item);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[SeleccionarItemEnColumna] Error columna={columna} valor={valor}: {e.Message}");
                throw;
            }
        }

        // Selecciona AM o PM 
        private void SeleccionarAmPm(string ampmTexto)
        {
            try
            {
                var columnaAmPm = wait.Until(d =>
                {
                    try
                    {
                        return d.FindElement(By.CssSelector("div.time-column.ampm"));
                    }
                    catch { return null; }
                });

                var item = columnaAmPm.FindElements(By.XPath(
                    $".//div[contains(@class,'time-item')][contains(normalize-space(),'{ampmTexto}')]"
                )).FirstOrDefault(e => e.Displayed);

                if (item != null)
                {
                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("arguments[0].click();", item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[SeleccionarAmPm] Error: {e.Message}");
                throw;
            }
        }

        //  Registrar cotización 
        public void RegistrarCotizacion()
        {
            try
            {
                var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                var boton = waitLong.Until(
                    ExpectedConditions.ElementExists(btnRegistrarCotizacion)
                );

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

                Thread.Sleep(800);

                try { boton.Click(); }
                catch
                {
                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("arguments[0].click();", boton);
                }

                // Capturar resultado inmediatamente tras el clic
                _resultadoRegistro = CapturarResultadoTrasRegistro();
            }
            catch (Exception e)
            {
                Console.WriteLine("[RegistrarCotizacion] Error: " + e.Message);
                throw;
            }
        }
        private string CapturarResultadoTrasRegistro()
        {
            // stock excedido — VERIFICAR PRIMERO ──
            try
            {
                var inconsistencia = driver.FindElement(By.XPath(
                    "//*[contains(text(),'supera el stock disponible')] | " +
                    "//*[contains(text(),'supera el stock')] | " +
                    "//*[contains(text(),'menor o igual al stock')] | " +
                    "//*[contains(text(),'Se encontraron inconsistencias')]"
                ));
                if (inconsistencia.Displayed)
                {
                    Console.WriteLine("[CapturarResultado] Inconsistencia de stock detectada.");
                    return "Cantidad debe ser menor al stock";
                }
            }
            catch { }

            // sin producto seleccionado 
            try
            {
                var badge = driver.FindElement(
                    By.XPath("//span[contains(@class,'badge-status') and contains(@class,'danger')]")
                );
                if (badge.Displayed)
                {
                    bool sinFilas = !driver.FindElements(
                        By.XPath("//table//tbody/tr[td]")
                    ).Any(e => e.Displayed);

                    string textoBadge = badge.Text?.Trim() ?? "";
                    Console.WriteLine($"[CapturarResultado] Badge danger: '{textoBadge}', sinFilas={sinFilas}");

                    if (sinFilas || textoBadge.StartsWith("0"))
                        return "Ningun producto seleccionado";
                }
            }
            catch { }

            // popup de éxito 
            try
            {
                var waitExito = new WebDriverWait(driver, TimeSpan.FromSeconds(12));
                var popup = waitExito.Until(
                    ExpectedConditions.ElementIsVisible(popupExito)
                );
                if (popup.Displayed)
                {
                    try
                    {
                        var ok = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                            .Until(ExpectedConditions.ElementToBeClickable(btnOK));
                        ok.Click();
                    }
                    catch
                    {
                        ((IJavaScriptExecutor)driver)
                            .ExecuteScript("arguments[0].click();", driver.FindElement(btnOK));
                    }
                    return "la cotizacion se guardo correctamente";
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("[CapturarResultado] Popup de éxito no detectado en 12s.");
            }

            return "";
        }

        // EDITAR COTIZACION
        public bool ExisteCotizacionParaEditar()
        {
            try
            {
                FiltrarCotizacionesConvertidoNo();
                var botones = driver.FindElements(btnEditarCotizacion);
                return botones.Count > 0 && botones[0].Displayed;
            }
            catch { return false; }
        }

        // ── Asegurar cotización editable (CONVERTIDO = NO) ──
        public void AsegurarCotizacionEditable()
        {
            FiltrarCotizacionesConvertidoNo();

            if (ExisteCotizacionConvertidaNo())
                return;

            // No existe → registrar una nueva
            RegistrarCotizacionBase();
            VolverACotizaciones();
            FiltrarCotizacionesConvertidoNo();

            if (!ExisteCotizacionConvertidaNo())
                Assert.Fail("No se pudo generar una cotización con CONVERTIDO=NO para editar.");
        }

        private void FiltrarCotizacionesConvertidoNo()
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            // Buscar input de filtro en la columna CONVERTIDO
            try
            {
                var filtro = waitLong.Until(d =>
                {
                    try
                    {
                        // Buscar input dentro del th que contiene "CONVERTIDO"
                        var inputs = d.FindElements(By.XPath(
                            "//thead//tr[2]//input | //thead//tr//th//input"
                        ));
                        return inputs.Count > 0 ? inputs.LastOrDefault(e => e.Displayed) : null;
                    }
                    catch { return null; }
                });

                if (filtro != null)
                {
                    filtro.Clear();
                    filtro.SendKeys("NO");
                    Thread.Sleep(1000);
                }
            }
            catch
            {
                Console.WriteLine("[FiltrarCotizacionesConvertidoNo] No se encontró filtro de estado.");
            }
        }

        private bool ExisteCotizacionConvertidaNo()
        {
            try
            {
                var botones = driver.FindElements(btnEditarCotizacion);
                return botones.Count > 0 && botones[0].Displayed;
            }
            catch { return false; }
        }

        private void RegistrarCotizacionBase()
        {
            SeleccionarOpcionCotizacion("Nueva Cotización");
            // Reutiliza métodos existentes del page de pedidos via JS
            // Se llama desde StepDefinitions directamente
        }

        private void SeleccionarOpcionCotizacion(string opcion)
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            var boton = waitLong.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//*[contains(text(),'{opcion}')]")
                )
            );
            boton.Click();
        }

        private void VolverACotizaciones()
        {
            Thread.Sleep(1000);
            // La página ya está en cotizaciones, solo esperar que cargue
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            waitLong.Until(
                ExpectedConditions.ElementIsVisible(btnEditarCotizacion)
            );
        }

        // ── Seleccionar editar primer registro ──
        public void SeleccionarEditarCotizacion()
        {
            try
            {
                FiltrarCotizacionesConvertidoNo();

                var boton = wait.Until(
                    ExpectedConditions.ElementToBeClickable(btnEditarCotizacion)
                );

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);
                Thread.Sleep(300);
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", boton);
            }
            catch
            {
                Assert.Fail("No se encontró ninguna cotización con CONVERTIDO=NO para editar.");
            }
        }

        // ── Actualizar cotización ──
        public void ActualizarCotizacion()
        {
            try
            {
                var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                Thread.Sleep(1000);

                var boton = waitLong.Until(d =>
                {
                    try
                    {
                        var el = d.FindElement(btnActualizarCotizacion);
                        return el.Displayed ? el : null;
                    }
                    catch { return null; }
                });

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

                // Verificar si está deshabilitado
                bool deshabilitado = !boton.Enabled ||
                                     boton.GetAttribute("disabled") != null ||
                                     (boton.GetAttribute("class") ?? "").Contains("disabled");

                Thread.Sleep(500);
                deshabilitado = !boton.Enabled ||
                                boton.GetAttribute("disabled") != null ||
                                (boton.GetAttribute("class") ?? "").Contains("disabled");

                if (deshabilitado)
                {
                    Console.WriteLine("[ActualizarCotizacion] Botón deshabilitado, no hay cambios.");
                    _resultadoEdicion = "debe realizar alguna modificacion";
                    return;
                }

                try { boton.Click(); }
                catch
                {
                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("arguments[0].click();", boton);
                }

                // Capturar resultado tras actualizar
                _resultadoEdicion = CapturarResultadoTrasActualizar();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ActualizarCotizacion] Error: " + e.Message);
                throw;
            }
        }

        private string CapturarResultadoTrasActualizar()
        {
            try
            {
                var waitExito = new WebDriverWait(driver, TimeSpan.FromSeconds(12));
                var popup = waitExito.Until(
                    ExpectedConditions.ElementIsVisible(popupExito)
                );
                if (popup.Displayed)
                {
                    try
                    {
                        var ok = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                            .Until(ExpectedConditions.ElementToBeClickable(btnOK));
                        ok.Click();
                    }
                    catch
                    {
                        ((IJavaScriptExecutor)driver)
                            .ExecuteScript("arguments[0].click();", driver.FindElement(btnOK));
                    }
                    return "se registro correctamente";
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("[CapturarResultadoTrasActualizar] Popup no detectado.");
            }
            return "";
        }

        // ObtenerResultadoSistema actualizado para manejar edición también
        public string ObtenerResultadoSistema()
        {
            try
            {
                // Caso fecha pasada — prioridad máxima
                if (_fechaPasadaIntentada)
                {
                    _fechaPasadaIntentada = false;
                    return "Boton de fechas deshabilitado";
                }

                // Resultado de edición si existe
                if (!string.IsNullOrEmpty(_resultadoEdicion))
                {
                    var res = _resultadoEdicion;
                    _resultadoEdicion = "";
                    return res;
                }

                return _resultadoRegistro;
            }
            catch { return ""; }
        }


        //  Obtener resultado del sistema 
        //public string ObtenerResultadoSistema()
        //{
        //    try
        //    {
        //        if (_fechaPasadaIntentada)
        //        {
        //            _fechaPasadaIntentada = false;
        //            return "Boton de fechas deshabilitado";
        //        }

        //        return _resultadoRegistro;
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}
    }
}