using FLOTA_VEHICULAR.Pages.Combustible;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace FLOTA_VEHICULAR.StepDefinitions.Combustible
{
    [Binding]
    public class VerAbastecimientosStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly VerAbastecimientosPage verAbastecimientosPage;

        public VerAbastecimientosStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            verAbastecimientosPage = new VerAbastecimientosPage(driver);
        }

        // ==========================================
        // COMUNES
        // ==========================================
        [When(@"Se ingresa al módulo ""(.*)"" y submódulo ""(.*)""")]
        public void WhenSeIngresaAlModuloYSubmodulo(string modulo, string submodulo)
        {
            verAbastecimientosPage.IngresarModuloYSubmodulo(modulo, submodulo);
        }

        [When(@"Se selecciona el boton Nuevo")]
        public void WhenSeSeleccionaElBotonNuevo()
        {
            verAbastecimientosPage.ClicBotonNuevo();
        }

        [When(@"Se adjunta el archivo ""(.*)""")]
        public void WhenSeAdjuntaElArchivo(string rutaArchivo)
        {
            verAbastecimientosPage.AdjuntarDocumento(rutaArchivo);
        }

        [Then(@"Se guarda el registro")]
        public void ThenSeGuardaElRegistro()
        {
            verAbastecimientosPage.ClicBotonGuardar();
        }

        [Then(@"Se verifica que el resultado del guardado sea ""(.*)""")]
        public void ThenSeVerificaQueElResultadoDelGuardadoSea(string resultadoEsperado)
        {
            verAbastecimientosPage.ValidarResultadoGuardadoConLogica(resultadoEsperado);
        }


        // ==========================================
        // FASE 1: CONDUCTOR
        // ==========================================
        [When(@"Se ingresa el DNI ""(.*)"" del conductor y se busca")]
        public void WhenSeIngresaElDniDelConductorYSeBusca(string dni)
        {
            verAbastecimientosPage.LlenarDniConductorYBuscar(dni);
        }

        // 🔥 AQUÍ ESTÁ LA CORRECCIÓN DEL AÑO DE NACIMIENTO
        [When(@"Se selecciona la fecha de nacimiento el dia ""(.*)"" y ano ""(.*)""")]
        public void WhenSeSeleccionaLaFechaDeNacimientoElDiaYAno(string dia, string ano)
        {
            verAbastecimientosPage.SeleccionarFechaNacimiento(dia, ano);
        }

        [When(@"Se selecciona el genero ""(.*)"" y area ""(.*)""")]
        public void WhenSeSeleccionaElGeneroYArea(string genero, string area)
        {
            verAbastecimientosPage.SeleccionarGeneroYArea(genero, area);
        }

        [When(@"Se ingresa el correo ""(.*)"", telefono ""(.*)"" y direccion ""(.*)""")]
        public void WhenSeIngresaElCorreoTelefonoYDireccion(string correo, string telefono, string direccion)
        {
            verAbastecimientosPage.LlenarDatosContacto(correo, telefono, direccion);
        }

        [When(@"Se ingresa la licencia ""(.*)"", clase ""(.*)"" y categoria ""(.*)""")]
        public void WhenSeIngresaLaLicenciaClaseYCategoria(string licencia, string clase, string categoria)
        {
            verAbastecimientosPage.LlenarLicencia(licencia, clase, categoria);
        }

        [When(@"Se selecciona la fecha de expedicion el dia ""(.*)"" y vencimiento el dia ""(.*)"" dentro de ""(.*)"" anos")]
        public void WhenSeSeleccionaLaFechaDeExpedicionYVencimiento(string diaExp, string diaVenc, int anosVencimiento)
        {
            // Le pasamos el nuevo parámetro de años a la página
            verAbastecimientosPage.SeleccionarFechasLicencia(diaExp, diaVenc, anosVencimiento);
        }

        [When(@"Se hace clic en el boton Agregar Licencia")]
        public void WhenSeHaceClicEnElBotonAgregarLicencia()
        {
            verAbastecimientosPage.ClicAgregarLicencia();
        }

        // ==========================================
        // FASE 2: CONTRATO
        // ==========================================
        [When(@"Se ingresa el numero de contrato ""(.*)""")]
        public void WhenSeIngresaElNumeroDeContrato(string contrato)
        {
            verAbastecimientosPage.LlenarNumeroContrato(contrato);
        }

        [When(@"Se selecciona la fecha del contrato DESDE el dia ""(.*)"" y HASTA el dia ""(.*)"" dentro de ""(.*)"" anos")]
        public void WhenSeSeleccionaLaFechaDelContratoDesdeYHastaDentroDeAnos(string diaDesde, string diaHasta, int anos)
        {
            verAbastecimientosPage.SeleccionarFechasContrato(diaDesde, diaHasta, anos);
        }

        [When(@"Se selecciona el tipo ""(.*)"", concepto ""(.*)"" y area ""(.*)""")]
        public void WhenSeSeleccionaElTipoConceptoYArea(string tipo, string concepto, string area)
        {
            verAbastecimientosPage.SeleccionarTipoConceptoArea(tipo, concepto, area);
        }

        [When(@"Se ingresa la cantidad ""(.*)"" y precio unitario ""(.*)""")]
        public void WhenSeIngresaLaCantidadYPrecioUnitario(string cantidad, string precio)
        {
            verAbastecimientosPage.LlenarCantidadYPrecio(cantidad, precio);
        }

        [When(@"Se ingresa el RUC ""(.*)"" del proveedor y se busca")]
        public void WhenSeIngresaElRucDelProveedorYSeBusca(string ruc)
        {
            verAbastecimientosPage.LlenarRucProveedor(ruc);
        }

        [When(@"Se ingresa la direccion ""(.*)"", correo ""(.*)"", telefono ""(.*)"" y clasificacion ""(.*)""")]
        public void WhenSeIngresaLaDireccionCorreoTelefonoYClasificacion(string direccion, string correo, string telefono, string clasificacion)
        {
            verAbastecimientosPage.LlenarDatosProveedor(direccion, correo, telefono, clasificacion);
        }

        // ==========================================
        // FASE 3: ABASTECIMIENTO
        // ==========================================
        [When(@"Se ingresa la placa ""(.*)"" en abastecimiento y se busca")]
        public void WhenSeIngresaLaPlacaEnAbastecimientoYSeBusca(string placa)
        {
            verAbastecimientosPage.IngresarPlacaAbastecimientoYBuscar(placa);
        }

        [When(@"Se ingresa la nota de despacho ""(.*)""")]
        public void WhenSeIngresaLaNotaDeDespacho(string nota)
        {
            verAbastecimientosPage.IngresarNotaDespacho(nota);
        }

        [When(@"Se selecciona la fecha de registro el dia ""(.*)""")]
        public void WhenSeSeleccionaLaFechaDeRegistroElDia(string dia)
        {
            verAbastecimientosPage.SeleccionarFechaRegistro(dia);
        }

        [When(@"Se selecciona el conductor ""(.*)"" en abastecimiento")]
        public void WhenSeSeleccionaElConductorEnAbastecimiento(string conductor)
        {
            verAbastecimientosPage.SeleccionarConductorAbastecimiento(conductor);
        }

        [When(@"Se ingresa la hora de despacho ""(.*)"" y odometro ""(.*)""")]
        public void WhenSeIngresaLaHoraDeDespachoYOdometro(string hora, string odometro)
        {
            verAbastecimientosPage.IngresarHoraYOdometro(hora, odometro);
        }

        [When(@"Se selecciona el area ""(.*)"" y contrato ""(.*)""")]
        public void WhenSeSeleccionaElAreaYContrato(string area, string contrato)
        {
            verAbastecimientosPage.SeleccionarAreaYContrato(area, contrato);
        }

        [When(@"Se selecciona el concepto ""(.*)"" y cantidad ""(.*)""")]
        public void WhenSeSeleccionaElConceptoYCantidad(string concepto, string cantidad)
        {
            verAbastecimientosPage.SeleccionarConceptoYCantidad(concepto, cantidad);
        }





        [When(@"Se recarga la página")]
        public void WhenSeRecargaLaPagina()
        {
            verAbastecimientosPage.RefrescarPagina();
        }





        // ==========================================
        // FASE 4: EDICIÓN DE ABASTECIMIENTO
        // ==========================================
        [When(@"Se filtra la tabla usando el selector de Placa ""(.*)""")]
        public void WhenSeFiltraLaTablaUsandoElSelectorDePlaca(string placa)
        {
            verAbastecimientosPage.SeleccionarPlacaFiltro(placa);
        }

        [When(@"Se hace clic en el boton BUSCAR de la grilla")]
        public void WhenSeHaceClicEnElBotonBuscarDeLaGrilla()
        {
            verAbastecimientosPage.ClicBotonBuscarGrilla();
        }

        [When(@"Se hace clic en la Lupa del primer registro de la tabla")]
        public void WhenSeHaceClicEnLaLupaDelPrimerRegistroDeLaTabla()
        {
            verAbastecimientosPage.ClicPrimeraLupaGrid();
        }

        [When(@"Se hace clic en el boton Editar abastecimiento")]
        public void WhenSeHaceClicEnElBotonEditarAbastecimiento()
        {
            verAbastecimientosPage.ClicBotonEditarAbastecimiento();
        }

        [Then(@"Se verifica que el resultado de la actualizacion sea ""(.*)""")]
        public void ThenSeVerificaQueElResultadoDeLaActualizacionSea(string resultadoEsperado)
        {
            verAbastecimientosPage.ValidarResultadoActualizacionConLogica(resultadoEsperado);
        }



        [When(@"Se modifica la cantidad por ""(.*)""")]
        public void WhenSeModificaLaCantidadPor(string cantidad)
        {
            verAbastecimientosPage.ModificarCantidad(cantidad);
        }




        // ==========================================
        // FASE 5: ANULACIÓN (DAR DE BAJA)
        // ==========================================
        [When(@"Se hace clic en el boton Anular abastecimiento")]
        public void WhenSeHaceClicEnElBotonAnularAbastecimiento()
        {
            verAbastecimientosPage.ClicBotonAnularAbastecimiento();
        }

        [When(@"Se ingresan las observaciones de baja ""(.*)"" y se guarda")]
        public void WhenSeIngresanLasObservacionesDeBajaYSeGuarda(string observacion)
        {
            verAbastecimientosPage.IngresarObservacionYGuardar(observacion);
        }

        [Then(@"Se verifica que el resultado de la anulacion sea ""(.*)""")]
        public void ThenSeVerificaQueElResultadoDeLaAnulacionSea(string resultadoEsperado)
        {
            verAbastecimientosPage.ValidarResultadoAnulacion(resultadoEsperado);
        }












    }
}