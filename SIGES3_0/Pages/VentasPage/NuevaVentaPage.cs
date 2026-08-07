using NUnit.Framework;
using SIGES3_0.Pages.Helpers;
using SIGES3_0.Pages.Adquisicion;
using SIGES3_0.Pages.Items.NewItem;
using SIGES3_0.Pages.Items.RegisterItemData;
using SIGES3_0.Pages.Items.ViewItems;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SIGES3_0.Pages.VentasPage
{
    public class NuevaVentaPage
    {
        private readonly IWebDriver driver;
        private readonly Utilities utilities;
        private readonly WebDriverWait wait;
        private bool _wasSaveEnabled = false;
        private bool _wasSaveExecuted = false;
        private string _lastObservedMessage = string.Empty;
        private string _lastObservedPaymentState = string.Empty;
        private DiscountContext _discountContext = DiscountContext.Empty;
        private PaymentContext _paymentContext = PaymentContext.Empty;
        private string _lastCreditInstallments = string.Empty;
        private string _conceptoTextoResueltoPrecondicion = string.Empty;
        private string _familiaPreparadaPrecondicion = string.Empty;
        private string _conceptoPreparadoPrecondicion = string.Empty;
        private PrecondicionConceptoVendibleConfig _configPrecondicionConceptoVendible = PrecondicionConceptoVendibleConfig.CreateDefault();
        private readonly List<string> _guiaCamposOmitidos = new();
        private bool _guiaConfirmadaAntesDeGuardar = false;
        private string _guiaEvidenciaConfirmacion = string.Empty;
        private static readonly By DiscountAmountModeLocator = By.XPath("//button[normalize-space()='$' or contains(normalize-space(),'Monto')] | //label[normalize-space()='$' or contains(normalize-space(),'Monto')]");
        private static readonly By DiscountPercentageModeLocator = By.XPath("//button[normalize-space()='%' or contains(normalize-space(),'Porcentaje')] | //label[normalize-space()='%' or contains(normalize-space(),'Porcentaje')]");
        private static readonly By DiscountValueInputLocator = By.XPath("//input[(@placeholder='0' or contains(@id,'discount') or contains(@formcontrolname,'discount')) and not(@type='hidden') and not(@type='checkbox') and not(@type='radio')]");

        public NuevaVentaPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(40));
        }

        // ─── MODO DE VENTA ────────────────────────────────────────────────────────────

        // Paso: selecciona el modo de venta (VENTA NORMAL / VENTA MODO CAJA / VENTA POR CONTINGENCIA)
        // Resetea el estado del escenario y espera que el formulario este listo.
        public void SelectSaleModeFlow(string modo)
        {
            _wasSaveEnabled = false;
            _wasSaveExecuted = false;
            _lastObservedMessage = string.Empty;
            _lastObservedPaymentState = string.Empty;
            _discountContext = DiscountContext.Empty;
            _paymentContext = PaymentContext.Empty;
            _lastCreditInstallments = string.Empty;
            _guiaCamposOmitidos.Clear();
            _guiaConfirmadaAntesDeGuardar = false;
            _guiaEvidenciaConfirmacion = string.Empty;

            WaitForFormReady();

            if (string.IsNullOrWhiteSpace(modo) || modo.Trim() == "-")
                return;

            Log($"Seleccionando modo de venta: {modo}");
            if (modo.Trim().Equals("VENTA NORMAL", StringComparison.OrdinalIgnoreCase) &&
                !ExisteElementoVisible(VentasLocators.NuevaVenta.ModoVenta(modo)) &&
                IsNewSaleFormReady())
            {
                Log("Modo VENTA NORMAL no visible; el formulario de Nueva Venta ya esta listo, se continua.");
                return;
            }

            Click(VentasLocators.NuevaVenta.ModoVenta(modo));
            Thread.Sleep(1000);
        }

        public void ConfigurarPrecondicionConceptoVendibleParaNuevaVenta(IDictionary<string, string> valores)
        {
            var config = PrecondicionConceptoVendibleConfig.CreateDefault();

            foreach (var entry in valores)
            {
                string campo = NormalizeText(entry.Key);
                string valor = entry.Value?.Trim() ?? string.Empty;

                if (EsValorOmitido(valor))
                    continue;

                switch (campo)
                {
                    case "tipo producto":
                    case "tipo de producto":
                    case "tipo familia":
                        config.TipoProducto = valor;
                        break;
                    case "tratamiento igv":
                    case "tratamiento igv familia":
                        config.TratamientoIgvFamilia = valor;
                        break;
                    case "categoria":
                    case "categoria familia":
                        config.CategoriaFamilia = valor;
                        break;
                    case "rol":
                    case "rol concepto":
                        config.RolConcepto = valor;
                        break;
                    case "modulo":
                    case "modulo concepto":
                        config.ModuloConcepto = valor;
                        break;
                    case "marca":
                    case "marca concepto":
                        config.MarcaConcepto = valor;
                        break;
                    case "sufijo":
                    case "sufijo concepto":
                    case "nombre concepto":
                        config.SufijoConcepto = valor;
                        break;
                    case "presentacion":
                    case "presentacion concepto":
                        config.PresentacionConcepto = valor;
                        break;
                    case "um comercial":
                    case "u.m. comercial":
                    case "unidad comercial":
                        config.UmComercialConcepto = valor;
                        break;
                    case "u medida":
                    case "u. medida":
                    case "unidad medida":
                    case "u medida concepto":
                        config.UMedidaConcepto = valor;
                        break;
                    case "cantidad base":
                    case "cantidad base concepto":
                        config.CantidadBaseConcepto = valor;
                        break;
                    case "tarifa":
                    case "tarifa concepto":
                        config.TarifaConcepto = valor;
                        break;
                    case "precio producto":
                    case "precio concepto":
                    case "precio venta":
                        config.PrecioProducto = valor;
                        break;
                    case "documento adquisicion":
                    case "documento compra":
                        config.DocumentoAdquisicion = valor;
                        break;
                    case "proveedor adquisicion":
                    case "proveedor":
                        config.ProveedorAdquisicion = valor;
                        break;
                    case "informacion adquisicion":
                    case "info adquisicion":
                    case "informacion adicional adquisicion":
                        config.InformacionAdquisicion = valor;
                        break;
                    case "tipo entrega adquisicion":
                    case "entrega adquisicion":
                        config.TipoEntregaAdquisicion = valor;
                        break;
                    case "rol adquisicion":
                        config.RolAdquisicion = valor;
                        break;
                    case "establecimiento adquisicion":
                    case "establecimiento":
                        config.EstablecimientoAdquisicion = valor;
                        break;
                    case "almacen adquisicion":
                    case "almacen":
                        config.AlmacenAdquisicion = valor;
                        break;
                    case "tipo pago adquisicion":
                        config.TipoPagoAdquisicion = valor;
                        break;
                    case "medio pago adquisicion":
                        config.MedioPagoAdquisicion = valor;
                        break;
                    case "observacion pago adquisicion":
                    case "observacion adquisicion":
                        config.ObservacionPagoAdquisicion = valor;
                        break;
                    case "precio compra":
                    case "precio compra adquisicion":
                    case "valor unitario adquisicion":
                        config.PrecioCompraAdquisicion = valor;
                        break;
                }
            }

            _configPrecondicionConceptoVendible = config;
            Log($"[PrecondicionNV] Configuracion aplicada: tipoProducto='{config.TipoProducto}', sufijoConcepto='{config.SufijoConcepto}', precioProducto='{config.PrecioProducto}', proveedorAdquisicion='{config.ProveedorAdquisicion}', precioCompra='{config.PrecioCompraAdquisicion}'.");
        }

        // Precondicion autonoma para escenarios de Ventas que necesitan concepto y stock.
        public void AsegurarConceptoVendibleParaNuevaVenta(string familia, string concepto, string stockMinimo)
        {
            Log($"[PrecondicionNV] Asegurando familia='{familia}', concepto='{concepto}', stockMinimo='{stockMinimo}'.");
            _conceptoTextoResueltoPrecondicion = string.Empty;
            _familiaPreparadaPrecondicion = familia.Trim();
            _conceptoPreparadoPrecondicion = concepto.Trim();

            if (ExisteConceptoDisponibleEnNuevaVenta(familia, concepto))
            {
                Log($"[PrecondicionNV][Items] El concepto '{concepto}' ya está disponible en Nueva Venta y se reutilizará.");
            }
            else if (ExisteConceptoEnVista(familia, concepto))
            {
                Log($"[PrecondicionNV][Items] El concepto '{concepto}' ya existe en Conceptos y se reutilizará.");
            }
            else
            {
                try
                {
                    AsegurarConceptoEnConceptos(familia, concepto, stockMinimo);
                }
                catch (AssertionException ex) when (ex.Message.Contains("No se pudo seleccionar la familia", StringComparison.OrdinalIgnoreCase))
                {
                    Log($"[PrecondicionNV] La familia '{familia}' no estuvo disponible en Nuevo Concepto. Se intentará crearla y reintentar el concepto.");
                    AsegurarFamiliaEnConceptos(familia);
                    AsegurarConceptoEnConceptos(familia, concepto, stockMinimo);
                }
            }

            if (TieneStockSuficienteParaPrecondicion(stockMinimo))
            {
                Log($"[PrecondicionNV][Adquisicion] Stock suficiente detectado para '{concepto}'. Se omite la adquisición.");
            }
            else
            {
                Log($"[PrecondicionNV][Adquisicion] No hay stock suficiente para '{concepto}'. Se ejecutará la adquisición.");
                AsegurarStockMedianteAdquisicion(concepto, stockMinimo);
            }

            VolverANuevaVentaDesdePrecondicion();
        }

        public void SeleccionarFamiliaPreparadaParaNuevaVenta()
        {
            Assert.That(string.IsNullOrWhiteSpace(_familiaPreparadaPrecondicion), Is.False,
                "No hay una familia preparada para Nueva Venta. Ejecuta primero la precondición del concepto vendible.");
            SeleccionarFamiliaNuevaVenta(_familiaPreparadaPrecondicion);
        }

        public void SeleccionarConceptoPreparadoParaNuevaVenta()
        {
            Assert.That(string.IsNullOrWhiteSpace(_conceptoPreparadoPrecondicion), Is.False,
                "No hay un concepto preparado para Nueva Venta. Ejecuta primero la precondición del concepto vendible.");
            SeleccionarConceptoNuevaVenta(_conceptoPreparadoPrecondicion);
        }

        // Paso: ingresa la fecha de emision (solo para Venta Contingencia)
        public void SetFechaEmisionFlow(string fecha)
        {
            if (string.IsNullOrWhiteSpace(fecha) || fecha.Trim() == "-")
                return;

            Log($"Ingresando fecha de emision: {fecha}");
            EstablecerFechaEnCampo(VentasLocators.NuevaVenta.FechaEmision, fecha);
        }

        // Paso: ingresa la fecha de crédito/primera cuota para ventas a crédito.
        public void SetFechaCreditoFlow(string fecha)
        {
            if (string.IsNullOrWhiteSpace(fecha) || fecha.Trim() == "-")
                return;

            Log($"Ingresando fecha de credito: {fecha}");
            AbrirPagoNuevaVenta();
            EstablecerFechaCreditoComoUsuario(VentasLocators.Payment.CreditDueDateInput, fecha);
        }

        // ─── DETALLE ─────────────────────────────────────────────────────────────────

        // Paso: configura IGV Y|N y Detalle Unificado Y|N
        public void ConfigurarIgvDetUnif(string igv, string detUnificado)
        {
            bool activarIgv = igv.Equals("Y", StringComparison.OrdinalIgnoreCase);
            bool activarDet = detUnificado.Equals("Y", StringComparison.OrdinalIgnoreCase);
            Log($"Configurando IGV={igv}, DetUnificado={detUnificado}");
            SetCheckbox(VentasLocators.NuevaVenta.IgvCheck, activarIgv);
            Thread.Sleep(500);
            SetCheckbox(VentasLocators.NuevaVenta.DetUnifCheck, activarDet);
            Thread.Sleep(500);
        }

        public void SeleccionarFamiliaNuevaVenta(string familia)
        {
            if (string.IsNullOrWhiteSpace(familia) || familia.Trim() == "-")
                return;

            SeleccionarDropdownCustomNuevaVenta(familia.Trim(), VentasLocators.Detail.FamilySelect);
            Thread.Sleep(700);
        }

        public void SeleccionarConceptoNuevaVenta(string concepto)
        {
            if (string.IsNullOrWhiteSpace(concepto) || concepto.Trim() == "-")
                return;

            SeleccionarDropdownCustomNuevaVenta(concepto.Trim(), VentasLocators.Detail.ConceptSelect);
            Thread.Sleep(700);
        }

        public void IngresarCantidadNuevaVenta(string cantidad)
        {
            if (string.IsNullOrWhiteSpace(cantidad) || cantidad.Trim() == "-" || cantidad.Trim() == "0")
                return;

            var input = driver.FindElements(VentasLocators.Detail.QuantityInputs)
                .LastOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            Assert.That(input, Is.Not.Null, "No se encontró el input de cantidad en Nueva Venta.");
            EstablecerValorInputNuevaVenta(input!, cantidad.Trim());
            Thread.Sleep(500);
        }

        private void EstablecerFechaEnCampo(By locator, string fechaTexto)
        {
            string valorVisual = ResolverFechaSoloDia(fechaTexto);
            var input = Find(locator);
            string tipoInput = (input.GetAttribute("type") ?? string.Empty).Trim();
            bool esCampoDate = tipoInput.Equals("date", StringComparison.OrdinalIgnoreCase);
            bool tieneFecha = TryResolverFechaSoloDia(fechaTexto, out DateTime fecha);
            string valor = esCampoDate && tieneFecha
                ? fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : valorVisual;

            ScrollToCenter(input);

            if (esCampoDate && tieneFecha)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "var el = arguments[0]; var val = arguments[1];" +
                    "el.removeAttribute('readonly');" +
                    "el.removeAttribute('disabled');" +
                    "var setter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;" +
                    "setter.call(el, val);" +
                    "el.dispatchEvent(new Event('input', { bubbles: true }));" +
                    "el.dispatchEvent(new Event('change', { bubbles: true }));" +
                    "el.dispatchEvent(new Event('blur', { bubbles: true }));",
                    input, valor);
                Thread.Sleep(200);
            }
            else
            {
                // Fallback JS para inputs que no son type="date" o sin fecha resuelta
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "var el = arguments[0]; var val = arguments[1];" +
                    "el.removeAttribute('readonly');" +
                    "el.removeAttribute('disabled');" +
                    "var setter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;" +
                    "setter.call(el, val);" +
                    "el.dispatchEvent(new Event('input', { bubbles: true }));" +
                    "el.dispatchEvent(new Event('change', { bubbles: true }));" +
                    "el.dispatchEvent(new Event('blur', { bubbles: true }));",
                    input, valor);
                Thread.Sleep(300);
            }

            // Clic en body para forzar el ciclo de change detection de Angular
            // y propagar el estado del FormControl al formulario padre (habilita/deshabilita Guardar)
            try { driver.FindElement(By.TagName("body")).Click(); } catch { /* no crítico */ }
            Thread.Sleep(300);

            string valorActual = input.GetAttribute("value") ?? string.Empty;
            if (esCampoDate && tieneFecha)
            {
                Assert.That(
                    valorActual,
                    Is.EqualTo(valor),
                    $"La fecha '{fechaTexto}' no quedo aplicada en el campo {locator}. Valor esperado: '{valor}'. Valor actual: '{valorActual}'.");
            }
            else
            {
                Assert.That(
                    valorActual,
                    Is.EqualTo(valor),
                    $"La fecha '{fechaTexto}' no quedo aplicada en el campo {locator}. Valor esperado: '{valor}'. Valor actual: '{valorActual}'.");
            }
            Log($"Fecha aplicada en campo {locator}: '{valorActual}'");
        }

        private void EstablecerFechaCreditoComoUsuario(By locator, string fechaTexto)
        {
            string valorVisual = ResolverFechaSoloDia(fechaTexto);
            bool tieneFecha = TryResolverFechaSoloDia(fechaTexto, out DateTime fecha);
            var input = Find(locator);
            string valorAEscribir = tieneFecha
                ? fecha.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                : SoloDigitos(valorVisual);
            if (string.IsNullOrWhiteSpace(valorAEscribir))
                valorAEscribir = valorVisual;

            ScrollToCenter(input);

            // Solo para fecha de credito: se escribe con teclado para que la mascara/date-picker
            // dispare la misma validacion que aparece cuando el usuario ingresa la fecha manualmente.
            try { input.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", input); }
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Backspace);
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            input.SendKeys(Keys.Home);
            input.SendKeys(Keys.Delete);
            input.SendKeys(Keys.End);
            input.SendKeys(Keys.Backspace);
            Thread.Sleep(150);
            input.SendKeys(valorAEscribir);
            input.SendKeys(Keys.Tab);
            Thread.Sleep(700);

            string valorActual = input.GetAttribute("value") ?? string.Empty;
            var validaciones = string.Join(" | ", CapturarValidacionesVisibles());
            if (!string.IsNullOrWhiteSpace(validaciones))
                Log($"Validacion visible despues de ingresar fecha de credito: '{validaciones}'");

            if (!FechaCreditoAplicadaCorrectamente(valorActual, fechaTexto, fecha, tieneFecha, valorVisual) &&
                string.IsNullOrWhiteSpace(validaciones))
            {
                Assert.Fail(
                    $"La fecha de credito '{fechaTexto}' no quedo aplicada como entrada de usuario. " +
                    $"Valor esperado: '{valorVisual}'. Valor escrito: '{valorAEscribir}'. Valor actual: '{valorActual}'.");
            }

            Log($"Fecha de credito aplicada con teclado en campo {locator}: '{valorActual}'");
        }

        private static string SoloDigitos(string valor) =>
            Regex.Replace(valor ?? string.Empty, "\\D", string.Empty);

        private static bool FechaCreditoAplicadaCorrectamente(
            string valorActual,
            string fechaTexto,
            DateTime fechaEsperada,
            bool tieneFecha,
            string valorVisual)
        {
            if (string.IsNullOrWhiteSpace(valorActual))
                return false;

            if (tieneFecha &&
                DateTime.TryParseExact(
                    valorActual.Trim(),
                    new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime fechaActual))
            {
                return fechaActual.Date == fechaEsperada.Date;
            }

            var esperado = tieneFecha
                ? fechaEsperada.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                : SoloDigitos(valorVisual);
            var actual = SoloDigitos(valorActual);

            return !string.IsNullOrWhiteSpace(actual) &&
                   (actual == esperado || actual == SoloDigitos(fechaTexto));
        }

        private void AsegurarFamiliaEnConceptos(string familia)
        {
            if (PuedeSeleccionarFamiliaEnNuevoConcepto(familia))
            {
                Log($"[PrecondicionNV] La familia '{familia}' ya existe y es seleccionable.");
                return;
            }

            var conceptosPage = new RegisterItemDataPage(driver);

            AbrirSubmoduloConceptos("Registrar Datos de Concepto");
            AbrirFormularioFamiliaEnItems();
            Thread.Sleep(1200);
            conceptosPage.MostrarTodasLasFamilias();

            if (ExisteFilaEnTabla($"//tbody/tr/td[normalize-space()='{familia}']"))
            {
                Log($"[PrecondicionNV] La familia '{familia}' ya existe.");
                return;
            }

            Log($"[PrecondicionNV] Creando familia '{familia}'.");
            try
            {
                conceptosPage.SeleccionarOpcionFamilia();
            }
            catch
            {
                AbrirFormularioFamiliaEnItems();
            }

            Assert.That(
                ExisteElementoEnDom(
                    By.XPath("//input[@id='tipoBien' or @id='tipoServicio']"),
                    By.XPath("//input[@placeholder='Código']"),
                    By.XPath("//input[@placeholder='Nombre']"),
                    By.XPath("//div[contains(@class,'select-trigger')][.//span[contains(normalize-space(),'Seleccione las categor')]]")),
                Is.True,
                "No se pudo abrir correctamente el formulario de Familia en Items desde la precondición de Ventas.");

            conceptosPage.SeleccionarTipo(_configPrecondicionConceptoVendible.TipoProducto);
            conceptosPage.SeleccionarTratamientoIGVDinamico(_configPrecondicionConceptoVendible.TratamientoIgvFamilia);
            conceptosPage.IngresarCodigoFamilia(ConstruirCodigoFamilia(familia));
            conceptosPage.IngresarNombreFamilia(familia);
            conceptosPage.SeleccionarCategoria(_configPrecondicionConceptoVendible.CategoriaFamilia);
            conceptosPage.GuardarRegistro();
            Thread.Sleep(1500);
        }

        private bool PuedeSeleccionarFamiliaEnNuevoConcepto(string familia)
        {
            try
            {
                AbrirNuevoConceptoEnItems();
                return IntentarSeleccionarFamiliaEnNuevoConcepto(familia);
            }
            catch
            {
                return false;
            }
        }

        private void AbrirFormularioFamiliaEnItems()
        {
            TryClickOptional(
                By.XPath("//button[normalize-space()='Familia']//i[contains(@class,'bi-house-door-fill')]/ancestor::button[1]"),
                By.XPath("//button[normalize-space()='Familia']"),
                By.XPath("//button[.//*[normalize-space()='Familia']]"),
                By.XPath("//*[self::button or self::a][contains(normalize-space(),'Familia')]")
            );
            Thread.Sleep(800);

            TryClickOptional(
                By.XPath("//button[@aria-controls='collapse-registro-familia']"),
                By.XPath("//button[contains(@aria-controls,'familia')]"),
                By.XPath("//button[contains(@class,'accordion-button')][contains(.,'Familia')]")
            );
            Thread.Sleep(800);
        }

        private void AsegurarConceptoEnConceptos(string familia, string concepto, string stockMinimo)
        {
            if (ExisteConceptoEnVista(familia, concepto))
            {
                Log($"[PrecondicionNV] El concepto '{concepto}' ya existe para la familia '{familia}'.");
                return;
            }

            Log($"[PrecondicionNV] Creando concepto '{concepto}' para la familia '{familia}'.");

            var newItemsPage = new NewItemsPage(driver);
            AbrirNuevoConceptoEnItems();
            SeleccionarFamiliaEnNuevoConcepto(familia);
            newItemsPage.IngresarCodigoDeBarra(concepto);
            newItemsPage.AgregarSufijo(ResolverSufijoConcepto(concepto));
            newItemsPage.SeleccionarUMComercial(_configPrecondicionConceptoVendible.UmComercialConcepto);
            newItemsPage.SeleccionarUMedida(_configPrecondicionConceptoVendible.UMedidaConcepto);
            newItemsPage.SeleccionarRol(_configPrecondicionConceptoVendible.RolConcepto);
            newItemsPage.SeleccionarModulo(_configPrecondicionConceptoVendible.ModuloConcepto);
            newItemsPage.SeleccionarMarca(_configPrecondicionConceptoVendible.MarcaConcepto);
            newItemsPage.SeleccionarPresentacion(_configPrecondicionConceptoVendible.PresentacionConcepto);
            newItemsPage.IngresarCantidad(_configPrecondicionConceptoVendible.CantidadBaseConcepto);
            newItemsPage.SeleccionarUnidadMedida(_configPrecondicionConceptoVendible.UMedidaConcepto);

            if (int.TryParse(stockMinimo, out int stockMinimoNumero) && stockMinimoNumero > 0)
                newItemsPage.IngresarStockMinimo(stockMinimoNumero.ToString(CultureInfo.InvariantCulture));

            newItemsPage.SeleccionarTarifa(_configPrecondicionConceptoVendible.TarifaConcepto);
            newItemsPage.IngresarPrecio(_configPrecondicionConceptoVendible.PrecioProducto);
            newItemsPage.GuardarConcepto();
            Thread.Sleep(1500);
        }

        private void AbrirNuevoConceptoEnItems()
        {
            if (IntentarAbrirNuevoConceptoDirecto())
                return;

            AbrirSubmoduloConceptos("Nuevo Concepto");
            Thread.Sleep(1500);
        }

        private bool IntentarAbrirNuevoConceptoDirecto()
        {
            string[] rutasCandidatas =
            {
                "/business-item/NewBusinessItem",
                "/business-item/RegisterBusinessItem",
                "/business-item/CreateBusinessItem"
            };

            foreach (var ruta in rutasCandidatas)
            {
                try
                {
                    NavegarARutaItems(ruta);
                    if (EsperarNuevoConceptoAbierto(8))
                        return true;
                }
                catch
                {
                }
            }

            return false;
        }

        private void SeleccionarFamiliaEnNuevoConcepto(string familia)
        {
            try
            {
                new NewItemsPage(driver).SeleccionarFamilia(familia);
                Thread.Sleep(700);
                return;
            }
            catch
            {
                Log($"[PrecondicionNV] No se pudo seleccionar familia '{familia}' con el flujo de Items. Se usará selector alternativo.");
            }

            bool seleccionada = IntentarSeleccionarFamiliaEnNuevoConcepto(familia);
            Assert.That(
                seleccionada,
                Is.True,
                $"No se pudo seleccionar la familia '{familia}' en Nuevo Concepto desde la precondición de Ventas.");
        }

        private bool IntentarSeleccionarFamiliaEnNuevoConcepto(string familia)
        {
            By dropdownFamilia = By.XPath(
                "//label[@for='familyId']/following-sibling::app-dropdown-search//div[contains(@class,'select-trigger')] | " +
                "//label[contains(normalize-space(),'Familia')]/following::app-dropdown-search[1]//div[contains(@class,'select-trigger')]");
            By buscadorFamilia = By.XPath(
                "(//app-dropdown-search//input[contains(@class,'search-input') or contains(@placeholder,'Buscar')])[last()]");
            By opcionFamilia = By.XPath(
                $"//span[contains(@class,'option-label') and normalize-space()='{familia}'] | //a[normalize-space()='{familia}']");

            var trigger = driver.FindElements(dropdownFamilia).FirstOrDefault(e =>
            {
                try { return e.Displayed && e.Enabled; }
                catch { return false; }
            });

            if (trigger == null)
                return false;

            ScrollToCenter(trigger);
            trigger.Click();
            Thread.Sleep(700);

            var buscador = driver.FindElements(buscadorFamilia).LastOrDefault(e =>
            {
                try { return e.Displayed && e.Enabled; }
                catch { return false; }
            });

            if (buscador == null)
                return false;

            buscador.SendKeys(Keys.Control + "a");
            buscador.SendKeys(Keys.Delete);
            buscador.SendKeys(familia);
            Thread.Sleep(900);

            var opcion = driver.FindElements(opcionFamilia).FirstOrDefault(e =>
            {
                try { return e.Displayed && e.Enabled; }
                catch { return false; }
            });

            if (opcion == null)
                return false;

            ScrollToCenter(opcion);
            opcion.Click();
            Thread.Sleep(700);
            return true;
        }

        private bool ExisteConceptoEnVista(string familia, string concepto)
        {
            var viewItemsPage = new ViewItemsPage(driver);

            try
            {
                AbrirSubmoduloConceptos("Ver Conceptos");
                viewItemsPage.SeleccionarFamilia(familia);
                viewItemsPage.IngresarPalabraClave(concepto);
                viewItemsPage.HacerBusqueda();
                Thread.Sleep(1200);

                return ExisteFilaEnTabla($"//tbody/tr[.//td[contains(normalize-space(),'{concepto}')]]");
            }
            catch (NoSuchElementException)
            {
                Log($"[PrecondicionNV] La familia '{familia}' aún no existe en Ver Conceptos.");
                return false;
            }
            catch (AssertionException ex)
            {
                Log($"[PrecondicionNV] No se pudo validar existencia en Ver Conceptos. Se intentará crear el concepto. Detalle: {ex.Message}");
                return false;
            }
            catch (WebDriverException ex)
            {
                Log($"[PrecondicionNV] Ver Conceptos no estuvo disponible. Se intentará crear el concepto. Detalle: {ex.Message}");
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Log($"[PrecondicionNV] Ver Conceptos no estuvo disponible. Se intentará crear el concepto. Detalle: {ex.Message}");
                return false;
            }
        }

        private bool ExisteConceptoDisponibleEnNuevaVenta(string familia, string concepto)
        {
            try
            {
                WaitForFormReady();
                SeleccionarFamiliaNuevaVenta(familia);

                var trigger = Find(VentasLocators.Detail.ConceptSelect);
                ScrollToCenter(trigger);
                trigger.Click();
                Thread.Sleep(500);

                var inputBusqueda = driver.FindElements(VentasLocators.Payment.DropdownSearchInput)
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed && e.Enabled; }
                        catch { return false; }
                    });

                if (inputBusqueda != null)
                {
                    inputBusqueda.SendKeys(Keys.Control + "a");
                    inputBusqueda.SendKeys(Keys.Delete);
                    inputBusqueda.SendKeys(concepto);
                    Thread.Sleep(700);
                }

                var opcionAmplia = BuscarOpcionVisibleNuevaVenta(concepto);
                var opcionEspecifica = BuscarOpcionConceptoEnDropdownNuevaVenta(concepto);

                if (opcionEspecifica != null)
                    _conceptoTextoResueltoPrecondicion = (opcionEspecifica.Text ?? string.Empty).Trim();
                else if (opcionAmplia != null)
                    _conceptoTextoResueltoPrecondicion = ExtraerTextoConceptoDesdeBloque(opcionAmplia.Text, concepto);

                if (inputBusqueda != null)
                    inputBusqueda.SendKeys(Keys.Escape);
                else
                    trigger.SendKeys(Keys.Escape);

                Thread.Sleep(400);
                return opcionAmplia != null;
            }
            catch
            {
                return false;
            }
        }

        private void AsegurarStockMedianteAdquisicion(string concepto, string stockMinimo)
        {
            for (int intento = 1; intento <= 2; intento++)
            {
                try
                {
                    EjecutarAdquisicionDePrecondicion(concepto, stockMinimo);
                    return;
                }
                catch (StaleElementReferenceException) when (intento < 2)
                {
                    Log($"[PrecondicionNV] Reintentando adquisición por stale element (intento {intento + 1}/2).");
                    Thread.Sleep(1500);
                }
            }
        }

        private void EjecutarAdquisicionDePrecondicion(string concepto, string stockMinimo)
        {
            string cantidad = ResolverCantidadStock(stockMinimo);
            string fechaHoy = DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            string productoParaAdquisicion = ResolverTextoProductoParaAdquisicion(concepto);
            var adquisicionPage = new NuevaAdquisicionPage(driver);
            string informacionAdquisicion = ResolverTextoPlantilla(_configPrecondicionConceptoVendible.InformacionAdquisicion, concepto);
            string observacionPago = ResolverTextoPlantilla(_configPrecondicionConceptoVendible.ObservacionPagoAdquisicion, concepto);

            Log($"[PrecondicionNV] Asegurando stock via Adquisicion para '{concepto}' con cantidad '{cantidad}'.");

            adquisicionPage.NavegarANuevaAdquisicion();
            adquisicionPage.ConfigurarDatosFacturacion(
                _configPrecondicionConceptoVendible.DocumentoAdquisicion,
                string.Empty,
                string.Empty,
                fechaHoy,
                _configPrecondicionConceptoVendible.ProveedorAdquisicion,
                informacionAdquisicion);
            adquisicionPage.SeleccionarTipoEntrega(_configPrecondicionConceptoVendible.TipoEntregaAdquisicion);
            adquisicionPage.ConfigurarDatosEntrega(
                _configPrecondicionConceptoVendible.RolAdquisicion,
                _configPrecondicionConceptoVendible.EstablecimientoAdquisicion,
                _configPrecondicionConceptoVendible.AlmacenAdquisicion);
            AgregarProductoEnAdquisicion(productoParaAdquisicion, cantidad, _configPrecondicionConceptoVendible.PrecioCompraAdquisicion);
            adquisicionPage.AbrirSeccionPago();
            adquisicionPage.SeleccionarTipoPago(_configPrecondicionConceptoVendible.TipoPagoAdquisicion);
            adquisicionPage.ConfigurarMedioPago(_configPrecondicionConceptoVendible.MedioPagoAdquisicion, observacionPago);
            adquisicionPage.ClicGuardarAdquisicion("SavePurchase");

            string mensaje = adquisicionPage.ObtenerMensajeConfirmacion();
            string mensajeNormalizado = NormalizeText(mensaje);
            Assert.That(
                mensajeNormalizado,
                Does.Contain("se registro correctamente").Or.Contain("se registró correctamente"),
                $"La adquisición de precondición no confirmó éxito. Mensaje actual: '{mensaje}'.");
        }

        private void AgregarProductoEnAdquisicion(string producto, string cantidad, string valorUnitario)
        {
            By comboConcepto = By.XPath("//label[@for='conceptSelect']/following-sibling::div//div[contains(@class, 'select-trigger')]");
            By buscador = By.XPath("(//input[@placeholder='Buscar...'])[last()]");
            By txtCantidad = By.XPath("//tbody/tr[contains(@class,'ng-star-inserted')]/td[3]//input");
            By txtValorUnitario = By.XPath("//tbody/tr[contains(@class,'ng-star-inserted')]/td[4]//input");
            string terminoBusqueda = ConstruirTerminoBusquedaProductoAdquisicion(producto);
            string xpathOpcionProducto = ConstruirXPathProductoAdquisicion(producto);

            Click(comboConcepto);
            Thread.Sleep(600);

            var inputBusqueda = Find(buscador);
            inputBusqueda.SendKeys(Keys.Control + "a");
            inputBusqueda.SendKeys(Keys.Delete);
            inputBusqueda.SendKeys(terminoBusqueda);
            Thread.Sleep(1000);

            By opcionProducto = By.XPath(xpathOpcionProducto);

            Click(opcionProducto);
            Thread.Sleep(1800);

            var cantidadInput = driver.FindElements(txtCantidad).LastOrDefault(e => e.Displayed);
            var valorInput = driver.FindElements(txtValorUnitario).LastOrDefault(e => e.Displayed);

            Assert.That(cantidadInput, Is.Not.Null, $"No se encontró la fila de cantidad para el producto '{producto}' en Adquisición.");
            Assert.That(valorInput, Is.Not.Null, $"No se encontró la fila de valor unitario para el producto '{producto}' en Adquisición.");

            cantidadInput!.Clear();
            cantidadInput.SendKeys(cantidad);
            Thread.Sleep(300);

            valorInput!.Clear();
            valorInput.SendKeys(valorUnitario);
            Thread.Sleep(700);
        }

        private void VolverANuevaVentaDesdePrecondicion()
        {
            bool yaEnNuevaVenta = driver.FindElements(VentasLocators.NuevaVenta.IgvCheck).Any(e =>
            {
                try { return e.Displayed; }
                catch { return false; }
            });

            if (yaEnNuevaVenta)
            {
                try
                {
                    driver.SwitchTo().ActiveElement().SendKeys(Keys.Escape);
                }
                catch
                {
                }

                Thread.Sleep(400);
                WaitForFormReady();
                return;
            }

            try
            {
                Click(By.XPath("//span[normalize-space()='Ventas']"));
                Thread.Sleep(1000);
                Click(By.XPath("//span[normalize-space()='Nueva Venta']"));
                Thread.Sleep(1800);
            }
            catch
            {
                // Si el sidebar no está accesible, reutilizamos la navegación directa del helper.
            }

            WaitForFormReady();
        }

        private static string ConstruirCodigoFamilia(string familia)
        {
            string baseTexto = NormalizeText(familia)
                .Replace(" ", string.Empty)
                .ToUpperInvariant();

            if (baseTexto.Length > 8)
                baseTexto = baseTexto[..8];

            return $"QA-{baseTexto}";
        }

        private static string ConstruirSufijoConcepto(string concepto)
        {
            string cola = concepto.Trim();
            if (cola.Length > 6)
                cola = cola[^6..];

            return $"QA-{cola}";
        }

        private string ResolverSufijoConcepto(string concepto)
        {
            if (string.IsNullOrWhiteSpace(_configPrecondicionConceptoVendible.SufijoConcepto))
                return ConstruirSufijoConcepto(concepto);

            return ResolverTextoPlantilla(_configPrecondicionConceptoVendible.SufijoConcepto, concepto);
        }

        private static string ResolverCantidadStock(string stockMinimo)
        {
            if (int.TryParse(stockMinimo, out int numero) && numero > 0)
                return numero.ToString(CultureInfo.InvariantCulture);

            return "5";
        }

        private bool TieneStockSuficienteParaPrecondicion(string stockMinimo)
        {
            if (string.IsNullOrWhiteSpace(_conceptoTextoResueltoPrecondicion))
                return false;

            int? stockActual = ExtraerStockDisponibleDesdeTextoConcepto();

            if (!stockActual.HasValue)
            {
                Log($"[PrecondicionNV] No se pudo determinar el stock actual desde '{_conceptoTextoResueltoPrecondicion}'.");
                return false;
            }

            if (!int.TryParse(stockMinimo, out int minimo) || minimo <= 0)
                return stockActual.Value > 0;

            Log($"[PrecondicionNV] Stock actual detectado={stockActual.Value}, stock mínimo requerido={minimo}.");
            return stockActual.Value >= minimo;
        }

        private int? ExtraerStockDisponibleDesdeTextoConcepto()
        {
            if (string.IsNullOrWhiteSpace(_conceptoTextoResueltoPrecondicion))
                return null;

            var match = Regex.Match(_conceptoTextoResueltoPrecondicion, @"stock\s*:\s*(?<stock>\d+)", RegexOptions.IgnoreCase);
            if (!match.Success)
                return null;

            return int.TryParse(match.Groups["stock"].Value, out int stock) ? stock : null;
        }

        private string ResolverTextoProductoParaAdquisicion(string concepto)
        {
            if (!string.IsNullOrWhiteSpace(_conceptoTextoResueltoPrecondicion))
                return NormalizarTextoProductoParaAdquisicion(_conceptoTextoResueltoPrecondicion);

            if (concepto.Contains("|", StringComparison.Ordinal))
                return NormalizarTextoProductoParaAdquisicion(concepto);

            return concepto.Trim();
        }

        private static string NormalizarTextoProductoParaAdquisicion(string textoProducto)
        {
            if (string.IsNullOrWhiteSpace(textoProducto))
                return string.Empty;

            var partes = textoProducto
                .Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(parte =>
                    !NormalizeText(parte).StartsWith("stock:", StringComparison.OrdinalIgnoreCase) &&
                    !NormalizeText(parte).StartsWith("pub:", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (partes.Count == 0)
                return textoProducto.Trim();

            return string.Join(" | ", partes);
        }

        private static string ConstruirTerminoBusquedaProductoAdquisicion(string producto)
        {
            var partes = producto
                .Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

            if (partes.Count >= 1)
                return partes[0];

            return producto;
        }

        private static string ConstruirXPathProductoAdquisicion(string producto)
        {
            var partes = producto
                .Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

            var relevantes = partes.Count >= 2 ? partes.Take(2).ToList() : partes;
            if (relevantes.Count == 0)
                relevantes.Add(producto);

            string condiciones = string.Join(
                " and ",
                relevantes.Select(parte => $"contains(normalize-space(),'{parte.Replace("'", "&apos;")}')"));
            string codigo = relevantes[0].Replace("'", "&apos;");

            return $"//span[{condiciones}] | //div[contains(@class,'option-item')][{condiciones}] | //a[{condiciones}] | " +
                   $"//span[contains(normalize-space(),'{codigo}')] | " +
                   $"//div[contains(@class,'option-item')][contains(normalize-space(),'{codigo}')] | " +
                   $"//a[contains(normalize-space(),'{codigo}')]";
        }

        private static string ResolverTextoPlantilla(string plantilla, string concepto)
        {
            if (string.IsNullOrWhiteSpace(plantilla))
                return string.Empty;

            return plantilla.Replace("{concepto}", concepto, StringComparison.OrdinalIgnoreCase);
        }

        private bool ExisteFilaEnTabla(string xpath)
        {
            return driver.FindElements(By.XPath(xpath)).Any(e =>
            {
                try { return e.Displayed; }
                catch { return false; }
            });
        }

        private bool ExisteElementoVisible(By locator)
        {
            return driver.FindElements(locator).Any(e =>
            {
                try { return e.Displayed; }
                catch { return false; }
            });
        }

        private bool ExisteElementoEnDom(params By[] locators)
        {
            return locators.Any(locator => driver.FindElements(locator).Count > 0);
        }

        private void AbrirSubmoduloConceptos(string submodulo)
        {
            string submoduloNormalizado = NormalizeText(submodulo);

            if (submoduloNormalizado == "ver conceptos")
            {
                AbrirVistaConceptosEnItems();
                return;
            }

            AbrirModuloConceptosEnSidebar();

            if (submoduloNormalizado == "registrar datos de concepto")
            {
                Click(By.XPath("//span[normalize-space()='Registrar Datos de Concepto']"));
                Thread.Sleep(1500);

                Assert.That(
                    ExisteElementoVisible(By.XPath("//button[normalize-space()='Familia']")),
                    Is.True,
                    "No se pudo abrir 'Registrar Datos de Concepto' desde la precondición de Nueva Venta.");
                return;
            }

            if (submoduloNormalizado == "nuevo concepto")
            {
                Click(
                    By.XPath("//a[.//span[normalize-space()='Nuevo Concepto']]"),
                    By.XPath("//a[contains(@href,'BusinessItem') and contains(normalize-space(),'Nuevo Concepto')]"),
                    By.XPath("//span[normalize-space()='Nuevo Concepto']")
                );

                Assert.That(
                    EsperarNuevoConceptoAbierto(10),
                    Is.True,
                    "No se pudo abrir 'Nuevo Concepto' desde la precondición de Nueva Venta.");
                return;
            }

            Click(
                By.XPath($"//a[.//span[normalize-space()='{submodulo}']]"),
                By.XPath($"//span[normalize-space()='{submodulo}']")
            );
            Thread.Sleep(1500);
        }

        private bool EsperarNuevoConceptoAbierto(int timeoutSeconds)
        {
            By formularioNuevoConcepto = By.XPath(
                "//input[contains(@placeholder,'Código') or contains(@placeholder,'Codigo') or " +
                "contains(@placeholder,'Barra') or contains(@placeholder,'barra') or " +
                "@id='autoBarcodeChk' or contains(@formcontrolname,'barcode') or contains(@formcontrolname,'barCode')]");

            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(250)
                }.Until(_ => ExisteElementoVisible(formularioNuevoConcepto));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void AbrirVistaConceptosEnItems()
        {
            try
            {
                NavegarARutaItems("/business-item/ViewBusinessItem");
                Thread.Sleep(1500);
            }
            catch
            {
                AbrirModuloConceptosEnSidebar();
                Click(
                    By.XPath("//a[@href='/business-item/ViewBusinessItem']"),
                    By.XPath("//span[normalize-space()='Ver Conceptos']")
                );
                Thread.Sleep(1500);
            }

            if (!ExisteElementoVisible(By.XPath("//input[@placeholder='Ingrese palabras claves']")))
                throw new InvalidOperationException("No se pudo abrir 'Ver Conceptos' desde la precondición de Nueva Venta.");
        }

        private void AbrirModuloConceptosEnSidebar()
        {
            TryClickOptional(
                By.XPath("//a[.//span[normalize-space()='Conceptos']]"),
                By.XPath("//span[normalize-space()='Conceptos']"),
                By.XPath("//span[text()='Conceptos']/following::input[1]")
            );
            Thread.Sleep(1200);
        }

        private void NavegarARutaItems(string rutaRelativa)
        {
            var actual = new Uri(driver.Url);
            var destino = new Uri(actual, rutaRelativa);
            driver.Navigate().GoToUrl(destino);
        }

        private static string ResolverFechaSoloDia(string fechaTexto)
        {
            if (TryResolverFechaSoloDia(fechaTexto, out DateTime fecha))
                return fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            return fechaTexto.Trim();
        }

        private static bool TryResolverFechaSoloDia(string fechaTexto, out DateTime fecha)
        {
            fecha = default;

            if (string.IsNullOrWhiteSpace(fechaTexto))
                return false;

            string valor = fechaTexto.Trim();
            string normalizado = NormalizeText(valor);
            DateTime hoy = DateTime.Today;

            if (normalizado == "hoy")
            {
                fecha = hoy;
                return true;
            }

            if (normalizado == "ayer")
            {
                fecha = hoy.AddDays(-1);
                return true;
            }

            if (normalizado == "manana")
            {
                fecha = hoy.AddDays(1);
                return true;
            }

            var match = Regex.Match(normalizado, @"^hace\s+(?<dias>\d+)\s+dia(s)?$");
            if (match.Success && int.TryParse(match.Groups["dias"].Value, out int dias))
            {
                fecha = hoy.AddDays(-dias);
                return true;
            }

            return DateTime.TryParseExact(
                valor,
                new[] { "dd/MM/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/M/yyyy", "yyyy-MM-dd" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fecha);
        }

        // ─── FACTURACIÓN ─────────────────────────────────────────────────────────────

        // Paso: selecciona el punto de venta (solo para Venta Modo Caja)
        public void SelectPuntoVentaFlow(string puntoVenta)
        {
            if (string.IsNullOrWhiteSpace(puntoVenta) || puntoVenta.Trim() == "-")
                return;

            Log($"Seleccionando punto de venta: {puntoVenta}");
            AbrirSeccionFacturacionSiNecesario();
            SeleccionarDropdownCustomNuevaVenta(puntoVenta.Trim(), VentasLocators.NuevaVenta.PuntoVentaChevron);
            Thread.Sleep(800);
        }

        // Paso: selecciona el vendedor (solo para Venta Modo Caja)
        public void SelectVendorFlow(string vendedor)
        {
            if (string.IsNullOrWhiteSpace(vendedor) || vendedor.Trim() == "-")
                return;

            Log($"Seleccionando vendedor: {vendedor}");
            AbrirSeccionFacturacionSiNecesario();
            SeleccionarDropdownCustomNuevaVenta(vendedor.Trim(), VentasLocators.NuevaVenta.VendedorChevron);
            Thread.Sleep(800);
        }

        // Paso: busca cliente, selecciona comprobante y serie en la seccion Facturacion.
        public void ConfigurarFacturacionNuevaVenta(string comprobante, string serie, string cliente)
        {
            AbrirSeccionFacturacionSiNecesario();
            BuscarClienteNuevaVenta(cliente);
            SeleccionarComprobanteNuevaVenta(comprobante);

            // Pausa breve para que Angular renderice un posible popup de error post-seleccion.
            Thread.Sleep(200);
            var popup = CaptureVisibleMessage(2);
            if (!string.IsNullOrWhiteSpace(popup) && IsBlockingMessage(popup) && string.IsNullOrWhiteSpace(_lastObservedMessage))
            {
                Log($"Popup bloqueante en Facturacion: {popup}");
                _lastObservedMessage = popup;
                TryClickOptional(
                    VentasLocators.NuevaVenta.ErrorOkButton,
                    VentasLocators.NuevaVenta.ErrorOkButtonFallback,
                    By.CssSelector(".ok-button")
                );
                return;
            }

            SeleccionarSerieNuevaVenta(serie);
        }

        // ─── ENTREGA ─────────────────────────────────────────────────────────────────

        // Paso: abre el acordeon Entrega, selecciona el tipo (Inmediata/Diferida) y abre Guia de remision si aplica.
        public void ConfigurarEntregaNuevaVenta(string entrega, string guiaRemision)
        {
            Log($"Configurando entrega: tipo='{entrega}', guia='{guiaRemision}'");

            // 1. Abrir acordeon Entrega si los radios aun no son visibles
            bool radiosVisible = driver.FindElements(VentasLocators.Delivery.ImmediateLabel)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (!radiosVisible)
            {
                Log("Abriendo seccion Entrega...");
                TryClickOptional(
                    VentasLocators.NuevaVenta.AccordionEntrega,
                    VentasLocators.NuevaVenta.AccordionEntregaFallback1
                );
                Thread.Sleep(800);
            }

            // 2. Seleccionar tipo de entrega
            if (entrega.Trim().Equals("Inmediata", StringComparison.OrdinalIgnoreCase))
                TryClickOptional(VentasLocators.Delivery.ImmediateLabel, VentasLocators.Delivery.Immediate);
            else if (entrega.Trim().Equals("Diferida", StringComparison.OrdinalIgnoreCase))
                TryClickOptional(VentasLocators.NuevaVenta.EntregaDiferida, VentasLocators.Delivery.DeferredLabel);
            Thread.Sleep(500);

            // 3. Si GuiaRemision = false, no hay nada mas que hacer
            if (!guiaRemision.Trim().Equals("true", StringComparison.OrdinalIgnoreCase)) return;

            // 4. Buscar boton "Guia de remision". La estructura del DOM en NuevaVenta puede diferir
            //    de VerPedidos (donde el boton esta en //div[@id='collapse-entrega']).
            //    Se intenta con locators progresivamente mas amplios para mayor robustez.
            Thread.Sleep(500);
            IWebElement? btnGuia = null;
            try
            {
                var shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                btnGuia = shortWait.Until(d =>
                {
                    // Intento 1: dentro de #collapse-entrega (estructura VerPedidos)
                    var b = d.FindElements(By.XPath("//div[@id='collapse-entrega']//button[contains(normalize-space(),'remi')]"))
                             .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
                    if (b != null) return b;
            // Intento 2: cualquier <button> visible que contenga 'remi' (sin restriccion de contenedor)
                    b = d.FindElements(By.XPath("//button[contains(normalize-space(),'remi')]"))
                         .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
                    if (b != null) return b;
                    // Intento 3: <a> o <div class='btn'> con 'remi' (Angular puede renderizar botones como otros elementos)
                    return d.FindElements(By.XPath("//*[self::a or (self::div and contains(@class,'btn'))][contains(normalize-space(),'remi')]"))
                             .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
                });
            }
            catch { btnGuia = null; }

            if (btnGuia == null)
            {
                // Diagnostico: listar todos los botones visibles para identificar el locator correcto
                Log("=== DIAGNOSTICO: botones visibles en pagina ===");
                foreach (var b in driver.FindElements(By.XPath("//button | //a[contains(@class,'btn')]"))
                    .Where(e => { try { return e.Displayed; } catch { return false; } }))
                {
                    try { Log($"  ELEM: <{b.TagName}> text='{b.Text?.Trim()}' class='{b.GetAttribute("class")}' id='{b.GetAttribute("id")}'"); }
                    catch { }
                }
                Log("=== FIN DIAGNOSTICO ===");
                Log("Boton 'Guia de remision' no encontrado.");
                _lastObservedMessage = "Boton de guia de remision no encontrado";
                return;
            }

            bool deshabilitado = !btnGuia.Enabled
                || btnGuia.GetAttribute("disabled") != null
                || (btnGuia.GetAttribute("class") ?? "").Contains("disabled")
                || !btnGuia.GetCssValue("pointer-events").Equals("auto", StringComparison.OrdinalIgnoreCase);

            if (deshabilitado)
            {
                    Log("Boton 'Guia de remision' deshabilitado - cliente sin RUC/DNI.");
                _lastObservedMessage = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                return;
            }

            ScrollToCenter(btnGuia);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuia);
            Thread.Sleep(1200);
            Log("'Guia de remision' abierta correctamente.");
        }

        // ─── GUÍA DE REMISIÓN (NuevaVenta) ───────────────────────────────────────────

        // GuiaRemisionPage.txtPesoBruto/txtNumeroBultos usan clases Bootstrap (g-2 mb-3)
        // que no existen en el formulario de NuevaVenta; se anclan al label en su lugar.
        public void IngresarPesoBrutoNV(string valor)
        {
            if (EsValorOmitible(valor))
            {
                RegistrarCampoGuiaOmitido("Peso bruto");
                return;
            }

            QuitarCampoGuiaOmitido("Peso bruto");
            EscribirCampoGuia(By.XPath(
                "//label[contains(normalize-space(),'Peso') or contains(normalize-space(),'PESO')]" +
                "/following::input[not(@type='hidden') and not(@type='date')][1]"), valor.Trim());
        }

        public void IngresarNumeroBultosNV(string valor)
        {
            if (EsValorOmitible(valor))
            {
                RegistrarCampoGuiaOmitido("Numero de bultos");
                return;
            }

            QuitarCampoGuiaOmitido("Numero de bultos");
            EscribirCampoGuia(By.XPath(
                "//label[contains(normalize-space(),'Bulto') or contains(normalize-space(),'BULTO')]" +
                "/following::input[not(@type='hidden') and not(@type='date')][1]"), valor.Trim());
        }

        private void RegistrarCampoGuiaOmitido(string campo)
        {
            if (!_guiaCamposOmitidos.Any(c => string.Equals(c, campo, StringComparison.OrdinalIgnoreCase)))
                _guiaCamposOmitidos.Add(campo);
        }

        private void QuitarCampoGuiaOmitido(string campo)
        {
            _guiaCamposOmitidos.RemoveAll(c => string.Equals(c, campo, StringComparison.OrdinalIgnoreCase));
        }

        private static bool EsValorOmitible(string valor) =>
            string.IsNullOrWhiteSpace(valor) ||
            valor.Trim().Equals("NA", StringComparison.OrdinalIgnoreCase);

        private void EscribirCampoGuia(By locator, string valor)
        {
            var el = Find(locator);
            ScrollToCenter(el);
            try { el.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", el); }
            el.SendKeys(Keys.Control + "a");
            el.SendKeys(Keys.Delete);
            Thread.Sleep(150);
            el.SendKeys(valor);
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                arguments[0].blur();
            ", el);
            el.SendKeys(Keys.Tab);
            Thread.Sleep(400);
        }

        // ─── PAGO, GUARDAR Y VALIDAR ──────────────────────────────────────────────────
        // Then: valida el resultado de venta contra la tabla de decision
        public void ValidarResultadoVenta(string resultadoEsperado)
        {
            if (string.IsNullOrWhiteSpace(resultadoEsperado) || resultadoEsperado.Trim() == "-")
                return;

            var norm = NormalizeText(resultadoEsperado);
            bool habiaGuiaPendiente = IsGuideModalVisible();
            if (habiaGuiaPendiente)
                TryResolvePendingGuideModal("Validacion guia");

            if (norm.Contains("guia emitida correctamente"))
            {
                ValidarEvidenciaGuiaEmitida();
            }
            else if (EsResultadoExitosoVenta(norm))
            {
                if (habiaGuiaPendiente)
                {
                    Assert.That(_guiaConfirmadaAntesDeGuardar, Is.True,
                        $"No se pudo confirmar la guia antes de guardar la venta. Mensaje capturado: '{_lastObservedMessage}'.");

                    if (!_wasSaveExecuted)
                    {
                        ConfigurePaymentFlow("Contado");
                        GuardarVentaFlow();
                    }
                }

                Assert.That(_wasSaveEnabled, Is.True,
                    $"Guardar deberia estar habilitado (venta exitosa). Mensaje capturado: '{_lastObservedMessage}'");
                Assert.That(_wasSaveExecuted, Is.True,
                    "El guardado deberia haberse ejecutado.");

                Assert.That(EsMensajeExitoVenta(_lastObservedMessage), Is.True,
                    $"Mensaje de exito no encontrado. Actual: '{_lastObservedMessage}'. " +
                    $"Textos visibles de resultado: '{CapturarDiagnosticoMensajesResultado()}'.");
            }
            else
            {
                Assert.That(_wasSaveEnabled, Is.False,
                    $"Guardar deberia estar deshabilitado. Resultado esperado: '{resultadoEsperado}'. Mensaje capturado: '{_lastObservedMessage}'");
                ValidarMensajeDeResultadoNoExitosoNuevaVenta(norm, _lastObservedMessage);
                Log($"Resultado esperado no exitoso validado: esperado='{resultadoEsperado}', capturado='{_lastObservedMessage}'");
            }

            TryCloseSuccessDialog();
        }

        private static bool EsResultadoExitosoVenta(string resultadoEsperadoNormalizado) =>
            resultadoEsperadoNormalizado.Contains("guarda exitosamente") ||
            resultadoEsperadoNormalizado.Contains("guia emitida correctamente");

        private void ValidarEvidenciaGuiaEmitida()
        {
            string evidencia = ConstruirEvidenciaGuiaCompacta(_lastObservedMessage, _guiaEvidenciaConfirmacion);

            Assert.That(_guiaConfirmadaAntesDeGuardar, Is.True,
                $"No se valido la guia antes de guardar la venta. Evidencia capturada: '{evidencia}'.");
            Assert.That(string.IsNullOrWhiteSpace(evidencia), Is.False,
                "La guia se marco como confirmada, pero no se capturo mensaje ni resumen de evidencia.");

            Log($"Guia emitida validada antes de guardar venta. {evidencia}");
        }

        private static bool EsMensajeConfirmacionGuia(string? mensaje)
        {
            var normalizado = NormalizeText(mensaje ?? string.Empty);
            if (string.IsNullOrWhiteSpace(normalizado))
                return false;

            return (normalizado.Contains("guia") && (normalizado.Contains("emit") || normalizado.Contains("gener"))) ||
                   normalizado.Contains("registr") ||
                   normalizado.Contains("correct") ||
                   normalizado.Contains("complet");
        }

        private bool IsGuideSummaryVisible()
        {
            return driver.FindElements(By.XPath("//*[normalize-space()]"))
                .Any(e =>
                {
                    try
                    {
                        return e.Displayed &&
                               NormalizeText(e.Text).Contains("guia de remision") &&
                               (NormalizeText(e.Text).Contains("destinatario") ||
                                NormalizeText(e.Text).Contains("conductor") ||
                                NormalizeText(e.Text).Contains("traslado"));
                    }
                    catch
                    {
                        return false;
                    }
                });
        }

        private string CapturarResumenGuiaVisible()
        {
            return driver.FindElements(By.XPath("//*[normalize-space()]"))
                .Where(e =>
                {
                    try
                    {
                        var texto = NormalizeText(e.Text);
                        return e.Displayed &&
                               texto.Contains("guia de remision") &&
                               (texto.Contains("destinatario") ||
                                texto.Contains("conductor") ||
                                texto.Contains("traslado"));
                    }
                    catch
                    {
                        return false;
                    }
                })
                .Select(e =>
                {
                    try { return (e.Text ?? string.Empty).Trim(); }
                    catch { return string.Empty; }
                })
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => new
                {
                    Texto = t,
                    Resumen = ConstruirResumenGuiaCompacto(t),
                    Campos = ContarCamposResumenGuia(t)
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.Resumen))
                .OrderByDescending(x => x.Campos)
                .ThenBy(x => x.Texto.Length)
                .Select(x => x.Resumen)
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t)) ?? string.Empty;
        }

        private static string ConstruirEvidenciaGuiaCompacta(string? mensaje, string? resumenGuia)
        {
            if (!string.IsNullOrWhiteSpace(resumenGuia))
                return resumenGuia.Trim();

            string resumenDesdeMensaje = ConstruirResumenGuiaCompacto(mensaje);
            if (!string.IsNullOrWhiteSpace(resumenDesdeMensaje))
                return resumenDesdeMensaje;

            return !string.IsNullOrWhiteSpace(mensaje)
                ? $"Mensaje={RecortarTextoParaLog(mensaje, 220)}"
                : string.Empty;
        }

        private static string ConstruirResumenGuiaCompacto(string? texto)
        {
            var lineas = ExtraerLineasLimpias(texto).ToList();
            if (lineas.Count == 0)
                return string.Empty;

            var campos = new List<string> { "Estado=confirmada" };
            AgregarCampoResumenGuia(campos, "Traslado", ExtraerValorResumenGuia(lineas, "Traslado"));
            AgregarCampoResumenGuia(campos, "Destinatario", ExtraerValorResumenGuia(lineas, "Destinatario"));
            AgregarCampoResumenGuia(campos, "Conductor", ExtraerValorResumenGuia(lineas, "Conductor"));
            AgregarCampoResumenGuia(campos, "Origen", ExtraerValorResumenGuia(lineas, "Origen"));
            AgregarCampoResumenGuia(campos, "Destino", ExtraerValorResumenGuia(lineas, "Destino"));

            if (campos.Count > 1)
                return string.Join(" | ", campos);

            return lineas.Any(l => NormalizeText(l).Contains("guia de remision"))
                ? "Estado=guia visible"
                : string.Empty;
        }

        private static int ContarCamposResumenGuia(string? texto)
        {
            var lineas = ExtraerLineasLimpias(texto).ToList();
            return new[] { "Traslado", "Destinatario", "Conductor", "Origen", "Destino" }
                .Count(etiqueta => !string.IsNullOrWhiteSpace(ExtraerValorResumenGuia(lineas, etiqueta)));
        }

        private static IEnumerable<string> ExtraerLineasLimpias(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                yield break;

            foreach (var linea in Regex.Split(texto, @"\r\n|\n|\r"))
            {
                var limpia = Regex.Replace(linea.Trim(), @"\s+", " ");
                if (!string.IsNullOrWhiteSpace(limpia))
                    yield return limpia;
            }
        }

        private static void AgregarCampoResumenGuia(ICollection<string> campos, string etiqueta, string? valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                campos.Add($"{etiqueta}={valor}");
        }

        private static string? ExtraerValorResumenGuia(IReadOnlyList<string> lineas, string etiqueta)
        {
            string etiquetaNormalizada = NormalizeText(etiqueta);

            for (int i = 0; i < lineas.Count; i++)
            {
                string linea = lineas[i];
                string lineaNormalizada = NormalizeText(linea);
                if (!lineaNormalizada.StartsWith(etiquetaNormalizada, StringComparison.OrdinalIgnoreCase))
                    continue;

                string valor = linea.Length > etiqueta.Length
                    ? linea.Substring(etiqueta.Length).Trim()
                    : string.Empty;

                valor = Regex.Replace(valor, @"^[:\-\s]+", string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(valor) && i + 1 < lineas.Count)
                    valor = lineas[i + 1].Trim();

                return LimpiarValorResumenGuia(valor);
            }

            return null;
        }

        private static string LimpiarValorResumenGuia(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            var limpio = Regex.Replace(valor.Trim().Trim('"'), @"\s+", " ");
            return RecortarTextoParaLog(limpio, 90);
        }

        private static string RecortarTextoParaLog(string? texto, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var limpio = Regex.Replace(texto.Trim(), @"\s+", " ");
            return limpio.Length <= maxLength
                ? limpio
                : limpio.Substring(0, Math.Max(0, maxLength - 3)) + "...";
        }

        private void ValidarMensajeDeResultadoNoExitosoNuevaVenta(string resultadoEsperadoNormalizado, string? mensajeCapturado)
        {
            if (!RequiereValidacionExplicitaDeMensaje(resultadoEsperadoNormalizado))
                return;

            Assert.That(string.IsNullOrWhiteSpace(mensajeCapturado), Is.False,
                $"Se esperaba una validacion visible para '{resultadoEsperadoNormalizado}', pero no se capturo ningun mensaje.");

            string mensajeNormalizado = NormalizeText(mensajeCapturado!);
            string resumenGuia = ConstruirResumenValidacionGuia(resultadoEsperadoNormalizado, mensajeCapturado);

            if (resultadoEsperadoNormalizado.Contains("ingrese numero de licencia"))
            {
                AssertValidacionDatoTransporteFlexible(
                    mensajeNormalizado,
                    mensajeCapturado,
                    resumenGuia,
                    "licencia");
                return;
            }

            if (resultadoEsperadoNormalizado.Contains("ingrese numero de placa"))
            {
                AssertValidacionDatoTransporteFlexible(
                    mensajeNormalizado,
                    mensajeCapturado,
                    resumenGuia,
                    "placa");
                return;
            }

            if (resultadoEsperadoNormalizado.Contains("ruc requerido") ||
                resultadoEsperadoNormalizado.Contains("identificar cliente"))
            {
                AssertValidacionClienteConDocumento(
                    mensajeNormalizado,
                    mensajeCapturado,
                    resultadoEsperadoNormalizado);
                return;
            }

            if (resultadoEsperadoNormalizado.Contains("identifique al conductor con dni"))
            {
                AssertValidacionIdentidadTransporte(mensajeNormalizado, mensajeCapturado, resumenGuia);
                return;
            }

            if (resultadoEsperadoNormalizado.Contains("identifique al transportista con ruc"))
            {
                AssertValidacionIdentidadTransporte(mensajeNormalizado, mensajeCapturado, resumenGuia);
                return;
            }

            if (resultadoEsperadoNormalizado.Contains("falta peso y numero de bultos"))
            {
                AssertValidacionGuiaFlexible(
                    mensajeNormalizado,
                    mensajeCapturado,
                    resumenGuia,
                    "peso",
                    "bulto");
                return;
            }

            if (resultadoEsperadoNormalizado.Contains("este campo es obligatorio") ||
                resultadoEsperadoNormalizado.Contains("campo obligatorio"))
            {
                AssertValidacionGuiaFlexible(
                    mensajeNormalizado,
                    mensajeCapturado,
                    resumenGuia,
                    "obligatorio");
            }
        }

        private void AssertValidacionGuiaFlexible(
            string mensajeNormalizado,
            string? mensajeCapturado,
            string resumenGuia,
            params string[] fragmentosEsperados)
        {
            bool mencionaCampo = fragmentosEsperados
                .Where(fragmento => !string.IsNullOrWhiteSpace(fragmento))
                .All(fragmento => mensajeNormalizado.Contains(NormalizeText(fragmento)));

            bool esRequeridoGenerico = EsMensajeRequeridoGenerico(mensajeNormalizado);

            if (mencionaCampo || esRequeridoGenerico)
            {
                Log(resumenGuia);
                return;
            }

            Assert.Fail(
                $"La validacion no coincide con el resultado esperado. " +
                $"Mensaje actual: '{mensajeCapturado}'. {resumenGuia}");
        }

        private void AssertValidacionIdentidadTransporte(
            string mensajeNormalizado,
            string? mensajeCapturado,
            string resumenGuia)
        {
            if (EsValidacionIdentidadTransporte(mensajeNormalizado) ||
                EsMensajeRequeridoGenerico(mensajeNormalizado))
            {
                Log(resumenGuia);
                return;
            }

            Assert.Fail(
                $"La validacion de identidad de transporte no coincide con el resultado esperado. " +
                $"Mensaje actual: '{mensajeCapturado}'. {resumenGuia}");
        }

        private void AssertValidacionDatoTransporteFlexible(
            string mensajeNormalizado,
            string? mensajeCapturado,
            string resumenGuia,
            params string[] fragmentosEsperados)
        {
            bool mencionaCampoEsperado = fragmentosEsperados
                .Where(fragmento => !string.IsNullOrWhiteSpace(fragmento))
                .All(fragmento => mensajeNormalizado.Contains(NormalizeText(fragmento)));

            if (mencionaCampoEsperado ||
                EsValidacionIdentidadTransporte(mensajeNormalizado) ||
                EsMensajeRequeridoGenerico(mensajeNormalizado))
            {
                Log(resumenGuia);
                return;
            }

            Assert.Fail(
                $"La validacion de datos de transporte no coincide con el resultado esperado. " +
                $"Mensaje actual: '{mensajeCapturado}'. {resumenGuia}");
        }

        private static void AssertValidacionClienteConDocumento(
            string mensajeNormalizado,
            string? mensajeCapturado,
            string resultadoEsperadoNormalizado)
        {
            bool esperaRuc = resultadoEsperadoNormalizado.Contains("ruc requerido");
            bool mencionaCliente = mensajeNormalizado.Contains("cliente") ||
                                  mensajeNormalizado.Contains("documento") ||
                                  mensajeNormalizado.Contains("identificar") ||
                                  mensajeNormalizado.Contains("identifique");
            bool mencionaRuc = mensajeNormalizado.Contains("ruc");

            if (esperaRuc && mencionaRuc && mencionaCliente)
                return;

            if (!esperaRuc && mencionaCliente)
                return;

            Assert.Fail(
                $"La validacion de cliente no coincide con el resultado esperado. " +
                $"Esperado: '{resultadoEsperadoNormalizado}'. Mensaje actual: '{mensajeCapturado}'.");
        }

        private static bool EsValidacionIdentidadTransporte(string mensajeNormalizado)
        {
            bool mencionaConductorDni =
                mensajeNormalizado.Contains("conductor") &&
                mensajeNormalizado.Contains("dni");

            bool mencionaTransportista =
                mensajeNormalizado.Contains("transportista") &&
                (mensajeNormalizado.Contains("ruc") ||
                 mensajeNormalizado.Contains("obligatorio") ||
                 mensajeNormalizado.Contains("requerido"));

            return mencionaConductorDni || mencionaTransportista;
        }

        private static bool EsMensajeRequeridoGenerico(string mensajeNormalizado)
        {
            if (string.IsNullOrWhiteSpace(mensajeNormalizado))
                return false;

            if (mensajeNormalizado.Contains("este campo es obligatorio") ||
                mensajeNormalizado.Contains("complete los campos requeridos") ||
                mensajeNormalizado.Contains("complete los campos obligatorios") ||
                mensajeNormalizado.Contains("complete los campos"))
                return true;

            bool mencionaCampoEspecifico =
                mensajeNormalizado.Contains("transportista") ||
                mensajeNormalizado.Contains("conductor") ||
                mensajeNormalizado.Contains("licencia") ||
                mensajeNormalizado.Contains("placa") ||
                mensajeNormalizado.Contains("peso") ||
                mensajeNormalizado.Contains("bulto") ||
                mensajeNormalizado.Contains("ruc") ||
                mensajeNormalizado.Contains("dni");

            return !mencionaCampoEspecifico &&
                   (mensajeNormalizado.Contains("obligatorio") ||
                    mensajeNormalizado.Contains("requerido"));
        }

        private string ConstruirResumenValidacionGuia(string resultadoEsperadoNormalizado, string? mensajeCapturado)
        {
            var campos = new List<string>();

            if (resultadoEsperadoNormalizado.Contains("falta peso y numero de bultos"))
            {
                campos.Add("Peso bruto");
                campos.Add("Numero de bultos");
            }

            if (resultadoEsperadoNormalizado.Contains("ingrese numero de licencia"))
                campos.Add("Numero de licencia");

            if (resultadoEsperadoNormalizado.Contains("ingrese numero de placa"))
                campos.Add("Numero de placa");

            if (resultadoEsperadoNormalizado.Contains("identifique al conductor con dni"))
                campos.Add("Identificacion de transporte");

            if (resultadoEsperadoNormalizado.Contains("identifique al transportista con ruc"))
                campos.Add("Identificacion de transporte");

            foreach (var campo in _guiaCamposOmitidos)
            {
                if (!campos.Any(c => string.Equals(c, campo, StringComparison.OrdinalIgnoreCase)))
                    campos.Add(campo);
            }

            var mensajes = CapturarValidacionesVisiblesDetalladas()
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (!string.IsNullOrWhiteSpace(mensajeCapturado) &&
                !mensajes.Any(m => string.Equals(m, mensajeCapturado, StringComparison.OrdinalIgnoreCase)))
            {
                if (campos.Count > 0 && EsMensajeRequeridoGenerico(NormalizeText(mensajeCapturado!)))
                {
                    foreach (var campo in campos)
                    {
                        string mensajeCampo = $"{campo}: {mensajeCapturado!.Trim()}";
                        if (!mensajes.Any(m => string.Equals(m, mensajeCampo, StringComparison.OrdinalIgnoreCase)))
                            mensajes.Add(mensajeCampo);
                    }
                }
                else
                {
                    mensajes.Insert(0, mensajeCapturado!.Trim());
                }
            }

            string camposResumen = campos.Count > 0
                ? string.Join(", ", campos)
                : "campos requeridos de guia de remision";

            string mensajesResumen = mensajes.Count > 0
                ? string.Join(" | ", mensajes)
                : "sin texto visible adicional";

            return $"Guia de remision bloqueada. Esperado QA: {camposResumen}. Validacion UI: {mensajesResumen}.";
        }

        private static bool RequiereValidacionExplicitaDeMensaje(string resultadoEsperadoNormalizado) =>
            resultadoEsperadoNormalizado.Contains("ingrese numero de licencia") ||
            resultadoEsperadoNormalizado.Contains("ingrese numero de placa") ||
            resultadoEsperadoNormalizado.Contains("ruc requerido") ||
            resultadoEsperadoNormalizado.Contains("identificar cliente") ||
            resultadoEsperadoNormalizado.Contains("identifique al conductor con dni") ||
            resultadoEsperadoNormalizado.Contains("identifique al transportista con ruc") ||
            resultadoEsperadoNormalizado.Contains("falta peso y numero de bultos") ||
            resultadoEsperadoNormalizado.Contains("este campo es obligatorio") ||
            resultadoEsperadoNormalizado.Contains("campo obligatorio");

        public void ValidarResultadoDescuentoEnVenta(string resultadoEsperado)
        {
            if (string.IsNullOrWhiteSpace(resultadoEsperado) || resultadoEsperado.Trim() == "-")
                return;

            var estado = CapturarEstadoDescuento();
            var esperado = NormalizeText(resultadoEsperado);
            var contexto = ObtenerContextoDescuento(resultadoEsperado, estado);
            var valorDescuento = contexto.Valor!.Value;
            var puedeValidarRecalculoTotal = _discountContext.Activo && _discountContext.TotalAntes.HasValue;
            var totalAntes = contexto.TotalAntes!.Value;

            Assert.That(driver.Url, Does.Contain("/sales/new-sales"),
                $"La pantalla actual no corresponde a Nueva Venta. URL actual: {driver.Url}");
            Assert.That(estado.ModoVentaVisible, Is.True,
                "No se visualiza la opcion 'VENTA NORMAL' en la pantalla de Nueva Venta.");
            Assert.That(estado.CantidadFilas, Is.GreaterThanOrEqualTo(1),
                $"Se esperaba al menos 1 producto en el grid y se obtuvieron {estado.CantidadFilas}.");
            Assert.That(estado.DescuentoMarcado, Is.True,
                "El check Descuento deberia quedar marcado.");
            Assert.That(estado.InputDescuentoHabilitado, Is.True,
                "El ingreso del descuento deberia estar habilitado.");

            if (contexto.Tipo.Contains("item"))
            {
                Assert.That(estado.ItemActivo, Is.True,
                    "La opcion Item deberia quedar seleccionada segun la configuracion del descuento.");
                Assert.That(estado.GlobalActivo, Is.False,
                    "La opcion Global no deberia quedar activa cuando el descuento configurado es por item.");
            }
            else if (contexto.Tipo.Contains("global"))
            {
                Assert.That(estado.GlobalActivo, Is.True,
                    "La opcion Global deberia quedar seleccionada segun la configuracion del descuento.");
                Assert.That(estado.ItemActivo, Is.False,
                    "La opcion Item no deberia quedar activa cuando el descuento configurado es global.");
            }

            if (contexto.Modo.Contains("$") || contexto.Modo.Contains("monto"))
            {
                Assert.That(estado.ModoPorcentajeActivo, Is.False,
                    "El modo porcentaje no deberia quedar activo cuando el descuento configurado es por monto.");
            }
            else if (contexto.Modo.Contains("%") || contexto.Modo.Contains("porcentaje"))
            {
                Assert.That(estado.ModoMontoActivo, Is.False,
                    "El modo monto no deberia quedar activo cuando el descuento configurado es porcentual.");
            }

            if (esperado.Contains("item monto valido"))
            {
                Assert.That(estado.ItemActivo, Is.True,
                    "La opcion Item deberia quedar seleccionada.");
                Assert.That(estado.GlobalActivo, Is.False,
                    "La opcion Global no deberia quedar activa en descuento por item.");
                Assert.That(estado.ValorDescuentoNormalizado, Is.EqualTo("1.00").Or.EqualTo("1"),
                    $"El valor del descuento deberia quedar en 1.00 y se obtuvo '{estado.ValorDescuentoRaw}'.");
                Assert.That(estado.HayErrorVisible, Is.False,
                    $"No se esperaba mensaje de error para descuento item monto valido. Mensaje actual: '{estado.MensajeValidacion}'.");

                if (puedeValidarRecalculoTotal)
                {
                    var totalEsperado = totalAntes - valorDescuento;
                    AssertMontoAproximado(estado.TotalActual, totalEsperado,
                        $"El total final deberia recalcularse restando el descuento al total previo ({totalAntes:0.00} - {valorDescuento:0.00}).");
                }
            }
            else if (esperado.Contains("global porcentaje valido"))
            {
                Assert.That(estado.GlobalActivo, Is.True,
                    "La opcion Global deberia quedar seleccionada.");
                Assert.That(estado.ItemActivo, Is.False,
                    "La opcion Item no deberia quedar activa en descuento global.");
                Assert.That(estado.ValorDescuentoNormalizado, Is.EqualTo("5.00").Or.EqualTo("5"),
                    $"El valor del descuento deberia quedar en 5 y se obtuvo '{estado.ValorDescuentoRaw}'.");
                Assert.That(estado.HayErrorVisible, Is.False,
                    $"No se esperaba mensaje de error para descuento global por porcentaje valido. Mensaje actual: '{estado.MensajeValidacion}'.");

                if (puedeValidarRecalculoTotal)
                {
                    var totalEsperado = totalAntes - (totalAntes * valorDescuento / 100m);
                    AssertMontoAproximado(estado.TotalActual, totalEsperado,
                        $"El total final deberia recalcularse aplicando el {valorDescuento:0.##}% al total previo ({totalAntes:0.00}).");
                }
            }
            else if (esperado.Contains("global monto invalido"))
            {
                Assert.That(estado.GlobalActivo, Is.True,
                    "La opcion Global deberia quedar seleccionada.");
                Assert.That(estado.ValorDescuentoNormalizado, Is.EqualTo("20.00").Or.EqualTo("20"),
                    $"El valor del descuento deberia quedar en 20.00 y se obtuvo '{estado.ValorDescuentoRaw}'.");
                Assert.That(estado.HayErrorVisible || estado.InputDescuentoInvalido, Is.True,
                    "Se esperaba que el sistema rechace el descuento global por monto invalido y muestre una validacion.");
                if (puedeValidarRecalculoTotal)
                {
                    AssertMontoAproximado(estado.TotalActual, totalAntes,
                        $"El total deberia mantenerse igual al total previo ({totalAntes:0.00}) cuando el descuento global por monto es invalido.");
                }
            }
            else if (esperado.Contains("item porcentaje invalido"))
            {
                Assert.That(estado.ItemActivo, Is.True,
                    "La opcion Item deberia quedar seleccionada.");
                Assert.That(estado.GlobalActivo, Is.False,
                    "La opcion Global no deberia quedar activa en descuento por item.");
                Assert.That(estado.ValorDescuentoNormalizado, Is.EqualTo("100.00").Or.EqualTo("100"),
                    $"El valor del descuento deberia quedar en 100 y se obtuvo '{estado.ValorDescuentoRaw}'.");
                Assert.That(estado.HayErrorVisible || estado.InputDescuentoInvalido, Is.True,
                    "Se esperaba que el sistema rechace el descuento item por porcentaje invalido y muestre una validacion.");
                if (puedeValidarRecalculoTotal)
                {
                    AssertMontoAproximado(estado.TotalActual, totalAntes,
                        $"El total deberia mantenerse igual al total previo ({totalAntes:0.00}) cuando el descuento item por porcentaje es invalido.");
                }
            }
            else
            {
                Assert.Fail($"No existe una validacion implementada para el resultado de descuento '{resultadoEsperado}'.");
            }
        }

        private DiscountContext ObtenerContextoDescuento(string resultadoEsperado, DiscountState estado)
        {
            if (_discountContext.Activo && _discountContext.TotalAntes.HasValue && _discountContext.Valor.HasValue)
                return _discountContext;

            var esperado = NormalizeText(resultadoEsperado);
            var tipo = estado.ItemActivo
                ? "item"
                : estado.GlobalActivo
                    ? "global"
                    : esperado.Contains("item")
                        ? "item"
                        : esperado.Contains("global")
                            ? "global"
                            : string.Empty;

            var modo = estado.ModoMontoActivo
                ? "monto"
                : estado.ModoPorcentajeActivo
                    ? "porcentaje"
                    : esperado.Contains("porcentaje")
                        ? "porcentaje"
                        : esperado.Contains("monto") || esperado.Contains("$")
                            ? "monto"
                            : string.Empty;

            var valor = _discountContext.Valor;
            if (!valor.HasValue && TryParseDecimalFlexible(estado.ValorDescuentoRaw, out var valorActual))
                valor = valorActual;

            var totalAntes = _discountContext.TotalAntes
                ?? ObtenerTotalVentaDesdeDetalle()
                ?? ObtenerTotalVentaActual(incluirMontoPago: false)
                ?? ObtenerTotalVentaActual();

            var contextoFallback = new DiscountContext
            {
                Activo = _discountContext.Activo || estado.DescuentoMarcado || !string.IsNullOrWhiteSpace(estado.ValorDescuentoRaw),
                Tipo = string.IsNullOrWhiteSpace(_discountContext.Tipo) ? tipo : _discountContext.Tipo,
                Modo = string.IsNullOrWhiteSpace(_discountContext.Modo) ? modo : _discountContext.Modo,
                Valor = valor,
                TotalAntes = totalAntes
            };

            Assert.That(contextoFallback.Activo, Is.True,
                $"No se encontro un contexto de descuento activo en Nueva Venta para validar '{resultadoEsperado}'.");
            Assert.That(contextoFallback.TotalAntes.HasValue, Is.True,
                $"No se pudo capturar el total previo al descuento para validar '{resultadoEsperado}'.");
            Assert.That(contextoFallback.Valor.HasValue, Is.True,
                $"No se pudo interpretar el valor del descuento configurado para validar '{resultadoEsperado}'.");

            return contextoFallback;
        }

        // Paso: configura el pago X
        public void ConfigurePaymentFlow(string pago) => UpdatePayment(pago);

        public void ConfigurarMediosDePagoNuevaVenta(
            string tipoPago,
            string multipago,
            string medioPago,
            string banco,
            string tarjeta,
            string cuentaBancaria,
            string nroOperacion,
            string montoPorMedio,
            string nroCuotas,
            string montoInicialCredito,
            string observacionPago)
        {
            var totalVentaAntesPago = EsperarTotalVentaDisponibleNuevaVenta();
            Log($"[PagoNV] Total de referencia antes de abrir Pago: {totalVentaAntesPago?.ToString("0.00", CultureInfo.InvariantCulture) ?? "NA"}");

            AbrirPagoNuevaVenta();
            SeleccionarTipoPagoNuevaVenta(tipoPago);

            bool esMultipago = DebeActivarOpcion(multipago);
            ConfigurarMultipagoNuevaVenta(esMultipago);
            _lastCreditInstallments = string.Empty;

            if (NormalizeText(tipoPago).Contains("credito"))
            {
                _lastCreditInstallments = EsNA(nroCuotas) ? string.Empty : nroCuotas.Trim();

                if (!EsNA(nroCuotas))
                    IngresarNumeroCuotasNuevaVenta(nroCuotas);

                if (!EsNA(montoInicialCredito))
                    IngresarMontoInicialCreditoNuevaVenta(montoInicialCredito);
            }

            var instrucciones = ConstruirInstruccionesPagoNuevaVenta(
                medioPago,
                banco,
                tarjeta,
                cuentaBancaria,
                nroOperacion,
                montoPorMedio,
                totalVentaAntesPago);

            _paymentContext = new PaymentContext
            {
                Configurado = true,
                TipoPago = NormalizeText(tipoPago),
                Multipago = esMultipago,
                Medios = instrucciones.Select(x => x.MedioPago).ToList(),
                Bancos = instrucciones.Select(x => x.Banco).Where(x => !EsNA(x)).ToList(),
                Tarjetas = instrucciones.Select(x => x.Tarjeta).Where(x => !EsNA(x)).ToList(),
                Cuentas = instrucciones.Select(x => x.CuentaBancaria).Where(x => !EsNA(x)).ToList(),
                Operaciones = instrucciones.Select(x => x.Operacion).Where(x => !EsNA(x)).ToList(),
                Montos = instrucciones.Select(x => x.MontoEsperado).ToList(),
                TotalAntes = totalVentaAntesPago ?? ObtenerTotalVentaActual(),
                MontoInicialCredito = TryParseDecimalFlexible(montoInicialCredito, out var montoInicial) ? montoInicial : (decimal?)null
            };

            NeutralizarPagoEfectivoPredeterminadoNuevaVenta(instrucciones);

            foreach (var instruccion in instrucciones)
            {
                SeleccionarTabMedioPagoNuevaVenta(instruccion.MedioPago);
                ConfigurarMedioPagoNuevaVenta(instruccion, tipoPago, observacionPago);

                if (esMultipago)
                    GuardarMedioPagoActualNuevaVenta();
            }
        }

        public void IngresarObservacionDelPagoNuevaVenta(string observacion)
        {
            if (EsNA(observacion)) return;

            AbrirPagoNuevaVenta();
            IngresarObservacionPagoNuevaVenta(observacion);
            Log($"[PagoNV] Observacion del pago configurada: '{observacion.Trim()}'.");
        }

        private List<PaymentInstruction> ConstruirInstruccionesPagoNuevaVenta(
            string medioPago,
            string banco,
            string tarjeta,
            string cuentaBancaria,
            string nroOperacion,
            string montoPorMedio,
            decimal? totalReferencia)
        {
            var medios = SepararValores(medioPago)
                .Select(NormalizeText)
                .ToList();

            if (medios.Count == 0 || (medios.Count == 1 && EsNA(medios[0])))
                return new List<PaymentInstruction>();

            Assert.That(medios.Count, Is.GreaterThan(0),
                "Debe existir al menos un medio de pago configurado en el feature.");

            var bancos = SepararValores(banco);
            var tarjetas = SepararValores(tarjeta);
            var cuentas = SepararValores(cuentaBancaria);
            var operaciones = SepararValores(nroOperacion);
            var montos = SepararValores(montoPorMedio);
            int bancoIndex = 0;
            int tarjetaIndex = 0;
            int cuentaIndex = 0;
            int operacionIndex = 0;

            var instrucciones = new List<PaymentInstruction>(medios.Count);
            for (int i = 0; i < medios.Count; i++)
            {
                var medioActual = medios[i];
                var montoConfigurado = ObtenerValorConfiguracionPago(montos, i);
                var bancoActual = "NA";
                var tarjetaActual = "NA";
                var cuentaActual = "NA";
                var operacionActual = "NA";

                switch (medioActual)
                {
                    case "tarjeta_credito":
                    case "tarjeta_debito":
                        bancoActual = ObtenerValorConfiguracionPago(bancos, bancoIndex++);
                        tarjetaActual = ObtenerValorConfiguracionPago(tarjetas, tarjetaIndex++);
                        operacionActual = ObtenerValorConfiguracionPago(operaciones, operacionIndex++);
                        break;
                    case "transferencia_fondos":
                    case "deposito_cuenta":
                        cuentaActual = ObtenerValorConfiguracionPago(cuentas, cuentaIndex++);
                        operacionActual = ObtenerValorConfiguracionPago(operaciones, operacionIndex++);
                        break;
                }

                var montoResuelto = ResolverMontoConfiguradoNuevaVenta(
                    medioActual,
                    montoConfigurado,
                    totalReferencia,
                    medios.Count);
                instrucciones.Add(new PaymentInstruction
                {
                    MedioPago = medioActual,
                    Banco = bancoActual,
                    Tarjeta = tarjetaActual,
                    CuentaBancaria = cuentaActual,
                    Operacion = operacionActual,
                    MontoConfigurado = montoResuelto,
                    MontoEsperado = TryParseDecimalFlexible(montoResuelto, out var montoEsperado) ? montoEsperado : (decimal?)null
                });
            }

            return instrucciones;
        }

        private static string ObtenerValorConfiguracionPago(IReadOnlyList<string> valores, int index)
        {
            if (valores == null || index < 0 || index >= valores.Count)
                return "NA";

            return valores[index].Trim();
        }

        private string ResolverMontoConfiguradoNuevaVenta(string medioPago, string montoConfigurado, decimal? totalReferencia, int totalMedios)
        {
            if (!EsNA(montoConfigurado))
                return ResolverMontoPago(montoConfigurado, totalReferencia);

            if (totalMedios == 1)
            {
                var total = totalReferencia
                    ?? _paymentContext.TotalAntes
                    ?? ObtenerTotalVentaActual(incluirMontoPago: false)
                    ?? ObtenerTotalVentaActual();

                Assert.That(total.HasValue, Is.True,
                    $"No se pudo inferir el monto del medio de pago '{medioPago}' a partir del total de la venta.");

                return total!.Value.ToString("0.00", CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }

        // Nueva Venta a veces precarga el total completo en EFECTIVO apenas se abre Pago.
        // Si el escenario declara un solo medio distinto de efectivo, se limpia ese valor
        // para evitar que el caso quede contaminado por un pago previo/autocompletado del sistema.
        // No se escribe "0": solo se limpia el input cuando ya venia con un monto positivo.
        private void NeutralizarPagoEfectivoPredeterminadoNuevaVenta(IReadOnlyList<PaymentInstruction> instrucciones)
        {
            if (_paymentContext.Multipago || !_paymentContext.TipoPago.Contains("contado"))
                return;

            if (instrucciones.Count != 1 || instrucciones[0].MedioPago == "efectivo")
                return;

            var input = driver.FindElements(VentasLocators.Payment.CashReceivedNewSale)
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });
            if (input == null)
                return;

            var valorActual = (input.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
            if (!TryParseDecimalFlexible(valorActual, out var montoActual) || montoActual <= 0m)
                return;

            Log($"[PagoNV] Se limpia el efectivo autocompletado '{valorActual}' antes de configurar '{instrucciones[0].MedioPago}'.");
            LimpiarValorInputNuevaVenta(input);
        }

        public void ValidarResultadoPagoEnNuevaVenta(string resultadoEsperado)
        {
            if (string.IsNullOrWhiteSpace(resultadoEsperado) || resultadoEsperado.Trim() == "-")
                return;

            var esperado = NormalizeText(resultadoEsperado);
            PaymentContext? contexto = _paymentContext.Configurado ? _paymentContext : null;
            AbrirPagoNuevaVenta();

            Assert.That(driver.Url, Does.Contain("/sales/new-sales"),
                $"La pantalla actual no corresponde a Nueva Venta. URL actual: {driver.Url}");

            if (esperado.Contains("puntos insuficiente"))
            {
                AssertTabPagoActiva("PUNTOS");
                if (contexto != null)
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                AssertAlgunMensajeValidacionPago(
                    "El sistema deberia mostrar la inconsistencia de puntos insuficientes.",
                    "puntos insuficiente",
                    "no hay suficientes puntos disponibles",
                    "suficientes puntos disponibles");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando el cliente no tiene puntos suficientes.");
            }
            else if (esperado.Contains("transferencia sin cuenta ni informacion"))
            {
                AssertTabPagoActiva("TRANSFERENCIA DE FONDOS");
                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El escenario de inconsistencia en transferencia deberia estar configurado como multipago.");
                }

                AssertAgregarMedioPagoDeshabilitado(
                    "El boton Agregar Medio de Pago deberia permanecer deshabilitado cuando falta cuenta e informacion en transferencia.");
                AssertMensajesValidacionPago(
                    "El sistema deberia mostrar las validaciones faltantes de transferencia.",
                    "cuenta bancaria",
                    "informacion");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando una transferencia multipago queda incompleta.");
            }
            else if (esperado.Contains("debito sin banco ni tarjeta"))
            {
                AssertTabPagoActiva("TARJETAS DE DEBITO");
                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El escenario de inconsistencia en debito deberia estar configurado como multipago.");
                }

                AssertAgregarMedioPagoDeshabilitado(
                    "El boton Agregar Medio de Pago deberia permanecer deshabilitado cuando falta banco y tarjeta.");
                AssertMensajesValidacionPago(
                    "El sistema deberia mostrar las validaciones faltantes de banco y tarjeta.",
                    "banco",
                    "tarjeta");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando un medio de debito multipago queda incompleto.");
            }
            else if (esperado.Contains("debito sin informacion"))
            {
                AssertTabPagoActiva("TARJETAS DE DEBITO");
                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El escenario de inconsistencia en debito deberia estar configurado como multipago.");
                }

                AssertAgregarMedioPagoDeshabilitado(
                    "El boton Agregar Medio de Pago deberia permanecer deshabilitado cuando falta la informacion del debito.");
                AssertMensajesValidacionPago(
                    "El sistema deberia mostrar la validacion faltante de informacion.",
                    "informacion");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando falta la informacion del debito.");
            }
            else if (esperado.Contains("credito multipago no cubre monto inicial"))
            {
                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El escenario de credito inconsistente deberia estar configurado como multipago.");
                }

                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando el multipago a credito no cubre el monto inicial.");
            }
            else if (esperado.Contains("multipago puntos no habilitado sin cliente"))
            {
                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El escenario deberia mantenerse en multipago para validar el bloqueo de puntos.");
                }

                Assert.That(EstaMarcado(VentasLocators.Payment.MultipaymentCheckbox), Is.True,
                    "La opcion Multipago deberia permanecer marcada.");
                AssertMedioPagoNoDisponibleNuevaVenta(
                    "PUNTOS",
                    "El sistema no deberia habilitar el medio de pago Puntos cuando no hay cliente identificado.",
                    VentasLocators.Payment.PointsMethod,
                    By.XPath("//span[normalize-space()='PUNTOS']"));
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando el pago queda incompleto y Puntos no esta disponible.");
            }
            else if (esperado.Contains("credito sin cliente"))
            {
                if (contexto != null)
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                AssertCronogramaCreditoConfiguradoNuevaVenta();

                if (contexto?.MontoInicialCredito is decimal montoInicial)
                {
                    AssertInputAproximado(VentasLocators.Payment.CreditInitialAmountInput, montoInicial,
                        "El monto inicial del credito deberia quedar registrado correctamente.");
                }

                AssertAlgunMensajeValidacionPago(
                    "El sistema deberia advertir que la venta a credito requiere cliente identificado.",
                    "es necesario identificar al cliente",
                    "es necesario seleccionar un cliente",
                    "identificar al cliente");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando se configura una venta a credito sin cliente.");
            }
            else if (esperado.Contains("monto inicial cero"))
            {
                if (contexto != null)
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                AssertInputAproximado(VentasLocators.Payment.CreditInitialAmountInput, 0m,
                    "El monto inicial deberia quedar registrado en 0 para disparar la validacion.");
                AssertMensajesValidacionPago(
                    "El sistema deberia mostrar la regla de monto inicial mayor a 0.",
                    "monto inicial",
                    "mayor a 0");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando el monto inicial es 0.");
            }
            else if (esperado.Contains("credito configurado exitoso"))
            {
                if (contexto != null)
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                AssertCronogramaCreditoConfiguradoNuevaVenta();

                if (contexto?.MontoInicialCredito is decimal montoInicial)
                {
                    AssertInputAproximado(VentasLocators.Payment.CreditInitialAmountInput, montoInicial,
                        "El monto inicial del credito deberia quedar registrado correctamente.");
                }

                AssertMensajePagoNoVisible(
                    "cliente",
                    "credito debe identificar");
                AssertGuardarHabilitadoEnPago(
                    "El boton Guardar deberia habilitarse cuando el credito queda configurado correctamente.");
            }
            else if (esperado.Contains("transferencia"))
            {
                AssertTabPagoActiva("TRANSFERENCIA DE FONDOS");
                AssertSeccionPagoListaParaGuardar(
                    "La seccion Pago deberia indicar que los campos requeridos fueron completados correctamente para transferencia.");

                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                    if (contexto.Cuentas.Count > 0)
                    {
                        AssertTextoSeleccionado(contexto.Cuentas,
                            "La cuenta bancaria deberia quedar registrada correctamente.",
                            0,
                            VentasLocators.Payment.BankAccountSelect,
                            VentasLocators.Payment.BankAccountTrigger);
                    }

                    if (contexto.Montos.Any(x => x.HasValue))
                    {
                        AssertMontoMedioPagoNuevaVenta(contexto.Montos,
                            "El monto del medio de pago deberia quedar registrado correctamente.");
                    }

                    if (contexto.Operaciones.Count > 0)
                    {
                        AssertInputExacto(VentasLocators.Payment.PaymentInfoInput, contexto.Operaciones,
                            "El numero de operacion deberia quedar registrado correctamente.");
                    }
                }

                AssertGuardarHabilitadoEnPago(
                    "El boton Guardar deberia habilitarse cuando la transferencia cubre el total de la venta.");
            }
            else if (esperado.Contains("debito"))
            {
                AssertTabPagoActiva("TARJETAS DE DEBITO");
                AssertSeccionPagoListaParaGuardar(
                    "La seccion Pago deberia indicar que los campos requeridos fueron completados correctamente para tarjeta de debito.");

                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                    if (contexto.Bancos.Count > 0)
                    {
                        AssertTextoSeleccionado(contexto.Bancos,
                            "El banco deberia quedar registrado correctamente.",
                            0,
                            VentasLocators.Payment.BankSelect,
                            VentasLocators.Payment.BankTrigger);
                    }

                    if (contexto.Tarjetas.Count > 0)
                    {
                        AssertTextoSeleccionado(contexto.Tarjetas,
                            "La tarjeta deberia quedar registrada correctamente.",
                            1,
                            VentasLocators.Payment.CardSelect,
                            VentasLocators.Payment.CardTrigger);
                    }

                    if (contexto.Montos.Any(x => x.HasValue))
                    {
                        AssertMontoMedioPagoNuevaVenta(contexto.Montos,
                            "El monto del medio de pago deberia quedar registrado correctamente.");
                    }

                    if (contexto.Operaciones.Count > 0)
                    {
                        AssertInputExacto(VentasLocators.Payment.PaymentInfoInput, contexto.Operaciones,
                            "La informacion de la operacion deberia quedar registrada correctamente.");
                    }
                }

                AssertGuardarHabilitadoEnPago(
                    "El boton Guardar deberia habilitarse cuando el pago con tarjeta de debito cubre el total.");
            }
            else if (esperado.Contains("efectivo"))
            {
                AssertTabPagoActiva("EFECTIVO");
                AssertSeccionPagoListaParaGuardar(
                    "La seccion Pago deberia indicar que los campos requeridos fueron completados correctamente para efectivo.");

                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                    var totalEsperado = contexto.TotalAntes ?? ObtenerTotalVentaActual();
                    Assert.That(totalEsperado.HasValue, Is.True,
                        "No se pudo capturar el total de la venta para validar el efectivo.");
                    var totalVentaEsperado = totalEsperado.GetValueOrDefault();

                    TryAssertInputAproximado(VentasLocators.Payment.CashAmount, totalVentaEsperado,
                        "El monto de la venta deberia mostrarse correctamente en efectivo.");

                    if (contexto.Montos.Any(x => x.HasValue))
                    {
                        AssertInputAproximado(VentasLocators.Payment.CashReceivedNewSale, contexto.Montos,
                            "El valor recibido en efectivo deberia quedar registrado correctamente.");

                        var recibido = contexto.Montos.FirstOrDefault();
                        if (recibido.HasValue)
                        {
                            var vueltoEsperado = recibido.Value - totalVentaEsperado;
                            AssertInputAproximado(VentasLocators.Payment.Change, vueltoEsperado,
                                "El vuelto calculado no coincide con el esperado.");
                        }
                    }
                }

                if (!esperado.Contains("sin validar guardar"))
                {
                    AssertGuardarHabilitadoEnPago(
                        "El boton Guardar deberia habilitarse cuando el pago en efectivo cubre el total.");
                }
            }
            else if (esperado.Contains("puntos"))
            {
                AssertTabPagoActiva("PUNTOS");
                AssertSeccionPagoListaParaGuardar(
                    "La seccion Pago deberia indicar que los campos requeridos fueron completados correctamente para puntos.");

                if (contexto != null)
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);

                AssertGuardarHabilitadoEnPago(
                    "El boton Guardar deberia habilitarse cuando el pago con puntos cubre el total.");
            }
            else if (esperado.Contains("multipago"))
            {
                AssertSeccionPagoListaParaGuardar(
                    "La seccion Pago deberia indicar que los campos requeridos fueron completados correctamente para multipago.");

                if (contexto != null)
                {
                    AssertTipoPagoSeleccionadoNuevaVenta(contexto);
                    Assert.That(contexto.Multipago, Is.True,
                        "El contexto de prueba deberia registrar que el pago fue configurado como multipago.");
                    Assert.That(contexto.Medios.Count, Is.GreaterThan(1),
                        "El contexto de pago deberia conservar mas de un medio de pago para validar el multipago.");
                }

                Assert.That(EstaMarcado(VentasLocators.Payment.MultipaymentCheckbox), Is.True,
                    "La opcion Multipago deberia quedar marcada.");
                AssertGuardarHabilitadoEnPago(
                    "El boton Guardar deberia habilitarse cuando la suma de los medios de pago cubre el total.");
            }
            else if (esperado.Contains("fecha no debe ser pasada"))
            {
                // La validacion aparece en la seccion Pago al ingresar una fecha de credito anterior a hoy.
                // El sistema debe mostrar el mensaje y dejar el boton Guardar deshabilitado.
                AssertAlgunMensajeValidacionPago(
                    "El sistema deberia mostrar el mensaje 'La fecha no debe ser pasada.' al ingresar una fecha de credito pasada.",
                    "la fecha no debe ser pasada",
                    "fecha no debe ser pasada",
                    "fecha invalida",
                    "fecha incorrecta");
                AssertGuardarDeshabilitadoEnPago(
                    "El boton Guardar deberia permanecer deshabilitado cuando la fecha de credito es anterior a hoy.");
            }
            else
            {
                Assert.Fail($"No existe una validacion implementada para el resultado de pago '{resultadoEsperado}'.");
            }
        }

        // Paso: guarda la venta
        // Intenta hacer click en Guardar. Si el boton esta deshabilitado, lo informa y no falla.
        // Captura el mensaje resultante sin sobrescribir mensajes de popup previos.
        public void GuardarVentaFlow()
        {
            Log("Paso 10 - Intentando guardar venta...");
            _wasSaveEnabled = false;
            _wasSaveExecuted = false;

            if (TryResolvePendingGuideModal("Guardar"))
            {
                if (IsGuideModalVisible())
                {
                    if (string.IsNullOrWhiteSpace(_lastObservedMessage))
                        _lastObservedMessage = CapturarValidaciones();

                    Log($"La guia de remision sigue abierta y bloquea el guardado. Mensaje actual: '{_lastObservedMessage}'");
                    return;
                }

                Thread.Sleep(500);
            }

            // Cerrar modal bloqueante si existe ANTES de interactuar con el formulario.
            // No retornar: el modal puede ser una advertencia informativa; el estado real
            // del boton Guardar determina si la venta puede proceder.
            if (TryHandleBlockingModal())
            {
                Log("Modal bloqueante cerrado antes de Guardar - continuando con el flujo.");
                Thread.Sleep(500);
            }

            var btn = driver.FindElements(VentasLocators.NuevaVenta.GuardarVenta)
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

            if (btn == null)
            {
                Log("Boton Guardar no encontrado en el DOM.");
                return;
            }

            _wasSaveEnabled = IsSaveEnabled();
            Log($"Boton Guardar habilitado: {_wasSaveEnabled}");

            var validacionBloqueanteAntesDeGuardar = CapturarValidacionesVisibles()
                .FirstOrDefault(EsValidacionBloqueanteFormulario);
            if (!string.IsNullOrWhiteSpace(validacionBloqueanteAntesDeGuardar))
            {
                _wasSaveEnabled = false;
                _lastObservedMessage = validacionBloqueanteAntesDeGuardar;
                Log($"Guardar bloqueado por validacion activa: '{_lastObservedMessage}'");
                return;
            }

            if (!_wasSaveEnabled)
            {
                // Capturar la validacion actualmente visible en el formulario.
                // Sobrescribir _lastObservedMessage: el mensaje de validacion del form tiene
                // prioridad sobre cualquier popup informativo capturado en pasos anteriores.
                var validacion = CapturarValidaciones();
                _lastObservedMessage = !string.IsNullOrWhiteSpace(validacion)
                    ? validacion
                    : "Formulario incompleto (sin mensaje de validacion visible)";
                Log($"Guardar DESHABILITADO - Validacion activa: '{_lastObservedMessage}'");
                return;
            }

            for (int intento = 1; intento <= 2 && !_wasSaveExecuted; intento++)
            {
                try
                {
                    btn = driver.FindElements(VentasLocators.NuevaVenta.GuardarVenta)
                        .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

                    if (btn == null)
                    {
                        Log("Boton Guardar no encontrado al momento de ejecutar el click.");
                        break;
                    }

                    ScrollToCenter(btn);
                    btn.Click();
                    Thread.Sleep(2000);
                    _wasSaveExecuted = true;
                }
                catch (ElementClickInterceptedException)
                {
                    Log("ElementClickInterceptedException - un modal intercepto el click en Guardar.");
                    var modalCerrado = TryHandleBlockingModal();
                    if (!modalCerrado)
                    {
                        try
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
                            Thread.Sleep(2000);
                            _wasSaveExecuted = true;
                        }
                        catch
                        {
                        }
                    }

                    if (!_wasSaveExecuted)
                        Thread.Sleep(800);
                }
                catch (StaleElementReferenceException)
                {
                    Thread.Sleep(500);
                }
            }

            if (!_wasSaveExecuted)
            {
                if (string.IsNullOrWhiteSpace(_lastObservedMessage))
                    _lastObservedMessage = CaptureVisibleMessage(2);

                if (string.IsNullOrWhiteSpace(_lastObservedMessage))
                    _lastObservedMessage = CapturarValidaciones();

                Log($"No se pudo ejecutar el click en Guardar. Mensaje actual: '{_lastObservedMessage}'");
                return;
            }

            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(12))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(200)
                }.Until(_ => IsNewSaleFormReset() || !string.IsNullOrWhiteSpace(CaptureVisibleMessage(1)));
            }
            catch
            {
            }

            // Resultado del guardado: form reiniciado = exito, mensaje visible = error post-guardado
            var msg = CaptureVisibleMessage(5);
            if (!string.IsNullOrWhiteSpace(msg))
                _lastObservedMessage = msg;
            else
            {
                var validacionPostGuardado = CapturarValidaciones();
                if (!string.IsNullOrWhiteSpace(validacionPostGuardado))
                    _lastObservedMessage = validacionPostGuardado;
            }

            Log($"Resultado: Habilitado={_wasSaveEnabled}, Ejecutado={_wasSaveExecuted}, Mensaje='{_lastObservedMessage}'");
        }

        // Helpers privados

        private void WaitForFormReady()
        {
            if (IsNewSaleFormReady())
            {
                Thread.Sleep(1000);
                return;
            }

            var baseUrl = new Uri(driver.Url).GetLeftPart(UriPartial.Authority);
            driver.Navigate().GoToUrl(baseUrl + "/sales/new-sales");
            WaitForDocumentReady();

            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(45))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(300)
                }.Until(_ => IsNewSaleFormReady());
            }
            catch (WebDriverTimeoutException ex)
            {
                string mensaje = CapturarValidaciones();
                throw new WebDriverTimeoutException(
                    $"No cargo el formulario de Nueva Venta. URL actual: {driver.Url}. " +
                    $"Mensaje visible: '{(string.IsNullOrWhiteSpace(mensaje) ? "sin mensaje visible" : mensaje)}'.",
                    ex);
            }

            Thread.Sleep(1000);
        }

        private bool IsNewSaleFormReady()
        {
            return driver.Url.Contains("/sales/new-sales", StringComparison.OrdinalIgnoreCase) &&
                   (ExisteElementoVisible(VentasLocators.NuevaVenta.IgvCheck) ||
                    ExisteElementoVisible(VentasLocators.NuevaVenta.ModoVenta("VENTA NORMAL")) ||
                    ExisteElementoVisible(VentasLocators.Detail.FamilySelect) ||
                    ExisteElementoVisible(VentasLocators.NuevaVenta.FamiliaDropdown));
        }

        private void WaitForDocumentReady()
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(300)
                }.Until(_ =>
                    string.Equals(
                        ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState")?.ToString(),
                        "complete",
                        StringComparison.OrdinalIgnoreCase));
            }
            catch (WebDriverException)
            {
                Thread.Sleep(2000);
            }
        }

        private void SetCheckbox(By locator, bool shouldBeChecked)
        {
            var checkbox = driver.FindElements(locator)
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
            if (checkbox == null) return;

            bool isChecked = checkbox.Selected;
            if (isChecked != shouldBeChecked)
            {
                ScrollToCenter(checkbox);
                checkbox.Click();
                Thread.Sleep(300);
            }
        }

        private void BuscarClienteNuevaVenta(string cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente) || cliente == "00000000" || cliente.Trim() == "-")
            {
                Log("Cliente VARIOS / sin identificar - omitiendo busqueda.");
                return;
            }

            AbrirSeccionFacturacionSiNecesario();

            Log($"Buscando cliente: {cliente}");
            var input = Find(
                By.Id("DocumentoIdentidad"),
                VentasLocators.NuevaVenta.ClienteBuscar,
                VentasLocators.Customer.DocumentFieldByLabel
            );
            ScrollToCenter(input);
            input.Clear();
            input.SendKeys(cliente);
            Thread.Sleep(150); // pequeña pausa para que el campo registre el valor antes del clic
            try { Click(VentasLocators.NuevaVenta.ClienteLupa); }
            catch { input.SendKeys(Keys.Enter); }
            // Espera a que el comprobante este disponible en lugar de un sleep fijo de 2000ms.
            // El comprobante se habilita cuando la API de busqueda de cliente responde.
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(10)) { PollingInterval = TimeSpan.FromMilliseconds(200) }
                    .Until(_ =>
                        driver.FindElements(VentasLocators.NuevaVenta.ComprobanteChevron).Any(e => { try { return e.Displayed; } catch { return false; } })
                        || driver.FindElements(VentasLocators.NuevaVenta.ComprobanteChevronFallback).Any(e => { try { return e.Displayed; } catch { return false; } }));
            }
            catch (WebDriverTimeoutException) { Thread.Sleep(500); /* fallback si el comprobante tarda mas de lo esperado */ }
        }

        private void AbrirSeccionFacturacionSiNecesario()
        {
            bool visible = driver.FindElements(By.Id("DocumentoIdentidad"))
                .Any(e => { try { return e.Displayed; } catch { return false; } })
                || driver.FindElements(VentasLocators.NuevaVenta.ClienteBuscar)
                .Any(e => { try { return e.Displayed; } catch { return false; } });

            if (visible) return;

            Log("Abriendo seccion Facturacion...");
            TryClickOptional(
                By.XPath("//div[contains(@id,'heading-collapse-factur')]//button"),
                By.XPath("//button[contains(@class,'accordion-button')][contains(normalize-space(),'Facturaci')]")
            );
            // Espera a que el campo de cliente sea visible en lugar de un sleep fijo.
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(5)) { PollingInterval = TimeSpan.FromMilliseconds(150) }
                    .Until(_ =>
                        driver.FindElements(By.Id("DocumentoIdentidad")).Any(e => { try { return e.Displayed; } catch { return false; } })
                        || driver.FindElements(VentasLocators.NuevaVenta.ClienteBuscar).Any(e => { try { return e.Displayed; } catch { return false; } }));
            }
            catch (WebDriverTimeoutException) { /* si no aparece, el flujo siguiente fallara con mensaje claro */ }
        }

        private void SeleccionarComprobanteNuevaVenta(string comprobante)
        {
            if (string.IsNullOrWhiteSpace(comprobante)) return;
            string textoOpcion = NormalizarTextComprobante(comprobante);
            Log($"Seleccionando comprobante: {textoOpcion}");
            if (ComprobanteYaSeleccionadoNuevaVenta(textoOpcion))
            {
                Log($"Comprobante ya seleccionado: {textoOpcion}");
                return;
            }

            // Paso 1: abrir el dropdown con el chevron
            Click(
                VentasLocators.NuevaVenta.ComprobanteChevron,
                VentasLocators.NuevaVenta.ComprobanteChevronFallback
            );
            // Espera a que las opciones del dropdown sean visibles antes de intentar hacer clic.
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(5)) { PollingInterval = TimeSpan.FromMilliseconds(150) }
                    .Until(_ => driver.FindElements(VentasLocators.NuevaVenta.ComprobanteOpcion(textoOpcion))
                        .Any(e => { try { return e.Displayed; } catch { return false; } }));
            }
            catch (WebDriverTimeoutException) { Thread.Sleep(400); }

            // Paso 2: seleccionar la opcion
            Click(
                VentasLocators.NuevaVenta.ComprobanteOpcion(textoOpcion),
                VentasLocators.NuevaVenta.ComprobanteOpcionFallback(textoOpcion)
            );
            // Espera a que el comprobante quede seleccionado (Angular actualiza el FormControl).
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(5)) { PollingInterval = TimeSpan.FromMilliseconds(150) }
                    .Until(_ => ComprobanteYaSeleccionadoNuevaVenta(textoOpcion));
            }
            catch (WebDriverTimeoutException) { Thread.Sleep(400); }
        }

        private bool ComprobanteYaSeleccionadoNuevaVenta(string comprobante)
        {
            return driver.FindElements(By.XPath(
                    $"//label[@for='businessDocumentTypeId']/following::app-dropdown-search[1]//*[contains(normalize-space(),'{comprobante}')] | " +
                    $"//label[contains(normalize-space(),'COMPROBANTE') or contains(normalize-space(),'Comprobante')]/following::app-dropdown-search[1]//*[contains(normalize-space(),'{comprobante}')]"))
                .Any(e => { try { return e.Displayed; } catch { return false; } });
        }

        private static string NormalizarTextComprobante(string comprobante)
        {
            string t = (comprobante ?? "").Trim().ToUpperInvariant();
            if (t.Contains("NOTA DE VENTA")) return "NOTA DE VENTA(INTERNA)";
            if (t.Contains("FACTURA"))       return "FACTURA ELECTRONICA";
            if (t.Contains("BOLETA"))        return "BOLETA DE VENTA ELECTRONICA";
            return t;
        }

        private void SeleccionarSerieNuevaVenta(string serie)
        {
            if (string.IsNullOrWhiteSpace(serie) || serie.Trim() == "-") return;
            bool hayRadios =
                driver.FindElements(VentasLocators.Voucher.SeriesRadio)
                    .Any(e => { try { return e.Displayed; } catch { return false; } })
                || driver.FindElements(By.XPath(
                    "//div[contains(@id,'collapse-factur')]//input[@type='radio']"))
                    .Any(e => { try { return e.Displayed; } catch { return false; } });
            if (!hayRadios)
            {
                Log($"Serie auto-asignada (unica disponible). Serie esperada: {serie}");
                return;
            }
            Log($"Seleccionando serie: {serie}");
            Click(
                VentasLocators.NuevaVenta.SeriePorTexto(serie),
                VentasLocators.Voucher.SeriesByText(serie)
            );
            // Pausa minima para que Angular procese la seleccion del radio de serie.
            Thread.Sleep(200);
        }

        private void UpdatePayment(string pago)
        {
            if (string.IsNullOrWhiteSpace(pago)) return;

            if (TryResolvePendingGuideModal("Pago"))
            {
                if (IsGuideModalVisible())
                {
                    if (string.IsNullOrWhiteSpace(_lastObservedMessage))
                        _lastObservedMessage = CapturarValidaciones();

                    Log($"[Pago] La guia de remision sigue abierta - se omite configuracion de pago '{pago}'.");
                    return;
                }

                Thread.Sleep(500);
            }

            if (HasVisibleBlockingModal())
            {
                Log($"[Pago] Modal bloqueante activo - omitiendo configuracion de pago '{pago}'.");
                return;
            }

            var pagoNormalizado = NormalizeText(pago);

            if (pagoNormalizado == "contado")
            {
                Log("Configurando pago contado en Nueva Venta...");
                AbrirPagoNuevaVenta();
                SeleccionarTipoPagoNuevaVenta("contado");
                SeleccionarTabMedioPagoNuevaVenta("efectivo");
                return;
            }

            if (pagoNormalizado == "incompleto")
            {
                Log("Configurando pago incompleto en Nueva Venta...");
                AbrirPagoNuevaVenta();
                SeleccionarTipoPagoNuevaVenta("contado");
                SeleccionarTabMedioPagoNuevaVenta("efectivo");

                var amountInput = Find(VentasLocators.Payment.CashReceivedNewSale);
                LimpiarYEscribirCampoNuevaVenta(amountInput, ResolverMontoParcialNuevaVenta());
                return;
            }

            if (pagoNormalizado == "credito")
            {
                Log("Configurando pago a credito rapido en Nueva Venta...");
                AbrirPagoNuevaVenta();
                SeleccionarTipoPagoNuevaVenta("credito");
                return;
            }

            if (pagoNormalizado == "creditoinicial")
            {
                Log("Configurando pago a credito con monto inicial en Nueva Venta...");
                AbrirPagoNuevaVenta();
                SeleccionarTipoPagoNuevaVenta("credito");

                var montoInicial = Find(VentasLocators.Payment.CreditInitialAmountInput);
                var montoParcial = ResolverMontoParcialNuevaVenta();
                LimpiarYEscribirCampoNuevaVenta(montoInicial, montoParcial);

                var recibido = Find(VentasLocators.Payment.CashReceivedNewSale);
                LimpiarYEscribirCampoNuevaVenta(recibido, montoParcial);
            }
        }

        private string CaptureVisibleMessage(int timeoutSeconds)
        {
            var until = DateTime.UtcNow.AddSeconds(Math.Max(1, timeoutSeconds));
            while (DateTime.UtcNow <= until)
            {
                var message = CapturarMensajesResultadoVisibles()
                    .FirstOrDefault(EsMensajeResultadoVisible);

                if (!string.IsNullOrWhiteSpace(message))
                    return message;

                Thread.Sleep(300);
            }

            return string.Empty;
        }

        private IReadOnlyList<string> CapturarMensajesResultadoVisibles()
        {
            var mensajes = new List<string>();

            mensajes.AddRange(CapturarTextoSweetAlert());

            mensajes.AddRange(CapturarTextoAgrupadoPorContenedor(
                "[role='alertdialog']:not(.swal2-popup), .modal.show .modal-content, ngb-modal-window .modal-content, .modal-overlay .modal-content",
                ".modal-title, .modal-body, h1, h2, h3, h4, h5, h6, p"));

            var selectors = new[]
            {
                ".swal2-title",
                ".swal2-html-container",
                ".swal2-content",
                ".custom-error-message",
                ".toast",
                ".alert",
                "[role='alert']",
                "[role='alertdialog']",
                ".modal.show .modal-title",
                ".modal.show .modal-body",
                ".modal.show .modal-content h1",
                ".modal.show .modal-content h2",
                ".modal.show .modal-content h3",
                ".modal.show .modal-content h4",
                ".modal.show .modal-content h5",
                ".modal.show .modal-content h6",
                ".modal.show .modal-content p",
                "ngb-modal-window .modal-title",
                "ngb-modal-window .modal-body",
                "ngb-modal-window .modal-content h1",
                "ngb-modal-window .modal-content h2",
                "ngb-modal-window .modal-content h3",
                "ngb-modal-window .modal-content h4",
                "ngb-modal-window .modal-content h5",
                "ngb-modal-window .modal-content h6",
                "ngb-modal-window .modal-content p",
                ".modal-overlay .modal-content h1",
                ".modal-overlay .modal-content h2",
                ".modal-overlay .modal-content h3",
                ".modal-overlay .modal-content h4",
                ".modal-overlay .modal-content h5",
                ".modal-overlay .modal-content h6",
                ".modal-overlay .modal-content p"
            };

            mensajes.AddRange(driver.FindElements(By.CssSelector(string.Join(", ", selectors)))
                .Where(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                })
                .Select(e => LimpiarTextoVisible(e.Text))
                .Where(t => !string.IsNullOrWhiteSpace(t) && !EsTextoAccionModal(t))
                .ToList());

            return mensajes
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private IReadOnlyList<string> CapturarTextoSweetAlert()
        {
            var textos = new List<string>();

            foreach (var popup in driver.FindElements(By.CssSelector(".swal2-popup")))
            {
                try
                {
                    if (!popup.Displayed)
                        continue;
                }
                catch
                {
                    continue;
                }

                var partes = new List<string>();
                partes.AddRange(CapturarTextosDentroDe(popup, ".swal2-title"));
                partes.AddRange(CapturarTextosDentroDe(popup, ".swal2-html-container"));
                partes.AddRange(CapturarTextosDentroDe(popup, ".swal2-validation-message"));

                if (!partes.Any())
                    partes.AddRange(CapturarTextosDentroDe(popup, ".swal2-content"));

                var texto = string.Join(" | ", QuitarPartesCompuestasDuplicadas(partes
                    .Select(RemoverAccionesModal)
                    .Where(t => !string.IsNullOrWhiteSpace(t) && !EsTextoAccionModal(t))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList()));

                if (!string.IsNullOrWhiteSpace(texto))
                    textos.Add(texto);
            }

            return textos;
        }

        private IReadOnlyList<string> CapturarTextoAgrupadoPorContenedor(string contenedorSelector, string contenidoSelector)
        {
            return driver.FindElements(By.CssSelector(contenedorSelector))
                .Where(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                })
                .Select(contenedor =>
                {
                    var partes = contenedor.FindElements(By.CssSelector(contenidoSelector))
                        .Where(e =>
                        {
                            try { return e.Displayed; }
                            catch { return false; }
                        })
                        .Select(e => RemoverAccionesModal(LimpiarTextoVisible(e.Text)))
                        .Where(t => !string.IsNullOrWhiteSpace(t) && !EsTextoAccionModal(t))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    return partes.Any()
                        ? string.Join(" | ", QuitarPartesCompuestasDuplicadas(partes))
                        : LimpiarTextoVisible(contenedor.Text);
                })
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToList();
        }

        private static IReadOnlyList<string> CapturarTextosDentroDe(IWebElement contenedor, string selector)
        {
            return contenedor.FindElements(By.CssSelector(selector))
                .Where(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                })
                .Select(e => RemoverAccionesModal(LimpiarTextoVisible(e.Text)))
                .Where(t => !string.IsNullOrWhiteSpace(t) && !EsTextoAccionModal(t))
                .ToList();
        }

        private string CapturarDiagnosticoMensajesResultado()
        {
            var mensajes = CapturarMensajesResultadoVisibles()
                .Take(5)
                .ToList();

            return mensajes.Any()
                ? string.Join(" | ", mensajes)
                : "sin textos visibles en modales/popup";
        }

        private static bool EsMensajeResultadoVisible(string? texto)
        {
            var normalizado = NormalizeText(texto ?? string.Empty);
            if (string.IsNullOrWhiteSpace(normalizado))
                return false;

            if (EsMensajeTransitorioGuardado(normalizado))
                return false;

            return normalizado.Contains("registr") ||
                   normalizado.Contains("correct") ||
                   normalizado.Contains("exito") ||
                   normalizado.Contains("exitos") ||
                   normalizado.Contains("guard") ||
                   normalizado.Contains("complet") ||
                   normalizado.Contains("error") ||
                   normalizado.Contains("inconsisten") ||
                   normalizado.Contains("obligatorio") ||
                   normalizado.Contains("requerido") ||
                   normalizado.Contains("no se pudo") ||
                   normalizado.Contains("no debe") ||
                   normalizado.Contains("invalido");
        }

        private static bool EsMensajeExitoVenta(string? texto)
        {
            var normalizado = NormalizeText(texto ?? string.Empty);
            if (string.IsNullOrWhiteSpace(normalizado) || EsMensajeTransitorioGuardado(normalizado))
                return false;

            return normalizado.Contains("correct") ||
                   normalizado.Contains("exito") ||
                   normalizado.Contains("exitos") ||
                   normalizado.Contains("registro") ||
                   normalizado.Contains("registrado") ||
                   normalizado.Contains("registrada");
        }

        private static bool EsMensajeTransitorioGuardado(string textoNormalizado)
        {
            return textoNormalizado.Contains("por favor espere") ||
                   textoNormalizado.Contains("registrando") ||
                   textoNormalizado.Contains("procesando") ||
                   textoNormalizado.Contains("cargando");
        }

        private static string LimpiarTextoVisible(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            return Regex.Replace(texto.Trim(), @"\s+", " ");
        }

        private static string RemoverAccionesModal(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            return LimpiarTextoVisible(Regex.Replace(texto, @"\b(OK|Aceptar|Cancelar|Cerrar)\b", string.Empty, RegexOptions.IgnoreCase));
        }

        private static IReadOnlyList<string> QuitarPartesCompuestasDuplicadas(IReadOnlyList<string> partes)
        {
            var limpias = partes
                .Select(LimpiarTextoVisible)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return limpias
                .Where(parte =>
                {
                    var normalizada = NormalizeText(parte);
                    var partesContenidas = limpias
                        .Where(otra => !otra.Equals(parte, StringComparison.OrdinalIgnoreCase))
                        .Count(otra => normalizada.Contains(NormalizeText(otra)));

                    return partesContenidas < 2;
                })
                .ToList();
        }

        private static bool EsTextoAccionModal(string texto)
        {
            var normalizado = NormalizeText(texto);
            return normalizado == "ok" ||
                   normalizado == "aceptar" ||
                   normalizado == "cancelar" ||
                   normalizado == "cerrar";
        }

        private void TryCloseSuccessDialog()
        {
            // 1. Cerrar popup "Correcto / Se registro correctamente" (boton OK)
            var okButton = driver.FindElements(By.XPath("//button[normalize-space()='OK' or contains(@class,'ok-button')]"))
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (okButton != null)
            {
                try
                {
                    ScrollToCenter(okButton);
                    okButton.Click();
                    Thread.Sleep(800);
                }
                catch
                {
                    // Si no se puede cerrar el popup OK, continua.
                }
            }

            // 2. Cerrar modal "Venta registrada XXXX" (boton Cancelar)
            //    Este modal aparece justo despues del OK para ofrecer envio por correo/WhatsApp.
            var cancelButton = driver.FindElements(By.XPath("//button[normalize-space()='Cancelar']"))
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (cancelButton != null)
            {
                try
                {
                    cancelButton.Click();
                    Thread.Sleep(800);
                }
                catch { }
            }
        }

        private bool HasVisibleBlockingModal()
        {
            return driver.FindElements(By.CssSelector(".modal-overlay, .modal.show, ngb-modal-window, .swal2-container, .cdk-overlay-backdrop"))
                .Any(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });
        }

        private bool IsGuideModalVisible()
        {
            if (!HasVisibleBlockingModal())
                return false;

            bool hasAccept = driver.FindElements(By.XPath("//button[normalize-space()='Aceptar']"))
                .Any(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });

            bool hasCancel = driver.FindElements(By.XPath("//button[normalize-space()='Cancelar']"))
                .Any(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });

            bool hasGuideFields = driver.FindElements(By.XPath(
                    "//input[@type='date'] | " +
                    "//label[contains(normalize-space(),'Peso') or contains(normalize-space(),'Bulto') or " +
                    "contains(normalize-space(),'LICENCIA') or contains(normalize-space(),'PLACA') or " +
                    "contains(normalize-space(),'Transportista') or contains(normalize-space(),'transporte')]"))
                .Any(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });

            return hasAccept && hasCancel && hasGuideFields;
        }

        private bool TryResolvePendingGuideModal(string origin)
        {
            if (!IsGuideModalVisible())
                return false;

            Log($"[{origin}] Guia de remision pendiente detectada - intentando confirmar el modal.");

            var acceptButton = driver.FindElements(By.XPath("//button[normalize-space()='Aceptar']"))
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (acceptButton == null)
            {
                if (string.IsNullOrWhiteSpace(_lastObservedMessage))
                    _lastObservedMessage = CapturarValidaciones();

                Log($"[{origin}] No se encontro el boton Aceptar de la guia. Mensaje actual: '{_lastObservedMessage}'");
                return true;
            }

            try
            {
                ScrollToCenter(acceptButton);
                acceptButton.Click();
            }
            catch
            {
                try
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", acceptButton);
                }
                catch
                {
                }
            }

            bool guideClosed = false;
            try
            {
                guideClosed = new WebDriverWait(driver, TimeSpan.FromSeconds(4))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(200)
                }.Until(_ => !IsGuideModalVisible());
            }
            catch
            {
                guideClosed = false;
            }

            if (!guideClosed)
            {
                var validation = CapturarValidaciones();
                if (string.IsNullOrWhiteSpace(validation))
                    validation = CaptureVisibleMessage(1);

                if (!string.IsNullOrWhiteSpace(validation))
                    _lastObservedMessage = validation;

                Log($"[{origin}] La guia de remision sigue abierta tras confirmar. Mensaje actual: '{_lastObservedMessage}'");
            }
            else
            {
                var successMessage = CaptureVisibleMessage(2);
                if (!string.IsNullOrWhiteSpace(successMessage) && EsMensajeConfirmacionGuia(successMessage))
                    _lastObservedMessage = successMessage;

                var resumenGuia = CapturarResumenGuiaVisible();
                _guiaEvidenciaConfirmacion = ConstruirEvidenciaGuiaCompacta(_lastObservedMessage, resumenGuia);
                _guiaConfirmadaAntesDeGuardar = !string.IsNullOrWhiteSpace(_guiaEvidenciaConfirmacion);

                Log($"[{origin}] Guia de remision confirmada antes de guardar venta. {_guiaEvidenciaConfirmacion}");
            }

            return true;
        }

        private bool TryHandleBlockingModal()
        {
            if (TryResolvePendingGuideModal("Modal bloqueante"))
                return true;

            bool hayModal = HasVisibleBlockingModal();
            if (!hayModal) return false;

            Log("Modal bloqueante detectado - capturando mensaje y cerrando.");
            var msg = driver.FindElements(By.CssSelector(".modal-overlay p, .modal-content p, .modal-body p"))
                .Where(e => { try { return e.Displayed; } catch { return false; } })
                .Select(e => e.Text?.Trim())
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t))
                ?? CaptureVisibleMessage(1);

            if (!string.IsNullOrWhiteSpace(msg) && string.IsNullOrWhiteSpace(_lastObservedMessage))
                _lastObservedMessage = msg;

            var okBtn = driver.FindElements(By.XPath("//button[normalize-space()='OK' or normalize-space()='Aceptar']"))
                .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
            if (okBtn != null)
            {
                try
                {
                    ScrollToCenter(okBtn);
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", okBtn);
                }
                catch { }

                try
                {
                    new WebDriverWait(driver, TimeSpan.FromSeconds(3))
                    {
                        PollingInterval = TimeSpan.FromMilliseconds(200)
                    }.Until(_ => !HasVisibleBlockingModal());
                }
                catch
                {
                    Thread.Sleep(800);
                }
            }
            return true;
        }

        private void AbrirPagoNuevaVenta()
        {
            if (EstaSeccionPagoVisibleNuevaVenta())
                return;

            var trigger = ObtenerTriggerAccordionPagoNuevaVenta();
            Assert.That(trigger, Is.Not.Null,
                "No se encontro un trigger seguro para abrir la seccion Pago en Nueva Venta.");

            ClickSeguroNuevaVenta(trigger!, preservarComoBoton: true);

            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(_ => EstaSeccionPagoVisibleNuevaVenta());
        }

        private bool EstaSeccionPagoVisibleNuevaVenta()
        {
            return driver.FindElements(VentasLocators.Payment.PaymentBody)
                .Any(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });
        }

        private IWebElement? ObtenerTriggerAccordionPagoNuevaVenta()
        {
            foreach (var locator in new[]
                     {
                         VentasLocators.Payment.PaymentAccordionButton,
                         VentasLocators.Payment.PaymentAccordionButtonFallback,
                         VentasLocators.Payment.PaymentAccordionHeader
                     })
            {
                var candidato = driver.FindElements(locator)
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed && e.Enabled; }
                        catch { return false; }
                    });

                if (candidato == null)
                    continue;

                return ResolverElementoInteractivoNuevaVenta(candidato);
            }

            return null;
        }

        private IWebElement ResolverElementoInteractivoNuevaVenta(IWebElement candidato)
        {
            try
            {
                var interactivo = candidato.FindElements(By.XPath(
                        ".//button[not(@disabled)] | .//a | .//*[@role='button' or @role='tab']"))
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed && e.Enabled; }
                        catch { return false; }
                    });

                if (interactivo != null)
                    return interactivo;
            }
            catch
            {
            }

            return candidato;
        }

        private void ClickSeguroNuevaVenta(IWebElement element, bool preservarComoBoton = false)
        {
            ScrollToCenter(element);

            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript(@"
                    const el = arguments[0];
                    const preserveButton = arguments[1];
                    if (!el) return;

                    if (preserveButton && el.tagName === 'BUTTON' && !el.getAttribute('type'))
                        el.setAttribute('type', 'button');

                    if (typeof el.focus === 'function')
                        el.focus();

                    el.dispatchEvent(new MouseEvent('click', {
                        bubbles: true,
                        cancelable: true,
                        view: window
                    }));
                ", element, preservarComoBoton);
            }
            catch
            {
                try
                {
                    element.Click();
                }
                catch
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
                }
            }

            Thread.Sleep(400);
        }

        private bool PerteneceAlContenedorNuevaVenta(IWebElement contenedor, IWebElement elemento)
        {
            try
            {
                var resultado = ((IJavaScriptExecutor)driver).ExecuteScript(
                    "return arguments[0] === arguments[1] || arguments[0].contains(arguments[1]);",
                    contenedor,
                    elemento);

                return resultado is bool pertenece && pertenece;
            }
            catch
            {
                return false;
            }
        }

        private IWebElement? FindFirstVisibleInPayment(params By[] locators)
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);

            foreach (var locator in locators)
            {
                var candidato = driver.FindElements(locator)
                    .FirstOrDefault(e =>
                    {
                        try
                        {
                            return e.Displayed &&
                                   e.Enabled &&
                                   (contenedorPago == null || PerteneceAlContenedorNuevaVenta(contenedorPago, e));
                        }
                        catch
                        {
                            return false;
                        }
                    });

                if (candidato != null)
                    return candidato;
            }

            return null;
        }

        private IWebElement? FindLastVisibleInPayment(params By[] locators)
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);

            foreach (var locator in locators)
            {
                var visibles = driver.FindElements(locator)
                    .Where(e =>
                    {
                        try
                        {
                            return e.Displayed &&
                                   e.Enabled &&
                                   (contenedorPago == null || PerteneceAlContenedorNuevaVenta(contenedorPago, e));
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .ToList();

                if (visibles.Any())
                    return visibles.Last();
            }

            return null;
        }

        private sealed class DiscountState
        {
            public int CantidadFilas { get; init; }
            public bool ModoVentaVisible { get; init; }
            public bool DescuentoMarcado { get; init; }
            public bool ItemActivo { get; init; }
            public bool GlobalActivo { get; init; }
            public bool ModoMontoActivo { get; init; }
            public bool ModoPorcentajeActivo { get; init; }
            public bool InputDescuentoHabilitado { get; init; }
            public bool InputDescuentoInvalido { get; init; }
            public string ValorDescuentoRaw { get; init; } = string.Empty;
            public string ValorDescuentoNormalizado { get; init; } = string.Empty;
            public decimal? TotalActual { get; init; }
            public string MensajeValidacion { get; init; } = string.Empty;
            public bool HayErrorVisible => !string.IsNullOrWhiteSpace(MensajeValidacion);
        }

        private sealed class DiscountContext
        {
            public static DiscountContext Empty { get; } = new();

            public bool Activo { get; init; }
            public string Tipo { get; init; } = string.Empty;
            public string Modo { get; init; } = string.Empty;
            public decimal? Valor { get; init; }
            public decimal? TotalAntes { get; init; }
        }

        private sealed class PaymentContext
        {
            public static PaymentContext Empty { get; } = new();

            public bool Configurado { get; init; }
            public string TipoPago { get; init; } = string.Empty;
            public bool Multipago { get; init; }
            public List<string> Medios { get; init; } = new();
            public List<string> Bancos { get; init; } = new();
            public List<string> Tarjetas { get; init; } = new();
            public List<string> Cuentas { get; init; } = new();
            public List<string> Operaciones { get; init; } = new();
            public List<decimal?> Montos { get; init; } = new();
            public decimal? TotalAntes { get; init; }
            public decimal? MontoInicialCredito { get; init; }
        }

        private sealed class PaymentInstruction
        {
            public string MedioPago { get; init; } = string.Empty;
            public string Banco { get; init; } = "NA";
            public string Tarjeta { get; init; } = "NA";
            public string CuentaBancaria { get; init; } = "NA";
            public string Operacion { get; init; } = "NA";
            public string MontoConfigurado { get; init; } = string.Empty;
            public decimal? MontoEsperado { get; init; }
        }

        private sealed class PointsPaymentState
        {
            public decimal? PuntosAcumulados { get; init; }
            public decimal? SolesAcumulados { get; init; }
            public decimal? PuntosRestantes { get; init; }
            public decimal? SolesRestantes { get; init; }
        }

        private DiscountState CapturarEstadoDescuento()
        {
            Thread.Sleep(700);

            var filas = driver.FindElements(By.XPath("//table//tbody/tr[td]"))
                .Count(e => { try { return e.Displayed; } catch { return false; } });

            var inputDescuento = FindFirstVisibleOrAny(
                DiscountValueInputLocator,
                VentasLocators.Discount.GlobalValueInput
            );

            var valorRaw = inputDescuento?.GetAttribute("value")?.Trim() ?? string.Empty;
            var total = ObtenerTotalVentaActual();
            var mensaje = CapturarValidaciones();

            var estado = new DiscountState
            {
                CantidadFilas = filas,
                ModoVentaVisible = FindFirstVisibleOrAny(VentasLocators.NuevaVenta.ModoVenta("VENTA NORMAL")) != null,
                DescuentoMarcado = EstaMarcado(VentasLocators.Detail.DiscountCheckbox)
                    || inputDescuento != null
                    || EstaActivo(VentasLocators.Discount.ItemScope)
                    || EstaActivo(VentasLocators.Discount.GlobalScope),
                ItemActivo = EstaActivo(VentasLocators.Discount.ItemScope),
                GlobalActivo = EstaActivo(VentasLocators.Discount.GlobalScope),
                ModoMontoActivo = EstaActivo(DiscountAmountModeLocator),
                ModoPorcentajeActivo = EstaActivo(DiscountPercentageModeLocator),
                InputDescuentoHabilitado = inputDescuento != null && inputDescuento.Enabled,
                InputDescuentoInvalido = EsCampoInvalido(inputDescuento),
                ValorDescuentoRaw = valorRaw,
                ValorDescuentoNormalizado = NormalizarNumero(valorRaw),
                TotalActual = total,
                MensajeValidacion = mensaje
            };

            Log($"[Descuento] Filas={estado.CantidadFilas} Check={estado.DescuentoMarcado} Item={estado.ItemActivo} Global={estado.GlobalActivo} " +
                $"Monto={estado.ModoMontoActivo} Porcentaje={estado.ModoPorcentajeActivo} Valor='{estado.ValorDescuentoRaw}' " +
                $"Total={estado.TotalActual?.ToString(CultureInfo.InvariantCulture) ?? "NA"} Mensaje='{estado.MensajeValidacion}'");

            return estado;
        }

        private IWebElement? FindFirstVisibleOrAny(params By[] locators)
        {
            foreach (var loc in locators)
            {
                var visible = driver.FindElements(loc)
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    });
                if (visible != null)
                    return visible;

                var any = driver.FindElements(loc).FirstOrDefault();
                if (any != null)
                    return any;
            }

            return null;
        }

        private bool EstaMarcado(By locator)
        {
            var element = FindFirstVisibleOrAny(locator);
            if (element == null) return false;

            try
            {
                if (element.TagName.Equals("input", StringComparison.OrdinalIgnoreCase))
                    return element.Selected ||
                           string.Equals(element.GetAttribute("checked"), "true", StringComparison.OrdinalIgnoreCase) ||
                           string.Equals(element.GetAttribute("aria-checked"), "true", StringComparison.OrdinalIgnoreCase);

                var js = (IJavaScriptExecutor)driver;
                var result = js.ExecuteScript(@"
                    const el = arguments[0];
                    if (!el) return false;
                    const input = el.matches('input') ? el : el.closest('label')?.querySelector('input') || el.previousElementSibling;
                    if (!input) return false;
                    return !!(input.checked || input.getAttribute('checked') === 'true' || input.getAttribute('aria-checked') === 'true');
                ", element);

                return result is bool b && b;
            }
            catch
            {
                return false;
            }
        }

        private bool EstaActivo(By locator)
        {
            var element = FindFirstVisibleOrAny(locator);
            if (element == null) return false;

            try
            {
                var classes = NormalizeText(element.GetAttribute("class") ?? string.Empty);
                var ariaPressed = NormalizeText(element.GetAttribute("aria-pressed") ?? string.Empty);
                var ariaSelected = NormalizeText(element.GetAttribute("aria-selected") ?? string.Empty);

                if (classes.Contains("active") ||
                    classes.Contains("selected") ||
                    ariaPressed == "true" ||
                    ariaSelected == "true")
                {
                    return true;
                }

                var js = (IJavaScriptExecutor)driver;
                var result = js.ExecuteScript(@"
                    const el = arguments[0];
                    if (!el) return false;

                    const ownClass = (el.getAttribute('class') || '').toLowerCase();
                    if (ownClass.includes('active') || ownClass.includes('selected')) return true;

                    const container = el.closest('label, button, .btn, .toggle, .input-group, .option, .radio-row');
                    const containerClass = (container?.getAttribute('class') || '').toLowerCase();
                    if (containerClass.includes('active') || containerClass.includes('selected')) return true;

                    const input = el.matches('input')
                        ? el
                        : el.querySelector('input[type=radio],input[type=checkbox]')
                            || container?.querySelector('input[type=radio],input[type=checkbox]')
                            || el.previousElementSibling
                            || el.closest('label')?.querySelector('input[type=radio],input[type=checkbox]');

                    if (!input) return false;

                    return !!(input.checked ||
                              input.getAttribute('checked') === 'true' ||
                              input.getAttribute('aria-checked') === 'true');
                ", element);

                return result is bool activo && activo;
            }
            catch
            {
                return false;
            }
        }

        private bool EsCampoInvalido(IWebElement? element)
        {
            if (element == null) return false;

            try
            {
                var classes = NormalizeText(element.GetAttribute("class") ?? string.Empty);
                var ariaInvalid = NormalizeText(element.GetAttribute("aria-invalid") ?? string.Empty);

                return classes.Contains("invalid") || ariaInvalid == "true";
            }
            catch
            {
                return false;
            }
        }

        private decimal? ObtenerTotalVentaActual(bool incluirMontoPago = true)
        {
            var candidatos = new List<string>
            {
                LeerValor(By.XPath("//*[normalize-space()='Total']/following::*[contains(normalize-space(),'S/') or contains(normalize-space(),'$')][1]")),
                LeerValor(By.XPath("//*[normalize-space()='Subtotal']/following::*[contains(normalize-space(),'S/') or contains(normalize-space(),'$')][1]")),
                LeerValor(By.XPath("//label[contains(normalize-space(),'Total')]/following::input[1]")),
                LeerValor(By.XPath("//label[contains(normalize-space(),'Importe total')]/following::input[1]")),
                LeerValor(By.XPath("//*[contains(normalize-space(),'Importe total') or contains(normalize-space(),'Total de venta')]/following::*[self::span or self::div or self::input][1]")),
                LeerValor(By.XPath("//*[contains(@class,'total') or contains(@class,'amount')][normalize-space()]"))
            };

            if (incluirMontoPago)
                candidatos.Add(LeerValor(VentasLocators.Payment.CashAmount));

            decimal? ceroCapturado = null;
            foreach (var candidato in candidatos)
            {
                if (!TryParseUltimoDecimalFlexible(candidato, out var valor))
                    continue;

                if (valor != 0m || candidato.Contains('-'))
                    return valor;

                ceroCapturado ??= valor;
            }

            var totalDetalle = ObtenerTotalVentaDesdeDetalle();
            if (totalDetalle.HasValue && totalDetalle.Value > 0m)
                return totalDetalle.Value;

            if (ceroCapturado.HasValue)
                return ceroCapturado.Value;

            var importes = driver.FindElements(VentasLocators.Detail.PriceInputs)
                .Select(e => e.GetAttribute("value") ?? e.Text ?? string.Empty)
                .Select(texto => TryParseDecimalFlexible(texto, out var valor) ? valor : (decimal?)null)
                .Where(v => v.HasValue)
                .Select(v => v!.Value)
                .ToList();

            if (importes.Count > 0)
                return importes.Sum();

            return null;
        }

        private decimal? ObtenerTotalVentaDesdeDetalle()
        {
            try
            {
                var cantidades = driver.FindElements(VentasLocators.Detail.QuantityInputs).ToList();
                var precios = driver.FindElements(VentasLocators.Detail.PriceInputs).ToList();
                var filas = Math.Min(cantidades.Count, precios.Count);

                if (filas == 0)
                    return null;

                decimal total = 0m;
                int filasValidas = 0;

                for (int i = 0; i < filas; i++)
                {
                    var cantidadTexto = cantidades[i].GetAttribute("value") ?? cantidades[i].Text ?? string.Empty;
                    var precioTexto = precios[i].GetAttribute("value") ?? precios[i].Text ?? string.Empty;

                    if (!TryParseDecimalFlexible(cantidadTexto, out var cantidad) ||
                        !TryParseDecimalFlexible(precioTexto, out var precio) ||
                        cantidad <= 0m ||
                        precio < 0m)
                    {
                        continue;
                    }

                    total += cantidad * precio;
                    filasValidas++;
                }

                return filasValidas > 0 ? Math.Round(total, 2) : (decimal?)null;
            }
            catch
            {
                return null;
            }
        }

        private decimal? EsperarTotalVentaDisponibleNuevaVenta(int timeoutSeconds = 6)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(200)
                }.Until(_ =>
                {
                    var total = ObtenerTotalVentaActual(incluirMontoPago: false);
                    if (total.HasValue && total.Value > 0m)
                        return total;

                    var totalDetalle = ObtenerTotalVentaDesdeDetalle();
                    return totalDetalle.HasValue && totalDetalle.Value > 0m ? totalDetalle : null;
                });
            }
            catch
            {
                return ObtenerTotalVentaActual(incluirMontoPago: false)
                    ?? ObtenerTotalVentaDesdeDetalle()
                    ?? ObtenerTotalVentaActual();
            }
        }

        private string ResolverMontoParcialNuevaVenta()
        {
            var total = EsperarTotalVentaDisponibleNuevaVenta()
                ?? ObtenerTotalVentaActual(incluirMontoPago: false)
                ?? ObtenerTotalVentaActual();

            Assert.That(total.HasValue && total.Value > 0m, Is.True,
                "No se pudo obtener el total actual de la venta para resolver un monto parcial.");

            var montoParcial = Math.Round(total!.Value / 2m, 2, MidpointRounding.AwayFromZero);
            if (montoParcial <= 0m || montoParcial >= total.Value)
                montoParcial = Math.Round(Math.Max(total.Value - 0.01m, 0.01m), 2, MidpointRounding.AwayFromZero);

            return montoParcial.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private string LeerValor(By locator)
        {
            var element = FindFirstVisibleOrAny(locator);
            if (element == null) return string.Empty;

            try
            {
                return (element.GetAttribute("value") ?? element.Text ?? string.Empty).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string NormalizarNumero(string value)
        {
            if (!TryParseDecimalFlexible(value, out var parsed))
                return string.Empty;

            return parsed.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static bool TryParseDecimalFlexible(string? value, out decimal parsed)
        {
            parsed = 0m;
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var sanitized = new string(value
                .Where(c => char.IsDigit(c) || c == ',' || c == '.' || c == '-')
                .ToArray());

            if (string.IsNullOrWhiteSpace(sanitized))
                return false;

            if (sanitized.Contains(',') && sanitized.Contains('.'))
            {
                var lastComma = sanitized.LastIndexOf(',');
                var lastDot = sanitized.LastIndexOf('.');
                sanitized = lastDot > lastComma
                    ? sanitized.Replace(",", string.Empty)
                    : sanitized.Replace(".", string.Empty).Replace(",", ".");
            }
            else if (sanitized.Count(c => c == ',') == 1 && sanitized.Count(c => c == '.') == 0)
            {
                sanitized = sanitized.Replace(",", ".");
            }
            else if (sanitized.Count(c => c == '.') > 1)
            {
                var lastDot = sanitized.LastIndexOf('.');
                sanitized = sanitized[..lastDot].Replace(".", string.Empty) + sanitized[lastDot..];
            }

            return decimal.TryParse(sanitized, NumberStyles.Number | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out parsed);
        }

        private static void AssertMontoAproximado(decimal? actual, decimal esperado, string mensaje)
        {
            Assert.That(actual.HasValue, Is.True, $"{mensaje} No se pudo capturar el total actual de la venta.");
            Assert.That(actual!.Value, Is.EqualTo(esperado).Within(0.05m), $"{mensaje} Total actual: {actual:0.00}");
        }

        private static bool TryParseUltimoDecimalFlexible(string? value, out decimal parsed)
        {
            parsed = 0m;
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var matches = Regex.Matches(value, @"-?\d+(?:[.,]\d+)?");
            if (matches.Count == 0)
                return false;

            return TryParseDecimalFlexible(matches[^1].Value, out parsed);
        }

        private static bool DebeActivarOpcion(string value)
        {
            var normalizado = NormalizeText(value);
            return normalizado is "true" or "1" or "y" or "yes" or "si";
        }

        private PaymentContext ObtenerContextoPago(string resultadoEsperado)
        {
            Assert.That(_paymentContext.Configurado, Is.True,
                $"La seccion Pago se pudo inspeccionar, pero no se encontro un contexto configurado en Nueva Venta para validar '{resultadoEsperado}'. Revise que el step 'el usuario configura los medios de pago ...' este resolviendo al binding scoped de NuevaVenta.");
            return _paymentContext;
        }

        private void AssertTipoPagoSeleccionadoNuevaVenta(PaymentContext contexto)
        {
            if (contexto.TipoPago.Contains("contado"))
            {
                Assert.That(EstaMarcado(VentasLocators.Payment.CashType), Is.True,
                    "El tipo de pago Contado deberia quedar seleccionado correctamente.");
            }
            else if (contexto.TipoPago.Contains("credito"))
            {
                Assert.That(EstaMarcado(VentasLocators.Payment.QuickCreditType), Is.True,
                    "El tipo de pago Credito deberia quedar seleccionado correctamente.");
            }
        }

        private void SeleccionarTipoPagoNuevaVenta(string tipoPago)
        {
            if (NormalizeText(tipoPago).Contains("contado"))
            {
                SeleccionarRadioPagoNuevaVenta(
                    "contado",
                    VentasLocators.Payment.CashType,
                    VentasLocators.Payment.CashTypeLabelText,
                    VentasLocators.Payment.CashTypeLabel,
                    VentasLocators.Payment.CashType);
                return;
            }

            if (NormalizeText(tipoPago).Contains("credito"))
            {
                SeleccionarRadioPagoNuevaVenta(
                    "credito",
                    VentasLocators.Payment.QuickCreditType,
                    VentasLocators.Payment.CreditTypeLabelText,
                    VentasLocators.Payment.QuickCreditTypeLabel,
                    VentasLocators.Payment.QuickCreditType);
            }
        }

        private void SeleccionarRadioPagoNuevaVenta(string descripcion, By radioLocator, params By[] locators)
        {
            AbrirPagoNuevaVenta();

            if (EstaMarcado(radioLocator))
                return;

            var objetivo = FindFirstVisibleInPayment(locators.Append(radioLocator).ToArray());
            Assert.That(objetivo, Is.Not.Null,
                $"No se encontro la opcion '{descripcion}' dentro de la seccion Pago de Nueva Venta.");

            ClickSeguroNuevaVenta(objetivo!, preservarComoBoton: true);

            var seleccionado = new WebDriverWait(driver, TimeSpan.FromSeconds(6))
            {
                PollingInterval = TimeSpan.FromMilliseconds(150)
            }.Until(_ => EstaMarcado(radioLocator));

            Assert.That(seleccionado, Is.True,
                $"La opcion '{descripcion}' no quedo seleccionada en el bloque Pago de Nueva Venta.");
        }

        private void ConfigurarMultipagoNuevaVenta(bool activar)
        {
            var chk = FindFirstVisibleOrAny(VentasLocators.Payment.MultipaymentCheckbox);
            Assert.That(chk, Is.Not.Null, "No se encontro el check Multipago en Nueva Venta.");

            bool marcado = false;
            try
            {
                marcado = chk!.Selected ||
                          string.Equals(chk.GetAttribute("checked"), "true", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                marcado = false;
            }

            if (marcado == activar)
                return;

            ScrollToCenter(chk!);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", chk);
            Thread.Sleep(500);
        }

        private void IngresarNumeroCuotasNuevaVenta(string nroCuotas)
        {
            var input = Find(By.XPath("//input[@type='number'][@min='1'][@max='60']"));
            LimpiarYEscribirCampoNuevaVenta(input, nroCuotas.Trim());
        }

        private void IngresarMontoInicialCreditoNuevaVenta(string monto)
        {
            var input = Find(VentasLocators.Payment.CreditInitialAmountInput);
            LimpiarYEscribirCampoNuevaVenta(input, ResolverMontoPago(monto));
        }

        private void SeleccionarTabMedioPagoNuevaVenta(string medioPago)
        {
            switch (NormalizeText(medioPago))
            {
                case "efectivo":
                    ClickTabPagoNuevaVenta("EFECTIVO",
                        VentasLocators.Payment.CashMethod,
                        VentasLocators.Payment.CashMethodFallback,
                        By.XPath("//span[normalize-space()='EFECTIVO']"));
                    break;
                case "tarjeta_credito":
                    ClickTabPagoNuevaVenta("TARJETAS DE CREDITO",
                        VentasLocators.Payment.CreditMethod,
                        By.XPath("//span[normalize-space()='TARJETAS DE CREDITO']"));
                    break;
                case "tarjeta_debito":
                    ClickTabPagoNuevaVenta("TARJETAS DE DEBITO",
                        VentasLocators.Payment.DebitMethod,
                        By.XPath("//span[normalize-space()='TARJETAS DE DEBITO']"));
                    break;
                case "transferencia_fondos":
                    ClickTabPagoNuevaVenta("TRANSFERENCIA DE FONDOS",
                        VentasLocators.Payment.TransferMethod,
                        By.XPath("//span[normalize-space()='TRANSFERENCIA DE FONDOS']"));
                    break;
                case "deposito_cuenta":
                    ClickTabPagoNuevaVenta("DEPOSITOS EN CUENTA",
                        VentasLocators.Payment.DepositMethod,
                        By.XPath("//span[normalize-space()='DEPOSITOS EN CUENTA']"));
                    break;
                case "puntos":
                    ClickTabPagoNuevaVenta("PUNTOS",
                        VentasLocators.Payment.PointsMethod,
                        By.XPath("//span[normalize-space()='PUNTOS']"));
                    break;
                case "nota_credito":
                    ClickTabPagoNuevaVenta("NOTA DE CREDITO",
                        VentasLocators.Payment.CreditNoteMethod,
                        By.XPath("//span[normalize-space()='NOTA DE CREDITO' or normalize-space()='NOTA DE CRÉDITO']"));
                    break;
                default:
                    throw new Exception($"Medio de pago no soportado en Nueva Venta: {medioPago}");
            }

            Thread.Sleep(500);
        }

        private void ClickTabPagoNuevaVenta(string textoEsperado, params By[] locators)
        {
            if (EsperarTabPagoNuevaVentaLista(textoEsperado))
                return;

            var candidatos = ObtenerCandidatosTabPagoNuevaVenta(textoEsperado, locators);
            Assert.That(candidatos.Count, Is.GreaterThan(0),
                $"No se encontro ningun tab visible para '{textoEsperado}' en Nueva Venta.");

            foreach (var candidato in candidatos)
            {
                var objetivo = ResolverObjetivoTabPagoNuevaVenta(candidato);
                EjecutarClickTabPagoNuevaVenta(objetivo);

                if (EsperarTabPagoNuevaVentaLista(textoEsperado))
                    return;
            }

            var resumen = ConstruirResumenPagoNuevaVenta($"tab_esperado={textoEsperado}");
            Log($"[PagoNV] {resumen}");
            Assert.Fail($"No se pudo activar el tab '{textoEsperado}' en Nueva Venta. {resumen}");
        }

        private bool EsperarTabPagoNuevaVentaLista(string textoEsperado)
        {
            return EsperarTabPagoNuevaVentaActiva(textoEsperado) &&
                   EsperarContenidoPagoNuevaVentaVisible(textoEsperado);
        }

        private bool EsTabPagoActiva(string textoEsperado)
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
            if (contenedorPago != null)
            {
                try
                {
                    var activaEnContenedor = contenedorPago
                        .FindElements(By.XPath(
                            ".//*[contains(@class,'custom-tab') or self::button or self::a or @role='tab']" +
                            "[contains(@class,'active') or contains(@class,'selected') or @aria-selected='true']"))
                        .FirstOrDefault(e =>
                        {
                            try { return e.Displayed; }
                            catch { return false; }
                        });

                    if (activaEnContenedor != null)
                        return NormalizeText(activaEnContenedor.Text).Contains(NormalizeText(textoEsperado));
                }
                catch
                {
                }
            }

            var tabActiva = FindFirstVisibleOrAny(
                VentasLocators.Payment.ActivePaymentTab,
                By.XPath("//*[(@aria-selected='true' or contains(@class,'active') or contains(@class,'selected')) and (contains(@class,'custom-tab') or @role='tab')]"));
            if (tabActiva == null)
                return false;

            return NormalizeText(tabActiva.Text).Contains(NormalizeText(textoEsperado));
        }

        private List<IWebElement> ObtenerCandidatosTabPagoNuevaVenta(string textoEsperado, params By[] locators)
        {
            var candidatos = new List<IWebElement>();
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
            var textoNormalizado = NormalizeText(textoEsperado);

            foreach (var locator in locators)
            {
                try
                {
                    foreach (var element in driver.FindElements(locator))
                    {
                        try
                        {
                            if (element.Displayed &&
                                element.Enabled &&
                                (contenedorPago == null || PerteneceAlContenedorNuevaVenta(contenedorPago, element)))
                            {
                                var objetivo = ResolverObjetivoTabPagoNuevaVenta(element);
                                if (NormalizeText(objetivo.Text).Contains(textoNormalizado))
                                    candidatos.Add(objetivo);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }

            if (contenedorPago != null)
            {
                try
                {
                    var elementosPorTexto = contenedorPago.FindElements(By.XPath(
                            ".//*[contains(@class,'custom-tab') or self::button or self::a or @role='tab'][normalize-space()]"))
                        .Where(e =>
                        {
                            try
                            {
                                return e.Displayed &&
                                       e.Enabled &&
                                       NormalizeText(e.Text).Contains(textoNormalizado);
                            }
                            catch
                            {
                                return false;
                            }
                        });

                    candidatos.AddRange(elementosPorTexto);
                }
                catch
                {
                }
            }

            return candidatos;
        }

        private IWebElement ResolverObjetivoTabPagoNuevaVenta(IWebElement candidato)
        {
            try
            {
                var ancestro = candidato.FindElements(By.XPath(
                        "./ancestor-or-self::*[contains(@class,'custom-tab') or self::button or self::a or @role='tab'][1]"))
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed && e.Enabled; }
                        catch { return false; }
                    });

                if (ancestro != null)
                    return ancestro;
            }
            catch
            {
            }

            return candidato;
        }

        private void EjecutarClickTabPagoNuevaVenta(IWebElement objetivo)
        {
            var interactivo = ResolverElementoInteractivoNuevaVenta(objetivo);
            ClickSeguroNuevaVenta(interactivo, preservarComoBoton: true);
        }

        private bool EsperarTabPagoNuevaVentaActiva(string textoEsperado)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(4))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(_ => EsTabPagoActiva(textoEsperado));
            }
            catch
            {
                return false;
            }
        }

        private bool EsContenidoPagoEsperadoVisible(string textoEsperado)
        {
            var esperado = NormalizeText(textoEsperado);

            if (esperado.Contains("efectivo"))
                return HayElementoVisible(VentasLocators.Payment.CashReceivedNewSale, VentasLocators.Payment.Change);

            if (esperado.Contains("tarjetas de credito") || esperado.Contains("tarjetas de debito"))
                return HayElementoVisible(
                    VentasLocators.Payment.BankSelect,
                    VentasLocators.Payment.CardSelect,
                    VentasLocators.Payment.BankTrigger,
                    VentasLocators.Payment.CardTrigger);

            if (esperado.Contains("transferencia") || esperado.Contains("depositos"))
                return HayElementoVisible(
                    VentasLocators.Payment.BankAccountSelect,
                    VentasLocators.Payment.BankAccountTrigger);

            if (esperado.Contains("puntos"))
                return HayElementoVisible(
                    VentasLocators.Payment.PointsPaymentInput,
                    VentasLocators.Payment.PointsPaymentCurrencyInput,
                    VentasLocators.Payment.PointsRemainingInput,
                    VentasLocators.Payment.PointsRemainingCurrencyInput);

            return false;
        }

        private bool HayElementoVisible(params By[] locators)
        {
            foreach (var locator in locators)
            {
                try
                {
                    if (driver.FindElements(locator).Any(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    }))
                    {
                        return true;
                    }
                }
                catch
                {
                }
            }

            return false;
        }

        private void ConfigurarMedioPagoNuevaVenta(
            PaymentInstruction instruccion,
            string tipoPago,
            string observacionPago)
        {
            switch (instruccion.MedioPago)
            {
                case "efectivo":
                    if (NormalizeText(tipoPago).Contains("contado") && !_paymentContext.Multipago)
                        IngresarMontoEfectivoNuevaVenta(instruccion.MontoConfigurado);
                    else
                        IngresarMontoMedioPagoNuevaVenta(instruccion.MontoConfigurado);
                    break;
                case "tarjeta_credito":
                case "tarjeta_debito":
                    SeleccionarBancoNuevaVenta(instruccion.Banco);
                    Thread.Sleep(500);
                    SeleccionarTarjetaNuevaVenta(instruccion.Tarjeta);
                    Thread.Sleep(300);
                    IngresarMontoMedioPagoNuevaVenta(instruccion.MontoConfigurado);
                    Thread.Sleep(300);
                    IngresarInformacionNuevaVenta(instruccion.Operacion);
                    Thread.Sleep(300);
                    break;
                case "transferencia_fondos":
                case "deposito_cuenta":
                    SeleccionarCuentaBancariaNuevaVenta(instruccion.CuentaBancaria);
                    Thread.Sleep(300);
                    IngresarMontoMedioPagoNuevaVenta(instruccion.MontoConfigurado);
                    Thread.Sleep(300);
                    IngresarInformacionNuevaVenta(instruccion.Operacion);
                    Thread.Sleep(300);
                    break;
                case "puntos":
                    ConfigurarPagoPuntosNuevaVenta(instruccion.MontoConfigurado);
                    break;
                case "nota_credito":
                    IngresarMontoMedioPagoNuevaVenta(instruccion.MontoConfigurado);
                    Thread.Sleep(300);
                    break;
            }

            if (!EsNA(observacionPago))
                IngresarObservacionPagoNuevaVenta(observacionPago);

            if (instruccion.MedioPago == "puntos")
                ConfirmarPagoPuntosNuevaVentaSiAplica();
        }

        private void GuardarMedioPagoActualNuevaVenta()
        {
            var boton = ObtenerBotonAgregarMedioPagoVisible();
            Assert.That(boton, Is.Not.Null,
                $"No se encontro el boton Agregar Medio de Pago en Nueva Venta. {ConstruirResumenPagoNuevaVenta()}");

            ScrollToCenter(boton!);
            if (!EstaHabilitadoBotonAccion(boton!))
            {
                Log($"[PagoNV] Agregar Medio de Pago deshabilitado. {ConstruirResumenPagoNuevaVenta()}");
                return;
            }

            try
            {
                boton!.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton);
            }

            Thread.Sleep(900);
        }

        private void IngresarMontoMedioPagoNuevaVenta(string monto)
        {
            if (EsNA(monto)) return;

            string valor = ResolverMontoPago(monto);
            if (string.IsNullOrWhiteSpace(valor)) return;

            if (TryParseDecimalFlexible(valor, out var montoSolicitado))
            {
                var totalDisponible = _paymentContext.TotalAntes ?? ObtenerTotalVentaActual();
                if (totalDisponible.HasValue &&
                    totalDisponible.Value > 0m &&
                    montoSolicitado > totalDisponible.Value)
                {
                    Log($"[PagoNV] El monto solicitado {montoSolicitado:0.00} excede el total disponible {totalDisponible.Value:0.00}. Se ajusta al total de la venta.");
                    valor = totalDisponible.Value.ToString("0.00", CultureInfo.InvariantCulture);
                }
            }

            var input = ObtenerInputMontoMedioPagoNuevaVenta();
            Assert.That(input, Is.Not.Null, "No se encontro el input de monto del medio de pago en Nueva Venta.");

            EstablecerValorInputNuevaVenta(input!, valor);

            try
            {
                input!.SendKeys(Keys.Tab);
            }
            catch
            {
            }

            Thread.Sleep(600);

            Log($"[PagoNV] Monto configurado en input id='{input!.GetAttribute("id")}' value='{input.GetAttribute("value")}'");
        }

        private bool DebeConservarMontoAutocompletadoNuevaVenta(IWebElement input, string valorSolicitado)
        {
            try
            {
                var actualTexto = (input.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
                if (!TryParseDecimalFlexible(actualTexto, out var actual) || actual <= 0m)
                    return false;

                if (!TryParseDecimalFlexible(valorSolicitado, out var solicitado))
                    return false;

                var totalDisponible = _paymentContext.TotalAntes ?? ObtenerTotalVentaActual();
                if (!totalDisponible.HasValue || totalDisponible.Value <= 0m)
                    return false;

                return Math.Abs(actual - totalDisponible.Value) <= 0.05m &&
                       solicitado >= totalDisponible.Value - 0.05m;
            }
            catch
            {
                return false;
            }
        }

        private bool EsperarContenidoPagoNuevaVentaVisible(string textoEsperado)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(4))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(_ => EsContenidoPagoEsperadoVisible(textoEsperado));
            }
            catch
            {
                return false;
            }
        }

        private void IngresarMontoEfectivoNuevaVenta(string monto)
        {
            if (EsNA(monto)) return;

            string valor = ResolverMontoPago(monto);
            Assert.That(string.IsNullOrWhiteSpace(valor), Is.False,
                "No se resolvio un monto valido para efectivo en Nueva Venta.");

            var montoBase = EsperarMontoBaseEfectivoNuevaVenta();
            if (!montoBase.HasValue || montoBase.Value <= 0m)
            {
                SeleccionarTabMedioPagoNuevaVenta("efectivo");
                Thread.Sleep(500);
                montoBase = EsperarMontoBaseEfectivoNuevaVenta(3);
            }

            Assert.That(montoBase.HasValue && montoBase.Value > 0m, Is.True,
                "No se pudo cargar el monto base de efectivo antes de ingresar el valor recibido.");

            var input = Find(VentasLocators.Payment.CashReceivedNewSale);
            ScrollToCenter(input);

            try
            {
                input.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", input);
            }

            EstablecerValorInputNuevaVenta(input, valor);

            try
            {
                input.SendKeys(Keys.Tab);
            }
            catch
            {
            }

            Thread.Sleep(600);
        }

        private decimal? EsperarMontoBaseEfectivoNuevaVenta(int timeoutSeconds = 6)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(_ =>
                {
                    var valor = LeerValor(VentasLocators.Payment.CashAmount);
                    return TryParseDecimalFlexible(valor, out var monto) && monto > 0m
                        ? monto
                        : (decimal?)null;
                });
            }
            catch
            {
                var actual = LeerMontoBaseEfectivoNuevaVenta();
                if (actual.HasValue && actual.Value > 0m)
                    return actual;

                var totalReferencia = _paymentContext.TotalAntes
                    ?? ObtenerTotalVentaActual(incluirMontoPago: false)
                    ?? ObtenerTotalVentaActual();

                if (!totalReferencia.HasValue || totalReferencia.Value <= 0m)
                    return actual;

                var inputMonto = driver.FindElements(VentasLocators.Payment.CashAmount)
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed && e.Enabled; }
                        catch { return false; }
                    });

                if (inputMonto == null)
                    return actual;

                Log($"[PagoNV] El monto base de efectivo no se cargo en la UI. Se sincroniza con el total real '{totalReferencia.Value:0.00}'.");
                EstablecerValorInputNuevaVenta(inputMonto, totalReferencia.Value.ToString("0.00", CultureInfo.InvariantCulture));
                Thread.Sleep(500);

                return LeerMontoBaseEfectivoNuevaVenta();
            }
        }

        private decimal? LeerMontoBaseEfectivoNuevaVenta()
        {
            var valor = LeerValor(VentasLocators.Payment.CashAmount);
            return TryParseDecimalFlexible(valor, out var monto) ? monto : (decimal?)null;
        }

        private void SeleccionarBancoNuevaVenta(string banco)
        {
            if (EsNA(banco)) return;

            var select = EsperarUltimoSelectVisibleNuevaVenta(VentasLocators.Payment.BankSelect);
            if (select == null)
            {
                var trigger = FindFirstVisibleOrAny(VentasLocators.Payment.BankTrigger) ?? ObtenerTriggerPagoVisible(0);
                Assert.That(trigger, Is.Not.Null,
                    $"No se encontro un dropdown visible de banco en Nueva Venta. {ConstruirResumenPagoNuevaVenta()}");
                SeleccionarDropdownCustomNuevaVenta(banco, trigger!);
                return;
            }

            SeleccionarOpcionSelectNuevaVenta(select, banco.Trim());

            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(6)).Until(d =>
                {
                    var tarjetaSelect = d.FindElements(VentasLocators.Payment.CardSelect)
                        .Where(e => e.Displayed && e.Enabled)
                        .LastOrDefault();

                    if (tarjetaSelect == null) return false;

                    return new SelectElement(tarjetaSelect).Options.Count > 1;
                });
            }
            catch
            {
            }
        }

        private void SeleccionarTarjetaNuevaVenta(string tarjeta)
        {
            if (EsNA(tarjeta)) return;

            var select = EsperarUltimoSelectVisibleNuevaVenta(VentasLocators.Payment.CardSelect);
            if (select == null)
            {
                var trigger = FindFirstVisibleOrAny(VentasLocators.Payment.CardTrigger) ?? ObtenerTriggerPagoVisible(1);
                Assert.That(trigger, Is.Not.Null,
                    $"No se encontro un dropdown visible de tarjeta en Nueva Venta. {ConstruirResumenPagoNuevaVenta()}");
                SeleccionarDropdownCustomNuevaVenta(tarjeta, trigger!);
                return;
            }

            SeleccionarOpcionSelectNuevaVenta(select, tarjeta.Trim());
        }

        private void SeleccionarCuentaBancariaNuevaVenta(string cuentaBancaria)
        {
            if (EsNA(cuentaBancaria)) return;

            var select = EsperarUltimoSelectVisibleNuevaVenta(VentasLocators.Payment.BankAccountSelect);
            if (select == null)
            {
                var trigger = FindFirstVisibleOrAny(VentasLocators.Payment.BankAccountTrigger) ?? ObtenerTriggerPagoVisible(0);
                Assert.That(trigger, Is.Not.Null,
                    $"No se encontro un dropdown visible de cuenta bancaria en Nueva Venta. {ConstruirResumenPagoNuevaVenta()}");
                SeleccionarDropdownCustomNuevaVenta(cuentaBancaria, trigger!);
                return;
            }

            var combo = new SelectElement(select);
            var texto = cuentaBancaria.Trim();

            try
            {
                combo.SelectByText(texto);
            }
            catch
            {
                var opcion = combo.Options.FirstOrDefault(x =>
                    x.Text.Trim().IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0);

                if (opcion != null)
                    opcion.Click();
                else
                    throw new Exception($"No se encontro la cuenta bancaria '{texto}' en Nueva Venta.");
            }

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                arguments[0].blur();
            ", select);

            Thread.Sleep(500);
        }

        private void IngresarInformacionNuevaVenta(string informacion)
        {
            if (EsNA(informacion)) return;

            var input = FindLastVisibleInPayment(VentasLocators.Payment.PaymentInfoInput);
            Assert.That(input, Is.Not.Null, "No se encontro el input visible de informacion en Nueva Venta.");

            LimpiarYEscribirCampoNuevaVenta(input!, informacion.Trim());
        }

        private void IngresarObservacionPagoNuevaVenta(string observacion)
        {
            if (EsNA(observacion)) return;

            var input = FindLastVisibleInPayment(VentasLocators.Payment.PaymentObservation);

            Assert.That(input, Is.Not.Null, "No se encontro el campo de observacion del pago en Nueva Venta.");

            EstablecerValorInputNuevaVenta(input!, observacion.Trim());
        }

        private IWebElement? BuscarUltimoSelectVisibleNuevaVenta(By locator)
        {
            try
            {
                return driver.FindElements(locator)
                    .Where(e => e.Displayed && e.Enabled)
                    .LastOrDefault();
            }
            catch
            {
                return null;
            }
        }

        private IWebElement? EsperarUltimoSelectVisibleNuevaVenta(By locator, int timeoutSeconds = 6)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(d =>
                {
                    try
                    {
                        var visibles = d.FindElements(locator)
                            .Where(e => e.Displayed && e.Enabled)
                            .ToList();
                        return visibles.Any() ? visibles.Last() : null;
                    }
                    catch
                    {
                        return null;
                    }
                });
            }
            catch
            {
                return BuscarUltimoSelectVisibleNuevaVenta(locator);
            }
        }

        private IWebElement? ObtenerUltimoInputVisibleNuevaVenta(By locator)
        {
            return wait.Until(d =>
            {
                try
                {
                    var visibles = d.FindElements(locator)
                        .Where(e => e.Displayed && e.Enabled)
                        .ToList();
                    return visibles.Any() ? visibles.Last() : null;
                }
                catch
                {
                    return null;
                }
            });
        }

        private IWebElement? ObtenerInputMontoMedioPagoNuevaVenta()
        {
            try
            {
                return wait.Until(d =>
                {
                    try
                    {
                        var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
                        var visiblesEnContenedor = d.FindElements(VentasLocators.Payment.PaymentAmountInput)
                            .Where(e =>
                            {
                                try
                                {
                                    return contenedorPago != null &&
                                           PerteneceAlContenedorNuevaVenta(contenedorPago, e);
                                }
                                catch
                                {
                                    return false;
                                }
                            })
                            .Where(EsInputMontoMedioPagoValido)
                            .ToList();

                        if (visiblesEnContenedor?.Any() == true)
                            return visiblesEnContenedor.Last();

                        var visiblesGlobales = d.FindElements(VentasLocators.Payment.PaymentAmountInput)
                            .Where(EsInputMontoMedioPagoValido)
                            .ToList();

                        return visiblesGlobales.Any() ? visiblesGlobales.Last() : null;
                    }
                    catch
                    {
                        return null;
                    }
                });
            }
            catch
            {
                Log($"[PagoNV] {ConstruirResumenPagoNuevaVenta("input_monto_no_resuelto")}");
                return null;
            }
        }

        private bool EsInputMontoMedioPagoValido(IWebElement input)
        {
            try
            {
                if (!input.Displayed || !input.Enabled)
                    return false;

                var id = NormalizeText(input.GetAttribute("id") ?? string.Empty);
                var name = NormalizeText(input.GetAttribute("name") ?? string.Empty);
                var formControl = NormalizeText(input.GetAttribute("formcontrolname") ?? string.Empty);
                var placeholder = NormalizeText(input.GetAttribute("placeholder") ?? string.Empty);

                if (id is "amountreceived" or "change" or "informacion")
                    return false;

                if (id.Contains("change") ||
                    name.Contains("change") ||
                    formControl.Contains("change") ||
                    placeholder.Contains("vuelto") ||
                    placeholder.Contains("observ"))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void EstablecerValorInputNuevaVenta(IWebElement input, string valor)
        {
            ScrollToCenter(input);

            try
            {
                input.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", input);
            }

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(150);

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                const el = arguments[0];
                const value = arguments[1];
                const proto = el.tagName === 'TEXTAREA'
                    ? window.HTMLTextAreaElement.prototype
                    : window.HTMLInputElement.prototype;
                const setter = Object.getOwnPropertyDescriptor(proto, 'value')?.set;

                if (setter)
                    setter.call(el, value);
                else
                    el.value = value;

                ['input', 'change', 'keyup', 'blur'].forEach(type => {
                    el.dispatchEvent(new Event(type, { bubbles: true }));
                });
            ", input, valor);

            Thread.Sleep(200);

            var valorActual = input.GetAttribute("value") ?? string.Empty;
            if (!NormalizeText(valorActual).Contains(NormalizeText(valor)))
            {
                input.SendKeys(valor);
                input.SendKeys(Keys.Tab);
                Thread.Sleep(500);
            }
        }

        private void LimpiarValorInputNuevaVenta(IWebElement input)
        {
            ScrollToCenter(input);

            try
            {
                input.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", input);
            }

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(150);

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                const el = arguments[0];
                const proto = el.tagName === 'TEXTAREA'
                    ? window.HTMLTextAreaElement.prototype
                    : window.HTMLInputElement.prototype;
                const setter = Object.getOwnPropertyDescriptor(proto, 'value')?.set;

                if (setter)
                    setter.call(el, '');
                else
                    el.value = '';

                ['input', 'change', 'keyup', 'blur'].forEach(type => {
                    el.dispatchEvent(new Event(type, { bubbles: true }));
                });
            ", input);

            Thread.Sleep(300);
        }

        private void SeleccionarOpcionSelectNuevaVenta(IWebElement selectElement, string texto)
        {
            var combo = new SelectElement(selectElement);
            var textoNormalizado = NormalizeText(texto);
            var opcion = combo.Options.FirstOrDefault(o =>
                NormalizeText(o.Text).Equals(textoNormalizado, StringComparison.OrdinalIgnoreCase));

            if (opcion == null)
                throw new Exception($"No se encontro la opcion '{texto}' en el combo de Nueva Venta.");

            string? value = opcion.GetAttribute("value");

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", selectElement);
            Thread.Sleep(200);
            combo.SelectByText(opcion.Text.Trim());
            Thread.Sleep(300);

            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                var el = arguments[0];
                var val = arguments[1];
                var nativeInputValueSetter = Object.getOwnPropertyDescriptor(
                    window.HTMLSelectElement.prototype, 'value').set;
                nativeInputValueSetter.call(el, val);
                el.dispatchEvent(new Event('input',  { bubbles: true }));
                el.dispatchEvent(new Event('change', { bubbles: true }));
                el.blur();
            ", selectElement, value);

            Thread.Sleep(600);
        }

        private void SeleccionarDropdownCustomNuevaVenta(string texto, params By[] triggerLocators)
        {
            Exception? ultimaExcepcion = null;

            for (int intento = 1; intento <= 3; intento++)
            {
                try
                {
                    var trigger = Find(triggerLocators);
                    SeleccionarDropdownCustomNuevaVenta(texto, trigger);
                    return;
                }
                catch (StaleElementReferenceException ex) when (intento < 3)
                {
                    ultimaExcepcion = ex;
                    Log($"[NuevaVenta] Reintentando seleccion de '{texto}' por stale element ({intento}/3).");
                    Thread.Sleep(500);
                }
                catch (WebDriverException ex) when (intento < 3 && EsStaleElementException(ex))
                {
                    ultimaExcepcion = ex;
                    Log($"[NuevaVenta] Reintentando seleccion de '{texto}' por refresco del DOM ({intento}/3).");
                    Thread.Sleep(500);
                }
            }

            throw new Exception($"No se pudo seleccionar '{texto}' en el dropdown de Nueva Venta por refrescos del DOM.", ultimaExcepcion);
        }

        private void SeleccionarDropdownCustomNuevaVenta(string texto, IWebElement trigger)
        {
            ScrollToCenter(trigger);
            trigger.Click();
            Thread.Sleep(500);

            var inputBusqueda = driver.FindElements(VentasLocators.Payment.DropdownSearchInput)
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (inputBusqueda != null)
            {
                inputBusqueda.SendKeys(Keys.Control + "a");
                inputBusqueda.SendKeys(Keys.Delete);
                inputBusqueda.SendKeys(texto);
                Thread.Sleep(500);

                var opcionBuscada = BuscarOpcionConceptoEnDropdownNuevaVenta(texto) ?? BuscarOpcionVisibleNuevaVenta(texto);
                if (opcionBuscada != null)
                {
                    ScrollToCenter(opcionBuscada);
                    ClickDropdownOptionNuevaVenta(opcionBuscada);
                    Thread.Sleep(700);
                    return;
                }

                inputBusqueda.SendKeys(Keys.Enter);
                Thread.Sleep(700);
                return;
            }

            var opcion = BuscarOpcionConceptoEnDropdownNuevaVenta(texto) ?? BuscarOpcionVisibleNuevaVenta(texto);
            Assert.That(opcion, Is.Not.Null, $"No se encontro una opcion visible para '{texto}' en el dropdown de Nueva Venta.");

            ScrollToCenter(opcion!);
            ClickDropdownOptionNuevaVenta(opcion!);
            Thread.Sleep(700);
        }

        private IWebElement? ObtenerTriggerPagoVisible(int index)
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
            if (contenedorPago == null)
                return null;

            try
            {
                var triggers = contenedorPago.FindElements(By.CssSelector(".select-trigger"))
                    .Where(e => e.Displayed && e.Enabled)
                    .ToList();

                if (triggers.Count == 0)
                    return null;

                return index >= 0 && index < triggers.Count ? triggers[index] : triggers.Last();
            }
            catch
            {
                return null;
            }
        }

        private string ConstruirResumenPagoNuevaVenta(string? contexto = null, bool? guardarHabilitado = null, string? mensajeVisible = null)
        {
            var partes = new List<string>();

            if (!string.IsNullOrWhiteSpace(contexto))
                partes.Add(contexto.Trim());

            var tipo = ObtenerTipoPagoActivoNuevaVenta();
            if (!string.IsNullOrWhiteSpace(tipo))
                partes.Add($"tipo={tipo}");

            partes.Add($"multipago={(EstaMarcado(VentasLocators.Payment.MultipaymentCheckbox) ? "si" : "no")}");

            var tab = ObtenerTextoTabPagoActivoNuevaVenta();
            if (!string.IsNullOrWhiteSpace(tab))
                partes.Add($"tab={tab}");

            var estadoGuardar = guardarHabilitado ?? ObtenerEstadoGuardarActualNuevaVenta();
            if (estadoGuardar.HasValue)
                partes.Add($"guardar={(estadoGuardar.Value ? "habilitado" : "deshabilitado")}");

            var estadoAgregar = ObtenerEstadoAgregarMedioPagoActualNuevaVenta();
            if (estadoAgregar.HasValue)
                partes.Add($"agregar_medio={(estadoAgregar.Value ? "habilitado" : "deshabilitado")}");

            var mensaje = string.IsNullOrWhiteSpace(mensajeVisible)
                ? CapturarValidaciones()
                : mensajeVisible;
            if (!string.IsNullOrWhiteSpace(mensaje))
                partes.Add($"mensaje='{mensaje}'");

            var estadoSeccion = CapturarEstadoSeccionPagoNuevaVenta();
            if (!string.IsNullOrWhiteSpace(estadoSeccion))
                partes.Add($"estado_pago='{estadoSeccion}'");

            var total = LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.CashAmount);
            if (!string.IsNullOrWhiteSpace(total))
                partes.Add($"total={total}");

            var tabNormalizado = NormalizeText(tab);
            if (tabNormalizado.Contains("efectivo"))
            {
                AgregarParteSiTieneValor(partes, "recibido", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.CashReceivedNewSale));
                AgregarParteSiTieneValor(partes, "vuelto", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.Change));
            }
            else if (tabNormalizado.Contains("tarjetas de credito") || tabNormalizado.Contains("tarjetas de debito"))
            {
                AgregarParteSiTieneValor(partes, "banco", LeerTextoSeleccionadoPagoResumenNuevaVenta(0, VentasLocators.Payment.BankSelect, VentasLocators.Payment.BankTrigger));
                AgregarParteSiTieneValor(partes, "tarjeta", LeerTextoSeleccionadoPagoResumenNuevaVenta(1, VentasLocators.Payment.CardSelect, VentasLocators.Payment.CardTrigger));
                AgregarParteSiTieneValor(partes, "monto", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PaymentAmountInput));
                AgregarParteSiTieneValor(partes, "info", LeerTextoResumenPagoNuevaVenta(VentasLocators.Payment.PaymentInfoInput));
            }
            else if (tabNormalizado.Contains("transferencia") || tabNormalizado.Contains("depositos"))
            {
                AgregarParteSiTieneValor(partes, "cuenta", LeerTextoSeleccionadoPagoResumenNuevaVenta(0, VentasLocators.Payment.BankAccountSelect, VentasLocators.Payment.BankAccountTrigger));
                AgregarParteSiTieneValor(partes, "monto", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PaymentAmountInput));
                AgregarParteSiTieneValor(partes, "info", LeerTextoResumenPagoNuevaVenta(VentasLocators.Payment.PaymentInfoInput));
            }
            else if (tabNormalizado.Contains("puntos"))
            {
                AgregarParteSiTieneValor(partes, "pago_pts", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PointsPaymentInput));
                AgregarParteSiTieneValor(partes, "pago_s", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PointsPaymentCurrencyInput));
                AgregarParteSiTieneValor(partes, "restantes_pts", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PointsRemainingInput));
                AgregarParteSiTieneValor(partes, "restantes_s", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PointsRemainingCurrencyInput));
            }
            else if (tabNormalizado.Contains("nota de credito"))
            {
                AgregarParteSiTieneValor(partes, "monto", LeerMontoResumenPagoNuevaVenta(VentasLocators.Payment.PaymentAmountInput));
            }

            return $"Resumen: {string.Join(" | ", partes.Where(x => !string.IsNullOrWhiteSpace(x)))}";
        }

        private string ObtenerTipoPagoActivoNuevaVenta()
        {
            if (EstaMarcado(VentasLocators.Payment.CashType))
                return "contado";

            if (EstaMarcado(VentasLocators.Payment.QuickCreditType))
                return "credito";

            return string.Empty;
        }

        private string ObtenerTextoTabPagoActivoNuevaVenta()
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
            if (contenedorPago == null)
                return string.Empty;

            try
            {
                var tabActiva = contenedorPago.FindElements(By.XPath(
                        ".//*[(@aria-selected='true' or contains(@class,'active') or contains(@class,'selected')) and (contains(@class,'custom-tab') or @role='tab')]"))
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    });

                return tabActiva?.Text?.Trim() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string CapturarEstadoSeccionPagoNuevaVenta()
        {
            var estadoActual = CapturarEstadoSeccionPagoNuevaVentaActual();
            if (!string.IsNullOrWhiteSpace(estadoActual))
                _lastObservedPaymentState = estadoActual;

            return !string.IsNullOrWhiteSpace(estadoActual)
                ? estadoActual
                : _lastObservedPaymentState;
        }

        private string CapturarEstadoSeccionPagoNuevaVentaActual()
        {
            var contenedorPago = FindFirstVisibleOrAny(VentasLocators.Payment.PaymentBody);
            var estadoEnPago = BuscarEstadoPagoVisibleNuevaVenta(
                contenedorPago?.FindElements(By.XPath(".//*[normalize-space()]")) ?? Enumerable.Empty<IWebElement>());

            if (!string.IsNullOrWhiteSpace(estadoEnPago))
                return estadoEnPago;

            return BuscarEstadoPagoVisibleNuevaVenta(driver.FindElements(By.XPath(
                "//*[contains(normalize-space(),'correct') or contains(normalize-space(),'Correct') or " +
                "contains(normalize-space(),'complet') or contains(normalize-space(),'Complet') or " +
                "contains(normalize-space(),'requerid') or contains(normalize-space(),'Requerid')]")));
        }

        private string BuscarEstadoPagoVisibleNuevaVenta(IEnumerable<IWebElement> elementos)
        {
            try
            {
                return elementos
                    .Where(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    })
                    .Select(e => e.Text?.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t) && EsTextoEstadoPago(t!))
                    .Distinct()
                    .OrderBy(t => t!.Length)
                    .FirstOrDefault() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string ObservarEstadoSeccionPagoNuevaVenta(int timeoutMs = 2500)
        {
            var ultimoEstado = string.Empty;
            var ultimoLogueado = string.Empty;
            var limite = DateTime.UtcNow.AddMilliseconds(timeoutMs);

            while (DateTime.UtcNow <= limite)
            {
                var estadoActual = CapturarEstadoSeccionPagoNuevaVentaActual();
                if (!string.IsNullOrWhiteSpace(estadoActual))
                {
                    ultimoEstado = estadoActual;
                    _lastObservedPaymentState = estadoActual;

                    if (!estadoActual.Equals(ultimoLogueado, StringComparison.Ordinal))
                    {
                        Log($"[PagoNV] Estado visible: '{estadoActual}'");
                        ultimoLogueado = estadoActual;
                    }

                    if (EsEstadoPagoExitoso(estadoActual))
                        return estadoActual;
                }

                Thread.Sleep(150);
            }

            return !string.IsNullOrWhiteSpace(ultimoEstado)
                ? ultimoEstado
                : _lastObservedPaymentState;
        }

        private static bool EsEstadoPagoExitoso(string estado)
        {
            var normalizado = NormalizeText(estado);
            if (!normalizado.Contains("correctamente"))
                return false;

            return normalizado.Contains("campos requeridos") ||
                   normalizado.Contains("completo los datos") ||
                   normalizado.Contains("se completo los datos");
        }

        private static bool EsEstadoPagoIncompleto(string estado)
        {
            var normalizado = NormalizeText(estado);
            return normalizado.Contains("campos requeridos") &&
                   !normalizado.Contains("correctamente");
        }

        private static bool EsTextoEstadoPago(string texto)
        {
            var normalizado = NormalizeText(texto);
            return normalizado.Contains("campos requeridos") ||
                   normalizado.Contains("completo los datos") ||
                   normalizado.Contains("se completo los datos");
        }

        private void AssertSeccionPagoListaParaGuardar(string mensajeError)
        {
            var estado = ObservarEstadoSeccionPagoNuevaVenta();
            var guardarHabilitado = ObtenerEstadoGuardarActualNuevaVenta();
            var resumen = ConstruirResumenPagoNuevaVenta(guardarHabilitado: guardarHabilitado, mensajeVisible: estado);

            if (EsEstadoPagoExitoso(estado))
                return;

            if (EsEstadoPagoIncompleto(estado))
            {
                Assert.Fail($"{mensajeError} Estado actual: '{estado}'. {resumen}");
                return;
            }

            if (guardarHabilitado == true)
            {
                Log($"[PagoNV] Estado de Pago no quedo visible; se toma como valido por guardar habilitado. {resumen}");
                return;
            }

            Assert.Fail($"{mensajeError} Estado actual: '{estado}'. {resumen}");
        }

        private bool? ObtenerEstadoGuardarActualNuevaVenta()
        {
            try
            {
                var boton = driver.FindElements(VentasLocators.NuevaVenta.GuardarVenta)
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    });

                return boton == null ? null : EstaHabilitadoBotonGuardar(boton);
            }
            catch
            {
                return null;
            }
        }

        private string LeerTextoResumenPagoNuevaVenta(By locator)
        {
            try
            {
                var input = FindLastVisibleInPayment(locator) ?? FindFirstVisibleInPayment(locator);
                return (input?.GetAttribute("value") ?? input?.Text ?? string.Empty).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        private string LeerMontoResumenPagoNuevaVenta(By locator)
        {
            var texto = LeerTextoResumenPagoNuevaVenta(locator);
            return TryParseDecimalFlexible(texto, out var monto)
                ? monto.ToString("0.00", CultureInfo.InvariantCulture)
                : string.Empty;
        }

        private string LeerTextoSeleccionadoPagoResumenNuevaVenta(int fallbackTriggerIndex, params By[] locators)
        {
            foreach (var locator in locators)
            {
                try
                {
                    var select = BuscarUltimoSelectVisibleNuevaVenta(locator);
                    if (select == null)
                        continue;

                    var texto = new SelectElement(select).SelectedOption?.Text?.Trim() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(texto))
                        return texto;
                }
                catch
                {
                }
            }

            try
            {
                var trigger = FindLastVisibleInPayment(locators) ??
                              FindFirstVisibleInPayment(locators) ??
                              ObtenerTriggerPagoVisible(fallbackTriggerIndex);

                return (trigger?.Text ?? string.Empty).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static void AgregarParteSiTieneValor(ICollection<string> partes, string clave, string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                partes.Add($"{clave}={valor}");
        }

        private void LogPaymentControlsSnapshot()
        {
            Log($"[PagoNV] {ConstruirResumenPagoNuevaVenta()}");
        }

        private IWebElement? BuscarOpcionVisibleNuevaVenta(string texto)
        {
            var textoNormalizado = NormalizeText(texto);

            for (int intento = 0; intento < 6; intento++)
            {
                var candidatos = ObtenerOpcionesDropdownNuevaVenta();
                var exacto = candidatos.FirstOrDefault(e =>
                {
                    var actual = ObtenerTextoSeguroNuevaVenta(e);
                    return !string.IsNullOrWhiteSpace(actual) &&
                           NormalizeText(actual).Equals(textoNormalizado, StringComparison.OrdinalIgnoreCase);
                });

                if (exacto != null)
                    return exacto;

                var contiene = candidatos.FirstOrDefault(e =>
                {
                    var actual = ObtenerTextoSeguroNuevaVenta(e);
                    return !string.IsNullOrWhiteSpace(actual) &&
                           NormalizeText(actual).Contains(textoNormalizado, StringComparison.OrdinalIgnoreCase);
                });

                if (contiene != null)
                    return contiene;

                if (texto.Contains('|'))
                {
                    var partes = texto.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    var porPartes = candidatos.FirstOrDefault(e =>
                    {
                        var actual = NormalizeText(ObtenerTextoSeguroNuevaVenta(e));
                        return !string.IsNullOrWhiteSpace(actual) &&
                               partes.All(parte => actual.Contains(NormalizeText(parte), StringComparison.OrdinalIgnoreCase));
                    });

                    if (porPartes != null)
                        return porPartes;
                }

                Thread.Sleep(200);
            }

            return null;
        }

        private IWebElement? BuscarOpcionConceptoEnDropdownNuevaVenta(string texto)
        {
            string textoSeguro = texto.Replace("'", "&apos;");
            for (int intento = 0; intento < 6; intento++)
            {
                var candidatos = driver.FindElements(By.XPath(
                        $"//span[contains(@class,'option-label')][contains(normalize-space(),'{textoSeguro}')] | " +
                        $"//div[contains(@class,'option-item')][contains(normalize-space(),'{textoSeguro}')] | " +
                        $"//a[contains(@class,'dropdown-item')][contains(normalize-space(),'{textoSeguro}')] | " +
                        $"//*[@role='option'][contains(normalize-space(),'{textoSeguro}')]"))
                    .Where(e =>
                    {
                        try { return e.Displayed && e.Enabled; }
                        catch { return false; }
                    })
                    .ToList();

                if (candidatos.Count > 0)
                {
                    var exacto = candidatos.FirstOrDefault(e =>
                    {
                        var actual = ObtenerTextoSeguroNuevaVenta(e);
                        return !string.IsNullOrWhiteSpace(actual) &&
                               NormalizeText(actual).Equals(NormalizeText(texto), StringComparison.OrdinalIgnoreCase);
                    });

                    if (exacto != null)
                        return exacto;

                    return candidatos.FirstOrDefault(e => !string.IsNullOrWhiteSpace(ObtenerTextoSeguroNuevaVenta(e)));
                }

                Thread.Sleep(200);
            }

            return null;
        }

        private List<IWebElement> ObtenerOpcionesDropdownNuevaVenta()
        {
            var opcionesEspecificas = driver.FindElements(By.XPath(
                    "//span[contains(@class,'option-label')] | " +
                    "//div[contains(@class,'option-item')] | " +
                    "//a[contains(@class,'dropdown-item')] | " +
                    "//*[@role='option']"))
                .Where(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                })
                .ToList();

            if (opcionesEspecificas.Count > 0)
                return opcionesEspecificas;

            return driver.FindElements(By.XPath("//*[self::div or self::span or self::li][normalize-space()]"))
                .Where(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                })
                .ToList();
        }

        private static string ObtenerTextoSeguroNuevaVenta(IWebElement elemento)
        {
            try
            {
                return (elemento.Text ?? string.Empty).Trim();
            }
            catch (StaleElementReferenceException)
            {
                return string.Empty;
            }
            catch (WebDriverException ex) when (EsStaleElementException(ex))
            {
                return string.Empty;
            }
        }

        private void ClickDropdownOptionNuevaVenta(IWebElement opcion)
        {
            try
            {
                opcion.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opcion);
            }
        }

        private static bool EsStaleElementException(WebDriverException ex) =>
            ex.Message.Contains("stale element reference", StringComparison.OrdinalIgnoreCase);

        private static string ExtraerTextoConceptoDesdeBloque(string bloque, string concepto)
        {
            if (string.IsNullOrWhiteSpace(bloque))
                return concepto;

            var linea = bloque
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .FirstOrDefault(parte => NormalizeText(parte).Contains(NormalizeText(concepto)));

            return string.IsNullOrWhiteSpace(linea) ? concepto : linea.Trim();
        }

        private void LimpiarYEscribirCampoNuevaVenta(IWebElement input, string valor)
        {
            input.Click();
            Thread.Sleep(150);
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(150);
            input.SendKeys(valor);
            input.SendKeys(Keys.Tab);
            Thread.Sleep(300);
        }

        private void ConfigurarPagoPuntosNuevaVenta(string monto)
        {
            var montoObjetivo = ResolverMontoPagoPuntosNuevaVenta(monto);
            Assert.That(string.IsNullOrWhiteSpace(montoObjetivo), Is.False,
                "No se resolvio un monto valido para el pago con puntos en Nueva Venta.");

            Assert.That(TryParseDecimalFlexible(montoObjetivo, out var montoObjetivoDecimal), Is.True,
                $"No se pudo interpretar el monto configurado para puntos '{montoObjetivo}'.");

            var inputSoles = FindLastVisibleInPayment(VentasLocators.Payment.PointsPaymentCurrencyInput);
            var inputPuntos = FindLastVisibleInPayment(VentasLocators.Payment.PointsPaymentInput);
            var estadoInicial = CapturarEstadoPagoPuntosNuevaVenta();

            Assert.That(inputSoles != null || inputPuntos != null, Is.True,
                "No se encontraron inputs visibles para el pago con puntos en Nueva Venta.");

            if (inputSoles != null)
            {
                EstablecerValorInputNuevaVenta(inputSoles, montoObjetivo);
                Thread.Sleep(700);

                var valorSoles = (inputSoles.GetAttribute("value") ?? inputSoles.Text ?? string.Empty).Trim();
                Log($"[PagoNV] Pago con puntos configurado en soles value='{valorSoles}'");

                if (EsperarPagoPuntosAplicadoNuevaVenta(estadoInicial, montoObjetivoDecimal))
                    return;
            }

            var valorPuntos = ResolverValorPuntosDesdeMontoNuevaVenta(montoObjetivo);
            Assert.That(string.IsNullOrWhiteSpace(valorPuntos), Is.False,
                "No se pudo calcular el equivalente en puntos para completar el pago.");
            Assert.That(inputPuntos, Is.Not.Null,
                "No se encontro el input de pago en puntos para aplicar el fallback.");

            EstablecerValorInputNuevaVenta(inputPuntos!, valorPuntos);
            Thread.Sleep(700);

            Log($"[PagoNV] Pago con puntos configurado en puntos value='{inputPuntos!.GetAttribute("value")}'");

            if (!EsperarPagoPuntosAplicadoNuevaVenta(estadoInicial, montoObjetivoDecimal))
                Log("[PagoNV] El pago con puntos no se reflejo en los saldos visibles luego de completar los inputs.");
        }

        private string ResolverMontoPagoPuntosNuevaVenta(string monto)
        {
            return ResolverMontoPago(monto);
        }

        private string ResolverValorPuntosDesdeMontoNuevaVenta(string montoSoles)
        {
            if (!TryParseDecimalFlexible(montoSoles, out var montoObjetivo) || montoObjetivo <= 0m)
                return string.Empty;

            var puntosAcumulados = LeerValor(VentasLocators.Payment.PointsAccumulatedInput);
            var equivalenteSoles = LeerValor(VentasLocators.Payment.PointsAccumulatedCurrencyInput);

            if (!TryParseDecimalFlexible(puntosAcumulados, out var totalPuntos) ||
                !TryParseDecimalFlexible(equivalenteSoles, out var totalSoles) ||
                totalPuntos <= 0m ||
                totalSoles <= 0m)
            {
                return string.Empty;
            }

            var valorPuntos = Math.Round(montoObjetivo * (totalPuntos / totalSoles), 2, MidpointRounding.AwayFromZero);
            return valorPuntos.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private PointsPaymentState CapturarEstadoPagoPuntosNuevaVenta()
        {
            return new PointsPaymentState
            {
                PuntosAcumulados = TryParseDecimalFlexible(LeerValor(VentasLocators.Payment.PointsAccumulatedInput), out var puntosAcumulados)
                    ? puntosAcumulados
                    : (decimal?)null,
                SolesAcumulados = TryParseDecimalFlexible(LeerValor(VentasLocators.Payment.PointsAccumulatedCurrencyInput), out var solesAcumulados)
                    ? solesAcumulados
                    : (decimal?)null,
                PuntosRestantes = TryParseDecimalFlexible(LeerValor(VentasLocators.Payment.PointsRemainingInput), out var puntosRestantes)
                    ? puntosRestantes
                    : (decimal?)null,
                SolesRestantes = TryParseDecimalFlexible(LeerValor(VentasLocators.Payment.PointsRemainingCurrencyInput), out var solesRestantes)
                    ? solesRestantes
                    : (decimal?)null
            };
        }

        private bool EsperarPagoPuntosAplicadoNuevaVenta(PointsPaymentState estadoInicial, decimal montoObjetivo, int timeoutSeconds = 4)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(_ => SeReflejoPagoPuntosNuevaVenta(estadoInicial, montoObjetivo));
            }
            catch
            {
                return SeReflejoPagoPuntosNuevaVenta(estadoInicial, montoObjetivo);
            }
        }

        private bool SeReflejoPagoPuntosNuevaVenta(PointsPaymentState estadoInicial, decimal montoObjetivo)
        {
            var actual = CapturarEstadoPagoPuntosNuevaVenta();

            var disminuyoSaldoSoles = estadoInicial.SolesRestantes.HasValue &&
                                      actual.SolesRestantes.HasValue &&
                                      actual.SolesRestantes.Value < estadoInicial.SolesRestantes.Value - 0.05m;

            var disminuyoSaldoPuntos = estadoInicial.PuntosRestantes.HasValue &&
                                       actual.PuntosRestantes.HasValue &&
                                       actual.PuntosRestantes.Value < estadoInicial.PuntosRestantes.Value - 0.05m;

            if (disminuyoSaldoSoles || disminuyoSaldoPuntos)
            {
                Log($"[PagoNV] Pago con puntos reflejado. Restantes S/='{actual.SolesRestantes?.ToString("0.00", CultureInfo.InvariantCulture) ?? "NA"}' Pts='{actual.PuntosRestantes?.ToString("0.00", CultureInfo.InvariantCulture) ?? "NA"}'");
                return true;
            }

            return montoObjetivo > 0m &&
                   string.IsNullOrWhiteSpace(CapturarValidaciones()) &&
                   IsSaveEnabled();
        }

        private void ConfirmarPagoPuntosNuevaVentaSiAplica()
        {
            if (_paymentContext.Multipago || IsSaveEnabled())
                return;

            var botonAgregar = driver.FindElements(VentasLocators.Payment.AddPaymentButton)
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed && e.Enabled; }
                    catch { return false; }
                });

            if (botonAgregar == null)
                return;

            Log("[PagoNV] Se intenta confirmar el pago con puntos agregando el medio actual.");
            GuardarMedioPagoActualNuevaVenta();
        }

        private string ResolverMontoPago(string monto, decimal? totalReferencia = null)
        {
            if (string.IsNullOrWhiteSpace(monto) || monto.Trim().Equals("NA", StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            var normalizado = NormalizeText(monto).Replace(" ", string.Empty);
            if (EsExpresionMontoBasadaEnTotal(normalizado))
            {
                var totalActual = totalReferencia
                    ?? _paymentContext.TotalAntes
                    ?? ObtenerTotalVentaActual(incluirMontoPago: false)
                    ?? ObtenerTotalVentaActual();
                Assert.That(totalActual.HasValue, Is.True,
                    "No se pudo obtener el total actual de la venta para resolver el monto del pago.");

                var montoResuelto = ResolverExpresionMontoBasadaEnTotal(normalizado, totalActual!.Value);
                Assert.That(montoResuelto, Is.GreaterThan(0m),
                    $"La expresion de monto '{monto}' debe resolver un valor mayor a cero.");
                return montoResuelto.ToString("0.00", CultureInfo.InvariantCulture);
            }

            return monto.Trim();
        }

        private static bool EsExpresionMontoBasadaEnTotal(string valorNormalizado)
        {
            if (string.IsNullOrWhiteSpace(valorNormalizado))
                return false;

            return valorNormalizado is "total" or "cubre_total" or "total_venta" ||
                   Regex.IsMatch(valorNormalizado, @"^(total|cubre_total|total_venta)[+-]\d+(?:[.,]\d+)?$");
        }

        private static decimal ResolverExpresionMontoBasadaEnTotal(string valorNormalizado, decimal totalReferencia)
        {
            if (valorNormalizado is "total" or "cubre_total" or "total_venta")
                return totalReferencia;

            var match = Regex.Match(valorNormalizado, @"^(total|cubre_total|total_venta)(?<operador>[+-])(?<delta>\d+(?:[.,]\d+)?)$");
            Assert.That(match.Success, Is.True,
                $"No se reconoce la expresion de monto '{valorNormalizado}'.");

            var deltaTexto = match.Groups["delta"].Value;
            Assert.That(TryParseDecimalFlexible(deltaTexto, out var delta), Is.True,
                $"No se pudo interpretar el delta '{deltaTexto}' de la expresion de monto.");

            return match.Groups["operador"].Value == "+"
                ? totalReferencia + delta
                : totalReferencia - delta;
        }

        private List<decimal?> ResolverMontosEsperadosPago(string montoPorMedio, decimal? totalReferencia)
        {
            return SepararValoresFiltrados(montoPorMedio)
                .Select(valor =>
                {
                    var resuelto = ResolverMontoPago(valor, totalReferencia);
                    return TryParseDecimalFlexible(resuelto, out var monto) ? monto : (decimal?)null;
                })
                .ToList();
        }

        private static bool EsNA(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ||
                   valor.Trim().Equals("NA", StringComparison.OrdinalIgnoreCase);
        }

        private static List<string> SepararValores(string valor)
        {
            if (EsNA(valor)) return new List<string>();

            return valor
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        private static List<string> SepararValoresFiltrados(string valor)
        {
            if (EsNA(valor)) return new List<string>();

            return valor
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x) &&
                            !x.Equals("NA", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private static string ConsumirSiguientePago(Queue<string> cola)
        {
            if (cola == null || cola.Count == 0)
                return "NA";

            return cola.Dequeue();
        }

        private void AssertGuardarHabilitadoEnPago(string mensajeError)
        {
            var habilitado = IsSaveEnabled();
            var mensajeActual = CapturarValidaciones();
            var resumen = ConstruirResumenPagoNuevaVenta(guardarHabilitado: habilitado, mensajeVisible: mensajeActual);

            if (!habilitado)
                Log($"[PagoNV] {resumen}");

            Assert.That(habilitado, Is.True,
                $"{mensajeError} {resumen}");
        }

        private void AssertGuardarDeshabilitadoEnPago(string mensajeError)
        {
            var habilitado = IsSaveEnabled();
            var mensajeActual = string.Join(" | ", CapturarValidacionesVisibles());
            var resumen = ConstruirResumenPagoNuevaVenta(guardarHabilitado: habilitado, mensajeVisible: mensajeActual);

            Assert.That(habilitado, Is.False,
                $"{mensajeError} {resumen}");
        }

        private void AssertCronogramaCreditoConfiguradoNuevaVenta()
        {
            if (!string.IsNullOrWhiteSpace(_lastCreditInstallments))
            {
                AssertInputExacto(
                    VentasLocators.Payment.CreditInstallmentsInput,
                    new[] { _lastCreditInstallments },
                    "El numero de cuotas deberia quedar registrado correctamente.");
            }
        }

        private void AssertMedioPagoNoDisponibleNuevaVenta(string textoEsperado, string mensajeError, params By[] locators)
        {
            var candidatos = ObtenerCandidatosTabPagoNuevaVenta(textoEsperado, locators);
            if (candidatos.Count == 0)
                return;

            var objetivo = ResolverObjetivoTabPagoNuevaVenta(candidatos[0]);
            var clases = NormalizeText(objetivo.GetAttribute("class") ?? string.Empty);
            var ariaDisabled = NormalizeText(objetivo.GetAttribute("aria-disabled") ?? string.Empty);

            if (clases.Contains("disabled") || ariaDisabled == "true")
                return;

            try
            {
                EjecutarClickTabPagoNuevaVenta(objetivo);
                Thread.Sleep(400);
            }
            catch
            {
                return;
            }

            var quedoDisponible = EsTabPagoActiva(textoEsperado) && EsContenidoPagoEsperadoVisible(textoEsperado);
            Assert.That(quedoDisponible, Is.False,
                $"{mensajeError} {ConstruirResumenPagoNuevaVenta($"tab_no_permitido={textoEsperado}")}");
        }

        private void AssertMensajePagoNoVisible(params string[] fragmentosNoPermitidos)
        {
            var visibles = CapturarValidacionesVisibles()
                .Select(NormalizeText)
                .ToList();

            foreach (var fragmento in fragmentosNoPermitidos.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var esperado = NormalizeText(fragmento);
                Assert.That(visibles.Any(v => v.Contains(esperado)), Is.False,
                    $"No deberia mostrarse el mensaje '{fragmento}'. {ConstruirResumenPagoNuevaVenta(mensajeVisible: string.Join(" | ", visibles))}");
            }
        }

        private void AssertTabPagoActiva(string textoEsperado)
        {
            Assert.That(EsTabPagoActiva(textoEsperado), Is.True,
                $"La pestana activa deberia corresponder a '{textoEsperado}'. {ConstruirResumenPagoNuevaVenta()}");
        }

        private void AssertTextoSeleccionado(IReadOnlyList<string> esperados, string mensaje, int fallbackTriggerIndex, params By[] locators)
        {
            Assert.That(esperados.Count, Is.GreaterThan(0), $"{mensaje} No se recibio un valor esperado.");

            var select = locators.Select(BuscarUltimoSelectVisibleNuevaVenta).FirstOrDefault(e => e != null);
            var trigger = FindFirstVisibleOrAny(locators) ?? ObtenerTriggerPagoVisible(fallbackTriggerIndex);
            var actual = select != null
                ? new SelectElement(select).SelectedOption?.Text?.Trim() ?? string.Empty
                : (trigger?.Text ?? string.Empty).Trim();
            var esperado = esperados[0];

            if (esperado.Contains('|'))
            {
                foreach (var parte in esperado.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    Assert.That(NormalizeText(actual), Does.Contain(NormalizeText(parte)),
                        $"{mensaje} Valor actual: '{actual}'.");
                }
                return;
            }

            Assert.That(NormalizeText(actual), Does.Contain(NormalizeText(esperado)),
                $"{mensaje} Valor actual: '{actual}'.");
        }

        private void AssertInputExacto(By locator, IReadOnlyList<string> esperados, string mensaje)
        {
            Assert.That(esperados.Count, Is.GreaterThan(0), $"{mensaje} No se recibio un valor esperado.");

            var input = ObtenerUltimoInputVisibleNuevaVenta(locator);
            Assert.That(input, Is.Not.Null, $"{mensaje} No se encontro el input visible.");

            var actual = (input!.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
            Assert.That(actual, Is.EqualTo(esperados[0]), $"{mensaje} Valor actual: '{actual}'.");
        }

        private void AssertInputAproximado(By locator, IReadOnlyList<decimal?> esperados, string mensaje)
        {
            var esperado = esperados.FirstOrDefault();
            Assert.That(esperado.HasValue, Is.True, $"{mensaje} No se recibio un monto esperado.");
            AssertInputAproximado(locator, esperado!.Value, mensaje);
        }

        private void AssertMontoMedioPagoNuevaVenta(IReadOnlyList<decimal?> esperados, string mensaje)
        {
            var esperado = esperados.FirstOrDefault();
            Assert.That(esperado.HasValue, Is.True, $"{mensaje} No se recibio un monto esperado.");

            var input = ObtenerInputMontoMedioPagoNuevaVenta();
            Assert.That(input, Is.Not.Null, $"{mensaje} No se encontro el campo visible de monto.");

            var actualTexto = (input!.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
            Assert.That(TryParseDecimalFlexible(actualTexto, out var actual), Is.True,
                $"{mensaje} No se pudo interpretar el valor actual '{actualTexto}'.");
            Assert.That(actual, Is.EqualTo(esperado!.Value).Within(0.05m),
                $"{mensaje} Valor actual: {actual:0.00} | esperado: {esperado:0.00}");
        }

        private void AssertInputAproximado(By locator, decimal esperado, string mensaje)
        {
            var input = FindFirstVisibleOrAny(locator);
            Assert.That(input, Is.Not.Null, $"{mensaje} No se encontro el campo visible.");

            var actualTexto = (input!.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
            Assert.That(TryParseDecimalFlexible(actualTexto, out var actual), Is.True,
                $"{mensaje} No se pudo interpretar el valor actual '{actualTexto}'.");
            Assert.That(actual, Is.EqualTo(esperado).Within(0.05m),
                $"{mensaje} Valor actual: {actual:0.00} | esperado: {esperado:0.00}");
        }

        private void TryAssertInputAproximado(By locator, decimal esperado, string mensaje)
        {
            try
            {
                var input = FindFirstVisibleOrAny(locator);
                if (input == null)
                    return;

                var actualTexto = (input.GetAttribute("value") ?? input.Text ?? string.Empty).Trim();
                if (!TryParseDecimalFlexible(actualTexto, out var actual))
                    return;

                if (actual <= 0m)
                    return;

                Assert.That(actual, Is.EqualTo(esperado).Within(0.05m),
                    $"{mensaje} Valor actual: {actual:0.00} | esperado: {esperado:0.00}");
            }
            catch
            {
            }
        }

        private bool IsNewSaleFormReset()
        {
            var indicator = driver.FindElements(By.XPath("//*[contains(normalize-space(),'Ningun producto seleccionado') or contains(normalize-space(),'NingÃºn producto seleccionado')]") )
                .FirstOrDefault(e =>
                {
                    try { return e.Displayed; }
                    catch { return false; }
                });
            return indicator != null;
        }

        private void TryClickOptional(params By[] locators)
        {
            foreach (var loc in locators)
            {
                try
                {
                    var element = driver.FindElements(loc)
                        .FirstOrDefault(e =>
                        {
                            try { return e.Displayed && e.Enabled; }
                            catch { return false; }
                        });

                    if (element == null)
                        continue;

                    ScrollToCenter(element);
                    element.Click();
                    Thread.Sleep(700);
                    return;
                }
                catch
                {
                }
            }
        }

        private IWebElement Find(params By[] locators)
        {
            foreach (var loc in locators)
            {
                var el = driver.FindElements(loc).FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });
                if (el != null) return el;
            }
            throw new NoSuchElementException($"No se encontro: {string.Join(" | ", locators.Select(l => l.ToString()))}");
        }

        private void ScrollToCenter(IWebElement el)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView({block:'center',behavior:'instant'});", el);
            Thread.Sleep(300);
        }

        private void Click(params By[] locators)
        {
            foreach (var loc in locators)
            {
                try
                {
                    var el = wait.Until(d =>
                    {
                        var elements = d.FindElements(loc);
                        return elements.FirstOrDefault(e => { try { return e.Displayed && e.Enabled; } catch { return false; } });
                    });
                    if (el != null)
                    {
                        ScrollToCenter(el);
                        el.Click();
                        Thread.Sleep(300);
                        return;
                    }
                }
                catch { continue; }
            }
            throw new NoSuchElementException($"No se pudo hacer clic: {string.Join(" | ", locators.Select(l => l.ToString()))}");
        }

        private bool IsSaveEnabled()
        {
            IWebElement? btn = null;
            for (int intento = 0; intento < 20; intento++)
            {
                btn = driver.FindElements(VentasLocators.NuevaVenta.GuardarVenta)
                    .FirstOrDefault(e => { try { return e.Displayed; } catch { return false; } });

                if (btn != null && EstaHabilitadoBotonGuardar(btn))
                    return true;

                Thread.Sleep(150);
            }

            if (btn == null) return false;
            return EstaHabilitadoBotonGuardar(btn);
        }

        private IWebElement? ObtenerBotonAgregarMedioPagoVisible()
        {
            try
            {
                return driver.FindElements(VentasLocators.Payment.AddPaymentButton)
                    .FirstOrDefault(e =>
                    {
                        try { return e.Displayed; }
                        catch { return false; }
                    });
            }
            catch
            {
                return null;
            }
        }

        private bool? ObtenerEstadoAgregarMedioPagoActualNuevaVenta()
        {
            var boton = ObtenerBotonAgregarMedioPagoVisible();
            return boton == null ? null : EstaHabilitadoBotonAccion(boton);
        }

        private bool EstaHabilitadoBotonAccion(IWebElement boton)
        {
            try
            {
                var classes = boton.GetAttribute("class") ?? string.Empty;
                var ariaDisabled = boton.GetAttribute("aria-disabled") ?? string.Empty;
                var disabled = boton.GetAttribute("disabled") ?? string.Empty;

                return boton.Enabled &&
                       !classes.Contains("disabled", StringComparison.OrdinalIgnoreCase) &&
                       !ariaDisabled.Equals("true", StringComparison.OrdinalIgnoreCase) &&
                       string.IsNullOrWhiteSpace(disabled);
            }
            catch
            {
                return false;
            }
        }

        private bool EstaHabilitadoBotonGuardar(IWebElement boton)
        {
            return EstaHabilitadoBotonAccion(boton);
        }

        // Captura el primer mensaje de validacion visible: primero toasts/popups bloqueantes,
        // luego invalid-feedback / text-danger inline en el formulario.
        private string CapturarValidaciones()
        {
            return CapturarValidacionesVisibles().FirstOrDefault() ?? string.Empty;
        }

        private IReadOnlyList<string> CapturarValidacionesVisibles()
        {
            var mensajes = new List<string>();

            var popup = CaptureVisibleMessage(1);
            if (!string.IsNullOrWhiteSpace(popup) && IsBlockingMessage(popup))
                mensajes.Add(popup.Trim());

            mensajes.AddRange(driver.FindElements(By.XPath(
                    "//*[contains(@class,'invalid-feedback') or contains(@class,'text-danger') or " +
                    "contains(@class,'custom-error-message') or contains(@class,'mat-error') or " +
                    "contains(@class,'error-message') or contains(@class,'validation-error') or " +
                    "contains(@class,'field-error')][normalize-space()] | " +
                    "//*[@role='alert'][normalize-space()] | " +
                    "//small[contains(@class,'error') or contains(@class,'danger')][normalize-space()]"))
                .Where(e => { try { return e.Displayed; } catch { return false; } })
                .Select(e => e.Text?.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))!
                .Select(t => t!));

            return mensajes
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private IReadOnlyList<string> CapturarValidacionesVisiblesDetalladas()
        {
            var mensajes = new List<string>();

            var popup = CaptureVisibleMessage(1);
            if (!string.IsNullOrWhiteSpace(popup) && IsBlockingMessage(popup))
                mensajes.Add(popup.Trim());

            var validaciones = driver.FindElements(By.XPath(
                    "//*[contains(@class,'invalid-feedback') or contains(@class,'text-danger') or " +
                    "contains(@class,'custom-error-message') or contains(@class,'mat-error') or " +
                    "contains(@class,'error-message') or contains(@class,'validation-error') or " +
                    "contains(@class,'field-error')][normalize-space()] | " +
                    "//*[@role='alert'][normalize-space()] | " +
                    "//small[contains(@class,'error') or contains(@class,'danger')][normalize-space()]"))
                .Where(e => { try { return e.Displayed; } catch { return false; } })
                .ToList();

            foreach (var validacion in validaciones)
            {
                string mensaje;
                try { mensaje = validacion.Text?.Trim() ?? string.Empty; }
                catch { continue; }

                if (string.IsNullOrWhiteSpace(mensaje))
                    continue;

                string campo = ObtenerCampoDeValidacion(validacion);
                mensajes.Add(string.IsNullOrWhiteSpace(campo) ? mensaje : $"{campo}: {mensaje}");
            }

            return mensajes
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private string ObtenerCampoDeValidacion(IWebElement validacion)
        {
            try
            {
                var result = ((IJavaScriptExecutor)driver).ExecuteScript(@"
                    const msg = arguments[0];
                    const clean = value => (value || '').replace(/\*/g, '').replace(/\s+/g, ' ').trim();
                    const invalid = value => {
                        const text = clean(value).toLowerCase();
                        return !text ||
                            text === 'este campo es obligatorio' ||
                            text === 'seleccione' ||
                            text === 'buscar...' ||
                            text === 'na';
                    };

                    let node = msg;
                    for (let depth = 0; node && depth < 6; depth++, node = node.parentElement) {
                        const labels = Array.from(node.querySelectorAll('label'))
                            .map(label => clean(label.textContent))
                            .filter(text => !invalid(text));
                        if (labels.length > 0) return labels[labels.length - 1];

                        let prev = node.previousElementSibling;
                        for (let guard = 0; prev && guard < 6; guard++, prev = prev.previousElementSibling) {
                            if (prev.matches && prev.matches('label')) {
                                const text = clean(prev.textContent);
                                if (!invalid(text)) return text;
                            }

                            const label = prev.querySelector && prev.querySelector('label');
                            if (label) {
                                const text = clean(label.textContent);
                                if (!invalid(text)) return text;
                            }
                        }
                    }

                    const container = msg.closest('.col, .col-md-6, .col-sm-6, .mb-3, .form-group, div');
                    const control = container ? container.querySelector('input, select, textarea') : null;
                    if (control) {
                        const id = control.getAttribute('id') || control.getAttribute('formcontrolname') || control.getAttribute('name');
                        if (id) {
                            const labels = Array.from(document.querySelectorAll('label'))
                                .filter(label => label.getAttribute('for') === id)
                                .map(label => clean(label.textContent))
                                .filter(text => !invalid(text));
                            if (labels.length > 0) return labels[labels.length - 1];
                            return id;
                        }
                    }

                    return '';
                ", validacion)?.ToString()?.Trim() ?? string.Empty;

                return NormalizarCampoGuia(result);
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string NormalizarCampoGuia(string campo)
        {
            if (string.IsNullOrWhiteSpace(campo))
                return string.Empty;

            string normalizado = NormalizeText(campo);

            if (normalizado.Contains("peso"))
                return "Peso bruto";
            if (normalizado.Contains("bulto"))
                return "Numero de bultos";
            if (normalizado.Contains("licencia"))
                return "Numero de licencia";
            if (normalizado.Contains("placa"))
                return "Numero de placa";
            if (normalizado.Contains("transportista") || normalizado.Contains("ruc"))
                return "Transportista RUC";
            if (normalizado.Contains("conductor") || normalizado.Contains("dni"))
                return "Conductor DNI";

            return campo.Replace("*", string.Empty).Trim();
        }

        private void AssertMensajesValidacionPago(string mensajeError, params string[] fragmentosEsperados)
        {
            var visibles = CapturarValidacionesVisibles();
            var resumenMensajes = string.Join(" | ", visibles);
            var resumen = ConstruirResumenPagoNuevaVenta(mensajeVisible: resumenMensajes);

            foreach (var fragmento in fragmentosEsperados.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var esperado = NormalizeText(fragmento);
                Assert.That(visibles.Any(v => NormalizeText(v).Contains(esperado)), Is.True,
                    $"{mensajeError} Falta el mensaje '{fragmento}'. {resumen}");
            }
        }

        private void AssertAlgunMensajeValidacionPago(string mensajeError, params string[] fragmentosEsperados)
        {
            var visibles = CapturarValidacionesVisibles();

            // Busqueda adicional: texto visible en toda la seccion de pago que coincida con los fragmentos.
            // Cubre casos donde el mensaje usa clases CSS no contempladas (ej: inline bajo campo de fecha).
            if (!visibles.Any())
            {
                var todoElTextoSeccionPago = driver.FindElements(By.XPath(
                    "//div[contains(@class,'accordion-body') or contains(@class,'pago') or contains(@id,'pago')]" +
                    "//*[normalize-space() and not(self::script) and not(self::style)]"))
                    .Where(e => { try { return e.Displayed; } catch { return false; } })
                    .Select(e => e.Text?.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t) && t!.Length < 200)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var esperadosNorm = fragmentosEsperados.Select(NormalizeText).ToList();
                var encontrado = todoElTextoSeccionPago
                    .FirstOrDefault(t => esperadosNorm.Any(e => NormalizeText(t!).Contains(e)));
                if (encontrado != null)
                    visibles = new List<string> { encontrado };
            }

            var resumenMensajes = string.Join(" | ", visibles);
            var resumen = ConstruirResumenPagoNuevaVenta(mensajeVisible: resumenMensajes);
            var esperados = fragmentosEsperados
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(NormalizeText)
                .ToList();

            Assert.That(esperados.Count, Is.GreaterThan(0),
                $"{mensajeError} No se recibieron fragmentos esperados.");
            Assert.That(visibles.Any(v => esperados.Any(e => NormalizeText(v).Contains(e))), Is.True,
                $"{mensajeError} No se encontro ninguno de los mensajes esperados: {string.Join(" | ", fragmentosEsperados)}. {resumen}");
        }

        private void AssertAgregarMedioPagoDeshabilitado(string mensajeError)
        {
            var habilitado = ObtenerEstadoAgregarMedioPagoActualNuevaVenta();
            var resumen = ConstruirResumenPagoNuevaVenta();

            Assert.That(habilitado.HasValue, Is.True,
                $"{mensajeError} No se encontro el boton Agregar Medio de Pago. {resumen}");
            Assert.That(habilitado.GetValueOrDefault(), Is.False,
                $"{mensajeError} {resumen}");
        }

        // Devuelve true si el mensaje es una validacion bloqueante real (error/advertencia del negocio).
        // Devuelve false para mensajes informativos de exito del sistema que no representan un problema.
        private static void Log(string msg) =>
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {msg}");

        private static bool IsBlockingMessage(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return false;
            var n = NormalizeText(msg);
            // Mensajes informativos del sistema que confirman que el formulario esta bien;
            // no son errores de validacion ni impiden guardar.
            if (n.Contains("completo los campos") ||
                n.Contains("completo los datos") ||
                n.Contains("campos requeridos correctamente"))
                return false;
            return true;
        }

        private static bool EsValidacionBloqueanteFormulario(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return false;
            var n = NormalizeText(msg);

            if (n.Contains("se completo los datos correctamente") ||
                n.Contains("se completo los campos correctamente"))
                return false;

            return n.Contains("no debe") ||
                   n.Contains("obligatorio") ||
                   n.Contains("requerido") ||
                   n.Contains("completar los campos") ||
                   n.Contains("invalido") ||
                   n.Contains("error");
        }

        private static string NormalizeText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var formD = value.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(formD.Length);

            foreach (var c in formD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(char.ToLowerInvariant(c));
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private static bool EsValorOmitido(string valor)
        {
            var normalizado = NormalizeText(valor);
            return string.IsNullOrWhiteSpace(normalizado) || normalizado == "-" || normalizado == "na";
        }

        private sealed class PrecondicionConceptoVendibleConfig
        {
            public string TipoProducto { get; set; } = "Bien";
            public string TratamientoIgvFamilia { get; set; } = "Exoneracion IGV";
            public string CategoriaFamilia { get; set; } = "SIN CATEGORÍA";
            public string UmComercialConcepto { get; set; } = "ML";
            public string UMedidaConcepto { get; set; } = "ML";
            public string RolConcepto { get; set; } = "Item Comercial";
            public string ModuloConcepto { get; set; } = "MOD0001";
            public string MarcaConcepto { get; set; } = "KR";
            public string SufijoConcepto { get; set; } = string.Empty;
            public string PresentacionConcepto { get; set; } = "BOTELLAS";
            public string CantidadBaseConcepto { get; set; } = "1";
            public string TarifaConcepto { get; set; } = "POR UNIDAD";
            public string PrecioProducto { get; set; } = "7.10";
            public string DocumentoAdquisicion { get; set; } = "NOTA DE COMPRA (INTERNA)";
            public string ProveedorAdquisicion { get; set; } = "10759012017";
            public string InformacionAdquisicion { get; set; } = "Stock QA Ventas {concepto}";
            public string TipoEntregaAdquisicion { get; set; } = "Inmediata";
            public string RolAdquisicion { get; set; } = "Item Comercial";
            public string EstablecimientoAdquisicion { get; set; } = "RECSA - CENTRAL";
            public string AlmacenAdquisicion { get; set; } = "CENTRO COMERCIAL CENTRAL";
            public string TipoPagoAdquisicion { get; set; } = "Contado";
            public string MedioPagoAdquisicion { get; set; } = "efectivo";
            public string ObservacionPagoAdquisicion { get; set; } = "QA Ventas {concepto}";
            public string PrecioCompraAdquisicion { get; set; } = "1";

            public static PrecondicionConceptoVendibleConfig CreateDefault() => new();
        }
    }

    }

