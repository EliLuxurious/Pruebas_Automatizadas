using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.StepDefinitions.Items.NewItems;
using SIGES3_0.StepDefinitions.Items.RegisterItemData;
using SIGES3_0.StepDefinitions.Items.ViewItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SIGES3_0.StepDefinitions.SharedVentasStep
{
    [Binding]
    public class PrecondicionItemComercialVentasStepDefinitions
    {
        private readonly IWebDriver _driver;

        public PrecondicionItemComercialVentasStepDefinitions(IWebDriver driver)
        {
            _driver = driver;
        }

        [Given("existe el concepto item comercial para ventas:")]
        public void GivenExisteElConceptoItemComercialParaVentas(Table table)
        {
            var item = table.Rows[0];
            var itemsSteps = new NewItemStepDefinitions(_driver);
            var datosConceptoSteps = new RegisterItemDataStepDefinitions(_driver);
            var verConceptosSteps = new ViewItemStepDefinitions(_driver);

            string familia = Valor(item, "Familia");
            string codigo = Valor(item, "Codigo");

            if (!FamiliaDisponibleEnNuevoConcepto(itemsSteps, familia))
                CrearFamiliaConStepsDeItems(itemsSteps, datosConceptoSteps, item);

            if (ConceptoExisteConStepsDeItems(itemsSteps, verConceptosSteps, familia, codigo))
                return;

            CrearConceptoConStepsDeItemsSiHaceFalta(itemsSteps, item, codigo);
        }

        private bool FamiliaDisponibleEnNuevoConcepto(NewItemStepDefinitions itemsSteps, string familia)
        {
            try
            {
                AbrirNuevoConceptoConStepsDeItems(itemsSteps);
                itemsSteps.WhenElUsuarioSeleccionaLaFamilia(familia);
                return true;
            }
            catch
            {
                CerrarMensajeSiExiste();
                return false;
            }
        }

        private void CrearFamiliaConStepsDeItems(
            NewItemStepDefinitions itemsSteps,
            RegisterItemDataStepDefinitions datosConceptoSteps,
            IDictionary<string, string> item)
        {
            try
            {
                AbrirModalFamiliaDesdeNuevoConcepto(itemsSteps);
                datosConceptoSteps.WhenElUsuarioSeleccionaElTipo(Valor(item, "TipoFamilia", "Tipo Familia", defaultValue: "Bien"));
                datosConceptoSteps.WhenElUsuarioSeleccionaElTipoDeTratamientoDinamico(Valor(item, "TratamientoIGVFamilia", "Tratamiento IGV Familia", defaultValue: "Exoneracion IGV"));
                IngresarCodigoFamiliaPrecondicion(Valor(item, "CodigoFamilia", "Codigo Familia", defaultValue: $"QA-{ConstruirCodigoFamilia(Valor(item, "Familia"))}"));
                datosConceptoSteps.WhenElUsuarioIngresaElNombreDeFamilia(Valor(item, "Familia"));
                SeleccionarCategoriaFamiliaPrecondicion(Valor(item, "CategoriaFamilia", "Categoria Familia", defaultValue: "SIN CATEGORIA"));
                if (!string.IsNullOrWhiteSpace(Valor(item, "Marca")))
                    SeleccionarCaracteristicaFamiliaPrecondicion("MARCA");
                GuardarFamiliaPrecondicion();
            }
            catch (Exception ex) when (EsRegistroExistente(ex))
            {
                CerrarMensajeSiExiste();
            }
        }

        private void AbrirModalFamiliaDesdeNuevoConcepto(NewItemStepDefinitions itemsSteps)
        {
            AbrirNuevoConceptoConStepsDeItems(itemsSteps);

            var botonAgregarFamilia = BuscarVisible(
                By.XPath("(//label[contains(normalize-space(),'Familia')]/following::button[normalize-space()='+'])[1]"),
                By.XPath("(//button[normalize-space()='+'])[1]"));

            ClickSeguro(botonAgregarFamilia);
            Thread.Sleep(800);

            BuscarVisible(
                By.XPath("//div[contains(@class,'modal') and .//*[contains(normalize-space(),'Registro') and contains(normalize-space(),'Familia')]]"),
                By.XPath("//*[contains(normalize-space(),'Registro Familia') or contains(normalize-space(),'Registro de Familia')]"));
        }

        private void SeleccionarCaracteristicaFamiliaPrecondicion(string caracteristica)
        {
            var fila = _driver.FindElements(By.XPath(
                    $"//tr[.//td[normalize-space()={XPathLiteral(caracteristica)}] or .//*[normalize-space()={XPathLiteral(caracteristica)}]]"))
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });

            if (fila == null)
                return;

            var checks = fila.FindElements(By.XPath(".//input[@type='checkbox']")).ToList();
            foreach (var check in checks.Take(2))
            {
                try
                {
                    if (!check.Selected && check.Enabled)
                    {
                        ClickSeguro(check);
                        Thread.Sleep(200);
                    }
                }
                catch
                {
                }
            }
        }

        private bool ConceptoExisteConStepsDeItems(
            NewItemStepDefinitions itemsSteps,
            ViewItemStepDefinitions verConceptosSteps,
            string familia,
            string codigo)
        {
            try
            {
                AbrirVerConceptosConStepsDeItems(itemsSteps, verConceptosSteps);
                verConceptosSteps.WhenElUsuarioSeleccionaElFiltroFamilia(familia);
                verConceptosSteps.WhenElUsuarioIngresaLaPalabraClave(codigo);
                verConceptosSteps.WhenElUsuarioPresionaElBotonBuscar();
                Thread.Sleep(1200);

                return ExisteFila($"//tbody/tr[contains(normalize-space(),{XPathLiteral(codigo)})]");
            }
            catch
            {
                CerrarMensajeSiExiste();
                return false;
            }
        }

        private void CrearConceptoConStepsDeItemsSiHaceFalta(
            NewItemStepDefinitions itemsSteps,
            IDictionary<string, string> item,
            string codigo)
        {
            try
            {
                AbrirNuevoConceptoConStepsDeItems(itemsSteps);
                itemsSteps.WhenElUsuarioSeleccionaLaFamilia(Valor(item, "Familia"));
                IngresarCodigoBarraPrecondicion(codigo);
                itemsSteps.WhenElUsuarioIngresaElSufijo(Valor(item, "Sufijo"));
                itemsSteps.WhenElUsuarioSeleccionaLaUMComercial(Valor(item, "UMComercial", "UM Comercial", defaultValue: "UN"));
                itemsSteps.WhenElUsuarioSeleccionaLaUMedida(Valor(item, "UMedida", "U Medida", defaultValue: "UN"));
                itemsSteps.WhenElUsuarioSeleccionaElRol(Valor(item, "Rol", defaultValue: "Item Comercial"));
                itemsSteps.WhenElUsuarioSeleccionaElModuloAMostrar(Valor(item, "Modulo", defaultValue: "MOD0001"));
                ConfigurarMarcaPrecondicion(Valor(item, "Marca"));
                itemsSteps.WhenElUsuarioSeleccionaLaPresentacion(Valor(item, "Presentacion"));
                itemsSteps.WhenElUsuarioIngresaLaCantidad(Valor(item, "Cantidad", defaultValue: "1"));
                itemsSteps.WhenElUsuarioSeleccionaLaUnidadDeMedida(Valor(item, "UnidadMedida", "Unidad Medida", defaultValue: "UN"));
                ConfigurarPrecioPrecondicion(
                    Valor(item, "Tarifa", defaultValue: "POR UNIDAD"),
                    Valor(item, "Precio"));
                itemsSteps.ThenGuardarConcepto();
            }
            catch (Exception ex) when (EsRegistroExistente(ex))
            {
                CerrarMensajeSiExiste();
            }
        }

        private void ConfigurarMarcaPrecondicion(string marca)
        {
            if (string.IsNullOrWhiteSpace(marca))
                return;

            var selectorMarca = _driver.FindElements(By.XPath("//select[contains(@class,'custom-select')]"))
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (selectorMarca != null)
            {
                selectorMarca.Click();
                var opcion = BuscarVisible(By.XPath($"//option[normalize-space()={XPathLiteral(marca)}]"));
                opcion.Click();
                Thread.Sleep(300);
                return;
            }

            AbrirSeccionCaracteristicasSiExiste();
            SeleccionarValorCaracteristicaPrecondicion("MARCA", marca);
        }

        private void AbrirSeccionCaracteristicasSiExiste()
        {
            var seccion = _driver.FindElements(By.XPath("//*[normalize-space()='Caracteristicas' or normalize-space()='Características']"))
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (seccion == null)
                return;

            ClickSeguro(seccion);
            Thread.Sleep(500);
        }

        private void SeleccionarValorCaracteristicaPrecondicion(string caracteristica, string valor)
        {
            var contenedor = _driver.FindElements(By.XPath(
                    $"//*[contains(normalize-space(),{XPathLiteral(caracteristica)})]/ancestor::*[self::div or self::tr][1]"))
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });

            if (contenedor == null)
                return;

            var trigger = contenedor.FindElements(By.XPath(".//div[contains(@class,'select-trigger')] | .//select | .//input[not(@type='checkbox') and not(@type='radio')]"))
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (trigger == null)
                return;

            trigger.Click();
            if (trigger.TagName.Equals("input", StringComparison.OrdinalIgnoreCase))
            {
                trigger.SendKeys(Keys.Control + "a");
                trigger.SendKeys(Keys.Delete);
                trigger.SendKeys(valor);
            }
            else
            {
                var buscador = _driver.FindElements(By.XPath("(//input[@placeholder='Buscar...'])[last()]"))
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed && e.Enabled; }
                        catch { return false; }
                    });

                if (buscador != null)
                    buscador.SendKeys(valor);

                var opcion = BuscarVisible(
                    By.XPath($"//*[normalize-space()={XPathLiteral(valor)}]"),
                    By.XPath($"//*[contains(normalize-space(),{XPathLiteral(valor)})]"));
                ClickSeguro(opcion);
            }

            Thread.Sleep(300);
        }

        private void ConfigurarPrecioPrecondicion(string tarifa, string precio)
        {
            if (string.IsNullOrWhiteSpace(precio))
                return;

            ConfigurarPrecioVentaPorTarifaPrecondicion(tarifa, precio);
        }

        private void ConfigurarPrecioVentaPorTarifaPrecondicion(string tarifa, string precio)
        {
            SeleccionarTarifaPrecondicion(tarifa);
            IngresarPrecioPrecondicion(precio);
        }

        private void SeleccionarTarifaPrecondicion(string tarifa)
        {
            if (string.IsNullOrWhiteSpace(tarifa))
                return;

            var opcionTarifa = BuscarVisible(
                By.XPath($"//label[contains(normalize-space(),{XPathLiteral(tarifa)})]/preceding-sibling::input"),
                By.XPath($"//label[contains(normalize-space(),{XPathLiteral(tarifa)})]"),
                By.XPath($"//*[self::span or self::button or self::div][normalize-space()={XPathLiteral(tarifa)}]"));

            ClickSeguro(opcionTarifa);
            Thread.Sleep(300);
        }

        private void IngresarPrecioPrecondicion(string precio)
        {
            var campoPrecio = BuscarCampoNuevoPrecioPrecondicion()
                ?? BuscarVisible(
                    By.XPath("(//*[contains(normalize-space(),'Precio de Producto') or contains(normalize-space(),'Precio de Producto/Servicio')]/following::input[@type='number' and not(@disabled)])[last()]"),
                    By.XPath("//input[contains(@placeholder,'0.00') and not(@disabled)]"));

            ((IJavaScriptExecutor)_driver).ExecuteScript(
                "arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});",
                campoPrecio);

            Thread.Sleep(500);

            try
            {
                campoPrecio.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].focus();", campoPrecio);
            }

            campoPrecio.SendKeys(Keys.Control + "a");
            campoPrecio.SendKeys(Keys.Delete);
            campoPrecio.SendKeys(precio);

            Thread.Sleep(300);
        }

        private IWebElement? BuscarCampoNuevoPrecioPrecondicion()
        {
            foreach (var tabla in _driver.FindElements(By.XPath("//table[.//th[contains(normalize-space(),'NUEVO PRECIO')]]")))
            {
                try
                {
                    if (!tabla.Displayed)
                        continue;

                    var encabezados = tabla.FindElements(By.XPath(".//th")).ToList();
                    var indiceNuevoPrecio = encabezados.FindIndex(e =>
                    {
                        try { return e.Text.Trim().Contains("NUEVO PRECIO", StringComparison.OrdinalIgnoreCase); }
                        catch { return false; }
                    });

                    if (indiceNuevoPrecio < 0)
                        continue;

                    foreach (var fila in tabla.FindElements(By.XPath(".//tbody/tr")))
                    {
                        var celdas = fila.FindElements(By.XPath("./td")).ToList();
                        if (celdas.Count <= indiceNuevoPrecio)
                            continue;

                        var input = celdas[indiceNuevoPrecio].FindElements(By.XPath(".//input[not(@disabled)]"))
                            .FirstOrDefault(e =>
                            {
                                try { return e.Displayed && e.Enabled; }
                                catch { return false; }
                            });

                        if (input != null)
                            return input;
                    }
                }
                catch
                {
                }
            }

            return null;
        }

        private void IngresarCodigoFamiliaPrecondicion(string codigo)
        {
            EscribirEnInputVisible(
                By.XPath("//label[contains(normalize-space(),'digo')]/following::input[1] | //input[contains(@placeholder,'digo') or contains(@placeholder,'Digo')]"),
                codigo);
        }

        private void IngresarCodigoBarraPrecondicion(string codigo)
        {
            EscribirEnInputVisible(
                By.XPath("//input[contains(@placeholder,'Barra')] | //label[contains(normalize-space(),'Barra')]/following::input[1]"),
                codigo);
        }

        private void SeleccionarCategoriaFamiliaPrecondicion(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria) || CategoriaFamiliaYaSeleccionada(categoria))
                return;

            var dropdown = BuscarVisible(
                By.XPath("//label[contains(normalize-space(),'Categor')]/following::div[contains(@class,'select-trigger')][1]"),
                By.XPath("//div[contains(@class,'select-trigger')][.//span[contains(normalize-space(),'Seleccione las categor')]]"));

            dropdown.Click();
            Thread.Sleep(400);

            EscribirEnInputVisible(By.XPath("(//input[@placeholder='Buscar...'])[last()]"), categoria);

            var opcion = BuscarVisible(
                By.XPath($"//span[contains(@class,'option-label') and normalize-space()={XPathLiteral(categoria)}]"),
                By.XPath($"//*[contains(normalize-space(),{XPathLiteral(categoria)}) and not(self::input)]"),
                By.XPath("//span[contains(@class,'option-label') and contains(normalize-space(),'SIN CATEGOR')]"));

            opcion.Click();
            Thread.Sleep(300);
        }

        private void GuardarFamiliaPrecondicion()
        {
            var botonGuardar = BuscarVisible(
                By.XPath("//div[contains(@class,'modal')]//button[normalize-space()='Guardar']"),
                By.XPath("//button[normalize-space()='Guardar']"));

            ClickSeguro(botonGuardar);
            Thread.Sleep(800);
            CerrarMensajeSiExiste();
            Thread.Sleep(500);
        }

        private bool CategoriaFamiliaYaSeleccionada(string categoria)
        {
            return _driver.FindElements(By.XPath(
                    $"//span[normalize-space()={XPathLiteral(categoria)}] | " +
                    $"//div[contains(@class,'selected') and contains(normalize-space(),{XPathLiteral(categoria)})] | " +
                    "//span[contains(normalize-space(),'SIN CATEGOR')]"))
                .Any(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });
        }

        private void EscribirEnInputVisible(By locator, string texto)
        {
            var input = BuscarVisible(locator);
            input.Click();
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            input.SendKeys(texto);
            Thread.Sleep(300);
        }

        private IWebElement BuscarVisible(params By[] locators)
        {
            foreach (var locator in locators)
            {
                var elemento = _driver.FindElements(locator).FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

                if (elemento != null)
                    return elemento;
            }

            throw new NoSuchElementException("No se encontro un elemento visible para la precondicion de item comercial de ventas.");
        }

        private void ClickSeguro(IWebElement elemento)
        {
            try
            {
                elemento.Click();
            }
            catch (WebDriverException)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", elemento);
            }
        }

        private static string Valor(IDictionary<string, string> item, params string[] columnas)
        {
            return Valor(item, columnas, defaultValue: string.Empty);
        }

        private static string Valor(IDictionary<string, string> item, string columna, string? alterna = null, string? tercera = null, string defaultValue = "")
        {
            return Valor(item, new[] { columna, alterna, tercera }.Where(c => !string.IsNullOrWhiteSpace(c)).Cast<string>().ToArray(), defaultValue);
        }

        private static string Valor(IDictionary<string, string> item, string[] columnas, string defaultValue)
        {
            foreach (var columna in columnas)
            {
                if (item.ContainsKey(columna))
                    return item[columna]?.Trim() ?? defaultValue;
            }

            return defaultValue;
        }

        private bool ExisteFila(string xpath)
        {
            return _driver.FindElements(By.XPath(xpath)).Any(e =>
            {
                try { return e.Displayed; }
                catch { return false; }
            });
        }

        private bool ExisteElementoVisible(By locator)
        {
            return _driver.FindElements(locator).Any(e =>
            {
                try { return e.Displayed && e.Enabled; }
                catch { return false; }
            });
        }

        private bool EsRegistroExistente(Exception ex)
        {
            string texto = $"{ex.Message} {TextoAlertasVisibles()}".ToLowerInvariant();

            return texto.Contains("ya existe") ||
                   texto.Contains("ya se encuentra") ||
                   texto.Contains("duplic") ||
                   texto.Contains("registrado previamente") ||
                   texto.Contains("codigo de barra ya") ||
                   texto.Contains("codigo barra ya") ||
                   texto.Contains("nombre ya");
        }

        private string TextoAlertasVisibles()
        {
            try
            {
                return string.Join(" ", _driver.FindElements(By.XPath("//*[contains(@class,'swal') or contains(@class,'toast') or contains(@class,'alert')]"))
                    .Where(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    })
                    .Select(e => e.Text));
            }
            catch
            {
                return string.Empty;
            }
        }

        private void CerrarMensajeSiExiste()
        {
            foreach (var locator in new[]
            {
                By.XPath("//button[normalize-space()='OK' or normalize-space()='Ok' or normalize-space()='Aceptar']"),
                By.CssSelector(".swal2-confirm"),
                By.XPath("//button[contains(@class,'btn-close')]")
            })
            {
                try
                {
                    var boton = _driver.FindElements(locator).FirstOrDefault(e => e.Displayed && e.Enabled);
                    if (boton == null)
                        continue;

                    boton.Click();
                    Thread.Sleep(300);
                    return;
                }
                catch
                {
                }
            }
        }

        private void AbrirRegistrarDatosConceptoConStepsDeItems(
            NewItemStepDefinitions itemsSteps,
            RegisterItemDataStepDefinitions datosConceptoSteps)
        {
            AbrirSubmenuConceptosConRetry(
                itemsSteps,
                "Registrar Datos de Concepto",
                () => datosConceptoSteps.WhenElUsuarioSeleccionaRegistrarDatosDeConcepto());
        }

        private void AbrirNuevoConceptoConStepsDeItems(NewItemStepDefinitions itemsSteps)
        {
            AbrirSubmenuConceptosConRetry(
                itemsSteps,
                "Nuevo Concepto",
                () => itemsSteps.WhenElUsuarioSeleccionaNuevoConcepto());
        }

        private void AbrirVerConceptosConStepsDeItems(
            NewItemStepDefinitions itemsSteps,
            ViewItemStepDefinitions verConceptosSteps)
        {
            AbrirSubmenuConceptosConRetry(
                itemsSteps,
                "Ver Concepto",
                () => verConceptosSteps.WhenElUsuarioSeleccionaVerConceptos());
        }

        private void AbrirSubmenuConceptosConRetry(NewItemStepDefinitions itemsSteps, string submodulo, Action seleccionarSubmenu)
        {
            if (!SubmenuConceptosVisible(submodulo))
                itemsSteps.WhenElUsuarioAccedeAlModuloConceptos();

            try
            {
                seleccionarSubmenu();
                Thread.Sleep(800);
                return;
            }
            catch
            {
                CerrarMensajeSiExiste();
            }

            if (!SubmenuConceptosVisible(submodulo))
                itemsSteps.WhenElUsuarioAccedeAlModuloConceptos();

            seleccionarSubmenu();
            Thread.Sleep(800);
        }

        private bool SubmenuConceptosVisible(string submodulo)
        {
            return ExisteElementoVisible(By.XPath(
                $"//span[normalize-space()={XPathLiteral(submodulo)}] | //a[normalize-space()={XPathLiteral(submodulo)}]"));
        }

        private static string ConstruirCodigoFamilia(string familia)
        {
            return new string(familia.ToUpperInvariant().Where(char.IsLetterOrDigit).Take(8).ToArray());
        }

        private static string XPathLiteral(string value)
        {
            if (!value.Contains('\''))
                return $"'{value}'";

            if (!value.Contains('"'))
                return $"\"{value}\"";

            return "concat('" + value.Replace("'", "',\"'\",'") + "')";
        }
    }
}
