using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.Adquisicion;
using SIGES3_0.Pages.CotizacionPage;
using SIGES3_0.Pages.PedidoPage;
using System;
using System.Linq;
using System.Threading;

namespace SIGES3_0.StepDefinitions.CotizacionStep
{
    [Binding]
    public class CotizacionStep
    {
        private readonly CotizacionPage cotizacionPage;
        private readonly VerPedidosPage verPedidosPage;
        private readonly IWebDriver driver;

        public CotizacionStep(IWebDriver driver)
        {
            this.driver = driver;
            cotizacionPage = new CotizacionPage(driver);
            verPedidosPage = new VerPedidosPage(driver);
        }

        // ════════════════════════════════════════════════════════════════════════════════
        // 1. GIVEN: PREPARACIÓN DE COTIZACIÓN
        // ════════════════════════════════════════════════════════════════════════════════
        [Given(@"existe una cotizacion editable con familia '(.*)' concepto '(.*)' cantidad '(.*)' cliente '(.*)' fecha '(.*)'")]
        public void GivenExisteUnaCotizacionEditable(string familia, string concepto, string cantidad, string cliente, string fecha)
        {
            if (!cotizacionPage.ExisteCotizacionParaEditar())
            {
                GenerarCotizacionGarantizandoStock(familia, concepto, cantidad, cliente, fecha);
            }
            cotizacionPage.AsegurarCotizacionEditable();
        }

        private void GenerarCotizacionGarantizandoStock(string familia, string concepto, string cantidad, string cliente, string fecha)
        {
            verPedidosPage.SeleccionarOpcion("Nueva Cotización");

            bool preparado = verPedidosPage.IntentarSeleccionarProductoYCantidad(familia, concepto, cantidad, false);

            if (!preparado)
            {
                Console.WriteLine($"[INFO] Sin stock para Cotización base ({concepto}). Comprando...");

                string cantidadAComprar = CalcularCantidadCompra(concepto);
                ComprarProductoEnAdquisicion(concepto, cantidadAComprar);

                verPedidosPage.SeleccionarOpcion("Cotización");
                verPedidosPage.SeleccionarOpcion("Nueva Cotización");
                verPedidosPage.IntentarSeleccionarProductoYCantidad(familia, concepto, cantidad, false);
            }

            verPedidosPage.ActivarIGV("false");
            verPedidosPage.ConfigurarDescuento("false", "NA", "NA", "0");
            verPedidosPage.BuscarCliente(cliente);
            cotizacionPage.IngresarFechaFinal(fecha);
            cotizacionPage.RegistrarCotizacion();

            try { verPedidosPage.ConfirmarMensaje(); } catch { }
            try { verPedidosPage.SeleccionarOpcion("Cotización"); } catch { }
        }

        // ════════════════════════════════════════════════════════════════════════════════
        // 2. WHEN: PASO INTELIGENTE PARA EL REGISTRO
        // ════════════════════════════════════════════════════════════════════════════════
        [When(@"el usuario prepara producto para cotizacion con familia '(.*)' concepto '(.*)' cantidad '(.*)' resultado '(.*)'")]
        public void WhenElUsuarioPreparaProductoParaCotizacion(string familia, string concepto, string cantidad, string resultadoEsperado)
        {
            if (familia.Equals("ninguno", StringComparison.OrdinalIgnoreCase)) return;

            bool esperaErrorStock = resultadoEsperado.Contains("menor al stock", StringComparison.OrdinalIgnoreCase);

            bool productoPreparado = verPedidosPage.IntentarSeleccionarProductoYCantidad(familia, concepto, cantidad, esperaErrorStock);

            if (productoPreparado) return;

            Console.WriteLine($"[INFO] Stock insuficiente detectado. Iniciando compra de emergencia...");

            string cantidadAComprar = CalcularCantidadCompra(concepto);
            ComprarProductoEnAdquisicion(concepto, cantidadAComprar);

            verPedidosPage.SeleccionarOpcion("Cotización");
            verPedidosPage.SeleccionarOpcion("Nueva Cotización");
            verPedidosPage.IntentarSeleccionarProductoYCantidad(familia, concepto, cantidad, esperaErrorStock);
        }

        // ════════════════════════════════════════════════════════════════════════════════
        // 3. MÉTODOS AUXILIARES LOCALES (COMPRA MASIVA)
        // ════════════════════════════════════════════════════════════════════════════════
        private string CalcularCantidadCompra(string concepto)
        {
            return concepto switch
            {
                "7753234003320" => "500", // Coca-Cola
                "7753234003313" => "500", // Inca Kola
                "7751234001115" => "400", // Azúcar Rubia
                _ => "500" // Por defecto
            };
        }

