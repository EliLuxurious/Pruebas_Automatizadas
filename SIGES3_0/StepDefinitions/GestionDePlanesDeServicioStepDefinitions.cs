using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SIGES3_0.Pages;
using System;

namespace SIGES3_0.StepDefinitions
{
    [Binding]
    public class GestionDePlanesDeServicioStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly PlanServicioPage planServicioPage;
        private readonly ScenarioContext _scenarioContext;
        private string nombrePlan; // esto se agrego

        public GestionDePlanesDeServicioStepDefinitions(IWebDriver driver, ScenarioContext scenarioContext)
        {
            this.driver = driver;
            this._scenarioContext = scenarioContext;
            planServicioPage = new PlanServicioPage(driver);
        }

        // ===================== GIVEN =====================

        //[Given("Inicio de sesión con usuario {string} y contraseña {string} en {string}")]
        //public void GivenInicioDeSesionConUsuarioYContrasenaEn(string usuario, string password, string url)
        //{
        //    planServicioPage.OpenToApplication(url);
        //    planServicioPage.LoginToApplication(usuario, password);
        //}

        [Given("Se ingresa al módulo {string}")]
        [When("Se ingresa al módulo {string}")]
        public void SeIngresaAlModulo(string modulo)
        {
            planServicioPage.IrModuloFacturacionCiclica();
        }

        [Given("Se ingresa al submódulo {string}")]
        [When("Se ingresa al submódulo {string}")]
        public void SeIngresaAlSubmodulo(string submodulo)
        {
            planServicioPage.NavegarAPlanDeServicio();
        }

        [Given("Se selecciona {string}")]
        public void GivenSeSelecciona(string opcion)
        {
            // Ejemplo: "Detalles del Plan"
            // La acción real se ejecuta al configurar los -
        }

        // ===================== WHEN =====================

        [When("Se configuran los límites de los comprobantes:")]
        public void WhenSeConfiguranLosLimitesDeLosComprobantes(DataTable dataTable)
        {
            string minimo = "";
            string maximo = "";

            foreach (var row in dataTable.Rows)
            {
                string campo = row["Campo"].Trim();
                string valor = row["Valor"].Trim();

                if (campo.Equals("Valor mínimo", StringComparison.OrdinalIgnoreCase))
                {
                    minimo = valor;
                }
                else if (campo.Equals("Valor máximo", StringComparison.OrdinalIgnoreCase))
                {
                    maximo = valor;
                }
            }

            planServicioPage.ConfigurarLimitesComprobantes(minimo, maximo);
        }

