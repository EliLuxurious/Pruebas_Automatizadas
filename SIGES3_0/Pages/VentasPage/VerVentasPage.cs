using SIGES3_0.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Linq;
using System.Threading;

namespace SIGES3_0.Pages.VentasPage
{
    public class VerVentasPage
    {
        private readonly IWebDriver driver;
        private readonly Utilities utilities;

        public VerVentasPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        public void QuerySales()
        {
            utilities.ClickButton(VentasLocators.ViewSales.QueryButton);
        }

        // Canje de Notas de Venta ??????????????????????????????????????????????

        public void ActivarModoCanje()
        {
            utilities.ClickButton(VentasLocators.ViewSales.ActivateRedeem);
            Thread.Sleep(800);
        }

        public void SeleccionarNVs(int cantidad)
        {
            for (int fila = 1; fila <= cantidad; fila++)
            {
                utilities.ClickButton(VentasLocators.ViewSales.NvRowCheckbox(fila));
                Thread.Sleep(400);
            }
        }

        public void ClickCanjear()
        {
            utilities.ClickButton(VentasLocators.ViewSales.RedeemButton);
            Thread.Sleep(1500);
        }

        public void SeleccionarComprobanteEnModal(string tipo)
        {
            utilities.ClickButton(VentasLocators.ViewSales.ModalComprobanteDropdown);
            Thread.Sleep(600);
            utilities.ClickButton(VentasLocators.ViewSales.ModalComprobanteOpcion(tipo));
            Thread.Sleep(600);
        }

        public void SeleccionarSerieEnModal(string serie)
        {
            utilities.ClickButton(VentasLocators.Voucher.SeriesInputByText(serie));
            Thread.Sleep(500);
        }

        public void ConfirmarCanje()
        {
            utilities.ClickButton(VentasLocators.ViewSales.AcceptRedeemButton);
            Thread.Sleep(2500);
        }

        public void VerificarCanjeExitoso()
        {
            Thread.Sleep(2000);

            bool hayError = driver.FindElements(By.XPath(
                "//*[contains(@class,'toast-error') or contains(@class,'alert-danger')]"))
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            Assert.IsFalse(hayError, "Se produjo un error al intentar canjear la nota de venta.");

            bool hayExito = driver.FindElements(VentasLocators.ViewSales.CanjeExitoToast)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (!hayExito)
            {
                bool modalCerrado = !driver.FindElements(
                    By.XPath("//div[contains(@class,'modal') and contains(@class,'show')]"))
                    .Any(e => { try { return e.Displayed; } catch { return false; } });

                Assert.IsTrue(modalCerrado, "El modal de canje no se cerró tras aceptar.");
            }
        }

        public void VerificarBotonCanjearDeshabilitado()
        {
            Thread.Sleep(500);
            var btn = driver.FindElements(VentasLocators.ViewSales.RedeemButton)
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

            Assert.IsTrue(
                btn == null || !btn.Enabled || btn.GetAttribute("disabled") != null,
                "Se esperaba que el botón Canjear estuviera deshabilitado.");
        }

        public void VerificarMensajeInconsistencia()
        {
            Thread.Sleep(1000);
            bool hayMensaje = driver.FindElements(VentasLocators.ViewSales.ModalInconsistencia)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            Assert.IsTrue(hayMensaje, "Se esperaba una advertencia de inconsistencia en el modal.");
        }

        public void VerificarBotonAceptarDeshabilitado()
        {
            var btn = driver.FindElements(VentasLocators.ViewSales.AcceptRedeemButton)
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

            Assert.IsTrue(
                btn == null || !btn.Enabled || btn.GetAttribute("disabled") != null,
                "Se esperaba que el botón Aceptar estuviera deshabilitado.");
        }
    }
}
