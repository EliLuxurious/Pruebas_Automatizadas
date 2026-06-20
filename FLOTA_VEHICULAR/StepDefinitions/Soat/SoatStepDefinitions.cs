using FLOTA_VEHICULAR.Pages.Soat;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace FLOTA_VEHICULAR.StepDefinitions.Soat
{
    [Binding]
    public class SoatStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly SoatPage soatPage;
        private DateTime? fechaDesdeSeleccionadaCP07;
        private DateTime? fechaDesdeSeleccionadaSoat;
        private DateTime? fechaHastaSeleccionadaSoat;

        public SoatStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            soatPage = new SoatPage(driver);
        }

        [When("Se ingresa al módulo SOAT")]
        public void WhenSeIngresaAlModuloSoat() => soatPage.IngresarModuloSoat();

        [When("Se selecciona Nuevo SOAT")]
        public void WhenSeSeleccionaNuevoSoat() => soatPage.ClicNuevoSoat();

        [When("Se ingresa la placa {string} y se busca en SOAT")]
        public void WhenSeIngresaLaPlacaYSeBuscaEnSoat(string placa) => soatPage.IngresarPlacaYBuscar(placa);

        [When("Se selecciona el proveedor {string}")]
        public void WhenSeSeleccionaElProveedor(string proveedor) => soatPage.SeleccionarProveedor(proveedor);

        [When("Se ingresa la póliza {string}")]
        public void WhenSeIngresaLaPoliza(string poliza) => soatPage.IngresarPoliza(poliza);

        // 🔥 STEP DINÁMICO: Recibe la cantidad exacta de días para probar los límites con precisión (0, 20, 21, etc.)
        [When(@"Se configuran las fechas dinámicas sumando ""(.*)"" dias para un SOAT ""(.*)""")]
        public void WhenSeConfiguranLasFechasDinamicasSumandoDiasParaUnSOAT(int dias, string estado)
        {
            DateTime fechaHoy = DateTime.Today;

            DateTime fechaHasta = fechaHoy.AddDays(dias);
            DateTime fechaDesde = fechaHasta.AddYears(-1);
            DateTime fechaContratante = fechaDesde;

            fechaDesdeSeleccionadaSoat = fechaDesde;
            fechaHastaSeleccionadaSoat = fechaHasta;

            soatPage.SeleccionarFechasVigencia(fechaDesde, fechaHasta);
            soatPage.SeleccionarFechaContratante(fechaContratante);
        }



        [When(@"Se configuran las fechas de vigencia del SOAT iniciando en ""(.*)"" dias y con duracion de ""(.*)"" dias")]
        public void WhenSeConfiguranLasFechasDeVigenciaDelSOATIniciandoEnDiasYConDuracionDeDias(int diasInicio, int diasDuracion)
        {
            DateTime fechaDesde = DateTime.Today.AddDays(diasInicio);
            DateTime fechaHasta = fechaDesde.AddDays(diasDuracion);

            fechaDesdeSeleccionadaSoat = fechaDesde;
            fechaHastaSeleccionadaSoat = fechaHasta;

            soatPage.SeleccionarFechasVigencia(fechaDesde, fechaHasta);
        }

        [When(@"Se selecciona la fecha de contratante igual al DESDE del SOAT")]
        public void WhenSeSeleccionaLaFechaDeContratanteIgualAlDESDEDelSOAT()
        {
            if (!fechaDesdeSeleccionadaSoat.HasValue)
            {
                throw new Exception("Fallo de QA: No se tiene registrada la fecha DESDE del SOAT.");
            }

            soatPage.SeleccionarFechaContratante(fechaDesdeSeleccionadaSoat.Value);
        }

        [When(@"Se selecciona la fecha de contratante ""(.*)"" dias despues del DESDE del SOAT")]
        public void WhenSeSeleccionaLaFechaDeContratanteDiasDespuesDelDESDEDelSOAT(int diasDespues)
        {
            if (!fechaDesdeSeleccionadaSoat.HasValue)
            {
                throw new Exception("Fallo de QA: No se tiene registrada la fecha DESDE del SOAT.");
            }

            DateTime fechaContratante = fechaDesdeSeleccionadaSoat.Value.AddDays(diasDespues);
            soatPage.SeleccionarFechaContratante(fechaContratante);
        }





        [When(@"Se escriben las fechas DESDE ""(.*)"" y HASTA ""(.*)""")]
        public void WhenSeEscribenLasFechasDESDEYHASTA(string fechaDesde, string fechaHasta)
        {
            DateTime dDesde = DateTime.ParseExact(fechaDesde, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dHasta = DateTime.ParseExact(fechaHasta, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            soatPage.SeleccionarFechasVigencia(dDesde, dHasta);
        }

        [When(@"Se escribe la fecha de contratante ""(.*)""")]
        public void WhenSeEscribeLaFechaDeContratante(string fecha)
        {
            DateTime dContra = DateTime.ParseExact(fecha, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            soatPage.SeleccionarFechaContratante(dContra);
        }

        [When("Se ingresa el RUC {string} y se busca")]
        public void WhenSeIngresaElRucYSeBusca(string ruc) => soatPage.IngresarRucYBuscar(ruc);

        [When("Se ingresa la hora de emisión {string} y el importe {string}")]
        public void WhenSeIngresaLaHoraDeEmisionYElImporte(string hora, string importe) => soatPage.IngresarHoraEImporte(hora, importe);

        [When("Se adjunta el documento {string}")]
        public void WhenSeAdjuntaElDocumento(string rutaArchivo) => soatPage.AdjuntarDocumento(rutaArchivo);

        [Then("Se guarda el SOAT")]
        public void ThenSeGuardaElSOAT() => soatPage.GuardarSoat();

        [When("Se ingresa la placa {string} sin buscar en SOAT")]
        public void WhenSeIngresaLaPlacaSinBuscarEnSoat(string placa) => soatPage.IngresarPlacaSinBuscar(placa);

        [When("Se ingresa el RUC {string} sin buscar")]
        public void WhenSeIngresaElRucSinBuscar(string ruc) => soatPage.IngresarRucSinBuscar(ruc);

        [Then("Se verifica que el boton Guardar del SOAT esta deshabilitado")]
        public void ThenSeVerificaQueElBotonGuardarDelSOATEstaDeshabilitado() => soatPage.VerificarBotonGuardarDeshabilitado();

        [Then("Se verifica que el día {string} está deshabilitado en el calendario HASTA")]
        public void ThenSeVerificaQueElDiaEstaDeshabilitadoEnElCalendarioHasta(string dia) => soatPage.VerificarDiaHastaDeshabilitado(dia);

        [Then("Se verifica el mensaje de error del SOAT {string}")]
        public void ThenSeVerificaElMensajeDeErrorDelSOAT(string mensajeError) => soatPage.VerificarMensajeErrorSoat(mensajeError);

        [When("Se abre el calendario DESDE del SOAT")]
        public void WhenSeAbreElCalendarioDesdeDelSoat() => soatPage.AbrirCalendarioDesdeParaValidacion();

        [When("Se selecciona solo la fecha DESDE el día {string}")]
        public void WhenSeSeleccionaSoloLaFechaDesdeElDia(string dia) => soatPage.SeleccionarSoloFechaDesde(dia);

        [When("Se busca el SOAT por placa {string}")]
        public void WhenSeBuscaElSOATPorPlaca(string placa) => soatPage.BuscarSoatEnGrillaPorPlaca(placa);

        [When("Se hace clic en ver SOAT")]
        public void WhenSeHaceClicEnVerSOAT() => soatPage.ClicVerSoat();

        [When("Se hace clic en editar SOAT")]
        public void WhenSeHaceClicEnEditarSOAT() => soatPage.ClicEditarSoat();

        [When("Se elimina el documento adjunto")]
        public void WhenSeEliminaElDocumentoAdjunto() => soatPage.EliminarDocumentoAdjunto();

        [When("Se hace clic en el boton Buscar Filtros")]
        public void WhenSeHaceClicEnElBotonBuscarFiltros() => soatPage.ClicBuscarFiltros();

        [Then("Se verifica que la grilla de SOAT muestra resultados")]
        public void ThenSeVerificaQueLaGrillaDeSOATMuestraResultados() => soatPage.VerificarGrillaConResultados();


        [Then(@"Se verifica que el SOAT de la placa ""(.*)"" se registró correctamente")]
        public void ThenSeVerificaQueElSOATDeLaPlacaSeRegistroCorrectamente(string placa)
        {
            soatPage.VerificarSoatRegistradoPorPlaca(placa);
        }


        [When(@"Se abre el filtro de ""(.*)""")]
        public void WhenSeAbreElFiltroDe(string nombreFiltro) => soatPage.AbrirFiltro(nombreFiltro);

        [When(@"Se desmarca la opcion TODAS")]
        public void WhenSeDesmarcaLaOpcionTODAS() => soatPage.DesmarcarOpcionTodas();

        [When(@"Se seleccionan las siguientes aseguradoras:")]
        public void WhenSeSeleccionanLasSiguientesAseguradoras(Table table)
        {
            foreach (var row in table.Rows) soatPage.SeleccionarOpcionEnFiltro(row["Aseguradora"]);
            soatPage.CerrarComboFiltro();
        }

        [When(@"Se ingresa la fecha de vencimiento DESDE ""(.*)"" y HASTA ""(.*)"" en los filtros")]
        public void WhenSeIngresaLaFechaDeVencimientoDESDEYHASTAEnLosFiltros(string fechaDesde, string fechaHasta) => soatPage.IngresarRangoFechasFiltro(fechaDesde, fechaHasta);

        [When(@"Se filtran fechas de vencimiento desde hoy hasta dentro de ""(.*)"" dias")]
        public void WhenSeFiltranFechasDesdeHoyHasta(int diasEnElFuturo)
        {
            DateTime desde = DateTime.Today;
            DateTime hasta = DateTime.Today.AddDays(diasEnElFuturo);

            soatPage.SeleccionarRangoFechasFiltro(desde, hasta);
        }

        [When(@"Se seleccionan las siguientes opciones en el filtro:")]
        public void WhenSeSeleccionanLasSiguientesOpcionesEnElFiltro(Table table)
        {
            foreach (var row in table.Rows) soatPage.SeleccionarOpcionEnFiltro(row["Opcion"]);
            soatPage.CerrarComboFiltro();
        }

        [When(@"Se ingresa ""(.*)"" en dias para vencer")]
        public void WhenSeIngresaEnDiasParaVencer(string dias) => soatPage.IngresarDiasParaVencer(dias);

        [When(@"Se hace clic en el boton Historial")]
        public void WhenSeHaceClicEnElBotonHistorial() => soatPage.ClicHistorial();

        [Then(@"Se cierra el historial del SOAT")]
        public void ThenSeCierraElHistorialDelSOAT() => soatPage.CerrarHistorial();


        [When(@"Se selecciona la fecha DESDE del SOAT sumando ""(.*)"" dias")]
        public void WhenSeSeleccionaLaFechaDESDEDelSOATSumandoDias(int dias)
        {
            DateTime fechaDesde = DateTime.Today.AddDays(dias);

            // Evita el caso borde donde el día anterior cae en otro mes no visible del calendario.
            if (fechaDesde.Day == 1)
            {
                fechaDesde = fechaDesde.AddDays(1);
            }

            fechaDesdeSeleccionadaCP07 = fechaDesde;
            soatPage.SeleccionarSoloFechaDesde(fechaDesde);
        }

        [Then(@"Se verifica que la fecha anterior al DESDE está deshabilitada en el calendario HASTA")]
        public void ThenSeVerificaQueLaFechaAnteriorAlDESDEEstaDeshabilitadaEnCalendarioHASTA()
        {
            if (!fechaDesdeSeleccionadaCP07.HasValue)
            {
                throw new Exception("Fallo de QA: No se tiene registrada la fecha DESDE para validar el calendario HASTA.");
            }

            DateTime fechaBloqueada = fechaDesdeSeleccionadaCP07.Value.AddDays(-1);
            soatPage.VerificarFechaHastaDeshabilitada(fechaBloqueada);
        }





        [Then("Se verifica que el SOAT no permite continuar sin buscar la placa")]
        public void ThenSeVerificaQueElSOATNoPermiteContinuarSinBuscarLaPlaca()
        {
            soatPage.VerificarSoatBloqueadoPorPlacaSinBuscar();
        }






        [When(@"Se selecciona solo la fecha DESDE de vigencia del SOAT iniciando en ""(.*)"" dias")]
        public void WhenSeSeleccionaSoloLaFechaDESDEDeVigenciaDelSOATIniciandoEnDias(int diasInicio)
        {
            DateTime fechaDesde = DateTime.Today.AddDays(diasInicio);

            fechaDesdeSeleccionadaSoat = fechaDesde;
            soatPage.SeleccionarSoloFechaDesde(fechaDesde);
        }

        [Then(@"Se verifica que la fecha HASTA con duracion de ""(.*)"" dias está deshabilitada")]
        public void ThenSeVerificaQueLaFechaHASTAConDuracionDeDiasEstaDeshabilitada(int diasDuracion)
        {
            if (!fechaDesdeSeleccionadaSoat.HasValue)
            {
                throw new Exception("Fallo de QA: No se tiene registrada la fecha DESDE del SOAT.");
            }

            DateTime fechaHastaBloqueada = fechaDesdeSeleccionadaSoat.Value.AddDays(diasDuracion);

            soatPage.VerificarFechaHastaDeshabilitada(fechaHastaBloqueada);
        }





        [Then(@"Se verifica que la fecha de contratante ""(.*)"" dias despues del HASTA del SOAT está deshabilitada")]
        public void ThenSeVerificaQueLaFechaDeContratanteDiasDespuesDelHASTAEstaDeshabilitada(int diasDespues)
        {
            if (!fechaHastaSeleccionadaSoat.HasValue)
            {
                throw new Exception("Fallo de QA: No se tiene registrada la fecha HASTA del SOAT.");
            }

            DateTime fechaBloqueada = fechaHastaSeleccionadaSoat.Value.AddDays(diasDespues);
            soatPage.VerificarFechaContratanteDeshabilitada(fechaBloqueada, fechaHastaSeleccionadaSoat.Value);
        }

        [Then(@"Se verifica que la fecha de contratante ""(.*)"" dias antes del DESDE del SOAT está deshabilitada")]
        public void ThenSeVerificaQueLaFechaDeContratanteDiasAntesDelDESDEEstaDeshabilitada(int diasAntes)
        {
            if (!fechaDesdeSeleccionadaSoat.HasValue)
            {
                throw new Exception("Fallo de QA: No se tiene registrada la fecha DESDE del SOAT.");
            }

            DateTime fechaBloqueada = fechaDesdeSeleccionadaSoat.Value.AddDays(-diasAntes);
            soatPage.VerificarFechaContratanteDeshabilitada(fechaBloqueada, fechaDesdeSeleccionadaSoat.Value);
        }

        [When(@"Se selecciona la fecha de contratante igual al HASTA del SOAT")]
        public void WhenSeSeleccionaLaFechaDeContratanteIgualAlHASTADelSOAT()
        {
            if (!fechaHastaSeleccionadaSoat.HasValue)
            {
                throw new Exception("Fallo de QA: No se tiene registrada la fecha HASTA del SOAT.");
            }

            soatPage.SeleccionarFechaContratante(fechaHastaSeleccionadaSoat.Value);
        }

        [When(@"Se configuran fechas vencidas del SOAT")]
        public void WhenSeConfiguranFechasVencidasDelSOAT()
        {
            DateTime fechaHasta = DateTime.Today.AddDays(-30);
            DateTime fechaDesde = fechaHasta.AddYears(-1);
            DateTime fechaContratante = fechaDesde;

            fechaDesdeSeleccionadaSoat = fechaDesde;
            fechaHastaSeleccionadaSoat = fechaHasta;

            soatPage.SeleccionarFechasVigencia(fechaDesde, fechaHasta);
            soatPage.SeleccionarFechaContratante(fechaContratante);
        }






        [When(@"Se seleccionan las siguientes areas:")]
        public void WhenSeSeleccionanLasSiguientesAreas(Table table)
        {
            foreach (var row in table.Rows)
            {
                soatPage.SeleccionarOpcionEnFiltro(row["Area"]);
            }

            soatPage.CerrarComboFiltro();
        }

        [When(@"Se filtran fechas de vencimiento desde hace ""(.*)"" dias hasta hoy")]
        public void WhenSeFiltranFechasDeVencimientoDesdeHaceDiasHastaHoy(int diasAtras)
        {
            DateTime desde = DateTime.Today.AddDays(-diasAtras);
            DateTime hasta = DateTime.Today;

            soatPage.SeleccionarRangoFechasFiltro(desde, hasta);
        }





        [When(@"Se selecciona la fecha de contratante ""(.*)"" dias despues del HASTA del SOAT")]
        public void WhenSeSeleccionaLaFechaDeContratanteDiasDespuesDelHASTADelSOAT(int diasDespues)
        {
            if (!fechaHastaSeleccionadaSoat.HasValue)
            {
                throw new Exception("Fallo de QA: No se tiene registrada la fecha HASTA del SOAT.");
            }

            DateTime fechaContratante = fechaHastaSeleccionadaSoat.Value.AddDays(diasDespues);
            soatPage.SeleccionarFechaContratante(fechaContratante);
        }






        [When(@"Se selecciona la fecha de contratante ""(.*)"" dias antes del DESDE del SOAT")]
        public void WhenSeSeleccionaLaFechaDeContratanteDiasAntesDelDESDEDelSOAT(int diasAntes)
        {
            if (!fechaDesdeSeleccionadaSoat.HasValue)
            {
                throw new Exception("Fallo de QA: No se tiene registrada la fecha DESDE del SOAT.");
            }

            DateTime fechaContratante = fechaDesdeSeleccionadaSoat.Value.AddDays(-diasAntes);
            soatPage.SeleccionarFechaContratante(fechaContratante);
        }





    }
}