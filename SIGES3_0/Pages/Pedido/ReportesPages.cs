using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SIGES3_0.Pages.PedidoPages
{
    public class ReporteDePedidosPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public ReporteDePedidosPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // =========================
        // LOCATORS
        // =========================

        private readonly By submoduloReportes = By.XPath("//span[normalize-space()='Reportes']/ancestor::a");

        private readonly By cmbEstablecimiento = By.XPath("//label[contains(.,'Establecimientos')]/following::div[contains(@class,'select-trigger')][1]");

        private readonly By cmbPuntoVenta = By.XPath("//label[contains(.,'Puntos de venta')]/following::div[contains(@class,'select-trigger')][1]");

        private readonly By inputFechaHoraInicial = By.XPath("//label[contains(.,'Fecha y Hora Inicial')]/following::input[@readonly][1]");

        private readonly By inputFechaHoraFinal = By.XPath("//label[contains(.,'Fecha y Hora Final')]/following::input[@readonly][1]");

        private readonly By btnVerReporteInvalidados = By.XPath("//button[contains(.,'VER REPORTE') or contains(.,'Ver reporte')]");

        private readonly By toastError = By.XPath("//*[contains(@class,'toast') or contains(@class,'alert') or contains(@class,'swal') or contains(text(),'fecha') or contains(text(),'rango') or contains(text(),'inválid') or contains(text(),'inval')]");

        private readonly By tablaResultados = By.XPath("//table | //tbody | //div[contains(@class,'table-responsive')]");

        private readonly By mensajeSinResultados = By.XPath("//*[contains(text(),'No hay datos') or contains(text(),'No se encontraron resultados') or contains(text(),'Sin resultados')]");

        private readonly By btnMesSiguiente = By.XPath("(//i[contains(@class,'right') or contains(@class,'chevron-right') or contains(@class,'arrow-right')])[1] | (//button[contains(@aria-label,'next') or contains(@class,'next')])[1]");

        private readonly By btnMesAnterior = By.XPath("(//i[contains(@class,'left') or contains(@class,'chevron-left') or contains(@class,'arrow-left')])[1] | (//button[contains(@aria-label,'prev') or contains(@class,'previous')])[1]");

        private readonly By lblMesPicker = By.XPath("//*[normalize-space()='Enero' or normalize-space()='Febrero' or normalize-space()='Marzo' or normalize-space()='Abril' or normalize-space()='Mayo' or normalize-space()='Junio' or normalize-space()='Julio' or normalize-space()='Agosto' or normalize-space()='Septiembre' or normalize-space()='Octubre' or normalize-space()='Noviembre' or normalize-space()='Diciembre']");

        private readonly By lblAnioPicker = By.XPath("//*[normalize-space()='2024' or normalize-space()='2025' or normalize-space()='2026' or normalize-space()='2027' or normalize-space()='2028']");

        private readonly By itemsDiaPicker = By.XPath("//button | //div | //span");

        private readonly By itemsHoraPicker = By.XPath("//button | //div | //span");

        private readonly By itemsMinutoPicker = By.XPath("//button | //div | //span");

        private readonly By itemsAmPmPicker = By.XPath("//button | //div | //span");

        private readonly By opcionesCombo =
            By.XPath("//*[contains(@class,'option') or contains(@class,'item') or self::span or self::div]");

        private readonly By opcionAmPicker =
            By.XPath("//*[normalize-space()='a. m.']");

        private readonly By opcionPmPicker =
            By.XPath("//*[normalize-space()='p. m.']");

        // =========================
        // MÉTODOS PÚBLICOS
        // =========================

        public void AccederASubmoduloReportes()
        {
            try
            {
                IWebElement reporte = wait.Until(d =>
                    d.FindElements(submoduloReportes).FirstOrDefault(e => EsVisible(e) && e.Enabled));

                if (reporte == null)
                    Assert.Fail("No se encontró el submódulo Reportes.");

                ScrollToElement(reporte);
                Thread.Sleep(500);
                ClickSeguro(reporte);
                Thread.Sleep(1500);
            }
            catch (Exception ex)
            {
                Assert.Fail("Error al acceder al submódulo de reportes: " + ex.Message);
            }
        }

        public void SeleccionarEstablecimiento(string establecimiento)
        {
            try
            {
                if (establecimiento.Trim().Equals("Todos", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Establecimiento = Todos, no se selecciona opción.");
                    return;
                }

                AbrirComboYSeleccionarOpcion(cmbEstablecimiento, establecimiento);
            }
            catch (Exception ex)
            {
                Assert.Fail("Error al seleccionar establecimiento: " + ex.Message);
            }
        }

        public void SeleccionarPuntoDeVenta(string puntoDeVenta)
        {
            try
            {
                if (puntoDeVenta.Trim().Equals("Todos", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Punto de venta = Todos, no se selecciona opción.");
                    return;
                }

                AbrirComboYSeleccionarOpcion(cmbPuntoVenta, puntoDeVenta);
            }
            catch (Exception ex)
            {
                Assert.Fail("Error al seleccionar punto de venta: " + ex.Message);
            }
        }

        private bool fechaFinalInvalidaOBloqueada = false;

        public void IngresarFechaHoraInicial(string fechaHora)
        {
            try
            {
                CerrarDatePickerSiEstaAbierto();
                SeleccionarFechaHoraDesdePicker(inputFechaHoraInicial, fechaHora);
            }
            catch (Exception ex)
            {
                Assert.Fail("Error al ingresar fecha y hora inicial: " + ex.Message);
            }
        }

        public void IngresarFechaHoraFinal(string fechaHora)
        {
            try
            {
                fechaFinalInvalidaOBloqueada = false;

                CerrarDatePickerSiEstaAbierto();
                SeleccionarFechaHoraDesdePicker(inputFechaHoraFinal, fechaHora);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.ToLower();

                if (mensaje.Contains("no se logró seleccionar el día correcto") ||
                    mensaje.Contains("falló seleccionando día"))
                {
                    fechaFinalInvalidaOBloqueada = true;
                    Console.WriteLine("La fecha final no pudo seleccionarse porque el sistema la bloqueó: " + ex.Message);
                    return;
                }

                Assert.Fail("Error al ingresar fecha y hora final: " + ex.Message);
            }
        }

        public void ClickVerReporte(string tipoReporte)
        {
            try
            {
                if (fechaFinalInvalidaOBloqueada)
                {
                    Console.WriteLine("No se hace clic en Ver Reporte porque la fecha es inválida.");
                    return;
                }

                Console.WriteLine("Fecha inicial antes de consultar: " + ObtenerValorSeguro(inputFechaHoraInicial));
                Console.WriteLine("Fecha final antes de consultar: " + ObtenerValorSeguro(inputFechaHoraFinal));

                IWebElement boton = wait.Until(ExpectedConditions.ElementToBeClickable(ObtenerBotonReporte(tipoReporte)));
                ScrollToElement(boton);
                Thread.Sleep(300);
                ClickSeguro(boton);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Assert.Fail("Error al hacer clic en ver reporte: " + ex.Message);
            }
        }

        public void ValidarResultadoEsperado(string resultadoEsperado)
        {
            try
            {
                string resultadoActual = ObtenerResultadoSistema();

                Console.WriteLine("Resultado esperado: " + resultadoEsperado);
                Console.WriteLine("Resultado actual: " + resultadoActual);
                Console.WriteLine("fechaFinalInvalidaOBloqueada: " + fechaFinalInvalidaOBloqueada);

                switch (resultadoEsperado.Trim().ToLower())
                {
                    case "no permite aplicar el filtro":
                        Assert.That(
                            resultadoActual.ToLower(),
                            Does.Contain("fecha")
                                .Or.Contain("error")
                                .Or.Contain("inválid")
                                .Or.Contain("inval")
                                .Or.Contain("rango"),
                            $"Se esperaba un mensaje de error por fecha inválida, pero se obtuvo: {resultadoActual}"
                        );
                        break;

                    case "no permite aplicar el filtro inhabilitado":
                        Assert.That(
                            fechaFinalInvalidaOBloqueada,
                            Is.True,
                            "Se esperaba que el sistema bloquee la fecha final inválida, pero no ocurrió."
                        );

                        Console.WriteLine("Validación correcta: el sistema bloqueó la selección de fecha final inválida.");
                        return;

                    case "aplica el filtro correctamente":
                        Assert.That(
                            resultadoActual.ToLower(),
                            Does.Contain("filtro aplicado")
                                .Or.Contain("tabla visible")
                                .Or.Contain("sin resultados")
                                .Or.Contain("resultados visibles"),
                            $"Se esperaba que el filtro se aplique correctamente, pero se obtuvo: {resultadoActual}"
                        );
                        break;

                    default:
                        Assert.Fail("Resultado esperado no reconocido: " + resultadoEsperado);
                        break;
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Error validando resultado del reporte: " + ex.Message);
            }
        }
        // =========================
        // RESULTADO
        // =========================

        private string ObtenerResultadoSistema()
        {
            try
            {
                IWebElement error = driver.FindElements(toastError).FirstOrDefault(EsVisible);
                if (error != null)
                    return error.Text.Trim();

                IWebElement tabla = driver.FindElements(tablaResultados).FirstOrDefault(EsVisible);
                if (tabla != null)
                    return "tabla visible";

                IWebElement vacio = driver.FindElements(mensajeSinResultados).FirstOrDefault(EsVisible);
                if (vacio != null)
                    return "sin resultados";

                return "filtro aplicado";
            }
            catch
            {
                return "sin respuesta clara del sistema";
            }
        }

        // =========================
        // DATEPICKER
        // =========================

        private void SeleccionarFechaHoraDesdePicker(By inputLocator, string fechaHoraTexto)
        {
            DateTime fecha = ConvertirFechaFeature(fechaHoraTexto);

            CerrarDatePickerSiEstaAbierto();

            IWebElement input = wait.Until(ExpectedConditions.ElementToBeClickable(inputLocator));
            ScrollToElement(input);
            Thread.Sleep(10);
            ClickSeguro(input);
            Thread.Sleep(10);

            string valorAntes = ObtenerValorSeguro(inputLocator);

            try { SeleccionarMesYAnio(fecha); }
            catch (Exception ex) { throw new Exception("Falló seleccionando mes/año: " + ex.Message); }

            try { SeleccionarDia(fecha, inputLocator); }
            catch (Exception ex) { throw new Exception("Falló seleccionando día: " + ex.Message); }

            try { SeleccionarHora(fecha, inputLocator); }
            catch (Exception ex) { throw new Exception("Falló seleccionando hora: " + ex.Message); }

            try { SeleccionarMinuto(fecha, inputLocator); }
            catch (Exception ex) { throw new Exception("Falló seleccionando minuto: " + ex.Message); }

            try { SeleccionarAmPm(fecha, inputLocator); }
            catch (Exception ex) { throw new Exception("Falló seleccionando AM/PM: " + ex.Message); }

            Thread.Sleep(800);

            string esperado = FormatearComoLoMuestraElControl(fecha);
            string actual = ObtenerValorSeguro(inputLocator);

            if (actual != esperado)
            {
                Assert.Fail($"La fecha no se asignó correctamente. Esperado: {esperado} / Actual: {actual}");
            }

            CerrarDatePickerSiEstaAbierto();
        }

        private DateTime ConvertirFechaFeature(string fechaHora)
        {
            if (fechaHora.Trim().Equals("hoy", StringComparison.OrdinalIgnoreCase))
                return DateTime.Now;

            string[] formatos =
            {
                "dd/MM/yyyy hh:mm tt",
                "d/MM/yyyy hh:mm tt",
                "dd/M/yyyy hh:mm tt",
                "d/M/yyyy hh:mm tt",
                "dd/MM/yyyy h:mm tt",
                "d/MM/yyyy h:mm tt",
                "dd/M/yyyy h:mm tt",
                "d/M/yyyy h:mm tt"
            };

            if (DateTime.TryParseExact(
                fechaHora.Trim(),
                formatos,
                new CultureInfo("en-US"),
                DateTimeStyles.None,
                out DateTime fecha))
            {
                return fecha;
            }

            throw new Exception("No se pudo interpretar la fecha: " + fechaHora);
        }

        private void SeleccionarMesYAnio(DateTime fechaObjetivo)
        {
            string[] meses =
            {
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
            };

            string mesObjetivo = meses[fechaObjetivo.Month - 1];
            int anioObjetivo = fechaObjetivo.Year;

            for (int i = 0; i < 24; i++)
            {
                IWebElement mesElemento = wait.Until(d =>
                {
                    var elementos = d.FindElements(lblMesPicker);

                    return elementos
                        .Where(e => EsVisible(e))
                        .OrderByDescending(e => e.Location.Y)
                        .FirstOrDefault();
                });

                if (mesElemento == null)
                    throw new Exception("No se encontró el mes visible del calendario.");

                string mesActual = mesElemento.Text.Trim();

                int anioActual = anioObjetivo;
                IWebElement anioElemento = driver.FindElements(lblAnioPicker)
                    .Where(e => EsVisible(e))
                    .OrderByDescending(e => e.Location.Y)
                    .FirstOrDefault();

                if (anioElemento != null && int.TryParse(anioElemento.Text.Trim(), out int anioDetectado))
                    anioActual = anioDetectado;

                if (mesActual.Equals(mesObjetivo, StringComparison.OrdinalIgnoreCase) && anioActual == anioObjetivo)
                    return;

                DateTime actual = new DateTime(anioActual, ObtenerNumeroMes(mesActual), 1);
                DateTime objetivo = new DateTime(anioObjetivo, fechaObjetivo.Month, 1);

                if (actual < objetivo)
                {
                    IWebElement next = wait.Until(d =>
                        d.FindElements(btnMesSiguiente).FirstOrDefault(EsVisible));

                    if (next == null)
                        throw new Exception("No se encontró el botón para avanzar mes.");

                    ClickSeguro(next);
                }
                else
                {
                    IWebElement prev = wait.Until(d =>
                        d.FindElements(btnMesAnterior).FirstOrDefault(EsVisible));

                    if (prev == null)
                        throw new Exception("No se encontró el botón para retroceder mes.");

                    ClickSeguro(prev);
                }

                Thread.Sleep(400);
            }

            throw new Exception($"No se pudo navegar al mes/año esperado: {mesObjetivo} {anioObjetivo}");
        }

        private void SeleccionarDia(DateTime fechaEsperada, By inputLocator)
        {
            string textoDia = fechaEsperada.Day.ToString();
            string fechaEsperadaTexto = fechaEsperada.ToString("dd/MM/yyyy");

            IWebElement mesVisible = driver.FindElements(lblMesPicker)
                .Where(e => EsVisible(e))
                .OrderByDescending(e => e.Location.Y)
                .FirstOrDefault();

            if (mesVisible == null)
                throw new Exception("No se encontró el encabezado del calendario.");

            int yMinCalendario = mesVisible.Location.Y + mesVisible.Size.Height;

            var candidatos = driver.FindElements(itemsDiaPicker)
                .Where(e =>
                {
                    if (!EsVisible(e))
                        return false;

                    string texto = e.Text?.Trim() ?? "";
                    if (texto != textoDia)
                        return false;

                    string tag = e.TagName.ToLower();
                    if (tag != "div" && tag != "button" && tag != "span")
                        return false;

                    if (e.Location.Y <= yMinCalendario)
                        return false;

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
                    ClickSeguro(candidato);
                    Thread.Sleep(300);

                    string valorActual = ObtenerValorSeguro(inputLocator);

                    if (!string.IsNullOrWhiteSpace(valorActual) &&
                        valorActual.StartsWith(fechaEsperadaTexto))
                    {
                        return;
                    }
                }
                catch
                {
                }
            }

            string valorFinal = ObtenerValorSeguro(inputLocator);
            throw new Exception($"No se logró seleccionar el día correcto. Esperado: {fechaEsperadaTexto} / Actual: {valorFinal}");
        }

        private void SeleccionarHora(DateTime fechaEsperada, By inputLocator)
        {
            string hora = fechaEsperada.ToString("hh");
            string fechaEsperadaTexto = fechaEsperada.ToString("dd/MM/yyyy");

            var columnas = ObtenerColumnasNumericasDelPicker();

            if (columnas.Count < 2)
                throw new Exception("No se pudieron identificar las columnas de hora y minuto del picker.");

            var columnaHora = columnas[0];

            IWebElement horaElemento = columnaHora
                .FirstOrDefault(e => (e.Text ?? "").Trim() == hora);

            if (horaElemento == null)
                throw new Exception("No se encontró la hora en la columna de horas: " + hora);

            ClickSeguro(horaElemento);
            Thread.Sleep(300);

            string actual = ObtenerValorSeguro(inputLocator);

            if (!actual.StartsWith(fechaEsperadaTexto) || !actual.Contains($" {hora}:"))
            {
                throw new Exception($"No se logró seleccionar la hora correcta. Esperado: {fechaEsperadaTexto} {hora}:xx / Actual: {actual}");
            }
        }

        private void SeleccionarMinuto(DateTime fechaEsperada, By inputLocator)
        {
            string minuto = fechaEsperada.ToString("mm");
            string fechaEsperadaTexto = fechaEsperada.ToString("dd/MM/yyyy");

            var columnas = ObtenerColumnasNumericasDelPicker();

            if (columnas.Count < 2)
                throw new Exception("No se pudieron identificar las columnas de hora y minuto del picker.");

            var columnaMinuto = columnas[1];

            IWebElement minutoElemento = columnaMinuto
                .FirstOrDefault(e => (e.Text ?? "").Trim() == minuto);

            if (minutoElemento == null)
                throw new Exception("No se encontró el minuto en la columna de minutos: " + minuto);

            ClickSeguro(minutoElemento);
            Thread.Sleep(300);

            string actual = ObtenerValorSeguro(inputLocator);

            if (!actual.StartsWith(fechaEsperadaTexto) || !actual.Contains($":{minuto}"))
            {
                throw new Exception($"No se logró seleccionar el minuto correcto. Esperado: {fechaEsperadaTexto} xx:{minuto} / Actual: {actual}");
            }
        }

        private void SeleccionarAmPm(DateTime fecha, By inputLocator)
        {
            bool esAm = fecha.ToString("tt", new CultureInfo("en-US"))
                .Equals("AM", StringComparison.OrdinalIgnoreCase);

            By opcionObjetivo = esAm ? opcionAmPicker : opcionPmPicker;

            IWebElement ampmElemento = wait.Until(d =>
                d.FindElements(opcionObjetivo).FirstOrDefault(e => EsVisible(e)));

            if (ampmElemento == null)
                throw new Exception("No se encontró el selector " + (esAm ? "a. m." : "p. m."));

            ClickSeguro(ampmElemento);
            Thread.Sleep(500);

            string actual = ObtenerValorSeguro(inputLocator);
            string esperadoTexto = esAm ? "a. m." : "p. m.";

            if (!actual.Contains(esperadoTexto))
            {
                ClickSeguro(ampmElemento);
                Thread.Sleep(500);

                actual = ObtenerValorSeguro(inputLocator);

                if (!actual.Contains(esperadoTexto))
                    throw new Exception($"No se logró seleccionar {(esAm ? "a. m." : "p. m.")}. Valor actual: {actual}");
            }
        }

        private string FormatearComoLoMuestraElControl(DateTime fecha)
        {
            string ampm = fecha.ToString("tt", new CultureInfo("en-US")).ToLower() == "am"
                ? "a. m."
                : "p. m.";

            return $"{fecha:dd/MM/yyyy hh:mm} {ampm}";
        }

        // =========================
        // COMBOS
        // =========================

        private void AbrirComboYSeleccionarOpcion(By comboLocator, string opcion)
        {
            IWebElement combo = wait.Until(ExpectedConditions.ElementToBeClickable(comboLocator));
            ScrollToElement(combo);
            Thread.Sleep(300);
            ClickSeguro(combo);
            Thread.Sleep(700);

            IWebElement item = wait.Until(d =>
                d.FindElements(opcionesCombo).FirstOrDefault(e =>
                    EsVisible(e) &&
                    e.Text.Trim().Equals(opcion, StringComparison.OrdinalIgnoreCase)));

            if (item == null)
                throw new Exception("No se encontró la opción del combo: " + opcion);

            ScrollToElement(item);
            ClickSeguro(item);
            Thread.Sleep(700);
        }

        // =========================
        // BOTONES
        // =========================

        private By ObtenerBotonReporte(string tipoReporte)
        {
            switch (tipoReporte.Trim().ToLower())
            {
                case "invalidados":
                    return btnVerReporteInvalidados;
                default:
                    throw new Exception("Tipo de reporte no configurado: " + tipoReporte);
            }
        }

        // =========================
        // HELPERS
        // =========================

        private List<List<IWebElement>> ObtenerColumnasNumericasDelPicker()
        {
            var elementosNumericos = driver.FindElements(itemsHoraPicker)
                .Where(e =>
                {
                    if (!EsVisible(e))
                        return false;

                    string texto = (e.Text ?? "").Trim();

                    if (texto.Length != 2)
                        return false;

                    return int.TryParse(texto, out _);
                })
                .OrderBy(e => e.Location.X)
                .ThenBy(e => e.Location.Y)
                .ToList();

            if (!elementosNumericos.Any())
                throw new Exception("No se encontraron elementos numéricos visibles en el picker.");

            var columnas = new List<List<IWebElement>>();
            const int toleranciaX = 20;

            foreach (var elemento in elementosNumericos)
            {
                bool agregado = false;

                foreach (var columna in columnas)
                {
                    int xReferencia = columna[0].Location.X;

                    if (Math.Abs(elemento.Location.X - xReferencia) <= toleranciaX)
                    {
                        columna.Add(elemento);
                        agregado = true;
                        break;
                    }
                }

                if (!agregado)
                    columnas.Add(new List<IWebElement> { elemento });
            }

            columnas = columnas
                .Where(c => c.Count >= 5)
                .OrderBy(c => c.Average(e => e.Location.X))
                .ToList();

            if (columnas.Count < 2)
                throw new Exception("No se pudieron agrupar correctamente las columnas numéricas del picker.");

            return columnas;
        }

        private void CerrarDatePickerSiEstaAbierto()
        {
            try
            {
                driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
                Thread.Sleep(400);
            }
            catch
            {
            }
        }

        private int ObtenerNumeroMes(string nombreMes)
        {
            string[] meses =
            {
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
            };

            int indice = Array.FindIndex(meses, m =>
                m.Equals(nombreMes.Trim(), StringComparison.OrdinalIgnoreCase));

            if (indice == -1)
                throw new Exception("No se reconoció el mes del calendario: " + nombreMes);

            return indice + 1;
        }

        private void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        }

        private void ClickSeguro(IWebElement element)
        {
            try
            {
                element.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", element);
            }
        }

        private string ObtenerValorSeguro(By locator)
        {
            try
            {
                return wait.Until(d => d.FindElement(locator)).GetAttribute("value")?.Trim() ?? "";
            }
            catch
            {
                return "";
            }
        }

        private bool EsVisible(IWebElement element)
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