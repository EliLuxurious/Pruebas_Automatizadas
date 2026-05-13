using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.SharedVentasPage;

namespace SIGES3_0.StepDefinitions.SharedVentasStep
{
    [Binding]
    public class CreacionClienteStepDefinitions
    {
        private readonly CreacionClientePage creacionClientePage;

        public CreacionClienteStepDefinitions(IWebDriver driver)
        {
            creacionClientePage = new CreacionClientePage(driver);
        }

        [When(@"abre el modal de creación de cliente")]
        public void WhenAbreElModalDeCreacionDeCliente()
        {
            creacionClientePage.AbrirModalCreacionCliente();
        }

        [When(@"selecciona tipo de documento '(.*)' con número '(.*)'")]
        public void WhenSeleccionaTipoDeDocumentoConNumero(string tipoDocumento, string numeroDocumento)
        {
            creacionClientePage.ValidarDocumentoFlow(tipoDocumento, numeroDocumento);
        }

        [When(@"completa datos generales con género '(.*)', estado civil '(.*)', correo '(.*)' y teléfono '(.*)'")]
        public void WhenCompletaDatosGeneralesConGeneroEstadoCivilCorreoYTelefono(
            string genero,
            string estadoCivil,
            string correo,
            string telefono)
        {
            creacionClientePage.DatosGeneralesPersonaNaturalFlow(genero, estadoCivil, correo, telefono);
        }

        [When(@"completa datos de empresa con correo '(.*)' y teléfono '(.*)'")]
        public void WhenCompletaDatosDeEmpresaConCorreoYTelefono(string correo, string telefono)
        {
            creacionClientePage.DatosGeneralesPersonaJuridicaFlow(correo, telefono);
        }

        [When(@"ingresa dirección '(.*)'")]
        public void WhenIngresaDireccion(string direccion)
        {
            creacionClientePage.IngresarDireccion(direccion);
        }

        [When(@"guarda y confirma el cliente")]
        public void WhenGuardaYConfirmaElCliente()
        {
            creacionClientePage.GuardarYConfirmarFlow();
        }

        [Then(@"el cliente '(.*)' queda registrado en la venta")]
        public void ThenElClienteQuedaRegistradoEnLaVenta(string numeroDocumento)
        {
            creacionClientePage.VerificarClienteEnBarra(numeroDocumento);
        }
    }
}
