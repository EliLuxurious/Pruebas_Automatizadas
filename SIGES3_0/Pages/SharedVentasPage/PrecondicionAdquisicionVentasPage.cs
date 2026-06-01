using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using System.Threading;

namespace SIGES3_0.Pages.SharedVentasPage
{
    public class PrecondicionAdquisicionVentasPage
    {
        private readonly IWebDriver _driver;

        /*
         * Page de apoyo para precondiciones de Ventas/Reportes.
         *
         * Esta precondición usa la vista RegisterProcurement para preparar stock.
         * En Adquisición ya existe lógica parecida para Entrega, pero los métodos
         * internos de selección están privados y no se pueden reutilizar parcialmente.
         *
         * Por eso se replica aquí la lógica necesaria, sin modificar Pages/Adquisicion
         * ni NuevaAdquisicionPage. Esta duplicación queda aislada en SharedVentas.
         */

        private readonly By panelEntrega = By.XPath(
            "//div[contains(@id,'entrega') or .//*[normalize-space()='Entrega']]"
        );

        /*
         * Locators basados en el mismo patrón usado en NuevaAdquisicionPage:
         * label/span del campo + following-sibling::app-dropdown-search.
         */
        private readonly By cmbRol = By.XPath(
            "//span[contains(text(), 'Rol') or contains(text(), 'Role')]" +
            "/following-sibling::app-dropdown-search//div[contains(@class, 'select-trigger')]"
        );

        private readonly By cmbEstablecimiento = By.XPath(
            "//span[contains(text(), 'Establecimiento') or contains(text(), 'Establishment')]" +
            "/following-sibling::app-dropdown-search//div[contains(@class, 'select-trigger')]"
        );

        private readonly By cmbAlmacen = By.XPath(
            "//span[contains(text(), 'Almacén') or contains(text(), 'Almacen') or contains(text(), 'Warehouse')]" +
            "/following-sibling::app-dropdown-search//div[contains(@class, 'select-trigger')]"
        );

        private readonly By inputBusquedaDropdown = By.XPath(
            "//input[@placeholder='Buscar.' or @placeholder='Buscar...' or contains(@placeholder,'Buscar') or contains(@placeholder,'buscar') or contains(@class,'select-search') or contains(@class,'search-input')]"
        );

        public PrecondicionAdquisicionVentasPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void SeleccionarYConfigurarProductoVentas(string producto, string cantidad, string valorUnitario)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            var js = (IJavaScriptExecutor)_driver;

            // Abrir el combo de concepto
            By cmbConcepto = By.XPath("//label[@for='conceptSelect']/following-sibling::div//div[contains(@class,'select-trigger')]");
            IWebElement combo = wait.Until(_ => _driver.FindElements(cmbConcepto)
                .FirstOrDefault(ElementoVisibleYHabilitado));
            js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", combo);
            Thread.Sleep(800);
            ClickSeguro(combo);
            Thread.Sleep(1200);

            // Escribir el código en el input de búsqueda para filtrar
            string codigo = producto.Contains('|') ? producto.Split('|')[0].Trim() : producto;
            By inputBuscar = By.XPath(
                "(//input[contains(@placeholder,'Buscar') or contains(@placeholder,'buscar')" +
                " or contains(@class,'select-search') or contains(@class,'search-input')])[last()]");
            IWebElement inputFiltro = wait.Until(_ => _driver.FindElements(inputBuscar)
                .FirstOrDefault(ElementoVisibleYHabilitado));
            inputFiltro.SendKeys(Keys.Control + "a");
            inputFiltro.SendKeys(Keys.Delete);
            inputFiltro.SendKeys(codigo);
            Thread.Sleep(1500);

            // Hacer clic en el contenedor de la opción (li/div con clase option/item) que Angular escucha
            string literal = XPathLiteral(codigo);
            By selectorOpcion = By.XPath(
                $"//*[contains(@class,'option') or contains(@class,'item') or contains(@class,'result')]" +
                $"[.//*[contains(normalize-space(),{literal})] or contains(normalize-space(),{literal})]");

            IWebElement opcion = wait.Until(_ => _driver.FindElements(selectorOpcion)
                .FirstOrDefault(ElementoVisibleYHabilitado));

            js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", opcion);
            Thread.Sleep(200);

            try
            {
                new Actions(_driver)
                    .MoveToElement(opcion)
                    .Pause(TimeSpan.FromMilliseconds(200))
                    .Click()
                    .Perform();
            }
            catch
            {
                js.ExecuteScript(
                    "arguments[0].dispatchEvent(new MouseEvent('click',{bubbles:true,cancelable:true}));",
                    opcion);
            }

            Thread.Sleep(2000);

