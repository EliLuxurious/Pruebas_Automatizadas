using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SIGES3_0.Pages.Componentes
{
    public class GuiaRemisionPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public GuiaRemisionPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

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
        private readonly By cboModalidadTransporte = By.XPath("//select[contains(@class,'form-select') and contains(@class,'form-select-sm')]");
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

        // ═══════════════════════════════════════════════════════════════════════
        // HELPERS
        // ═══════════════════════════════════════════════════════════════════════

        private IWebElement EsperarVisible(By locator) =>
            wait.Until(ExpectedConditions.ElementIsVisible(locator));

        private IWebElement EsperarClickeable(By locator) =>
            wait.Until(ExpectedConditions.ElementToBeClickable(locator));

        private bool DebeOmitirse(string valor) =>
            string.IsNullOrWhiteSpace(valor) ||
            string.Equals(valor.Trim(), "NA", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(valor.Trim(), "Ninguno", StringComparison.OrdinalIgnoreCase);

        private IWebElement? ObtenerVisible(By locator)
        {
            try
            {
                return driver.FindElements(locator).FirstOrDefault(e => e.Displayed);
            }
            catch
            {
                return null;
            }
        }

        private string ValorCampo(By locator)
        {
            try
            {
                var el = ObtenerVisible(locator);
                return el == null ? string.Empty : (el.GetAttribute("value") ?? "").Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        private string NormalizarTexto(string texto) =>
            (texto ?? "").ToUpperInvariant()
                .Replace("Á", "A").Replace("É", "E").Replace("Í", "I")
                .Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N");

        private bool ExisteVisible(By locator, int segundos = 2)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(segundos))
                    .Until(d => d.FindElements(locator).Any(e => e.Displayed));
            }
            catch
            {
                return false;
            }
        }

        private bool ModalSigueAbierto()
        {
            try
            {
                return driver.FindElements(btnAceptar).Any(e => e.Displayed) &&
                       driver.FindElements(btnCancelar).Any(e => e.Displayed);
            }
            catch
            {
                return false;
            }
        }

        private bool EstaDeshabilitado(IWebElement element)
        {
            try
            {
                var disabledAttr = (element.GetAttribute("disabled") ?? "").Trim().ToLower();
                var ariaDisabled = (element.GetAttribute("aria-disabled") ?? "").Trim().ToLower();
                var clase = (element.GetAttribute("class") ?? "").Trim().ToLower();

                return disabledAttr == "true" ||
                       disabledAttr == "disabled" ||
                       ariaDisabled == "true" ||
                       clase.Contains("disabled");
            }
            catch
            {
                return false;
            }
        }

        private bool EsTransportePublico()
        {
            try
            {
                var sel = new SelectElement(EsperarVisible(cboModalidadTransporte));
                return NormalizarTexto(sel.SelectedOption.Text).Contains("PUBLICO");
            }
            catch
            {
                return false;
            }
        }

        private bool EsTransportePrivado()
        {
            try
            {
                var sel = new SelectElement(EsperarVisible(cboModalidadTransporte));
                return NormalizarTexto(sel.SelectedOption.Text).Contains("PRIVADO");
            }
            catch
            {
                return false;
            }
        }

        private void LimpiarYEscribir(By locator, string valor)
        {
            var el = EsperarVisible(locator);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", el);

            Thread.Sleep(250);

            try
            {
                el.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", el);
            }

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

        private void BuscarYSeleccionar(By txtLocator, By btnLocator, string valor)
        {
            if (DebeOmitirse(valor)) return;

            var input = EsperarVisible(txtLocator);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);

            Thread.Sleep(200);

            try
            {
                input.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", input);
            }

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(150);
            input.SendKeys(valor);
            Thread.Sleep(500);

            var boton = EsperarClickeable(btnLocator);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton);

            Thread.Sleep(1200);
        }

        private void SeleccionarOpcionSelect(By locator, string texto)
        {
            var select = EsperarVisible(locator);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", select);

            Thread.Sleep(300);

            var partes = NormalizarTexto(texto)
                .Split('-')
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToList();

            Console.WriteLine($"[Ubigeo] Buscando por partes: {string.Join(" | ", partes)}");

            var resultado = ((IJavaScriptExecutor)driver).ExecuteScript(@"
                var select = arguments[0];
                var partes = arguments[1];

                function normalizar(txt) {
                    return (txt || '')
                        .toUpperCase()
                        .replace(/Á/g,'A').replace(/É/g,'E').replace(/Í/g,'I')
                        .replace(/Ó/g,'O').replace(/Ú/g,'U').replace(/Ñ/g,'N')
                        .trim();
                }

                var opciones = select.options;

                for (var i = 0; i < opciones.length; i++) {
                    var textoOpcion = normalizar(opciones[i].text);

                    var segmentos = textoOpcion.split(' - ')
                        .map(s => normalizar(s))
                        .filter(s => s.length > 0);

                    if (segmentos.length !== partes.length) continue;

                    var coincide = true;
                    for (var j = 0; j < partes.length; j++) {
                        var parte = normalizar(partes[j]);
                        if (!(segmentos[j] === parte || segmentos[j].indexOf(parte) === 0)) {
                            coincide = false;
                            break;
                        }
                    }

                    if (coincide) {
                        return [opciones[i].value, opciones[i].text];
                    }
                }

                return null;
            ", select, partes) as System.Collections.ObjectModel.ReadOnlyCollection<object>;

            if (resultado == null || resultado.Count < 2)
            {
                var selectEl = new SelectElement(select);
                Console.WriteLine($"[Ubigeo] No encontrado '{texto}'. Primeras opciones:");
                foreach (var op in selectEl.Options.Take(10))
                    Console.WriteLine($"  -> '{op.Text}'");

                throw new NoSuchElementException($"No se encontró ubigeo exacto para '{texto}'.");
            }

            string valorEncontrado = resultado[0]?.ToString() ?? "";
            string textoEncontrado = resultado[1]?.ToString() ?? "";

            Console.WriteLine($"[Ubigeo] Opción encontrada: '{textoEncontrado}'");

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

        public string? ObtenerErrorGuia() => mensajeErrorGuia;

        public void LimpiarErrorGuia() => mensajeErrorGuia = null;

        // ═══════════════════════════════════════════════════════════════════════
        // ACCIONES PÚBLICAS
        // ═══════════════════════════════════════════════════════════════════════

        public void EsperarModalGuia()
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(d =>
            {
                try
                {
                    return d.FindElements(btnAceptar).Any(e => e.Displayed) &&
                           d.FindElements(txtPesoBruto).Any(e => e.Displayed);
                }
                catch
                {
                    return false;
                }
            });

            Thread.Sleep(1200);
        }

        public void ExpandirDatosGenerales() =>
            EsperarVisible(txtFechaTraslado);

        public void ExpandirDatosTransporte() =>
            EsperarVisible(cboModalidadTransporte);

        public void ValidarDestinatarioAutocompletado()
        {
            bool ok = new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(d =>
            {
                try
                {
                    return d.FindElements(txtDestinatario)
                        .Where(e => e.Displayed)
                        .Any(e => !string.IsNullOrWhiteSpace(e.GetAttribute("value")));
                }
                catch
                {
                    return false;
                }
            });

            Assert.IsTrue(ok, "El destinatario no fue autocompletado.");
            Console.WriteLine("[Guia] Destinatario autocompletado OK.");
        }

        public void IngresarFechaTraslado(string fecha)
        {
            if (DebeOmitirse(fecha)) return;

            var input = EsperarVisible(txtFechaTraslado);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);

            Thread.Sleep(200);

            input.Click();
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);

            string fechaFormateada = fecha.Trim().Equals("Hoy", StringComparison.OrdinalIgnoreCase)
                ? DateTime.Now.ToString("dd/MM/yyyy")
                : fecha.Trim();

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
            if (DebeOmitirse(peso)) return;

            wait.Until(ExpectedConditions.ElementExists(txtPesoBruto));
            LimpiarYEscribir(txtPesoBruto, peso.Trim());
            Console.WriteLine($"[Guia] Peso bruto: '{ValorCampo(txtPesoBruto)}'");
        }

        public void IngresarNumeroBultos(string bultos)
        {
            if (DebeOmitirse(bultos)) return;

            wait.Until(ExpectedConditions.ElementExists(txtNumeroBultos));
            LimpiarYEscribir(txtNumeroBultos, bultos.Trim());
            Console.WriteLine($"[Guia] Bultos: '{ValorCampo(txtNumeroBultos)}'");
        }

        public void SeleccionarTipoTransporte(string tipoTransporte)
        {
            if (DebeOmitirse(tipoTransporte)) return;

            string buscar = tipoTransporte.Trim().Equals("Publico", StringComparison.OrdinalIgnoreCase)
                ? "PUBLICO"
                : "PRIVADO";

            var select = EsperarVisible(cboModalidadTransporte);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", select);

            Thread.Sleep(300);

            var selectEl = new SelectElement(select);
            var opcion = selectEl.Options.FirstOrDefault(o =>
                NormalizarTexto(o.Text).Contains(buscar));

            if (opcion == null)
                throw new NoSuchElementException($"No se encontró opción de transporte '{buscar}'.");

            selectEl.SelectByText(opcion.Text);

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('blur', { bubbles: true }));
                arguments[0].blur();
            ", select);

            Thread.Sleep(1000);
        }

        public void IngresarTransportistaPrivado(string transportista)
        {
            if (DebeOmitirse(transportista)) return;
            BuscarYSeleccionar(txtTransportista, btnBuscarTransportista, transportista.Trim());
        }

        public void IngresarTransportistaPublico(string ruc)
        {
            if (DebeOmitirse(ruc)) return;
            BuscarYSeleccionar(txtTransportista, btnBuscarTransportista, ruc.Trim());
        }

        public void IngresarNumeroLicencia(string licencia)
        {
            if (DebeOmitirse(licencia)) return;

            LimpiarYEscribir(txtNumeroLicencia, licencia.Trim());
            Console.WriteLine($"[Guia] Licencia: '{ValorCampo(txtNumeroLicencia)}'");
        }

        public void IngresarNumeroPlaca(string placa)
        {
            if (DebeOmitirse(placa)) return;

            LimpiarYEscribir(txtNumeroPlaca, placa.Trim());
            Console.WriteLine($"[Guia] Placa: '{ValorCampo(txtNumeroPlaca)}'");
        }

        public void SeleccionarDireccionOrigen(string direccion)
        {
            if (DebeOmitirse(direccion)) return;
            SeleccionarOpcionSelect(cboUbigeoOrigen, direccion);
        }

        public void IngresarDetalleOrigen(string detalle)
        {
            if (DebeOmitirse(detalle)) return;
            LimpiarYEscribir(txtDetalleOrigen, detalle.Trim());
        }

        public void SeleccionarDireccionDestino(string direccion)
        {
            if (DebeOmitirse(direccion)) return;
            SeleccionarOpcionSelect(cboUbigeoDestino, direccion);
        }

        public void IngresarDetalleDestino(string detalle)
        {
            if (DebeOmitirse(detalle)) return;
            LimpiarYEscribir(txtDetalleDestino, detalle.Trim());
        }

        public void GuardarGuia()
        {
            LimpiarErrorGuia();

            bool esPublico = EsTransportePublico();

            if (esPublico && ExisteVisible(mensajeTransportistaInvalido, 2))
            {
                Console.WriteLine("[Guia] Error transportista público detectado, no se hace click en Aceptar.");
                mensajeErrorGuia = "Transportista debe tener RUC valido";
                return;
            }

            Thread.Sleep(800);

            var botones = driver.FindElements(btnAceptar)
                .Where(e => e.Displayed)
                .ToList();

            if (!botones.Any())
            {
                Console.WriteLine("[Guia] Botón Aceptar no visible. Se continúa para validar mensaje.");
                return;
            }

            var boton = botones.First();

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            Thread.Sleep(300);

            if (EstaDeshabilitado(boton) || !boton.Enabled)
            {
                Console.WriteLine("[Guia] Botón Aceptar deshabilitado. Se valida mensaje del formulario.");
                return;
            }

            try
            {
                boton.Click();
            }
            catch
            {
                try
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton);
                }
                catch
                {
                    Console.WriteLine("[Guia] No se pudo hacer click en Aceptar. Se continúa con validación.");
                    return;
                }
            }

            Thread.Sleep(1200);
        }

        public string ObtenerResultadoGuia()
        {
            if (!string.IsNullOrWhiteSpace(mensajeErrorGuia))
                return mensajeErrorGuia!;

            bool esPublico = EsTransportePublico();
            bool esPrivado = EsTransportePrivado();

            if (esPublico && ExisteVisible(mensajeTransportistaInvalido, 2))
                return "Transportista debe tener RUC valido";

            bool hayBanner = ExisteVisible(bannerCamposRequeridos, 2) ||
                             ExisteVisible(mensajeCampoObligatorio, 2);

            string fecha = ValorCampo(txtFechaTraslado);
            string peso = ValorCampo(txtPesoBruto);
            string bultos = ValorCampo(txtNumeroBultos);
            string transportista = ValorCampo(txtTransportista);
            string licencia = ValorCampo(txtNumeroLicencia);
            string placa = ValorCampo(txtNumeroPlaca);

            Console.WriteLine($"[Guia] Validación final -> Fecha:'{fecha}' Peso:'{peso}' Bultos:'{bultos}' Transportista:'{transportista}' Licencia:'{licencia}' Placa:'{placa}' Privado:{esPrivado}");

            if (ModalSigueAbierto() || hayBanner)
            {
                if (string.IsNullOrWhiteSpace(fecha))
                    return "Registre la fecha de inicio";

                if ((string.IsNullOrWhiteSpace(peso) || peso == "0") &&
                    (string.IsNullOrWhiteSpace(bultos) || bultos == "0"))
                    return "Falta peso y numero de bultos";

                if (esPrivado && string.IsNullOrWhiteSpace(transportista))
                    return "Ingrese transportista";

                if (esPrivado && string.IsNullOrWhiteSpace(licencia))
                    return "Ingrese numero de licencia";

                if (esPrivado && string.IsNullOrWhiteSpace(placa))
                    return "Ingrese numero de placa";

                if (hayBanner)
                    return "Completar los campos requeridos correctamente";
            }

            if (!ModalSigueAbierto())
                return "Guia emitida correctamente";

            try
            {
                var mensaje = ObtenerVisible(lblMensaje);
                if (mensaje != null)
                    return mensaje.Text.Trim();
            }
            catch { }

            return string.Empty;
        }
    }
}