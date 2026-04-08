using Reqnroll;
using OpenQA.Selenium;
using SIGES3_0.Pages;
using System;

namespace SIGES3_0.StepDefinitions
{
    [Binding]
    public class GestionDePlanesDeServicioStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly PlanServicioPage planServicioPage;

        public GestionDePlanesDeServicioStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            planServicioPage = new PlanServicioPage(driver);
        }

        // ===================== GIVEN =====================

        [Given("Inicio de sesión con usuario {string} y contraseña {string} en {string}")]
        public void GivenInicioDeSesionConUsuarioYContrasenaEn(string usuario, string password, string url)
        {
            planServicioPage.OpenToApplication(url);
            planServicioPage.LoginToApplication(usuario, password);
        }

        [Given("Se ingresa al módulo {string}")]
        public void GivenSeIngresaAlModulo(string modulo)
        {
            // El texto del feature es solo descriptivo
            planServicioPage.NavegarAPlanDeServicio();
        }

        [Given("Se ingresa al submódulo {string}")]
        public void GivenSeIngresaAlSubmodulo(string submodulo)
        {
            // Ya cubierto en NavegarAPlanDeServicio()
        }

        [Given("Se selecciona {string}")]
        public void GivenSeSelecciona(string opcion)
        {
            // Ejemplo: "Detalles del Plan"
            // La acción real se ejecuta al configurar los límites
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
                    nombre = valor;
                }
                else if (campo.Equals("Descripción", StringComparison.OrdinalIgnoreCase))
                {
                    descripcion = valor;
                }
            }

            // Ciclo y precio se manejan en pasos separados
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
    }
}