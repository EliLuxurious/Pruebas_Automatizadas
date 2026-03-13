using SIGES3_0.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SIGES3_0.Pages
{
    public class VerPedidosPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        //-------------------------
        private int filaSeleccionadaParaInvalidar = -1;


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
        private By cmbConcepto = By.XPath("//span[@class='select-value select-value-placeholder ng-star-inserted']");
        private By txtCantidad = By.XPath("// table/tbody/tr[1]//input");
        private By chkIGV = By.XPath("//label[normalize-space()='IGV']");
        private By chkDetUnif = By.XPath("//label[normalize-space()='DET.UNIF.']");
        private By chkDescuento = By.XPath("//label[normalize-space()='Descuento']");
        private By btnDescuentoItem = By.XPath("//button[normalize-space()='Item']");
        private By btnDescuentoGlobal = By.XPath("//button[normalize-space()='Global']");
        private By btnDescuentoSoles = By.XPath("//button[normalize-space()='$']");
        private By btnDescuentoPorcentaje = By.XPath("//button[normalize-space()='%']");
        private By txtDescuento = By.XPath("//input[@placeholder='0']");
        private By txtCliente = By.XPath("//input[@placeholder='Buscar...']");
        private By rbtEntregaInmediata = By.XPath("//label[normalize-space()='Inmediata']");
        private By rbtEntregaDiferida = By.XPath("//label[normalize-space()='Diferida']");
        private By btnRegistrarPedido = By.XPath("//button[normalize-space()='Registrar Pedido']");
        private By mensajeError = By.XPath("//div[contains(@class,'alert')]");
        private By btnOKConfirmacion = By.XPath("//button[normalize-space()='OK']");
        private By mensajeAdvertencia = By.XPath("//span[contains(@class,'badge-status') and contains(@class,'danger')]");
        private By mensajeSinProducto = By.XPath("//span[@class='badge-status danger']");

        // INVALIDAR PEDIDO
        private By txtFiltroEstado = By.XPath("//th[8]//input[1]");
        private By btnInvalidarPrimerRegistro = By.XPath("//tbody/tr[1]/td[9]/div[1]/button[2]/i[1]");

        private By txtMotivoInvalidacion = By.XPath("//textarea[@placeholder='Ingrese el motivo de la anulación...']");
        private By btnSiInvalidar = By.XPath("//button[normalize-space()='Sí']");
        private By btnNoInvalidar = By.XPath("//button[normalize-space()='No']");

        // ======================================================
        // METODOS
        // ======================================================

        public void SeleccionarOpcion(string opcion)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            var boton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//*[contains(text(),'{opcion}')]")
                )
            );

            boton.Click();
        }

        public void SeleccionarFamilia(string familia)
        {
            if (familia == "ninguno") return;

            // Abrir dropdown familia
            var dropdown = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(cmbFamilia)
            );
            dropdown.Click();

            // Esperar opción dentro del dropdown
            var opcion = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//span[normalize-space()='{familia}']")
                )
            );

            opcion.Click();
        }

        public void SeleccionarConcepto(string concepto)
        {
            if (concepto == "ninguno") return;

            // abrir dropdown concepto
            var dropdown = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(cmbConcepto)
            );

            dropdown.Click();

            // seleccionar opción por código
            var opcion = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//span[contains(text(),'{concepto}')]")
                )
            );

            opcion.Click();
        }

        public void IngresarCantidad(string cantidad)
        {
            if (cantidad == "0") return;

            var input = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(txtCantidad)
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

                var input = wait.Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(txtCliente)
                );

                // limpiar campo
                input.Clear();

                // escribir DNI o RUC
                input.SendKeys(cliente);

                // presionar ENTER
                input.SendKeys(Keys.Enter);

                // esperar que el campo tenga valor (cliente cargado)
                wait.Until(d => input.GetAttribute("value").Contains(cliente));

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
            var boton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(btnRegistrarPedido)
            );

            // Scroll al botón
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);

            Thread.Sleep(800);

            // Click con JavaScript (evita interceptación)
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", boton);
        }

        // -------------------------
        // INVALIDAR PEDIDO
        // -------------------------

        public void SeleccionarInvalidarPedido()
        {
            try
            {
                var filtroEstado = wait.Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(txtFiltroEstado)
                );

                filtroEstado.Clear();
                filtroEstado.SendKeys("REGISTRADO");

                Thread.Sleep(1000);

                var botonInvalidar = wait.Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(btnInvalidarPrimerRegistro)
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
        }

        public void ConfirmarInvalidacion(string accion)
        {
            if (accion.Trim().Equals("SI", StringComparison.OrdinalIgnoreCase) ||
                accion.Trim().Equals("Sí", StringComparison.OrdinalIgnoreCase) ||
                accion.Trim().Equals("Si", StringComparison.OrdinalIgnoreCase))
            {
                var botonSi = wait.Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(btnSiInvalidar)
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

                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].click();", botonSi);

                return;
            }

            if (accion.Trim().Equals("NO", StringComparison.OrdinalIgnoreCase))
            {
                var botonNo = wait.Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(btnNoInvalidar)
                );

                botonNo.Click();
            }
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

                
                // invalidación exitosa
                try
                {
                    WebDriverWait waitEstado = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                    bool invalidado = waitEstado.Until(d =>
                    {
                        var estado = d.FindElement(By.XPath("//tbody/tr[1]/td[8]")).Text.Trim().ToUpper();
                        return estado == "INVALIDADO";
                    });

                    if (invalidado)
                        return "el pedido se Invalido correctamente";
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

                // Registro exitoso
                try
                {
                    var botonOK = wait.Until(
                        SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(btnOKConfirmacion)
                    );

                    if (botonOK.Displayed)
                        return "el pedido se guardo correctamente";
                }
                catch { }

                return "";
            }
            catch
            {
                return "";
            }
        }

        public void ConfirmarMensaje()
        {
            var boton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(btnOKConfirmacion)
            );

            boton.Click();
        }

        public string ObtenerMensajeError()
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(mensajeError)).Text;
        }


    }
}