using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.StepDefinitions.SharedVentasStep;
using SIGES3_0.Pages.VentasPage;
using System;

namespace SIGES3_0.StepDefinitions.VentasStep
{
    [Binding]
    [Scope(Tag = "Reportes")]
    public class ReportesStepDefinitions
    {
        private readonly ReportesPage _reportesPage;
        private readonly ScenarioContext _scenarioContext;

        public ReportesStepDefinitions(IWebDriver driver, ScenarioContext scenarioContext)
        {
            _reportesPage = new ReportesPage(driver);
            _scenarioContext = scenarioContext;
        }

        [When("selecciona la vista {string}")]
        public void WhenSeleccionaLaVista(string tabName)
        {
            _reportesPage.SeleccionarVista(tabName);
        }

        [When("hace clic en {string} en la tarjeta {string}")]
        public void WhenHaceClicEnEnLaTarjeta(string btn, string tarjeta)
        {
            _reportesPage.ClickVerReporte(tarjeta);
        }

        [When("selecciona el tipo de comprobante {string}")]
        public void WhenSeleccionaElTipoDeComprobante(string tipoComprobante)
        {
            _reportesPage.SeleccionarTipoComprobante(tipoComprobante);
        }

        [When("selecciona la serie {string}")]
        public void WhenSeleccionaLaSerie(string serie)
        {
            _reportesPage.SeleccionarSerie(serie);
        }

        [When("selecciona el comprobante y serie {string}")]
        public void WhenSeleccionaElComprobanteSerie(string valor)
        {
            _reportesPage.SeleccionarComprobanteSerie(valor);
        }

        [When("selecciona el punto de venta {string}")]
        public void WhenSeleccionaElPuntoDeVenta(string puntoVenta)
        {
            _reportesPage.SeleccionarPuntoVenta(puntoVenta);
        }

        [When("selecciona la familia {string}")]
        public void WhenSeleccionaLaFamilia(string familia)
        {
            _reportesPage.SeleccionarFamilia(familia);
        }

        [When("selecciona la característica {string} en la tarjeta {string}")]
        public void WhenSeleccionaLaCaracteristicaEnLaTarjeta(string caracteristica, string tarjeta)
        {
            _reportesPage.SeleccionarCaracteristica(caracteristica, tarjeta);
        }

        [When("selecciona el vendedor {string}")]
        public void WhenSeleccionaElVendedor(string vendedor)
        {
            _reportesPage.SeleccionarVendedor(vendedor);
        }

        [When("selecciona {string} en el filtro {string} de la tarjeta {string}")]
        public void WhenSeleccionaEnElFiltroDeLaTarjeta(string valor, string filtro, string tarjeta)
        {
            _reportesPage.SeleccionarFiltroEnTarjeta(valor, filtro, tarjeta);
        }

        [When("selecciona el establecimiento {string}")]
        public void WhenSeleccionaElEstablecimiento(string establecimiento)
        {
            _reportesPage.SeleccionarEstablecimiento(establecimiento);
        }

        [Then("el sistema muestra el resultado esperado del reporte de ventas {string}")]
        public void ThenElSistemaMuestraElResultadoEsperadoDelReporte(string resultadoEsperado)
        {
            _reportesPage.ValidarResultadoReporte(resultadoEsperado);
        }

        [Then("el sistema valida el comportamiento esperado de la fecha final {string}")]
        public void ThenElSistemaValidaElComportamientoEsperadoDeLaFechaFinal(string resultadoEsperado)
        {
            string valorAntes = _scenarioContext.TryGetValue(DatePickerStepDefinitions.FechaFinalValorAntesKey, out string? antes)
                ? antes ?? string.Empty
                : string.Empty;
            string valorDespues = _scenarioContext.TryGetValue(DatePickerStepDefinitions.FechaFinalValorDespuesKey, out string? despues)
                ? despues ?? string.Empty
                : string.Empty;

            if (resultadoEsperado.Trim().Equals("No permite aplicar el filtro Inhabilitado", StringComparison.OrdinalIgnoreCase))
            {
                Assert.AreEqual(
                    valorAntes,
                    valorDespues,
                    $"La fecha final invalida no deberia aplicarse al campo. Antes: '{valorAntes}' / Despues: '{valorDespues}'.");
                return;
            }

            Assert.AreNotEqual(
                valorAntes,
                valorDespues,
                $"Se esperaba que la fecha final valida si se aplicara al campo. Antes: '{valorAntes}' / Despues: '{valorDespues}'.");
        }

        [Then("el sistema genera el reporte exitosamente")]
        public void ThenElSistemaGeneraElReporteExitosamente()
        {
            Assert.IsTrue(_reportesPage.VerificarReporteGenerado(), "El reporte no se genero exitosamente.");
        }
    }
}
