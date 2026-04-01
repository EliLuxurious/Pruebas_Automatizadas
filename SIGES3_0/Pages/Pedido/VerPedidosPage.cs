using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SIGES3_0.Pages.Componentes;
using SIGES3_0.Pages.Helpers;
using System;
using System.Linq;

namespace SIGES3_0.Pages.PedidoPage
{
    public class VerPedidosPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        public VerPedidosPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // --- Navegación ---
        private By moduloPedido = By.XPath("//span[normalize-space()='Pedidos']/ancestor::a");
        private By submoduloVerPedidos = By.XPath("//span[normalize-space()='Ver Pedidos']");

        // --- Nuevo pedido ---
        private By btnNuevoPedido = By.XPath("//button[normalize-space()='Nuevo Pedido']");
        private By cmbFamilia = By.XPath("//span[normalize-space()='Seleccionar familia']");
        private By cmbConcepto = By.XPath("//span[normalize-space()='Seleccionar un concepto']");
        private By txtCantidad = By.XPath("// table/tbody/tr[1]//input");
        private By chkIGV = By.XPath("//label[normalize-space()='IGV']");
        private By chkDetUnif = By.XPath("//label[normalize-space()='DET.UNIF.']");
        private By chkDescuento = By.XPath("//label[normalize-space()='Descuento']");
        private By btnDescuentoItem = By.XPath("//button[normalize-space()='Item']");
        private By btnDescuentoGlobal = By.XPath("//button[normalize-space()='Global']");
        private By btnDescuentoSoles = By.XPath("//button[normalize-space()='$']");
        private By btnDescuentoPorcentaje = By.XPath("//button[normalize-space()='%']");
        private By txtDescuento = By.XPath("//input[@placeholder='0']");

        private By txtCliente = By.CssSelector("input.search-input[placeholder='Buscar...']");
        private By rbtEntregaInmediata = By.XPath("//label[normalize-space()='Inmediata']");
        private By rbtEntregaDiferida = By.XPath("//label[normalize-space()='Diferida']");
        private By btnRegistrarPedido = By.XPath("//button[normalize-space()='Registrar Pedido']");
        private By mensajeError = By.XPath("//strong[normalize-space()='Se encontraron inconsistencias en los datos:']");
        private By btnOKConfirmacion = By.XPath("//button[normalize-space()='OK']");
        private By mensajeAdvertencia = By.XPath("//span[contains(@class,'badge-status') and contains(@class,'danger')]");
        private By mensajeSinProducto = By.XPath("//span[@class='badge-status danger']");
        private By loadingContainer = By.CssSelector("div.loading-container");

        // EDITAR PEDIDO
        private By btnEditarPrimerRegistro = By.XPath("//tbody/tr[1]/td[9]/div[1]/button[1]");
        private By btnEditarPedidoFinal = By.XPath("//button[normalize-space()='Editar Pedido']");

        // INVALIDAR PEDIDO
        private By txtFiltroEstado = By.XPath("//th[8]//input[1]");
        private By btnInvalidarPrimerRegistro = By.XPath("//tbody/tr[1]/td[9]/div[1]/button[2]");

        private By txtMotivoInvalidacion = By.XPath("//textarea[@placeholder='Ingrese el motivo de la anulación...']");
        private By btnSiInvalidar = By.XPath("//button[normalize-space()='Sí']");
        private By btnNoInvalidar = By.XPath("//button[normalize-space()='No']");

        // CONFIRMAR PEDIDO
        private By txtFiltroTotal = By.XPath("//th[7]//input[1]");
        private By btnConfirmarPrimerRegistro = By.XPath("//tbody/tr[1]/td[9]/div[1]/button[3]");
        // Botón final del modal: el texto puede estar dentro de un <span>, por eso usamos contains(.) en vez de text()
        private By btnConfirmarPedidoFinal = By.XPath("//button[contains(normalize-space(.),'Confirmar Pedido') or .//*[contains(normalize-space(.),'Confirmar Pedido')]]");

        private By seccionFacturacionConfirmacion = By.XPath("//div[contains(@class,'d-flex') and contains(@class,'align-items-center') and contains(@class,'w-100')]" +"[.//span[normalize-space()='Facturación']]");
        private By seccionEntregaConfirmacion = By.XPath("//span[normalize-space()='Entrega']/ancestor::div[contains(@class,'d-flex align-items-center w-100')][1]");
        private By seccionPagoConfirmacion = By.XPath("//span[normalize-space()='Pago']/ancestor::div[contains(@class,'d-flex align-items-center w-100')][1]");

        private By txtClienteConfirmacion = By.CssSelector("input.search-input[placeholder='Buscar...']");

        //private By cmbTipoComprobanteConfirmacion = By.XPath("//span[@class='select-value ng-star-inserted']");
        private By cmbTipoComprobanteConfirmacion = By.XPath("//div[contains(@class,'select-trigger') and contains(@class,'form-control')]");
        private By panelDropdownNgSelect = By.CssSelector(".ng-dropdown-panel");

        // ENTREGA CONFIRMAR
        private By rbtEntregaInmediataConfirmacion = By.XPath("//label[normalize-space()='Inmediata']");
        private By rbtEntregaDiferidaConfirmacion = By.XPath("//labelS[normalize-space()='Diferida']");
        private By btnGuiaRemisionConfirmacion = By.XPath("//span[normalize-space()='Guia de remisión']");
        private By btnCerrarEntregaConfirmacion = By.XPath("(//*[contains(@class,'ri-arrow-up-s-line') or contains(@class,'ri-arrow-down-s-line')])[2]");

        // PAGO CONFIRMAR
        private By rbtContadoConfirmacion = By.XPath("//label[normalize-space()='Contado']");
        private By tabEfectivoConfirmacion = By.XPath("//*[contains(text(),'EFECTIVO')]");
        private By txtRecibidoEfectivo = By.XPath("//input[@id='amountReceived']");

        private By btnCerrarPagoConfirmacion = By.XPath("(//*[contains(@class,'ri-arrow-up-s-line') or contains(@class,'ri-arrow-down-s-line')])[3]");

        // CONFIRMAR PEDIDO - MEDIOS DE PAGOS
        private By chkMultipagoConfirmacion = By.XPath("//input[@id='checkTypePaymentMethod']");

        private By tabTarjetaCreditoConfirmacion = By.XPath("//span[normalize-space()='TARJETAS DE CREDITO']");
        private By tabTarjetaDebitoConfirmacion = By.XPath("//span[normalize-space()='TARJETAS DE DEBITO']");
        private By tabTransferenciaConfirmacion = By.XPath("//span[normalize-space()='TRANSFERENCIA DE FONDOS']");
        private By tabDepositosConfirmacion = By.XPath("//span[normalize-space()='DEPOSITOS EN CUENTA']");
        private By tabPuntosConfirmacion = By.XPath("//span[normalize-space()='PUNTOS']");

        private By cmbBancoConfirmacion = By.XPath("//select[@id='bankEntityId']");
        private By cmbTarjetaConfirmacion = By.XPath("//select[@id='bankingCard']");
        private By txtInformacionConfirmacion = By.XPath("//input[@id='informacion']");
        private By cmbCuentaBancariaConfirmacion = By.XPath("//select[@id='bankEntityId']");
        private By txtNumeroCuotasConfirmacion = By.XPath("//input[@type='number'][@min='1'][@max='60']");
        private By txtMontoInicialCreditoConfirmacion = By.XPath("//input[@type='number'][@min='0']");
        private By btnAgregarMedioPagoConfirmacion = By.XPath("//button[normalize-space()='Agregar Medio de Pago']");

        private By rbtCreditoConfirmacion = By.XPath("//label[normalize-space()='Crédito']");

        //PARA TOTAL BASE
        private const string TOTAL_BASE_MAYOR_700 = "759";
        private const string TOTAL_BASE_MENOR_IGUAL_700 = "64";

        //-------------------------
        private string ultimaAccion = "";

