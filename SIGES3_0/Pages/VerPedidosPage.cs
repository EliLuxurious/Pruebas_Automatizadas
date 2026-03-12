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

        // Dropdown de familia
        private By cmbFamilia = By.XPath("//span[normalize-space()='Seleccionar familia']");

        // Dropdown de concepto
        // Buscar algo como:
        // <ng-select placeholder="Seleccionar un concepto">
        private By cmbConcepto = By.XPath("//span[@class='select-value select-value-placeholder ng-star-inserted']");

        // Campo cantidad
        // Ajustar si el input tiene name o formcontrolname
        private By txtCantidad = By.XPath("// table/tbody/tr[1]//input");


        // Checkbox IGV
        // En tu pantalla aparece "IGV" al lado del checkbox
        // Si no funciona usar:
        // //label[contains(text(),'IGV')]/preceding-sibling::input
        private By chkIGV = By.XPath("//label[normalize-space()='IGV']");


        // Checkbox DET.UNIF
        // Ajustar según texto visible
        private By chkDetUnif = By.XPath("//label[normalize-space()='DET.UNIF.']");


        // Checkbox activar descuento
        // Buscar input checkbox dentro del bloque de descuento
        private By chkDescuento = By.XPath("//label[normalize-space()='Descuento']");


        // Botón descuento ITEM
        // Texto visible en la UI: "Item"
        private By btnDescuentoItem = By.XPath("//button[normalize-space()='Item']");

        // Botón descuento GLOBAL
        private By btnDescuentoGlobal = By.XPath("//button[normalize-space()='Global']");


        // Botón modo descuento en soles
        private By btnDescuentoSoles = By.XPath("//button[normalize-space()='$']");

        // Botón modo porcentaje
        private By btnDescuentoPorcentaje = By.XPath("//button[normalize-space()='%']");


        // Campo donde se escribe el valor del descuento
        private By txtDescuento = By.XPath("//input[@placeholder='0']");


        // Campo de cliente
        // Buscar input con placeholder "Cliente"
        private By txtCliente = By.XPath("//input[@placeholder='Buscar...']");


        // Botón lupa de búsqueda
        // En tu UI tiene un icono de search
        // <button><i class="search"></i></button>
       // private By btnBuscarCliente = By.XPath("//i[@class='bi bi-search ng-star-inserted']");


        // Radio button entrega inmediata
        private By rbtEntregaInmediata = By.XPath("//label[normalize-space()='Inmediata']");


        // Radio button entrega diferida
        private By rbtEntregaDiferida = By.XPath("//label[normalize-space()='Diferida']");


        // Botón registrar pedido
        // Ajustar si tiene id o data-test
        private By btnRegistrarPedido = By.XPath("//button[normalize-space()='Registrar Pedido']");


        // Mensaje de error o alerta
        private By mensajeError = By.XPath("//div[contains(@class,'alert')]");


        // botón OK del mensaje de confirmación
        private By btnOKConfirmacion = By.XPath("//button[normalize-space()='OK']");

        //Mensaje de advertencia 
        private By mensajeAdvertencia = By.XPath("//span[contains(@class,'badge-status') and contains(@class,'danger')]");

        // mensaje de ningun producto seleccionado
        private By mensajeSinProducto = By.XPath("//span[@class='badge-status danger']");

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

        public string ObtenerResultadoSistema()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // 1️⃣ inconsistencias específicas
                try
                {
                    var errorDetalle = driver.FindElement(
                        By.XPath("//li[contains(text(),'cantidad')]")
                    );

                    if (errorDetalle.Displayed)
                    {
                        return errorDetalle.Text;
                    }
                }
                catch { }

                // 2️⃣ inconsistencias generales
                try
                {
                    var inconsistencia = driver.FindElement(
                        By.XPath("//strong[contains(text(),'Se encontraron inconsistencias')]")
                    );

                    if (inconsistencia.Displayed)
                    {
                        return "Se encontraron inconsistencias en los datos";
                    }
                }
                catch { }

                // 3️⃣ sin producto
                try
                {
                    var mensajeProducto = driver.FindElement(
                        By.XPath("//*[contains(text(),'Ningún producto seleccionado')]")
                    );

                    if (mensajeProducto.Displayed)
                    {
                        return "Ningún producto seleccionado";
                    }
                }
                catch { }

                // 4️⃣ registro exitoso
                try
                {
                    var botonOK = wait.Until(
                        SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                            By.XPath("//button[normalize-space()='OK']")
                        )
                    );

                    if (botonOK.Displayed)
                    {
                        return "el pedido se guardo correctamente";
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