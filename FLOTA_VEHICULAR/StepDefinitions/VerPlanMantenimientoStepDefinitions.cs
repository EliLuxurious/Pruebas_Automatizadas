using FLOTA_VEHICULAR.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace FLOTA_VEHICULAR.StepDefinitions
{
    [Binding]
    public class VerPlanMantenimientoStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly VerPlanMantenimientoPage verPlanMantenimientoPage;

        public VerPlanMantenimientoStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            verPlanMantenimientoPage = new VerPlanMantenimientoPage(driver);
        }

        // ===============================
        // NAVEGACIÓN
        // ===============================

        [When(@"Se ingresa al módulo Mantenimiento y submódulo Ver Plan Mantenimientos")]
        public void WhenSeIngresaAlModuloMantenimientoYSubmoduloVerPlanMantenimientos()
        {
            verPlanMantenimientoPage.IngresarSubmoduloVerPlanMantenimientos();
            Console.WriteLine("Navegación exitosa a Ver Plan Mantenimientos.");
        }

        // ===============================
        // ACCIONES DE FORMULARIO - PLAN
        // ===============================

        [When(@"Se hace clic en el botón Nuevo Plan de Mantenimiento")]
        public void WhenSeHaceClicEnElBotonNuevoPlanDeMantenimiento()
        {
            verPlanMantenimientoPage.ClicNuevoPlan();
        }

        [When(@"Se ingresa el RUC ""(.*)"" y se busca la Razón Social")]
        public void WhenSeIngresaElRUCYSeBuscaLaRazonSocial(string ruc)
        {
            verPlanMantenimientoPage.IngresarRUCYBuscar(ruc);
        }

        [When(@"Se ingresa la dirección ""(.*)""")]
        public void WhenSeIngresaLaDireccion(string direccion)
        {
            verPlanMantenimientoPage.IngresarDireccion(direccion);
        }

        [When(@"Se ingresa el número de contrato ""(.*)""")]
        public void WhenSeIngresaElNumeroDeContrato(string contrato)
        {
            verPlanMantenimientoPage.IngresarNumeroContrato(contrato);
        }

        [When(@"Se selecciona la fecha Desde ""(.*)""")]
        public void WhenSeSeleccionaLaFechaDesde(string fecha)
        {
            verPlanMantenimientoPage.IngresarFechaDesde(fecha);
        }

        [When(@"Se selecciona la fecha Hasta ""(.*)""")]
        public void WhenSeSeleccionaLaFechaHasta(string fecha)
        {
            verPlanMantenimientoPage.IngresarFechaHasta(fecha);
        }

        [When(@"Se adjunta el documento del plan ""(.*)""")]
        public void WhenSeAdjuntaElDocumentoDelPlan(string archivo)
        {
            verPlanMantenimientoPage.SubirDocumento(archivo);
            Console.WriteLine($"Se adjuntó el documento: {archivo}");
        }

        [Then(@"Se guarda el registro del plan de mantenimiento")]
        public void ThenSeGuardaElRegistroDelPlanDeMantenimiento()
        {
            verPlanMantenimientoPage.GuardarPlanMantenimiento();
        }


        [Then(@"el sistema debe mostrar el mensaje de éxito de plan ""(.*)""")]
        public void ThenElSistemaDebeMostrarElMensajeDeExitoDePlan(string mensajeEsperado)
        {
            bool exito = verPlanMantenimientoPage.ValidarMensajeExito(mensajeEsperado);
            Assert.IsTrue(exito, $"[ERROR QA]: No se mostró el mensaje de confirmación con el texto '{mensajeEsperado}'.");
        }




        [Then(@"el sistema debe mostrar el mensaje de error de plan ""(.*)""")]
        public void ThenElSistemaDebeMostrarElMensajeDeErrorDePlan(string mensajeEsperado)
        {
            bool error = verPlanMantenimientoPage.ValidarMensajeError(mensajeEsperado);

            Assert.IsTrue(
                error,
                $"[ERROR QA]: No se mostró el mensaje de error esperado con el texto '{mensajeEsperado}'."
            );
        }





        [When(@"Se busca el plan de mantenimiento por número de contrato ""(.*)""")]
        public void WhenSeBuscaElPlanDeMantenimientoPorNumeroDeContrato(string contrato)
        {
            verPlanMantenimientoPage.BuscarPlanPorNumeroContrato(contrato);
        }

        [When(@"Se abre el detalle del plan de mantenimiento encontrado")]
        public void WhenSeAbreElDetalleDelPlanDeMantenimientoEncontrado()
        {
            verPlanMantenimientoPage.AbrirDetallePlanEncontrado();
        }

        [When(@"Se agregan vehículos al plan de mantenimiento")]
        public void WhenSeAgreganVehiculosAlPlanDeMantenimiento()
        {
            verPlanMantenimientoPage.AgregarVehiculosAlPlan();
        }

        [When(@"Se aprueba el plan de mantenimiento")]
        public void WhenSeApruebaElPlanDeMantenimiento()
        {
            verPlanMantenimientoPage.AprobarPlanMantenimiento();
        }

        [Then(@"el sistema debe mostrar el plan con estado ""(.*)""")]
        public void ThenElSistemaDebeMostrarElPlanConEstado(string estadoEsperado)
        {
            bool estadoCorrecto = verPlanMantenimientoPage.ValidarEstadoPlan(estadoEsperado);

            Assert.IsTrue(
                estadoCorrecto,
                $"[ERROR QA]: El plan no cambió al estado esperado '{estadoEsperado}'."
            );
        }



        [Then(@"Se busca el plan de mantenimiento por número de contrato ""(.*)""")]
        public void ThenSeBuscaElPlanDeMantenimientoPorNumeroDeContrato(string contrato)
        {
            verPlanMantenimientoPage.BuscarPlanPorNumeroContrato(contrato);
        }





        [When(@"Se intenta aprobar el plan de mantenimiento sin vehículos asociados")]
        public void WhenSeIntentaAprobarElPlanDeMantenimientoSinVehiculosAsociados()
        {
            verPlanMantenimientoPage.IntentarAprobarPlanSinVehiculos();
        }

        [Then(@"el sistema debe mostrar mensaje de validación al aprobar plan sin vehículos")]
        public void ThenElSistemaDebeMostrarMensajeDeValidacionAlAprobarPlanSinVehiculos()
        {
            bool mensajeMostrado = verPlanMantenimientoPage.ValidarMensajePlanSinVehiculos();

            Assert.IsTrue(
                mensajeMostrado,
                "[ERROR QA]: No se mostró mensaje de validación al intentar aprobar un plan sin vehículos asociados."
            );
        }



        [When(@"Se crea mantenimiento preventivo con clase ""(.*)"" y fecha de ejecución ""(.*)""")]
        public void WhenSeCreaMantenimientoPreventivoConClaseYFechaDeEjecucion(string clase, string fechaEjecucion)
        {
            verPlanMantenimientoPage.CrearMantenimientoPreventivo(clase, fechaEjecucion);
        }

        [Then(@"el sistema debe mostrar mensaje de mantenimiento registrado exitosamente")]
        public void ThenElSistemaDebeMostrarMensajeDeMantenimientoRegistradoExitosamente()
        {
            bool registrado = verPlanMantenimientoPage.ValidarMensajeMantenimientoRegistrado();

            Assert.IsTrue(
                registrado,
                "[ERROR QA]: No se mostró el mensaje de mantenimiento registrado exitosamente."
            );
        }

        [When(@"Se agregan los vehículos con placas ""(.*)"" y ""(.*)"" al plan de mantenimiento")]
        public void WhenSeAgreganLosVehiculosConPlacasAlPlanDeMantenimiento(string placa1, string placa2)
        {
            verPlanMantenimientoPage.AgregarVehiculosPorPlaca(placa1, placa2);
        }


    }




























}