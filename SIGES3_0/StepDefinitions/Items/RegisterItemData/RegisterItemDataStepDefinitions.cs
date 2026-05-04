using AventStack.ExtentReports.Gherkin.Model;
using Newtonsoft.Json.Bson;
using OpenQA.Selenium;
using SIGES3_0.Pages.Items.RegisterItemData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGES3_0.StepDefinitions.Items.RegisterItemData
{
    [Binding]
    public class RegisterItemDataStepDefinitions
    {
        private IWebDriver driver;
        RegisterItemDataPage conceptosPage;

        public RegisterItemDataStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            conceptosPage = new RegisterItemDataPage(driver);
        }

        [When("el usuario selecciona Registrar Datos de Concepto")]
        public void WhenElUsuarioSeleccionaRegistrarDatosDeConcepto()
        {
            conceptosPage.SeleccionarRegistrarDatosConcepto();
        }

        //-------------------------------------FAMILIA ------------------------------------------------------------

        [When("el usuario selecciona la opción Familia")]
        public void WhenElUsuarioSeleccionaLaOpcionFamilia()
        {
            conceptosPage.SeleccionarOpcionFamilia();
        }

        [When("el usuario selecciona el tipo {string}")]
        public void WhenElUsuarioSeleccionaElTipo(string tipo)
        {
            conceptosPage.SeleccionarTipo(tipo);
        }

        [When("el usuario selecciona el tipo de tratamiento {string}")]
        public void WhenElUsuarioSeleccionaElTipoDeTratamientoDinamico(string tratamientoIGV)
        {
            conceptosPage.SeleccionarTratamientoIGVDinamico(tratamientoIGV);
        }

        [When("el usuario selecciona la opción Detracción")]
        public void WhenElUsuarioSeleccionaLaOpcionDetraccion()
        {
            conceptosPage.SeleccionarDetraccion();
        }

        //PARA TABLA DE DECISIONES
        [When("el usuario establece la detracción en {string}")]
        public void WhenElUsuarioEstableceLaDetraccion(string estadoDetraccion)
        {
            if (estadoDetraccion.ToUpper() == "ACTIVO")
            {
                conceptosPage.SeleccionarDetraccion();
            }
        }


        [When("el usuario selecciona el tipo de detracción {string}")]
        public void WhenElUsuarioSeleccionaElTipoDeDetraccion(string tipoDetraccion)
        {
            if (!string.IsNullOrEmpty(tipoDetraccion))
            {
                conceptosPage.SeleccionarPorcentajeDetraccion(tipoDetraccion);
            }
        }

        [When("el usuario ingresa el código de familia {string}")]
        public void WhenElUsuarioIngresaElCodigoDeFamilia(string codigo)
        {
            conceptosPage.IngresarCodigoFamilia(codigo);
        }

        [When("el usuario ingresa el nombre de familia {string}")]
        public void WhenElUsuarioIngresaElNombreDeFamilia(string nombre)
        {
            conceptosPage.IngresarNombreFamilia(nombre);
        }

        [When("el usuario selecciona la categoria {string}")]
        public void WhenElUsuarioSeleccionaLaCategoria(string categoria)
        {
            if (!string.IsNullOrEmpty(categoria))
            {
                conceptosPage.SeleccionarCategoria(categoria);
            }
        }


        [When(@"el usuario ingresa la caracteristica comun {string} y su estado {string}")]
        public void WhenElUsuarioIngresaLaCaracteristicaComunYSuEstado(string caractComun, string estadoComun)
        {
            if (!string.IsNullOrWhiteSpace(caractComun))
            {
                // 1. PRIMERO nos aseguramos de estar en la pestaña correcta
                conceptosPage.SeleccionarTipoCaracteristica("Comun");

                conceptosPage.MostrarTodasLasCaracteristicas();

                // 2. LUEGO buscamos y marcamos la característica
                conceptosPage.SeleccionarCaracteristica(caractComun, estadoComun);
            }
        }

        [When(@"el usuario ingresa la caracteristica propia {string} y su estado {string}")]
        public void WhenElUsuarioIngresaLaCaracteristicaPropiaYSuEstado(string caractPropia, string estadoPropia)
        {
            if (!string.IsNullOrWhiteSpace(caractPropia))
            {
                // 1. PRIMERO cambiamos a la pestaña de características propias
                conceptosPage.SeleccionarTipoCaracteristica("Propia");

                conceptosPage.MostrarTodasLasCaracteristicas();

                // 2. LUEGO buscamos y marcamos la característica
                conceptosPage.SeleccionarCaracteristica(caractPropia, estadoPropia);
            }
        }


        [When("el usuario va a la opcion Familia")]
        public void WhenElUsuarioVaALaOpcionFamilia()
        {
            conceptosPage.IrAOpcionFamilia();
        }


        [When("el usuario edita la familia {string}")]
        public void WhenElUsuarioEditaLaFamilia(string familia)
        {
            conceptosPage.EditarFamilia(familia);
        }

        [When("el usuario guarda los cambios de familia")]
        public void WhenElUsuarioGuardaLosCambiosDeFamilia()
        {
            conceptosPage.GuardarEdicionFamilia();
        }

        [When("el usuario oculta el menu")]
        public void WhenElUsuarioOcultaElMenu()
        {
            conceptosPage.CerrarSidebar();
        }

        [When("el usuario elimina el concepto a editar familia")]
        public void WhenElUsuarioEliminaElConcepto()
        {
            conceptosPage.EliminarConceptoEditarFamilia();
        }


        [Then("el usuario aplica los cambios de familia")]
        public void WhenElUsuarioAplicaLosCambiosDeFamilia()
        {
            conceptosPage.AplicarCambiosEdicionFamilia();
        }


        // EL USUARIO ELIMINA FAMILIA
        [When("el usuario da de baja la familia {string}")]
        public void WhenElUsuarioDaDeBajaLaFamilia(string familia)
        {
            conceptosPage.EliminarFamilia(familia);
        }


        [When("el usuario reasigna los conceptos a la nueva familia {string}")]
        public void WhenElUsuarioReasignaLosConceptosALaNuevaFamilia(string nuevaFamilia)
        {
            conceptosPage.SeleccionarNuevaFamiliaParaReasignar(nuevaFamilia);
        }

        [When("el usuario selecciona la característica {string}")] 
        public void WhenElUsuarioSeleccionaLaCaracteristica(string caracteristica)
        {
            conceptosPage.SeleccionarCaracteristica(caracteristica);
        }

        [When("el usuario elimina el concepto a dar de baja")]
        public void WhenElUsuarioEliminaElConceptoADarDeBaja()
        {
            conceptosPage.EliminarConceptoParaBajaFamilia();
        }

        [When("el usuario guarda los cambios de reasignacion")]
        public void WhenElUsuarioGuardaLosCambios()
        {
            conceptosPage.GuardarReasignacionFamilia();
        }

        [When("el usuario desactiva la familia {string}")]
        public void WhenElUsuarioDesactivaLaFamilia(string familia)
        {
            conceptosPage.DesactivarFamiliaDesdeGrilla(familia);
        }

        [Then("el sistema muestra un mensaje de confirmación de baja de familia")]
        public void ThenElSistemaMuestraUnMensajeDeConfirmacionDeBajaDeFamilia()
        {
            conceptosPage.confirmarBajaFamilia();
        }

        [Then("el sistema muestra un mensaje de error al dar de baja")]
        public void ThenElSistemaMuestraUnMensajeDeError()
        {
            conceptosPage.ConfirmarReasignacionFamilia();
        }


        [Then("el usuario confirma la reasignación")]
        public void WhenElUsuarioConfirmaLaReasignacion()
        {
            conceptosPage.ConfirmarReasignacionFamilia();
        }

        //---------------------------------------------------------------------------------------------------------


        //-------------------------------------CATEGORIA ------------------------------------------------------------

        [When("el usuario selecciona la opción Categoría")]
        public void WhenElUsuarioSeleccionaLaOpcionCategoria()
        {
            conceptosPage.SeleccionarCategoria();
        }

        [When("el usuario ingresa el nombre de categoría {string}")]
        public void WhenElUsuarioIngresaElNombreDeCategoria(string nombre)
        {
            conceptosPage.IngresarNombreCategoria(nombre);
        }

        [When("el usuario ingresa la descripcion de categoría {string}")]
        public void WhenElUsuarioIngresaLaDescripcionDeCategoria(string descripcion)
        {
            conceptosPage.IngresarDescripcionCategoria(descripcion);
        }

        [When("el usuario selecciona la categoria padre {string}")]
        public void WhenElUsuarioSeleccionaLaCategoriaPadre(string categoriaPadre)
        {
            conceptosPage.SeleccionarCategoriaPadre(categoriaPadre);
        }


        //ESTO ES PARA EDITAR CATEGORIA
        [When("el usuario va a la opcion categoria")]
        public void WhenElUsuarioVaALaOpcionCategoria()
        {
            conceptosPage.IraCategoria();
        }

        [When("el usuario edita la categoria {string}")]
        public void WhenElUsuarioEditaLaCategoria(string categoria)
        {
            conceptosPage.EditarCategoria(categoria);
        }

        [Then("el sistema guarda los cambios al editar categoria")]
        public void ThenElSistemaGuardaLosCambiosAlEditarCategoria()
        {
            conceptosPage.GuardarCambiosCategoria();
        }

        [Then("el sistema no guarda los cambios de editar categoria")]
        public void ThenElSistemaNoGuardaLosCambiosDeEditarCategoria()
        {
            conceptosPage.GuardarCambiosCategoria();
        }

        [Then("el usuario elimina la categoria {string}")]
        public void ThenElUsuarioEliminaLaCategoria(string categoria)
        {
            conceptosPage.EliminarCategoria(categoria);
        }


        //---------------------------------------------------------------------------------------------------------


        //-------------------------------------PRESENTACION------------------------------------------------------------

        [When("el usuario selecciona la opcion Presentación")]
        public void WhenElUsuarioSeleccionaLaOpcionPresentacion()
        {
            conceptosPage.SeleccionarPresentacion();
        }

        [When("el usuario ingresa el codigo de presentación {string}")]
        public void WhenElUsuarioIngresaElCodigoDePresentacion(string codigo)
        {
            conceptosPage.IngresarCodigoPresentacion(codigo);
        }

        [When("el usuario ingresa el nombre de presentación {string}")]
        public void WhenElUsuarioIngresaElNombreDePresentacion(string nombre)
        {
            conceptosPage.IngresarNombrePresentacion(nombre);
        }

        [When("el usuario ingresa la descripcion de presentación {string}")]
        public void WhenElUsuarioIngresaLaDescripcionDePresentacion(string descripcion)
        {
            conceptosPage.IngresarDescripcionPresentacion(descripcion);
        }

        //PARA EDITAR PRESENTACION
        [When("el usuario va a la opcion Presentacion")]
        public void WhenElUsuarioVaALaOpcionPresentacion()
        {
            conceptosPage.IraPresentacion();
        }

        [When("el usuario edita la presentacion {string}")]
        public void WhenElUsuarioEditaLaPresentacion(string presentacion)
        {
            conceptosPage.EditarPresentacion(presentacion);
        }

        [When("el usuario guarda los cambios al editar presentacion")]
        public void WhenElUsuarioGuardaLosCambiosAlEditarPresentacion()
        {
            conceptosPage.GuardarCambiosEditarPresentacion();
        }

        [When("el usuario elimina el concepto al editar presentacion")]
        public void WhenElUsuarioEliminaElConceptoAlEditarPresentacion()
        {
            conceptosPage.eliminarConceptoEditarPresentacion();
        }

        [Then("el usuario aplica los cambios al editar presentacion")]
        public void ThenElUsuarioAplicaLosCambiosAlEditarPresentacion()
        {
            conceptosPage.aplicarcambiosEditarPresentacion();
        }

        //ELIMINAR PRESENTACION
        [When("el usuario elimina la presentacion {string}")]
        public void WhenElUsuarioEliminaLaPresentacion(string presentacion)
        {
            conceptosPage.EliminarPresentacion(presentacion);
        }

        [When("el usuario selecciona la presentacion a reasignar {string}")]
        public void WhenElUsuarioSeleccionaLaPresentacionAReasignar(string nuevapresentacion)
        {
            conceptosPage.SeleccionarNuevaPresentacionParaReasignar(nuevapresentacion);
        }

        [When("el usuario elimina el concepto al eliminar la presentacion")]
        public void WhenElUsuarioEliminaElConceptoAlEliminarLaPresentacion()
        {
            conceptosPage.eliminarConceptoEliminarPresentacion();
        }

        [Then("el usuario desactiva la presentación {string}")]
        public void ThenElUsuarioDesactivaLaPresentacion(string nombre)
        {
            conceptosPage.DesactivarPresentacionDesdeGrilla(nombre);
        }

        [Then("el usuario reasigna y elimina la presentacion")]
        public void ThenElUsuarioReasignaYEliminaLaPresentacion()
        {
            conceptosPage.reasignarEliminarPresentacion();
        }


        //-------------------------------------------------------------------------------------------------------------


        //-------------------------------------CARACTERÍSTICAS------------------------------------------------------------

        [When("el usuario selecciona la opción Características")]
        public void WhenElUsuarioSeleccionaLaOpcionCaracteristicas()
        {
            conceptosPage.SeleccionarOpcionCaracteristica();
        }

        [When("el usuario selecciona el tipo de Caracteristica {string}")]
        public void WhenElUsuarioSeleccionaElTipoDeCaracteristica(string tipo)
        {
            conceptosPage.SeleccionarTipoCaracteristica(tipo);
        }

        [When("el usuario ingresa el nombre de Caracteristica Comun {string}")]
        public void WhenElUsuarioIngresaElNombreDeCaracteristicaComun(string nombre)
        {
            if (!string.IsNullOrEmpty(nombre))
            {
                conceptosPage.IngresarNombreCaracteristicaComun(nombre);
            }
        }

        [When("el usuario edita la característica común {string}")]
        public void WhenElUsuarioEditaLaCaracteristicaComun(string nombre)
        {
            conceptosPage.EditarCaracteristicaComun(nombre);
        }

        [Then("el usuario guarda los cambios al editar caracteristica comun")]
        public void ThenElUsuarioGuardaLosCambiosAlEditarCaracteristicaComun()
        {
            conceptosPage.guardarcambiosCaracteristicaComun();
        }

        [When("el usuario ingresa el codigo de caracteristica propia {string}")]
        public void WhenElUsuarioIngresaElCodigoDeCaracteristicaPropia(string codigo)
        {
            conceptosPage.IngresarCodigoCaracteristicaPropia(codigo);
        }

        [When("el usuario ingresa el nombre de caracteristica propia {string}")]
        public void WhenElUsuarioIngresaElNombreDeCaracteristicaPropia(string nombre)
        {
            if (!string.IsNullOrEmpty(nombre))
            {
                conceptosPage.IngresarNombreCaracteristicaPropia(nombre);
            }
        }

        [When("el usuario selecciona el tipo de dato {string}")]
        public void WhenElUsuarioSeleccionaElTipoDeDato(string tipoDato)
        {
            conceptosPage.SeleccionarTipoDatoCaracteristicaPropia(tipoDato);
        }

        [When("el usuario edita la característica propia {string}")]
        public void WhenElUsuarioEditaLaCaracteristicaPropia(string nombre)
        {
            conceptosPage.EditarCaracteristicaPropia(nombre);
        }

        [Then("el usuario guarda los cambios al editar caracteristica propia")]
        public void ThenElUsuarioGuardaLosCambiosAlEditarCaracteristicaPropia()
        {
            conceptosPage.guardarcambiosCaracteristicaPropia();
        }

        //-------------------------------------------------------------------------------------------------------------


        //-------------------------------------VALOR CARACTERISTICA------------------------------------------------------------



        [When("el usuario selecciona la opcion Valor de Caracteristica")]
        public void WhenElUsuarioSeleccionaLaOpcionValorDeCaracteristica()
        {
            conceptosPage.SeleccionarOpcionValorCaracteristica();
        }

        [When("el usuario selecciona la caracteristica comun {string}")]
        public void WhenElUsuarioSeleccionaLaCaracteristicaComun(string caracteristicaComun)
        {
            conceptosPage.SeleccionarCaracteristicaComun(caracteristicaComun);
        }

        [When("el usuario ingresa el valor de caracteristica comun {string}")]
        public void WhenElUsuarioIngresaElValorDeCaracteristicaComun(string valor)
        {
            conceptosPage.IngresarValorCaracteristicaComun(valor);
        }

        [Then("se guarda el valor de caracteristica comun")]
        public void ThenSeGuardaElValorDeCaracteristicaComun()
        {
            conceptosPage.guardarValorCaracteristicaComun();
        }

        [Then("no se guarda el valor de caracteristica comun")]
        public void ThenNoSeGuardaElValorDeCaracteristicaComun()
        {
            conceptosPage.guardarValorCaracteristicaComun();
        }

        [When("el usuario edita el valor de característica común {string}")]
        public void WhenElUsuarioEditaValorCaracteristicaComun(string valor)
        {
            conceptosPage.EditarValorCaracteristicaComun(valor);
        }

        [When("el usuario guarda los cambios al editar valor de caracteristica comun")]
        public void WhenElUsuarioGuardaLosCambiosAlEditarValorDeCaracteristicaComun()
        {
            conceptosPage.guardarcambiosEditarValorCaracteristicaComun();
        }

        [When("el usuario elimina el concepto registrado al editar valor de caracteristica comun")]
        public void WhenElUsuarioEliminaElConceptoRegistradoAlEditarValorDeCaracteristicaComun()
        {
            conceptosPage.eliminarconceptoEditarValorCaracteristicaComun();
        }

        [Then("el usuario acepta actualizar el concepto con el valor de caracteristica comun")]
        public void ThenElUsuarioAceptaActualizarElConceptoConElValorDeCaracteristicaComun()
        {
            conceptosPage.actualizarValorCaracteristicaComun();
        }

        [When("el usuario elimina el valor de característica común {string}")]
        public void WhenElUsuarioEliminaValorCaracteristicaComun(string valor)
        {
            conceptosPage.EliminarValorCaracteristicaComun(valor);
        }

        [When("el usuario selecciona el nuevo valor de característica común {string}")]
        public void WhenElUsuarioSeleccionaNuevoValorCaracteristicaComun(string valor)
        {
            conceptosPage.SeleccionarNuevoValorCaracteristicaComun(valor);
        }

        [When("el usuario elimina el concepto registrado al eliminar valor de caracteristica comun")]
        public void WhenElUsuarioEliminaElConceotoRegistradoAlEliminarValorDeCaracteristicaComun()
        {
            conceptosPage.eliminarconceptoEliminarValorCaracteristicaComun();
        }

        //-------------------------------------------------------------------------------------------------------------


        //------------------------------------ASIGNAR VALOR DE CARACTERISTICA---------------------------------------------------

        [When("el usuario selecciona la opcion Asignar Valor de Caracteristica")]
        public void WhenElUsuarioSeleccionaLaOpcionAsignarValorDeCaracteristica()
        {
            conceptosPage.SeleccionarOpcionAsignarValorCaracteristicaFamilia();
        }

        [When("el usuario selecciona la Familia a asignar {string}")]
        public void WhenElUsuarioSeleccionaLaFamilia(string familia)
        {
            conceptosPage.SeleccionarFamiliaParaAsignarValor(familia);
        }

        [When("el usuario ingresa el valor a asignar {string}")]
        public void WhenElUsuarioIngresaElValorAAsignar(string valor)
        {
            conceptosPage.IngresarValorParaAsignarFamilia(valor);
        }


        [When("el usuario arrastra el valor {string}")]
        public void WhenElUsuarioArrastraElValor(string valor)
        {
            conceptosPage.ArrastrarValorAFamilia(valor);
        }


        [Then("se guarda la asignacion")]
        public void ThenSeGuardaLaAsignacion()
        {
            conceptosPage.guardarAsignacion();
        }


        //-----------------------------------------------------------------------------------------------------------------------------------


        [Then("se guarda el registro")]
        public void ThenSeGuardaElRegistro()
        {
            conceptosPage.GuardarRegistro();
        }

        [Then("no se guarda el registro")]
        public void ThenNoSeGuardaElRegistro()
        {
            conceptosPage.NoguardarRegistro();
        }

    }
}




