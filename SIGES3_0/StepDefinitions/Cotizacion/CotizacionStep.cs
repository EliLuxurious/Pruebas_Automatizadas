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
        private readonly CotizacionPage cotizacionPage;
        private readonly VerPedidosPage verPedidosPage;

        public CotizacionStep(IWebDriver driver)
        {
            cotizacionPage = new CotizacionPage(driver);
            verPedidosPage = new VerPedidosPage(driver);
        }

        [Given(@"existe una cotizacion editable con familia '(.*)' concepto '(.*)' cantidad '(.*)' cliente '(.*)' fecha '(.*)'")]
        public void GivenExisteUnaCotizacionEditable(string familia, string concepto, string cantidad, string cliente, string fecha)
        {
            if (!cotizacionPage.ExisteCotizacionParaEditar())
            {
                // Ahora usamos las variables que vienen del Feature
                verPedidosPage.SeleccionarOpcion("Nueva Cotización");
                verPedidosPage.SeleccionarFamilia(familia);
                verPedidosPage.SeleccionarConcepto(concepto);
                verPedidosPage.IngresarCantidad(cantidad);
                verPedidosPage.ActivarIGV("false");
                verPedidosPage.ActivarDetUnif("false");
                verPedidosPage.ConfigurarDescuento("false", "NA", "NA", "0");

                verPedidosPage.BuscarCliente(cliente);

                cotizacionPage.IngresarFechaFinal(fecha);
                cotizacionPage.RegistrarCotizacion();
                cotizacionPage.ObtenerResultadoSistema(); // Limpia la variable de resultado
            }
            cotizacionPage.AsegurarCotizacionEditable();
        }

        [When(@"el usuario selecciona editar la cotizacion")]
        public void WhenElUsuarioSeleccionaEditarLaCotizacion() => cotizacionPage.SeleccionarEditarCotizacion();

        [When(@"el usuario ingresa la fecha final '(.*)'")]
        public void WhenElUsuarioIngresaLaFechaFinal(string fecha) => cotizacionPage.IngresarFechaFinal(fecha);

        [When(@"el usuario registra la cotizacion")]
        public void WhenElUsuarioRegistraLaCotizacion()
        {
            if (cotizacionPage.FechaPasadaIntentada) return;
            cotizacionPage.RegistrarCotizacion();
        }

        [When(@"el usuario actualiza la cotizacion")]
        public void WhenElUsuarioActualizaLaCotizacion() => cotizacionPage.ActualizarCotizacion();

        [Then(@"el sistema valida el resultado de la cotizacion '(.*)'")]
        public void ThenElSistemaValidaElResultadoDeLaCotizacion(string resultadoEsperado)
        {
            string resultado = cotizacionPage.ObtenerResultadoSistema()?.Trim() ?? string.Empty;
            string esperado = resultadoEsperado?.Trim() ?? string.Empty;

            Assert.That(resultado, Is.Not.Empty, "El sistema no devolvió ningún mensaje o resultado visible.");

            string resultadoNormalizado = NormalizarTexto(resultado);
            string esperadoNormalizado = NormalizarTexto(esperado);

            Assert.That(resultadoNormalizado, Does.Contain(esperadoNormalizado),
                $"Resultado esperado: {resultadoEsperado}. Resultado obtenido: {resultado}");
        }

        private static string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return string.Empty;

            var normalized = texto.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();

            foreach (var c in normalized)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            string sinTildes = sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
            return System.Text.RegularExpressions.Regex.Replace(sinTildes, @"\s+", " ").Trim().ToLower();
        }
    }
}