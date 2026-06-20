using FLOTA_VEHICULAR.Pages.RevisionTecnica;
using OpenQA.Selenium;
using Reqnroll;

namespace FLOTA_VEHICULAR.StepDefinitions.RevisionTecnica
{
    [Binding]
    public class RevisionTecnicaStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly RevisionTecnicaPage revTecnicaPage;

        public RevisionTecnicaStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            revTecnicaPage = new RevisionTecnicaPage(driver);
        }

        [When(@"Se navega al módulo ""Revisión Técnica""")]
        public void WhenSeNavegaAlModuloRevisionTecnica()
        {
            revTecnicaPage.IngresarModuloRevisionTecnica();
        }

        [When(@"Se selecciona ""+Nuevo"" en Revisión Técnica")]
        public void WhenSeSeleccionaNuevoEnRevisionTecnica()
        {
            revTecnicaPage.ClicNuevo();
        }

        [When(@"Se ingresa la placa ""(.*)"" y se busca en Revisión Técnica")]
        public void WhenSeIngresaLaPlacaYSeBuscaEnRevisionTecnica(string placa)
        {
            revTecnicaPage.IngresarPlacaYBuscar(placa);
        }

        [When(@"Se ingresa el N de certificado ""(.*)""")]
        public void WhenSeIngresaElNDeCertificado(string certificado)
        {
            revTecnicaPage.IngresarCertificado(certificado);
        }

        [When(@"Se selecciona el proveedor de revisión ""(.*)""")]
        public void WhenSeSeleccionaElProveedorDeRevision(string proveedor)
        {
            revTecnicaPage.SeleccionarProveedor(proveedor);
        }

        [When(@"Se selecciona la fecha de revisión el día ""(.*)"" y vencimiento el día ""(.*)"" del próximo año")]
        public void WhenSeSeleccionaLaFechaDeRevisionYVencimientoDelProximoAno(string diaRev, string diaVenc)
        {
            revTecnicaPage.SeleccionarFechasRevisionYVencimiento(diaRev, diaVenc);
        }

        [When(@"Se adjunta el documento de revisión ""(.*)""")]
        public void WhenSeAdjuntaElDocumentoDeRevision(string rutaArchivo)
        {
            revTecnicaPage.AdjuntarDocumento(rutaArchivo);
        }

        [Then(@"Se guarda la Revisión Técnica")]
        public void ThenSeGuardaLaRevisionTecnica()
        {
            revTecnicaPage.GuardarRevisionTecnica();
        }







        [When(@"Se busca la revisión técnica por placa ""(.*)""")]
        public void WhenSeBuscaLaRevisionTecnicaPorPlaca(string placa)
        {
            revTecnicaPage.BuscarEnGrillaPorPlaca(placa);
        }

        [When(@"Se hace clic en ver Revisión Técnica")]
        public void WhenSeHaceClicEnVerRevisionTecnica()
        {
            revTecnicaPage.ClicVerRegistroEnGrilla();
        }

        [When(@"Se hace clic en editar Revisión Técnica")]
        public void WhenSeHaceClicEnEditarRevisionTecnica()
        {
            revTecnicaPage.ClicEditarRegistro();
        }


        [When(@"Se editan las fechas seleccionando el día ""(.*)"" para revisión y el día ""(.*)"" para vencimiento del próximo año")]
        public void WhenSeEditanLasFechasSeleccionandoElDia(string diaRev, string diaVenc)
        {
            revTecnicaPage.EditarFechasPorCalendario(diaRev, diaVenc);
        }




        [When(@"Se hace clic en dar de baja Revisión Técnica")]
        public void WhenSeHaceClicEnDarDeBajaRevisionTecnica()
        {
            revTecnicaPage.ClicDarDeBaja();
        }

        [When(@"Se ingresan las observaciones de baja en Revisión Técnica ""(.*)""")]
        public void WhenSeIngresanLasObservacionesDeBaja(string observaciones)
        {
            revTecnicaPage.IngresarObservacionesBaja(observaciones);
        }

        [Then(@"Se guarda la baja de la Revisión Técnica")]
        public void ThenSeGuardaLaBajaDeLaRevisionTecnica()
        {
            revTecnicaPage.GuardarDarDeBaja();
        }






        [When(@"Se ingresan las fechas de filtro DESDE el día ""(.*)"" y HASTA el día ""(.*)""")]
        public void WhenSeIngresanLasFechasDeFiltroDESDEYHASTA(string diaDesde, string diaHasta)
        {
            revTecnicaPage.SeleccionarRangoFechasFiltro(diaDesde, diaHasta);
        }

        [When(@"Se abre el filtro de Revisión Técnica de ""(.*)""")]
        public void WhenSeAbreElFiltroDeRevisionTecnicaDe(string nombreFiltro)
        {
            revTecnicaPage.AbrirFiltro(nombreFiltro);
        }

        [When(@"Se desmarca la opción TODAS en Revisión Técnica")]
        public void WhenSeDesmarcaLaOpcionTODASEnRevisionTecnica()
        {
            revTecnicaPage.DesmarcarOpcionTodas();
        }

        [When(@"Se selecciona la opción de filtro ""(.*)""")]
        public void WhenSeSeleccionaLaOpcionDeFiltro(string opcion)
        {
            revTecnicaPage.SeleccionarOpcionEnFiltroLista(opcion);
            revTecnicaPage.CerrarComboFiltro();
        }

        [When(@"Se selecciona el Estado de Revisión Técnica ""(.*)""")]
        public void WhenSeSeleccionaElEstadoDeRevisionTecnica(string estado)
        {
            revTecnicaPage.ConfigurarEstadoFiltro(estado);
        }

        [When(@"Se hace clic en el botón BUSCAR filtros de Revisión Técnica")]
        public void WhenSeHaceClicEnElBotonBUSCARFiltrosDeRevisionTecnica()
        {
            revTecnicaPage.ClicBuscarFiltrosAvanzados();
        }

        [Then(@"Se verifica que la grilla muestra resultados para el filtro")]
        public void ThenSeVerificaQueLaGrillaMuestraResultadosParaElFiltro()
        {
            revTecnicaPage.VerificarGrillaFiltrosConResultados();
        }







        // Step para los casos donde el vencimiento es en el mismo año/mes
        [When(@"Se selecciona la fecha de revisión el día ""(.*)"" y vencimiento el día ""(.*)"" del mismo año")]
        public void WhenSeSeleccionaLaFechaDeRevisionYVencimientoDelMismoAno(string diaRev, string diaVenc)
        {
            // Si la revisión es 28 y el vencimiento es 01, sabemos que es el CP-05. Forzamos el error.
            if (diaRev == "28" && diaVenc == "01")
            {
                revTecnicaPage.ForzarFechasIncoherentesPorTeclado(diaRev, diaVenc);
            }
            else
            {
                // Para los demás casos (CP-06, 07, 08) usamos la lógica normal del calendario
                revTecnicaPage.SeleccionarFechasRevisionYVencimientoMismoAno(diaRev, diaVenc);
            }
        }

        // Step general para validar cualquier mensaje de bloqueo o error
        [Then(@"Se valida el mensaje de error ""(.*)""")]
        public void ThenSeValidaElMensajeDeError(string mensajeEsperado)
        {
            revTecnicaPage.ValidarMensajeToast(mensajeEsperado);
        }




        [Then(@"Se valida que el botón Guardar esté deshabilitado")]
        public void ThenSeValidaQueElBotonGuardarEsteDeshabilitado()
        {
            revTecnicaPage.ValidarBotonGuardarDeshabilitado();
        }





        [Then(@"Se valida que la opción de editar está bloqueada u oculta")]
        public void ThenSeValidaQueLaOpcionDeEditarEstaBloqueadaUOculta()
        {
            revTecnicaPage.ValidarBotonEditarBloqueadoU_Oculto();
        }

        [When(@"Se editan las fechas seleccionando el día ""(.*)"" para revisión y el día ""(.*)"" para vencimiento del mismo año")]
        public void WhenSeEditanLasFechasSeleccionandoElDiaParaRevisionYVencimientoDelMismoAno(string diaRev, string diaVenc)
        {
            revTecnicaPage.EditarFechasMismoAno(diaRev, diaVenc);
        }




        [When(@"Se busca la revisión técnica por N° de certificado ""(.*)""")]
        public void WhenSeBuscaLaRevisionTecnicaPorNDeCertificado(string certificado)
        {
            revTecnicaPage.BuscarEnGrillaPorCertificado(certificado);
        }






        [When(@"Se actualiza la página")]
        public void WhenSeActualizaLaPagina()
        {
            revTecnicaPage.RefrescarPagina();
        }






        [When(@"Se fuerzan las fechas por teclado el día ""(.*)"" para revisión y el día ""(.*)"" para vencimiento")]
        public void WhenSeFuerzanLasFechasPorTeclado(string diaRev, string diaVenc)
        {
            // Usamos directamente tu método estrella para esquivar el bloqueo del calendario
            revTecnicaPage.ForzarFechasIncoherentesPorTeclado(diaRev, diaVenc);
        }






        [When(@"Se editan las fechas retrocediendo ""(.*)"" meses seleccionando el día ""(.*)"" para revisión y el día ""(.*)"" para vencimiento")]
        public void WhenSeEditanFechasRetrocediendo(int meses, string diaRev, string diaVenc)
        {
            revTecnicaPage.EditarFechasCompletamenteFlexible(diaRev, diaVenc, meses, 0);
        }

        [When(@"Se editan las fechas avanzando ""(.*)"" meses seleccionando el día ""(.*)"" para revisión y el día ""(.*)"" para vencimiento")]
        public void WhenSeEditanFechasAvanzando(int meses, string diaRev, string diaVenc)
        {
            revTecnicaPage.EditarFechasCompletamenteFlexible(diaRev, diaVenc, 0, meses);
        }





        


        [When(@"Se edita la fecha de vencimiento al estándar caducado de inicio de año")]
        public void WhenSeEditaLaFechaDeVencimientoAlEstandarCaducado()
        {
            revTecnicaPage.EditarFechaVencimientoEstandarCaducado();
        }






        [When(@"Se seleccionan las fechas del año pasado el día ""(.*)"" para revisión y el día ""(.*)"" para vencimiento")]
        public void WhenSeSeleccionanFechasAnoPasado(string diaRev, string diaVenc)
        {
            revTecnicaPage.SeleccionarFechasAnoPasado(diaRev, diaVenc);
        }















        [When(@"Se configuran dinámicamente las fechas por calendario para el estado ""(.*)""")]
        public void WhenSeConfiguranDinamicamenteLasFechasParaElEstado(string estadoDeseado)
        {
            revTecnicaPage.EstablecerEstadoDinamico(estadoDeseado);
        }








    }
}