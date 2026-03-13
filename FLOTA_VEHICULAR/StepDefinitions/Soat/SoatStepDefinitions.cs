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

        public SoatStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            soatPage = new SoatPage(driver);
        }

        [When("Se ingresa al módulo SOAT")]
        public void WhenSeIngresaAlModuloSoat()
        {
            soatPage.IngresarModuloSoat();
        }

        [When("Se selecciona Nuevo SOAT")]
        public void WhenSeSeleccionaNuevoSoat()
        {
            soatPage.ClicNuevoSoat();
        }

        [When("Se ingresa la placa {string} y se busca en SOAT")]
        public void WhenSeIngresaLaPlacaYSeBuscaEnSoat(string placa)
        {
            soatPage.IngresarPlacaYBuscar(placa);
        }

        [When("Se selecciona el proveedor {string}")]
        public void WhenSeSeleccionaElProveedor(string proveedor)
        {
            soatPage.SeleccionarProveedor(proveedor);
        }

        [When("Se ingresa la póliza {string}")]
        public void WhenSeIngresaLaPoliza(string poliza)
        {
            soatPage.IngresarPoliza(poliza);
        }

        [When("Se selecciona la fecha DESDE el día {string} y HASTA el día {string} del próximo año")]
        public void WhenSeSeleccionaLaFechaDesdeElDiaYHastaElDiaDelProximoAno(string diaDesde, string diaHasta)
        {
            soatPage.SeleccionarFechaDesdeYHastaUnAnoDespues(diaDesde, diaHasta);
        }

        [When("Se ingresa el RUC {string} y se busca")]
        public void WhenSeIngresaElRucYSeBusca(string ruc)
        {
            soatPage.IngresarRucYBuscar(ruc);
        }

        [When("Se selecciona la fecha del contratante el día {string}")]
        public void WhenSeSeleccionaLaFechaDelContratanteElDia(string dia)
        {
            soatPage.SeleccionarFechaContratante(dia);
        }

        [When("Se ingresa la hora de emisión {string} y el importe {string}")]
        public void WhenSeIngresaLaHoraDeEmisionYElImporte(string hora, string importe)
        {
            soatPage.IngresarHoraEImporte(hora, importe);
        }

        [When("Se adjunta el documento {string}")]
        public void WhenSeAdjuntaElDocumento(string rutaArchivo)
        {
            soatPage.AdjuntarDocumento(rutaArchivo);
        }

        [Then("Se guarda el SOAT")]
        public void ThenSeGuardaElSOAT()
        {
            // Ya no usamos ValidarFormularioCompleto, vamos directo al guardado validado
            soatPage.GuardarSoat();
        }





        [When("Se ingresa la placa {string} sin buscar en SOAT")]
        public void WhenSeIngresaLaPlacaSinBuscarEnSoat(string placa)
        {
            soatPage.IngresarPlacaSinBuscar(placa);
        }

        [When("Se ingresa el RUC {string} sin buscar")]
        public void WhenSeIngresaElRucSinBuscar(string ruc)
        {
            soatPage.IngresarRucSinBuscar(ruc);
        }

        [Then("Se verifica que el boton Guardar del SOAT esta deshabilitado")]
        public void ThenSeVerificaQueElBotonGuardarDelSOATEstaDeshabilitado()
        {
            soatPage.VerificarBotonGuardarDeshabilitado();
        }







        [Then("Se verifica que el día {string} está deshabilitado en el calendario HASTA")]
        public void ThenSeVerificaQueElDiaEstaDeshabilitadoEnElCalendarioHasta(string dia)
        {
            soatPage.VerificarDiaHastaDeshabilitado(dia);
        }

        [Then("Se verifica el mensaje de error del SOAT {string}")]
        public void ThenSeVerificaElMensajeDeErrorDelSOAT(string mensajeError)
        {
            soatPage.VerificarMensajeErrorSoat(mensajeError);
        }





        [When("Se selecciona solo la fecha DESDE el día {string}")]
        public void WhenSeSeleccionaSoloLaFechaDesdeElDia(string dia)
        {
            soatPage.SeleccionarSoloFechaDesde(dia);
        }






        [When("Se busca el SOAT por placa {string}")]
        public void WhenSeBuscaElSOATPorPlaca(string placa)
        {
            soatPage.BuscarSoatEnGrillaPorPlaca(placa);
        }

        [When("Se hace clic en ver SOAT")]
        public void WhenSeHaceClicEnVerSOAT()
        {
            soatPage.ClicVerSoat();
        }

        [When("Se hace clic en editar SOAT")]
        public void WhenSeHaceClicEnEditarSOAT()
        {
            soatPage.ClicEditarSoat();
        }

        [When("Se elimina el documento adjunto")]
        public void WhenSeEliminaElDocumentoAdjunto()
        {
            soatPage.EliminarDocumentoAdjunto();
        }


        [When("Se hace clic en el boton Buscar Filtros")]
        public void WhenSeHaceClicEnElBotonBuscarFiltros()
        {
            // Usaremos el mismo método que creaste para Odómetro si el botón es igual
            soatPage.ClicBuscarFiltros();
        }

        [Then("Se verifica que la grilla de SOAT muestra resultados")]
        public void ThenSeVerificaQueLaGrillaDeSOATMuestraResultados()
        {
            soatPage.VerificarGrillaConResultados();
        }



        [When(@"Se abre el filtro de ""(.*)""")]
        public void WhenSeAbreElFiltroDe(string nombreFiltro)
        {
            soatPage.AbrirFiltro(nombreFiltro);
        }

        [When(@"Se desmarca la opcion TODAS")]
        public void WhenSeDesmarcaLaOpcionTODAS()
        {
            soatPage.DesmarcarOpcionTodas();
        }

        [When(@"Se seleccionan las siguientes aseguradoras:")]
        public void WhenSeSeleccionanLasSiguientesAseguradoras(Table table)
        {
            foreach (var row in table.Rows)
            {
                string aseguradora = row["Aseguradora"];
                soatPage.SeleccionarOpcionEnFiltro(aseguradora);
            }
            // Presionamos Escape para cerrar el combo flotante
            soatPage.CerrarComboFiltro();
        }

        [When(@"Se ingresa la fecha de vencimiento DESDE ""(.*)"" y HASTA ""(.*)"" en los filtros")]
        public void WhenSeIngresaLaFechaDeVencimientoDESDEYHASTAEnLosFiltros(string fechaDesde, string fechaHasta)
        {
            soatPage.IngresarRangoFechasFiltro(fechaDesde, fechaHasta);
        }






        [When(@"Se seleccionan las siguientes opciones en el filtro:")]
        public void WhenSeSeleccionanLasSiguientesOpcionesEnElFiltro(Table table)
        {
            foreach (var row in table.Rows)
            {
                string opcion = row["Opcion"];
                soatPage.SeleccionarOpcionEnFiltro(opcion);
            }
            soatPage.CerrarComboFiltro();
        }

        [When(@"Se ingresa ""(.*)"" en dias para vencer")]
        public void WhenSeIngresaEnDiasParaVencer(string dias)
        {
            soatPage.IngresarDiasParaVencer(dias);
        }

        [When(@"Se hace clic en el boton Historial")]
        public void WhenSeHaceClicEnElBotonHistorial()
        {
            soatPage.ClicHistorial();
        }

        [Then(@"Se cierra el historial del SOAT")]
        public void ThenSeCierraElHistorialDelSOAT()
        {
            soatPage.CerrarHistorial();
        }




        [When(@"Se selecciona la fecha DESDE el día ""(.*)"" y HASTA el día ""(.*)"" del mismo mes")]
        public void WhenSeSeleccionaLaFechaDesdeElDiaYHastaElDiaDelMismoMes(string diaDesde, string diaHasta)
        {
            // Reutilizamos un método que ya tenías creado en SoatPage, ¡pero que no habíamos usado!
            soatPage.SeleccionarFechaDesdeYHasta(diaDesde, diaHasta);
        }

        [When(@"Se escriben las fechas DESDE ""(.*)"" y HASTA ""(.*)""")]
        public void WhenSeEscribenLasFechasDESDEYHASTA(string fechaDesde, string fechaHasta)
        {
            soatPage.EscribirFechasVigencia(fechaDesde, fechaHasta);
        }




        [When(@"Se selecciona la fecha DESDE el día ""(.*)"" y HASTA el día ""(.*)"" del año pasado")]
        public void WhenSeSeleccionaLaFechaDesdeElDiaYHastaElDiaDelAnoPasado(string diaDesde, string diaHasta)
        {
            soatPage.SeleccionarFechaDesdeYHastaAnoPasado(diaDesde, diaHasta);
        }





    }
}