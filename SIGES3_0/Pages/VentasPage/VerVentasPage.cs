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
            var js = (IJavaScriptExecutor)driver;
            Thread.Sleep(800); // Esperar que canje mode termine de renderizar checkboxes
            for (int fila = 1; fila <= cantidad; fila++)
            {
                // Busca el elemento clickeable dentro de td[1] en orden de prioridad:
                // input ? label ? [role=checkbox] ? primer hijo ? td[1] mismo
                js.ExecuteScript(
                    "var rows = document.querySelectorAll('tbody tr');" +
                    "var idx = arguments[0] - 1;" +
                    "if (idx >= rows.length) return;" +
                    "var td = rows[idx].querySelector('td:first-child');" +
                    "if (!td) return;" +
                    "var target = td.querySelector('input') ||" +
                    "             td.querySelector('label') ||" +
                    "             td.querySelector('[role=\"checkbox\"]') ||" +
                    "             td.firstElementChild ||" +
                    "             td;" +
                    "target.click();",
                    (long)fila);
                Console.WriteLine($"Fila {fila}: click enviado");
                Thread.Sleep(600);
            }
        }

        public void ClickCanjear()
        {
            var btn = driver.FindElement(VentasLocators.ViewSales.RedeemButton);
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btn);
            Thread.Sleep(500);
            js.ExecuteScript("arguments[0].click();", btn);
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

        public void SeleccionarNVsPorSerie(string nvListCsv)
        {
            foreach (var serie in nvListCsv.Split(',').Select(s => s.Trim()))
            {
                utilities.ClickButton(VentasLocators.ViewSales.NvRowCheckboxBySerie(serie));
                Thread.Sleep(800);
            }
        }

        public void VerificarMensajeInconsistencia()
        {
            Thread.Sleep(1000);
            bool hayMensaje = driver.FindElements(VentasLocators.ViewSales.ModalInconsistencia)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            Assert.IsTrue(hayMensaje, "Se esperaba una advertencia de inconsistencia en el modal.");
        }

        public void FiltrarVentasDiaDeHoy()
        {
            var hoy = DateTime.Now.ToString("dd/MM/yyyy");

            var inicioLocator = EsperarLocadorFecha(
                VentasLocators.ViewSales.InitialDate,
                VentasLocators.ViewSales.FechaHoraInicial);
            IngresarFechaHora(inicioLocator, $"{hoy} 12:00 am");
            Thread.Sleep(400);

            var finLocator = EsperarLocadorFecha(
                VentasLocators.ViewSales.FinalDate,
                VentasLocators.ViewSales.FechaHoraFinal);
            IngresarFechaHora(finLocator, $"{hoy} 11:59 pm");
            Thread.Sleep(400);

            QuerySales();
        }

        // Espera hasta 10 s a que aparezca el locator primario; si no, usa el alternativo.
        private By EsperarLocadorFecha(By primario, By alternativo, int timeoutSeconds = 10)
        {
            var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);
            while (DateTime.UtcNow < deadline)
            {
                if (driver.FindElements(primario).Count > 0)
                    return primario;
                if (driver.FindElements(alternativo).Count > 0)
                    return alternativo;
                Thread.Sleep(500);
            }
            return alternativo;
        }

        // Establece el valor del campo fecha/hora.
        // Usa el setter nativo de HTMLInputElement para que Angular detecte el cambio
        // (equivalente a interacción real del usuario en componentes controlados).
        private void IngresarFechaHora(By locator, string valor)
        {
            var el = driver.FindElement(locator);
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(
                "var el = arguments[0]; var val = arguments[1];" +
                "el.removeAttribute('readonly');" +
                "var setter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;" +
                "setter.call(el, val);" +
                "el.dispatchEvent(new Event('input', { bubbles: true }));" +
                "el.dispatchEvent(new Event('change', { bubbles: true }));",
                el, valor);
            Thread.Sleep(300);
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
