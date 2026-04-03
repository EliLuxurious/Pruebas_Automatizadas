using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.VentasPage;

namespace SIGES3_0.StepDefinitions.VentasStep
{
    [Binding]
    public class AjusteComprobanteStepDefinitions
    {
        private readonly AjusteComprobantePage _ajustePage;
        private readonly NuevaVentaPage _nuevaVentaPage;

        public AjusteComprobanteStepDefinitions(IWebDriver driver)
        {
            _ajustePage = new AjusteComprobantePage(driver);
            _nuevaVentaPage = new NuevaVentaPage(driver);
        }

        // ── Precondición: crear venta completa ──────────────────────────────
        [StepDefinition("crea una venta con familia {string}, concepto {string}, cantidad {string}, documento {string}, comprobante {string}, serie {string}, entrega {string}, pago {string}")]
        public void CreaUnaVenta(string familia, string concepto, string cantidad, string documento,
            string comprobante, string serie, string entrega, string pago)
        {
            _nuevaVentaPage.SelectProductFlow(familia, concepto);
            _nuevaVentaPage.UpdateQuantityFlow(cantidad);
            _nuevaVentaPage.EnterDocumentAndSearch(documento);
            _nuevaVentaPage.SelectVoucherFlow(comprobante, serie);
            _nuevaVentaPage.SelectDeliveryFlow(entrega);
            _nuevaVentaPage.ConfigurePaymentFlow(pago);
            _nuevaVentaPage.GuardarVentaFlow();
            _nuevaVentaPage.VerifyConfirmationMessage("Se registr");
            Thread.Sleep(1000);
        }

        // ── Ver Ventas: filtrar por ayer ─────────────────────────────────────
        [StepDefinition("filtra ventas por fecha de ayer")]
        public void FiltraVentasPorFechaDeAyer()
        {
            _ajustePage.FiltrarVentasPorFechaAyer();
        }

        // ── Abrir modal de ajuste ───────────────────────────────────────────
        [StepDefinition("accede a las opciones del comprobante recien registrado")]
        public void AccedeALasOpcionesDelComprobanteRecienRegistrado()
        {
            _ajustePage.ClickAccionPrimerComprobante();
        }

        // ── Tab del modal ───────────────────────────────────────────────────
        [StepDefinition("selecciona {string} en el modal de ajuste")]
        public void SeleccionaEnElModalDeAjuste(string tab)
        {
            _ajustePage.SeleccionarTabAjuste(tab);
        }

        // ── Tipo nota de débito ─────────────────────────────────────────────
        [StepDefinition("selecciona tipo de nota de debito {string}")]
        public void SeleccionaTipoDeNotaDeDebito(string tipo)
        {
            _ajustePage.SeleccionarTipoNotaDebito(tipo);
        }

        // ── Tipo nota de crédito ────────────────────────────────────────────
        [StepDefinition("selecciona tipo de nota de credito {string}")]
        public void SeleccionaTipoDeNotaDeCredito(string tipo)
        {
            _ajustePage.SeleccionarTipoNotaCredito(tipo);
        }

        // ── Comprobante destino ─────────────────────────────────────────────
        [StepDefinition("selecciona comprobante destino {string}")]
        public void SeleccionaComprobanteDestino(string comprobante)
        {
            _ajustePage.SeleccionarComprobanteDestino(comprobante);
        }

        // ── Serie en ajuste ─────────────────────────────────────────────────
        [StepDefinition("selecciona serie {string} en el ajuste")]
        public void SeleccionaSerieEnElAjuste(string serie)
        {
            _ajustePage.SeleccionarSerie(serie);
        }

        // ── Motivo o Sustento ───────────────────────────────────────────────
        [StepDefinition("ingresa motivo o sustento {string}")]
        public void IngresaMotivoOSustento(string motivo)
        {
            _ajustePage.IngresarMotivoSustento(motivo);
        }

        // ── Monto del interés ───────────────────────────────────────────────
        [StepDefinition("ingresa monto del interes {string}")]
        public void IngresaMontoDelInteres(string monto)
        {
            _ajustePage.IngresarMontoInteres(monto);
        }

        // ── Importe NC ─────────────────────────────────────────────────────
        [StepDefinition("ingresa importe NC {string}")]
        public void IngresaImporteNC(string importe)
        {
            _ajustePage.IngresarImporteNC(importe);
        }

        // ── Importe detalle por ítem ────────────────────────────────────────
        [StepDefinition("ingresa importe detalle {string} para el item")]
        public void IngresaImporteDetalleParaElItem(string importe)
        {
            _ajustePage.IngresarImporteDetalle(importe);
        }

        // ── Cantidad a devolver ─────────────────────────────────────────────
        [StepDefinition("ingresa cantidad a devolver {string}")]
        public void IngresaCantidadADevolver(string cantidad)
        {
            _ajustePage.IngresarCantidadDevolver(cantidad);
        }

        // ── Total aumento del valor ─────────────────────────────────────────
        [StepDefinition("ingresa total aumento del valor {string}")]
        public void IngresaTotalAumentoDelValor(string monto)
        {
            if (EsOpcionalVacio(monto)) return;
            _ajustePage.IngresarTotalAumentoValor(monto);
        }

        // ── Expandir sección ────────────────────────────────────────────────
        [StepDefinition("expande la seccion {string} del ajuste")]
        public void ExpandeLaSeccionDelAjuste(string nombre)
        {
            _ajustePage.ExpandirSeccion(nombre);
        }