        private void ComprarProductoEnAdquisicion(string concepto, string cantidad)
        {
            var nuevaAdquisicionPage = new NuevaAdquisicionPage(driver);
            nuevaAdquisicionPage.NavegarANuevaAdquisicion();

            string correlativo = DateTime.Now.ToString("HHmmss");
            string productoAdquisicion = concepto switch
            {
                "7753234003320" => "7753234003320|Coca-Cola Gaseosa Botella 1.5L",
                "7753234003313" => "7753234003313|Inca Kola Gaseosa Botella 1.5L",
                "7751234001115" => "7751234001115|Azúcar Rubia",
                _ => concepto
            };
            string valorUnitario = concepto switch { "7753234003320" => "6.9", "7753234003313" => "7.1", "7751234001115" => "3.2", _ => "6.9" };

            nuevaAdquisicionPage.ConfigurarDatosFacturacion("FACTURA ELECTRONICA", "F001", correlativo, DateTime.Now.ToString("dd/MM/yyyy"), "10759012017", "Stock automático");

            try
            {
                var inputProv = driver.FindElement(By.XPath("//input[contains(@placeholder, 'Proveedor') or @id='proveedor']"));
                inputProv.Click(); // Aseguramos el foco primero
                Thread.Sleep(500);
                inputProv.SendKeys(Keys.Enter);
                Thread.Sleep(1500);
            }
            catch { }

            nuevaAdquisicionPage.SeleccionarTipoEntrega("Inmediata");
            nuevaAdquisicionPage.ConfigurarDatosEntrega("Item Comercial", "RECSA - CENTRAL", "CENTRO COMERCIAL CENTRAL");
            nuevaAdquisicionPage.AgregarProducto(productoAdquisicion, cantidad, valorUnitario);
            nuevaAdquisicionPage.AbrirSeccionPago();
            nuevaAdquisicionPage.SeleccionarTipoPago("Contado");
            nuevaAdquisicionPage.ConfigurarMedioPago("Efectivo", "NINGUNO");
            nuevaAdquisicionPage.ClicGuardarAdquisicion("SavePurchase");

            try { nuevaAdquisicionPage.ObtenerMensajeConfirmacion(); } catch { }
        }

        // ════════════════════════════════════════════════════════════════════════════════
        // 4. PASOS EXCLUSIVOS DE COTIZACIÓN
        // ════════════════════════════════════════════════════════════════════════════════
        [When(@"el usuario hace clic en el icono pregenerar venta")]
        public void WhenElUsuarioHaceClicEnElIconoPregenerarVenta() => cotizacionPage.SeleccionarPregenerarVenta();

        
        [When(@"el usuario selecciona editar la cotizacion")]
        public void WhenElUsuarioSeleccionaEditarLaCotizacion() => cotizacionPage.SeleccionarEditarCotizacion();

        [When(@"el usuario ingresa la fecha final '(.*)'")]
        public void WhenElUsuarioIngresaLaFechaFinal(string fecha) => cotizacionPage.IngresarFechaFinal(fecha);

        [When(@"el usuario registra la cotizacion")]
        public void WhenElUsuarioRegistraLaCotizacion()
        {
            if (cotizacionPage.FechaPasadaIntentada) return;
            cotizacionPage.RegistrarCotizacion();
        }

        [When(@"el usuario actualiza la cotizacion")]
        public void WhenElUsuarioActualizaLaCotizacion() => cotizacionPage.ActualizarCotizacion();

        [Then(@"el sistema valida el resultado de la cotizacion '(.*)'")]
        public void ThenElSistemaValidaElResultadoDeLaCotizacion(string resultadoEsperado)
        {
            string resultado = cotizacionPage.ObtenerResultadoSistema()?.Trim() ?? string.Empty;
            string esperado = resultadoEsperado?.Trim() ?? string.Empty;

            Assert.That(resultado, Is.Not.Empty, "El sistema no devolvió ningún mensaje.");

            Assert.That(NormalizarTexto(resultado), Does.Contain(NormalizarTexto(esperado)),
                $"Resultado esperado: {resultadoEsperado}. Resultado obtenido: {resultado}");
        }

        private static string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return string.Empty;
            var normalized = texto.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();
            foreach (var c in normalized)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return System.Text.RegularExpressions.Regex.Replace(sb.ToString().Normalize(System.Text.NormalizationForm.FormC), @"\s+", " ").Trim().ToLower();
        }
    }
}