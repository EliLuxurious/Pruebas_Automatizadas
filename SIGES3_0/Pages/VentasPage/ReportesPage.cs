using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SIGES3_0.Pages.Helpers;
using System;
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

        public void NavegarAReportes()
        {
            _utilities.ClickButton(VentasLocators.Navigation.SalesMenu);
            _utilities.ClickButton(VentasLocators.Navigation.Reports);
            Thread.Sleep(2000); // Wait for page to load
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
                    case "series": _utilities.ClickButton(VentasLocators.Reportes.TabSeries); break;
                    case "conceptos": _utilities.ClickButton(VentasLocators.Reportes.TabConceptos); break;
                    case "vendedor": _utilities.ClickButton(VentasLocators.Reportes.TabVendedor); break;
                    case "grupos": _utilities.ClickButton(VentasLocators.Reportes.TabGrupos); break;
                    case "excepciones": _utilities.ClickButton(VentasLocators.Reportes.TabExcepciones); break;
                    default: throw new Exception($"La vista/tab '{tabName}' no existe en Reportes.");
                }
            }
            Thread.Sleep(1000);
        }

        public void IngresarFechas(string fechaInicial, string fechaFinal)
        {
            _utilities.ClearAndEnterText(VentasLocators.Reportes.FechaHoraInicial, fechaInicial);
            _utilities.ClearAndEnterText(VentasLocators.Reportes.FechaHoraFinal, fechaFinal);
            // sometimes it requires pressing enter or clicking outside
            _driver.FindElement(VentasLocators.Reportes.FechaHoraFinal).SendKeys(Keys.Tab);
        }

        public void SeleccionarTipoComprobante(string tipoComprobante)
        {
            _utilities.ClickButton(VentasLocators.Reportes.TipoComprobanteDropdown);
            _utilities.ClearAndEnterText(VentasLocators.Reportes.TipoComprobanteSearch, tipoComprobante);
            _utilities.ClickButton(VentasLocators.Reportes.TipoComprobanteOption(tipoComprobante));
            Thread.Sleep(1000); // wait for series to load
        }

        public void SeleccionarSerie(string serie)
        {
            _utilities.ClickButton(VentasLocators.Reportes.SerieDropdown);
            _utilities.ClearAndEnterText(VentasLocators.Reportes.SerieSearch, serie);
            _utilities.ClickButton(VentasLocators.Reportes.SerieOption(serie));
        }

        public void ClickVerReporte(string tarjeta)
        {
            if (tarjeta.Equals("POR COMPROBANTE", StringComparison.OrdinalIgnoreCase))
            {
                _utilities.ClickButton(VentasLocators.Reportes.PorComprobanteVerReporte);
            }
            else if (tarjeta.Equals("POR FAMILIA", StringComparison.OrdinalIgnoreCase))
            {
                _utilities.ClickButton(VentasLocators.Reportes.PorFamiliaVerReporte);
            }
            Thread.Sleep(2000);
        }

        public bool VerificarReporteGenerado()
        {
            try
            {
                return _driver.FindElement(VentasLocators.Reportes.HeaderReporteResultado).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool VerificarBotonHabilitado(string tarjeta)
        {
            IWebElement btn = null;
            if (tarjeta.Equals("POR COMPROBANTE", StringComparison.OrdinalIgnoreCase))
            {
                btn = _driver.FindElement(VentasLocators.Reportes.PorComprobanteVerReporte);
            }
            else if (tarjeta.Equals("POR FAMILIA", StringComparison.OrdinalIgnoreCase))
            {
                btn = _driver.FindElement(VentasLocators.Reportes.PorFamiliaVerReporte);
            }

            if (btn == null) return false;
            return btn.Enabled && btn.GetAttribute("disabled") == null;
        }

        public void SeleccionarPuntoVenta(string puntoVenta)
        {
            _utilities.ClickButton(VentasLocators.Reportes.PuntoVentaDropdown);
            _utilities.ClearAndEnterText(VentasLocators.Reportes.PuntoVentaSearch, puntoVenta);
            _utilities.ClickButton(VentasLocators.Reportes.PuntoVentaOption(puntoVenta));
        }

        public void SeleccionarFamilia(string familia)
        {
            _utilities.ClickButton(VentasLocators.Reportes.FamiliaDropdown);
            _utilities.ClearAndEnterText(VentasLocators.Reportes.FamiliaSearch, familia);
            _utilities.ClickButton(VentasLocators.Reportes.FamiliaOption(familia));
        }
    }
}
