using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages;

namespace SIGES3_0.StepDefinitions
{
    [Binding]
    public class OrdenDePagoSteps
    {
        private readonly OrdenDePagoPage ordenPage;

        public OrdenDePagoSteps(IWebDriver driver)
        {
            ordenPage = new OrdenDePagoPage(driver);
        }

        // --- NAVEGACIÓN ---

        [When("navega al módulo {string}")]
        public void NavegaAlModulo(string modulo)
        {
            ordenPage.IrFacturacionCiclica();
        }

        [When("accede a la pestaña {string}")]
        public void AccedeALaPestana(string pestana)
        {
            ordenPage.AbrirOrdenDePago();
        }

        [When("busca la orden con ID {string}")]
        public void BuscaLaOrden(string idOrden)
        {
            ordenPage.BuscarOrden(idOrden);
        }

        [When("abre el detalle de la orden {string}")]
        public void AbreDetalleOrden(string idOrden)
        {
            ordenPage.AbrirDetalleOrden(idOrden);
        }

        // --- ACCIONES GENERALES ---

        [When("hace clic en el botón {string}")]
        public void HaceClickBoton(string boton)
        {
            ordenPage.ClickBoton(boton);
        }

        [When("selecciona la opción {string}")]
        public void SeleccionaOpcion(string opcion)
        {
            ordenPage.SeleccionarOpcion(opcion);
        }

        // --- MANEJO DE FORMATOS (Scenario Outline) ---

        [When("selecciona el formato {string}")]
        public void SeleccionaFormato(string formato)
        {
            ordenPage.SeleccionarFormato(formato);
        }

        [When("hace clic en el botón {string} en la sección de formatos")]
        public void HaceClickAccion(string accion)
        {
            // Reutilizamos el ClickBoton que ya es robusto
            ordenPage.ClickBoton(accion);
        }

        // --- VALIDACIONES ---

        [Then("el sistema muestra las opciones de compartir")]
        public void ThenOpcionesCompartir()
        {
            Assert.IsTrue(ordenPage.OrdenCompartida(), "La ventana de compartir no se mostró.");
        }

        [Then("el sistema genera la impresión de la orden de pago")]
        public void ThenOrdenImpresa()
        {
            Assert.IsTrue(ordenPage.OrdenImpresa(), "No se pudo generar la impresión.");
        }

        [Then("el sistema descarga la orden de pago correctamente")]
        public void ValidarDescarga()
        {
            Assert.IsTrue(ordenPage.OrdenDescargada(), "La descarga no se completó correctamente.");
        }

        // PASO DINÁMICO PARA EL SCENARIO OUTLINE
        // Este es el que te faltaba para que funcione la tabla de ejemplos
        [Then("el sistema procesa la {string} en formato {string}")]
        public void ThenElSistemaProcesaAccionEnFormato(string accion, string formato)
        {
            if (accion == "Imprimir")
            {
                Assert.IsTrue(ordenPage.OrdenImpresa(), $"Fallo al imprimir en formato {formato}");
            }
            else if (accion == "Descargar")
            {
                Assert.IsTrue(ordenPage.OrdenDescargada(), $"Fallo al descargar en formato {formato}");
            }
            else if (accion == "Compartir")
            {
                Assert.IsTrue(ordenPage.OrdenCompartida(), $"Fallo al mostrar ventana de compartir en formato {formato}");
            }
        }

        // ============================================
        // 🔥 NUEVOS STEPS CP032 (PAGO MANUAL)
        // ============================================

        [When("genero una nueva orden de pago")]
        public void GenerarOrden()
        {
            ordenPage.GenerarOrden();
        }

        [When("configuro la paginación a {int}")]
        public void ConfigurarPaginacion(int cantidad)
        {
            ordenPage.ConfigurarPaginacion(cantidad);
        }

        [When("busco al cliente {string}")]
        public void BuscarCliente(string nombre)
        {
            ordenPage.BuscarCliente(nombre);
        }

        

        [When("apruebo el pago")]
        public void AprobarPago()
        {
            ordenPage.AprobarPago();
        }

        [When("rechazo el pago")]
        public void RechazarPago()
        {
            ordenPage.RechazarPago();
        }