        [When("Se configuran los límites de locales y usuarios:")]
        public void WhenSeConfiguranLosLimitesDeLocalesYUsuarios(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                string entidad = row["Entidad"];
                string minimo = row["Mínimo"];
                string maximo = row["Máximo"];

                planServicioPage.ConfigurarLimitesLocalesYUsuarios(entidad, minimo, maximo);
            }
        }

        [When("Se selecciona la pestaña {string}")]
        public void WhenSeSeleccionaLaPestana(string pestana)
        {
            // La pestaña "Datos Generales" se abre dentro del PageObject
        }

        [When("Se ingresa la información básica del plan:")]
        public void WhenSeIngresaLaInformacionBasicaDelPlan(DataTable dataTable)
        {
            string nombre = "";
            string descripcion = "";

            foreach (var row in dataTable.Rows)
            {
                string campo = row["Campo"].Trim();
                string valor = row["Valor"].Trim();

                if (campo.Equals("Nombre del plan", StringComparison.OrdinalIgnoreCase))
                {
                    // Genera el nombre dinámico (Ej: Plan Agro 1703_1020)
                    string timeStamp = DateTime.Now.ToString("ddMM_HHmm");
                    nombre = $"{valor} {timeStamp}";

                    // CRUCIAL: Guardamos el nombre en el contexto para que 
                    // la prueba de "Registro de Cliente" sepa cuál seleccionar.
                    _scenarioContext["NombrePlanActual"] = nombre;

                    Console.WriteLine($"DEBUG: Nombre dinámico generado y guardado: {nombre}");
                }
                else if (campo.Equals("Descripción", StringComparison.OrdinalIgnoreCase))
                {
                    descripcion = valor;
                }
            }

            // Enviamos los datos al Page Object
            planServicioPage.CompletarDatosGenerales(nombre, descripcion, "", "");
        }

        [When("Se selecciona el ciclo de facturación {string}")]
        public void WhenSeSeleccionaElCicloDeFacturacion(string ciclo)
        {
            // Se reutiliza el método del PageObject
            planServicioPage.CompletarDatosGenerales("", "", ciclo, "");
        }

        [When("Se ingresa el precio del plan {string}")]
        public void WhenSeIngresaElPrecioDelPlan(string precio)
        {
            planServicioPage.CompletarDatosGenerales("", "", "", precio);
        }

        // ===================== THEN =====================

        [Then("Se procede a {string} los cambios del plan")]
        public void ThenSeProcedeALosCambiosDelPlan(string accion)
        {
            if (accion.ToUpper().Contains("GUARDAR"))
            {
                planServicioPage.ClickGuardar();
            }
        }

        [Then(@"Se confirma el registro exitoso")]
        public void ThenSeConfirmaElRegistroExitoso()
        {
            planServicioPage.ConfirmarRegistroCorrecto();
        }

        [When(@"Se crea un nuevo plan con nombre dinámico")]
        public void CrearPlanDinamico()
        {
            nombrePlan = "PlanQA_" + DateTime.Now.ToString("yyyyMMddHHmmss");

            planServicioPage.ConfigurarLimitesComprobantes("50", "500");
            planServicioPage.ConfigurarLimitesLocalesYUsuarios("Locales", "1", "5");
            planServicioPage.ConfigurarLimitesLocalesYUsuarios("Usuarios", "2", "15");

            planServicioPage.CompletarDatosGenerales(nombrePlan, "Plan QA automático", "MENSUAL", "100");

            planServicioPage.ClickGuardar();
            planServicioPage.ConfirmarRegistroCorrecto();

            Thread.Sleep(2000);
            driver.Navigate().Refresh();
        }

        [When(@"Se busca el plan creado")]
        public void BuscarPlan()
        {
            planServicioPage.BuscarPlan(nombrePlan);
        }

        [When(@"Se selecciona el plan en estado 'Activo'")]
        public void SeleccionarPlan()
        {
            planServicioPage.SeleccionarPlan(nombrePlan);
        }

        [When(@"Se hace clic en 'Solicitar Baja'")]
        public void ClickBaja()
        {
            planServicioPage.ClickSolicitarBaja(nombrePlan);
        }

        [When(@"En el modal se selecciona 'Si'")]
        public void ModalSi()
        {
            planServicioPage.ConfirmarModalSi();
        }

        [When(@"En el modal se selecciona 'No'")]
        public void ModalNo()
        {
            planServicioPage.ConfirmarModalNo();
        }

        [Then(@"Se valida que el estado del plan cambie a 'Dado de Baja'")]
        public void ValidarBaja()
        {
            // opcional: manejar modal
            planServicioPage.ManejarModalOkSiExiste();

            // VALIDACIÓN REAL
            planServicioPage.EsperarQueDesaparezcaPlan(nombrePlan);

            Assert.IsFalse(planServicioPage.ExistePlan(nombrePlan),
                "El plan aún aparece en la lista después de darlo de baja");
        }

        [Then(@"Se valida que el estado del plan permanezca como 'Activo'")]
        public void ValidarActivo()
        {
            Assert.IsTrue(planServicioPage.EstaActivo(nombrePlan));
        }

        [Then(@"Se confirma la operación exitosa")]
        public void ConfirmarOperacionExitosa()
        {
            planServicioPage.ConfirmarRegistroCorrecto();
        }

        [When(@"Se busca un plan existente")]
        public void BuscarPlanExistente()
        {
            if (string.IsNullOrEmpty(nombrePlan))
                nombrePlan = "PlanQA_Default";

            planServicioPage.BuscarPlan(nombrePlan);
        }

        [When(@"Si no existe un plan, se crea uno nuevo")]
        public void CrearSiNoExiste()
        {
            nombrePlan = planServicioPage.BuscarOCrearPlan(nombrePlan);
            Console.WriteLine($"DEBUG: Plan actual para el escenario: {nombrePlan}");
        }

        [When(@"Se hace clic en 'Editar Plan'")]
        public void ClickEditarPlan()
        {
            planServicioPage.ClickEditarPlan(nombrePlan);

            planServicioPage.EliminarCicloSiExiste();
        }

        [When(@"Se hace clic en 'Guardar'")]
        public void ClickGuardarEdicion()
        {
            planServicioPage.ClickGuardarCambios();
        }

        [When(@"En la alerta de confirmación se hace clic en 'OK'")]
        public void ConfirmarAlertaOK()
        {
            planServicioPage.ManejarModalOkSiExiste();
        }

        [Then(@"Se valida que el plan fue actualizado correctamente")]
        public void ValidarEdicion()
        {
            // Puedes mejorar esto luego (precio, ciclo, etc.)
            Assert.IsTrue(planServicioPage.ExistePlan(nombrePlan),
                "El plan no existe después de la edición");
        }

        [When("Se ingresa el nuevo monto {string}")]
        public void WhenSeIngresaElNuevoMonto(string monto)
        {
            planServicioPage.CompletarDatosGenerales("", "", "", monto);
        }

        [When("Se configuran los límites de usuarios:")]
        public void WhenSeConfiguranLosLimitesDeUsuarios(DataTable table)
        {
            string min = table.Rows[0]["Min"];
            string max = table.Rows[0]["Max"];

            planServicioPage.ConfigurarLimitesLocalesYUsuarios("Usuarios", min, max);
        }

        [When("Se configuran los límites de comprobantes:")]
        public void WhenSeConfiguranLosLimitesDeComprobantes(DataTable table)
        {
            string min = table.Rows[0]["Min"];
            string max = table.Rows[0]["Max"];

            planServicioPage.ConfigurarLimitesComprobantes(min, max);
        }

        [When("Se configuran los límites de locales:")]
        public void WhenSeConfiguranLosLimitesDeLocales(DataTable table)
        {
            string min = table.Rows[0]["Min"];
            string max = table.Rows[0]["Max"];

            planServicioPage.ConfigurarLimitesLocalesYUsuarios("Locales", min, max);
        }

        [When(@"Si no existe un plan, se crea uno nuevo y se desactiva")]
        public void CrearYDesactivar()
        {
            planServicioPage.BuscarOCrearPlan(nombrePlan);

            // Desactivar
            planServicioPage.ClickToggleEstado(nombrePlan);
            planServicioPage.ConfirmarModalSi();
        }

        [When(@"Se hace clic en el toggle de estado del plan")]
        public void ClickToggleEstadoPlan()
        {
            planServicioPage.ClickToggleEstado(nombrePlan);
        }

        [Then(@"Se valida que el estado del plan cambie a 'Inactivo'")]
        public void ValidarPlanInactivoYNoDisponible()
        {
            // 1️⃣ Validar toggle = false
            bool activo = planServicioPage.EstaActivo(nombrePlan);
            Assert.IsFalse(activo, "❌ El plan sigue activo (toggle ON)");

            Console.WriteLine("✅ Plan está INACTIVO correctamente");

            // 2️⃣ Ir a Nuevo Cliente
            ClientesPage clientesPage = new ClientesPage(driver);

            clientesPage.IrFacturacionCiclica();
            clientesPage.ClickNuevoCliente();

            // 3️⃣ Abrir Facturación (CLAVE)
            clientesPage.AbrirFacturacion();

            // 4️⃣ Seleccionar ciclo mensual
            clientesPage.SeleccionarCicloFacturacion("MENSUAL");

            // 5️⃣ Validar que NO aparece el plan
            bool existeEnCombo = clientesPage.ExistePlanEnClientes(nombrePlan);

            Assert.IsFalse(existeEnCombo, $"❌ El plan {nombrePlan} aparece y NO debería");

            Console.WriteLine("✅ Plan NO aparece en Nuevo Cliente (correcto)");
        }

        [When(@"Se selecciona el plan en estado 'Inactivo'")]
        public void SeleccionarPlanInactivo()
        {
            // 🔥 Buscar nuevamente el plan (por si cambió estado)
            planServicioPage.BuscarPlan(nombrePlan);

            // 🔥 Validar que realmente esté inactivo
            bool activo = planServicioPage.EstaActivo(nombrePlan);
            Assert.IsFalse(activo, "❌ El plan no está en estado INACTIVO");

            // 🔥 Seleccionar
            planServicioPage.SeleccionarPlan(nombrePlan);

            Console.WriteLine("✅ Plan en estado INACTIVO seleccionado");
        }

        [Then(@"Se valida que el estado del plan cambie a 'Activo'")]
        public void ValidarPlanActivo()
        {
            // 1️⃣ Validar toggle = true
            bool activo = planServicioPage.EstaActivo(nombrePlan);
            Assert.IsTrue(activo, "❌ El plan sigue INACTIVO");

            Console.WriteLine("✅ Plan está ACTIVO correctamente");

            // 2️⃣ Ir a Nuevo Cliente
            ClientesPage clientesPage = new ClientesPage(driver);

            clientesPage.IrFacturacionCiclica();
            clientesPage.ClickNuevoCliente();

            // 🔥 CLAVE
            clientesPage.AbrirFacturacion();

            // 3️⃣ Seleccionar ciclo mensual
            clientesPage.SeleccionarCicloFacturacion("MENSUAL");

            // 4️⃣ Validar que SÍ aparece el plan
            Console.WriteLine("⏳ Buscando y seleccionando plan en Nuevo Cliente...");

            clientesPage.ConfigurarPaginacion("100");
            clientesPage.SeleccionarPlanSeguro(nombrePlan);

            Console.WriteLine("✅ Plan aparece y se puede seleccionar correctamente");

            //Assert.IsTrue(existeEnCombo, $"❌ El plan {nombrePlan} NO aparece y debería");
        }

        [Given(@"se crea un plan de servicio válido")]
        public void GivenSeCreaUnPlanDeServicioValido()
        {
            string nombrePlan = "PlanQA_Default";

            planServicioPage.IrModuloFacturacionCiclica();
            planServicioPage.NavegarAPlanDeServicio();

            nombrePlan = planServicioPage.BuscarOCrearPlan(nombrePlan);

            _scenarioContext["NombrePlanActual"] = nombrePlan;

            Console.WriteLine($"📌 Plan listo: {nombrePlan}");
        }
    }
}