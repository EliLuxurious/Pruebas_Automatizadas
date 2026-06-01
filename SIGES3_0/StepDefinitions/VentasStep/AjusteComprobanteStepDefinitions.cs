using System;
using System.Linq;
using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.VentasPage;

namespace SIGES3_0.StepDefinitions.VentasStep
{
    [Binding]
    public class AjusteComprobanteStepDefinitions
    {
        private readonly AjusteComprobantePage _ajustePage;
        private readonly ScenarioContext _scenarioContext;

        public AjusteComprobanteStepDefinitions(IWebDriver driver, ScenarioContext scenarioContext)
        {
            _ajustePage = new AjusteComprobantePage(driver);
            _scenarioContext = scenarioContext;
        }

        [StepDefinition("abre el modal ajustes de comprobante")]
        public void AbreElModalAjustesDeComprobante()
        {
            _ajustePage.AbrirModalAjustesDeComprobante();
        }

        [StepDefinition("selecciona la opcion {string} del modal ajustes de comprobante")]
        public void SeleccionaLaOpcionDelModalAjustesDeComprobante(string opcion)
        {
            _ajustePage.SeleccionarOpcionDesdeAcciones(opcion);

            if (opcion.Trim().Equals("Invalidar", StringComparison.OrdinalIgnoreCase)
                && _scenarioContext.ScenarioInfo.Tags.Contains("FueraDePlazo"))
            {
                Console.WriteLine("[Invalidar] Validando internamente que la seccion Entrega no se muestre por fuera de plazo.");
                _ajustePage.VerificarSeccionEntregaNoVisibleEnInvalidacion();
            }
        }

        [StepDefinition("selecciona {string} en el modal de ajuste")]
        public void SeleccionaEnElModalDeAjuste(string tab)
        {
            AbreElModalAjustesDeComprobante();
            SeleccionaLaOpcionDelModalAjustesDeComprobante(tab);
        }

        [Then("el sistema muestra el modal Clonar venta")]
        public void ThenElSistemaMuestraElModalClonarVenta()
        {
            LogResultadoEsperado("muestra el modal Clonar venta");
            _ajustePage.VerificarModalClonarVisible();
        }

        [StepDefinition("selecciona la pestaña {string} en el modal Clonar venta")]
        public void SeleccionaLaPestanaEnElModalClonarVenta(string modo)
        {
            _ajustePage.SeleccionarPestanaModoClonar(modo);
        }

        [StepDefinition("modifica la cantidad a {string} en el modal Clonar venta")]
        public void ModificaLaCantidadEnElModalClonarVenta(string cantidad)
        {
            _ajustePage.ModificarCantidadPrimerItemClonar(cantidad);
        }

        [StepDefinition("selecciona la entrega {string} en el modal Clonar venta")]
        public void SeleccionaLaEntregaEnElModalClonarVenta(string entrega)
        {
            _ajustePage.SeleccionarEntregaClonar(entrega);
        }

        [StepDefinition("modifica el cliente a {string} en el modal Clonar venta")]
        public void ModificaElClienteEnElModalClonarVenta(string documento)
        {
            _ajustePage.ModificarClienteClonar(documento);
        }

        [When("hace clic en Clonar venta")]
        public void WhenHaceClicEnClonarVenta()
        {
            _ajustePage.ClickClonarVenta();
        }

        [Then("el sistema registra la venta clonada correctamente")]
        public void ThenElSistemaRegistraLaVentaClonadaCorrectamente()
        {
            LogResultadoEsperado("registra la venta clonada correctamente");
            _ajustePage.VerificarClonacionExitosa();
        }

        [StepDefinition("selecciona tipo de nota de debito {string}")]
        public void SeleccionaTipoDeNotaDeDebito(string tipo)
        {
            _ajustePage.SeleccionarTipoNotaDebito(tipo);
        }

        [StepDefinition("selecciona tipo de nota de credito {string}")]
        public void SeleccionaTipoDeNotaDeCredito(string tipo)
        {
            _ajustePage.SeleccionarTipoNotaCredito(tipo);
        }

        [StepDefinition("selecciona comprobante destino {string}")]
        public void SeleccionaComprobanteDestino(string comprobante)
        {
            _ajustePage.SeleccionarComprobanteDestino(comprobante);
        }

        [StepDefinition("selecciona serie {string} en el ajuste")]
        public void SeleccionaSerieEnElAjuste(string serie)
        {
            _ajustePage.SeleccionarSerie(serie);
        }

        [StepDefinition("ingresa motivo o sustento {string}")]
        public void IngresaMotivoOSustento(string motivo)
        {
            if (EsOpcionalVacio(motivo)) return;
            _ajustePage.IngresarMotivoSustento(motivo);
        }

        [StepDefinition("ingresa monto del interes {string}")]
        public void IngresaMontoDelInteres(string monto)
        {
            _ajustePage.IngresarMontoInteres(monto);
        }

        [StepDefinition("ingresa importe NC {string}")]
        public void IngresaImporteNC(string importe)
        {
            _ajustePage.IngresarImporteNC(importe);
        }

        [StepDefinition("ingresa importe detalle {string} para el item")]
        public void IngresaImporteDetalleParaElItem(string importe)
        {
            _ajustePage.IngresarImporteDetalle(importe);
        }

        [StepDefinition("ingresa cantidad a devolver {string}")]
        public void IngresaCantidadADevolver(string cantidad)
        {
            if (EsOpcionalVacio(cantidad)) return;
            _ajustePage.IngresarCantidadDevolver(cantidad);
        }