            // Llenar cantidad en la última fila
            By txtCantidad = By.XPath("//tbody/tr[contains(@class,'ng-star-inserted')]/td[3]//input");
            var cantidades = _driver.FindElements(txtCantidad);
            if (cantidades.Count > 0)
            {
                var campo = cantidades[cantidades.Count - 1];
                campo.Clear();
                campo.SendKeys(cantidad);
            }

            Thread.Sleep(500);

            // Llenar valor unitario en la última fila
            By txtValorUnitario = By.XPath("//tbody/tr[contains(@class,'ng-star-inserted')]/td[4]//input | //tbody/tr[contains(@class,'ng-star-inserted')]/td[5]//input");
            var precios = _driver.FindElements(txtValorUnitario);
            if (precios.Count > 0)
            {
                var campo = precios[precios.Count - 1];
                campo.Clear();
                campo.SendKeys(valorUnitario);
            }

            Thread.Sleep(1000);
        }

        public void ConfigurarEntregaVentas(string rol, string establecimiento, string almacen)
        {
            SeleccionarDropdownEntrega(cmbRol, rol, "Rol");
            EsperarCarga();

            SeleccionarDropdownEntrega(cmbEstablecimiento, establecimiento, "Establecimiento");
            EsperarCarga();

            SeleccionarDropdownEntrega(cmbAlmacen, almacen, "Almacén");
            EsperarCarga();
        }

        private void SeleccionarDropdownEntrega(By locatorPrincipal, string valor, string campo)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return;

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            var js = (IJavaScriptExecutor)_driver;

            CerrarDropdownsFuerte();

            IWebElement trigger = wait.Until(_ => BuscarTriggerEntrega(locatorPrincipal, campo));

            js.ExecuteScript(
                "arguments[0].scrollIntoView({block:'center', inline:'nearest'});",
                trigger
            );

            Thread.Sleep(500);

            ClickSeguro(trigger);
            Thread.Sleep(800);

            IWebElement input = wait.Until(_ => BuscarInputDropdownVisible());

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            input.SendKeys(valor);

            Thread.Sleep(1200);

            IWebElement opcion = wait.Until(_ => BuscarOpcionVisible(valor));

            js.ExecuteScript(
                "arguments[0].scrollIntoView({block:'center', inline:'nearest'});",
                opcion
            );

            Thread.Sleep(300);

            ClickSeguro(opcion);
            Thread.Sleep(1000);

            CerrarDropdownsFuerte();

            Thread.Sleep(800);

