using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SIGES3_0.Pages.Helpers;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace SIGES3_0.Pages.VentasPage
{
    public class ReportesPage
    {
        private readonly IWebDriver _driver;
        private readonly Utilities _utilities;
        private readonly WebDriverWait _wait;

        public ReportesPage(IWebDriver driver)
        {
            _driver = driver;
            _utilities = new Utilities(driver);
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        public void SeleccionarVista(string tabName)
        {
            var tabLocator = VentasLocators.Reportes.TabDinamico(tabName);
            try
            {
                _utilities.ClickButton(tabLocator);
            }
            catch (NoSuchElementException)
            {
                // Fallbacks seguros en caso de que el dinámico no lo encuentre directamente
                switch (tabName.Trim().ToLower())
                {
                    case "comprobantes": _utilities.ClickButton(VentasLocators.Reportes.TabComprobantes); break;
                    case "series":       _utilities.ClickButton(VentasLocators.Reportes.TabSeries); break;
                    case "conceptos":    _utilities.ClickButton(VentasLocators.Reportes.TabConceptos); break;
                    case "vendedor":     _utilities.ClickButton(VentasLocators.Reportes.TabVendedor); break;
                    case "grupos":       _utilities.ClickButton(VentasLocators.Reportes.TabGrupos); break;
                    case "excepciones":  _utilities.ClickButton(VentasLocators.Reportes.TabExcepciones); break;
                    default: throw new Exception($"La vista/tab '{tabName}' no existe en Reportes.");
                }
            }
            Thread.Sleep(1000);
        }

        // ── Tab: Comprobantes ────────────────────────────────────────────────────
        public void SeleccionarTipoComprobante(string tipoComprobante)
        {
            _utilities.ClickButton(VentasLocators.Reportes.TipoComprobanteDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.TipoComprobanteOption(tipoComprobante));
            Thread.Sleep(1000);
        }

        public void SeleccionarSerie(string serie)
        {
            _utilities.ClickButton(VentasLocators.Reportes.SerieDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.SerieOption(serie));
        }

        // ── Tab: Series ──────────────────────────────────────────────────────────
        // Dropdown "Comprobante y Serie" vive DENTRO de la tarjeta POR SERIE.
        // Formato: "Todos" | "XX : YYYY"  (ej: "01 : F002", "03 : B002")
        public void SeleccionarComprobanteSerie(string valor)
        {
            _utilities.ClickButton(VentasLocators.Reportes.ComprobanteSerieDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.ComprobanteSerieOpcion(valor));
        }

        // ── Tab: Conceptos ───────────────────────────────────────────────────────
        public void SeleccionarPuntoVenta(string puntoVenta)
        {
            // Si ya está como chip seleccionado, omitir (evita deseleccionar por comportamiento toggle)
            var chips = _driver.FindElements(VentasLocators.Reportes.PuntoVentaChip(puntoVenta));
            if (chips.Any(e => { try { return e.Displayed; } catch { return false; } }))
            {
                Thread.Sleep(500);
                return;
            }

            _utilities.ClickButton(VentasLocators.Reportes.PuntoVentaDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.PuntoVentaOption(puntoVenta));
            Thread.Sleep(500);
        }

        public void SeleccionarFamilia(string familia)
        {
            _utilities.ClickButton(VentasLocators.Reportes.FamiliaDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.FamiliaOption(familia));
        }

        public void SeleccionarCaracteristica(string caracteristica, string tarjeta)
        {
            _utilities.ClickButton(VentasLocators.Reportes.CaracteristicaDropdown(tarjeta));
            _utilities.ClickButton(VentasLocators.Reportes.CaracteristicaOpcion(caracteristica));
            _driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
            Thread.Sleep(500);
        }

        // ── Tab: Vendedor ────────────────────────────────────────────────────────
        public void SeleccionarVendedor(string vendedor)
        {
            var chips = _driver.FindElements(VentasLocators.Reportes.VendedorChip(vendedor));
            if (chips.Any(e => { try { return e.Displayed; } catch { return false; } }))
            { Thread.Sleep(500); return; }
            _utilities.ClickButton(VentasLocators.Reportes.VendedorDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.VendedorOption(vendedor));
            Thread.Sleep(500);
        }

        public void SeleccionarFiltroEnTarjeta(string valor, string filtro, string tarjeta)
        {
            _utilities.ClickButton(VentasLocators.Reportes.FiltroEnTarjeta(tarjeta, filtro));
            _utilities.ClickButton(VentasLocators.Reportes.FiltroOpcion(valor));
            _driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
            Thread.Sleep(500);
        }

        // ── Tab: Grupos ──────────────────────────────────────────────────────────
        public void SeleccionarEstablecimiento(string establecimiento)
        {
            var chips = _driver.FindElements(VentasLocators.Reportes.EstablecimientoChip(establecimiento));
            if (chips.Any(e => { try { return e.Displayed; } catch { return false; } }))
            { Thread.Sleep(500); return; }
            _utilities.ClickButton(VentasLocators.Reportes.EstablecimientoDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.EstablecimientoOption(establecimiento));
            Thread.Sleep(500);
        }


        public void ClickVerReporte(string tarjeta)
        {
            var locator = VentasLocators.Reportes.VerReporteEnTarjeta(tarjeta);
            IWebElement btn = null;
            try
            {
                btn = _wait.Until(d =>
                    d.FindElements(locator).FirstOrDefault(e =>
                    { try { return e.Displayed; } catch { return false; } }));
            }
            catch { }

            if (btn == null)
                throw new Exception($"No se encontró el botón VER REPORTE para la tarjeta '{tarjeta}'.");

            var handlesAntes = _driver.WindowHandles.ToList();
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btn);
            Thread.Sleep(300);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(2000);

            string handleActual = null;
            try { handleActual = _driver.CurrentWindowHandle; } catch { }
            var handlesDespues = _driver.WindowHandles.ToList();
            if (handleActual == null || !handlesDespues.Contains(handleActual))
            {
                if (handlesDespues.Any()) _driver.SwitchTo().Window(handlesDespues.Last());
            }
            else
            {
                var nuevaPestana = handlesDespues.Except(handlesAntes).FirstOrDefault();
                if (nuevaPestana != null) _driver.SwitchTo().Window(nuevaPestana);
            }
        }

        // ── Verificaciones ───────────────────────────────────────────────────────
        public bool VerificarReporteGenerado()
        {
            try
            {
                Thread.Sleep(2000);

                bool hayError = _driver.FindElements(By.XPath(
                    "//*[contains(@class,'toast-error') or contains(@class,'alert-danger')" +
                    " or (contains(@class,'swal2-popup') and .//*[contains(@class,'swal2-error-icon')])]"))
                    .Any(e => { try { return e.Displayed; } catch { return false; } });
                if (hayError) return false;

                var waitReporte = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
                var elemento = waitReporte.Until(d =>
                    d.FindElements(By.XPath(
                        "//div[contains(@class,'table-responsive')]//table" +
                        " | //table[.//tbody/tr or .//thead/tr]" +
                        " | //ngx-datatable" +
                        " | //canvas" +
                        " | //*[contains(normalize-space(),'No hay datos') or contains(normalize-space(),'Sin resultado') or contains(normalize-space(),'no se encontraron') or contains(normalize-space(),'Sin datos')]"))
                     .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } })
                );
                return elemento != null;
            }
            catch
            {
                return false;
            }
        }

        public bool VerificarBotonHabilitado(string tarjeta)
        {
            var btn = _driver.FindElements(VentasLocators.Reportes.VerReporteEnTarjeta(tarjeta))
                             .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
            if (btn == null) return false;
            return btn.Enabled && btn.GetAttribute("disabled") == null;
        }

        public void ValidarResultadoReporte(string resultadoEsperado)
        {
            switch (resultadoEsperado.Trim().ToLower())
            {
                case "no permite aplicar el filtro inhabilitado":
                    Assert.IsFalse(_driver.Url.Contains("/sales/report/view"),
                        "El sistema no debería haber generado el reporte con fechas inválidas.");
                    break;
                case "aplica el filtro correctamente":
                    Assert.IsTrue(VerificarReporteGenerado(),
                        "Se esperaba que el filtro se aplicara y el reporte se generara correctamente.");
                    break;
                default:
                    Assert.Fail($"Resultado esperado no reconocido: '{resultadoEsperado}'");
                    break;
            }
        }
        // =========================
        // REUTILIZABLE
        // =========================

        public void AccederModulo(string modulo) =>
            _utilities.ClickButton(
                By.XPath($"//span[normalize-space()='{modulo}']/ancestor::a[1]"));

        public void AccederSubmodulo(string submodulo) =>
            _utilities.ClickButton(
                By.XPath($"//span[contains(text(),'{submodulo}')]"));

        // DATE PICKER

        private static readonly By LblMes = By.XPath("//*[normalize-space()='Enero' or normalize-space()='Febrero' or normalize-space()='Marzo' or normalize-space()='Abril' or normalize-space()='Mayo' or normalize-space()='Junio' or normalize-space()='Julio' or normalize-space()='Agosto' or normalize-space()='Septiembre' or normalize-space()='Octubre' or normalize-space()='Noviembre' or normalize-space()='Diciembre']");
        private static readonly By LblAnio = By.XPath(
            "//*[string-length(normalize-space()) = 4"
            + " and string-length(translate(normalize-space(),'0123456789','')) = 0"
            + " and number(normalize-space()) >= 2020]");
        private static readonly By BtnSiguiente = By.XPath("(//i[contains(@class,'right') or contains(@class,'chevron-right') or contains(@class,'arrow-right')])[1] | (//button[contains(@aria-label,'next') or contains(@class,'next')])[1]");
        private static readonly By BtnAnterior  = By.XPath("(//i[contains(@class,'left') or contains(@class,'chevron-left') or contains(@class,'arrow-left')])[1] | (//button[contains(@aria-label,'prev') or contains(@class,'previous')])[1]");

        private static readonly By ItemsDia = By.XPath(
            "//*[self::button or self::span or self::div or self::td or self::li]"
            + "[string-length(normalize-space()) >= 1 and string-length(normalize-space()) <= 2]"
            + "[string-length(translate(normalize-space(),'0123456789','')) = 0]");

        private static readonly By ItemsTiempo = By.XPath(
            "//*[self::button or self::span or self::div or self::td or self::li]"
            + "[string-length(normalize-space()) = 2]"
            + "[string-length(translate(normalize-space(),'0123456789','')) = 0]");

        private static readonly By OpcionAm = By.XPath("//*[normalize-space()='a. m.']");
        private static readonly By OpcionPm = By.XPath("//*[normalize-space()='p. m.']");

        public void IngresarFechaHora(By inputLocator, string fechaHoraTexto)
        {
            DateTime fecha = ParseFecha(fechaHoraTexto);
            CerrarPicker();

            IWebElement input = _wait.Until(ExpectedConditions.ElementToBeClickable(inputLocator));
            ScrollPicker(input);
            ClickPicker(input);
            try { new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).Until(d => d.FindElements(LblMes).Any(EsVisible)); }
            catch { Thread.Sleep(400); }

            try { NavegaMes(fecha); }
            catch (Exception ex) { throw new Exception("Falló seleccionando mes/año: " + ex.Message); }

            try { ClickDia(fecha, inputLocator); }
            catch (Exception ex) { throw new Exception("Falló seleccionando día: " + ex.Message); }

            try { ClickHora(fecha, inputLocator); }
            catch (Exception ex) { throw new Exception("Falló seleccionando hora: " + ex.Message); }

            try { ClickMinuto(fecha, inputLocator); }
            catch (Exception ex) { throw new Exception("Falló seleccionando minuto: " + ex.Message); }

            try { ClickAmPm(fecha, inputLocator); }
            catch (Exception ex) { throw new Exception("Falló seleccionando AM/PM: " + ex.Message); }

            string esperado = FormatearFecha(fecha);
            try { _wait.Until(d => ValorCampo(inputLocator) == esperado); }
            catch { }

            string actual = ValorCampo(inputLocator);
            if (actual != esperado)
                Assert.Fail($"La fecha no se asignó correctamente. Esperado: {esperado} / Actual: {actual}");

            CerrarPicker();
        }

        private static DateTime ParseFecha(string fechaHora)
        {
            if (fechaHora.Trim().Equals("hoy", StringComparison.OrdinalIgnoreCase))
                return DateTime.Now;

            string[] formatos =
            {
                "dd/MM/yyyy hh:mm tt", "d/MM/yyyy hh:mm tt",
                "dd/M/yyyy hh:mm tt",  "d/M/yyyy hh:mm tt",
                "dd/MM/yyyy h:mm tt",  "d/MM/yyyy h:mm tt",
                "dd/M/yyyy h:mm tt",   "d/M/yyyy h:mm tt"
            };

            if (DateTime.TryParseExact(fechaHora.Trim(), formatos,
                new CultureInfo("en-US"), DateTimeStyles.None, out DateTime fecha))
            {
                return fecha;
            }

            throw new Exception("No se pudo interpretar la fecha: " + fechaHora);
        }

        private void NavegaMes(DateTime objetivo)
        {
            string[] meses =
            {
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
            };

            string mesObjetivo = meses[objetivo.Month - 1];
            int anioObjetivo   = objetivo.Year;

            for (int i = 0; i < 24; i++)
            {
                IWebElement mesElem = _wait.Until(d =>
                    d.FindElements(LblMes)
                     .Where(e => EsVisible(e))
                     .OrderByDescending(e => e.Location.Y)
                     .FirstOrDefault());

                if (mesElem == null)
                    throw new Exception("No se encontró el mes visible del calendario.");

                string mesActual  = mesElem.Text.Trim();
                int    anioActual = anioObjetivo;

                IWebElement anioElem = _driver.FindElements(LblAnio)
                    .Where(e => EsVisible(e))
                    .OrderByDescending(e => e.Location.Y)
                    .FirstOrDefault();

                if (anioElem != null && int.TryParse(anioElem.Text.Trim(), out int anioDetectado))
                    anioActual = anioDetectado;

                if (mesActual.Equals(mesObjetivo, StringComparison.OrdinalIgnoreCase) && anioActual == anioObjetivo)
                    return;

                DateTime act  = new DateTime(anioActual, NumeroMes(mesActual), 1);
                DateTime meta = new DateTime(anioObjetivo, objetivo.Month, 1);

                if (act < meta)
                {
                    IWebElement next = _wait.Until(d => d.FindElements(BtnSiguiente).FirstOrDefault(EsVisible));
                    if (next == null) throw new Exception("No se encontró el botón para avanzar mes.");
                    ClickPicker(next);
                }
                else
                {
                    IWebElement prev = _wait.Until(d => d.FindElements(BtnAnterior).FirstOrDefault(EsVisible));
                    if (prev == null) throw new Exception("No se encontró el botón para retroceder mes.");
                    ClickPicker(prev);
                }

                try { _wait.Until(ExpectedConditions.StalenessOf(mesElem)); }
                catch { Thread.Sleep(300); }
            }

            throw new Exception($"No se pudo navegar al mes/año esperado: {mesObjetivo} {anioObjetivo}");
        }

        private void ClickDia(DateTime fecha, By inputLocator)
        {
            string textoDia   = fecha.Day.ToString();
            string fechaTexto = fecha.ToString("dd/MM/yyyy");

            IWebElement mesVisible = _driver.FindElements(LblMes)
                .Where(e => EsVisible(e))
                .OrderByDescending(e => e.Location.Y)
                .FirstOrDefault();

            if (mesVisible == null)
                throw new Exception("No se encontró el encabezado del calendario.");

            int yMin = mesVisible.Location.Y + mesVisible.Size.Height;

            var candidatos = _driver.FindElements(ItemsDia)
                .Where(e =>
                {
                    if (!EsVisible(e)) return false;
                    if ((e.Text?.Trim() ?? "") != textoDia) return false;
                    if (e.Location.Y <= yMin) return false;
                    return true;
                })
                .OrderBy(e => e.Location.X)
                .ThenBy(e => e.Location.Y)
                .ToList();

            if (!candidatos.Any())
                throw new Exception("No se encontró ningún candidato para el día: " + textoDia);

            foreach (var candidato in candidatos)
            {
                try
                {
                    ClickPicker(candidato);
                    Thread.Sleep(300);
                    string v = ValorCampo(inputLocator);
                    if (!string.IsNullOrWhiteSpace(v) && v.StartsWith(fechaTexto))
                        return;
                }
                catch { }
            }

            throw new Exception($"No se logró seleccionar el día correcto. Esperado: {fechaTexto} / Actual: {ValorCampo(inputLocator)}");
        }

        private void ClickHora(DateTime fecha, By inputLocator)
        {
            string hora       = fecha.ToString("hh");
            string fechaTexto = fecha.ToString("dd/MM/yyyy");

            var columnas = GetColumns();
            if (columnas.Count < 2)
                throw new Exception("No se pudieron identificar las columnas de hora y minuto del picker.");

            IWebElement elem = columnas[0].FirstOrDefault(e => (e.Text ?? "").Trim() == hora);

            if (elem == null)
            {
                int topVal    = int.Parse((columnas[0][0].Text ?? "01").Trim());
                int targetVal = int.Parse(hora);
                DesplazarColumna(columnas[0][columnas[0].Count / 2], targetVal - topVal);

                columnas = GetColumns();
                elem = columnas[0].FirstOrDefault(e => (e.Text ?? "").Trim() == hora);
            }

            if (elem == null)
                throw new Exception("No se encontró la hora en la columna de horas: " + hora);

            ClickPicker(elem);
            try { _wait.Until(d => ValorCampo(inputLocator).Contains($" {hora}:")); } catch { }

            string actual = ValorCampo(inputLocator);
            if (!actual.StartsWith(fechaTexto) || !actual.Contains($" {hora}:"))
                throw new Exception($"No se logró seleccionar la hora correcta. Esperado: {fechaTexto} {hora}:xx / Actual: {actual}");
        }

        private void ClickMinuto(DateTime fecha, By inputLocator)
        {
            string minuto     = fecha.ToString("mm");
            string fechaTexto = fecha.ToString("dd/MM/yyyy");

            var columnas = GetColumns();
            if (columnas.Count < 2)
                throw new Exception("No se pudieron identificar las columnas de hora y minuto del picker.");

            IWebElement elem = columnas[1].FirstOrDefault(e => (e.Text ?? "").Trim() == minuto);

            if (elem == null)
            {
                int topVal    = int.Parse((columnas[1][0].Text ?? "00").Trim());
                int targetVal = int.Parse(minuto);
                DesplazarColumna(columnas[1][columnas[1].Count / 2], targetVal - topVal);

                columnas = GetColumns();
                elem = columnas[1].FirstOrDefault(e => (e.Text ?? "").Trim() == minuto);
            }

            if (elem == null)
                throw new Exception("No se encontró el minuto en la columna de minutos: " + minuto);

            ClickPicker(elem);
            try { _wait.Until(d => ValorCampo(inputLocator).Contains($":{minuto}")); } catch { }

            string actual = ValorCampo(inputLocator);
            if (!actual.StartsWith(fechaTexto) || !actual.Contains($":{minuto}"))
                throw new Exception($"No se logró seleccionar el minuto correcto. Esperado: {fechaTexto} xx:{minuto} / Actual: {actual}");
        }

        private void ClickAmPm(DateTime fecha, By inputLocator)
        {
            bool esAm = fecha.ToString("tt", new CultureInfo("en-US"))
                .Equals("AM", StringComparison.OrdinalIgnoreCase);

            By target = esAm ? OpcionAm : OpcionPm;

            IWebElement elem = _wait.Until(d => d.FindElements(target).FirstOrDefault(e => EsVisible(e)));
            if (elem == null)
                throw new Exception("No se encontró el selector " + (esAm ? "a. m." : "p. m."));

            string esperadoTexto = esAm ? "a. m." : "p. m.";
            ClickPicker(elem);
            try { _wait.Until(d => ValorCampo(inputLocator).Contains(esperadoTexto)); }
            catch
            {
                ClickPicker(elem);
                try { _wait.Until(d => ValorCampo(inputLocator).Contains(esperadoTexto)); }
                catch { throw new Exception($"No se logró seleccionar {esperadoTexto}. Valor actual: {ValorCampo(inputLocator)}"); }
            }
        }

        private static string FormatearFecha(DateTime fecha)
        {
            string ampm = fecha.ToString("tt", new CultureInfo("en-US")).ToLower() == "am"
                ? "a. m."
                : "p. m.";
            return $"{fecha:dd/MM/yyyy hh:mm} {ampm}";
        }

        private List<List<IWebElement>> GetColumns()
        {
            var numericos = _driver.FindElements(ItemsTiempo)
                .Where(e => EsVisible(e))
                .OrderBy(e => e.Location.X)
                .ThenBy(e => e.Location.Y)
                .ToList();

            if (!numericos.Any())
                throw new Exception("No se encontraron elementos numéricos visibles en el picker.");

            var columnas   = new List<List<IWebElement>>();
            const int tolX = 20;

            foreach (var elem in numericos)
            {
                bool agregado = false;
                foreach (var col in columnas)
                {
                    if (Math.Abs(elem.Location.X - col[0].Location.X) <= tolX)
                    {
                        col.Add(elem);
                        agregado = true;
                        break;
                    }
                }
                if (!agregado)
                    columnas.Add(new List<IWebElement> { elem });
            }

            columnas = columnas
                .Where(c => c.Count >= 3)
                .OrderBy(c => c.Average(e => e.Location.X))
                .ToList();

            if (columnas.Count < 2)
                throw new Exception("No se pudieron agrupar correctamente las columnas numéricas del picker.");

            // El picker siempre muestra: [Calendario izq. | Horas | Minutos der.]
            // Los días del calendario (10-31) también tienen 2 dígitos y forman columnas con >= 3 items.
            // Las columnas de Horas y Minutos son siempre las 2 más a la derecha.
            if (columnas.Count > 2)
                columnas = columnas.Skip(columnas.Count - 2).ToList();

            return columnas;
        }

        private void CerrarPicker()
        {
            try
            {
                _driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
                Thread.Sleep(400);
            }
            catch { }
        }

        private void DesplazarColumna(IWebElement refItem, int pasos)
        {
            int deltaY = pasos > 0 ? 100 : -100;
            for (int k = 0; k < Math.Abs(pasos); k++)
                ((IJavaScriptExecutor)_driver).ExecuteScript(
                    "arguments[0].dispatchEvent(new WheelEvent('wheel',{deltaY:arguments[1],bubbles:true}));",
                    refItem, deltaY);
            if (pasos != 0) Thread.Sleep(400);
        }

        private static int NumeroMes(string nombreMes)
        {
            string[] meses =
            {
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
            };
            int idx = Array.FindIndex(meses, m => m.Equals(nombreMes.Trim(), StringComparison.OrdinalIgnoreCase));
            if (idx == -1)
                throw new Exception("No se reconoció el mes del calendario: " + nombreMes);
            return idx + 1;
        }

        private void ScrollPicker(IWebElement element) =>
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);

        private void ClickPicker(IWebElement element)
        {
            try { element.Click(); }
            catch { ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element); }
        }

        private string ValorCampo(By locator)
        {
            try { return _wait.Until(d => d.FindElement(locator)).GetAttribute("value")?.Trim() ?? ""; }
            catch { return ""; }
        }

        private static bool EsVisible(IWebElement element)
        {
            try { return element != null && element.Displayed; }
            catch { return false; }
        }
    }
}
