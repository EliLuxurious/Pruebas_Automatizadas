using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SIGES3_0.Pages.Helpers;
using System;
using System.Linq;
using System.Threading;

namespace SIGES3_0.Pages.VentasPage
{
    public class ReportesPage
    {
        private readonly IWebDriver _driver;
        private readonly Utilities _utilities;
        private readonly WebDriverWait _wait;

        public ReportesPage(IWebDriver driver)
        {
            _driver = driver;
            _utilities = new Utilities(driver);
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        public void SeleccionarVista(string tabName)
        {
            var tabLocator = VentasLocators.Reportes.TabDinamico(tabName);
            try
            {
                _utilities.ClickButton(tabLocator);
            }
            catch (NoSuchElementException)
            {
                // Fallbacks seguros en caso de que el dinámico no lo encuentre directamente
                switch (tabName.Trim().ToLower())
                {
                    case "comprobantes": _utilities.ClickButton(VentasLocators.Reportes.TabComprobantes); break;
                    case "series":       _utilities.ClickButton(VentasLocators.Reportes.TabSeries); break;
                    case "conceptos":    _utilities.ClickButton(VentasLocators.Reportes.TabConceptos); break;
                    case "vendedor":     _utilities.ClickButton(VentasLocators.Reportes.TabVendedor); break;
                    case "grupos":       _utilities.ClickButton(VentasLocators.Reportes.TabGrupos); break;
                    case "excepciones":  _utilities.ClickButton(VentasLocators.Reportes.TabExcepciones); break;
                    default: throw new Exception($"La vista/tab '{tabName}' no existe en Reportes.");
                }
            }
            Thread.Sleep(1000);
        }

        // ── Tab: Comprobantes ────────────────────────────────────────────────────
        public void SeleccionarTipoComprobante(string tipoComprobante)
        {
            _utilities.ClickButton(VentasLocators.Reportes.TipoComprobanteDropdown);
            _utilities.ClearAndEnterText(VentasLocators.Reportes.TipoComprobanteSearch, tipoComprobante);
            _utilities.ClickButton(VentasLocators.Reportes.TipoComprobanteOption(tipoComprobante));
            Thread.Sleep(1000);
        }

        public void SeleccionarSerie(string serie)
        {
            _utilities.ClickButton(VentasLocators.Reportes.SerieDropdown);
            _utilities.ClearAndEnterText(VentasLocators.Reportes.SerieSearch, serie);
            _utilities.ClickButton(VentasLocators.Reportes.SerieOption(serie));
        }

        // ── Tab: Series ──────────────────────────────────────────────────────────
        // Dropdown "Comprobante y Serie" vive DENTRO de la tarjeta POR SERIE.
        // Formato: "Todos" | "XX : YYYY"  (ej: "01 : F002", "03 : B002")
        public void SeleccionarComprobanteSerie(string valor)
        {
            if (valor.Trim().Equals("Todos", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("[ComprobanteSerie] 'Todos' ya está preseleccionado, se omite.");
                return;
            }
            _utilities.ClickButton(VentasLocators.Reportes.ComprobanteSerieDropdown);
            _utilities.ClearAndEnterText(VentasLocators.Reportes.ComprobanteSerieSearch, valor);
            _utilities.ClickButton(VentasLocators.Reportes.ComprobanteSerieOpcion(valor));
        }

        // ── Tab: Conceptos ───────────────────────────────────────────────────────
        public void SeleccionarPuntoVenta(string puntoVenta)
        {
            if (_utilities.IsVisible(VentasLocators.Reportes.PuntoVentaChip(puntoVenta)))
                return;

            _utilities.ClickButton(VentasLocators.Reportes.PuntoVentaDropdown);
            _utilities.ClearAndEnterText(VentasLocators.Reportes.PuntoVentaSearch, puntoVenta);
            _utilities.ClickButton(VentasLocators.Reportes.PuntoVentaOption(puntoVenta));
            _wait.Until(d => _utilities.IsVisible(VentasLocators.Reportes.PuntoVentaChip(puntoVenta)));
        }

        public void SeleccionarFamilia(string familia)
        {
            _utilities.ClickButton(VentasLocators.Reportes.FamiliaDropdown);
            _utilities.ClearAndEnterText(VentasLocators.Reportes.FamiliaSearch, familia);
            _utilities.ClickButton(VentasLocators.Reportes.FamiliaOption(familia));
        }

        public void SeleccionarCaracteristica(string caracteristica, string tarjeta)
        {
            _utilities.ClickButton(VentasLocators.Reportes.CaracteristicaDropdown(tarjeta));
            _utilities.ClearAndEnterText(VentasLocators.Reportes.CaracteristicaSearch(tarjeta), caracteristica);
            _utilities.ClickButton(VentasLocators.Reportes.CaracteristicaOpcion(caracteristica));
        }

        public void ValidarResultadoReporte(string resultadoEsperado)
        {
            switch (resultadoEsperado.Trim().ToLower())
            {
                case "aplica el filtro correctamente":
                    Assert.IsTrue(VerificarReporteGenerado(), "Se esperaba que el reporte se generara correctamente.");
                    break;
                case "no permite aplicar el filtro inhabilitado":
                    Assert.IsFalse(VerificarReporteGenerado(), "Se esperaba que el sistema no generara el reporte con rango de fechas inválido.");
                    break;
                default:
                    Assert.Fail("Resultado esperado no reconocido: " + resultadoEsperado);
                    break;
            }
        }

        // ── Acción compartida por todos los tabs ─────────────────────────────────
        public void ClickVerReporte(string tarjeta)
        {
            try
            {
                _utilities.ClickButton(VentasLocators.Reportes.VerReporteEnTarjeta(tarjeta));
                Thread.Sleep(2000);
            }
            catch
            {
                Console.WriteLine($"[ClickVerReporte] Botón '{tarjeta}' no fue clickeable. Se evalúa en la validación.");
            }
        }

        // ── Verificaciones ───────────────────────────────────────────────────────
        public bool VerificarReporteGenerado()
        {
            try
            {
                var waitReporte = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
                var elemento = waitReporte.Until(d =>
                    d.FindElements(VentasLocators.Reportes.HeaderReporteResultado)
                     .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } })
                );
                return elemento != null;
            }
            catch
            {
                return false;
            }
        }

        public bool VerificarBotonHabilitado(string tarjeta)
        {
            var btn = _driver.FindElements(VentasLocators.Reportes.VerReporteEnTarjeta(tarjeta))
                             .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
            if (btn == null) return false;
            return btn.Enabled && btn.GetAttribute("disabled") == null;
        }
    }
}