        // ================================
        // 🟢 CLIENTE
        // ================================

        [When("reviso la notificación en la campanita")]
        public void Campanita()
        {
            ordenPage.AbrirCampanita();
        }

        [When("selecciono la orden generada")]
        public void SeleccionarOrden()
        {
            ordenPage.SeleccionarOrdenCliente();
        }

        [When("elijo el método \"Pago Manual\"")]
        public void PagoManual()
        {
            ordenPage.SeleccionarPagoManual();
        }

        [When("adjunto el archivo {string}")]
        public void SubirArchivo(string archivo)
        {
            ordenPage.SubirComprobante(archivo);
        }

        [When("envío el pago")]
        public void EnviarPago()
        {
            ordenPage.EnviarPago();
        }

        // ================================
        // 🟢 VALIDACIÓN FINAL
        // ================================

        [When("accedo al detalle del cliente")]
        public void DetalleCliente()
        {
            ordenPage.VerDetalleCliente();
        }

        [When("ingreso a historial de planes")]
        public void HistorialPlanes()
        {
            ordenPage.IrHistorialPlanes();
        }

        [Then("verifico que el estado sea {string}")]
        public void ValidarEstado(string estado)
        {
            Assert.IsTrue(ordenPage.ValidarEstado(estado),
                $"❌ El estado no cambió a {estado}");
        }

        [When("filtro las órdenes pendientes")]
        public void FiltroPendientes()
        {
            ordenPage.ClickPendientes();
        }

        [When("genero la orden de pago")]
        public void Generar()
        {
            ordenPage.ClickGenerar();
        }

        [When("gestiono la aprobación del pago")]
        public void GestionarAprobacionPago()
        {
            if (ordenPage.ExisteRevisarPago())
            {
                ordenPage.ClickRevisarPago();
                ordenPage.AprobarPago();
            }
            else
            {
                Console.WriteLine("El pago ya fue validado/aprobado, se omite esta parte");
            }
        }

        [When("gestiono el rechazo del pago")]
        public void gestionarRechazoPago()
        {
            // 🔹 Caso 1: Aún no está validado
            if (ordenPage.ExisteRevisarPago())
            {
                ordenPage.ClickRechazarPago();
                ordenPage.RechazarPago();
            }
            else
            {
                Console.WriteLine("El pago ya fue rechazado, se omite esta parte");
            }
        }

        [When("intento validar documento si aplica")]
        public void ValidarDocumentoSiAplica()
        {
            if (ordenPage.ExisteValidar())
            {
                ordenPage.ValidarDocumento();
                ordenPage.ConfirmarModalOk();
            }
            else
            {
                Console.WriteLine("⚠️ Documento ya validado");
            }
        }

        [When("ingreso el número de operación {string}")]
        public void IngresoNumeroOperacion(string numero)
        {
            ordenPage.IngresarNumeroOperacion(numero);
        }

        [When("confirmo el mensaje de operación")]
        [Then("confirmo el mensaje de operación")]
        public void ConfirmarOperacion()
        {
            ordenPage.ConfirmarModalOk();
        }

        [When("vuelvo a revisar la notificación")]
        public void RevisarNotificacionNuevamente()
        {
            ordenPage.AbrirCampanita();
            ordenPage.SeleccionarOrdenCliente();
        }

        [Then("el estado del pago debe estar en proceso")]
        public void ValidarPagoEnProceso()
        {
            Assert.IsTrue(ordenPage.ValidarPagoEnProceso(),
                "❌ El mensaje de pago en proceso no apareció");
        }

        [When("valido el documento")]
        public void ValidarDocumento()
        {
            ordenPage.ValidarDocumento();
        }

        [When("hago clic en {string}")]
        public void HagoClicEn(string boton)
        {
            if (boton == "Realizar Pago")
            {
                ordenPage.ClickRealizarPago();
            }
            else if (boton == "Revisar Pago")
            {
                ordenPage.ClickRevisarPago();
            }
            else
            {
                ordenPage.ClickBoton(boton);
            }
        }

        [When("facturo el pago")]
        public void FacturoElPago()
        {
            ordenPage.ClickFacturar();
        }



    }
}