        private By OpcionComprobante(string tipoComprobante)
        {
            string clave = NormalizarTextoComprobante(tipoComprobante);

            return By.XPath(
                $"//div[contains(@class,'ng-option') and normalize-space(.)='{clave}']" +
                $" | //div[contains(@class,'ng-option')]//span[normalize-space(.)='{clave}']" +
                $" | //li[normalize-space(.)='{clave}']" +
                $" | //span[normalize-space(.)='{clave}']"
            );
        }

        private string NormalizarTextoComprobante(string tipoComprobante)
        {
            string t = (tipoComprobante ?? "").Trim().ToUpperInvariant()
                .Replace("Á", "A").Replace("É", "E").Replace("Í", "I")
                .Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N");

            // Mapeo: texto del feature → fragmento exacto que aparece en el DOM
            if (t.Contains("NOTA DE VENTA")) return "NOTA DE VENTA";   // matchea "NOTA DE VENTA(INTERNA)"
            if (t.Contains("FACTURA")) return "FACTURA ELECTRONICA";
            if (t.Contains("BOLETA")) return "BOLETA DE VENTA ELECTRONICA";

            return t;
        }

        private By OpcionSerie(string serie)
        {
            return By.XPath($"//*[contains(text(),'{serie}')]");
        }

        private string ObtenerTextoComprobanteSeleccionado()
        {
            try
            {
                var combo = wait.Until(d =>
                {
                    try
                    {
                        var combos = d.FindElements(cmbTipoComprobanteConfirmacion)
                            .Where(e => e.Displayed)
                            .ToList();

                        if (!combos.Any()) return null;

                        // Priorizar el que está en la sección de facturación visible
                        return combos.Last();
                    }
                    catch
                    {
                        return null;
                    }
                });

                return (combo?.Text ?? string.Empty).Trim().ToUpperInvariant();
            }
            catch
            {
                return string.Empty;
            }
        }

        private bool ComprobanteSeleccionadoCoincide(string tipoComprobanteEsperado)
        {
            string actual = ObtenerTextoComprobanteSeleccionado();

            if (string.IsNullOrWhiteSpace(actual))
                return false;

            string esperado = NormalizarTextoComprobante(tipoComprobanteEsperado);

            // Caso especial del DOM: "NOTA DE VENTA(INTERNA)"
            if (esperado.Contains("NOTA DE VENTA"))
                return actual.Contains("NOTA DE VENTA");

            if (esperado.Contains("FACTURA"))
                return actual.Contains("FACTURA ELECTRONICA");

            if (esperado.Contains("BOLETA"))
                return actual.Contains("BOLETA DE VENTA ELECTRONICA");

            return actual.Contains(esperado);
        }

        private string ObtenerTextoBusquedaComprobante(string tipoComprobante)
        {
            string t = (tipoComprobante ?? "").Trim().ToUpperInvariant();

            if (t.Contains("NOTA DE VENTA")) return "NOTA";
            if (t.Contains("FACTURA")) return "FACTURA";
            if (t.Contains("BOLETA")) return "BOLETA";

            return t;
        }

        private bool CoincideComprobante(string textoVisible, string tipoComprobante)
        {
            string actual = (textoVisible ?? "").Trim().ToUpperInvariant();
            string esperado = NormalizarTextoComprobante(tipoComprobante);

            if (esperado.Contains("NOTA DE VENTA"))
                return actual.Contains("NOTA DE VENTA");

            if (esperado.Contains("FACTURA"))
                return actual.Contains("FACTURA ELECTRONICA");

            if (esperado.Contains("BOLETA"))
                return actual.Contains("BOLETA DE VENTA ELECTRONICA");

            return actual.Contains(esperado);
        }

        // ======================================================
        // METODOS
        // ======================================================

