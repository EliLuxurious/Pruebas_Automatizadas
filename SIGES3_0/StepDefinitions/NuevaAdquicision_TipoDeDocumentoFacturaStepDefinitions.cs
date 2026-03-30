using Reqnroll;
using OpenQA.Selenium;
using SIGES3_0.Pages;
using System;
using OpenQA.Selenium.Support.UI;

namespace SIGES3_0.StepDefinitions
{
    [Binding]
    public class NuevaAdquicision_TipoDeDocumentoFacturaStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly NuevaAdquisicionPage nuevaAdquisicionPage;

        // Constructor para inyectar el WebDriver e inicializar el PageObject
        public NuevaAdquicision_TipoDeDocumentoFacturaStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            nuevaAdquisicionPage = new NuevaAdquisicionPage(driver);
        }

        // ===================== GIVEN (Inicio de Sesión) =====================

        [Given(@"Inicio de sesión en el módulo de Adquisición con usuario '(.*)' y contraseña '(.*)' en '(.*)'")]
        public void GivenInicioDeSesionEnAdquisicion(string usuario, string password, string url)
        {
            // 1. Navegar
            nuevaAdquisicionPage.OpenToApplication(url);

            // 2. Login (Asegúrate que este método en el Page tenga el WebDriverWait para el logo)
            nuevaAdquisicionPage.LoginToApplication(usuario, password);
        }
        // ===================== GIVEN (Navegación Única) =====================

        [Given(@"Navego al módulo de 'Adquisición'")]
        public void GivenNavegoAlModuloDeAdquisicion()
        {
            // Llamamos al método que arreglamos, el cual gestiona ambos clics (módulo y submódulo)
            // de forma segura esperando a que termine la animación.
            nuevaAdquisicionPage.NavegarANuevaAdquisicion();
        }

        [Given(@"Entro al submódulo específico de 'Nueva Adquisición'")]
        public void GivenEntroAlSubmoduloEspecificoDeNuevaAdquisicion()
        {
            // Lo dejamos vacío intencionalmente porque el clic ya se realizó en el paso anterior.
            // Sirve para mantener la lectura natural y semántica en el archivo .feature.
        }
        // ===================== WHEN =====================

        [When("Se configuran los datos de {string}:")]
        public void WhenSeConfiguranLosDatosDe(string seccion, DataTable dataTable)
        {
            switch (seccion.ToLower())
            {
                case "facturación":
                    string documento = "", serie = "", correlativo = "", fechaEmision = "", proveedor = "", infoAdicional = "";

                    foreach (var row in dataTable.Rows)
                    {
                        string campo = row["Campo"].Trim();
                        string valor = row["Valor"].Trim();

                        if (campo.Equals("Documento", StringComparison.OrdinalIgnoreCase)) documento = valor;
                        else if (campo.Equals("Serie", StringComparison.OrdinalIgnoreCase)) serie = valor;
                        else if (campo.Equals("Correlativo", StringComparison.OrdinalIgnoreCase)) correlativo = valor;
                        else if (campo.Equals("Fecha de emisión", StringComparison.OrdinalIgnoreCase)) fechaEmision = valor;
                        else if (campo.Equals("Proveedor", StringComparison.OrdinalIgnoreCase)) proveedor = valor;
                        else if (campo.Equals("Información Adicional", StringComparison.OrdinalIgnoreCase)) infoAdicional = valor;
                    }

                    nuevaAdquisicionPage.ConfigurarDatosFacturacion(documento, serie, correlativo, fechaEmision, proveedor, infoAdicional);
                    break;

                case "entrega":
                    string rol = "", establecimiento = "", almacen = "";

                    foreach (var row in dataTable.Rows)
                    {
                        string campo = row["Campo"].Trim();
                        string valor = row["Valor"].Trim();

                        if (campo.Equals("Rol", StringComparison.OrdinalIgnoreCase)) rol = valor;
                        else if (campo.Equals("Establecimiento", StringComparison.OrdinalIgnoreCase)) establecimiento = valor;
                        else if (campo.Equals("Almacén", StringComparison.OrdinalIgnoreCase)) almacen = valor;
                    }

                    nuevaAdquisicionPage.ConfigurarDatosEntrega(rol, establecimiento, almacen);
                    break;

                case "pago":
                    string tipoPago = "", metodoPago = "", observacionPago = "", codigoTx = "", billeteraName = "", tarjetaName = "", cuentaPropia = "", cuentaProveedor = "", caja ="", montoInicial = "", cuotas ="", frecuencia = "";

                    foreach (var row in dataTable.Rows)
                    {
                        string campo = row["Campo"].Trim();
                        string valor = row["Valor"].Trim();

                        if (campo.Equals("Tipo", StringComparison.OrdinalIgnoreCase)) tipoPago = valor;
                        else if (campo.Equals("Método", StringComparison.OrdinalIgnoreCase)) metodoPago = valor;
                        // Mapeamos 'Información' u 'Observación' al mismo campo de texto
                        else if (campo.Equals("Observación", StringComparison.OrdinalIgnoreCase) || campo.Equals("Información", StringComparison.OrdinalIgnoreCase)) observacionPago = valor;
                        else if (campo.Equals("Código", StringComparison.OrdinalIgnoreCase)) codigoTx = valor;
                        else if (campo.Equals("Billetera", StringComparison.OrdinalIgnoreCase)) billeteraName = valor;
                        else if (campo.Equals("Tarjeta", StringComparison.OrdinalIgnoreCase)) tarjetaName = valor;
                        else if (campo.Contains("Bancaria Propia")) cuentaPropia = valor;
                        else if (campo.Contains("proveedor")) cuentaProveedor = valor;
                        else if (campo.Equals("Caja", StringComparison.OrdinalIgnoreCase)) caja = valor;

                        // Dentro del switch de "pago" en el Step Definition:
                        else if (campo.Equals("Monto Inicial", StringComparison.OrdinalIgnoreCase)) montoInicial = valor;
                        else if (campo.Equals("Cuotas", StringComparison.OrdinalIgnoreCase)) cuotas = valor;
                        else if (campo.Equals("Frecuencia (Días)", StringComparison.OrdinalIgnoreCase)) frecuencia = valor;
                    }

                    nuevaAdquisicionPage.AbrirSeccionPago();
                    nuevaAdquisicionPage.SeleccionarTipoPago(tipoPago);

                    // Pasamos todos los parámetros al método del Page
                    nuevaAdquisicionPage.ConfigurarMedioPago(metodoPago, observacionPago, codigoTx, billeteraName, tarjetaName, cuentaPropia, cuentaProveedor, caja);
                    break;

                default:
                    throw new ArgumentException($"La sección '{seccion}' no está soportada.");


            }
        }

        [When("Se selecciona el tipo de entrega {string}")]
        public void WhenSeSeleccionaElTipoDeEntrega(string tipoEntrega)
        {
            nuevaAdquisicionPage.SeleccionarTipoEntrega(tipoEntrega);
        }

        [When(@"Se selecciona el tipo de compra '(.*)'")]
        public void WhenSeSeleccionaElTipoDeCompra(string tipoCompra)
        {
            // Llamamos al método de la página que crearemos a continuación
            nuevaAdquisicionPage.SeleccionarTipoCompra(tipoCompra);
        }

        [When(@"Se selecciona y configura el producto a adquirir:")]
        public void WhenSeSeleccionaYConfiguraElProductoAAdquirir(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                string producto = row["Producto"];
                string cantidad = row["Cantidad"];
                string valorUnitario = row["V. U"];

                if (dataTable.Header.Contains("Rol") && dataTable.Header.Contains("Almacén"))
                {
                    string rol = row["Rol"];
                    string almacen = row["Almacén"];
                    nuevaAdquisicionPage.AgregarProductoConAlmacenVarios(producto, rol, almacen, cantidad, valorUnitario);
                }
                else
                {
                    nuevaAdquisicionPage.AgregarProducto(producto, cantidad, valorUnitario);
                }
            }
        }
        // ===================== THEN =====================

        [Then("Se procede a guardar la adquisición mediante la acción {string}")]
        public void ThenSeProcedeAGuardarLaAdquisicionMedianteLaAccion(string accionGuardar)
        {
            nuevaAdquisicionPage.ClicGuardarAdquisicion(accionGuardar);
        }

        [Then("Se confirma el registro exitoso con el mensaje {string}")]
        public void ThenSeConfirmaElRegistroExitosoConElMensaje(string mensajeEsperado)
        {
            string mensajeActual = nuevaAdquisicionPage.ObtenerMensajeConfirmacion();

            if (!mensajeActual.Contains(mensajeEsperado))
            {
                throw new Exception($"El mensaje de éxito no coincide. Esperado: '{mensajeEsperado}', Actual: '{mensajeActual}'");
            }
        }

        [When(@"Se activa la opción de '(.*)'")]
        public void WhenSeActivaLaOpcionDe(string opcion)
        {
            if (opcion.Equals("Varios", StringComparison.OrdinalIgnoreCase))
            {
                nuevaAdquisicionPage.ActivarVariosAlmacenes(true);
            }
        }

        [When(@"Se habilita la sección de descuento")]
        public void WhenSeHabilitaLaSeccionDeDescuentos()
        {
            // Este método debe darle clic al checkbox de la derecha (arriba de la tabla)
            nuevaAdquisicionPage.HabilitarDescuentoPorItem(true);
        }

        [When(@"Se selecciona y configura el producto con descuento por item:")]
        public void WhenSeSeleccionaYConfiguraElProductoConDescuentoPorItem(Table table)
        {
            // 1. Primero habilitamos la columna de descuento (el checkbox que vimos en el video)
            nuevaAdquisicionPage.HabilitarDescuentoPorItem(true);

            foreach (var row in table.Rows)
            {
                string producto = row["Producto"];
                string cantidad = row["Cantidad"];
                string precio = row["V. U"];
                string descuento = row["Descuento"];

                // Usamos el método que ya tienes para agregar el producto base
                nuevaAdquisicionPage.AgregarProducto(producto, cantidad, precio);

                nuevaAdquisicionPage.ConfigurarDescuentoEnFila(descuento);
            }
        }
        [When(@"Se habilita la sección de descuento:")]
        public void WhenSeHabilitaLaSeccionDeDescuento(Reqnroll.DataTable dataTable)
        {
            string montoDescuento = dataTable.Rows[0]["Descuento"];

            nuevaAdquisicionPage.HabilitarDescuentoPorItem(true);

            nuevaAdquisicionPage.AplicarDescuentoGlobal(montoDescuento);
        }

        [Then(@"El sistema debe mostrar la alerta de validacion '(.*)'")]
        public void ThenElSistemaDebeMostrarLaAlertaDeValidacion(string mensajeEsperado)
        {
            string mensajeActual = nuevaAdquisicionPage.ObtenerMensajeDeValidacion();

            if (mensajeActual == "SIN_ALERTA")
            {
                NUnit.Framework.Assert.Fail($"❌ BUG DETECTADO: El sistema permitió la acción. Se esperaba que mostrara la validación: '{mensajeEsperado}'.");
            }

            NUnit.Framework.Assert.IsTrue(mensajeActual.Contains(mensajeEsperado, StringComparison.OrdinalIgnoreCase),
                $"El sistema mostró una alerta, pero no era la esperada. \nEsperaba: {mensajeEsperado} \nMostró: {mensajeActual}");
        }

    }
}