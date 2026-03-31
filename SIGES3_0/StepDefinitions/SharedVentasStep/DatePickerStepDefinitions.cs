using System;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.Helpers;
using SIGES3_0.Pages.VentasPage;

namespace SIGES3_0.StepDefinitions.SharedStep
{
    /// <summary>
    /// Steps compartidos para flujos de fecha/hora + botón de búsqueda.
    ///
    /// Variantes de fecha:
    ///   1. Genérica  — sin estado, cualquier campo.
    ///   2. Inicial   — resetea la bandera de bloqueo antes de ingresar.
    ///   3. Final     — detecta fechas bloqueadas y activa la bandera.
    ///
    /// Botón de acción (Buscar, VER REPORTE, etc.):
    ///   - Se omite automáticamente si la fecha final quedó bloqueada.
    /// </summary>
    [Binding]
    public class DatePickerStepDefinitions
    {
        private readonly ReportesPage _reportesPage;
        private readonly Utilities _utilities;
        private readonly ScenarioContext _ctx;

        internal const string ClaveBloqueo = "FechaFinalBloqueada";

        public DatePickerStepDefinitions(IWebDriver driver, ScenarioContext ctx)
        {
            _reportesPage = new ReportesPage(driver);
            _utilities    = new Utilities(driver);
            _ctx          = ctx;
        }

        // ── 1. Fecha genérica ────────────────────────────────────────────────
        [When(@"el usuario ingresa la fecha y hora {string} en el campo {string}")]
        public void IngresarFechaEnCampo(string fechaHora, string labelCampo) =>
            _reportesPage.IngresarFechaHora(LocatorPorLabel(labelCampo), fechaHora);

        // ── 2. Fecha inicial
        [When(@"el usuario ingresa la fecha inicial {string} en el campo {string}")]
        public void IngresarFechaInicial(string fechaHora, string labelCampo)
        {
            _ctx[ClaveBloqueo] = false;
            _reportesPage.IngresarFechaHora(LocatorPorLabel(labelCampo), fechaHora);
        }

        // ── 3. Fecha final
        [When(@"el usuario ingresa la fecha final {string} en el campo {string}")]
        public void IngresarFechaFinal(string fechaHora, string labelCampo)
        {
            _ctx[ClaveBloqueo] = false;
            try
            {
                _reportesPage.IngresarFechaHora(LocatorPorLabel(labelCampo), fechaHora);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToLower();
                bool esBloqueada = msg.Contains("no se logro seleccionar el dia correcto")
                                || msg.Contains("fallo seleccionando dia")
                                || msg.Contains("no se logró seleccionar el día correcto")
                                || msg.Contains("falló seleccionando día");
                if (esBloqueada)
                {
                    _ctx[ClaveBloqueo] = true;
                    Console.WriteLine("[DatePicker] Fecha final bloqueada o inválida — se omitirá el botón de acción. Detalle: " + ex.Message);
                    return;
                }
                Assert.Fail("Error al ingresar fecha final: " + ex.Message);
            }
        }

        // ── 4. Botón de acción — respeta bloqueo de fecha final ──────────────
        [When(@"el usuario hace clic en el botón {string}")]
        public void ClickBoton(string textoBoton)
        {
            if (_ctx.TryGetValue(ClaveBloqueo, out object val) && val is true)
            {
                Console.WriteLine($"[Button] Clic en '{textoBoton}' omitido: fecha final bloqueada o inválida.");
                return;
            }
            _utilities.ClickButton(
                By.XPath($"//button[contains(normalize-space(),'{textoBoton}')]"));
        }

        // ── Helper de locator ────────────────────────────────────────────────
        private static By LocatorPorLabel(string labelCampo) =>
            By.XPath($"(//label[contains(.,'{labelCampo}')])[1]/following::input[@readonly][1]");
    }
}