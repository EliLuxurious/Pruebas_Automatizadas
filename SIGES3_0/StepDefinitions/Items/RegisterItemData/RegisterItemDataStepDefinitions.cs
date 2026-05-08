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
            conceptosPage.GuardarCambiosGenerales();
        }

        [When("el usuario oculta el menu")]
        public void WhenElUsuarioOcultaElMenu()
        {
            conceptosPage.CerrarSidebar();
        }

        [When("el usuario aplica los cambios de familia")]
        public void WhenElUsuarioAplicaLosCambiosDeFamilia()
        {
            conceptosPage.AplicarNuevosCambios();
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

        [When("el usuario guarda los cambios de reasignacion")]
        public void WhenElUsuarioGuardaLosCambios()
        {
            conceptosPage.GuardarCambiosGenerales();
        }

        [Then("el usuario desactiva la familia {string}")]
        public void WhenElUsuarioDesactivaLaFamilia(string familia)
        {
            conceptosPage.DesactivarFamiliaDesdeGrilla(familia);
        }

        [When("el usuario confirma la reasignación")]
        public void WhenElUsuarioConfirmaLaReasignacion()
        {
            conceptosPage.ConfirmarReasignacionYEliminar();
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

        [When("el sistema guarda los cambios al editar categoria")]
        public void ThenElSistemaGuardaLosCambiosAlEditarCategoria()
        {
            conceptosPage.GuardarCambiosGenerales();
        }

        [Then("el sistema no guarda los cambios de editar categoria")]
        public void ThenElSistemaNoGuardaLosCambiosDeEditarCategoria()
        {
            conceptosPage.GuardarCambiosGenerales();
        }

        [When("el usuario elimina la categoria {string}")]
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
            conceptosPage.GuardarCambiosGenerales();
        }

        [When("el usuario aplica los cambios al editar presentacion")]
        public void ThenElUsuarioAplicaLosCambiosAlEditarPresentacion()
        {
            conceptosPage.AplicarNuevosCambios();
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

        [Then("el usuario desactiva la presentación {string}")]
        public void ThenElUsuarioDesactivaLaPresentacion(string nombre)
        {
            conceptosPage.DesactivarPresentacionDesdeGrilla(nombre);
        }

        [When("el usuario reasigna y elimina la presentacion")]
        public void ThenElUsuarioReasignaYEliminaLaPresentacion()
        {
            conceptosPage.ConfirmarReasignacionYEliminar();
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

        [When("el usuario guarda los cambios al editar caracteristica comun")]
        public void ThenElUsuarioGuardaLosCambiosAlEditarCaracteristicaComun()
        {
            conceptosPage.GuardarCambiosGenerales();
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

        [When("el usuario guarda los cambios al editar caracteristica propia")]
        public void ThenElUsuarioGuardaLosCambiosAlEditarCaracteristicaPropia()
        {
            conceptosPage.GuardarCambiosGenerales();
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

        [When("el usuario guarda el valor de caracteristica comun")]
        public void ThenSeGuardaElValorDeCaracteristicaComun()
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
            conceptosPage.GuardarCambiosGenerales();
        }

        [When("el usuario acepta actualizar el concepto con el valor de caracteristica comun")]
        public void ThenElUsuarioAceptaActualizarElConceptoConElValorDeCaracteristicaComun()
        {
            conceptosPage.SiValorCaracteristicaComun();
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

        [When("el usuario acepta eliminar el concepto con el valor de caracteristica comun")]
        public void ThenElUsuarioAceptaEliminarElConceptoConElValorDeCaracteristicaComun()
        {
            conceptosPage.SiValorCaracteristicaComun();
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

        //PASOS GENERALES PARA ELIMINAR CONCEPTOS EN FAMILIA, PRESENTACION, VALOR DE CARACTERISTICA
        [When("el usuario elimina todos los conceptos de la tabla")]
        public void WhenElUsuarioEliminaTodosLosConceptosDeLaTabla()
        {
            conceptosPage.EliminarTodosLosConceptos();
        }

        [When("el usuario elimina los siguientes conceptos:")]
        public void WhenElUsuarioEliminaLosSiguientesConceptos(Table table)
        {
            // Convertimos la tabla Gherkin en una Lista para tu método dinámico
            List<string> nombres = new List<string>();
            foreach (var row in table.Rows)
            {
                nombres.Add(row[0]);
            }

            conceptosPage.EliminarConceptosPorNombre(nombres);
        }

        [When(@"el usuario cierra el sidebar")]
        public void WhenElUsuarioCierraElSidebar()
        {
            conceptosPage.CerrarSidebar();
        }


        [Then("el sistema muestra un mensaje de confirmacion")]
        public void WhenElSistemaMuestraUnMensajeDeConfirmacion()
        {
            conceptosPage.SistemaMensajeOk();
        }

        [Then("el sistema muestra un mensaje de error")]
        public void WhenElSistemaMuestraUnMensajeDeError()
        {
            conceptosPage.SistemaMensajeOk();
        }

        //PASOS GENERALES PARA GUARDAR O NO GUARDAR REGISTROS EN FAMILIA, CATEGORIA, PRESENTACION, CARACTERISTICA, ETC.

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




