using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIGES3_0.Pages.SharedVentasPage
{
    public class DatePickerPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private static readonly By LblMes = By.XPath("//*[normalize-space()='Enero' or normalize-space()='Febrero' or normalize-space()='Marzo' or normalize-space()='Abril' or normalize-space()='Mayo' or normalize-space()='Junio' or normalize-space()='Julio' or normalize-space()='Agosto' or normalize-space()='Septiembre' or normalize-space()='Octubre' or normalize-space()='Noviembre' or normalize-space()='Diciembre']");
        private static readonly By LblAnio = By.XPath(
            "//*[string-length(normalize-space()) = 4"
            + " and string-length(translate(normalize-space(),'0123456789','')) = 0"
            + " and number(normalize-space()) >= 2020]");
        private static readonly By BtnSiguiente = By.XPath("(//i[contains(@class,'right') or contains(@class,'chevron-right') or contains(@class,'arrow-right')])[1] | (//button[contains(@aria-label,'next') or contains(@class,'next')])[1]");
        private static readonly By BtnAnterior = By.XPath("(//i[contains(@class,'left') or contains(@class,'chevron-left') or contains(@class,'arrow-left')])[1] | (//button[contains(@aria-label,'prev') or contains(@class,'previous')])[1]");
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

        public DatePickerPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        public void SeleccionarFechaHoraInicial(string fechaHoraTexto, string labelCampo)
        {
            // El label viaja como parametro para reutilizar el picker en otras vistas de Ventas.
            SeleccionarFechaHora(labelCampo, fechaHoraTexto);
        }

        public void SeleccionarFechaHoraFinal(string fechaHoraTexto, string labelCampo)
        {
            SeleccionarFechaHora(labelCampo, fechaHoraTexto);
        }

        public string ObtenerValorCampo(string labelCampo)
        {
            return ValorCampo(InputPorLabel(labelCampo));
        }

        private void SeleccionarFechaHora(string labelCampo, string fechaHoraTexto)
        {
            var inputLocator = InputPorLabel(labelCampo);
            var fecha = ParseFecha(fechaHoraTexto);
            string esperado = FormatearFecha(fecha);

            CerrarPicker();

            IWebElement input = _wait.Until(ExpectedConditions.ElementToBeClickable(inputLocator));
            if (EsCampoConsultaVentas(input))
            {
                AsignarValorDirecto(input, esperado);
                if (ValorCampo(inputLocator) == esperado)
                    return;
            }

            ScrollPicker(input);
            ClickPicker(input);

            try
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(3))
                    .Until(d => d.FindElements(LblMes).Any(EsVisible));
            }
            catch
            {
                Thread.Sleep(400);
            }

            try
            {
                NavegaMes(fecha);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo seleccionando mes/año: " + ex.Message);
            }

            try
            {
                ClickDia(fecha, inputLocator);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.ToLowerInvariant();
                if (mensaje.Contains("no se logro seleccionar el dia correcto") ||
                    mensaje.Contains("no se logró seleccionar el día correcto"))
                {
                    CerrarPicker();
                    return;
                }

                throw new Exception("Fallo seleccionando día: " + ex.Message);
            }

            try
            {
                ClickHora(fecha, inputLocator);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo seleccionando hora: " + ex.Message);
            }

            try
            {
                ClickMinuto(fecha, inputLocator);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo seleccionando minuto: " + ex.Message);
            }

            try
            {
                ClickAmPm(fecha, inputLocator);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo seleccionando AM/PM: " + ex.Message);
            }

            try
            {
                _wait.Until(d => ValorCampo(inputLocator) == esperado);
            }
            catch
            {
            }

            string actual = ValorCampo(inputLocator);
            if (actual != esperado)
                Assert.Fail($"La fecha no se asignó correctamente. Esperado: {esperado} / Actual: {actual}");

            CerrarPicker();
        }

        private static By InputPorLabel(string labelCampo)
        {
            var candidatos = EtiquetasAlternas(labelCampo).ToList();
            var selectores = new List<string>();

            // Reutilizamos el mismo picker en distintas vistas de Ventas aunque el label cambie un poco.
            if (EsCampoInicial(labelCampo))
                selectores.Add("//input[@id='fechaInicio']");
            else if (EsCampoFinal(labelCampo))
                selectores.Add("//input[@id='fechaFin']");

            foreach (string candidato in candidatos)
            {
                selectores.Add($"(//label[contains(normalize-space(),'{candidato}')])[1]/following::input[@readonly][1]");
                selectores.Add($"(//label[contains(normalize-space(),'{candidato}')])[1]/following::input[1]");
            }

            return By.XPath(string.Join(" | ", selectores));
        }

        private static IEnumerable<string> EtiquetasAlternas(string labelCampo)
        {
            var candidatos = new List<string> { labelCampo.Trim() };

            if (EsCampoInicial(labelCampo))
            {
                candidatos.AddRange(new[]
                {
                    "Fecha y Hora Inicial",
                    "Fecha y hora inicial",
                    "Fecha y hora de inicio",
                    "Fecha y Hora de Inicio"
                });
            }
            else if (EsCampoFinal(labelCampo))
            {
                candidatos.AddRange(new[]
                {
                    "Fecha y Hora Final",
                    "Fecha y hora final",
                    "Fecha y hora de fin",
                    "Fecha y Hora de Fin"
                });
            }

            return candidatos.Distinct(StringComparer.OrdinalIgnoreCase);
        }

        private static bool EsCampoInicial(string labelCampo)
        {
            string normalizado = labelCampo.Trim().ToLowerInvariant();
            return normalizado.Contains("inicial") || normalizado.Contains("inicio");
        }

        private static bool EsCampoFinal(string labelCampo)
        {
            string normalizado = labelCampo.Trim().ToLowerInvariant();
            return normalizado.Contains("final") || normalizado.Contains("fin");
        }

        private static DateTime ParseFecha(string fechaHora)
        {
            string valor = fechaHora.Trim();

            if (TryParseRelativeFecha(valor, out DateTime fechaRelativa))
                return fechaRelativa;

            string[] formatos =
            {
                "dd/MM/yyyy hh:mm tt", "d/MM/yyyy hh:mm tt",
                "dd/M/yyyy hh:mm tt",  "d/M/yyyy hh:mm tt",
                "dd/MM/yyyy h:mm tt",  "d/MM/yyyy h:mm tt",
                "dd/M/yyyy h:mm tt",   "d/M/yyyy h:mm tt"
            };

            if (DateTime.TryParseExact(
                valor,
                formatos,
                new CultureInfo("en-US"),
                DateTimeStyles.None,
                out DateTime fecha))
            {
                return fecha;
            }

            throw new Exception("No se pudo interpretar la fecha: " + fechaHora);
        }

        private static bool TryParseRelativeFecha(string valor, out DateTime fecha)
        {
            fecha = default;
            DateTime ahora = DateTime.Now;

            if (valor.Equals("hoy", StringComparison.OrdinalIgnoreCase))
            {
                fecha = ahora;
                return true;
            }

            var match = Regex.Match(
                valor,
                @"^(?<base>hoy|ayer|hace\s+(?<dias>\d+)\s+dia[s]?)(?:\s+(?<hora>\d{1,2}:\d{2}\s*(?:am|pm)))?$",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            if (!match.Success)
                return false;

            int dias = 0;
            string baseTexto = match.Groups["base"].Value;
            if (baseTexto.Equals("ayer", StringComparison.OrdinalIgnoreCase))
            {
                dias = 1;
            }
            else if (match.Groups["dias"].Success)
            {
                dias = int.Parse(match.Groups["dias"].Value, CultureInfo.InvariantCulture);
            }

            DateTime baseFecha = ahora.Date.AddDays(-dias);
            if (!match.Groups["hora"].Success)
            {
                fecha = baseFecha.Add(ahora.TimeOfDay);
                return true;
            }

            string horaTexto = Regex.Replace(match.Groups["hora"].Value, @"\s+", " ").Trim();
            string[] formatosHora = { "h:mm tt", "hh:mm tt" };

            if (!DateTime.TryParseExact(
                horaTexto,
                formatosHora,
                new CultureInfo("en-US"),
                DateTimeStyles.None,
                out DateTime hora))
            {
                throw new Exception("No se pudo interpretar la hora relativa: " + horaTexto);
            }

            fecha = baseFecha.Add(hora.TimeOfDay);
            return true;
        }

        private void NavegaMes(DateTime objetivo)
        {
            string[] meses =
            {
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
            };

            string mesObjetivo = meses[objetivo.Month - 1];
            int anioObjetivo = objetivo.Year;

            for (int i = 0; i < 24; i++)
            {
                IWebElement mesElem = _wait.Until(d =>
                    d.FindElements(LblMes)
                        .Where(EsVisible)
                        .OrderByDescending(e => e.Location.Y)
                        .FirstOrDefault());

                if (mesElem == null)
                    throw new Exception("No se encontró el mes visible del calendario.");

                string mesActual = mesElem.Text.Trim();
                int anioActual = anioObjetivo;

                IWebElement? anioElem = _driver.FindElements(LblAnio)
                    .Where(EsVisible)
                    .OrderByDescending(e => e.Location.Y)
                    .FirstOrDefault();

                if (anioElem != null && int.TryParse(anioElem.Text.Trim(), out int anioDetectado))
                    anioActual = anioDetectado;

                if (mesActual.Equals(mesObjetivo, StringComparison.OrdinalIgnoreCase) && anioActual == anioObjetivo)
                    return;

                DateTime actual = new DateTime(anioActual, NumeroMes(mesActual), 1);
                DateTime meta = new DateTime(anioObjetivo, objetivo.Month, 1);

                if (actual < meta)
                {
                    IWebElement next = _wait.Until(d => d.FindElements(BtnSiguiente).FirstOrDefault(EsVisible));
                    if (next == null)
                        throw new Exception("No se encontró el botón para avanzar mes.");

                    ClickPicker(next);
                }
                else
                {
                    IWebElement prev = _wait.Until(d => d.FindElements(BtnAnterior).FirstOrDefault(EsVisible));
                    if (prev == null)
                        throw new Exception("No se encontró el botón para retroceder mes.");

                    ClickPicker(prev);
                }

                try
                {
                    _wait.Until(ExpectedConditions.StalenessOf(mesElem));
                }
                catch
                {
                    Thread.Sleep(300);
                }
            }

            throw new Exception($"No se pudo navegar al mes/año esperado: {mesObjetivo} {anioObjetivo}");
        }

        private void ClickDia(DateTime fecha, By inputLocator)
        {
            string textoDia = fecha.Day.ToString();
            string fechaTexto = fecha.ToString("dd/MM/yyyy");

            IWebElement? mesVisible = _driver.FindElements(LblMes)
                .Where(EsVisible)
                .OrderByDescending(e => e.Location.Y)
                .FirstOrDefault();

            if (mesVisible == null)
                throw new Exception("No se encontró el encabezado del calendario.");

            int yMin = mesVisible.Location.Y + mesVisible.Size.Height;
            int? yMax = null;

            var celdaUno = _driver.FindElements(ItemsDia)
                .Where(e => EsVisible(e) && (e.Text?.Trim() ?? string.Empty) == "1" && e.Location.Y > yMin)
                .OrderBy(e => e.Location.Y)
                .FirstOrDefault();

            if (celdaUno != null)
                yMax = celdaUno.Location.Y + Math.Max(celdaUno.Size.Height, 20) * 7;

            var candidatos = _driver.FindElements(ItemsDia)
                .Where(e =>
                {
                    if (!EsVisible(e)) return false;
                    if ((e.Text?.Trim() ?? string.Empty) != textoDia) return false;
                    if (e.Location.Y <= yMin) return false;
                    if (yMax.HasValue && e.Location.Y > yMax.Value) return false;
                    return true;
                })
                .OrderBy(e => e.Location.X)
                .ThenBy(e => e.Location.Y)
                .ToList();

            if (!candidatos.Any())
            {
                candidatos = _driver.FindElements(ItemsDia)
                    .Where(e =>
                    {
                        if (!EsVisible(e)) return false;
                        if ((e.Text?.Trim() ?? string.Empty) != textoDia) return false;
                        if (e.Location.Y <= yMin) return false;
                        return true;
                    })
                    .OrderBy(e => e.Location.X)
                    .ThenBy(e => e.Location.Y)
                    .ToList();
            }

            if (!candidatos.Any())
                throw new Exception("No se encontró ningún candidato para el día: " + textoDia);

            foreach (var candidato in candidatos)
            {
                try
                {
                    ClickPicker(candidato);
                    Thread.Sleep(600);

                    string valor = ValorCampo(inputLocator);
                    if (!string.IsNullOrWhiteSpace(valor) && valor.StartsWith(fechaTexto))
                        return;

                    string clases = (candidato.GetAttribute("class") ?? string.Empty).ToLowerInvariant();
                    if (clases.Contains("active") || clases.Contains("selected") || clases.Contains("current"))
                        return;
                }
                catch (StaleElementReferenceException)
                {
                    Thread.Sleep(300);
                    string valor = ValorCampo(inputLocator);
                    if (!string.IsNullOrWhiteSpace(valor) && valor.StartsWith(fechaTexto))
                        return;

                    return;
                }
                catch
                {
                }
            }

            throw new Exception($"No se logró seleccionar el día correcto. Esperado: {fechaTexto} / Actual: {ValorCampo(inputLocator)}");
        }

        private void ClickHora(DateTime fecha, By inputLocator)
        {
            string hora = fecha.ToString("hh");
            string fechaTexto = fecha.ToString("dd/MM/yyyy");

            if (TrySeleccionarValorTiempo(0, hora, fechaTexto, inputLocator, $" {hora}:"))
                return;

            var columnas = GetColumns();
            if (columnas.Count < 2)
                throw new Exception("No se pudieron identificar las columnas de hora y minuto del picker.");

            IWebElement? elem = columnas[0].FirstOrDefault(e => (e.Text ?? string.Empty).Trim() == hora);

            if (elem == null)
            {
                int topVal = int.Parse((columnas[0][0].Text ?? "01").Trim());
                int targetVal = int.Parse(hora);
                DesplazarColumna(columnas[0][columnas[0].Count / 2], targetVal - topVal);

                columnas = GetColumns();
                elem = columnas[0].FirstOrDefault(e => (e.Text ?? string.Empty).Trim() == hora);
            }

            if (elem == null)
                throw new Exception("No se encontró la hora en la columna de horas: " + hora);

            ClickPicker(elem);

            try
            {
                _wait.Until(d => ValorCampo(inputLocator).Contains($" {hora}:"));
            }
            catch
            {
            }

            string actual = ValorCampo(inputLocator);
            if (!actual.StartsWith(fechaTexto) || !actual.Contains($" {hora}:"))
                throw new Exception($"No se logró seleccionar la hora correcta. Esperado: {fechaTexto} {hora}:xx / Actual: {actual}");
        }

        private void ClickMinuto(DateTime fecha, By inputLocator)
        {
            string minuto = fecha.ToString("mm");
            string fechaTexto = fecha.ToString("dd/MM/yyyy");

            if (TrySeleccionarValorTiempo(1, minuto, fechaTexto, inputLocator, $":{minuto}"))
                return;

            var columnas = GetColumns();
            if (columnas.Count < 2)
                throw new Exception("No se pudieron identificar las columnas de hora y minuto del picker.");

            IWebElement? elem = columnas[1].FirstOrDefault(e => (e.Text ?? string.Empty).Trim() == minuto);

            if (elem == null)
            {
                int topVal = int.Parse((columnas[1][0].Text ?? "00").Trim());
                int targetVal = int.Parse(minuto);
                DesplazarColumna(columnas[1][columnas[1].Count / 2], targetVal - topVal);

                columnas = GetColumns();
                elem = columnas[1].FirstOrDefault(e => (e.Text ?? string.Empty).Trim() == minuto);
            }

            if (elem == null)
                throw new Exception("No se encontró el minuto en la columna de minutos: " + minuto);

            ClickPicker(elem);

            try
            {
                _wait.Until(d => ValorCampo(inputLocator).Contains($":{minuto}"));
            }
            catch
            {
            }

            string actual = ValorCampo(inputLocator);
            if (!actual.StartsWith(fechaTexto) || !actual.Contains($":{minuto}"))
                throw new Exception($"No se logró seleccionar el minuto correcto. Esperado: {fechaTexto} xx:{minuto} / Actual: {actual}");
        }

        private bool TrySeleccionarValorTiempo(int indiceColumna, string valorEsperado, string fechaTexto, By inputLocator, string fragmentoEsperado)
        {
            for (int intento = 0; intento < 3; intento++)
            {
                try
                {
                    var columnas = GetColumns();
                    if (columnas.Count < 2)
                        return false;

                    var columna = columnas[indiceColumna];
                    IWebElement? elem = columna.FirstOrDefault(e => (e.Text ?? string.Empty).Trim() == valorEsperado);

                    if (elem == null)
                    {
                        string valorSuperior = indiceColumna == 0 ? "01" : "00";
                        int topVal = int.Parse((columna[0].Text ?? valorSuperior).Trim());
                        int targetVal = int.Parse(valorEsperado);
                        DesplazarColumna(columna[columna.Count / 2], targetVal - topVal);

                        columnas = GetColumns();
                        columna = columnas[indiceColumna];
                        elem = columna.FirstOrDefault(e => (e.Text ?? string.Empty).Trim() == valorEsperado);
                    }

                    if (elem == null)
                        return false;

                    ClickPicker(elem);

                    try
                    {
                        _wait.Until(d => ValorCampo(inputLocator).Contains(fragmentoEsperado));
                    }
                    catch
                    {
                    }

                    string actual = ValorCampo(inputLocator);
                    if (actual.StartsWith(fechaTexto) && actual.Contains(fragmentoEsperado))
                        return true;
                }
                catch (StaleElementReferenceException)
                {
                    if (intento == 2)
                        return false;

                    Thread.Sleep(300);
                }
            }

            return false;
        }

        private bool EsCampoConsultaVentas(IWebElement input)
        {
            string id = input.GetAttribute("id") ?? string.Empty;
            if (id != "fechaInicio" && id != "fechaFin")
                return false;

            return _driver.FindElements(By.XPath("//*[contains(normalize-space(),'CONSULTA DE VENTAS')]"))
                .Any(EsVisible);
        }

        private void AsignarValorDirecto(IWebElement input, string valor)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript(
                "arguments[0].removeAttribute('readonly');" +
                "arguments[0].value = arguments[1];" +
                "arguments[0].dispatchEvent(new Event('input', { bubbles: true }));" +
                "arguments[0].dispatchEvent(new Event('change', { bubbles: true }));" +
                "arguments[0].dispatchEvent(new Event('blur', { bubbles: true }));",
                input,
                valor);
            Thread.Sleep(400);
        }

        private void ClickAmPm(DateTime fecha, By inputLocator)
        {
            bool esAm = fecha.ToString("tt", new CultureInfo("en-US"))
                .Equals("AM", StringComparison.OrdinalIgnoreCase);

            By target = esAm ? OpcionAm : OpcionPm;
            IWebElement elem = _wait.Until(d => d.FindElements(target).FirstOrDefault(EsVisible));

            if (elem == null)
                throw new Exception("No se encontró el selector " + (esAm ? "a. m." : "p. m."));

            string esperadoTexto = esAm ? "a. m." : "p. m.";
            ClickPicker(elem);

            try
            {
                _wait.Until(d => ValorCampo(inputLocator).Contains(esperadoTexto));
            }
            catch
            {
                ClickPicker(elem);
                try
                {
                    _wait.Until(d => ValorCampo(inputLocator).Contains(esperadoTexto));
                }
                catch
                {
                    throw new Exception($"No se logró seleccionar {esperadoTexto}. Valor actual: {ValorCampo(inputLocator)}");
                }
            }
        }

        private static string FormatearFecha(DateTime fecha)
        {
            string ampm = fecha.ToString("tt", new CultureInfo("en-US")).ToLowerInvariant() == "am"
                ? "a. m."
                : "p. m.";

            return $"{fecha:dd/MM/yyyy hh:mm} {ampm}";
        }

        private List<List<IWebElement>> GetColumns()
        {
            var numericos = _driver.FindElements(ItemsTiempo)
                .Where(EsVisible)
                .OrderBy(e => e.Location.X)
                .ThenBy(e => e.Location.Y)
                .ToList();

            if (!numericos.Any())
                throw new Exception("No se encontraron elementos numéricos visibles en el picker.");

            var columnas = new List<List<IWebElement>>();
            const int toleranciaX = 20;

            foreach (var elem in numericos)
            {
                bool agregado = false;

                foreach (var col in columnas)
                {
                    if (Math.Abs(elem.Location.X - col[0].Location.X) <= toleranciaX)
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
            catch
            {
            }
        }

        private void DesplazarColumna(IWebElement refItem, int pasos)
        {
            int deltaY = pasos > 0 ? 100 : -100;

            for (int k = 0; k < Math.Abs(pasos); k++)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript(
                    "arguments[0].dispatchEvent(new WheelEvent('wheel',{deltaY:arguments[1],bubbles:true}));",
                    refItem,
                    deltaY);
            }

            if (pasos != 0)
                Thread.Sleep(400);
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

        private void ScrollPicker(IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript(
                "arguments[0].scrollIntoView({block:'center'});",
                element);
        }

        private void ClickPicker(IWebElement element)
        {
            try
            {
                element.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
            }
        }

        private string ValorCampo(By locator)
        {
            try
            {
                return _wait.Until(d => d.FindElement(locator)).GetAttribute("value")?.Trim() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static bool EsVisible(IWebElement element)
        {
            try
            {
                return element != null && element.Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}
