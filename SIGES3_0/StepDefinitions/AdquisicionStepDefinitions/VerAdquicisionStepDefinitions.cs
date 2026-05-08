using System;
using Reqnroll;
using NUnit.Framework;
using SIGES3_0.Pages.Adquisicion;

namespace SIGES3_0.StepDefinitions.AdquisicionStepDefinitions
{
    [Binding]
    public class VerAdquicisionStepDefinitions
    {
        private readonly VerAdquisicionPage verAdquisicionPage;

        public VerAdquicisionStepDefinitions(OpenQA.Selenium.IWebDriver driver)
        {
            verAdquisicionPage = new VerAdquisicionPage(driver);
        }

        [Given("Entro al submódulo específico de 'Ver Adquisición'")]
        public void GivenEntroAlSubmoduloEspecificoDeVerAdquisicion()
        {
            verAdquisicionPage.IngresarASubmoduloVerAdquisicion();
        }

        [When(@"Hago clic en el botón superior de 'Nueva Compra'")]
        public void WhenHagoClicEnElBotonSuperiorDeNuevaCompra()
        {
            verAdquisicionPage.ClicEnNuevaCompra();
        }

        [When(@"Se configuran los filtros de búsqueda:")]
        public void WhenSeConfiguranLosFiltrosDeBusqueda(DataTable dataTable)
        {
            string fechaInicial = "", fechaFinal = "", proveedor = "";

            foreach (var row in dataTable.Rows)
            {
                string campo = row["Campo"].Trim();
                string valor = row["Valor"].Trim();

                if (campo.Equals("Fecha Inicial", StringComparison.OrdinalIgnoreCase)) fechaInicial = valor;
                else if (campo.Equals("Fecha Final", StringComparison.OrdinalIgnoreCase)) fechaFinal = valor;
                else if (campo.Equals("Proveedor", StringComparison.OrdinalIgnoreCase)) proveedor = valor;
            }

            verAdquisicionPage.ConfigurarFiltros(fechaInicial, fechaFinal, proveedor);
        }

        [When(@"Se hace clic en el botón de buscar")]
        public void WhenSeHaceClicEnElBotonDeBuscar()
        {
            verAdquisicionPage.ClicBuscar();
        }

        [Then(@"El sistema actualiza la tabla mostrando los registros correspondientes al proveedor '(.*)'")]
        public void ThenElSistemaActualizaLaTablaMostrandoLosRegistrosCorrespondientesAlProveedor(string proveedorEsperado)
        {
            bool coinciden = verAdquisicionPage.ValidarRegistrosEnTabla(proveedorEsperado);
            Assert.IsTrue(coinciden, $"Validación fallida: La tabla contiene registros que no pertenecen a {proveedorEsperado} o está vacía.");
        }

        // --- NUEVOS PASOS PARA EL VISOR DE DOCUMENTOS ---

        [When(@"Selecciono el primer registro de la tabla para ver su detalle")]
        public void WhenSeleccionoElPrimerRegistroDeLaTablaParaVerSuDetalle()
        {
            verAdquisicionPage.AbrirVisorPrimerRegistro();
        }

        [Then(@"Verifico que el botón '(.*)' esté disponible en el visor")]
        public void ThenVerificoQueElBotonEsteDisponibleEnElVisor(string accion)
        {
            bool estaVisible = verAdquisicionPage.EsBotonVisibleEnVisor(accion);
            Assert.IsTrue(estaVisible, $"❌ Validación Fallida: El botón '{accion}' no se encontró en el modal.");
        }

        [When(@"Cambio el formato del documento a '(.*)'")]
        public void WhenCambioElFormatoDelDocumentoA(string formato)
        {
            verAdquisicionPage.CambiarFormatoDocumento(formato);
        }

        [Then(@"Ejecuto la acción de '(.*)' el documento")]
        public void ThenEjecutoLaAccionDeElDocumento(string accion)
        {
            verAdquisicionPage.EjecutarAccionDocumento(accion);
        }

        [Then(@"Valido que la acción de '(.*)' se ejecutó correctamente")]
        public void ThenValidoQueLaAccionDeSeEjecutoCorrectamente(string accion)
        {
            bool fueExitoso = verAdquisicionPage.ValidarAccionExitosa(accion);
            Assert.IsTrue(fueExitoso, $"❌ Falló la validación para la acción: {accion}. (Si fue descarga, revisa tu carpeta Downloads).");
        }

        //Notas
        [When(@"Se hace click en el boton de 'Nota de Credito'")]
        public void WhenSeHaceClickEnElBotonDeNotaDeCredito()
        {
            verAdquisicionPage.ConfigurarNotaCredito();

            Thread.Sleep(2000);
            Assert.Inconclusive("⚠️ AVISO: Se hizo clic en 'Nota de Crédito' correctamente, pero falta implementar y automatizar el resto de este formulario en el sistema.");
        }
        [When(@"Se hace click en el boton de 'Nota de Debito'")]
        public void WhenSeHaceClickEnElBotonDeNotaDeDebito()
        {
            verAdquisicionPage.ConfigurarNotaDebito();

            Thread.Sleep(2000);
            Assert.Inconclusive("⚠️ AVISO: Se hizo clic en 'Nota de Débito' correctamente, pero falta implementar y automatizar el resto de este formulario en el sistema.");
        }


    }
}