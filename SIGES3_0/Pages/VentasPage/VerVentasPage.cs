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
            // El modal fue rediseñado (Bug 21099) y puede tener native <select> o custom dropdown.
            // Se intenta primero con native select; si no existe se usa custom dropdown con trigger.
            var nativeSelect = driver.FindElements(
                By.XPath("//div[contains(@class,'modal')]//select"))
                .FirstOrDefault(e => { try { return e.Displayed && e.Enabled; } catch { return false; } });

            if (nativeSelect != null)
            {
                new OpenQA.Selenium.Support.UI.SelectElement(nativeSelect).SelectByText(tipo);
                Thread.Sleep(600);
                return;
            }

            // Custom dropdown: busca el trigger asociado a la etiqueta "comprobante"
            var trigger = driver.FindElements(By.XPath(
                "//div[contains(@class,'modal')]//*[contains(normalize-space(),'Selecciona un comprobante') or contains(normalize-space(),'Comprobante')]/following::*[contains(@class,'select-trigger') or contains(@class,'dropdown-toggle')][1]"))
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

            // Fallback: segundo select-trigger del modal (el primero es "Emitir a nombre de")
            trigger ??= driver.FindElements(By.XPath(
                "//div[contains(@class,'modal')]//*[contains(@class,'select-trigger')]"))
                .Where(e => { try { return e.Displayed; } catch { return false; } })
                .Skip(1).FirstOrDefault();

            Assert.IsNotNull(trigger, $"No se encontro el dropdown de comprobante en el modal de canje.");
            trigger!.Click();
            Thread.Sleep(800);

            // Espera a que la opcion sea visible despues de abrir el dropdown
            IWebElement? opcion = null;
            try
            {
                opcion = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(200)
                }.Until(d => d.FindElements(VentasLocators.ViewSales.ModalComprobanteOpcion(tipo))
                    .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } }));
            }
            catch { /* fallback abajo */ }

            Assert.IsNotNull(opcion, $"No se encontro la opcion '{tipo}' en el dropdown de comprobante del modal de canje.");
            opcion!.Click();
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

                Assert.IsTrue(modalCerrado, "El modal de canje no se cerr� tras aceptar.");
            }
        }

        public void VerificarBotonCanjearDeshabilitado()
        {
            Thread.Sleep(500);
            var btn = driver.FindElements(VentasLocators.ViewSales.RedeemButton)
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

            Assert.IsTrue(
                btn == null || !btn.Enabled || btn.GetAttribute("disabled") != null,
                "Se esperaba que el bot�n Canjear estuviera deshabilitado.");
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

        // Filtra la tabla de Ver Ventas por el tipo de documento (ej: "BV", "FAC").
        // Aunque la idea original era tomar el ultimo comprobante emitido buscando por fecha,
        // la cantidad de registros en Ver Ventas y el orden variable de la tabla hacen que
        // filtrar por tipo de documento sea mas confiable para ubicar el comprobante correcto.
        public void FiltrarPorTipoDoc(string tipoDoc)
        {
            // Los inputs de filtro de columna solo aparecen despues de que la tabla tiene datos.
            // Si aun no hay resultados se consulta primero para forzar la carga de la tabla.
            bool tablaVacia = !driver.FindElements(By.CssSelector("table tbody tr")).Any(e =>
            {
                try { return e.Displayed; } catch { return false; }
            });

            if (tablaVacia)
            {
                Console.WriteLine("[VerVentas] Tabla vacia - consultando ventas antes de filtrar.");
                QuerySales();
                Thread.Sleep(1000);
            }

            var input = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10))
            {
                PollingInterval = TimeSpan.FromMilliseconds(200)
            }.Until(d => d.FindElements(VentasLocators.ViewSales.FiltroTipoDoc)
                .FirstOrDefault(e => { try { return e.Displayed && e.Enabled; } catch { return false; } }));

            if (input == null)
            {
                Console.WriteLine("[VerVentas] No se encontro el filtro de Tipo Doc - se omite el filtrado.");
                return;
            }

            input.Clear();
            input.SendKeys(tipoDoc.Trim());
            Thread.Sleep(800);
            Console.WriteLine($"[VerVentas] Filtrado por tipo doc: '{tipoDoc}'");
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
        // (equivalente a interacci�n real del usuario en componentes controlados).
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
                "Se esperaba que el bot�n Aceptar estuviera deshabilitado.");
        }
    }
}