        // ── Tipo de pago en ajuste ──────────────────────────────────────────
        [StepDefinition("selecciona tipo de pago {string} en la seccion pago del ajuste")]
        public void SeleccionaTipoDePagoEnElAjuste(string tipo)
        {
            _ajustePage.SeleccionarTipoPagoAjuste(tipo);
        }

        // ── Monto inicial ───────────────────────────────────────────────────
        [StepDefinition("ingresa monto inicial {string} en el ajuste")]
        public void IngresaMontoInicialEnElAjuste(string monto)
        {
            _ajustePage.IngresarMontoInicial(monto);
        }

        // ── Medio de pago ───────────────────────────────────────────────────
        [StepDefinition("selecciona medio de pago {string} en el ajuste")]
        public void SeleccionaMedioDePagoEnElAjuste(string medio)
        {
            _ajustePage.SeleccionarMedioPago(medio);
        }

        // ── Observación ─────────────────────────────────────────────────────
        [StepDefinition("ingresa observacion {string} en el ajuste")]
        public void IngresaObservacionEnElAjuste(string observacion)
        {
            _ajustePage.IngresarObservacion(observacion);
        }

        // ── Entrega (NC) ───────────────────────────────────────────────────
        [StepDefinition("selecciona entrega {string} en el ajuste")]
        public void SeleccionaEntregaEnElAjuste(string tipo)
        {
            _ajustePage.SeleccionarEntrega(tipo);
        }

        // ── Devolución (NC) ─────────────────────────────────────────────────
        [StepDefinition("selecciona devolucion {string} en el ajuste")]
        public void SeleccionaDevolucionEnElAjuste(string tipo)
        {
            _ajustePage.SeleccionarDevolucion(tipo);
        }

        // ── Guardar ajuste ─────────────────────────────────────────────────
        [StepDefinition("hace clic en Guardar en el modal de ajuste")]
        public void HaceClicEnGuardarEnElModalDeAjuste()
        {
            _ajustePage.ClickGuardarAjuste();
        }

        // ── Validaciones ───────────────────────────────────────────────────
        [Then("el sistema genera el comprobante de ajuste exitosamente")]
        public void ThenElSistemaGeneraElComprobanteDeAjusteExitosamente()
        {
            _ajustePage.VerificarAjusteExitoso();
        }

        [Then("el sistema bloquea el guardado del ajuste")]
        public void ThenElSistemaBloqueaElGuardadoDelAjuste()
        {
            _ajustePage.VerificarBloqueoGuardar();
        }

        [Then("el sistema muestra mensaje de monto mayor al total")]
        public void ThenElSistemaMuestraMensajeDeMontoMayor()
        {
            _ajustePage.VerificarMensajeMontoMayor();
        }

        [Then("el sistema muestra mensaje de cantidad mayor a la entregada")]
        public void ThenElSistemaMuestraMensajeDeCantidadMayor()
        {
            _ajustePage.VerificarMensajeCantidadMayor();
        }

        // ═══════════════════════════════════════════════════════════════════
        // Pasos opcionales para Scenario Outline (valor "-" = omitir paso)
        // ═══════════════════════════════════════════════════════════════════

        private static bool EsOpcionalVacio(string valor) =>
            string.IsNullOrWhiteSpace(valor) || valor.Trim() == "-";

        [StepDefinition("opcionalmente ingresa monto inicial {string} en el ajuste")]
        public void OpIngresaMontoInicial(string monto)
        {
            if (EsOpcionalVacio(monto)) return;
            _ajustePage.IngresarMontoInicial(monto);
        }

        [StepDefinition("opcionalmente selecciona medio de pago {string} en el ajuste")]
        public void OpSeleccionaMedioPago(string medio)
        {
            if (EsOpcionalVacio(medio)) return;
            _ajustePage.SeleccionarMedioPago(medio);
        }

        [StepDefinition("opcionalmente selecciona entrega {string} en el ajuste")]
        public void OpSeleccionaEntrega(string tipo)
        {
            if (EsOpcionalVacio(tipo)) return;
            _ajustePage.SeleccionarEntrega(tipo);
        }

        [StepDefinition("opcionalmente ingresa cantidad a devolver {string}")]
        public void OpIngresaCantidadADevolver(string cantidad)
        {
            if (EsOpcionalVacio(cantidad)) return;
            _ajustePage.IngresarCantidadDevolver(cantidad);
        }

        [StepDefinition("opcionalmente selecciona devolucion {string} en el ajuste")]
        public void OpSeleccionaDevolucion(string tipo)
        {
            if (EsOpcionalVacio(tipo)) return;
            _ajustePage.SeleccionarDevolucion(tipo);
        }

        [StepDefinition("opcionalmente ingresa observacion {string} en el ajuste")]
        public void OpIngresaObservacion(string observacion)
        {
            if (EsOpcionalVacio(observacion)) return;
            _ajustePage.IngresarObservacion(observacion);
        }

        [StepDefinition("opcionalmente ingresa importe NC {string}")]
        public void OpIngresaImporteNC(string importe)
        {
            if (EsOpcionalVacio(importe)) return;
            _ajustePage.IngresarImporteNC(importe);
        }

        [StepDefinition("opcionalmente ingresa importe detalle {string} para el item")]
        public void OpIngresaImporteDetalleParaElItem(string importe)
        {
            if (EsOpcionalVacio(importe)) return;
            _ajustePage.IngresarImporteDetalle(importe);
        }
    }
}