        [StepDefinition("ingresa total aumento del valor {string}")]
        public void IngresaTotalAumentoDelValor(string monto)
        {
            if (EsOpcionalVacio(monto)) return;
            _ajustePage.IngresarTotalAumentoValor(monto);
        }

        [StepDefinition("expande la seccion {string} del ajuste")]
        public void ExpandeLaSeccionDelAjuste(string nombre)
        {
            _ajustePage.ExpandirSeccion(nombre);
        }

        [StepDefinition("selecciona tipo de pago {string} en la seccion pago del ajuste")]
        public void SeleccionaTipoDePagoEnElAjuste(string tipo)
        {
            _ajustePage.SeleccionarTipoPagoAjuste(tipo);
        }

        [StepDefinition("ingresa monto inicial {string} en el ajuste")]
        public void IngresaMontoInicialEnElAjuste(string monto)
        {
            _ajustePage.IngresarMontoInicial(monto);
        }

        [StepDefinition("selecciona medio de pago {string} en el ajuste")]
        public void SeleccionaMedioDePagoEnElAjuste(string medio)
        {
            _ajustePage.SeleccionarMedioPago(medio);
        }

        [StepDefinition("ingresa observacion {string} en el ajuste")]
        public void IngresaObservacionEnElAjuste(string observacion)
        {
            _ajustePage.IngresarObservacion(observacion);
        }

        [StepDefinition("selecciona entrega {string} en el ajuste")]
        public void SeleccionaEntregaEnElAjuste(string tipo)
        {
            _ajustePage.SeleccionarEntrega(tipo);
        }

        [StepDefinition("selecciona devolucion {string} en el ajuste")]
        public void SeleccionaDevolucionEnElAjuste(string tipo)
        {
            _ajustePage.SeleccionarDevolucion(tipo);
        }

        [StepDefinition("hace clic en Guardar en el modal de ajuste")]
        public void HaceClicEnGuardarEnElModalDeAjuste()
        {
            _ajustePage.ClickGuardarAjuste();
        }

        [Then("el sistema genera el comprobante de ajuste exitosamente")]
        public void ThenElSistemaGeneraElComprobanteDeAjusteExitosamente()
        {
            LogResultadoEsperado("genera el comprobante de ajuste exitosamente");
            _ajustePage.VerificarAjusteExitoso();
        }

        [Then("el sistema bloquea el guardado del ajuste")]
        public void ThenElSistemaBloqueaElGuardadoDelAjuste()
        {
            LogResultadoEsperado("bloquea el guardado del ajuste");
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

        [StepDefinition("selecciona devolucion {string} en el modal de invalidacion")]
        public void SeleccionaDevolucionEnElModalDeInvalidacion(string tipo)
        {
            if (EsOpcionalVacio(tipo)) return;
            _ajustePage.SeleccionarEntregaInvalidacion(tipo);
        }

        [StepDefinition("ingresa observacion de invalidacion {string}")]
        public void IngresaObservacionDeInvalidacion(string observacion)
        {
            _ajustePage.IngresarObservacionInvalidacion(observacion);
        }

        [Then("el sistema procesa la invalidacion correctamente")]
        public void ThenElSistemaProcesamLaInvalidacionCorrectamente()
        {
            LogResultadoEsperado("procesa la invalidacion correctamente");
            _ajustePage.ClickInvalidarEnModal();
            _ajustePage.VerificarInvalidacionExitosa();
        }

        [Then("el sistema muestra el resultado esperado de la invalidacion {string}")]
        public void ThenElSistemaMuestraElResultadoEsperadoDeLaInvalidacion(string resultadoEsperado)
        {
            LogResultadoEsperado(resultadoEsperado);
            switch (resultadoEsperado.Trim())
            {
                case "procesa la invalidacion correctamente":
                    _ajustePage.ClickInvalidarEnModal();
                    _ajustePage.VerificarInvalidacionExitosa();
                    break;
                case "no activa el boton Invalidar":
                    _ajustePage.VerificarBotonInvalidarNoActivo();
                    break;
                default:
                    throw new ArgumentException($"Resultado esperado de invalidacion no reconocido: {resultadoEsperado}");
            }
        }

        [Then("el sistema muestra el mensaje fuera de plazo para invalidar")]
        public void ThenElSistemaMuestraElMensajeFueraDePlazoParaInvalidar()
        {
            LogResultadoEsperado("muestra el mensaje fuera de plazo para invalidar");
            _ajustePage.VerificarBotonInvalidarNoVisible();
            _ajustePage.VerificarMensajeFueraDePlazoInvalidacion();
        }

        [Then("el sistema no muestra la opcion {string} en el modal ajustes de comprobante")]
        public void ThenElSistemaNoMuestraLaOpcionEnElModalAjustesDeComprobante(string opcion)
        {
            LogResultadoEsperado($"no muestra la opcion {opcion} en el modal ajustes de comprobante");
            _ajustePage.VerificarOpcionDelModalAjustesNoVisible(opcion);
        }

        private void LogResultadoEsperado(string verificacion)
        {
            var escenario = (_scenarioContext.ScenarioInfo.Title ?? "Escenario sin titulo")
                .Replace(" - <caso>", string.Empty, StringComparison.OrdinalIgnoreCase);
            var argumentos = _scenarioContext.ScenarioInfo.Arguments;
            var caso = argumentos != null && argumentos.Count > 0
                ? argumentos[0]?.ToString() ?? "(sin caso)"
                : "(sin caso)";

            Console.WriteLine(
                $"[Ajuste][Validacion] escenario='{escenario}', caso='{caso}', esperado='{verificacion}'.");
        }
    }
}
