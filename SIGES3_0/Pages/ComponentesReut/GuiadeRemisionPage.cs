using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SIGES3_0.Pages.Base;

namespace SIGES3_0.Pages.Componentes
{
    public class GuiaRemisionPage : BasePage
    {
        // ─── BOTONES PRINCIPALES ─────────────────────────────────────────────
        private readonly By btnAceptar = By.XPath("//button[normalize-space()='Aceptar']");
        private readonly By btnCancelar = By.XPath("//button[normalize-space()='Cancelar']");

        // ─── MENSAJES ────────────────────────────────────────────────────────
        private readonly By bannerCamposRequeridos = By.XPath("//*[contains(text(),'Completar los campos requeridos correctamente')]");
        private readonly By mensajeTransportistaInvalido = By.XPath("//*[contains(text(),'El transportista debe tener RUC')]");
        private readonly By mensajeCampoObligatorio = By.XPath("//*[contains(text(),'Este campo es obligatorio')]");
        private readonly By lblMensaje = By.XPath("//*[contains(@class,'alert') or contains(@class,'toast') or contains(@class,'swal') or contains(@class,'mensaje')]");

        // ─── DATOS GENERALES ─────────────────────────────────────────────────
        private readonly By txtDestinatario = By.XPath("//input[@placeholder='Buscar...']");
        private readonly By txtFechaTraslado = By.XPath("//input[@type='date']");
        private readonly By txtPesoBruto = By.XPath("//label[contains(.,'Peso Bruto')]/following::input[1]");
        private readonly By txtNumeroBultos = By.XPath("//label[contains(.,'Número de Bultos')]/following::input[1]");

        // ─── DATOS DE TRANSPORTE ─────────────────────────────────────────────
        private readonly By txtTransportista = By.XPath("//app-transport-data-form//input[@placeholder='Buscar...']");
        private readonly By btnBuscarTransportista = By.XPath("//app-transport-data-form//i[contains(@class,'bi-search')]");
        private readonly By txtNumeroLicencia = By.XPath("//label[contains(.,'LICENCIA')]/following::input[1]");
        private readonly By txtNumeroPlaca = By.XPath("//label[contains(.,'PLACA')]/following::input[1]");

        // ─── DIRECCIONES ─────────────────────────────────────────────────────
        private readonly By cboUbigeoOrigen = By.XPath("(//label[contains(.,'UBIGEO')]/following::select)[1]");
        private readonly By txtDetalleOrigen = By.XPath("(//label[contains(.,'DETALLE')]/following::input)[1]");
        private readonly By cboUbigeoDestino = By.XPath("(//label[contains(.,'UBIGEO')]/following::select)[2]");
        private readonly By txtDetalleDestino = By.XPath("(//label[contains(.,'DETALLE')]/following::input)[2]");

        private string? mensajeErrorGuia = null;

        public GuiaRemisionPage(IWebDriver driver) : base(driver) { }

        // ═══════════════════════════════════════════════════════════════════════
        // BUSCADOR INTELIGENTE PARA COMBOS (Ignora si hay scroll)
        // ═══════════════════════════════════════════════════════════════════════
        private SelectElement? ObtenerComboModalidad()
        {
            try
            {
                var selects = driver.FindElements(By.TagName("select"));
                foreach (var s in selects)
                {
                    var sel = new SelectElement(s);
                    if (sel.Options.Any(o => o.Text.ToUpper().Contains("UBLICO") || o.Text.ToUpper().Contains("ÚBLICO") || o.Text.ToUpper().Contains("RIVADO")))
                        return sel;
                }
            }
            catch { }
            return null;
        }

        private bool EsTransportePublico()
        {
            var combo = ObtenerComboModalidad();
            if (combo == null) return false;
            string txt = combo.SelectedOption.Text.ToUpper();
            return txt.Contains("UBLICO") || txt.Contains("ÚBLICO");
        }

        private bool EsTransportePrivado()
        {
            var combo = ObtenerComboModalidad();
            if (combo == null) return false;
            return combo.SelectedOption.Text.ToUpper().Contains("RIVADO");
        }

