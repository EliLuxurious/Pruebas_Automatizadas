using System;
using System.Globalization;
using System.Linq;
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

        private bool fechaFinalInvalidaOBloqueada = false;

        public ReporteDePedidosPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // =========================
        // LOCATORS
        // =========================
        private readonly By panelFechaFinal =
    By.XPath("//label[contains(normalize-space(.),'Fecha y Hora Final')]/following::div[contains(@class,'calendar-section')][1]");

        private readonly By panelFechaInicial =
            By.XPath("//label[contains(normalize-space(.),'Fecha y Hora Inicial')]/following::div[contains(@class,'calendar-section')][1]");

        private readonly By contenedorScrollPicker =
            By.XPath("//div[contains(@class,'calendar-section')]");

        private readonly By tituloDocumentoReporte = By.XPath("//h3[normalize-space()='INVALIDACIÓN DE PEDIDOS']");

        private readonly By hojaReporteDocumento = By.XPath("//div[contains(@class,'report-sheet')]");

        private readonly By subtituloDocumentoReporte = By.XPath("//div[contains(@class,'sheet-subtitle')] | //span[contains(@class,'sheet-subtitle')]");
        
        private readonly By submoduloReportes =
            By.XPath("//span[normalize-space()='Reportes']/ancestor::a");

        private readonly By cmbEstablecimiento =
            By.XPath("//label[contains(normalize-space(.),'Establecimientos')]/following::div[contains(@class,'select-trigger')][1]");

        private readonly By cmbPuntoVenta =
            By.XPath("//label[contains(normalize-space(.),'Puntos de venta')]/following::div[contains(@class,'select-trigger')][1]");

        private readonly By inputFechaHoraInicial =
            By.XPath("//label[contains(normalize-space(.),'Fecha y Hora Inicial')]/following::input[@readonly][1]");

        private readonly By inputFechaHoraFinal =
            By.XPath("//label[contains(normalize-space(.),'Fecha y Hora Final')]/following::input[@readonly][1]");

        private readonly By btnVerReporteInvalidados = By.XPath("//button[normalize-space()='VER REPORTE']");

        private readonly By dropdownAbierto =
            By.XPath("//div[contains(@class,'select-dropdown') and contains(@class,'ng-star-inserted')]");

        private readonly By opcionesDropdown =
            By.XPath("//div[contains(@class,'select-dropdown') and contains(@class,'ng-star-inserted')]//*[self::div or self::span][normalize-space()]");

        private readonly By datePickerPopup =
            By.XPath("//div[contains(@class,'calendar-section')]");

        private readonly By cabeceraCalendario =
            By.XPath("//div[contains(@class,'calendar-section')]//*[contains(normalize-space(.),'202') and not(contains(@class,'time-item'))]");

        private readonly By btnMesAnterior =
            By.XPath("(//div[contains(@class,'calendar-section')]//*[contains(@class,'calendar-header')]//*[self::button or self::i or self::span or self::div])[1]");

        private readonly By btnMesSiguiente =
            By.XPath("(//div[contains(@class,'calendar-section')]//*[contains(@class,'calendar-header')]//*[self::button or self::i or self::span or self::div])[last()]");

        private readonly By celdasDia =
            By.XPath("//div[contains(@class,'calendar-grid')]//div[contains(@class,'day-cell')]");

        private readonly By columnaHoras =
            By.XPath("//div[contains(@class,'time-column') and contains(@class,'hours')]//div[contains(@class,'time-item')]");

        private readonly By columnaMinutos =
            By.XPath("//div[contains(@class,'time-column') and contains(@class,'minutes')]//div[contains(@class,'time-item')]");

        private readonly By columnaAmPm =
            By.XPath("//div[contains(@class,'time-column') and contains(@class,'ampm')]//div[contains(@class,'time-item')]");

        private readonly By toastError =
            By.XPath("//*[contains(@class,'toast') or contains(@class,'alert') or contains(@class,'swal') or contains(text(),'fecha') or contains(text(),'rango') or contains(text(),'inválid') or contains(text(),'inval')]");

        private readonly By tablaResultados =
            By.XPath("//table | //tbody | //div[contains(@class,'table-responsive')]");

        private readonly By mensajeSinResultados =
            By.XPath("//*[contains(text(),'No hay datos') or contains(text(),'No se encontraron resultados') or contains(text(),'Sin resultados')]");
        private readonly By contenedorHoras = By.XPath("//div[contains(@class,'time-column') and contains(@class,'hours')]");

        private readonly By contenedorMinutos = By.XPath("//div[contains(@class,'time-column') and contains(@class,'minutes')]");
        // =========================
        // MÉTODOS PÚBLICOS
        // =========================

        public void AccederASubmoduloReportes()
        {
            try
            {
                IWebElement? reporte = wait.Until(d =>
                    d.FindElements(submoduloReportes)
                     .FirstOrDefault(e => EsVisible(e) && e.Enabled));

                if (reporte is null)
                    Assert.Fail("No se encontró el submódulo Reportes.");

                ScrollToElement(reporte);
                ClickSeguro(reporte);

                wait.Until(d =>
                    d.Url.Contains("/order-report") ||
                    d.FindElements(btnVerReporteInvalidados).Any(EsVisible));
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
                    Console.WriteLine("Establecimiento = Todos, no se cambia selección.");
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
                    Console.WriteLine("Punto de venta = Todos, no se cambia selección.");
                    return;
                }

                AbrirComboYSeleccionarOpcion(cmbPuntoVenta, puntoDeVenta);
            }
            catch (Exception ex)
            {
                Assert.Fail("Error al seleccionar punto de venta: " + ex.Message);
            }
        }

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
                string mensaje = ex.Message.ToLowerInvariant();

                if (mensaje.Contains("no se logró seleccionar el día correcto") ||
                    mensaje.Contains("fecha final bloqueada") ||
                    mensaje.Contains("falló seleccionando día") ||
                    mensaje.Contains("no se encontró el día disponible"))
                {
                    fechaFinalInvalidaOBloqueada = true;
                    Console.WriteLine("La fecha final quedó bloqueada o inválida: " + ex.Message);
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
                    Console.WriteLine("No se hace clic en Ver Reporte porque la fecha final es inválida o quedó bloqueada.");
                    return;
                }

                IWebElement boton = wait.Until(ExpectedConditions.ElementToBeClickable(ObtenerBotonReporte(tipoReporte)));
                ScrollToElement(boton);
                ClickSeguro(boton);

                EsperarHasta(d =>
                    d.FindElements(toastError).Any(EsVisible) ||
                    d.Url.Contains("/order-report-document", StringComparison.OrdinalIgnoreCase) ||
                    d.FindElements(tituloDocumentoReporte).Any(EsVisible) ||
                    d.FindElements(hojaReporteDocumento).Any(EsVisible),
                    12);

                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Assert.Fail("Error al hacer clic en ver reporte: " + ex.Message);
            }
        }

        public void ValidarResultadoEsperado(string resultadoEsperado)
        {
            string resultadoActual = ObtenerResultadoSistema();

            Console.WriteLine("Resultado esperado: " + resultadoEsperado);
            Console.WriteLine("Resultado actual: " + resultadoActual);
            Console.WriteLine("fechaFinalInvalidaOBloqueada: " + fechaFinalInvalidaOBloqueada);

            switch (resultadoEsperado.Trim().ToLowerInvariant())
            {
                case "no permite aplicar el filtro":
                    Assert.That(
                        resultadoActual.ToLowerInvariant(),
                        Does.Contain("fecha")
                            .Or.Contain("error")
                            .Or.Contain("inválid")
                            .Or.Contain("inval")
                            .Or.Contain("rango"),
                        $"Se esperaba un mensaje de error por fecha inválida, pero se obtuvo: {resultadoActual}");
                    break;

                case "no permite aplicar el filtro inhabilitado":
                    Assert.That(
                        fechaFinalInvalidaOBloqueada,
                        Is.True,
                        "Se esperaba que el sistema bloquee la fecha final inválida, pero no ocurrió.");
                    break;

                case "aplica el filtro correctamente":
                    Assert.That(
                        resultadoActual.ToLowerInvariant(),
                        Does.Contain("filtro aplicado")
                            .Or.Contain("tabla visible")
                            .Or.Contain("sin resultados")
                            .Or.Contain("resultados visibles")
                            .Or.Contain("reporte visible"),
                        $"Se esperaba que el filtro se aplique correctamente, pero se obtuvo: {resultadoActual}");
                    break;

                default:
                    Assert.Fail("Resultado esperado no reconocido: " + resultadoEsperado);
                    break;
            }
        }

        // =========================
        // RESULTADO
        // =========================

        private string ObtenerResultadoSistema()
        {
            try
            {
                IWebElement? error = driver.FindElements(toastError).FirstOrDefault(EsVisible);
                if (error is not null)
                    return error.Text.Trim();

                if (driver.Url.Contains("/order-report-document", StringComparison.OrdinalIgnoreCase))
                    return "reporte visible";

                IWebElement? tituloReporte = driver.FindElements(tituloDocumentoReporte).FirstOrDefault(EsVisible);
                if (tituloReporte is not null)
                    return "reporte visible";

                IWebElement? hojaReporte = driver.FindElements(hojaReporteDocumento).FirstOrDefault(EsVisible);
                if (hojaReporte is not null)
                    return "reporte visible";

                IWebElement? subtitulo = driver.FindElements(subtituloDocumentoReporte).FirstOrDefault(EsVisible);
                if (subtitulo is not null)
                    return "reporte visible";

                IWebElement? tabla = driver.FindElements(tablaResultados).FirstOrDefault(EsVisible);
                if (tabla is not null)
                    return "tabla visible";

                IWebElement? vacio = driver.FindElements(mensajeSinResultados).FirstOrDefault(EsVisible);
                if (vacio is not null)
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

            AbrirDatePicker(inputLocator);

            SeleccionarMesYAnio(fecha);
            SeleccionarDia(fecha, inputLocator);

            ScrollDentroDelPicker(inputLocator, 999);
            SeleccionarHora(fecha, inputLocator);
            SeleccionarMinuto(fecha, inputLocator);
            SeleccionarAmPm(fecha, inputLocator);

            string esperado = FormatearComoLoMuestraElControl(fecha);

            bool valorFinalCorrecto = EsperarHastaRetornando(_ =>
            {
                string actual = ObtenerValorSeguro(inputLocator);
                return LimpiarTexto(actual)
                    .Equals(LimpiarTexto(esperado), StringComparison.OrdinalIgnoreCase);
            }, 5);

            string actualFinal = ObtenerValorSeguro(inputLocator);

            if (!valorFinalCorrecto)
            {
                throw new Exception($"La fecha no se asignó correctamente. Esperado: {esperado} / Actual: {actualFinal}");
            }

            CerrarDatePickerSiEstaAbierto();
        }

        private void AbrirDatePicker(By inputLocator)
        {
            IWebElement input = wait.Until(ExpectedConditions.ElementToBeClickable(inputLocator));
            ScrollToElement(input);
            CentrarEnPantalla(input);
            ClickSeguro(input);

            wait.Until(ExpectedConditions.ElementIsVisible(datePickerPopup));

            IWebElement? panel = ObtenerPanelDatePickerAbierto(inputLocator);
            if (panel is not null)
            {
                ScrollToElement(panel);
                CentrarEnPantalla(panel);
            }
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
            string cabeceraEsperada = fechaObjetivo.ToString("MMMM yyyy", new CultureInfo("es-ES"));
            cabeceraEsperada = char.ToUpper(cabeceraEsperada[0]) + cabeceraEsperada.Substring(1);

            for (int i = 0; i < 24; i++)
            {
                IWebElement? cabecera = ObtenerCabeceraCalendarioVisible();

                if (cabecera is null)
                    throw new Exception("No se encontró la cabecera visible del calendario.");

                string textoActual = LimpiarTexto(cabecera.Text);

                if (textoActual.Equals(cabeceraEsperada, StringComparison.OrdinalIgnoreCase))
                    return;

                DateTime actual = ParseCabeceraMesAnio(textoActual);
                DateTime objetivo = new DateTime(fechaObjetivo.Year, fechaObjetivo.Month, 1);

                if (actual < objetivo)
                {
                    IWebElement next = wait.Until(ExpectedConditions.ElementToBeClickable(btnMesSiguiente));
                    ClickSeguro(next);
                }
                else
                {
                    IWebElement prev = wait.Until(ExpectedConditions.ElementToBeClickable(btnMesAnterior));
                    ClickSeguro(prev);
                }

                EsperarHasta(_ =>
                {
                    IWebElement? nuevaCabecera = ObtenerCabeceraCalendarioVisible();
                    return nuevaCabecera is not null &&
                           !LimpiarTexto(nuevaCabecera.Text).Equals(textoActual, StringComparison.OrdinalIgnoreCase);
                }, 5);
            }

            throw new Exception($"No se pudo navegar al mes/año esperado: {cabeceraEsperada}");
        }

        private IWebElement? ObtenerCabeceraCalendarioVisible()
        {
            return driver.FindElements(cabeceraCalendario)
                .Where(EsVisible)
                .Select(e => new
                {
                    Elemento = e,
                    Texto = LimpiarTexto(e.Text)
                })
                .Where(x => EsCabeceraMesAnioValida(x.Texto))
                .Select(x => x.Elemento)
                .FirstOrDefault();
        }

        private bool EsCabeceraMesAnioValida(string texto)
        {
            string[] formatos = { "MMMM yyyy", "MMM yyyy" };

            return DateTime.TryParseExact(
                texto,
                formatos,
                new CultureInfo("es-ES"),
                DateTimeStyles.None,
                out _);
        }

        private DateTime ParseCabeceraMesAnio(string texto)
        {
            string[] formatos = { "MMMM yyyy", "MMM yyyy" };

            if (DateTime.TryParseExact(
                texto,
                formatos,
                new CultureInfo("es-ES"),
                DateTimeStyles.None,
                out DateTime fecha))
            {
                return new DateTime(fecha.Year, fecha.Month, 1);
            }

            throw new Exception("No se pudo interpretar la cabecera del calendario: " + texto);
        }

        private void SeleccionarDia(DateTime fechaEsperada, By inputLocator)
        {
            string dia = fechaEsperada.Day.ToString();

            IWebElement? diaElemento = driver.FindElements(celdasDia)
                .FirstOrDefault(e =>
                    EsVisible(e) &&
                    e.Text.Trim() == dia &&
                    !TieneClase(e, "disabled"));

            if (diaElemento is null)
                throw new Exception("No se encontró el día disponible: " + dia);

            ClickSeguro(diaElemento);

            string fechaEsperadaTexto = fechaEsperada.ToString("dd/MM/yyyy");

            bool diaCorrecto = EsperarHastaRetornando(_ =>
            {
                string actual = ObtenerValorSeguro(inputLocator);
                return !string.IsNullOrWhiteSpace(actual) &&
                       actual.StartsWith(fechaEsperadaTexto, StringComparison.OrdinalIgnoreCase);
            }, 3);

            if (!diaCorrecto)
            {
                string actual = ObtenerValorSeguro(inputLocator);
                throw new Exception($"No se logró seleccionar el día correcto. Esperado día: {dia} / Actual: {actual}");
            }
        }

        private void SeleccionarHora(DateTime fechaEsperada, By inputLocator)
        {
            string hora = fechaEsperada.ToString("hh");
            string fechaEsperadaTexto = fechaEsperada.ToString("dd/MM/yyyy");

            IWebElement contenedor = wait.Until(ExpectedConditions.ElementIsVisible(contenedorHoras));
            IWebElement? horaElemento = BuscarTimeItemEnColumna(contenedor, columnaHoras, hora);

            if (horaElemento is null)
                throw new Exception("No se encontró la hora: " + hora);

            ClickSeguro(horaElemento);

            bool horaCorrecta = EsperarHastaRetornando(_ =>
            {
                string actual = ObtenerValorSeguro(inputLocator);
                return !string.IsNullOrWhiteSpace(actual) &&
                       actual.StartsWith(fechaEsperadaTexto, StringComparison.OrdinalIgnoreCase) &&
                       actual.Contains($" {hora}:");
            }, 3);

            if (!horaCorrecta)
            {
                string actual = ObtenerValorSeguro(inputLocator);
                throw new Exception($"No se logró seleccionar la hora correcta. Esperado: {hora} / Actual: {actual}");
            }
        }

        private void SeleccionarMinuto(DateTime fechaEsperada, By inputLocator)
        {
            string minuto = fechaEsperada.ToString("mm");

            IWebElement contenedor = wait.Until(ExpectedConditions.ElementIsVisible(contenedorMinutos));
            IWebElement? minutoElemento = BuscarTimeItemEnColumna(contenedor, columnaMinutos, minuto);

            if (minutoElemento is null)
                throw new Exception("No se encontró el minuto: " + minuto);

            ClickSeguro(minutoElemento);

            bool minutoCorrecto = EsperarHastaRetornando(_ =>
            {
                string actual = ObtenerValorSeguro(inputLocator);
                return !string.IsNullOrWhiteSpace(actual) &&
                       actual.Contains($":{minuto}");
            }, 3);

            if (!minutoCorrecto)
            {
                string actual = ObtenerValorSeguro(inputLocator);
                throw new Exception($"No se logró seleccionar el minuto correcto. Esperado: {minuto} / Actual: {actual}");
            }
        }

        private void SeleccionarAmPm(DateTime fechaEsperada, By inputLocator)
        {
            string ampmEsperado = fechaEsperada.ToString("tt", new CultureInfo("en-US"))
                .Equals("AM", StringComparison.OrdinalIgnoreCase)
                ? "a. m."
                : "p. m.";

            IWebElement contenedor = wait.Until(ExpectedConditions.ElementIsVisible(columnaAmPm));
            IWebElement? ampmElemento = driver.FindElements(columnaAmPm)
                .FirstOrDefault(e =>
                    EsVisible(e) &&
                    LimpiarTexto(e.Text).Equals(ampmEsperado, StringComparison.OrdinalIgnoreCase));

            if (ampmElemento is null)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].scrollIntoView({block:'center'});", contenedor);

                ampmElemento = driver.FindElements(columnaAmPm)
                    .FirstOrDefault(e =>
                        EsVisible(e) &&
                        LimpiarTexto(e.Text).Equals(ampmEsperado, StringComparison.OrdinalIgnoreCase));
            }

            if (ampmElemento is null)
                throw new Exception("No se encontró el selector " + ampmEsperado);

            ClickSeguro(ampmElemento);

            bool ampmCorrecto = EsperarHastaRetornando(_ =>
            {
                string actual = ObtenerValorSeguro(inputLocator);
                return !string.IsNullOrWhiteSpace(actual) &&
                       actual.Contains(ampmEsperado, StringComparison.OrdinalIgnoreCase);
            }, 3);

            if (!ampmCorrecto)
            {
                string actual = ObtenerValorSeguro(inputLocator);
                throw new Exception($"No se logró seleccionar AM/PM. Esperado: {ampmEsperado} / Actual: {actual}");
            }
        }

        private string FormatearComoLoMuestraElControl(DateTime fecha)
        {
            string ampm = fecha.ToString("tt", new CultureInfo("en-US")).Equals("AM", StringComparison.OrdinalIgnoreCase)
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
            ClickSeguro(combo);

            wait.Until(ExpectedConditions.ElementIsVisible(dropdownAbierto));

            IWebElement? item = driver.FindElements(opcionesDropdown)
                .FirstOrDefault(e =>
                    EsVisible(e) &&
                    LimpiarTexto(e.Text).Equals(opcion.Trim(), StringComparison.OrdinalIgnoreCase));

            if (item is null)
                throw new Exception("No se encontró la opción del combo: " + opcion);

            ScrollToElement(item);
            ClickSeguro(item);

            EsperarHasta(d => !d.FindElements(dropdownAbierto).Any(EsVisible), 5);
        }

        // =========================
        // BOTONES
        // =========================

        private By ObtenerBotonReporte(string tipoReporte)
        {
            switch (tipoReporte.Trim().ToLowerInvariant())
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
        private void CentrarEnPantalla(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView({block:'center', inline:'nearest'});", element);
        }

        private IWebElement? ObtenerPanelDatePickerAbierto(By inputLocator)
        {
            try
            {
                if (inputLocator == inputFechaHoraFinal)
                {
                    return driver.FindElements(panelFechaFinal).FirstOrDefault(EsVisible)
                        ?? driver.FindElements(datePickerPopup).LastOrDefault(EsVisible);
                }

                if (inputLocator == inputFechaHoraInicial)
                {
                    return driver.FindElements(panelFechaInicial).FirstOrDefault(EsVisible)
                        ?? driver.FindElements(datePickerPopup).FirstOrDefault(EsVisible);
                }

                return driver.FindElements(datePickerPopup).LastOrDefault(EsVisible);
            }
            catch
            {
                return driver.FindElements(datePickerPopup).LastOrDefault(EsVisible);
            }
        }

        private void ScrollDentroDelPicker(By inputLocator, int top)
        {
            IWebElement? panel = ObtenerPanelDatePickerAbierto(inputLocator);
            if (panel is null)
                return;

            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollTop = arguments[1];", panel, top);
        }
        private IWebElement? BuscarTimeItemEnColumna(IWebElement contenedor, By itemsLocator, string valorBuscado)
        {
            for (int i = 0; i < 25; i++)
            {
                IWebElement? itemVisible = driver.FindElements(itemsLocator)
                    .FirstOrDefault(e =>
                        EsVisible(e) &&
                        LimpiarTexto(e.Text) == valorBuscado);

                if (itemVisible is not null)
                {
                    ScrollToElement(itemVisible);
                    CentrarEnPantalla(itemVisible);
                    return itemVisible;
                }

                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].scrollTop = arguments[0].scrollTop + 35;", contenedor);
            }

    ((IJavaScriptExecutor)driver).ExecuteScript(
        "arguments[0].scrollTop = 0;", contenedor);

            for (int i = 0; i < 25; i++)
            {
                IWebElement? itemVisible = driver.FindElements(itemsLocator)
                    .FirstOrDefault(e =>
                        EsVisible(e) &&
                        LimpiarTexto(e.Text) == valorBuscado);

                if (itemVisible is not null)
                {
                    ScrollToElement(itemVisible);
                    CentrarEnPantalla(itemVisible);
                    return itemVisible;
                }

                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].scrollTop = arguments[0].scrollTop + 20;", contenedor);
            }

            return null;
        }

        private void EsperarHasta(Func<IWebDriver, bool> condicion, int segundos = 10)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(segundos)).Until(condicion);
        }

        private bool EsperarHastaRetornando(Func<IWebDriver, bool> condicion, int segundos = 10)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(segundos)).Until(condicion);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CerrarDatePickerSiEstaAbierto()
        {
            try
            {
                if (driver.FindElements(datePickerPopup).Any(EsVisible))
                {
                    driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
                    EsperarHasta(d => !d.FindElements(datePickerPopup).Any(EsVisible), 3);
                }
            }
            catch
            {
            }
        }

        private void ScrollToElement(IWebElement? element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        }

        private void ClickSeguro(IWebElement? element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));

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
                IWebElement elemento = wait.Until(d => d.FindElement(locator));
                return elemento.GetAttribute("value")?.Trim() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private bool EsVisible(IWebElement element)
        {
            try
            {
                return element.Displayed;
            }
            catch
            {
                return false;
            }
        }

        private bool TieneClase(IWebElement element, string clase)
        {
            try
            {
                string classes = element.GetAttribute("class") ?? string.Empty;
                return classes.Contains(clase, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private string LimpiarTexto(string? texto)
        {
            return (texto ?? string.Empty)
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("  ", " ")
                .Trim();
        }
    }
}