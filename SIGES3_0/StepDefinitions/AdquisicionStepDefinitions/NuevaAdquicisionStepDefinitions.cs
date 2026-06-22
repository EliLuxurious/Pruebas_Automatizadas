using Reqnroll;
using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using SIGES3_0.Pages.Items;
using SIGES3_0.Pages.Items.NewItem;
using SIGES3_0.Pages.Adquisicion;

namespace SIGES3_0.StepDefinitions.AdquisicionStepDefinitions
{
    [Binding]
    public class NuevaAdquicisionStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly NuevaAdquisicionPage nuevaAdquisicionPage;
        private readonly NewItemsPage conceptosPage;

        public NuevaAdquicisionStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            nuevaAdquisicionPage = new NuevaAdquisicionPage(driver);
            conceptosPage = new NewItemsPage(driver);
        }


        [Given(@"Inicio de sesión en el módulo de Adquisición con usuario '(.*)' y contraseña '(.*)' en '(.*)'")]
        public void GivenInicioDeSesionEnAdquisicion(string usuario, string password, string url)
        {
            
            nuevaAdquisicionPage.OpenToApplication(url);

           
            nuevaAdquisicionPage.LoginToApplication(usuario, password);
        }
      

        [Given(@"Navego al módulo de 'Adquisición'")]
        public void GivenNavegoAlModuloDeAdquisicion()
        {
            
            nuevaAdquisicionPage.NavegarANuevaAdquisicion();
        }

        [Given(@"Entro al submódulo específico de 'Nueva Adquisición'")]
        public void GivenEntroAlSubmoduloEspecificoDeNuevaAdquisicion()
        {

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
                    string tipoPago = "", metodoPago = "", observacionPago = "", codigoTx = "", billeteraName = "", tarjetaName = "", cuentaPropia = "", cuentaProveedor = "", caja = "", montoInicial = "", cuotas = "", frecuencia = "";
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
                    nuevaAdquisicionPage.SeleccionarTipoPago(tipoPago, montoInicial, cuotas, frecuencia);
                    //nuevaAdquisicionPage.ConfigurarMedioPago(metodoPago, observacionPago, codigoTx, billeteraName, tarjetaName, cuentaPropia, cuentaProveedor, caja);
                    nuevaAdquisicionPage.ConfigurarMedioPago(
                    medio: metodoPago,
                    observacion: observacionPago,
                    codigo: codigoTx,
                    billetera: billeteraName,
                    tarjetaName: tarjetaName,
                    cuentaPropia: cuentaPropia,
                    cuentaProveedor: cuentaProveedor,
                    caja: caja
                );

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

        // ===================== STEPS DEL MODAL DE NUEVO CONCEPTO =====================

        [When(@"Se abre el modal de '(.*)' en la sección de productos")]
        public void WhenSeAbreElModalDeEnLaSeccionDeProductos(string nombreModal)
        {
            nuevaAdquisicionPage.AbrirModalNuevoConcepto();
        }

        [When(@"Se registran los datos del nuevo concepto en el modal:")]
        public void WhenSeRegistranLosDatosDelNuevoConceptoEnElModal(DataTable dataTable)
        {
            var row = dataTable.Rows[0];

            conceptosPage.SeleccionarFamilia(row["familia"]);
            conceptosPage.IngresarCodigoDeBarra(row["codigo"]);
            conceptosPage.AgregarSufijo(row["sufijo"]);
            conceptosPage.SeleccionarMarca(row["marca"]);
            conceptosPage.SeleccionarPresentacion(row["presentacion"]);
            conceptosPage.SeleccionarTarifa(row["tarifa"]);
            conceptosPage.IngresarPrecio(row["precio"]);
        }

        [When(@"Se guarda el concepto desde el modal")]
        public void WhenSeGuardaElConceptoDesdeElModal()
        {
            conceptosPage.GuardarConcepto();
            System.Threading.Thread.Sleep(4000);
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
        public void WhenSeHabilitaLaSeccionDeDescuento(DataTable dataTable)
        {
            string montoDescuento = dataTable.Rows[0]["Descuento"];

            nuevaAdquisicionPage.HabilitarDescuentoPorItem(true);

            nuevaAdquisicionPage.AplicarDescuentoGlobal(montoDescuento);
        }
        //MULTIPAGOS y VARIOS ALMACENES

        [When(@"Se activa la opción de '(.*)'")]
        public void WhenSeActivaLaOpcionDe(string opcion)
        {
            if (opcion.Equals("Varios", StringComparison.OrdinalIgnoreCase))
            {
                nuevaAdquisicionPage.ActivarVariosAlmacenes(true);
            }
            else if (opcion.Equals("Multipago", StringComparison.OrdinalIgnoreCase))
            {
                // Llamamos a tu nuevo método dedicado 
                nuevaAdquisicionPage.ActivarMultipago(true);
            }
        }

        [When(@"Se agregan los siguientes medios de pago fraccionados:")]
        public void WhenSeAgreganLosSiguientesMediosDePagoFraccionados(Table table)
        {
            foreach (var row in table.Rows)
            {
                // Base
                string metodo = row.ContainsKey("Método") ? row["Método"] : "";
                string monto = row.ContainsKey("Monto") ? row["Monto"] : "";
                string observacion = row.ContainsKey("Observación") ? row["Observación"] : "";

                // Tarjetas
                string tarjeta = row.ContainsKey("Tarjeta") ? row["Tarjeta"] : "";

                // Billetera Digital
                string billetera = row.ContainsKey("Billetera") ? row["Billetera"] : "";
                string codigo = row.ContainsKey("Código") ? row["Código"] : "";

                // Transferencia y Depósito
                string cuentaPropia = row.ContainsKey("Cuenta Propia") ? row["Cuenta Propia"] : "";
                string cuentaProveedor = row.ContainsKey("Cuenta Proveedor") ? row["Cuenta Proveedor"] : "";
                string caja = row.ContainsKey("Caja") ? row["Caja"] : "";

                nuevaAdquisicionPage.ConfigurarMedioPago(
                    medio: metodo,
                    observacion: observacion,
                    monto: monto,
                    codigo: codigo,
                    billetera: billetera,
                    tarjetaName: tarjeta,
                    cuentaPropia: cuentaPropia,
                    cuentaProveedor: cuentaProveedor,
                    caja: caja
                );

                nuevaAdquisicionPage.AgregarPagoGrid();
            }
        }

        /*[Then(@"El sistema debe mostrar la alerta de validacion '(.*)'")]
        public void ThenElSistemaDebeMostrarLaAlertaDeValidacion(string mensajeEsperado)
        {
            string mensajeActual = nuevaAdquisicionPage.ObtenerMensajeDeValidacion();

            if (mensajeActual == "SIN_ALERTA")
            {
                Assert.Fail($"❌ BUG DETECTADO: El sistema permitió la acción. Se esperaba que mostrara la validación: '{mensajeEsperado}'.");
            }

            Assert.IsTrue(mensajeActual.Contains(mensajeEsperado, StringComparison.OrdinalIgnoreCase),
                $"El sistema mostró una alerta, pero no era la esperada. \nEsperaba: {mensajeEsperado} \nMostró: {mensajeActual}");
        }*/
        [Then(@"El sistema debe mostrar la alerta de validacion '(.*)'")]
        public void ThenElSistemaDebeMostrarLaAlertaDeValidacion(string mensajeEsperado)
        {
            string mensajeActual = nuevaAdquisicionPage.ObtenerMensajeDeValidacion();
            if (mensajeActual == "SIN_ALERTA")
            {
                mensajeActual = nuevaAdquisicionPage.ObtenerMensajeConfirmacion();
            }

            if (mensajeActual == "SIN_ALERTA" || mensajeActual == "NO_SE_DETECTO_MODAL_DE_EXITO")
            {
                Assert.Fail($"❌ BUG DETECTADO: El sistema permitió la acción. Se esperaba la validación: '{mensajeEsperado}', pero no apareció ninguna alerta en pantalla.");
            }

            Assert.IsTrue(mensajeActual.Contains(mensajeEsperado, StringComparison.OrdinalIgnoreCase),
                $"❌ El sistema mostró una alerta, pero no era la esperada. \nEsperaba: '{mensajeEsperado}' \nMostró: '{mensajeActual}'");
        }

        

    }
}