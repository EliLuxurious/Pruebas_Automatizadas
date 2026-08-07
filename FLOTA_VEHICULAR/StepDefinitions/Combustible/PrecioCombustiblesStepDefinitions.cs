using FLOTA_VEHICULAR.Pages.Combustible;
using OpenQA.Selenium;
using Reqnroll;

namespace FLOTA_VEHICULAR.StepDefinitions.Combustible
{
    [Binding]
    public class PrecioCombustibleStepDefinitions
    {
        private readonly PrecioCombustiblePage precioPage;

        public PrecioCombustibleStepDefinitions(IWebDriver driver)
        {
            precioPage = new PrecioCombustiblePage(driver);
        }

        [When(@"Se selecciona el contrato ""(.*)"" y concepto ""(.*)"" en precios")]
        public void WhenSeSeleccionaElContratoYConceptoEnPrecios(string contrato, string concepto)
        {
            precioPage.SeleccionarContratoYConcepto(contrato, concepto);
        }



        [When(@"Se ingresa el valor ""(.*)""")]
        public void WhenSeIngresaElValor(string valor)
        {
            precioPage.IngresarValor(valor);
        }





        [When(@"Se selecciona la fecha de vigencia del dia ""(.*)"" dentro de ""(.*)"" anos")]
        public void WhenSeSeleccionaLaFechaDeVigenciaDelDiaDentroDeAnos(string dia, int anos)
        {
            precioPage.SeleccionarFechaVigencia(dia, anos);
        }

        [When(@"Se ingresa el precio final en planta ""(.*)"" y precio anterior ""(.*)""")]
        public void WhenSeIngresaElPrecioFinalEnPlantaYPrecioAnterior(string pFinal, string pAnterior)
        {
            precioPage.IngresarPreciosPlanta(pFinal, pAnterior);
        }

        [When(@"Se hace clic en el boton COMPROBAR PRECIO")]
        public void WhenSeHaceClicEnElBotonComprobarPrecio()
        {
            precioPage.ClicComprobarPrecio();
        }


        [Then(@"Se verifica que el resultado de la edicion sea ""(.*)""")]
        public void ThenSeVerificaQueElResultadoDeLaEdicionSea(string resultado)
        {
            // Reutilizamos toda la magia de tu método de validación
            precioPage.ValidarResultadoGuardadoPrecio(resultado);
        }



        [Then(@"Se verifica que el resultado del guardado de precio sea ""(.*)""")]
        public void ThenSeVerificaQueElResultadoDelGuardadoDePrecioSea(string resultado)
        {
            precioPage.ValidarResultadoGuardadoPrecio(resultado);
        }



        [When(@"Se busca el contrato ""(.*)"" en la grilla principal")]
        public void WhenSeBuscaElContratoEnLaGrillaPrincipal(string contrato)
        {
            // Llamamos a nuestro nuevo cazador de filtros
            precioPage.BuscarContratoEnGrilla(contrato);
        }

        [When(@"Se hace clic en editar el precio con estado ""(.*)""")]
        public void WhenSeHaceClicEnEditarElPrecioConEstado(string estado)
        {
            precioPage.ClicEditarPrecioPorEstado(estado);
        }

        [When(@"Se modifica el valor a ""(.*)""")]
        public void WhenSeModificaElValorA(string nuevoValor)
        {
            precioPage.IngresarValor(nuevoValor); // Reutilizas el que ya tienes
        }

        [When(@"Se hace clic en el boton Guardar Edicion")]
        public void WhenSeHaceClicEnElBotonGuardarEdicion()
        {
            // No hacemos nada aquí porque tu método ValidarResultadoGuardadoPrecio() 
            // YA TIENE programado el clic al botón guardar. ¡Así evitamos un doble clic fatal!
            Console.WriteLine("⏩ Paso omitido intencionalmente: El clic en Guardar lo hará el método de Validación.");
        }



        [When(@"Se intenta adjuntar el archivo ""(.*)""")]
        public void WhenSeIntentaAdjuntarElArchivo(string rutaArchivo)
        {
            if (rutaArchivo.ToUpper() != "SIN_ARCHIVO")
            {
                // Llama a tu método maestro que ya tenías para adjuntar
                precioPage.AdjuntarDocumento(rutaArchivo);
            }
            else
            {
                Console.WriteLine("⚠️ Se omite adjuntar archivo por solicitud del caso de prueba (SIN_ARCHIVO).");
            }
        }




        [When(@"Se escribe el texto ""(.*)"" en el filtro de texto ""(.*)""")]
        public void WhenSeEscribeElTextoEnElFiltroDeTexto(string texto, string nombreFiltro)
        {
            precioPage.EscribirEnFiltro(nombreFiltro, texto);
        }

        [Then(@"Se verifica que el filtro ""(.*)"" reaccione con el comportamiento ""(.*)""")]
        public void ThenSeVerificaQueElFiltroReaccioneConElComportamiento(string nombreFiltro, string comportamiento)
        {
            precioPage.ValidarComportamientoFiltro(nombreFiltro, comportamiento);
        }

        [Then(@"Se verifica que el sistema calcule el resultado en pantalla como ""(.*)""")]
        public void ThenSeVerificaQueElSistemaCalculeElResultadoEnPantallaComo(string resultadoEsperado)
        {
            precioPage.ValidarCalculoMatematico(resultadoEsperado);
        }











    }
}