using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SIGES3_0.Pages.Helpers;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            try
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(VentasLocators.Reportes.VistaReporte(tabName.Trim().ToLower()))).Click();
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception($"No se encontró la vista '{tabName}' en Reportes. Valores válidos: comprobantes, series, conceptos, vendedor, grupos, excepciones.");
            }
            Thread.Sleep(1000);
        }

        // ── Tab: Comprobantes ────────────────────────────────────────────────────
        public void SeleccionarTipoComprobante(string tipoComprobante)
        {
            _utilities.ClickButton(VentasLocators.Reportes.TipoComprobanteDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.TipoComprobanteOption(tipoComprobante));
            Thread.Sleep(1000);
        }

        public void SeleccionarSerie(string serie)
        {
            _utilities.ClickButton(VentasLocators.Reportes.SerieDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.SerieOption(serie));
        }

        // ── Tab: Series ──────────────────────────────────────────────────────────
        // Dropdown "Comprobante y Serie" vive DENTRO de la tarjeta POR SERIE.
        // Formato: "Todos" | "XX : YYYY"  (ej: "01 : F002", "03 : B002")
        public void SeleccionarComprobanteSerie(string valor)
        {
            _utilities.ClickButton(VentasLocators.Reportes.ComprobanteSerieDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.ComprobanteSerieOpcion(valor));
        }

        // ── Tab: Conceptos ───────────────────────────────────────────────────────
        public void SeleccionarPuntoVenta(string puntoVenta)
        {
            // Si ya está como chip seleccionado, omitir (evita deseleccionar por comportamiento toggle)
            var chips = _driver.FindElements(VentasLocators.Reportes.PuntoVentaChip(puntoVenta));
            if (chips.Any(e => { try { return e.Displayed; } catch { return false; } }))
            {
                Thread.Sleep(500);
                return;
            }

            _utilities.ClickButton(VentasLocators.Reportes.PuntoVentaDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.PuntoVentaOption(puntoVenta));
            Thread.Sleep(500);
        }

        public void SeleccionarFamilia(string familia)
        {
            _utilities.ClickButton(VentasLocators.Reportes.FamiliaDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.FamiliaOption(familia));
        }

        public void SeleccionarCaracteristica(string caracteristica, string tarjeta)
        {
            _utilities.ClickButton(VentasLocators.Reportes.CaracteristicaDropdown(tarjeta));
            _utilities.ClickButton(VentasLocators.Reportes.CaracteristicaOpcion(caracteristica));
            _driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
            Thread.Sleep(500);
        }

        // ── Tab: Vendedor ────────────────────────────────────────────────────────
        public void SeleccionarVendedor(string vendedor)
        {
            var chips = _driver.FindElements(VentasLocators.Reportes.VendedorChip(vendedor));
            if (chips.Any(e => { try { return e.Displayed; } catch { return false; } }))
            { Thread.Sleep(500); return; }
            _utilities.ClickButton(VentasLocators.Reportes.VendedorDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.VendedorOption(vendedor));
            Thread.Sleep(500);
        }

        public void SeleccionarFiltroEnTarjeta(string valor, string filtro, string tarjeta)
        {
            _utilities.ClickButton(VentasLocators.Reportes.FiltroEnTarjeta(tarjeta, filtro));
            _utilities.ClickButton(VentasLocators.Reportes.FiltroOpcion(valor));
            _driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
            Thread.Sleep(500);
        }

        // ── Tab: Grupos ──────────────────────────────────────────────────────────
        public void SeleccionarEstablecimiento(string establecimiento)
        {
            var chips = _driver.FindElements(VentasLocators.Reportes.EstablecimientoChip(establecimiento));
            if (chips.Any(e => { try { return e.Displayed; } catch { return false; } }))
            { Thread.Sleep(500); return; }
            _utilities.ClickButton(VentasLocators.Reportes.EstablecimientoDropdown);
            _utilities.ClickButton(VentasLocators.Reportes.EstablecimientoOption(establecimiento));
            Thread.Sleep(500);
        }


        public void ClickVerReporte(string tarjeta)
        {
            var locator = VentasLocators.Reportes.VerReporteEnTarjeta(tarjeta);
            IWebElement btn = null;
            try
            {
                btn = _wait.Until(d =>
                    d.FindElements(locator).FirstOrDefault(e =>
                    { try { return e.Displayed; } catch { return false; } }));
            }
            catch { }

            if (btn == null)
                throw new Exception($"No se encontró el botón VER REPORTE para la tarjeta '{tarjeta}'.");

            var handlesAntes = _driver.WindowHandles.ToList();
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btn);
            Thread.Sleep(300);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(2000);

            string handleActual = null;
            try { handleActual = _driver.CurrentWindowHandle; } catch { }
            var handlesDespues = _driver.WindowHandles.ToList();
            if (handleActual == null || !handlesDespues.Contains(handleActual))
            {
                if (handlesDespues.Any()) _driver.SwitchTo().Window(handlesDespues.Last());
            }
            else
            {
                var nuevaPestana = handlesDespues.Except(handlesAntes).FirstOrDefault();
                if (nuevaPestana != null) _driver.SwitchTo().Window(nuevaPestana);
            }
        }

        // ── Verificaciones ───────────────────────────────────────────────────────
        public bool VerificarReporteGenerado()
        {
            try
            {
                Thread.Sleep(2000);

                bool hayError = _driver.FindElements(By.XPath(
                    "//*[contains(@class,'toast-error') or contains(@class,'alert-danger')" +
                    " or (contains(@class,'swal2-popup') and .//*[contains(@class,'swal2-error-icon')])]"))
                    .Any(e => { try { return e.Displayed; } catch { return false; } });
                if (hayError) return false;

                var waitReporte = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
                var elemento = waitReporte.Until(d =>
                    d.FindElements(By.XPath(
                        "//div[contains(@class,'table-responsive')]//table" +
                        " | //table[.//tbody/tr or .//thead/tr]" +
                        " | //ngx-datatable" +
                        " | //canvas" +
                        " | //*[contains(normalize-space(),'No hay datos') or contains(normalize-space(),'Sin resultado') or contains(normalize-space(),'no se encontraron') or contains(normalize-space(),'Sin datos')]"))
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

        public void ValidarResultadoReporte(string resultadoEsperado)
        {
            switch (resultadoEsperado.Trim().ToLower())
            {
                case "no permite aplicar el filtro inhabilitado":
                    Assert.IsFalse(_driver.Url.Contains("/sales/report/view"),
                        "El sistema no debería haber generado el reporte con fechas inválidas.");
                    break;
                case "aplica el filtro correctamente":
                    Assert.IsTrue(VerificarReporteGenerado(),
                        "Se esperaba que el filtro se aplicara y el reporte se generara correctamente.");
                    break;
                default:
                    Assert.Fail($"Resultado esperado no reconocido: '{resultadoEsperado}'");
                    break;
            }
        }
        // =========================
        // REUTILIZABLE DE Ventas
        // =========================

        // Modulo y submodulos

        public void AccederModulo(string modulo)
        {
            var locator = By.XPath($"//span[normalize-space()='{modulo}']/ancestor::a[1]");
            _wait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();
        }

        public void AccederSubmodulo(string submodulo)
        {
            var locator = By.XPath($"//span[contains(text(),'{submodulo}')]");
            _wait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();
        }
    }
}
       