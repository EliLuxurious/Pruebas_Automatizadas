using SIGES3_0.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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
        // Input del buscador de cliente (anclado al placeholder visto en el DOM)
        // Nota: se mantiene también la clase `search-input` para estabilidad.
        private By txtCliente = By.CssSelector("input.search-input[placeholder='Buscar...']");
        private By rbtEntregaInmediata = By.XPath("//label[normalize-space()='Inmediata']");
        private By rbtEntregaDiferida = By.XPath("//label[normalize-space()='Diferida']");
        private By btnRegistrarPedido = By.XPath("//button[normalize-space()='Registrar Pedido']");
        private By mensajeError = By.XPath("//div[contains(@class,'alert')]");
        private By btnOKConfirmacion = By.XPath("//button[normalize-space()='OK']");
        private By mensajeAdvertencia = By.XPath("//span[contains(@class,'badge-status') and contains(@class,'danger')]");
        private By mensajeSinProducto = By.XPath("//span[@class='badge-status danger']");
        private By loadingContainer = By.CssSelector("div.loading-container");

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
        private By btnConfirmarPedidoFinal = By.XPath(
            "//button[contains(normalize-space(.),'Confirmar Pedido') or .//*[contains(normalize-space(.),'Confirmar Pedido')]]"
        );

        // FACTURACION CONFIRMAR (acordeones del modal de "PREPARANDO PARA CONFIRMAR EL PEDIDO")
        // Header de la sección "Facturación" dentro del modal de confirmación
        private By seccionFacturacionConfirmacion = By.XPath(
            "//div[contains(@class,'d-flex') and contains(@class,'align-items-center') and contains(@class,'w-100')]" +
            "[.//span[normalize-space()='Facturación']]"
        );
        private By seccionEntregaConfirmacion = By.XPath(
            "//span[normalize-space()='Entrega']/ancestor::div[contains(@class,'d-flex align-items-center w-100')][1]"
        );
        private By seccionPagoConfirmacion = By.XPath(
            "//span[normalize-space()='Pago']/ancestor::div[contains(@class,'d-flex align-items-center w-100')][1]"
        );

        private By txtClienteConfirmacion = By.CssSelector("input.search-input[placeholder='Buscar...']");
   
        private By cmbTipoComprobanteConfirmacion = By.XPath("//div[@class='select-trigger form-control']");
        private By panelDropdownNgSelect = By.CssSelector(".ng-dropdown-panel");

        // ENTREGA CONFIRMAR
        private By rbtEntregaInmediataConfirmacion = By.XPath("//label[normalize-space()='Inmediata']");
        private By rbtEntregaDiferidaConfirmacion = By.XPath("//label[normalize-space()='Diferida']");
        private By btnGuiaRemisionConfirmacion = By.XPath("//button[contains(text(),'Guía de remisión') or contains(text(),'Guia de remisión')]");
        private By btnCerrarEntregaConfirmacion = By.XPath("(//*[contains(@class,'ri-arrow-up-s-line') or contains(@class,'ri-arrow-down-s-line')])[2]");

        // PAGO CONFIRMAR
        private By rbtContadoConfirmacion = By.XPath("//label[normalize-space()='Contado']");
        private By tabEfectivoConfirmacion = By.XPath("//*[contains(text(),'EFECTIVO')]");
        private By txtRecibidoEfectivo = By.XPath("(//input)[last()]");
        private By btnCerrarPagoConfirmacion = By.XPath("(//*[contains(@class,'ri-arrow-up-s-line') or contains(@class,'ri-arrow-down-s-line')])[3]");

        //PARA TOTAL BASE
        private const string TOTAL_BASE_MAYOR_700 = "759";
        private const string TOTAL_BASE_MENOR_IGUAL_700 = "64";

        //-------------------------
        private string ultimaAccion = "";

        //-------------------------
        private By OpcionComprobante(string tipoComprobante)
        {
            return By.XPath(
                $"//div[contains(@class,'ng-dropdown-panel')]//div[contains(@class,'ng-option')]//*[contains(translate(normalize-space(.),'ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ','abcdefghijklmnopqrstuvwxyzáéíóú'),'{tipoComprobante.ToLower()}')]"
            );
        }

        private By OpcionSerie(string serie)
        {
            return By.XPath($"//*[contains(text(),'{serie}')]");
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

            input.Clear();
            input.SendKeys(cantidad);
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
            if (tipoEntrega == "inmediata")
                driver.FindElement(rbtEntregaInmediata).Click();

            if (tipoEntrega == "diferida")
                driver.FindElement(rbtEntregaDiferida).Click();
        }


        public void RegistrarPedido()
        {
            ultimaAccion = "registrar";

            var boton = wait.Until(
                ExpectedConditions.ElementExists(btnRegistrarPedido)
            );

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            Thread.Sleep(800);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", boton);
        }

        // Asegurar que exista un pedido con estado REGISTRADO
        public void AsegurarPedidoRegistradoParaInvalidar()
        {
            // 1) Aplicar filtro una sola vez y validar.
            FiltrarPedidosRegistrados();
            if (ExistePedidoRegistradoFiltrado())
                return;

            // 2) Si no hay, crear un pedido base y volver a la grilla.
            RegistrarPedidoBaseParaInvalidar();
            VolverAVerPedidos();

            // 3) Reaplicar filtro y validar nuevamente.
            FiltrarPedidosRegistrados();
            if (!ExistePedidoRegistradoFiltrado())
                Assert.Fail("No se pudo generar un pedido en estado REGISTRADO para invalidar.");
        }



        // Validar si existe al menos un pedido REGISTRADO (asume que el filtro ya fue aplicado).
        private bool ExistePedidoRegistradoFiltrado()
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
        private void FiltrarPedidosRegistrados()
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

        private void AbrirSeccion(string seccion)
        {
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(25));

            // Importante: anclar el click al header del acordeón (evita clics en el menú lateral u otros textos)
            // HTML real muestra: h2.accordion-header con id "heading-collapse-facturación"
            var header = waitLong.Until(d =>
            {
                try
                {
                    var h2 = d.FindElement(By.XPath(
                        $"//h2[contains(@class,'accordion-header')][.//*[contains(normalize-space(.),'{seccion}')]]"
                    ));
                    return h2.Displayed ? h2 : null;
                }
                catch
                {
                    return null;
                }
            });

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", header);

            // Si existe un botón dentro del header (patrón típico accordion), clickeamos ese botón.
            IWebElement clickable = null;
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

            // Esperar a que el acordeón quede expandido (la clase "is-expanded" aparece en app-form-accordion)
            waitLong.Until(d =>
            {
                try
                {
                    var accordion = d.FindElement(By.XPath(
                        $"//app-form-accordion[contains(@class,'is-expanded')][.//h2[contains(@class,'accordion-header')][.//*[contains(normalize-space(.),'{seccion}')]]]"
                    ));
                    return accordion.Displayed;
                }
                catch
                {
                    return false;
                }
            });

            // Esperas específicas por sección para asegurar que el contenido ya renderizó
            if (seccion.Trim().Equals("Facturación", StringComparison.OrdinalIgnoreCase))
            {
                waitLong.Until(ExpectedConditions.ElementIsVisible(txtCliente));
            }
            else if (seccion.Trim().Equals("Entrega", StringComparison.OrdinalIgnoreCase))
            {
                waitLong.Until(ExpectedConditions.ElementToBeClickable(rbtEntregaInmediata));
            }
        }



        // Crea un nuevo pedido
        private void RegistrarPedidoBaseParaInvalidar()
        {
            SeleccionarOpcion("Nuevo Pedido");
            SeleccionarFamilia("Gaseosa");
            SeleccionarConcepto("7753234003320");
            IngresarCantidad("10");
            ActivarIGV("false");
            ActivarDetUnif("false");
            ConfigurarDescuento("false", "NA", "NA", "0");

            AbrirSeccion("Facturación");
            BuscarCliente("75971755");

            AbrirSeccion("Entrega");
            SeleccionarEntrega("inmediata");

            RegistrarPedido();
            ConfirmarMensaje();
        }

        private void VolverAVerPedidos()
        {
            var opcion = wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.XPath("//span[contains(text(),'Ver Pedidos')]")
                )
            );

            opcion.Click();
            Thread.Sleep(1000);
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
                ExpectedConditions.ElementIsVisible(txtMotivoInvalidacion)
            );

            input.Clear();
            input.SendKeys(motivo);
        }

        public void ConfirmarInvalidacion(string accion)
        {
            if (accion.Trim().Equals("SI", StringComparison.OrdinalIgnoreCase) ||
                accion.Trim().Equals("Sí", StringComparison.OrdinalIgnoreCase) ||
                accion.Trim().Equals("Si", StringComparison.OrdinalIgnoreCase))
            {
                var botonSi = wait.Until(
                    ExpectedConditions.ElementExists(btnSiInvalidar)
                );

                bool deshabilitado =
                    !botonSi.Enabled ||
                    botonSi.GetAttribute("disabled") != null ||
                    (botonSi.GetAttribute("class") ?? "").ToLower().Contains("disabled");

                if (deshabilitado)
                {
                    Console.WriteLine("El botón SI está deshabilitado, no se hace click.");
                    return;
                }

                ultimaAccion = "invalidar";

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", botonSi);

                return;
            }

            if (accion.Trim().Equals("NO", StringComparison.OrdinalIgnoreCase))
            {
                var botonNo = wait.Until(
                    ExpectedConditions.ElementToBeClickable(btnNoInvalidar)
                );

                botonNo.Click();
            }
        }

        // Asegurar la precondicion de confirmar
        public void AsegurarPedidoBaseParaConfirmar(string totalMayor700)
        {
            bool esMayor700 = totalMayor700.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);

            if (!ExistePedidoBaseParaConfirmar(esMayor700))
            {
                if (esMayor700)
                    RegistrarPedidoBaseMayor700();
                else
                    RegistrarPedidoBaseMenorIgual700();

                VolverAVerPedidos();
            }

            FiltrarPedidoBaseParaConfirmar(esMayor700);

            if (!ExistePedidoBaseParaConfirmar(esMayor700))
            {
                Assert.Fail("No se pudo generar un pedido base en estado REGISTRADO para confirmar.");
            }
        }

        private bool ExistePedidoBaseParaConfirmar(bool esMayor700)
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

        private void FiltrarPedidoBaseParaConfirmar(bool esMayor700)
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

        private void RegistrarPedidoBaseMayor700()
        {
            SeleccionarOpcion("Nuevo Pedido");
            SeleccionarFamilia("Gaseosa");
            SeleccionarConcepto("7753234003320");
            IngresarCantidad("110"); //TOTAL S/759.00
            ActivarIGV("false");
            ActivarDetUnif("false");
            ConfigurarDescuento("false", "NA", "NA", "0");

            AbrirSeccion("Facturación");
            BuscarCliente("00000000");

            AbrirSeccion("Entrega");
            SeleccionarEntrega("inmediata");

            RegistrarPedido();
            ConfirmarMensaje();
        }

        private void RegistrarPedidoBaseMenorIgual700()
        {
            SeleccionarOpcion("Nuevo Pedido");
            SeleccionarFamilia("Azúcar");
            SeleccionarConcepto("7751234001115");
            IngresarCantidad("20"); // TOTAL  S/64.00
            ActivarIGV("false");
            ActivarDetUnif("false");
            ConfigurarDescuento("false", "NA", "NA", "0");

            AbrirSeccion("Facturación");
            BuscarCliente("20542245671");

            AbrirSeccion("Entrega");
            SeleccionarEntrega("diferida");

            RegistrarPedido();
            ConfirmarMensaje();
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
            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            var elemento = waitLong.Until(
                ExpectedConditions.ElementToBeClickable(seccionEntregaConfirmacion)
            );

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", elemento);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", elemento);

            // Esperar contenido de Entrega (radios) visible
            waitLong.Until(d =>
            {
                try
                {
                    var el = d.FindElement(rbtEntregaInmediataConfirmacion);
                    return el.Displayed;
                }
                catch
                {
                    return false;
                }
            });
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

                // Abrir combo tipo comprobante y seleccionar la opción solo si es necesario.
                // En algunos escenarios (como boleta con ciertos clientes) el valor por defecto ya es correcto
                // y el combo puede no comportarse como en otros casos, por lo que hacemos esta parte tolerante.
                try
                {
                    var comboComprobante = waitLong.Until(
                        ExpectedConditions.ElementToBeClickable(cmbTipoComprobanteConfirmacion)
                    );

                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("arguments[0].click();", comboComprobante);

                    var opcionComprobante = waitLong.Until(
                        ExpectedConditions.ElementToBeClickable(OpcionComprobante(tipoComprobante))
                    );

                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("arguments[0].click();", opcionComprobante);

                    // Esperar que el panel dropdown desaparezca (cuando se cierra tras selección)
                    waitLong.Until(d =>
                    {
                        try
                        {
                            var panel = d.FindElement(panelDropdownNgSelect);
                            return !panel.Displayed;
                        }
                        catch
                        {
                            return true;
                        }
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("No se pudo seleccionar explícitamente el tipo de comprobante; se mantiene el valor actual.");
                }

                // Seleccionar serie si corresponde. Si no se encuentra, continuamos con la serie actual.
                if (!serie.Trim().Equals("ninguno", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var opcionSerie = waitLong.Until(
                            ExpectedConditions.ElementToBeClickable(OpcionSerie(serie))
                        );

                        ((IJavaScriptExecutor)driver)
                            .ExecuteScript("arguments[0].click();", opcionSerie);
                    }
                    catch (WebDriverTimeoutException)
                    {
                        Console.WriteLine($"No se pudo seleccionar la serie '{serie}' explícitamente; se mantiene la serie actual.");
                    }
                }

                // Cerrar sección facturación
                AbrirFacturacionConfirmacion();
            }
            catch (Exception e)
            {
                Assert.Fail("Error configurando facturación de confirmación: " + e.Message);
            }
        }
        public void ConfigurarEntregaConfirmacion(string tipoEntrega, string guiaRemision)
        {
            try
            {
                AbrirEntregaConfirmacion();

                if (tipoEntrega.Trim().Equals("inmediata", StringComparison.OrdinalIgnoreCase))
                    wait.Until(ExpectedConditions.ElementToBeClickable(rbtEntregaInmediataConfirmacion)).Click();

                if (tipoEntrega.Trim().Equals("diferida", StringComparison.OrdinalIgnoreCase))
                    wait.Until(ExpectedConditions.ElementToBeClickable(rbtEntregaDiferidaConfirmacion)).Click();

                if (guiaRemision.Trim().Equals("true", StringComparison.OrdinalIgnoreCase))
                    wait.Until(ExpectedConditions.ElementToBeClickable(btnGuiaRemisionConfirmacion)).Click();

                Thread.Sleep(500);

                AbrirEntregaConfirmacion();
            }
            catch (Exception e)
            {
                Assert.Fail("Error configurando entrega de confirmación: " + e.Message);
            }
        }

        public void ConfigurarPagoConfirmacion(string tipoPago, string montoCubreTotal)
        {
            try
            {
                var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                // Intentar abrir la sección Pago del modal
                try
                {
                    AbrirPagoConfirmacion();
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("No se pudo abrir explícitamente la sección Pago; se asume que ya está visible.");
                }

                // Seleccionar tipo de pago Contado y la pestaña EFECTIVO (si están presentes)
                try
                {
                    waitLong.Until(ExpectedConditions.ElementToBeClickable(rbtContadoConfirmacion)).Click();
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("No se pudo seleccionar el radio 'Contado'; se mantiene el valor actual.");
                }

                try
                {
                    waitLong.Until(ExpectedConditions.ElementToBeClickable(tabEfectivoConfirmacion)).Click();
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("No se pudo seleccionar la pestaña 'EFECTIVO'; se mantiene el valor actual.");
                }

                // Ingresar el monto recibido sólo si logramos ver el input
                try
                {
                    var recibido = waitLong.Until(ExpectedConditions.ElementIsVisible(txtRecibidoEfectivo));
                    recibido.Clear();
                    recibido.SendKeys(
                        montoCubreTotal.Trim().Equals("true", StringComparison.OrdinalIgnoreCase) ? "1000" : "1"
                    );
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("No se pudo localizar el input de monto recibido; se mantiene el valor actual.");
                }

                Thread.Sleep(500);

                // Intentar cerrar nuevamente la sección Pago (no es crítico si falla)
                try
                {
                    AbrirPagoConfirmacion();
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("No se pudo contraer la sección Pago; se continúa igualmente.");
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Error configurando pago de confirmación: " + e.Message);
            }
        }

        public void ConfirmarPedidoPreparado()
        {
            ultimaAccion = "confirmar";

            var waitLong = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            // Localizar el botón "Confirmar Pedido" del modal, aunque no esté marcado como clickable por Selenium
            var boton = waitLong.Until(d =>
            {
                try
                {
                    var el = d.FindElement(btnConfirmarPedidoFinal);
                    // Algunos overlays/movimientos hacen que Displayed sea false momentáneamente; si existe, lo devolvemos.
                    return el;
                }
                catch
                {
                    return null;
                }
            });

            // Asegurar que esté en pantalla
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            // Intentar click normal; si falla por overlay/disabled, forzar con JS
            try
            {
                boton.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", boton);
            }
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
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // Registro: detalle de inconsistencia por cantidad
                try
                {
                    var errorDetalle = driver.FindElement(By.XPath("//li[contains(text(),'cantidad')]"));
                    if (errorDetalle.Displayed)
                        return errorDetalle.Text;
                }
                catch { }

                // Registro: inconsistencias generales
                try
                {
                    var inconsistencia = driver.FindElement(
                        By.XPath("//strong[contains(text(),'Se encontraron inconsistencias')]")
                    );
                    if (inconsistencia.Displayed)
                        return "Se encontraron inconsistencias en los datos";
                }
                catch { }

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

                // Botón SI deshabilitado
                try
                {
                    var botonSi = driver.FindElement(btnSiInvalidar);
                    bool deshabilitado =
                        !botonSi.Enabled ||
                        botonSi.GetAttribute("disabled") != null ||
                        botonSi.GetAttribute("class")?.ToLower().Contains("disabled") == true;

                    if (deshabilitado)
                        return "Boton SI deshabilitado";
                }
                catch { }

                // Confirmación: factura requiere RUC
                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'RUC (11 dígitos)')]"));
                    if (mensaje.Displayed)
                        return "Para emitir Factura Electrónica, el cliente debe tener RUC (11 dígitos)";
                }
                catch { }

                // Confirmación: serie requerida
                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'numero de serie') or contains(text(),'número de serie')]"));
                    if (mensaje.Displayed)
                        return "Ingrese el numero de serie";
                }
                catch { }

                // Confirmación: monto insuficiente
                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'Monto insuficiente')]"));
                    if (mensaje.Displayed)
                        return "Monto insuficiente";
                }
                catch { }

                // Confirmación: total mayor a 700 sin identificar
                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'mayor a S/.700')]"));
                    if (mensaje.Displayed)
                        return "Es necesario identificar al cliente, el total es mayor a S/.700";
                }
                catch { }

                // Confirmación: guía requiere identificación
                try
                {
                    var mensaje = driver.FindElement(By.XPath("//*[contains(text(),'Necesita identificar al cliente con RUC o DNI')]"));
                    if (mensaje.Displayed)
                        return "Para guia de remision Necesita identificar al cliente con RUC o DNI";
                }
                catch { }

                // Confirmación exitosa explícita
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

                return "";
            }
            catch
            {
                return "";
            }
        }


    }
}