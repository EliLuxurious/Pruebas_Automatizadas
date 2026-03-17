using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SIGES3_0.Pages.PedidoPage;
using System;

namespace SIGES3_0.StepDefinitions.PedidoStep
{
    [Binding]
    public class VerPedidosStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly VerPedidosPage verPedidosPage;

        

        public VerPedidosStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            verPedidosPage = new VerPedidosPage(driver);
        }

        // -------------------------
        // NAVEGACIÓN
        // ------------------------

        [When(@"el usuario accede al módulo '(.*)'")]
        public void WhenElUsuarioAccedeAlModulo(string modulo)
        {
            driver.FindElement(By.XPath($"//span[normalize-space()='{modulo}']/ancestor::a")).Click();
        }

        [When(@"el usuario accede al submodulo '(.*)'")]
        public void WhenElUsuarioAccedeAlSubmodulo(string submodulo)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var elemento = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//span[contains(text(),'{submodulo}')]")
                )
            );

            elemento.Click();
        }

        [When(@"el usuario selecciona la opci[oó]n '(.*)'")]
        public void WhenElUsuarioSeleccionaLaOpcion(string opcion)
        {
            if (opcion.Trim().Equals("Invalidar pedido", StringComparison.OrdinalIgnoreCase))
            {
                verPedidosPage.SeleccionarInvalidarPedido();
                return;
            }

            if (opcion.Trim().Equals("Confirmar pedido", StringComparison.OrdinalIgnoreCase))
            {
                verPedidosPage.SeleccionarConfirmarPedido();
                return;
            }

            verPedidosPage.SeleccionarOpcion(opcion);
        }



        // -------------------------
        // PRODUCTO
        // -------------------------

        [When(@"el usuario selecciona la familia '(.*)'")]
        public void WhenElUsuarioSeleccionaLaFamilia(string familia)
        {
            verPedidosPage.SeleccionarFamilia(familia);
        }

        [When(@"el usuario selecciona el concepto '(.*)'")]
        public void WhenElUsuarioSeleccionaElConcepto(string concepto)
        {
            verPedidosPage.SeleccionarConcepto(concepto);
        }

        [When(@"el usuario ingresa la cantidad '(.*)'")]
        public void WhenElUsuarioIngresaLaCantidad(string cantidad)
        {
            verPedidosPage.IngresarCantidad(cantidad);
        }

        // -------------------------
        // OPCIONES
        // -------------------------

        [When(@"el usuario activa IGV '(.*)'")]
        public void WhenElUsuarioActivaIGV(string igv)
        {
            verPedidosPage.ActivarIGV(igv);
        }

        [When(@"el usuario activa DET.UNIF '(.*)'")]
        public void WhenElUsuarioActivaDETUNIF(string detUnif)
        {
            verPedidosPage.ActivarDetUnif(detUnif);
        }

        // -------------------------
        // DESCUENTO
        // -------------------------

        [When(@"el usuario configura descuento '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraDescuento(string activo, string tipo, string modo, string valor)
        {
            verPedidosPage.ConfigurarDescuento(activo, tipo, modo, valor);
        }

        // -------------------------
        // CLIENTE
        // -------------------------

        [When(@"el usuario abre la sección '(.*)'")]
        public void WhenElUsuarioAbreLaSeccion(string seccion)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            var elemento = wait.Until(d =>
                d.FindElement(By.XPath($"//*[contains(text(),'{seccion}')]/ancestor::button"))
            );

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", elemento);

            Thread.Sleep(500);

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", elemento);
        }


        [When(@"el usuario busca el cliente '(.*)'")]
        public void WhenElUsuarioBuscaElCliente(string cliente)
        {
            verPedidosPage.BuscarCliente(cliente);
        }

        // -------------------------
        // ENTREGA
        // -------------------------

        [When(@"el usuario selecciona tipo de entrega '(.*)'")]
        public void WhenElUsuarioSeleccionaTipoDeEntrega(string tipoEntrega)
        {
            verPedidosPage.SeleccionarEntrega(tipoEntrega);
        }

        // -------------------------
        // REGISTRO
        // -------------------------

        [When(@"el usuario registra el pedido")]
        public void WhenElUsuarioRegistraElPedido()
        {
            verPedidosPage.RegistrarPedido();
        }

        // Validar q exista un pedido en estado registrado 
        [Given(@"existe un pedido en estado registrado para invalidar")]
        public void GivenExisteUnPedidoEnEstadoRegistradoParaInvalidar()
        {
            verPedidosPage.AsegurarPedidoRegistradoParaInvalidar();
        }


        // -------------------------
        // INVALIDACIÓN
        // -------------------------

        [When(@"el usuario ingresa el motivo '(.*)'")]
        public void WhenElUsuarioIngresaElMotivo(string motivo)
        {
            verPedidosPage.IngresarMotivoInvalidacion(motivo);
        }

        [When(@"el usuario confirma '(.*)'")]
        public void WhenElUsuarioConfirma(string accion)
        {
            verPedidosPage.ConfirmarInvalidacion(accion);
        }


        //CONFIRMAR PEDIDO
        [Given(@"existe un pedido base registrado para confirmar con total mayor a 700 '(.*)'")]
        public void GivenExisteUnPedidoBaseRegistradoParaConfirmarConTotalMayorA700(string totalMayor700)
        {
            verPedidosPage.AsegurarPedidoBaseParaConfirmar(totalMayor700);
        }

        [When(@"el usuario configura la facturacion '(.*)' '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraLaFacturacion(string tipoComprobante, string serie, string cliente)
        {
            verPedidosPage.ConfigurarFacturacionConfirmacion(tipoComprobante, serie, cliente);
        }

        [When(@"el usuario configura la entrega '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraLaEntrega(string tipoEntrega, string guiaRemision)
        {
            verPedidosPage.ConfigurarEntregaConfirmacion(tipoEntrega, guiaRemision);
        }

        [When(@"el usuario configura el pago '(.*)' '(.*)'")]
        public void WhenElUsuarioConfiguraElPago(string tipoPago, string montoCubreTotal)
        {
            verPedidosPage.ConfigurarPagoConfirmacion(tipoPago, montoCubreTotal);
        }

        [When(@"el usuario confirma el pedido preparado")]
        public void WhenElUsuarioConfirmaElPedidoPreparado()
        {
            verPedidosPage.ConfirmarPedidoPreparado();
        }

        [Then(@"el sistema valida '(.*)'")]
        public void ThenElSistemaValida(string resultadoEsperado)
        {
            string resultado = verPedidosPage.ObtenerResultadoSistema();

            Assert.IsTrue(
                resultado.ToLower().Contains(resultadoEsperado.ToLower()),
                $"Resultado esperado: {resultadoEsperado}. Resultado obtenido: {resultado}"
            );
        }


    }
}