        public void SeleccionarOpcion(string opcion)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            var boton = wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//*[contains(text(),'{opcion}')]")
                )
            );

            boton.Click();
        }

        public void SeleccionarFamilia(string familia)
        {
            if (familia == "ninguno") return;

            // abrir dropdown
            wait.Until(ExpectedConditions.ElementToBeClickable(cmbFamilia)).Click();

            // volver a buscar la opción (evita stale)
            var opcion = wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(By.XPath($"//span[normalize-space()='{familia}']"));
                    return el.Displayed ? el : null;
                }
                catch
                {
                    return null;
                }
            });

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", opcion);
        }



        public void SeleccionarConcepto(string concepto)
        {
            if (concepto == "ninguno") return;

            // abrir dropdown concepto
            var dropdown = wait.Until(
                ExpectedConditions.ElementToBeClickable(cmbConcepto)
            );
            dropdown.Click();

            Thread.Sleep(1000); // dar tiempo a que cargue la lista según la familia

            // intentar ubicar la opción del concepto
            var opcion = wait.Until(d =>
            {
                try
                {
                    var elementos = d.FindElements(By.XPath($"//*[contains(text(),'{concepto}')]"));
                    foreach (var elemento in elementos)
                    {
                        if (elemento.Displayed)
                            return elemento;
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            });

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", opcion);

            Thread.Sleep(300);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", opcion);
        }

        public void IngresarCantidad(string cantidad)
        {
            if (cantidad == "0") return;

            var input = wait.Until(
                ExpectedConditions.ElementIsVisible(txtCantidad)
            );

            // Dar foco
            input.Click();
            Thread.Sleep(200);

            // Seleccionar todo y reemplazar — esto SÍ dispara eventos en Angular
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(200);

            // Escribir valor nuevo
            input.SendKeys(cantidad);

            // Forzar blur para que Angular valide el cambio y habilite el botón
            input.SendKeys(Keys.Tab);
            Thread.Sleep(500);
        }

        public void ActivarIGV(string valor)
        {
            if (valor == "true")
            {
                driver.FindElement(chkIGV).Click();
            }
        }

        public void ActivarDetUnif(string valor)
        {
            if (valor == "true")
            {
                driver.FindElement(chkDetUnif).Click();
            }
        }

        public void ConfigurarDescuento(string activo, string tipo, string modo, string valor)
        {
            if (activo != "true") return;

            driver.FindElement(chkDescuento).Click();

            // Selección tipo descuento
            if (tipo == "item")
                driver.FindElement(btnDescuentoItem).Click();

            if (tipo == "global")
                driver.FindElement(btnDescuentoGlobal).Click();


            // Selección modo descuento
            if (modo == "$")
                driver.FindElement(btnDescuentoSoles).Click();

            if (modo == "%")
                driver.FindElement(btnDescuentoPorcentaje).Click();


            // Ingresar valor
            var input = wait.Until(ExpectedConditions.ElementIsVisible(txtDescuento));

            input.Clear();
            input.SendKeys(valor);
        }

        public void BuscarCliente(string cliente)
        {
            try
            {
                // cliente VARIOS
                if (cliente == "00000000" || cliente.ToLower() == "varios")
                {
                    Console.WriteLine("Cliente VARIOS - no se realiza búsqueda");
                    return;
                }

                var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(25));

                // Esperar visible + scrollear + dar foco real al input
                var input = waitLong.Until(
                    ExpectedConditions.ElementIsVisible(txtCliente)
                );

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);

                waitLong.Until(ExpectedConditions.ElementToBeClickable(txtCliente)).Click();

                // Limpieza robusta (más confiable que Clear() en inputs con autocomplete/máscara)
                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Delete);

                // Escribir DNI o RUC
                input.SendKeys(cliente);

                // Algunos ambientes muestran un autocomplete (lista de opciones) y otros
                // resuelven la búsqueda con ENTER sin desplegar lista.
                // 1) Intentar seleccionar una opción visible (si existe) con un timeout corto.
                try
                {
                    var waitDropdown = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                    var opcion = waitDropdown.Until(d =>
                    {
                        try
                        {
                            var opciones = d.FindElements(By.CssSelector(
                                ".ng-dropdown-panel .ng-option, .dropdown-menu .dropdown-item, .autocomplete-items *"
                            ));
                            return opciones.FirstOrDefault(o => o.Displayed && o.Enabled);
                        }
                        catch
                        {
                            return null;
                        }
                    });

                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("arguments[0].click();", opcion);
                }
                catch (WebDriverTimeoutException)
                {
                    // 2) Fallback: disparar búsqueda con ENTER
                    input.SendKeys(Keys.Enter);
                }

                // Esperar que el campo quede poblado (puede ser DNI/RUC o el nombre del cliente, depende del UI)
                waitLong.Until(d =>
                {
                    try
                    {
                        var val = input.GetAttribute("value") ?? "";
                        return val.Trim().Length > 0;
                    }
                    catch
                    {
                        return false;
                    }
                });

            }
            catch (Exception e)
            {
                Console.WriteLine("Error buscando cliente: " + e.Message);
                throw;
            }
        }

        public void SeleccionarEntrega(string tipoEntrega)
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            string xpathOpcion = tipoEntrega.Trim().Equals("inmediata", StringComparison.OrdinalIgnoreCase)
                ? "//label[normalize-space()='Inmediata']"
                : "//label[normalize-space()='Diferida']";

            var opcion = waitLong.Until(d =>
            {
                try
                {
                    var elementos = d.FindElements(By.XPath(xpathOpcion));
                    return elementos.FirstOrDefault(e => e.Displayed);
                }
                catch { return null; }
            });

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", opcion);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", opcion);
        }

        public void RegistrarPedido()
        {
            ultimaAccion = "registrar";

            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            var boton = waitLong.Until(
                ExpectedConditions.ElementExists(btnRegistrarPedido)
            );

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            Thread.Sleep(800);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", boton);
        }

        // Validar si existe al menos un pedido REGISTRADO (asume que el filtro ya fue aplicado).
        public bool ExistePedidoRegistradoFiltrado()
        {
            try
            {
                var botonesInvalidar = driver.FindElements(btnInvalidarPrimerRegistro);
                return botonesInvalidar.Count > 0 && botonesInvalidar[0].Displayed;
            }
            catch
            {
                return false;
            }
        }

        // aplica filtro por estado
        public void FiltrarPedidosRegistrados()
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            var filtroEstado = waitLong.Until(
                ExpectedConditions.ElementIsVisible(txtFiltroEstado)
            );

            filtroEstado.Clear();
            filtroEstado.SendKeys("REGISTRADO");

            // Esperar a que termine cualquier overlay de carga (si existe)
            try
            {
                waitLong.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingContainer));
            }
            catch
            {
                // Si el overlay no existe o no se detecta, continuamos igual.
            }
        }

        //ABRIR SECCION
        public void AbrirSeccion(string seccion)
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(25));

            // Esperar que no haya overlay activo
            try
            {
                waitLong.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingContainer));
            }
            catch { }

            // Localizar el h2 header del acordeón
            var header = waitLong.Until(d =>
            {
                try
                {
                    var h2 = d.FindElement(By.XPath(
                        $"//h2[contains(@class,'accordion-header')][.//*[contains(normalize-space(.),'{seccion}')]]"
                    ));
                    return h2.Displayed ? h2 : null;
                }
                catch { return null; }
            });

            // Verificar si el contenido YA está visible (acordeón abierto)
            bool yaEstaAbierto = EsContenidoVisible(seccion);

            if (yaEstaAbierto)
            {
                Console.WriteLine($"[AbrirSeccion] Sección '{seccion}' ya está abierta, no se hace click.");
                return;
            }

    // Scroll y click en el header
    ((IJavaScriptExecutor)driver)
        .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", header);

            Thread.Sleep(300);

            IWebElement clickable;
            try
            {
                clickable = header.FindElement(By.XPath(".//button | .//*[@role='button']"));
            }
            catch
            {
                clickable = header;
            }

    ((IJavaScriptExecutor)driver)
        .ExecuteScript("arguments[0].click();", clickable);

            // Esperar que el contenido quede visible
            waitLong.Until(d => EsContenidoVisible(seccion));
        }

        // Verifica si el contenido de la sección está visible buscando en todo el DOM
        private bool EsContenidoVisible(string seccion)
        {
            try
            {
                if (seccion.Trim().Equals("Facturación", StringComparison.OrdinalIgnoreCase))
                {
                    return driver.FindElements(
                        By.CssSelector("input.search-input[placeholder='Buscar...']")
                    ).Any(e => e.Displayed);
                }

                if (seccion.Trim().Equals("Entrega", StringComparison.OrdinalIgnoreCase))
                {
                    return driver.FindElements(
                        By.XPath("//label[normalize-space()='Inmediata' or normalize-space()='Diferida']")
                    ).Any(e => e.Displayed);
                }

                // Genérico: busca el app-form-accordion con clase is-expanded
                return driver.FindElements(By.XPath(
                    $"//app-form-accordion[contains(@class,'is-expanded')]" +
                    $"[.//h2[contains(@class,'accordion-header')][.//*[contains(normalize-space(.),'{seccion}')]]]"
                )).Any(e => e.Displayed);
            }
            catch
            {
                return false;
            }
        }

        private IWebElement ObtenerAccordion(string seccion)
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            return waitLong.Until(d =>
            {
                try
                {
                    var acc = d.FindElement(By.XPath(
                        $"//app-form-accordion[.//h2[contains(@class,'accordion-header') and contains(normalize-space(.),'{seccion}')]]"
                    ));

                    return acc.Displayed ? acc : null;
                }
                catch
                {
                    return null;
                }
            });
        }

        private bool ContenidoVisible(IWebElement accordion, string seccion)
        {
            try
            {
                if (seccion.Trim().Equals("Facturación", StringComparison.OrdinalIgnoreCase))
                {
                    return accordion.FindElements(By.CssSelector("input.search-input[placeholder='Buscar...']"))
                                    .Any(e => e.Displayed);
                }

                if (seccion.Trim().Equals("Entrega", StringComparison.OrdinalIgnoreCase))
                {
                    return accordion.FindElements(By.XPath(
                        ".//label[normalize-space()='Inmediata' or normalize-space()='Diferida']"
                    )).Any(e => e.Displayed);
                }

                return accordion.FindElements(By.XPath(".//div[contains(@class,'accordion-body')]"))
                                .Any(e => e.Displayed);
            }
            catch
            {
                return false;
            }
        }     

        public void VolverAVerPedidos()
        {
            var opcion = wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.XPath("//span[contains(text(),'Ver Pedidos')]")
                )
            );

            opcion.Click();
            Thread.Sleep(1000);
        }

        // EDITAR PEDIDOS      
        public void ActualizarPedido(
        string familia, string concepto, string cantidad, string igv,
        string detUnif, string descuentoActivo, string tipoDescuento,
        string modoDescuento, string valorDescuento, string cliente, string tipoEntrega)
        {
            // Valores que indican "no tocar este campo"
            var sinCambio = new[] { "NA", "sin_cambio" };

            bool HayCambio(string valor) =>
                !sinCambio.Any(s => s.Equals(valor.Trim(), StringComparison.OrdinalIgnoreCase));

            // Si absolutamente ningún campo tiene un cambio real, marcar como sin modificación
            bool algunCambioReal =
                HayCambio(familia) ||
                HayCambio(concepto) ||
                HayCambio(cantidad) ||
                HayCambio(igv) ||
                HayCambio(detUnif) ||
                HayCambio(descuentoActivo) ||
                HayCambio(cliente) ||
                HayCambio(tipoEntrega);

            if (!algunCambioReal)
            {
                Console.WriteLine("[ActualizarPedido] Ningún campo fue modificado (todo NA/sin_cambio).");
                ultimaAccion = "editar_sin_cambio";
                return;
            }

            // A partir de acá, solo tocar los campos que realmente tienen valor
            if (HayCambio(familia))
                SeleccionarFamilia(familia);

            if (HayCambio(concepto))
                SeleccionarConcepto(concepto);

            if (HayCambio(cantidad))
                IngresarCantidad(cantidad);

            if (HayCambio(igv))
                ActivarIGV(igv);

            if (HayCambio(detUnif))
                ActivarDetUnif(detUnif);

            if (HayCambio(descuentoActivo))
                ConfigurarDescuento(descuentoActivo, tipoDescuento, modoDescuento, valorDescuento);

            if (HayCambio(cliente))
            {
                AbrirSeccion("Facturación");
                BuscarCliente(cliente);
            }

            if (HayCambio(tipoEntrega))
            {
                AbrirSeccion("Entrega");
                SeleccionarEntrega(tipoEntrega);
            }
        }

        public void AsegurarPedidoRegistradoParaEditar()
        {
            FiltrarPedidosRegistrados();
            if (ExistePedidoRegistradoParaEditar())
                return;

            FiltrarPedidosRegistrados();
            if (!ExistePedidoRegistradoParaEditar())
                Assert.Fail("No se pudo generar un pedido en estado REGISTRADO para editar.");
        }

        private bool ExistePedidoRegistradoParaEditar()
        {
            try
            {
                var botonesEditar = driver.FindElements(btnEditarPrimerRegistro);
                return botonesEditar.Count > 0 && botonesEditar[0].Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void SeleccionarEditarPedido()
        {
            try
            {
                FiltrarPedidosRegistrados();

                var botonEditar = wait.Until(
                    ExpectedConditions.ElementExists(btnEditarPrimerRegistro)
                );

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", botonEditar);

                Thread.Sleep(300);

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", botonEditar);
            }
            catch
            {
                Assert.Fail("No se encontró ningún pedido con estado REGISTRADO para editar.");
            }
        }

        public void GuardarEdicionPedido()
        {
            ultimaAccion = "editar";

            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            // Dar tiempo a Angular para procesar el cambio de cantidad
            Thread.Sleep(2500);

            IWebElement boton = waitLong.Until(d =>
            {
                try
                {
                    var el = d.FindElement(btnEditarPedidoFinal);
                    return el.Displayed ? el : null;
                }
                catch { return null; }
            });

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);
            bool deshabilitado = !boton.Enabled;

            // Doble verificación con pausa para confirmar estabilidad
            Thread.Sleep(500);
            deshabilitado = !boton.Enabled;

            if (deshabilitado)
            {
                ultimaAccion = "editar_deshabilitado";
                Console.WriteLine("[GuardarEdicion] Botón deshabilitado confirmado.");
                return;
            }

            Console.WriteLine("[GuardarEdicion] Botón habilitado, procediendo a guardar.");

            try
            {
                boton.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", boton);
            }

            // Esperar respuesta del sistema
            try
            {
                waitLong.Until(d =>
                {
                    try
                    {
                        var ok = d.FindElement(btnOKConfirmacion);
                        if (ok.Displayed) return true;
                    }
                    catch { }

                    try
                    {
                        d.FindElement(btnEditarPedidoFinal);
                        return false;
                    }
                    catch (NoSuchElementException)
                    {
                        return true;
                    }
                });
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("[GuardarEdicion] Timeout esperando respuesta post-guardado.");
            }
        }

        // INVALIDAR PEDIDO

        public void SeleccionarInvalidarPedido()
        {
            try
            {
                FiltrarPedidosRegistrados();

                var botonInvalidar = wait.Until(
                    ExpectedConditions.ElementExists(btnInvalidarPrimerRegistro)
                );

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", botonInvalidar);

                Thread.Sleep(300);

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", botonInvalidar);
            }
            catch
            {
                Assert.Fail("No se encontró ningún pedido con estado REGISTRADO para invalidar.");
            }
        }

        public void IngresarMotivoInvalidacion(string motivo)
        {
            if (motivo.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                return;

            var input = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(txtMotivoInvalidacion)
            );

            input.Clear();
            input.SendKeys(motivo);

            wait.Until(d =>
            {
                try
                {
                    var valor = input.GetAttribute("value") ?? "";
                    return valor.Trim().Equals(motivo.Trim(), StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            });
        }



        public void ConfirmarInvalidacion(string accion)
        {
            if (accion.Trim().Equals("SI", StringComparison.OrdinalIgnoreCase) ||
                accion.Trim().Equals("Sí", StringComparison.OrdinalIgnoreCase) ||
                accion.Trim().Equals("Si", StringComparison.OrdinalIgnoreCase))
            {
                var botonSi = wait.Until(
                    ExpectedConditions.ElementIsVisible(btnSiInvalidar)
                );

                bool deshabilitado =
                    !botonSi.Enabled ||
                    botonSi.GetAttribute("disabled") != null ||
                    (botonSi.GetAttribute("class") ?? "").ToLower().Contains("disabled");

                if (deshabilitado)
                {
                    Console.WriteLine("El botón SI está deshabilitado, no se hace click.");
                    ultimaAccion = "invalidar_deshabilitado";
                    return;
                }

                var inputMotivo = wait.Until(
                    ExpectedConditions.ElementIsVisible(txtMotivoInvalidacion)
                );

                wait.Until(d =>
                {
                    try
                    {
                        var valor = inputMotivo.GetAttribute("value") ?? "";
                        return valor.Trim().Length > 0;
                    }
                    catch
                    {
                        return false;
                    }
                });

                ultimaAccion = "invalidar";

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonSi);
                return;
            }

            if (accion.Trim().Equals("NO", StringComparison.OrdinalIgnoreCase))
            {
                var botonNo = wait.Until(
                    ExpectedConditions.ElementToBeClickable(btnNoInvalidar)
                );

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botonNo);
            }
        }

        public bool ExistePedidoBaseParaConfirmar(bool esMayor700)
        {
            try
            {
                FiltrarPedidoBaseParaConfirmar(esMayor700);

                var botonesConfirmar = driver.FindElements(btnConfirmarPrimerRegistro);
                return botonesConfirmar.Count > 0 && botonesConfirmar[0].Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void FiltrarPedidoBaseParaConfirmar(bool esMayor700)
        {
            var filtroEstado = wait.Until(
                ExpectedConditions.ElementIsVisible(txtFiltroEstado)
            );
            filtroEstado.Clear();
            filtroEstado.SendKeys("REGISTRADO");

            Thread.Sleep(500);

            var filtroTotal = wait.Until(
                ExpectedConditions.ElementIsVisible(txtFiltroTotal)
            );
            filtroTotal.Clear();
            filtroTotal.SendKeys(esMayor700 ? TOTAL_BASE_MAYOR_700 : TOTAL_BASE_MENOR_IGUAL_700);

            Thread.Sleep(1500);
        }

        //-----------------------
        //private void SeleccionarComprobanteConfirmacion(WebDriverWait waitLong, string tipoComprobante)
        //{
        //    // 1. Esperar el combo y abrirlo (hasta 3 intentos)
        //    var combo = waitLong.Until(
        //        ExpectedConditions.ElementToBeClickable(cmbTipoComprobanteConfirmacion)
        //    );

        //    bool panelAbierto = false;
        //    for (int i = 0; i < 3 && !panelAbierto; i++)
        //    {
        //        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", combo);
        //        Thread.Sleep(700 + i * 300);

        //        try
        //        {
        //            var panel = driver.FindElement(panelDropdownNgSelect);
        //            panelAbierto = panel.Displayed;
        //        }
        //        catch { }
        //    }

        //    if (!panelAbierto)
        //    {
        //        Console.WriteLine($"[Comprobante] Panel no se abrió para '{tipoComprobante}', se mantiene valor actual.");
        //        return;
        //    }

        //    // 2. Esperar que las ng-option estén renderizadas (listas para clickear)
        //    try
        //    {
        //        waitLong.Until(d =>
        //        {
        //            try
        //            {
        //                var opts = d.FindElements(
        //                    By.XPath("//div[contains(@class,'ng-dropdown-panel')]//div[contains(@class,'ng-option')]")
        //                );
        //                return opts.Count > 0 && opts[0].Displayed;
        //            }
        //            catch { return false; }
        //        });
        //    }
        //    catch (WebDriverTimeoutException)
        //    {
        //        Console.WriteLine("[Comprobante] Timeout esperando ng-option.");
        //        return;
        //    }

        //    // 3. Clickear la opción correcta
        //    try
        //    {
        //        var opcion = waitLong.Until(
        //            ExpectedConditions.ElementToBeClickable(OpcionComprobante(tipoComprobante))
        //        );
        //        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opcion);
        //        Console.WriteLine($"[Comprobante] '{tipoComprobante}' seleccionado.");
        //    }
        //    catch (WebDriverTimeoutException)
        //    {
        //        Console.WriteLine($"[Comprobante] No se encontró opción para '{tipoComprobante}'.");
        //        return;
        //    }

        //    // 4. Esperar que el panel se cierre
        //    try
        //    {
        //        waitLong.Until(d =>
        //        {
        //            try { return !d.FindElement(panelDropdownNgSelect).Displayed; }
        //            catch { return true; }
        //        });
        //    }
        //    catch { }

        //    Thread.Sleep(300);
        //}

        private bool SeleccionarComprobanteConfirmacion(WebDriverWait waitLong, string tipoComprobante)
        {
            Console.WriteLine($"[Comprobante] Intentando seleccionar: '{tipoComprobante}'");

            var combo = waitLong.Until(d =>
            {
                try
                {
                    var combos = d.FindElements(cmbTipoComprobanteConfirmacion)
                        .Where(e => e.Displayed && e.Enabled)
                        .ToList();

                    Console.WriteLine($"[Comprobante] Combos visibles encontrados: {combos.Count}");

                    if (!combos.Any()) return null;

                    return combos.FirstOrDefault(c =>
                    {
                        var txt = (c.Text ?? "").Trim().ToUpperInvariant();
                        return txt.Contains("BOLETA") || txt.Contains("FACTURA") || txt.Contains("NOTA DE VENTA");
                    }) ?? combos.Last();
                }
                catch
                {
                    return null;
                }
            });

            if (combo == null)
            {
                Console.WriteLine("[Comprobante] No se encontró el combo de comprobante.");
                return false;
            }

    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", combo);
            Thread.Sleep(300);

            try
            {
                combo.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", combo);
            }

            // Esperar que aparezca el buscador del dropdown
            var inputBuscar = waitLong.Until(d =>
            {
                try
                {
                    var inputs = d.FindElements(By.XPath(
                        "//input[@placeholder='Buscar...' or contains(@placeholder,'Buscar')]"
                    ))
                    .Where(e => e.Displayed && e.Enabled)
                    .ToList();

                    return inputs.FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            });

            if (inputBuscar == null)
            {
                Console.WriteLine("[Comprobante] No apareció el buscador del dropdown.");
                return false;
            }

            string textoBusqueda = ObtenerTextoBusquedaComprobante(tipoComprobante);

            inputBuscar.Click();
            inputBuscar.SendKeys(Keys.Control + "a");
            inputBuscar.SendKeys(Keys.Delete);
            Thread.Sleep(200);
            inputBuscar.SendKeys(textoBusqueda);
            Thread.Sleep(800);

            var opcion = waitLong.Until(d =>
            {
                try
                {
                    var opciones = d.FindElements(By.XPath(
                        "//div[contains(@class,'ng-option')]" +
                        " | //li" +
                        " | //div[@role='option']" +
                        " | //span"
                    ))
                    .Where(e => e.Displayed)
                    .ToList();

                    return opciones.FirstOrDefault(e =>
                    {
                        var txt = (e.Text ?? "").Trim().ToUpperInvariant();
                        return CoincideComprobante(txt, tipoComprobante);
                    });
                }
                catch
                {
                    return null;
                }
            });

            if (opcion == null)
            {
                Console.WriteLine($"[Comprobante] No se encontró opción visible para '{tipoComprobante}'.");
                return false;
            }

            Console.WriteLine($"[Comprobante] Opción encontrada: '{opcion.Text}'");

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", opcion);
            Thread.Sleep(200);

            try
            {
                opcion.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opcion);
            }

            Thread.Sleep(1000);

            return ComprobanteSeleccionadoCoincide(tipoComprobante);
        }

        private void SeleccionarSerieConfirmacion(WebDriverWait waitLong, string serie)
        {
            // Las series son radio buttons con label que contiene el código (B002, F002, NV02, etc.)
            // Solo aparecen si el comprobante tiene múltiples series configuradas.
            // Si no aparece ninguna, se continúa sin error (serie única o no aplica).
            try
            {
                var radioSerie = waitLong.Until(d =>
                {
                    try
                    {
                        // Buscar label de radio button cuyo texto sea exactamente la serie
                        var candidatos = d.FindElements(By.XPath(
                            $"//label[normalize-space(.)='{serie}']"
                        ));

                        // Fallback: buscar input radio con value o id que contenga la serie
                        if (!candidatos.Any(e => e.Displayed))
                        {
                            candidatos = d.FindElements(By.XPath(
                                $"//input[@type='radio'][@value='{serie}' or contains(@id,'{serie}')]"
                            ));
                        }

                        return candidatos.FirstOrDefault(e => e.Displayed && e.Enabled);
                    }
                    catch { return null; }
                });

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", radioSerie);
                Thread.Sleep(200);
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", radioSerie);

                Console.WriteLine($"[Serie] '{serie}' seleccionada.");
                Thread.Sleep(300);
            }
            catch (WebDriverTimeoutException)
            {
                // No es error: puede que ese comprobante tenga una sola serie (ya preseleccionada)
                Console.WriteLine($"[Serie] '{serie}' no encontrada como radio button; se asume preseleccionada.");
            }
        }

        // Abrir subsecciones de la confirmación del pedido para configurar opciones antes de confirmar
        private void AbrirFacturacionConfirmacion()
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            // 1) Localizar el header de "Facturación" dentro del modal de confirmación
            var header = waitLong.Until(d =>
            {
                try
                {
                    var h = d.FindElement(seccionFacturacionConfirmacion);
                    return h.Displayed ? h : null;
                }
                catch
                {
                    return null;
                }
            });

            // 2) Scroll al header
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", header);

            // 3) Si hay un botón/chevron dentro del header, clickeamos ese; si no, el propio header
            IWebElement clickable;
            try
            {
                clickable = header.FindElement(By.XPath(".//button | .//*[@role='button']"));
            }
            catch
            {
                clickable = header;
            }

            waitLong.Until(d => clickable.Displayed && clickable.Enabled);
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", clickable);

            // 4) Esperar a que el cuerpo del acordeón de Facturación esté visible (cliente dentro)
            waitLong.Until(d =>
            {
                try
                {
                    var body = d.FindElement(By.XPath(
                        "//div[contains(@class,'accordion-body')]" +
                        "[.//label[contains(normalize-space(),'Cliente')]]"
                    ));
                    return body.Displayed;
                }
                catch
                {
                    return false;
                }
            });
        }

        private void AbrirEntregaConfirmacion()
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(25));

            // Si los radios ya están visibles, no hacer nada
            bool yaVisible = driver.FindElements(By.XPath(
                "//label[normalize-space()='Inmediata' or normalize-space()='Diferida']"
            )).Any(e => e.Displayed);

            if (yaVisible)
            {
                Console.WriteLine("[AbrirEntregaConfirmacion] Ya está visible, no se hace click.");
                return;
            }

            // Buscar el header de Entrega y hacer click
            var header = waitLong.Until(d =>
            {
                try
                {
                    var candidatos = d.FindElements(By.XPath(
                        "//*[.//span[normalize-space()='Entrega'] or normalize-space()='Entrega']" +
                        "[self::div or self::button or self::h2 or self::h3]"
                    ));
                    return candidatos.FirstOrDefault(e => e.Displayed && e.Enabled);
                }
                catch { return null; }
            });

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", header);
            Thread.Sleep(300);
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", header);

            // Esperar radios visibles
            waitLong.Until(d =>
                d.FindElements(By.XPath(
                    "//label[normalize-space()='Inmediata' or normalize-space()='Diferida']"
                )).Any(e => e.Displayed)
            );
        }

        public void ConfigurarEntregaConfirmacion(string tipoEntrega, string guiaRemision)
        {
            try
            {
                var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(25));

                AbrirEntregaConfirmacion();

                if (tipoEntrega.Trim().Equals("inmediata", StringComparison.OrdinalIgnoreCase))
                {
                    var radio = waitLong.Until(d =>
                        d.FindElements(By.XPath("//label[normalize-space()='Inmediata']"))
                         .FirstOrDefault(e => e.Displayed)
                    );
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radio);
                }

                if (tipoEntrega.Trim().Equals("diferida", StringComparison.OrdinalIgnoreCase))
                {
                    var radio = waitLong.Until(d =>
                        d.FindElements(By.XPath("//label[normalize-space()='Diferida']"))
                         .FirstOrDefault(e => e.Displayed)
                    );
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radio);
                }

                if (guiaRemision.Trim().Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    var btnGuia = waitLong.Until(
                        ExpectedConditions.ElementToBeClickable(btnGuiaRemisionConfirmacion)
                    );
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuia);
                }

                Thread.Sleep(500);
            }
            catch (Exception e)
            {
                Assert.Fail("Error configurando entrega de confirmación: " + e.Message);
            }
        }

        
        private void AbrirPagoConfirmacion()
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            var elemento = waitLong.Until(
                ExpectedConditions.ElementToBeClickable(seccionPagoConfirmacion)
            );

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", elemento);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", elemento);

            // Esperar contenido de Pago visible
            waitLong.Until(d =>
            {
                try
                {
                    var el = d.FindElement(rbtContadoConfirmacion);
                    return el.Displayed;
                }
                catch
                {
                    return false;
                }
            });
        }

        public void SeleccionarConfirmarPedido()
        {
            try
            {
                var botonConfirmar = wait.Until(
                    ExpectedConditions.ElementToBeClickable(btnConfirmarPrimerRegistro)
                );

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", botonConfirmar);

                Thread.Sleep(300);

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", botonConfirmar);
            }
            catch
            {
                Assert.Fail("No se encontró ningún pedido con estado REGISTRADO para confirmar.");
            }
        }

        private void BuscarClienteConfirmacion(string cliente)
        {
            try
            {
                if (cliente == "00000000" || cliente.ToLower() == "varios")
                {
                    Console.WriteLine("Cliente VARIOS - no se realiza búsqueda en confirmación");
                    return;
                }

                var input = wait.Until(
                    ExpectedConditions.ElementIsVisible(txtClienteConfirmacion)
                );

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);

                Thread.Sleep(300);

                input.Clear();
                input.SendKeys(cliente);
                input.SendKeys(Keys.Enter);

                Thread.Sleep(1500);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error buscando cliente en confirmación: " + e.Message);
                throw;
            }
        }
        // Métodos para configurar opciones en la confirmación del pedido
        public void ConfigurarFacturacionConfirmacion(string tipoComprobante, string serie, string cliente)
        {
            try
            {
                var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                // Abrir sección Facturación
                AbrirFacturacionConfirmacion();

                // Esperar que el formulario cargue
                waitLong.Until(ExpectedConditions.ElementIsVisible(txtClienteConfirmacion));

                // Buscar cliente
                BuscarClienteConfirmacion(cliente);


                bool comprobanteOk = SeleccionarComprobanteConfirmacion(waitLong, tipoComprobante);

                if (!comprobanteOk)
                {
                    string actual = ObtenerTextoComprobanteSeleccionado();
                    Assert.Fail(
                        $"No se pudo seleccionar el comprobante esperado '{tipoComprobante}'. Valor actual visible: '{actual}'."
                    );
                }

                Thread.Sleep(800);

                if (!serie.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                {
                    SeleccionarSerieConfirmacion(waitLong, serie);
                }

                // Cerrar sección facturación
                AbrirFacturacionConfirmacion();
            }
            catch (AssertionException)
            {
                throw;
            }
            catch (Exception e)
            {
                Assert.Fail("Error configurando facturación de confirmación: " + e.Message);
            }
        }
        public void ConfigurarPagoConfirmacion(string tipoPago, string montoCubreTotal)
        {
            try
            {
                AbrirPagoConfirmacion();
                SeleccionarTipoPagoConfirmacion("contado");
                SeleccionarTabMedioPagoConfirmacion("efectivo");
                IngresarMontoEfectivoConfirmacion(montoCubreTotal);
            }
            catch (Exception e)
            {
                Assert.Fail("Error configurando pago en efectivo de confirmación: " + e.Message);
            }
        }

        //CONFIRMAR PEDIDO - MEDIOS DE PAGO
        public void ConfigurarMediosDePagoConfirmacion(
        string tipoPago,
        string multipago,
        string medioPago,
        string banco,
        string tarjeta,
        string cuentaBancaria,
        string nroOperacion,
        string monto,
        string nroCuotas)
        {
            try
            {
                AbrirPagoConfirmacion();

                SeleccionarTipoPagoConfirmacion(tipoPago);

                bool esMultipago = multipago.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
                ConfigurarMultipagoConfirmacion(esMultipago);

                if (tipoPago.Trim().Equals("credito", StringComparison.OrdinalIgnoreCase))
                {
                    IngresarNumeroCuotasConfirmacion(nroCuotas);

                    if (!EsNA(monto))
                        IngresarMontoInicialCreditoConfirmacion(monto);
                }

                var medios = SepararValores(medioPago);
                var bancos = new Queue<string>(SepararValoresFiltrados(banco));
                var tarjetas = new Queue<string>(SepararValoresFiltrados(tarjeta));
                var cuentas = new Queue<string>(SepararValoresFiltrados(cuentaBancaria));
                var operaciones = new Queue<string>(SepararValoresFiltrados(nroOperacion));

                for (int i = 0; i < medios.Count; i++)
                {
                    string medioActual = medios[i];

                    if (i > 0 && esMultipago)
                        AgregarMedioPagoConfirmacion();

                    SeleccionarTabMedioPagoConfirmacion(medioActual);

                    ConfigurarMedioPagoConfirmacion(
                        medioActual,
                        tipoPago,
                        monto,
                        bancos,
                        tarjetas,
                        cuentas,
                        operaciones
                    );
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Error configurando medios de pago en confirmación: " + e.Message);
            }
        }
        public void ConfirmarPedidoPreparado()
        {
            ultimaAccion = "confirmar";

            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            IWebElement boton = waitLong.Until(d =>
            {
                try
                {
                    var el = d.FindElement(btnConfirmarPedidoFinal);
                    return el.Displayed ? el : null;
                }
                catch
                {
                    return null;
                }
            });

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            Thread.Sleep(500);

            bool deshabilitado =
                !boton.Enabled ||
                boton.GetAttribute("disabled") != null ||
                (boton.GetAttribute("class") ?? "").ToLower().Contains("disabled");

            if (deshabilitado)
            {
                Console.WriteLine("[ConfirmarPedidoPreparado] El botón Confirmar Pedido está deshabilitado.");
                return;
            }

            try
            {
                boton.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", boton);
            }

            Thread.Sleep(800);
        }

        // HLEP
        private void SeleccionarTipoPagoConfirmacion(string tipoPago)
        {
            if (tipoPago.Trim().Equals("contado", StringComparison.OrdinalIgnoreCase))
            {
                var radio = wait.Until(ExpectedConditions.ElementToBeClickable(rbtContadoConfirmacion));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radio);
                return;
            }

            if (tipoPago.Trim().Equals("credito", StringComparison.OrdinalIgnoreCase))
            {
                //var radio = wait.Until(ExpectedConditions.ElementToBeClickable(rbtCredito));
                var radio = wait.Until(ExpectedConditions.ElementToBeClickable(rbtCreditoConfirmacion));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", radio);
            }
        }

        private void ConfigurarMultipagoConfirmacion(bool activar)
        {
            var chk = wait.Until(ExpectedConditions.ElementExists(chkMultipagoConfirmacion));
            bool marcado = chk.Selected;

            if (marcado != activar)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", chk);
                Thread.Sleep(500);
            }
        }

        private void IngresarNumeroCuotasConfirmacion(string nroCuotas)
        {
            if (EsNA(nroCuotas)) return;

            var input = wait.Until(ExpectedConditions.ElementIsVisible(txtNumeroCuotasConfirmacion));
            LimpiarYEscribirCampo(input, nroCuotas.Trim());
        }

        private void IngresarMontoInicialCreditoConfirmacion(string monto)
        {
            var inputs = driver.FindElements(txtMontoInicialCreditoConfirmacion)
                .Where(e => e.Displayed && e.Enabled)
                .ToList();

            if (!inputs.Any()) return;

            var input = inputs.Last();
            LimpiarYEscribirCampo(input, ResolverMontoPago(monto));
        }

        private void AgregarMedioPagoConfirmacion()
        {
            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnAgregarMedioPagoConfirmacion));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);
            Thread.Sleep(300);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton);
            Thread.Sleep(700);
        }

        private void SeleccionarTabMedioPagoConfirmacion(string medioPago)
        {
            string medio = medioPago.Trim().ToLower();

            if (medio == "efectivo")
            {
                ClickSeguroConfirmacion(tabEfectivoConfirmacion);
                Thread.Sleep(500);
                wait.Until(d => d.FindElements(txtRecibidoEfectivo).Any(e => e.Displayed && e.Enabled));
                return;
            }

            if (medio == "tarjeta_credito")
            {
                ClickSeguroConfirmacion(tabTarjetaCreditoConfirmacion);
                Thread.Sleep(700);
                wait.Until(d => ObtenerSelectVisiblePorIndiceConfirmacion(cmbBancoConfirmacion, 0) != null);
                return;
            }

            if (medio == "tarjeta_debito")
            {
                ClickSeguroConfirmacion(tabTarjetaDebitoConfirmacion);
                Thread.Sleep(700);
                wait.Until(d => ObtenerSelectVisiblePorIndiceConfirmacion(cmbBancoConfirmacion, 0) != null);
                return;
            }

            if (medio == "transferencia_fondos")
            {
                ClickSeguroConfirmacion(tabTransferenciaConfirmacion);
                Thread.Sleep(700);
                wait.Until(d => ObtenerSelectVisiblePorIndiceConfirmacion(cmbCuentaBancariaConfirmacion, 0) != null);
                return;
            }

            if (medio == "deposito_cuenta")
            {
                ClickSeguroConfirmacion(tabDepositosConfirmacion);
                Thread.Sleep(700);
                wait.Until(d => ObtenerSelectVisiblePorIndiceConfirmacion(cmbCuentaBancariaConfirmacion, 0) != null);
                return;
            }

            if (medio == "puntos")
            {
                ClickSeguroConfirmacion(tabPuntosConfirmacion);
                Thread.Sleep(500);
                return;
            }

            throw new Exception($"Medio de pago no soportado: {medioPago}");
        }

        private void ConfigurarMedioPagoConfirmacion(
            string medioPago,
            string tipoPago,
            string monto,
            Queue<string> bancos,
            Queue<string> tarjetas,
            Queue<string> cuentas,
            Queue<string> operaciones)
        {
            string medio = medioPago.Trim().ToLower();

            switch (medio)
            {
                case "efectivo":
                    if (tipoPago.Trim().Equals("contado", StringComparison.OrdinalIgnoreCase))
                    {
                        // Reforzar selección del tab aunque ya venga por defecto
                        ClickSeguroConfirmacion(tabEfectivoConfirmacion);
                        Thread.Sleep(500);

                        IngresarMontoEfectivoConfirmacion(monto);
                    }
                    break;

                case "tarjeta_credito":
                case "tarjeta_debito":
                    SeleccionarBancoConfirmacion(ConsumirSiguientePago(bancos));
                    Thread.Sleep(500);
                    SeleccionarTarjetaConfirmacion(ConsumirSiguientePago(tarjetas));
                    Thread.Sleep(300);
                    IngresarInformacionConfirmacion(ConsumirSiguientePago(operaciones));
                    break;

                case "transferencia_fondos":
                case "deposito_cuenta":
                    SeleccionarCuentaBancariaConfirmacion(ConsumirSiguientePago(cuentas));
                    Thread.Sleep(300);
                    IngresarInformacionConfirmacion(ConsumirSiguientePago(operaciones));
                    break;

                case "puntos":
                    break;
            }
        }

        private void IngresarMontoEfectivoConfirmacion(string monto)
        {
            if (EsNA(monto)) return;

            string valor = ResolverMontoPago(monto);
            if (string.IsNullOrWhiteSpace(valor)) return;

            var input = wait.Until(d =>
            {
                try
                {
                    var elementos = d.FindElements(txtRecibidoEfectivo);
                    return elementos.FirstOrDefault(e => e.Displayed && e.Enabled);
                }
                catch
                {
                    return null;
                }
            });

            if (input == null)
                throw new Exception("No se encontró el input de monto recibido en efectivo.");

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);

            Thread.Sleep(400);

            // Forzar foco
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", input);

            // Limpiar valor y disparar evento input
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
            arguments[0].value = '';
            arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
            arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
            ", input);

            Thread.Sleep(200);

            // Escribir valor
            input.SendKeys(valor);

            Thread.Sleep(200);

            // Disparar eventos después de escribir
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
            arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
            arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
            arguments[0].blur();
        ", input);

            input.SendKeys(Keys.Tab);

            Thread.Sleep(800);
        }

        private void SeleccionarBancoConfirmacion(string banco)
        {
            if (EsNA(banco) || banco.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                return;

            var select = ObtenerSelectVisiblePorIndiceConfirmacion(cmbBancoConfirmacion, 0);
            if (select == null)
                throw new Exception("No se encontró un combo visible de banco.");

            SeleccionarOpcionSelectConfirmacion(select, banco.Trim());
        }

        private void SeleccionarTarjetaConfirmacion(string tarjeta)
        {
            if (EsNA(tarjeta) || tarjeta.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                return;

            var select = ObtenerSelectVisiblePorIndiceConfirmacion(cmbTarjetaConfirmacion, 0);
            if (select == null)
                throw new Exception("No se encontró un combo visible de tarjeta.");

            SeleccionarOpcionSelectConfirmacion(select, tarjeta.Trim());
        }

        private void SeleccionarCuentaBancariaConfirmacion(string cuentaBancaria)
        {
            if (EsNA(cuentaBancaria) || cuentaBancaria.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                return;

            var select = ObtenerSelectVisiblePorIndiceConfirmacion(cmbCuentaBancariaConfirmacion, 0);
            if (select == null)
                throw new Exception("No se encontró un combo visible de cuenta bancaria.");

            var opciones = new SelectElement(select);
            var texto = cuentaBancaria.Trim();

            try
            {
                opciones.SelectByText(texto);
            }
            catch
            {
                var opcion = opciones.Options.FirstOrDefault(x =>
                    x.Text.Trim().IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0);

                if (opcion != null)
                    opcion.Click();
                else
                    throw;
            }
        }

        private void IngresarInformacionConfirmacion(string informacion)
        {
            if (EsNA(informacion) || informacion.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                return;

            var input = ObtenerInputVisiblePorIndiceConfirmacion(txtInformacionConfirmacion, 0);
            if (input == null)
                throw new Exception("No se encontró el input visible de información.");

            LimpiarYEscribirCampo(input, informacion.Trim());
        }

        private IWebElement ObtenerSelectVisiblePorIndiceConfirmacion(By locator, int indiceVisible)
        {
            return wait.Until(d =>
            {
                try
                {
                    var visibles = d.FindElements(locator)
                        .Where(e => e.Displayed && e.Enabled)
                        .ToList();

                    if (visibles.Count > indiceVisible)
                        return visibles[indiceVisible];

                    return null;
                }
                catch
                {
                    return null;
                }
            });
        }

        private IWebElement ObtenerInputVisiblePorIndiceConfirmacion(By locator, int indiceVisible = 0)
        {
            return wait.Until(d =>
            {
                try
                {
                    var visibles = d.FindElements(locator)
                        .Where(e => e.Displayed && e.Enabled)
                        .ToList();

                    if (visibles.Count > indiceVisible)
                        return visibles[indiceVisible];

                    return null;
                }
                catch
                {
                    return null;
                }
            });
        }

        private void SeleccionarOpcionSelectConfirmacion(IWebElement selectElement, string texto)
        {
            var combo = new SelectElement(selectElement);

            var opcion = combo.Options.FirstOrDefault(o =>
                o.Text.Trim().Equals(texto.Trim(), StringComparison.OrdinalIgnoreCase));

            if (opcion == null)
                throw new Exception($"No se encontró la opción '{texto}' en el combo.");

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = arguments[1];", selectElement, opcion.GetAttribute("value"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].dispatchEvent(new Event('change', { bubbles: true }));", selectElement);

            Thread.Sleep(400);
        }

        private void ClickSeguroConfirmacion(By locator)
        {
            var elemento = wait.Until(ExpectedConditions.ElementToBeClickable(locator));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", elemento);
            Thread.Sleep(300);

            try
            {
                elemento.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elemento);
            }
        }

        private void LimpiarYEscribirCampo(IWebElement input, string valor)
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

        private string ResolverMontoPago(string monto)
        {
            if (string.IsNullOrWhiteSpace(monto) || monto.Trim().Equals("NA", StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            if (monto.Trim().Equals("true", StringComparison.OrdinalIgnoreCase))
                return "1000";

            if (monto.Trim().Equals("false", StringComparison.OrdinalIgnoreCase))
                return "1";

            return monto.Trim();
        }

        private bool EsNA(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ||
                   valor.Trim().Equals("NA", StringComparison.OrdinalIgnoreCase);
        }

        private List<string> SepararValores(string valor)
        {
            if (EsNA(valor)) return new List<string>();

            return valor
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        private List<string> SepararValoresFiltrados(string valor)
        {
            if (EsNA(valor)) return new List<string>();

            return valor
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x) &&
                            !x.Equals("NA", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private string ConsumirSiguientePago(Queue<string> cola)
        {
            if (cola == null || cola.Count == 0)
                return "NA";

            return cola.Dequeue();
        }

        //--------------------

        public void ConfirmarMensaje()
        {
            var boton = wait.Until(
                ExpectedConditions.ElementToBeClickable(btnOKConfirmacion)
            );

            boton.Click();
        }

        public string ObtenerMensajeError()
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(mensajeError)).Text;
        }
        public string ObtenerResultadoSistema()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // Registro: sin producto
                try
                {
                    var mensajeProducto = driver.FindElement(
                        By.XPath("//*[contains(text(),'Ningún producto seleccionado')]")
                    );
                    if (mensajeProducto.Displayed)
                        return "Ningún producto seleccionado";
                }
                catch { }

                // Registro: inconsistencias generales (stock, cantidad, validaciones)
                try
                {
                    var inconsistencia = driver.FindElement(By.XPath(
                        "//*[contains(text(),'Se encontraron inconsistencias')] | " +
                        "//*[contains(text(),'supera el stock disponible')] | " +
                        "//*[contains(text(),'menor o igual al stock')] | " +
                        "//*[contains(text(),'cantidad')]"
                    ));
                    if (inconsistencia.Displayed)
                        return "muestra mensaje de inconsistencia";
                }
                catch { }

                // ── EDITAR ──────────────────────────────────────────────
                if (ultimaAccion == "editar_deshabilitado" || ultimaAccion == "editar_sin_cambio")
                {
                    ultimaAccion = "";
                    return "Boton deshabilitado";
                }

                try
                {
                    var mensajeSinCambio = driver.FindElement(By.XPath(
                        "//*[contains(text(),'Debe realizar alguna modificacion') " +
                        "or contains(text(),'Debe realizar alguna modificación')]"
                    ));
                    if (mensajeSinCambio.Displayed)
                        return "Boton deshabilitado";
                }
                catch { }

                try
                {
                    var botonOK = new WebDriverWait(driver, TimeSpan.FromSeconds(8))
                        .Until(ExpectedConditions.ElementIsVisible(btnOKConfirmacion));

                    if (botonOK.Displayed && ultimaAccion == "editar")
                    {
                        ultimaAccion = "";
                        botonOK.Click();
                        return "el pedido se edito correctamente";
                    }
                }
                catch { }

                if (ultimaAccion == "editar")
                {
                    try
                    {
                        driver.FindElement(btnEditarPedidoFinal);
                    }
                    catch (NoSuchElementException)
                    {
                        ultimaAccion = "";
                        return "el pedido se edito correctamente";
                    }
                }

                // ── INVALIDAR ───────────────────────────────────────────
                try
                {
                    if (ultimaAccion == "invalidar_deshabilitado")
                    {
                        ultimaAccion = "";
                        return "Boton SI deshabilitado";
                    }

                    var botonSi = driver.FindElement(btnSiInvalidar);
                    bool deshabilitado =
                        !botonSi.Enabled ||
                        botonSi.GetAttribute("disabled") != null ||
                        botonSi.GetAttribute("class")?.ToLower().Contains("disabled") == true;

                    if (deshabilitado)
                        return "Boton SI deshabilitado";
                }
                catch { }

                // ── CONFIRMAR PEDIDO: VALIDACIONES DE NEGOCIO ──────────
                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'RUC (11 dígitos)')]"));
                    if (mensaje.Displayed)
                        return "Para emitir Factura Electrónica, el cliente debe tener RUC (11 dígitos)";
                }
                catch { }

                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'numero de serie') or contains(text(),'número de serie')]"));
                    if (mensaje.Displayed)
                        return "Ingrese el numero de serie";
                }
                catch { }

                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'Monto insuficiente')]"));
                    if (mensaje.Displayed)
                        return "Monto insuficiente";
                }
                catch { }

                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'mayor a S/.700')]"));
                    if (mensaje.Displayed)
                        return "Es necesario identificar al cliente, el total es mayor a S/.700";
                }
                catch { }

                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'Necesita identificar al cliente con RUC o DNI')]"));
                    if (mensaje.Displayed)
                        return "Para guia de remision Necesita identificar al cliente con RUC o DNI";
                }
                catch { }

                // ── ÉXITO DE CONFIRMACIÓN: PRIORIDAD ALTA ──────────────
                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'Pedido confirmado correctamente')]"));
                    if (mensaje.Displayed)
                        return "Pedido confirmado correctamente";
                }
                catch { }

                // Popup genérico OK: decidir según la última acción ejecutada
                try
                {
                    var botonOK = wait.Until(
                        ExpectedConditions.ElementIsVisible(btnOKConfirmacion)
                    );

                    if (botonOK.Displayed)
                    {
                        if (ultimaAccion == "invalidar")
                        {
                            ultimaAccion = "";
                            return "el pedido se Invalido correctamente";
                        }

                        if (ultimaAccion == "confirmar")
                        {
                            ultimaAccion = "";
                            return "Pedido confirmado correctamente";
                        }

                        if (ultimaAccion == "registrar")
                        {
                            ultimaAccion = "";
                            return "el pedido se guardo correctamente";
                        }
                    }
                }
                catch { }
                //-----------------------------------------
                try
                {
                    var boton = driver.FindElement(btnConfirmarPedidoFinal);
                    bool deshabilitado =
                        !boton.Enabled ||
                        boton.GetAttribute("disabled") != null ||
                        (boton.GetAttribute("class") ?? "").ToLower().Contains("disabled");

                    if (deshabilitado && ultimaAccion == "confirmar")
                        return "Boton Confirmar Pedido deshabilitado";
                }
                catch { }

                // ── MEDIOS DE PAGO: ERRORES ESPECÍFICOS ────────────────
                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'Seleccione una entidad bancaria')]"));
                    if (mensaje.Displayed)
                        return "Seleccione una entidad bancaria";
                }
                catch { }

                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'Seleccione una cuenta bancaria')]"));
                    if (mensaje.Displayed)
                        return "Seleccione una cuenta bancaria";
                }
                catch { }

                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'Puntos insuficiente') or contains(text(),'Puntos insuficientes')]"));
                    if (mensaje.Displayed)
                        return "Puntos insuficiente";
                }
                catch { }

                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'Para el pago con puntos debe identificar al cliente')]"));
                    if (mensaje.Displayed)
                        return "Para el pago con puntos debe identificar al cliente";
                }
                catch { }

                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'Para dar a credito debe identificar al cliente') or contains(text(),'Para dar a crédito debe identificar al cliente')]"));
                    if (mensaje.Displayed)
                        return "Para dar a credito debe identificar al cliente";
                }
                catch { }

                return "";
            }
            catch
            {
                return "";
            }
        }

    }
}