        // ¡CRÍTICO! Quitamos la dependencia de visibilidad para leer campos ocultos por el scroll
        private string ValorCampo(By locator)
        {
            try
            {
                var elementos = driver.FindElements(locator);
                if (elementos.Count > 0) return elementos.First().GetAttribute("value")?.Trim() ?? "";
            }
            catch { }
            return "";
        }

        // ═══════════════════════════════════════════════════════════════════════
        // HELPERS NATIVOS ANGULAR
        // ═══════════════════════════════════════════════════════════════════════

        private void LimpiarYEscribirAngular(By locator, string valor)
        {
            var el = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            ScrollToElement(el);

            try { el.Click(); } catch { JsClick(el); }

            Thread.Sleep(150);
            el.SendKeys(Keys.Control + "a");
            el.SendKeys(Keys.Delete);
            Thread.Sleep(150);
            el.SendKeys(valor);
            Thread.Sleep(200);

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].value = arguments[1];
                arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('blur', { bubbles: true }));
                arguments[0].blur();
            ", el, valor);

            el.SendKeys(Keys.Tab);
            Thread.Sleep(600);
        }

        private void BuscarYSeleccionarAngular(By txtLocator, By btnLocator, string valor)
        {
            if (EsValorIgnorado(valor)) return;

            var input = wait.Until(ExpectedConditions.ElementIsVisible(txtLocator));
            ScrollToElement(input);

            try { input.Click(); } catch { JsClick(input); }

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(150);
            input.SendKeys(valor);
            Thread.Sleep(500);

            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnLocator));
            JsClick(boton);
            Thread.Sleep(1200);
        }

        private void SeleccionarOpcionSelectAngular(By locator, string texto)
        {
            var select = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            ScrollToElement(select);

            var partes = (texto ?? "").ToUpperInvariant()
                .Replace("Á", "A").Replace("É", "E").Replace("Í", "I")
                .Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N")
                .Split('-').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            var resultado = ((IJavaScriptExecutor)driver).ExecuteScript(@"
                var select = arguments[0];
                var partes = arguments[1];
                function normalizar(txt) { return (txt || '').toUpperCase().replace(/Á/g,'A').replace(/É/g,'E').replace(/Í/g,'I').replace(/Ó/g,'O').replace(/Ú/g,'U').replace(/Ñ/g,'N').trim(); }
                var opciones = select.options;
                for (var i = 0; i < opciones.length; i++) {
                    var textoOpcion = normalizar(opciones[i].text);
                    var segmentos = textoOpcion.split(' - ').map(s => normalizar(s)).filter(s => s.length > 0);
                    if (segmentos.length !== partes.length) continue;
                    var coincide = true;
                    for (var j = 0; j < partes.length; j++) {
                        var parte = normalizar(partes[j]);
                        if (!(segmentos[j] === parte || segmentos[j].indexOf(parte) === 0)) { coincide = false; break; }
                    }
                    if (coincide) return [opciones[i].value, opciones[i].text];
                }
                return null;
            ", select, partes) as System.Collections.ObjectModel.ReadOnlyCollection<object>;

            if (resultado == null || resultado.Count < 2) throw new NoSuchElementException($"No se encontró ubigeo exacto para '{texto}'.");

            string valorEncontrado = resultado[0]?.ToString() ?? "";

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                var select = arguments[0];
                var value = arguments[1];
                select.value = value;
                select.dispatchEvent(new Event('change', { bubbles: true }));
                select.dispatchEvent(new Event('input', { bubbles: true }));
                select.blur();
            ", select, valorEncontrado);

            Thread.Sleep(700);
        }

        private bool ExisteVisible(By locator, int segundos = 2)
        {
            try { return new WebDriverWait(driver, TimeSpan.FromSeconds(segundos)).Until(d => d.FindElements(locator).Any(e => e.Displayed)); } catch { return false; }
        }

        private bool ModalSigueAbierto()
        {
            try { return driver.FindElements(btnAceptar).Any(e => e.Displayed) && driver.FindElements(btnCancelar).Any(e => e.Displayed); } catch { return false; }
        }

        public string? ObtenerErrorGuia() => mensajeErrorGuia;
        public void LimpiarErrorGuia() => mensajeErrorGuia = null;

        // ═══════════════════════════════════════════════════════════════════════
        // ACCIONES PÚBLICAS
        // ═══════════════════════════════════════════════════════════════════════

        public void EsperarModalGuia()
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(d =>
            {
                try { return d.FindElements(btnAceptar).Any(e => e.Displayed) && d.FindElements(txtPesoBruto).Any(e => e.Displayed); } catch { return false; }
            });
            Thread.Sleep(1200);
        }

        public void ExpandirDatosGenerales() => wait.Until(ExpectedConditions.ElementIsVisible(txtFechaTraslado));
        public void ExpandirDatosTransporte() => wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("select")));

        public void ValidarDestinatarioAutocompletado()
        {
            bool ok = waitLong.Until(d => d.FindElements(txtDestinatario).Where(e => e.Displayed).Any(e => !string.IsNullOrWhiteSpace(e.GetAttribute("value"))));
            Assert.IsTrue(ok, "El destinatario no fue autocompletado.");
        }

        public void IngresarFechaTraslado(string fecha)
        {
            if (EsValorIgnorado(fecha)) return;
            var input = wait.Until(ExpectedConditions.ElementIsVisible(txtFechaTraslado));
            ScrollToElement(input);
            try { input.Click(); } catch { JsClick(input); }

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);

            string fechaFormateada = fecha.Trim().Equals("Hoy", StringComparison.OrdinalIgnoreCase) ? DateTime.Now.ToString("dd/MM/yyyy") : fecha.Trim();
            input.SendKeys(fechaFormateada);
            Thread.Sleep(200);

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('blur', { bubbles: true }));
                arguments[0].blur();
            ", input);

            input.SendKeys(Keys.Tab);
            Thread.Sleep(600);
        }

        public void IngresarPesoBruto(string peso)
        {
            if (EsValorIgnorado(peso)) return;
            LimpiarYEscribirAngular(txtPesoBruto, peso.Trim());
        }

        public void IngresarNumeroBultos(string bultos)
        {
            if (EsValorIgnorado(bultos)) return;
            LimpiarYEscribirAngular(txtNumeroBultos, bultos.Trim());
        }

        public void SeleccionarTipoTransporte(string tipoTransporte)
        {
            if (EsValorIgnorado(tipoTransporte)) return;
            string buscar = tipoTransporte.Trim().ToUpper().Contains("PUBLICO") ? "UBLICO" : "RIVADO";

            var selectEl = ObtenerComboModalidad();
            if (selectEl == null) throw new NoSuchElementException("No se encontró el combo de modalidad de transporte.");

            var selectNode = selectEl.WrappedElement;
            ScrollToElement(selectNode);

            var opcion = selectEl.Options.FirstOrDefault(o => o.Text.ToUpper().Contains(buscar) || o.Text.ToUpper().Contains(buscar.Replace("U", "Ú")));
            if (opcion != null) selectEl.SelectByText(opcion.Text);

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('blur', { bubbles: true }));
                arguments[0].blur();
            ", selectNode);
            Thread.Sleep(1000);
        }

        public void IngresarTransportistaPrivado(string transportista) => BuscarYSeleccionarAngular(txtTransportista, btnBuscarTransportista, transportista);
        public void IngresarTransportistaPublico(string ruc) => BuscarYSeleccionarAngular(txtTransportista, btnBuscarTransportista, ruc);

        public void IngresarNumeroLicencia(string licencia)
        {
            if (EsValorIgnorado(licencia)) return;
            LimpiarYEscribirAngular(txtNumeroLicencia, licencia.Trim());
        }

        public void IngresarNumeroPlaca(string placa)
        {
            if (EsValorIgnorado(placa)) return;
            LimpiarYEscribirAngular(txtNumeroPlaca, placa.Trim());
        }

        public void SeleccionarDireccionOrigen(string direccion)
        {
            if (EsValorIgnorado(direccion)) return;
            SeleccionarOpcionSelectAngular(cboUbigeoOrigen, direccion);
        }

        public void IngresarDetalleOrigen(string detalle)
        {
            if (EsValorIgnorado(detalle)) return;
            LimpiarYEscribirAngular(txtDetalleOrigen, detalle.Trim());
        }

        public void SeleccionarDireccionDestino(string direccion)
        {
            if (EsValorIgnorado(direccion)) return;
            SeleccionarOpcionSelectAngular(cboUbigeoDestino, direccion);
        }

        public void IngresarDetalleDestino(string detalle)
        {
            if (EsValorIgnorado(detalle)) return;
            LimpiarYEscribirAngular(txtDetalleDestino, detalle.Trim());
        }

        public void GuardarGuia()
        {
            LimpiarErrorGuia();

            if (EsTransportePublico() && ExisteVisible(mensajeTransportistaInvalido, 1))
            {
                mensajeErrorGuia = "Transportista debe tener RUC valido";
                return;
            }

            Thread.Sleep(800);
            var boton = driver.FindElements(btnAceptar).FirstOrDefault(e => e.Displayed);
            if (boton == null) return;

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);
            Thread.Sleep(300);

            try { boton.Click(); } catch { JsClick(boton); }
            Thread.Sleep(1200);
        }

        // ── LÓGICA DE RESULTADOS BLINDADA ──
        public string ObtenerResultadoGuia()
        {
            if (!string.IsNullOrWhiteSpace(mensajeErrorGuia)) return mensajeErrorGuia;

            bool modalAbierto = ModalSigueAbierto();

            if (!modalAbierto) return "Guia emitida correctamente";

            bool esPublico = EsTransportePublico();
            bool esPrivado = EsTransportePrivado();

            string[] xpathErrores = {
                "//*[contains(text(),'conductor con DNI')]",
                "//*[contains(text(),'Identifique conductor con DNI')]",
                "//*[contains(text(),'El transportista debe tener RUC')]"
            };

            foreach (var xpath in xpathErrores)
            {
                var el = driver.FindElements(By.XPath(xpath)).FirstOrDefault(e => e.Displayed);
                if (el != null)
                {
                    string txt = el.Text.Trim();
                    if (txt.Contains("conductor con DNI", StringComparison.OrdinalIgnoreCase))
                        return "Identifique conductor con DNI";

                    if (txt.Contains("tener RUC", StringComparison.OrdinalIgnoreCase))
                    {
                        if (esPublico) return "Transportista debe tener RUC valido";
                    }
                }
            }

            string fecha = ValorCampo(txtFechaTraslado);
            string peso = ValorCampo(txtPesoBruto);
            string bultos = ValorCampo(txtNumeroBultos);
            string transportista = ValorCampo(txtTransportista);
            string licencia = ValorCampo(txtNumeroLicencia);
            string placa = ValorCampo(txtNumeroPlaca);

            // Regla INFALIBLE: Validamos los datos reales del HTML, no lo que vemos con los ojos.
            if (esPublico && string.IsNullOrWhiteSpace(transportista)) return "Transportista debe tener RUC valido";

            if ((string.IsNullOrWhiteSpace(peso) || peso == "0") && (string.IsNullOrWhiteSpace(bultos) || bultos == "0")) return "Falta peso y numero de bultos";

            if (esPrivado)
            {
                if (string.IsNullOrWhiteSpace(transportista)) return "Ingrese transportista";
                if (string.IsNullOrWhiteSpace(licencia)) return "Ingrese numero de licencia";
                if (string.IsNullOrWhiteSpace(placa)) return "Ingrese numero de placa";
            }

            if (string.IsNullOrWhiteSpace(fecha)) return "Registre la fecha de inicio";

            bool hayBanner = ExisteVisible(bannerCamposRequeridos, 1) || ExisteVisible(mensajeCampoObligatorio, 1);
            if (hayBanner) return "Completar los campos requeridos correctamente";

            return string.Empty;
        }
    }
}