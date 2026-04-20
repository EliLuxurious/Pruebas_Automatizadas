using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.CotizacionPage;
using SIGES3_0.Pages.PedidoPage;
using System;
using System.Linq;

namespace SIGES3_0.StepDefinitions.CotizacionStep
{
    [Binding]
    public class CotizacionStep
    {
        private readonly IWebDriver driver;
        private readonly CotizacionPage cotizacionPage;
        private readonly VerPedidosPage verPedidosPage;

        public CotizacionStep(IWebDriver driver)
        {
            this.driver = driver;
            cotizacionPage = new CotizacionPage(driver);
            verPedidosPage = new VerPedidosPage(driver);
        }

        [Given(@"existe una cotizacion editable")]
        public void GivenExisteUnaCotizacionEditable()
        {
            if (!cotizacionPage.ExisteCotizacionParaEditar())
            {
                verPedidosPage.SeleccionarOpcion("Nueva Cotización");
                verPedidosPage.SeleccionarFamilia("Gaseosa");
                verPedidosPage.SeleccionarConcepto("7753234003320");
                verPedidosPage.IngresarCantidad("10");
                verPedidosPage.ActivarIGV("false");
                verPedidosPage.ActivarDetUnif("false");
                verPedidosPage.ConfigurarDescuento("false", "NA", "NA", "0");
                cotizacionPage.BuscarCliente("00000000");
                cotizacionPage.IngresarFechaFinal("30/04/2026 12:00:am");
                cotizacionPage.RegistrarCotizacion();
                cotizacionPage.ObtenerResultadoSistema();
            }

            cotizacionPage.AsegurarCotizacionEditable();
        }

        [When(@"el usuario selecciona editar la cotizacion")]
        public void WhenElUsuarioSeleccionaEditarLaCotizacion()
        {
            cotizacionPage.SeleccionarEditarCotizacion();
        }

        [When(@"el usuario busca el cliente cotizacion '(.*)'")]
        public void WhenElUsuarioBuscaElClienteCotizacion(string cliente)
        {
            if (cliente == "NO_CAMBIO")
            {
                Console.WriteLine("[Cotizacion] Cliente = NO_CAMBIO, no se modifica.");
                return;
            }

            cotizacionPage.BuscarCliente(cliente);
        }

        [When(@"el usuario ingresa la fecha final '(.*)'")]
        public void WhenElUsuarioIngresaLaFechaFinal(string fecha)
        {
            if (fecha == "NO_CAMBIO")
            {
                Console.WriteLine("[Cotizacion] Fecha final = NO_CAMBIO, no se modifica.");
                return;
            }

            cotizacionPage.IngresarFechaFinal(fecha);
        }

        [When(@"el usuario registra la cotizacion")]
        public void WhenElUsuarioRegistraLaCotizacion()
        {
            if (cotizacionPage.FechaPasadaIntentada)
            {
                Console.WriteLine("[RegistrarCotizacion] Fecha pasada detectada, se omite el registro.");
                return;
            }

            cotizacionPage.RegistrarCotizacion();
        }

        [When(@"el usuario actualiza la cotizacion")]
        public void WhenElUsuarioActualizaLaCotizacion()
        {
            cotizacionPage.ActualizarCotizacion();
        }

        [Then(@"el sistema valida el resultado de la cotizacion '(.*)'")]
        public void ThenElSistemaValidaElResultadoDeLaCotizacion(string resultadoEsperado)
        {
            string resultado = cotizacionPage.ObtenerResultadoSistema()?.Trim() ?? string.Empty;
            string esperado = resultadoEsperado?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(resultado))
            {
                try
                {
                    var badge = driver.FindElements(By.XPath("//span[contains(@class,'badge-status') and contains(@class,'danger')]"))
                                      .FirstOrDefault(e => e.Displayed);

                    if (badge != null)
                        resultado = badge.Text?.Trim() ?? string.Empty;
                }
                catch
                {
                }
            }

            Assert.That(resultado, Is.Not.Empty,
                "El sistema no devolvió ningún mensaje o resultado visible.");

            string resultadoNormalizado = NormalizarTexto(resultado);
            string esperadoNormalizado = NormalizarTexto(esperado);

            Assert.That(resultadoNormalizado, Does.Contain(esperadoNormalizado),
                $"Resultado esperado: {resultadoEsperado}. Resultado obtenido: {resultado}");
        }

        private static string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            texto = QuitarTildes(texto);
            texto = System.Text.RegularExpressions.Regex.Replace(texto, @"\s+", " ");

            return texto.Trim().ToLower();
        }

        private static string QuitarTildes(string texto)
        {
            var normalized = texto.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();

            foreach (var c in normalized)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }
    }
}