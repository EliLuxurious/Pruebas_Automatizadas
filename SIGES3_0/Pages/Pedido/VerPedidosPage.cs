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
        private By cmbFamilia = By.XPath("//span[normalize-space()='Seleccione una familia']");
        private By cmbConcepto = By.XPath("//span[normalize-space()='Seleccione un concepto']");
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
        // mensajes de advertencia
        private By mensajeInconsistenciaRegistro = By.XPath("//strong[normalize-space()='Se encontraron inconsistencias en los datos:']");
        private By detalleInconsistenciaRegistro = By.XPath("//div[contains(@class,'alert-danger')]//li");
        private By mensajeSinProductoRegistro = By.XPath("//span[@class='badge-status danger']");

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
        private By btnConfirmarPedidoFinal = By.XPath("//button[contains(normalize-space(.),'Confirmar Pedido') or .//*[contains(normalize-space(.),'Confirmar Pedido')]]");

        private By seccionFacturacionConfirmacion = By.XPath("//div[contains(@class,'d-flex') and contains(@class,'align-items-center') and contains(@class,'w-100')]" +"[.//span[normalize-space()='Facturación']]");
        private By seccionEntregaConfirmacion = By.XPath("//span[normalize-space()='Entrega']/ancestor::div[contains(@class,'d-flex align-items-center w-100')][1]");
        private By seccionPagoConfirmacion = By.XPath("//span[normalize-space()='Pago']/ancestor::div[contains(@class,'d-flex align-items-center w-100')][1]");

        private By txtClienteConfirmacion = By.CssSelector("input.search-input[placeholder='Buscar...']");

        private By cmbTipoComprobanteConfirmacion = By.XPath("//div[contains(@class,'select-trigger') and contains(@class,'form-control')]");
        private By panelDropdownNgSelect = By.CssSelector(".ng-dropdown-panel");

        // ENTREGA CONFIRMAR
        private By rbtEntregaInmediataConfirmacion = By.XPath("//label[normalize-space()='Inmediata']");
        private By rbtEntregaDiferidaConfirmacion = By.XPath("//labelS[normalize-space()='Diferida']");
        private By btnGuiaRemisionConfirmacion = By.XPath("//button[.//span[normalize-space()='Guia de remisión'] or normalize-space()='Guia de remisión']");
        private By btnCerrarEntregaConfirmacion = By.XPath("(//*[contains(@class,'ri-arrow-up-s-line') or contains(@class,'ri-arrow-down-s-line')])[2]");

        // PAGO CONFIRMAR
        private By rbtContadoConfirmacion = By.XPath("//label[normalize-space()='Al contado']");
        private By tabEfectivoConfirmacion = By.XPath("//*[contains(text(),'EFECTIVO')]");
        private By txtRecibidoEfectivoConfirmacion = By.XPath("//input[@id='amountReceived']");

        // CONFIRMAR PEDIDO - MEDIOS DE PAGOS
        // PAGO CONFIRMAR
        private By bodyPagoConfirmacion = By.XPath(
            "//div[contains(@class,'accordion-body')]" +
            "[.//label[normalize-space()='Contado' or normalize-space()='Crédito']]");

        private By tabEfectivoActivoConfirmacion = By.XPath(
            "//span[normalize-space()='EFECTIVO']/ancestor::*[" +
            "contains(@class,'active') or contains(@class,'selected') or @aria-selected='true']");

        private By inputMontoEfectivoConfirmacion = By.XPath("//input[@id='amountReceived']");
        //---------------


        private By chkMultipagoConfirmacion = By.XPath("//input[@id='checkTypePaymentMethod']");

        private By tabTarjetaCreditoConfirmacion = By.XPath("//span[normalize-space()='TARJETAS DE CREDITO']");
        private By tabTarjetaDebitoConfirmacion = By.XPath("//span[normalize-space()='TARJETAS DE DEBITO']");
        private By tabTransferenciaConfirmacion = By.XPath("//span[normalize-space()='TRANSFERENCIA DE FONDOS']");
        private By tabDepositosConfirmacion = By.XPath("//span[normalize-space()='DEPOSITOS EN CUENTA']");
        private By tabPuntosConfirmacion = By.XPath("//span[normalize-space()='PUNTOS']");

        private By cmbBancoConfirmacion = By.XPath("//select[@id='bankEntityId']");
        private By cmbTarjetaConfirmacion = By.XPath("//select[@id='bankingCard']");
        private By txtInformacionConfirmacion = By.XPath("//input[@id='informacion']");
        //private By cmbCuentaBancariaConfirmacion = By.XPath("//select[@id='bankEntityId']");
        private By cmbCuentaBancariaConfirmacion = By.XPath("//select[@id='bankAccountId' or @id='bankEntityId']");
        private By txtNumeroCuotasConfirmacion = By.XPath("//input[@type='number'][@min='1'][@max='60']");
        private By txtMontoInicialCreditoConfirmacion = By.XPath("//input[@type='number'][@min='0']");
        private By btnAgregarMedioPagoConfirmacion = By.XPath("//button[normalize-space()='Agregar Medio de Pago']");

        private By rbtCreditoConfirmacion = By.XPath("//label[normalize-space()='Crédito']");

        private By txtMontoMedioPagoConfirmacion = By.XPath("//input[@type='number' and not(@id='amountReceived')]");

        //PARA TOTAL BASE
        private const string TOTAL_BASE_MAYOR_700 = "759";
        private const string TOTAL_BASE_MENOR_IGUAL_700 = "32";

        //-------------------------
        private string ultimaAccion = "";
        private string ultimoMedioPagoConfirmacion = "";
        //private string? mensajeErrorCapturado = null;

        private string? mensajeErrorCapturado = null;
        public bool HayErrorCapturado() => !string.IsNullOrEmpty(mensajeErrorCapturado);
        
        private string NormalizarTextoComprobante(string tipoComprobante)
        {
            string t = (tipoComprobante ?? "").Trim().ToUpperInvariant()
                .Replace("Á", "A").Replace("É", "E").Replace("Í", "I")
                .Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N");

            // Mapeo: texto del feature → fragmento exacto que aparece en el DOM
            if (t.Contains("NOTA DE VENTA")) return "NOTA DE VENTA";   //  "NOTA DE VENTA(INTERNA)"
            if (t.Contains("FACTURA")) return "FACTURA ELECTRONICA";
            if (t.Contains("BOLETA")) return "BOLETA DE VENTA ELECTRONICA";

            return t;
        }

        private string ObtenerTextoComprobanteSeleccionado()
        {
            try
            {
                var combos = driver.FindElements(cmbTipoComprobanteConfirmacion)
                    .Where(e => e.Displayed && e.Enabled)
                    .ToList();

                if (!combos.Any())
                    return string.Empty;

                var combo = combos.FirstOrDefault(c =>
                {
                    var txt = (c.Text ?? "").Trim().ToUpperInvariant();
                    return txt.Contains("BOLETA") || txt.Contains("FACTURA") || txt.Contains("NOTA DE VENTA");
                }) ?? combos.Last();

                return (combo.Text ?? string.Empty).Trim().ToUpperInvariant();
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

        private void ClickTabConfirmacion(By locator)
        {
            var tab = wait.Until(d =>
            {
                try
                {
                    var visibles = d.FindElements(locator)
                        .Where(e => e.Displayed && e.Enabled)
                        .ToList();

                    return visibles.FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            });

            if (tab == null)
                throw new Exception($"No se encontró el tab visible para locator: {locator}");

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", tab);

            Thread.Sleep(300);

            try
            {
                tab.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", tab);
            }

            Thread.Sleep(600);
        }

        private bool BotonEstaDeshabilitado(IWebElement boton)
        {
            try
            {
                string disabled = (boton.GetAttribute("disabled") ?? "").Trim().ToLower();
                string ariaDisabled = (boton.GetAttribute("aria-disabled") ?? "").Trim().ToLower();
                string clases = (boton.GetAttribute("class") ?? "").Trim().ToLower();
                string pointerEvents = (boton.GetCssValue("pointer-events") ?? "").Trim().ToLower();

                return !boton.Enabled ||
                       disabled == "true" ||
                       disabled == "disabled" ||
                       ariaDisabled == "true" ||
                       clases.Contains("disabled") ||
                       pointerEvents == "none";
            }
            catch
            {
                return false;
            }
        }

        // METODOS
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

            Thread.Sleep(1000);
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

            input.Click();
            Thread.Sleep(200);

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(200);

            input.SendKeys(cantidad);

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
            if (!activo.Trim().Equals("true", StringComparison.OrdinalIgnoreCase))
                return;

            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            waitLong.PollingInterval = TimeSpan.FromMilliseconds(200);

            void ClickSeguro(By locator)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        var el = waitLong.Until(d =>
                        {
                            try
                            {
                                var e = d.FindElement(locator);
                                return (e.Displayed && e.Enabled) ? e : null;
                            }
                            catch
                            {
                                return null;
                            }
                        });

                        ((IJavaScriptExecutor)driver)
                            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", el);

                        Thread.Sleep(150);

                        try
                        {
                            el.Click();
                        }
                        catch
                        {
                            ((IJavaScriptExecutor)driver)
                                .ExecuteScript("arguments[0].click();", el);
                        }

                        Thread.Sleep(250);
                        return;
                    }
                    catch (StaleElementReferenceException)
                    {
                        Thread.Sleep(250);
                    }
                }

                throw new Exception($"No se pudo hacer click en el elemento: {locator}");
            }

            ClickSeguro(chkDescuento);

            if (tipo.Trim().Equals("item", StringComparison.OrdinalIgnoreCase))
                ClickSeguro(btnDescuentoItem);

            if (tipo.Trim().Equals("global", StringComparison.OrdinalIgnoreCase))
                ClickSeguro(btnDescuentoGlobal);

            if (modo.Trim().Equals("$", StringComparison.OrdinalIgnoreCase))
                ClickSeguro(btnDescuentoSoles);

            if (modo.Trim().Equals("%", StringComparison.OrdinalIgnoreCase))
                ClickSeguro(btnDescuentoPorcentaje);

            var input = waitLong.Until(d =>
            {
                try
                {
                    var e = d.FindElement(txtDescuento);
                    return (e.Displayed && e.Enabled) ? e : null;
                }
                catch
                {
                    return null;
                }
            });

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);

            Thread.Sleep(150);

            input.Click();
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            input.SendKeys(valor);
            input.SendKeys(Keys.Tab);

            Thread.Sleep(300);
        }

        public void BuscarCliente(string cliente)
        {
            try
            {
                if (cliente == "00000000" || cliente.ToLower() == "varios")
                {
                    Console.WriteLine("Cliente VARIOS - no se realiza búsqueda");
                    return;
                }

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                var input = wait.Until(ExpectedConditions.ElementIsVisible(txtCliente));

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);

                wait.Until(ExpectedConditions.ElementToBeClickable(txtCliente)).Click();

                // limpiar
                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Delete);

                Thread.Sleep(200);

                // ESCRIBIR CLIENTE 
                input.SendKeys(cliente);

                Thread.Sleep(300);

                // ENTER 
                input.SendKeys(Keys.Enter);

                //  esperar que cambie el valor o se procese la búsqueda
                wait.Until(d =>
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

        // Validar si existe al menos un pedido REGISTRADO
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

            try
            {
                waitLong.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingContainer));
            }
            catch { }

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

    // Scroll y click
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
                catch { return null; }
            });

            if (combo == null)
            {
                Console.WriteLine("[Comprobante] No se encontró el combo de comprobante.");
                return false;
            }

     ((IJavaScriptExecutor)driver)
         .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", combo);

            try { combo.Click(); }
            catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", combo); }

            IWebElement inputBuscar = null!;
            try
            {
                inputBuscar = new WebDriverWait(driver, TimeSpan.FromSeconds(2))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(120)
                }.Until(d =>
                {
                    try
                    {
                        var inputs = d.FindElements(By.XPath(
                            "//ng-select[.//div[contains(@class,'select-trigger') and contains(@class,'form-control')]]" +
                            "//input[@type='text']"
                        ))
                        .Where(e => e.Displayed && e.Enabled)
                        .ToList();

                        Console.WriteLine($"[Comprobante] Inputs en ng-select del comprobante: {inputs.Count}");
                        if (inputs.Any()) return inputs.First();

                        var fallbackInputs = d.FindElements(By.XPath(
                            "//input[@type='text' and not(contains(@class,'search-input'))]"
                        ))
                        .Where(e => e.Displayed && e.Enabled)
                        .ToList();

                        Console.WriteLine($"[Comprobante] Inputs fallback: {fallbackInputs.Count}");
                        return fallbackInputs.FirstOrDefault();
                    }
                    catch { return null; }
                });
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("[Comprobante] No apareció input de búsqueda, se intentará seleccionar directo por texto.");
            }

            string textoBusqueda = ObtenerTextoBusquedaComprobante(tipoComprobante);

            if (inputBuscar != null)
            {
                Console.WriteLine($"[Comprobante] Escribiendo '{textoBusqueda}' en el input de búsqueda.");
                try { inputBuscar.Click(); } catch { }

                try { inputBuscar.Clear(); }
                catch
                {
                    inputBuscar.SendKeys(Keys.Control + "a");
                    inputBuscar.SendKeys(Keys.Delete);
                }

                inputBuscar.SendKeys(textoBusqueda);
            }

            var opcion = new WebDriverWait(driver, TimeSpan.FromSeconds(4))
            {
                PollingInterval = TimeSpan.FromMilliseconds(120)
            }.Until(d =>
            {
                try
                {
                    var opciones = d.FindElements(By.CssSelector(".option-item.ng-star-inserted"))
                        .Where(e => e.Displayed)
                        .ToList();

                    Console.WriteLine($"[Comprobante] Opciones .option-item.ng-star-inserted visibles: {opciones.Count}");

                    var encontrada = opciones.FirstOrDefault(e =>
                        CoincideComprobante((e.Text ?? "").Trim(), tipoComprobante));

                    if (encontrada != null) return encontrada;

                    var opcionesXPath = d.FindElements(By.XPath(
                        "//div[contains(@class,'ng-option')]" +
                        " | //div[@role='option']" +
                        " | //div[contains(@class,'option-item')]"
                    ))
                    .Where(e => e.Displayed)
                    .ToList();

                    return opcionesXPath.FirstOrDefault(e =>
                        CoincideComprobante((e.Text ?? "").Trim(), tipoComprobante));
                }
                catch { return null; }
            });

            if (opcion == null)
            {
                Console.WriteLine($"[Comprobante] No se encontró opción visible para '{tipoComprobante}'.");
                return false;
            }

            Console.WriteLine($"[Comprobante] Opción encontrada: '{opcion.Text}'");

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", opcion);

            try { opcion.Click(); }
catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", opcion); }

try
{
    new WebDriverWait(driver, TimeSpan.FromSeconds(3))
    {
        PollingInterval = TimeSpan.FromMilliseconds(120)
    }.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(panelDropdownNgSelect));
}
catch
{
    Console.WriteLine("[Comprobante] El panel dropdown no se ocultó a tiempo, se continúa con validación.");
}

try
{
    new WebDriverWait(driver, TimeSpan.FromSeconds(2))
    {
        PollingInterval = TimeSpan.FromMilliseconds(120)
    }.Until(d =>
    {
        try
        {
            var textoCombo = (combo.Text ?? "").Trim();
            Console.WriteLine($"[Comprobante] Texto actual combo: '{textoCombo}'");
            return CoincideComprobante(textoCombo, tipoComprobante);
        }
        catch
        {
            return false;
        }
    });

    return true;
}
catch
{
    Console.WriteLine("[Comprobante] La validación directa del combo no confirmó a tiempo.");
}

return ComprobanteSeleccionadoCoincide(tipoComprobante);
        }

        private void SeleccionarSerieConfirmacion(WebDriverWait waitLong, string serie)
        {
            try
            {
                // CAMBIO: usar timeout corto (2s) en vez del waitLong de 20s
                var waitSerie = new WebDriverWait(driver, TimeSpan.FromSeconds(2))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                };

                var radioSerie = waitSerie.Until(d =>
                {
                    try
                    {
                        var candidatos = d.FindElements(By.XPath(
                            $"//label[normalize-space(.)='{serie}']"
                        ));

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
                Console.WriteLine($"[Serie] '{serie}' no encontrada como radio button; se asume preseleccionada.");
            }
        }
        private void AbrirFacturacionConfirmacion()
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            bool yaVisible = driver.FindElements(By.XPath(
                "//div[contains(@class,'accordion-body')][.//label[contains(normalize-space(),'Cliente')]]"
            )).Any(e => e.Displayed);

            if (yaVisible)
            {
                Console.WriteLine("[AbrirFacturacionConfirmacion] Ya está visible, no se hace click.");
                return;
            }

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

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", header);

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
                    var btnGuia = new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d =>
                    {
                        try
                        {
                            return d.FindElements(btnGuiaRemisionConfirmacion)
                                .FirstOrDefault(e => e.Displayed);
                        }
                        catch
                        {
                            return null;
                        }
                    });

                    if (btnGuia == null)
                    {
                        Console.WriteLine("[Entrega] Botón guía no encontrado.");
                        mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                        return;
                    }

    ((IJavaScriptExecutor)driver)
        .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btnGuia);

                    Thread.Sleep(300);

                    string clases = (btnGuia.GetAttribute("class") ?? "").ToLower();
                    string disabled = btnGuia.GetAttribute("disabled") ?? "";
                    string ariaDisabled = btnGuia.GetAttribute("aria-disabled") ?? "";
                    string pointerEvents = btnGuia.GetCssValue("pointer-events") ?? "";

                    Console.WriteLine($"[Entrega] Botón guía — enabled:{btnGuia.Enabled}, class:'{clases}', disabled:'{disabled}', aria-disabled:'{ariaDisabled}', pointer-events:'{pointerEvents}'");

                    bool estaDeshabilitado = BotonEstaDeshabilitado(btnGuia);

                    if (estaDeshabilitado)
                    {
                        Console.WriteLine("[Entrega] Botón guía deshabilitado. No se hace click.");
                        mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                        return;
                    }

                    try
                    {
                        btnGuia.Click();
                    }
                    catch
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnGuia);
                    }

                    bool modalAbierto = false;
                    try
                    {
                        new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(d =>
                            d.FindElements(By.XPath("//button[normalize-space()='Aceptar']"))
                             .Any(e => e.Displayed));
                        modalAbierto = true;
                    }
                    catch { }

                    if (!modalAbierto)
                    {
                        Console.WriteLine("[Entrega] No se abrió el modal de guía.");
                        mensajeErrorCapturado = "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
                        return;
                    }

                    Console.WriteLine("[Entrega] Modal guía abierto correctamente.");
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
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            {
                PollingInterval = TimeSpan.FromMilliseconds(200)
            };

            bool yaVisible = driver.FindElements(bodyPagoConfirmacion).Any(e => e.Displayed);
            if (yaVisible)
            {
                Console.WriteLine("[AbrirPagoConfirmacion] Ya está visible, no se hace click.");
                return;
            }

            var header = waitLong.Until(d =>
            {
                try
                {
                    var el = d.FindElements(seccionPagoConfirmacion)
                        .FirstOrDefault(e => e.Displayed && e.Enabled);
                    return el;
                }
                catch
                {
                    return null;
                }
            });

            if (header == null)
                throw new Exception("No se encontró la sección Pago de confirmación.");

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", header);

            Thread.Sleep(250);

            try
            {
                header.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", header);
            }

            waitLong.Until(d =>
                d.FindElements(bodyPagoConfirmacion).Any(e => e.Displayed));
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

                Thread.Sleep(100);
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

                string errorModal = VerificarErrorModalComprobante();
                if (errorModal != null)
                {
                    Console.WriteLine($"[Facturacion] Error modal detectado: '{errorModal}'");
                    mensajeErrorCapturado = errorModal;
                    return; // salir sin continuar con serie ni cerrar sección
                }


                Thread.Sleep(100);

                if (!serie.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                {
                    SeleccionarSerieConfirmacion(waitLong, serie);
                }

                // Cerrar sección facturación
                AbrirFacturacionConfirmacion();
            }

            catch (AssertionException) { throw; }
            catch (InvalidOperationException ex) when (ex.Message.StartsWith("ERROR_MODAL:"))
            {
                throw; // re-lanzar para que llegue al step
            }
            catch (Exception e)
            {
                Assert.Fail("Error configurando facturación de confirmación: " + e.Message);
            }
        }

        // Verificacion del error de RUC con factura
        private string VerificarErrorModalComprobante()
        {
            try
            {
                var mensajeError = new WebDriverWait(driver, TimeSpan.FromSeconds(2))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(150)
                }.Until(d =>
                {
                    try
                    {
                        var modal = d.FindElements(By.XPath(
                            "//*[contains(text(),'Para emitir Factura') or " +
                            "contains(text(),'RUC (11 dígitos)') or " +
                            "contains(text(),'número de serie') or " +
                            "contains(text(),'numero de serie')]"
                        )).FirstOrDefault(e => e.Displayed);

                        return modal;
                    }
                    catch { return null; }
                });

                string texto = mensajeError.Text.Trim();

                // Cerrar el modal haciendo click en OK
                try
                {
                    var btnOK = driver.FindElement(btnOKConfirmacion);
                    if (btnOK.Displayed)
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnOK);
                }
                catch { }

                return texto;
            }
            catch (WebDriverTimeoutException)
            {
                return null!; // No hubo error modal
            }
        }
        public void ConfigurarPagoConfirmacion(string tipoPago, string montoCubreTotal)
        {
            try
            {
                var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(200)
                };

                AbrirPagoConfirmacion();

                // usar el parámetro real
                SeleccionarTipoPagoConfirmacion(tipoPago);

                SeleccionarTabMedioPagoConfirmacion("efectivo");

                waitLong.Until(d =>
                {
                    try
                    {
                        return d.FindElements(txtRecibidoEfectivoConfirmacion)
                            .Any(e => e.Displayed && e.Enabled);
                    }
                    catch
                    {
                        return false;
                    }
                });

                IngresarMontoEfectivoConfirmacion(montoCubreTotal);
            }
            catch (Exception e)
            {
                Assert.Fail("Error configurando pago en efectivo de confirmación: " + e.Message);
            }
        }

        public void ConfigurarMediosDePagoConfirmacion(
        string tipoPago,
        string multipago,
        string medioPago,
        string banco,
        string tarjeta,
        string cuentaBancaria,
        string nroOperacion,
        string montoPorMedio,
        string nroCuotas,
        string montoInicialCredito)
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

                    if (!EsNA(montoInicialCredito))
                        IngresarMontoInicialCreditoConfirmacion(montoInicialCredito);
                }

                var medios = SepararValores(medioPago);
                var bancos = new Queue<string>(SepararValoresFiltrados(banco));
                var tarjetas = new Queue<string>(SepararValoresFiltrados(tarjeta));
                var cuentas = new Queue<string>(SepararValoresFiltrados(cuentaBancaria));
                var operaciones = new Queue<string>(SepararValoresFiltrados(nroOperacion));
                var montos = new Queue<string>(SepararValoresFiltrados(montoPorMedio));

                Console.WriteLine($"[Multipago] Medios: {medios.Count}");
                Console.WriteLine($"[Multipago] Montos: {montos.Count}");

                for (int i = 0; i < medios.Count; i++)
                {
                    string medioActual = medios[i].Trim();
                    string montoActual = montos.Count > 0 ? montos.Dequeue() : "NA";

                    Console.WriteLine($"[Multipago] Inicio medio #{i + 1}: {medioActual}");
                    Console.WriteLine($"[Multipago] Monto usado para medio #{i + 1}: {montoActual}");

                    Console.WriteLine($"[Multipago] Seleccionando tab: {medioActual}");
                    SeleccionarTabMedioPagoConfirmacion(medioActual);
                    Console.WriteLine($"[Multipago] Tab seleccionado: {medioActual}");

                    ConfigurarMedioPagoConfirmacion(
                        medioActual,
                        tipoPago,
                        montoActual,
                        bancos,
                        tarjetas,
                        cuentas,
                        operaciones
                    );

                    Console.WriteLine($"[Multipago] Medio configurado: {medioActual}");

                    if (esMultipago)
                    {
                        Console.WriteLine($"[Multipago] Guardando medio #{i + 1}");
                        GuardarMedioPagoActual();
                    }
                }

                if (esMultipago)
                {
                    ultimoMedioPagoConfirmacion = string.Empty;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Error configurando medios de pago en confirmación: " + e.Message);
            }
        }

        private void GuardarMedioPagoActual()
        {
            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(btnAgregarMedioPagoConfirmacion));

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            Thread.Sleep(300);

            try
            {
                boton.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", boton);
            }

            Thread.Sleep(900);
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
                catch { return null; }
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

                // Verificar error visible con botón deshabilitado
                string? errorPago = VerificarErrorCamposPago();
                if (errorPago != null)
                {
                    Console.WriteLine($"[ConfirmarPedidoPreparado] Error detectado (botón deshabilitado): '{errorPago}'");
                    mensajeErrorCapturado = errorPago;
                }
                return;
            }

            // verificar error ANTES del click aunque el botón esté habilitado
            string? errorPreClick = VerificarErrorCamposPago();
            if (errorPreClick != null)
            {
                Console.WriteLine($"[ConfirmarPedidoPreparado] Error detectado (pre-click): '{errorPreClick}'");
                mensajeErrorCapturado = errorPreClick;
                return; // ← NO hacer click, salir directo
            }

            try { boton.Click(); }
            catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton); }

            Thread.Sleep(800);

            //  verificar error también DESPUÉS del click (ej: puntos insuficientes que habilita botón)
            string? errorPostClick = VerificarErrorCamposPago();
            if (errorPostClick != null)
            {
                Console.WriteLine($"[ConfirmarPedidoPreparado] Error detectado (post-click): '{errorPostClick}'");
                mensajeErrorCapturado = errorPostClick;
            }
        }

        private string? VerificarErrorCamposPago()
        {
            try
            {
                var mensajePuntos = driver.FindElements(By.XPath(
                    "//*[contains(text(),'No hay suficientes puntos disponibles') or contains(text(),'Puntos insuficiente') or contains(text(),'Puntos insuficientes')]"
                )).FirstOrDefault(e => e.Displayed);

                if (mensajePuntos != null)
                    return "Puntos insuficiente";

                var mensajeMonto = driver.FindElements(By.XPath(
                    "//*[contains(text(),'Monto insuficiente')]"
                )).FirstOrDefault(e => e.Displayed);

                if (mensajeMonto != null)
                    return "Monto insuficiente";

                var mensajeCredito = driver.FindElements(By.XPath(
                    "//*[contains(text(),'Para dar a credito debe identificar al cliente') or contains(text(),'Para dar a crédito debe identificar al cliente')]"
                )).FirstOrDefault(e => e.Displayed);

                if (mensajeCredito != null)
                    return "Para dar a credito debe identificar al cliente";

                var mensajePuntosCliente = driver.FindElements(By.XPath(
                    "//*[contains(text(),'Para el pago con puntos debe identificar al cliente')]"
                )).FirstOrDefault(e => e.Displayed);

                if (mensajePuntosCliente != null)
                    return "Para el pago con puntos debe identificar al cliente";

                //----------
                try
                {
                    var botonConfirmar = driver.FindElements(btnConfirmarPedidoFinal)
                        .FirstOrDefault(e => e.Displayed);

                    if (botonConfirmar != null)
                    {
                        bool habilitado =
                            botonConfirmar.Enabled &&
                            botonConfirmar.GetAttribute("disabled") == null &&
                            !(botonConfirmar.GetAttribute("class") ?? "").ToLower().Contains("disabled");

                        if (habilitado)
                            return null;
                    }
                }
                catch { }

                if (string.IsNullOrWhiteSpace(ultimoMedioPagoConfirmacion))
                {
                    var mensajeCompletoSolo = driver.FindElements(
                        By.XPath("//*[contains(text(),'Complete los campos requeridos')]")
                    ).FirstOrDefault(e => e.Displayed);

                    if (mensajeCompletoSolo != null)
                        return null;

                    return null;
                }

                // EFECTIVO
                if (ultimoMedioPagoConfirmacion == "efectivo")
                {
                    var inputEfectivo = driver.FindElements(txtRecibidoEfectivoConfirmacion)
                        .FirstOrDefault(e => e.Displayed && e.Enabled);

                    if (inputEfectivo != null)
                    {
                        var valor = (inputEfectivo.GetAttribute("value") ?? "").Trim();
                        if (string.IsNullOrEmpty(valor) || valor == "0")
                            return "Monto insuficiente";
                    }
                }

                // TARJETA CRÉDITO / DÉBITO
                if (ultimoMedioPagoConfirmacion == "tarjeta_credito" ||
                    ultimoMedioPagoConfirmacion == "tarjeta_debito")
                {
                    var selectBanco = driver.FindElements(cmbBancoConfirmacion)
                        .Where(e => e.Displayed && e.Enabled)
                        .LastOrDefault();

                    if (selectBanco == null)
                        return "Seleccione una entidad bancaria";

                    var comboBanco = new SelectElement(selectBanco);
                    var textoSeleccionado = (comboBanco.SelectedOption?.Text ?? "").Trim();

                    if (string.IsNullOrWhiteSpace(textoSeleccionado) ||
                        textoSeleccionado.Equals("Seleccione", StringComparison.OrdinalIgnoreCase) ||
                        textoSeleccionado.Equals("Seleccione una opción", StringComparison.OrdinalIgnoreCase) ||
                        textoSeleccionado.Equals("ninguno", StringComparison.OrdinalIgnoreCase) ||
                        textoSeleccionado.StartsWith("--Se", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Seleccione una entidad bancaria";
                    }

                    var selectTarjeta = driver.FindElements(cmbTarjetaConfirmacion)
                        .Where(e => e.Displayed && e.Enabled)
                        .LastOrDefault();

                    if (selectTarjeta != null)
                    {
                        var comboTarjeta = new SelectElement(selectTarjeta);
                        var textoTarjeta = (comboTarjeta.SelectedOption?.Text ?? "").Trim();

                        if (string.IsNullOrWhiteSpace(textoTarjeta) ||
                            textoTarjeta.Equals("Seleccione", StringComparison.OrdinalIgnoreCase) ||
                            textoTarjeta.Equals("Seleccione una opción", StringComparison.OrdinalIgnoreCase) ||
                            textoTarjeta.Equals("ninguno", StringComparison.OrdinalIgnoreCase) ||
                            textoTarjeta.StartsWith("--Se", StringComparison.OrdinalIgnoreCase))
                        {
                            return "Seleccione una entidad bancaria";
                        }
                    }

                    return null;
                }

                // TRANSFERENCIA / DEPÓSITO
                if (ultimoMedioPagoConfirmacion == "transferencia_fondos" ||
                    ultimoMedioPagoConfirmacion == "deposito_cuenta")
                {
                    var selectCuenta = driver.FindElements(cmbCuentaBancariaConfirmacion)
                        .Where(e => e.Displayed && e.Enabled)
                        .LastOrDefault();

                    if (selectCuenta == null)
                        return "Seleccione una cuenta bancaria";

                    var comboCuenta = new SelectElement(selectCuenta);
                    var textoSeleccionado = (comboCuenta.SelectedOption?.Text ?? "").Trim();

                    if (string.IsNullOrWhiteSpace(textoSeleccionado) ||
                        textoSeleccionado.Equals("Seleccione", StringComparison.OrdinalIgnoreCase) ||
                        textoSeleccionado.Equals("Seleccione una opción", StringComparison.OrdinalIgnoreCase) ||
                        textoSeleccionado.Equals("ninguno", StringComparison.OrdinalIgnoreCase) ||
                        textoSeleccionado.StartsWith("--Se", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Seleccione una cuenta bancaria";
                    }

                    return null;
                }

                // Mensaje genérico final
                var mensajeCompleto = driver.FindElements(
                    By.XPath("//*[contains(text(),'Complete los campos requeridos')]")
                ).FirstOrDefault(e => e.Displayed);

                if (mensajeCompleto != null)
                    return "Complete los campos requeridos";

                return null;
            }
            catch
            {
                return null;
            }
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

        private void SeleccionarTabMedioPagoConfirmacion(string medioPago)
        {
            string medio = medioPago.Trim().ToLowerInvariant();
            ultimoMedioPagoConfirmacion = medio;

            switch (medio)
            {
                case "efectivo":
                    ClickTabConfirmacion(tabEfectivoConfirmacion);

                    new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                    {
                        PollingInterval = TimeSpan.FromMilliseconds(150)
                    }.Until(d =>
                    {
                        try
                        {
                            return d.FindElements(txtRecibidoEfectivoConfirmacion)
                                .Any(e => e.Displayed && e.Enabled);
                        }
                        catch
                        {
                            return false;
                        }
                    });
                    return;

                case "tarjeta_credito":
                    ClickTabConfirmacion(tabTarjetaCreditoConfirmacion);
                    return;

                case "tarjeta_debito":
                    ClickTabConfirmacion(tabTarjetaDebitoConfirmacion);
                    return;

                case "transferencia_fondos":
                    ClickTabConfirmacion(tabTransferenciaConfirmacion);
                    return;

                case "deposito_cuenta":
                    ClickTabConfirmacion(tabDepositosConfirmacion);
                    return;

                case "puntos":
                    ClickTabConfirmacion(tabPuntosConfirmacion);
                    return;

                default:
                    throw new Exception($"Medio de pago no soportado: {medioPago}");
            }
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
                    ClickTabConfirmacion(tabEfectivoConfirmacion);
                    Thread.Sleep(500);

                    // Contado simple: usa recibido
                    if (tipoPago.Trim().Equals("contado", StringComparison.OrdinalIgnoreCase))
                    {
                        IngresarMontoEfectivoConfirmacion(monto);
                    }
                    else
                    {
                        // Crédito: usa monto del medio (amountToPay)
                        IngresarMontoMedioPagoConfirmacion(monto);
                    }
                    break;

                case "tarjeta_credito":
                case "tarjeta_debito":
                    SeleccionarBancoConfirmacion(ConsumirSiguientePago(bancos));
                    Thread.Sleep(500);
                    SeleccionarTarjetaConfirmacion(ConsumirSiguientePago(tarjetas));
                    Thread.Sleep(300);
                    IngresarMontoMedioPagoConfirmacion(monto);
                    Thread.Sleep(300);
                    IngresarInformacionConfirmacion(ConsumirSiguientePago(operaciones));
                    break;

                case "transferencia_fondos":
                case "deposito_cuenta":
                    SeleccionarCuentaBancariaConfirmacion(ConsumirSiguientePago(cuentas));
                    Thread.Sleep(300);
                    IngresarMontoMedioPagoConfirmacion(monto);
                    Thread.Sleep(300);
                    IngresarInformacionConfirmacion(ConsumirSiguientePago(operaciones));
                    break;

                case "puntos":
                    break;
            }
        }

        // HLEP para ingresar monto en efectivo, ya que tiene un comportamiento especial de validación al escribir
        private void IngresarMontoMedioPagoConfirmacion(string monto)
        {
            if (EsNA(monto)) return;

            string valor = ResolverMontoPago(monto);
            if (string.IsNullOrWhiteSpace(valor)) return;

            var input = wait.Until(d =>
            {
                try
                {
                    var elementos = d.FindElements(txtMontoMedioPagoConfirmacion)
                        .Where(e => e.Displayed && e.Enabled)
                        .ToList();

                    return elementos.Any() ? elementos.Last() : null;
                }
                catch
                {
                    return null;
                }
            });

            if (input == null)
                throw new Exception("No se encontró el input de monto del medio de pago.");

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);

            Thread.Sleep(300);

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", input);

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Delete);
            Thread.Sleep(150);
            input.SendKeys(valor);
            input.SendKeys(Keys.Tab);

            Thread.Sleep(500);
        }

        private void IngresarMontoEfectivoConfirmacion(string monto)
        {
            if (EsNA(monto)) return;

            string valor = ResolverMontoPago(monto);
            Console.WriteLine($"[Efectivo] Parámetro recibido: '{monto}'");
            Console.WriteLine($"[Efectivo] Monto resuelto: '{valor}'");

            if (string.IsNullOrWhiteSpace(valor))
                throw new Exception("No se resolvió un monto válido para efectivo.");

            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(15))
            {
                PollingInterval = TimeSpan.FromMilliseconds(150)
            };

            var input = waitLong.Until(d =>
            {
                try
                {
                    return d.FindElements(txtRecibidoEfectivoConfirmacion)
                        .FirstOrDefault(e => e.Displayed && e.Enabled);
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

            Thread.Sleep(200);

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
            input.SendKeys(valor);
            input.SendKeys(Keys.Tab);

            waitLong.Until(d =>
            {
                try
                {
                    var v = input.GetAttribute("value") ?? "";
                    return !string.IsNullOrWhiteSpace(v);
                }
                catch
                {
                    return false;
                }
            });
        }

        private void SeleccionarBancoConfirmacion(string banco)
        {
            if (EsNA(banco) || banco.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                return;

            Console.WriteLine($"[Banco] Intentando seleccionar banco: {banco}");

            var select = ObtenerUltimoSelectVisibleConfirmacion(cmbBancoConfirmacion);
            if (select == null)
                throw new Exception("No se encontró un combo visible de banco.");

            SeleccionarOpcionSelectConfirmacion(select, banco.Trim());

            Console.WriteLine($"[Banco] Banco seleccionado: {banco}");

            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(6)).Until(d =>
                {
                    var tarjetaSelect = d.FindElements(cmbTarjetaConfirmacion)
                        .Where(e => e.Displayed && e.Enabled)
                        .LastOrDefault();

                    if (tarjetaSelect == null) return false;

                    return new SelectElement(tarjetaSelect).Options.Count > 1;
                });

                Console.WriteLine("[Banco] Combo de tarjeta cargado correctamente.");
            }
            catch
            {
                Console.WriteLine("[Banco] Timeout esperando combo de tarjeta.");
                throw;
            }
        }

        private void SeleccionarTarjetaConfirmacion(string tarjeta)
        {
            if (EsNA(tarjeta) || tarjeta.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                return;

            Console.WriteLine($"[Tarjeta] Intentando seleccionar tarjeta: {tarjeta}");

            var select = ObtenerUltimoSelectVisibleConfirmacion(cmbTarjetaConfirmacion);
            if (select == null)
                throw new Exception("No se encontró un combo visible de tarjeta.");

            SeleccionarOpcionSelectConfirmacion(select, tarjeta.Trim());

            Console.WriteLine($"[Tarjeta] Tarjeta seleccionada: {tarjeta}");
        }

        private void SeleccionarCuentaBancariaConfirmacion(string cuentaBancaria)
        {
            if (EsNA(cuentaBancaria) || cuentaBancaria.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                return;

            Console.WriteLine($"[Cuenta] Intentando seleccionar cuenta: {cuentaBancaria}");

            var select = ObtenerUltimoSelectVisibleConfirmacion(cmbCuentaBancariaConfirmacion);
            if (select == null)
                throw new Exception("No se encontró un combo visible de cuenta bancaria.");

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
                    throw new Exception($"No se encontró la cuenta bancaria '{texto}' en el combo.");
            }

        ((IJavaScriptExecutor)driver).ExecuteScript(@"
        arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
        arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
        arguments[0].blur();
    ", select);

            Console.WriteLine($"[Cuenta] Cuenta seleccionada: {cuentaBancaria}");
            Thread.Sleep(500);
        }

        private void IngresarInformacionConfirmacion(string informacion)
        {
            if (EsNA(informacion) || informacion.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                return;

            var input = ObtenerUltimoInputVisibleConfirmacion(txtInformacionConfirmacion);
            if (input == null)
                throw new Exception("No se encontró el input visible de información.");

            LimpiarYEscribirCampo(input, informacion.Trim());
        }

        // medio pag mult
        private IWebElement ObtenerUltimoSelectVisibleConfirmacion(By locator)
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

        private IWebElement ObtenerUltimoInputVisibleConfirmacion(By locator)
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

        private void SeleccionarOpcionSelectConfirmacion(IWebElement selectElement, string texto)
        {
            var combo = new SelectElement(selectElement);

            var opcion = combo.Options.FirstOrDefault(o =>
                o.Text.Trim().Equals(texto.Trim(), StringComparison.OrdinalIgnoreCase));

            if (opcion == null)
                throw new Exception($"No se encontró la opción '{texto}' en el combo.");

            string? value = opcion.GetAttribute("value");

            // Forzar foco primero
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", selectElement);
            Thread.Sleep(200);

            // Seleccionar con Selenium (genera click real)
            combo.SelectByText(opcion.Text.Trim());
            Thread.Sleep(300);

            // Disparar todos los eventos que Angular necesita
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
                if (!string.IsNullOrEmpty(mensajeErrorCapturado))
        {
            string msg = mensajeErrorCapturado;
            mensajeErrorCapturado = null;
            return msg;
        }


                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // ── REGISTRO: MENSAJES DEL FORMULARIO ─────────────────
                try
                {
                    var waitRegistro = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

                    bool hayInconsistencia = waitRegistro.Until(d =>
                        d.FindElements(mensajeInconsistenciaRegistro).Any(e => e.Displayed) ||
                        d.FindElements(detalleInconsistenciaRegistro).Any(e => e.Displayed) ||
                        d.FindElements(mensajeSinProductoRegistro).Any(e => e.Displayed)
                    );

                    if (hayInconsistencia)
                    {
                        if (driver.FindElements(mensajeSinProductoRegistro).Any(e => e.Displayed))
                            return "Ningún producto seleccionado";

                        if (driver.FindElements(mensajeInconsistenciaRegistro).Any(e => e.Displayed) ||
                            driver.FindElements(detalleInconsistenciaRegistro).Any(e => e.Displayed))
                        {
                            return "muestra mensaje de inconsistencia";
                        }
                    }
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
                        return "Boton de guia de remision inhabilitado Para guia de remision Necesita identificar al cliente con RUC o DNI";
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
                }
                catch { }
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


                return "";
            }
            catch
            {
                return "";
            }
        }

    }
}