            if (!TriggerContieneValor(trigger, valor))
            {
                throw new InvalidOperationException(
                    $"Se encontró '{valor}' en '{campo}', pero no quedó seleccionado en el formulario.");
            }
        }

        private IWebElement? BuscarTriggerEntrega(By locatorPrincipal, string campo)
        {
            /*
             * Primero intenta con el mismo locator usado en Adquisición.
             */
            var principal = _driver.FindElements(locatorPrincipal)
                .FirstOrDefault(ElementoVisibleYHabilitado);

            if (principal != null)
                return principal;

            /*
             * Segundo intento: busca dentro de la sección Entrega por label exacto.
             * Esto evita depender del orden: Rol, Establecimiento, Almacén.
             */
            string campoLiteral = XPathLiteral(campo);

            var porLabel = _driver.FindElements(By.XPath(
                $"//*[contains(normalize-space(),'Entrega')]/following::*[self::label or self::span][contains(normalize-space(),{campoLiteral})][1]" +
                $"/following::app-dropdown-search[1]//div[contains(@class,'select-trigger')]"
            ))
            .FirstOrDefault(ElementoVisibleYHabilitado);

            if (porLabel != null)
                return porLabel;

            /*
             * Tercer intento: por placeholder visible del combo.
             */
            string placeholder = campo switch
            {
                "Rol" => "Seleccione un rol",
                "Establecimiento" => "Seleccione un establecimiento",
                "Almacén" => "Seleccione un almacén",
                _ => string.Empty
            };

            if (!string.IsNullOrWhiteSpace(placeholder))
            {
                string placeholderLiteral = XPathLiteral(placeholder);

                var porPlaceholder = _driver.FindElements(By.XPath(
                    $"//span[normalize-space()={placeholderLiteral} or contains(normalize-space(),{placeholderLiteral})]" +
                    "/ancestor::div[contains(@class,'select-trigger')][1]"
                ))
                .FirstOrDefault(ElementoVisibleYHabilitado);

                if (porPlaceholder != null)
                    return porPlaceholder;
            }

            throw new NoSuchElementException($"No se encontró el dropdown de '{campo}' en Entrega.");
        }

        private IWebElement? BuscarInputDropdownVisible()
        {
            return _driver.FindElements(inputBusquedaDropdown)
                .LastOrDefault(ElementoVisibleYHabilitado);
        }

        private IWebElement? BuscarOpcionVisible(string valor)
        {
            string valorLiteral = XPathLiteral(valor);

            /*
             * Busca primero dentro de la lista abierta del dropdown.
             */
            var candidatos = _driver.FindElements(By.XPath(
                $"//*[contains(@class,'select-results') or contains(@class,'options') or contains(@class,'dropdown') or contains(@class,'option')]" +
                $"//*[self::span or self::div or self::li or self::button][normalize-space()={valorLiteral}]"
            ))
            .Where(ElementoVisibleYHabilitado)
            .ToList();

            /*
             * Si no aparece exacto, busca por contenido.
             */
            if (candidatos.Count == 0)
            {
                candidatos = _driver.FindElements(By.XPath(
                    $"//*[contains(@class,'select-results') or contains(@class,'options') or contains(@class,'dropdown') or contains(@class,'option')]" +
                    $"//*[self::span or self::div or self::li or self::button][contains(normalize-space(),{valorLiteral})]"
                ))
                .Where(ElementoVisibleYHabilitado)
                .ToList();
            }

            /*
             * Último intento global, por si Angular renderiza el dropdown fuera del componente.
             */
            if (candidatos.Count == 0)
            {
                candidatos = _driver.FindElements(By.XPath(
                    $"//*[self::span or self::div or self::li or self::button][normalize-space()={valorLiteral} or contains(normalize-space(),{valorLiteral})]"
                ))
                .Where(ElementoVisibleYHabilitado)
                .ToList();
            }

            var textoVisible = candidatos.LastOrDefault();

            if (textoVisible == null)
                throw new NoSuchElementException($"No se encontró la opción visible '{valor}'.");

            /*
             * El texto puede estar en un span; el clic real suele estar en el padre.
             */
            var padreClickeable = textoVisible.FindElements(By.XPath(
                "./ancestor::*[contains(@class,'option') or contains(@class,'item') or contains(@class,'result')][1]"
            ))
            .FirstOrDefault(ElementoVisibleYHabilitado);

            return padreClickeable ?? textoVisible;
        }

        private void ClickSeguro(IWebElement elemento)
        {
            var js = (IJavaScriptExecutor)_driver;

            try
            {
                new Actions(_driver)
                    .MoveToElement(elemento)
                    .Pause(TimeSpan.FromMilliseconds(150))
                    .Click()
                    .Perform();
            }
            catch
            {
                js.ExecuteScript(@"
                    arguments[0].dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
                    arguments[0].dispatchEvent(new MouseEvent('mouseup', { bubbles: true }));
                    arguments[0].dispatchEvent(new MouseEvent('click', { bubbles: true }));
                ", elemento);
            }
        }

        private void CerrarDropdownsFuerte()
        {
            try
            {
                var body = _driver.FindElement(By.TagName("body"));

                body.SendKeys(Keys.Escape);
                Thread.Sleep(250);

                body.SendKeys(Keys.Tab);
                Thread.Sleep(250);

                ((IJavaScriptExecutor)_driver).ExecuteScript(@"
                    if (document.activeElement) {
                        document.activeElement.blur();
                    }

                    document.body.dispatchEvent(new KeyboardEvent('keydown', {
                        key: 'Escape',
                        code: 'Escape',
                        keyCode: 27,
                        which: 27,
                        bubbles: true
                    }));
                ");

                Thread.Sleep(350);
            }
            catch
            {
            }
        }

        private bool TriggerContieneValor(IWebElement trigger, string valor)
        {
            try
            {
                return trigger.Text.Contains(valor, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private void EsperarCarga()
        {
            Thread.Sleep(1500);
        }

        private static bool ElementoVisibleYHabilitado(IWebElement element)
        {
            try
            {
                var classes = element.GetAttribute("class") ?? string.Empty;
                var ariaDisabled = element.GetAttribute("aria-disabled") ?? string.Empty;
                var disabled = element.GetAttribute("disabled") ?? string.Empty;

                return element.Displayed &&
                       element.Enabled &&
                       !classes.Contains("disabled", StringComparison.OrdinalIgnoreCase) &&
                       !ariaDisabled.Equals("true", StringComparison.OrdinalIgnoreCase) &&
                       string.IsNullOrWhiteSpace(disabled);
            }
            catch
            {
                return false;
            }
        }

        private static string XPathLiteral(string value)
        {
            if (!value.Contains("'"))
                return $"'{value}'";

            if (!value.Contains("\""))
                return $"\"{value}\"";

            return "concat('" + value.Replace("'", "',\"'\",'") + "')";
        }
    }
}
