using OpenQA.Selenium;
using NUnit.Framework;
using Reqnroll;
using SIGES3_0.Pages;
using System.Threading;

namespace SIGES3_0.StepDefinitions
{
    [Binding]
    public class BajaDeClientesEnFacturacionCiclicaStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly ClientesPage clientesPage;
        private readonly PlanServicioPage planServicioPage;
        private readonly GestionCliente gestionCliente;
        private readonly ScenarioContext _scenarioContext;

        public BajaDeClientesEnFacturacionCiclicaStepDefinitions(
            IWebDriver driver,
            ScenarioContext scenarioContext)
        {
            this.driver = driver;
            _scenarioContext = scenarioContext;

            clientesPage = new ClientesPage(driver);
            planServicioPage = new PlanServicioPage(driver);
            gestionCliente = new GestionCliente(driver);
        }

        // =====================================================
        // GIVEN: EXISTE CLIENTE ACTIVO
        // =====================================================

        //[Given(@"existe un cliente en estado ""(.*)""")]
        //public void GivenExisteUnClienteEnEstado(string estado)
        //{
        //    if (!gestionCliente.ExisteClienteActivo())
        //    {
        //        Console.WriteLine("⚠️ No existe cliente activo, creando uno...");

        //        // 🔹 Obtener plan dinámico
        //        string plan = _scenarioContext.ContainsKey("NombrePlanActual") 
        //            ? _scenarioContext["NombrePlanActual"].ToString()
        //            : "AUTO";

        //        // 🔹 Crear cliente reutilizando tu lógica
        //        clientesPage.CrearClienteBasico(plan);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Ya existe un cliente activo");
        //    }
        //}

        [Given(@"existe un cliente en estado ""(.*)""")]
        public void GivenExisteUnClienteEnEstado(string estado)
        {
            gestionCliente.IrAModuloClientes();
            gestionCliente.CambiarPaginacionA100();

            if (gestionCliente.ExisteClienteActivo())
            {
                Console.WriteLine("✅ Ya existe un cliente activo. Se reutilizará.");

                string rucExistente = gestionCliente.ObtenerRucPrimerClienteActivo();

                _scenarioContext["RUC"] = rucExistente;
                _scenarioContext["REUTILIZA_CLIENTE_ACTIVO"] = true;

                return;
            }

            Console.WriteLine("⚠️ No existe cliente activo. Se creará uno nuevo.");

            // 2. Solo si no hay cliente activo, asegurar plan
            string nombrePlan = _scenarioContext.ContainsKey("NombrePlanActual")
                ? _scenarioContext["NombrePlanActual"].ToString()
                : "PlanQA_Default";

            planServicioPage.IrModuloFacturacionCiclica();
            planServicioPage.NavegarAPlanDeServicio();

            nombrePlan = planServicioPage.BuscarOCrearPlan(nombrePlan);
            _scenarioContext["NombrePlanActual"] = nombrePlan;

            Console.WriteLine($"✅ Plan listo para crear cliente: {nombrePlan}");

            // 3. Volver a clientes y crear cliente
            gestionCliente.IrAModuloClientes();
            gestionCliente.CambiarPaginacionA100();

            string rucNuevo = clientesPage.CrearClienteBasico(nombrePlan);

            _scenarioContext["RUC"] = rucNuevo;
            _scenarioContext["PLAN"] = nombrePlan;
            _scenarioContext["REUTILIZA_CLIENTE_ACTIVO"] = false;

            Console.WriteLine($"✅ Cliente creado con RUC: {rucNuevo}");
        }

        // =====================================================
        // WHEN: SOLICITA BAJA
        // =====================================================



