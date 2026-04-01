using System;
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

        // Modal
        private readonly By modalGuiaRemision = By.XPath("//span[normalize-space()='Guia de remisión']");

        // Botones
        private readonly By btnAceptar = By.XPath("//button[normalize-space()='ACEPTAR']");
        private readonly By btnCancelar = By.XPath("//button[normalize-space()='CANCELAR']");

        // DATOS GENERALES — siempre visible, no necesita expandirse
        private readonly By txtDestinatario = By.XPath("(//input[@placeholder='Buscar...'])[1]");
        private readonly By btnBuscarDestinatario = By.XPath("(//div[contains(@class, 'mb-3')]//input[@placeholder='Buscar...']");
        private readonly By txtFechaTraslado = By.XPath("//input[@type='date']");
        private readonly By txtPesoBruto = By.XPath("//div[contains(@class,'row') and contains(@class,'mb-3')]//input[@type='text']");
        private readonly By txtNumeroBultos = By.XPath("//div[contains(@class,'card-body')]//input[@type='text']");

        // MODALIDAD DE TRANSPORTE
        private readonly By cboModalidadTransporte = By.XPath("//div[contains(@class,'col-md-4')]//select[contains(@class,'form-select')]");

        // DATOS DE TRANSPORTE — Transportista público
        private readonly By txtTransportista = By.XPath("//div[contains(@class,'mb-1')]//input[@placeholder='Buscar...']");
        private readonly By btnBuscarTransportista = By.XPath("(//button[.//i[contains(@class,'bi-search')]])[2]");

        // DATOS DE TRANSPORTE — Conductor privado
        private readonly By txtDniConductor = By.XPath("//input[contains(@placeholder,'DNI') or contains(@formcontrolname,'dni')]");
        private readonly By btnBuscarConductor = By.XPath("(//button[.//i[contains(@class,'bi-search')]])[3]");
        private readonly By txtNumeroLicencia = By.XPath("//input[contains(@placeholder,'licencia') or contains(@formcontrolname,'licencia')]");
        private readonly By txtNumeroPlaca = By.XPath("//input[contains(@placeholder,'placa') or contains(@formcontrolname,'placa')]");

        // DIRECCIÓN ORIGEN / DESTINO — dropdowns UBIGEO
        private readonly By cboUbigeoOrigen = By.XPath("//app-address-form[@title='DIRECCIÓN ORIGEN']//select[@class='form-select form-select-sm bg-light text-secondary']");
        private readonly By cboUbigeoDestino = By.XPath("//app-address-form[@title='DIRECCIÓN DESTINO']//select[@class='form-select form-select-sm bg-light text-secondary']");

        // Mensajes
        private readonly By lblMensaje = By.XPath("//*[contains(@class,'toast') or contains(@class,'alert') or contains(@class,'p-toast-detail')]");

        // =========================
        // HELPERS
        // =========================

        private IWebElement EsperarVisible(By locator)
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        private IWebElement EsperarClickeable(By locator)
        {
            return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        private bool DebeOmitirse(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ||
                   string.Equals(valor.Trim(), "NA", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(valor.Trim(), "Ninguno", StringComparison.OrdinalIgnoreCase);
        }

        private void LimpiarYEscribir(By locator, string valor)
        {
            var el = EsperarVisible(locator);
            el.Click();
            el.SendKeys(Keys.Control + "a");
            el.SendKeys(Keys.Delete);
            el.SendKeys(valor);
        }

        private void SeleccionarOpcionSelect(By locator, string texto)
        {
            var select = EsperarVisible(locator);
            var selectEl = new OpenQA.Selenium.Support.UI.SelectElement(select);
            try
            {
                selectEl.SelectByText(texto);
            }
            catch
            {
                // Fallback: buscar opción que contenga el texto
                foreach (var option in selectEl.Options)
                {
                    if (option.Text.Contains(texto, StringComparison.OrdinalIgnoreCase))
                    {
                        selectEl.SelectByText(option.Text);
                        return;
                    }
                }
            }
        }

        private void BuscarYSeleccionar(By txtLocator, By btnLocator, string valor)
        {
            LimpiarYEscribir(txtLocator, valor);
            Thread.Sleep(500);
            EsperarClickeable(btnLocator).Click();
            Thread.Sleep(1000);
        }

        // =========================
        // ACCIONES
        // =========================

        public void EsperarModalGuia()
        {
            EsperarVisible(modalGuiaRemision);
        }

        // La interfaz nueva no tiene acordeones — datos siempre visibles
        public void ExpandirDatosGenerales()
        {
            // Ya no necesita expandirse, pero se mantiene por compatibilidad
            EsperarVisible(txtFechaTraslado);
        }

        public void ExpandirDatosTransporte()
        {
            // Ya no necesita expandirse, pero se mantiene por compatibilidad
            EsperarVisible(cboModalidadTransporte);
        }

        public void ValidarDestinatarioAutocompletado()
        {
            var valor = EsperarVisible(txtDestinatario).GetAttribute("value")?.Trim() ?? "";
            Assert.IsFalse(string.IsNullOrWhiteSpace(valor), "El destinatario no fue autocompletado.");
        }

        // =========================
        // DATOS GENERALES
        // =========================

        public void IngresarFechaTraslado(string fecha)
        {
            if (DebeOmitirse(fecha)) return;

            var input = EsperarVisible(txtFechaTraslado);
            input.Click();
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);

            string fechaFormateada = string.Equals(fecha.Trim(), "Hoy", StringComparison.OrdinalIgnoreCase)
                ? DateTime.Now.ToString("dd/MM/yyyy")
                : fecha;

            input.SendKeys(fechaFormateada);
            input.SendKeys(Keys.Tab);
        }

        public void IngresarPesoBruto(string peso)
        {
            if (DebeOmitirse(peso)) return;
            LimpiarYEscribir(txtPesoBruto, peso);
        }

        public void IngresarNumeroBultos(string bultos)
        {
            if (DebeOmitirse(bultos)) return;
            LimpiarYEscribir(txtNumeroBultos, bultos);
        }

        // =========================
        // DATOS DE TRANSPORTE
        // =========================

        public void SeleccionarTipoTransporte(string tipoTransporte)
        {
            if (DebeOmitirse(tipoTransporte)) return;

            // La modalidad se selecciona en el dropdown de MODALIDAD DE TRANSPORTE
            // "Publico" → "TRANSPORTE PÚBLICO", "Privado" → "TRANSPORTE PRIVADO"
            string opcion = tipoTransporte.Trim().Equals("Publico", StringComparison.OrdinalIgnoreCase)
                ? "TRANSPORTE PÚBLICO"
                : "TRANSPORTE PRIVADO";

            SeleccionarOpcionSelect(cboModalidadTransporte, opcion);
            Thread.Sleep(500);
        }

        public void IngresarTransportistaPublico(string ruc)
        {
            if (DebeOmitirse(ruc)) return;
            BuscarYSeleccionar(txtTransportista, btnBuscarTransportista, ruc);
        }

        public void IngresarConductorPrivado(string dni)
        {
            if (DebeOmitirse(dni)) return;
            BuscarYSeleccionar(txtDniConductor, btnBuscarConductor, dni);
        }

        public void IngresarNumeroLicencia(string licencia)
        {
            if (DebeOmitirse(licencia)) return;
            LimpiarYEscribir(txtNumeroLicencia, licencia);
        }

        public void IngresarNumeroPlaca(string placa)
        {
            if (DebeOmitirse(placa)) return;
            LimpiarYEscribir(txtNumeroPlaca, placa);
        }

        public void SeleccionarDireccionOrigen(string direccion)
        {
            if (DebeOmitirse(direccion)) return;
            // El formato es "Departamento-Provincia-Distrito"
            // Seleccionar por texto que contenga el valor
            SeleccionarOpcionSelect(cboUbigeoOrigen, direccion);
        }

        public void SeleccionarDireccionDestino(string direccion)
        {
            if (DebeOmitirse(direccion)) return;
            SeleccionarOpcionSelect(cboUbigeoDestino, direccion);
        }

        // =========================
        // GUARDAR / VALIDAR
        // =========================

        public void GuardarGuia()
        {
            // Botón cambió de "Guardar" a "ACEPTAR"
            EsperarClickeable(btnAceptar).Click();
            Thread.Sleep(1000);
        }

        public void ValidarResultado(string resultadoEsperado)
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            try
            {
                var mensaje = waitLong.Until(d =>
                {
                    try
                    {
                        var el = d.FindElement(lblMensaje);
                        return el.Displayed ? el : null;
                    }
                    catch { return null; }
                });

                Assert.IsTrue(
                    mensaje.Text.Contains(resultadoEsperado, StringComparison.OrdinalIgnoreCase),
                    $"Esperado: '{resultadoEsperado}'. Obtenido: '{mensaje.Text}'"
                );
            }
            catch
            {
                Assert.IsTrue(
                    driver.PageSource.Contains(resultadoEsperado),
                    $"No se encontró el resultado esperado: {resultadoEsperado}"
                );
            }
        }
    }
}