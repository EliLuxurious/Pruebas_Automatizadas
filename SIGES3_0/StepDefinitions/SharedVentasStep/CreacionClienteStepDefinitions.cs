using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.SharedVentasStep;

namespace SIGES3_0.StepDefinitions.SharedStep
{
    [Binding]
    public class CreacionClienteStepDefinitions
    {
        private readonly CreacionClientePage creacionClientePage;

        public CreacionClienteStepDefinitions(IWebDriver driver)
        {
            creacionClientePage = new CreacionClientePage(driver);
        }

        [StepDefinition("abre el modal de creación de cliente")]
        public void WhenAbreElModalDeCreacionDeCliente()
        {
            creacionClientePage.AbrirModalCreacionCliente();
        }

        [StepDefinition("selecciona tipo de documento {string} con número {string}")]
        public void WhenSeleccionaTipoDeDocumentoConNumero(string tipoDocumento, string numero)
        {
            creacionClientePage.ValidarDocumentoFlow(tipoDocumento, numero);
        }

        [StepDefinition("completa datos generales con género {string}, estado civil {string}, correo {string} y teléfono {string}")]
        public void WhenCompletaDatosGenerales(string genero, string estadoCivil, string correo, string telefono)
        {
            creacionClientePage.DatosGeneralesPersonaNaturalFlow(genero, estadoCivil, correo, telefono);
        }

        [StepDefinition("completa datos de empresa con correo {string} y teléfono {string}")]
        public void WhenCompletaDatosDeEmpresa(string correo, string telefono)
        {
            creacionClientePage.DatosGeneralesPersonaJuridicaFlow(correo, telefono);
        }

        [StepDefinition("ingresa dirección {string}")]
        public void WhenIngresaDireccion(string direccion)
        {
            creacionClientePage.IngresarDireccion(direccion);
        }

        [StepDefinition("guarda y confirma el cliente")]
        public void WhenGuardaYConfirmaElCliente()
        {
            creacionClientePage.GuardarYConfirmarFlow();
        }

        [StepDefinition("el cliente {string} queda registrado en la venta")]
        public void ThenElClienteQuedaRegistradoEnLaVenta(string numeroDocumento)
        {
            creacionClientePage.VerificarClienteEnBarra(numeroDocumento);
        }
    }
}