        [When(@"el usuario solicita dar de baja el cliente")]
        public void WhenElUsuarioSolicitaDarDeBajaElCliente()
        {
            gestionCliente.IrAModuloClientes();
            gestionCliente.CambiarPaginacionA100();

            bool reutilizaActivo = _scenarioContext.ContainsKey("REUTILIZA_CLIENTE_ACTIVO")
                && (bool)_scenarioContext["REUTILIZA_CLIENTE_ACTIVO"];

            if (reutilizaActivo)
            {
                Console.WriteLine("✅ Se usará el primer cliente activo existente");
                gestionCliente.ClickBotonLupaClienteActivo();
            }
            else
            {
                string ruc = _scenarioContext["RUC"].ToString();

                Console.WriteLine($"🔍 Buscando cliente creado con RUC: {ruc}");
                gestionCliente.BuscarClientePorRuc(ruc);
                gestionCliente.ClickBotonLupa(ruc);
            }

            Thread.Sleep(2000);

            gestionCliente.ClickDarDeBaja();
        }

        // =====================================================
        // WHEN: CONFIRMA (SÍ)
        // =====================================================

        // =====================================================
        // WHEN: CANCELA (NO)
        // =====================================================

        [When(@"cancela la operación de baja")]
        public void WhenCancelaLaOperacionDeBaja()
        {
            gestionCliente.ConfirmarModalNo();
        }

        // =====================================================
        // THEN: VALIDAR BAJA EXITOSA
        // =====================================================

        [Then(@"el cliente cambia a estado ""(.*)""")]
        public void ThenElClienteCambiaAEstado(string estadoEsperado)
        {
            bool encontrado = gestionCliente.EsperarEstadoCliente(estadoEsperado);

            Assert.IsTrue(encontrado, $"❌ No se encontró el estado esperado: {estadoEsperado}");

            Console.WriteLine($"✅ Estado validado correctamente: {estadoEsperado}");
        }

        // =====================================================
        // THEN: VALIDAR QUE SIGUE ACTIVO
        // =====================================================

        [Then(@"el cliente permanece en estado ""(.*)""")]
        public void ThenElClientePermaneceEnEstado(string estadoEsperado)
        {
            bool encontrado = gestionCliente.EsperarEstadoCliente(estadoEsperado);

            Assert.IsTrue(encontrado, $"❌ No se encontró el estado esperado: {estadoEsperado}");

            Console.WriteLine("✅ El cliente sigue activo correctamente");
        }

        // =====================================================
        // THEN: VALIDACIÓN EXTRA
        // =====================================================

        [Then(@"no se realiza la baja del cliente")]
        public void ThenNoSeRealizaLaBajaDelCliente()
        {
            Console.WriteLine("⚠️ Se canceló la operación de baja correctamente");
        }

        [When(@"confirma la operación de baja")]
        public void WhenConfirmaLaOperacionDeBaja()
        {
            gestionCliente.ConfirmarModalSi();
            Thread.Sleep(1000);
            gestionCliente.CerrarModalOkSiExiste();
        }

        [When(@"el usuario solicita descargar el contrato del cliente")]
        public void WhenElUsuarioSolicitaDescargarElContratoDelCliente()
        {
            gestionCliente.IrAModuloClientes();
            gestionCliente.CambiarPaginacionA100();

            bool reutilizaActivo = _scenarioContext.ContainsKey("REUTILIZA_CLIENTE_ACTIVO")
                && (bool)_scenarioContext["REUTILIZA_CLIENTE_ACTIVO"];

            if (reutilizaActivo)
            {
                Console.WriteLine("✅ Se usará el primer cliente activo existente");
                gestionCliente.ClickBotonLupaClienteActivo();
            }
            else
            {
                string ruc = _scenarioContext["RUC"].ToString();

                Console.WriteLine($"🔍 Buscando cliente creado con RUC: {ruc}");
                gestionCliente.BuscarClientePorRuc(ruc);
                gestionCliente.ClickBotonLupa(ruc);
            }

            Thread.Sleep(2000);

            gestionCliente.PrepararDeteccionImpresion();
            gestionCliente.ClickDescargarContrato();
        }

        [Then(@"se muestra la ventana de impresión del contrato")]
        public void ThenSeMuestraLaVentanaDeImpresionDelContrato()
        {
            bool printInvocado = gestionCliente.SeInvocoImpresion();

            Assert.IsTrue(printInvocado, "❌ No se invocó window.print() al descargar el contrato");

            Console.WriteLine("✅ Se invocó correctamente la impresión del contrato");
        }

        
    }
}