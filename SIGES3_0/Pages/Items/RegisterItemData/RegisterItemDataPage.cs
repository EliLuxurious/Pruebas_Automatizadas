using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V137.DOM;
using OpenQA.Selenium.Interactions;
using SIGES3_0.Pages.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SIGES3_0.Pages.Items.RegisterItemData
{
    public class RegisterItemDataPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public RegisterItemDataPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // MENU CONCEPTOS
        private By conceptosMenu = By.XPath("//span[text()='Conceptos']/following::input[1]");

        // SUBMENU REGISTRAR DATOS DE CONCEPTO
        private By registrarDatoConcepto = By.XPath("//span[normalize-space()='Registrar Datos de Concepto']");

        //------------------------------------------------FAMILIA------------------------------------------------------------

        // OPCIÓN FAMILIA
        private By opcionFamilia = By.XPath("//button[normalize-space()='Familia']//i[@class='bi bi-house-door-fill']");

        // TRATAMIENTO IGV - EXNOERACIÓN DE IGV
        private By tratamientoIgvExoneracion = By.XPath("//input[@id='exoneracion']");

        // TRATAMIENTO IGV - IGV RESTAURANTES
        private By tratamientoIgvRestaurantes = By.XPath("//input[@id='igvRestaurantes']");

        // DETRACCION
        private By detraccion = By.XPath("//input[@id='detraccion']");

        // PORCENTAJE DE DETRACCION
        private By dropdownDetraccion = By.XPath("//span[normalize-space()='Seleccione la detracción']/ancestor::div[contains(@class,'select-trigger')]");

        // CODIGO FAMILIA
        private By codigoFamilia = By.XPath("//input[@placeholder='Código']");

        // NOMBRE FAMILIA
        private By nombreFamilia = By.XPath("//input[@placeholder='Nombre']");

        // DROPDOWN CATEGORIA
        private By dropdownCategoria = By.XPath("//div[contains(@class,'select-trigger')][.//span[normalize-space()='Seleccione las categorías']]");

        //-----------------------------------------------------------------------------------------------------------------------------------

        //------------------------------------------------CATEGORÍA----------------------------------------------------------------------------

        // OPCION CATEGORIA
        private By opcionCategoria = By.XPath("//button[normalize-space()='Categoría']");

        // NOMBRE CATEGORIA
        private By nombreCategoria = By.XPath("//input[@placeholder='Nombre']");

        // DESCRIPCION CATEGORIA
        private By descripcionCategoria = By.XPath("//input[@placeholder='Descripción']");

        // DROPDOWN CATEGORIA PADRE
        private By dropdownCategoriaPadre = By.XPath("//div[contains(@class,'select-trigger')]//span[normalize-space()='Seleccione las categorías']");

        //-----------------------------------------------------------------------------------------------------------------------------------

        //------------------------------------------------PRESENTACIÓN----------------------------------------------------------------------------

        // OPCION PRESENTACIÓN
        private By opcionPresentacion = By.XPath("//button[normalize-space()='Presentación']");

        // CODIGO PRESENTACION
        private By codigoPresentacion = By.XPath("//input[@id='code']");

        // NOMBRE PRESENTACION
        private By nombrePresentacion = By.XPath("//input[@id='name']");

        // DESCRIPCION PRESENTACION
        private By descripcionPresentacion = By.XPath("//input[@id='description']");

        //-----------------------------------------------------------------------------------------------------------------------------------

        //------------------------------------------------CARACTERISTICA----------------------------------------------------------------------------

        // OPCION CARACTERÍSTICAS
        private By opcionCaracteristicas = By.XPath("//button[normalize-space()='Caracteristicas']");

        //------------------------------------------------CARACTERISTICA COMUN----------------------------------------------------------------------------

        // NOMBRE CARACTERISTICA COMUN
        private By nombreCaracteristicaComun = By.XPath("//input[@placeholder='Nombre']");

        //---------------------------------------------------------------------------------------------------------------------------------------------

        //------------------------------------------------CARACTERISTICA PROPIA----------------------------------------------------------------------------

        // CODIGO CARACTERISTICA PROPIA
        private By codigoCaracteristicaPropia = By.XPath("//input[@placeholder='Código']");

        // NOMBRE CARACTERISTICA PROPIA
        private By nombreCaracteristicaPropia = By.XPath("//input[@placeholder='Nombre']");

        // TIPO DATO CARACTERISTICA PROPIA
        private By tipoDatoCaracteristicaPropia = By.Id("tipo-dato");

        //---------------------------------------------------------------------------------------------------------------------------------------------


        //------------------------------------------------VALOR DE CARACTERISTICA----------------------------------------------------------------------------

        // OPCION VALOR DE CARACTERÍSTICA
        private By opcionValorCaracteristica = By.XPath("//button[normalize-space()='Valor de Caracteristica']");

        // SELECCIÓN DE CARACTERISTICA COMUN
        private By seleccionCaracteristicaComun = By.XPath("//select[@id='tipo-dato']");


        //---------------------------------------------------------------------------------------------------------------------------------------------


        //------------------------------------------------ASIGNAR VALOR DE CARACTERISTICA POR FAMILIA----------------------------------------------------------------------------


        // OPCION ASIGNAR VALOR DE CARACTERÍSTICA POR FAMILIA
        private By opcionAsignarValorCaracteristicaFamilia = By.XPath("//button[normalize-space()='Asignar Valor de Caracteritica por Familia']");

        // SELECCION FAMILIA
        private By dropdownFamilia = By.Id("familia");

        // BOTÓN PARA AGREGAR VALOR A ASIGNAR
        private By botonAgregarValorParaAsignar = By.XPath("//button[normalize-space()='Agregar valor']");


        //---------------------------------------------------------------------------------------------------------------------------------------------

        // BOTON GUARDAR    
        private By botonGuardar = By.XPath("//button[normalize-space()='Guardar']");


        // MÉTODOS

        public void AbrirModuloConceptos()
        {
            utilities.ClickButton(conceptosMenu);
        }

        public void SeleccionarRegistrarDatosConcepto()
        {
            utilities.ClickButton(registrarDatoConcepto);
        }


        //---------------------------------------------------FAMILIA----------------------------------------------------------

        public void SeleccionarOpcionFamilia()
        {
            utilities.ClickButton(opcionFamilia);
            By dropdownFamilia = By.XPath("//button[@aria-controls='collapse-registro-familia']");
            utilities.ClickButton(dropdownFamilia);
        }

        public void SeleccionarTipo(string tipo)
        {
            By tipoRadio = By.XPath($"//input[@id='tipo{tipo}']");
            utilities.ClickButton(tipoRadio);
        }

        public void SeleccionarTratamientoIGVDinamico(string tratamientoIGV)
        {
            // Evaluamos el string que nos pasan en el feature
            if (tratamientoIGV.Contains("Exoneración") || tratamientoIGV.Contains("Exoneracion"))
            {
                utilities.ClickButton(tratamientoIgvExoneracion);
            }
            else if (tratamientoIGV.Contains("Restaurantes"))
            {
                utilities.ClickButton(tratamientoIgvRestaurantes);
            }
        }


        public void SeleccionarDetraccion()
        {
            utilities.ClickButton(detraccion);
        }

        public void SeleccionarPorcentajeDetraccion(string porcentaje)
        {
            utilities.ClickButton(dropdownDetraccion);
            By buscadorporcentaje = By.XPath("//input[@placeholder='Buscar...']");
            utilities.EnterText(buscadorporcentaje, porcentaje);
            By opcionporcentaje = By.XPath($"//span[contains(@class,'option-label') and text()='{porcentaje}']");
            utilities.ClickButton(opcionporcentaje);
        }

        public void IngresarCodigoFamilia(string codigo)
        {
            utilities.EnterText(codigoFamilia, codigo);
        }

        public void IngresarNombreFamilia(string nombre)
        {
            utilities.EnterText(nombreFamilia, nombre);
        }

        public void SeleccionarCategoria(string categoria)
        {
            utilities.ClickButton(dropdownCategoria);
            By buscadorCategoria = By.XPath("//input[@placeholder='Buscar...'][last()]");
            utilities.EnterText(buscadorCategoria, categoria);
            By opcionCategoria = By.XPath($"//span[contains(@class,'option-label') and text()='{categoria}']");
            utilities.ClickButton(opcionCategoria);
            By cerrarCategoria = By.XPath("//i[@class='bi bi-chevron-down']");
            utilities.ClickButton(cerrarCategoria);
        }

        public void SeleccionarTipoCaracteristica(string tipo)
        {
            By opcion = By.XPath($"//button[normalize-space()='Caracteristica {tipo}']");
            utilities.ClickButton(opcion);
        }

        public void MostrarTodasLasCaracteristicas()
        {
            try
            {
                By desplegablePaginacion = By.XPath("//app-view-common-attributer-for-business-item-family//select[@class='form-select custom-input']");
                By opcionCien = By.XPath("//app-view-common-attributer-for-business-item-family//select[@class='form-select custom-input']/option[@value='100']");

                utilities.ClickButton(desplegablePaginacion);
                Thread.Sleep(300);
                utilities.ClickButton(opcionCien);
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se encontró o no fue necesario ajustar el paginador de características: " + ex.Message);
            }
        }

        public void MostrarTodasLasFamilias()
        {
            try
            {
                By desplegablePaginacion = By.XPath("//div[@class='container-fluid mt-3 siges-container']//select[@class='form-select custom-input']");
                By opcionCien = By.XPath("//div[@class='container-fluid mt-3 siges-container']//select[@class='form-select custom-input']/option[@value='100']");

                utilities.ClickButton(desplegablePaginacion);
                Thread.Sleep(300);
                utilities.ClickButton(opcionCien);
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se encontró o no fue necesario ajustar el paginador de familias: " + ex.Message);
            }
        }

        public void EditarFamilia(string nombreFamilia)
        {
            MostrarTodasLasFamilias();

            By botonEditar = By.XPath($"//td[normalize-space()='{nombreFamilia}']/parent::tr//app-button-actions//button[1]");

            utilities.ClickButton(botonEditar);
            Thread.Sleep(500);

            By dropdownFamilia = By.XPath("//button[@aria-controls='collapse-edición-familia']");
            utilities.ClickButton(dropdownFamilia);
        }


        public void SeleccionarCaracteristica(string nombreCaracteristica, string estadoObligatorio)
        {
            // 1. Buscamos y hacemos clic en el checkbox de SELECCIÓN de la característica
            By checkboxSeleccion = By.XPath($"//td[normalize-space()='{nombreCaracteristica}']/parent::tr/td[1]/input");
            utilities.ClickButton(checkboxSeleccion);

            // 2. Evaluamos si debe ser obligatorio. 
            // Como ya hicimos clic arriba, el checkbox de obligatorio ya debe estar habilitado.
            // Solo hacemos clic si queremos que sea "ACTIVO" o "SI" (por defecto asumimos que está desmarcado)
            if (!string.IsNullOrWhiteSpace(estadoObligatorio) &&
               (estadoObligatorio.ToUpper() == "ACTIVO" || estadoObligatorio.ToUpper() == "SI"))
            {
                By checkboxObligatorio = By.XPath($"//td[normalize-space()='{nombreCaracteristica}']/parent::tr/td[3]/input");
                utilities.ClickButton(checkboxObligatorio);
            }
        }


        //ESTO ES SOLO PARA EDITAR FAMILIA
        public void IrAOpcionFamilia()
        {
            utilities.ClickButton(opcionFamilia);
        }

        //GUARDA CAMBIOS AL EDITAR FAMILIA
        public void GuardarEdicionFamilia()
        {
            By botonGuardar = By.XPath("//button[normalize-space()='Guardar Cambios']");
            utilities.ClickButton(botonGuardar);
        }

        //OCULTA SIDEBAR
        public void CerrarSidebar()
        {
            By botonCerrarSidebar = By.XPath("//i[@class='bi bi-list']");
            utilities.ClickButton(botonCerrarSidebar);
        }

        //ELIMINAR CONCEPTO AL EDITAR FAMILIA
        public void EliminarConceptoEditarFamilia()
        {
            By checkEliminarConcepto = By.XPath("//input[@class='ng-untouched ng-pristine ng-valid ng-star-inserted']");
            utilities.ClickButton(checkEliminarConcepto);
        }


        //APLICAR CAMBIOS DE EDICIÓN DE FAMILIA
        public void AplicarCambiosEdicionFamilia()
        {
            By botonAplicarCambios = By.XPath("//button[normalize-space()='Crear nueva y aplicar cambios (1)']");
            utilities.ClickButton(botonAplicarCambios);
            By botonOk = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonOk);
        }


        public void EliminarFamilia(string nombreFamilia)
        {
            MostrarTodasLasFamilias();

            By botonEliminar = By.XPath($"//td[normalize-space()='{nombreFamilia}']/parent::tr//app-button-actions//button[2]");

            utilities.ClickButton(botonEliminar);
            Thread.Sleep(500);

            By botonConfirmar = By.XPath("//button[normalize-space()='Sí, eliminar']");
            utilities.ClickButton(botonConfirmar);
        }

        public void SeleccionarNuevaFamiliaParaReasignar(string nombreNuevaFamilia)
        {
            // 1. Damos 1 segundo para esperar a que termine la animación de apertura del modal
            Thread.Sleep(1000);

            // 2. Ubicamos el combobox asegurándonos que esté DENTRO de la ventana activa ('modal-content')
            By dropdownReasignar = By.XPath("//div[contains(@class, 'modal-content')]//select[contains(@class,'form-select')]");
            utilities.ClickButton(dropdownReasignar);

            // Una pausa pequeñísima de medio segundo para que bajen las opciones del select
            Thread.Sleep(500);

            // 3. Construimos el XPath dinámico de la opción, también forzando que sea la del modal
            By opcionNuevaFamilia = By.XPath($"//div[contains(@class, 'modal-content')]//select[contains(@class,'form-select')]//option[normalize-space()='{nombreNuevaFamilia}']");

            // 4. Hacemos clic en la familia 
            utilities.ClickButton(opcionNuevaFamilia);
        }


        // Método para asignar la característica a la familia
        public void SeleccionarCaracteristica(string nombreCaracteristica)
        {
            // 1. Damos un respiro para que cargue la animación del SEGUNDO modal
            Thread.Sleep(1000);

            // 2. XPath invencible: Buscamos un 'modal-content' que sí o sí tenga adentro el título con ID 'attributeValuesModalLabel'.
            // Así evitamos que Selenium toque el modal de la Familia que se quedó de fondo.
            string xpathModalCaracteristica = "//div[contains(@class, 'modal-content') and .//h5[@id='attributeValuesModalLabel']]";

            // Combinamos el XPath del modal exacto con el select de la característica
            By dropdownCaracteristica = By.XPath($"{xpathModalCaracteristica}//select[contains(@class,'form-select')]");

            // Hacemos clic para abrir las opciones (los colores como AZUL, ROJO, etc)
            utilities.ClickButton(dropdownCaracteristica);

            // Pequeñísima pausa para que bajen las opciones
            Thread.Sleep(500);

            // 3. Ubicamos y hacemos clic directo a la opción deseada
            By opcionCaracteristica = By.XPath($"{xpathModalCaracteristica}//select[contains(@class,'form-select')]//option[normalize-space()='{nombreCaracteristica}']");
            utilities.ClickButton(opcionCaracteristica);
        }

        public void GuardarReasignacionFamilia()
        {
            By botonGuardarReasignacion = By.XPath("//button[normalize-space()='Guardar cambios']");
            utilities.ClickButton(botonGuardarReasignacion);
        }


        //CONFIRMAR REASIGNACIÓN DE CONCEPTOS A OTRA FAMILIA
        public void ConfirmarReasignacionFamilia()
        {
            By botonConfirmarReasignacion = By.XPath("//button[normalize-space()='Reasignar y eliminar (1)']");
            utilities.ClickButton(botonConfirmarReasignacion);
        }


        //ELIMINAR CONCEPTO PARA LA FUNCIONALIDAD DAR DE BAJA A FAMILIA
        public void EliminarConceptoParaBajaFamilia()
        {
            By checkEliminarConcepto = By.XPath("//input[@class='ng-untouched ng-pristine ng-valid ng-star-inserted']");
            utilities.ClickButton(checkEliminarConcepto);
        }

        public void DesactivarFamiliaDesdeGrilla(string nombreFamilia)
        {
            MostrarTodasLasFamilias();

            By switchFamilia = By.XPath($"//tbody/tr[td[normalize-space()='{nombreFamilia}']]//input[@type='checkbox']");

            try
            {
                var elemento = driver.FindElement(switchFamilia);

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", elemento);
                Thread.Sleep(500);

                utilities.ClickButton(switchFamilia);
            }
            catch
            {
                // fallback si el input no es clickeable
                By switchAlternativo = By.XPath($"//tbody/tr[td[normalize-space()='{nombreFamilia}']]//label");

                utilities.ClickButton(switchAlternativo);
            }
        }


        public void confirmarBajaFamilia()
        {
            By botonConfirmarBaja = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonConfirmarBaja);
        }


        //-----------------------------------------------------------------------------------------------------------------------


        //---------------------------------------------------CATEGORÍA----------------------------------------------------------

        public void SeleccionarCategoria()
        {
            utilities.ClickButton(opcionCategoria);
            By dropdownCategoria2 = By.XPath("//button[@aria-controls='collapse-registro-categoria']");
            utilities.ClickButton(dropdownCategoria2);
        }

        public void IngresarNombreCategoria(string nombre)
        {
            utilities.EnterText(nombreCategoria, nombre);
        }

        public void IngresarDescripcionCategoria(string descripcion)
        {
            utilities.ClearAndEnterText(descripcionCategoria, descripcion);
        }

        public void SeleccionarCategoriaPadre(string categoriaPadre)
        {
            utilities.ClickButton(dropdownCategoriaPadre);
            By buscadorCategoriaPadre = By.XPath("//input[@placeholder='Buscar...'][last()]");
            utilities.EnterText(buscadorCategoriaPadre, categoriaPadre);
            By opcionCategoriaPadre = By.XPath($"//span[contains(@class,'option-label') and text()='{categoriaPadre}']");
            utilities.ClickButton(opcionCategoriaPadre);
        }


        //ESTO ES SOLO PARA EDITAR CATEGORIA
        public void IraCategoria()
        {
            utilities.ClickButton(opcionCategoria);
        }

        public void MostrarTodasLasCategorias()
        {
            try
            {
                By desplegablePaginacion = By.XPath("(//select[contains(@class,'form-select')])[1]");
                By opcionCien = By.XPath("(//select[contains(@class,'form-select')])[1]/option[@value='100']");

                utilities.ClickButton(desplegablePaginacion);
                Thread.Sleep(300);
                utilities.ClickButton(opcionCien);
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se encontró o no fue necesario ajustar el paginador de categorías: " + ex.Message);
            }
        }

        public void EditarCategoria(string nombreCategoria)
        {
            try
            {
                // Mostrar más registros
                MostrarTodasLasCategorias();

                // Botón editar dinámico por nombre de categoría
                By botonEditar = By.XPath($"//td[normalize-space()='{nombreCategoria}']/parent::tr//app-button-actions//button[1]");

                var elementos = driver.FindElements(botonEditar);

                if (elementos.Count > 0)
                {
                    // Click en editar
                    utilities.ClickButton(botonEditar);

                    Thread.Sleep(500); // pequeña espera para que cargue el panel

                    // Abrir dropdown de edición de categoría
                    By dropdownCategoria = By.XPath("//button[@aria-controls='collapse-edición-categoría' and @aria-expanded='false']");
                    utilities.ClickButton(dropdownCategoria);
                }
                else
                {
                    throw new Exception($"No se encontró la categoría '{nombreCategoria}' en la tabla.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar editar la categoría: " + ex.Message);
                throw;
            }
        }


        public void GuardarCambiosCategoria()
        {
            By botonGuardarCambiosCategoria = By.XPath("//button[normalize-space()='Guardar Cambios']");
            utilities.ClickButton(botonGuardarCambiosCategoria);
            By botonOK = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonOK);
        }


        public void EliminarCategoria(string nombreCategoria)
        {
            try
            {
                // 1. Mostrar registros
                MostrarTodasLasCategorias();

                // 2. Botón eliminar dinámico
                By botonEliminar = By.XPath($"//td[normalize-space()='{nombreCategoria}']/parent::tr//app-button-actions//button[2]");

                var elementos = driver.FindElements(botonEliminar);

                if (elementos.Count > 0)
                {
                    // Click en eliminar
                    utilities.ClickButton(botonEliminar);

                    Thread.Sleep(500); // esperar que aparezca el modal

                    // 3. Confirmar eliminación
                    By botonConfirmar = By.XPath("//button[normalize-space()='Sí, eliminar']");
                    utilities.ClickButton(botonConfirmar);
                }
                else
                {
                    throw new Exception($"No se encontró la categoría '{nombreCategoria}' para eliminar.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar la categoría: " + ex.Message);
                throw;
            }
        }





        //-----------------------------------------------------------------------------------------------------------------------


        //---------------------------------------------------PRESENTACIÓN----------------------------------------------------------

        public void SeleccionarPresentacion()
        {
            utilities.ClickButton(opcionPresentacion);
            By dropdownPresentacion = By.XPath("//button[@aria-controls='collapse-registro-presentacion']");
            utilities.ClickButton(dropdownPresentacion);
        }

        public void IngresarCodigoPresentacion(string codigo)
        {
            utilities.EnterText(codigoPresentacion, codigo);
        }

        public void IngresarNombrePresentacion(string nombre)
        {
            utilities.EnterText(nombrePresentacion, nombre);
        }

        public void IngresarDescripcionPresentacion(string descripcion)
        {
            utilities.EnterText(descripcionPresentacion, descripcion);
        }

        //ESTO ES SOLO PARA EDITAR PRESENTACION
        public void IraPresentacion()
        {
            utilities.ClickButton(opcionPresentacion);
        }

        //METODO PARA MOSTRAR TODAS LAS PRESENTACIONES
        public void MostrarTodasLasPresentaciones()
        {
            try
            {
                By desplegable = By.XPath("(//select[contains(@class,'form-select')])[1]");
                By opcionCien = By.XPath("(//select[contains(@class,'form-select')])[1]/option[@value='100']");

                utilities.ClickButton(desplegable);
                Thread.Sleep(300);
                utilities.ClickButton(opcionCien);
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se pudo ajustar el paginador de presentaciones: " + ex.Message);
            }
        }

        //MÉTODO PARA EDITAR PRESENTACIÓN
        public void EditarPresentacion(string nombrePresentacion)
        {
            try
            {
                // 1. Mostrar registros
                MostrarTodasLasPresentaciones();

                // 2. Botón editar dinámico
                By botonEditar = By.XPath($"//td[normalize-space()='{nombrePresentacion}']/parent::tr//app-button-actions//button[1]");

                var elementos = driver.FindElements(botonEditar);

                if (elementos.Count > 0)
                {
                    // Click en editar
                    utilities.ClickButton(botonEditar);

                    Thread.Sleep(500);

                    // 3. Abrir dropdown de edición
                    By dropdown = By.XPath("//button[@aria-controls='collapse-edicion-presentacion' and @aria-expanded='false']");
                    utilities.ClickButton(dropdown);
                }
                else
                {
                    throw new Exception($"No se encontró la presentación '{nombrePresentacion}'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al editar la presentación: " + ex.Message);
                throw;
            }
        }


        //GUARDAR CAMBIOS AL EDITAR PRESENTACION
        public void GuardarCambiosEditarPresentacion()
        {
            By guardarcambiosPresentacion = By.XPath("//button[normalize-space()='Guardar Cambios']");
            utilities.ClickButton(guardarcambiosPresentacion);
        }

        //ELIMINAR CONCEPTO AL EDITAR PRESENTACION
        public void eliminarConceptoEditarPresentacion()
        {
            By eliminarconceptoEditarPresentacion = By.XPath("//input[@class='ng-untouched ng-pristine ng-valid ng-star-inserted']");
            utilities.ClickButton(eliminarconceptoEditarPresentacion);
        }

        // CREAR NUEVA Y APLICAR CAMBIOS EDITAR PRESENTACION
        public void aplicarcambiosEditarPresentacion()
        {
            By aplicarcambiosEditarPresentacion = By.XPath("//button[normalize-space()='Crear nueva y aplicar cambios (1)']");
            utilities.ClickButton(aplicarcambiosEditarPresentacion);
            By botonOK = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonOK);
        }

        //METODO PARA ELIMINAR PRESENTACION
        public void EliminarPresentacion(string nombrePresentacion)
        {
            try
            {
                MostrarTodasLasPresentaciones();

                By botonEliminar = By.XPath($"//td[normalize-space()='{nombrePresentacion}']/parent::tr//app-button-actions//button[2]");

                var elementos = driver.FindElements(botonEliminar);

                if (elementos.Count > 0)
                {
                    utilities.ClickButton(botonEliminar);

                    Thread.Sleep(500);

                    By confirmar = By.XPath("//button[normalize-space()='Sí, eliminar']");
                    utilities.ClickButton(confirmar);
                }
                else
                {
                    throw new Exception($"No se encontró la presentación '{nombrePresentacion}' para eliminar.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar la presentación: " + ex.Message);
                throw;
            }
        }


        public void SeleccionarNuevaPresentacionParaReasignar(string nuevaPresentacion)
        {
            try
            {
                // 1. Esperar que el modal aparezca
                Thread.Sleep(1000);

                // 2. Ubicar el dropdown dentro del modal
                By dropdown = By.XPath("//div[contains(@class,'modal-content')]//select[contains(@class,'form-select')]");
                utilities.ClickButton(dropdown);

                Thread.Sleep(500);

                // 3. Opción dinámica
                By opcion = By.XPath($"//div[contains(@class,'modal-content')]//option[normalize-space()='{nuevaPresentacion}']");

                var elementos = driver.FindElements(opcion);

                if (elementos.Count > 0)
                {
                    utilities.ClickButton(opcion);
                }
                else
                {
                    throw new Exception($"No se encontró la presentación '{nuevaPresentacion}' en el modal.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al reasignar presentación: " + ex.Message);
                throw;
            }
        }

        public void eliminarConceptoEliminarPresentacion()
        {
            By checkeliminarconcepto = By.XPath("//input[@class='ng-untouched ng-pristine ng-valid ng-star-inserted']");
            utilities.ClickButton(checkeliminarconcepto);
        }

        public void reasignarEliminarPresentacion()
        {
            By botonreasignarEliminarPresentacion = By.XPath("//button[normalize-space()='Reasignar y eliminar (1)']");
            utilities.ClickButton(botonreasignarEliminarPresentacion);
        }

        public void DesactivarPresentacionDesdeGrilla(string nombrePresentacion)
        {
            // 1. MOSTRAR TODAS LAS PRESENTACIONES
            MostrarTodasLasPresentaciones();

            // 2. XPath dinámico
            By switchPresentacion = By.XPath($"//tbody/tr[td[normalize-space()='{nombrePresentacion}']]//input[@type='checkbox']");

            var elementos = driver.FindElements(switchPresentacion);

            if (elementos.Count > 0)
            {
                utilities.ClickButton(switchPresentacion);
            }
            else
            {
                throw new Exception($"No se encontró la presentación '{nombrePresentacion}' en la grilla.");
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------


        //---------------------------------------------------CARACTERÍSTICAS --------------------------------------------------

        public void SeleccionarOpcionCaracteristica()
        {
            utilities.ClickButton(opcionCaracteristicas);
        }

        public void IngresarNombreCaracteristicaComun(string nombre)
        {
            utilities.EnterText(nombreCaracteristicaComun, nombre);
        }

        public void MostrarTodasLasCaracteristicasComunes()
        {
            try
            {
                By desplegable = By.XPath("(//select[contains(@class,'form-select')])[1]");
                By opcionCien = By.XPath("(//select[contains(@class,'form-select')])[1]/option[@value='100']");

                utilities.ClickButton(desplegable);
                Thread.Sleep(300);
                utilities.ClickButton(opcionCien);
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se pudo ajustar el paginador de características: " + ex.Message);
            }
        }

        public void EditarCaracteristicaComun(string nombreCaracteristica)
        {
            MostrarTodasLasCaracteristicas();

            By botonEditar = By.XPath($"//tbody/tr[td[normalize-space()='{nombreCaracteristica}']]//app-button-actions//button[1]");

            var elementos = driver.FindElements(botonEditar);

            if (elementos.Count > 0)
            {
                utilities.ClickButton(botonEditar);
            }
            else
            {
                throw new Exception($"No se encontró la característica '{nombreCaracteristica}'.");
            }
        }

        public void guardarcambiosCaracteristicaComun()
        {
            By botonguardarcambiosCaracteristicaComun = By.XPath("//button[normalize-space()='Guardar Cambios']");
            utilities.ClickButton(botonguardarcambiosCaracteristicaComun);
            By botonOK = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonOK);
        }

        public void IngresarCodigoCaracteristicaPropia(string codigo)
        {
            utilities.EnterText(codigoCaracteristicaPropia, codigo);
        }


        public void IngresarNombreCaracteristicaPropia(string nombre)
        {
            utilities.EnterText(nombreCaracteristicaPropia, nombre);
        }

        public void SeleccionarTipoDatoCaracteristicaPropia(string tipoDato)
        {
            utilities.ClickButton(tipoDatoCaracteristicaPropia);

            By opcionTipoDato = By.XPath($"//select[@id='tipo-dato']/option[normalize-space()='{tipoDato}']");
            utilities.ClickButton(opcionTipoDato);
        }

        public void MostrarTodasLasCaracteristicasPropias()
        {
            try
            {
                By desplegable = By.XPath("(//select[contains(@class,'form-select')])[1]");
                By opcionCien = By.XPath("(//select[contains(@class,'form-select')])[1]/option[@value='100']");

                utilities.ClickButton(desplegable);
                Thread.Sleep(300);
                utilities.ClickButton(opcionCien);
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se pudo ajustar el paginador de características propias: " + ex.Message);
            }
        }

        public void EditarCaracteristicaPropia(string nombreCaracteristica)
        {
            MostrarTodasLasCaracteristicasPropias();

            By botonEditar = By.XPath($"//tbody/tr[td[normalize-space()='{nombreCaracteristica}']]//app-button-actions//button[1]");

            var elementos = driver.FindElements(botonEditar);

            if (elementos.Count > 0)
            {
                utilities.ClickButton(botonEditar);
            }
            else
            {
                throw new Exception($"No se encontró la característica propia '{nombreCaracteristica}'.");
            }
        }

        public void guardarcambiosCaracteristicaPropia()
        {
            By botonguardarcambiosCaracteristicaComun = By.XPath("//button[normalize-space()='Guardar Cambios']");
            utilities.ClickButton(botonguardarcambiosCaracteristicaComun);
            By botonOK = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonOK);
        }

        //-------------------------------------------------------------------------------------------------------------


        //---------------------------------------------------VALOR CARACTERISTICA COMÚN--------------------------------------------------


        public void SeleccionarOpcionValorCaracteristica()
        {
            utilities.ClickButton(opcionValorCaracteristica);
        }

        public void SeleccionarCaracteristicaComun(string valor)
        {
            utilities.ClickButton(seleccionCaracteristicaComun);

            By opcionValor = By.XPath($"//select[@id='tipo-dato']//option[normalize-space()='{valor}']");
            utilities.ClickButton(opcionValor);
        }

        public void IngresarValorCaracteristicaComun(string valor)
        {
            By valorcaracteristicacomun = By.XPath("//input[@placeholder='Valor']");
            utilities.EnterText(valorcaracteristicacomun, valor);
        }

        public void guardarValorCaracteristicaComun()
        {
            By botonagregar = By.XPath("//i[@class='icon-plus']");
            utilities.ClickButton(botonagregar);
            By botonok = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonok);
        }

        public void EditarValorCaracteristicaComun(string valor)
        {
            By botonEditar = By.XPath($"//tbody/tr[td[normalize-space()='{valor}']]//app-button-actions//button[1]");

            var elementos = driver.FindElements(botonEditar);

            if (elementos.Count > 0)
            {
                utilities.ClickButton(botonEditar);
            }
            else
            {
                throw new Exception($"No se encontró el valor '{valor}' para editar.");
            }
        }


        public void guardarcambiosEditarValorCaracteristicaComun()
        {
            By botonguardarcambiosValorCaracteristicacomun = By.XPath("//button[normalize-space()='Guardar Cambios']");
            utilities.ClickButton(botonguardarcambiosValorCaracteristicacomun);
            By botonOK = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonOK);
        }

        public void actualizarValorCaracteristicaComun()
        {
            By botonSI = By.XPath("//button[normalize-space()='SI']");
            utilities.ClickButton(botonSI);
        }

        public void eliminarconceptoEditarValorCaracteristicaComun()
        {
            By checkeliminarconceptoValorCaracteristicaComun = By.XPath("//input[@class='ng-untouched ng-pristine ng-valid ng-star-inserted']");
            utilities.ClickButton(checkeliminarconceptoValorCaracteristicaComun);
        }

        
        public void EliminarValorCaracteristicaComun(string valor)
        {
            By botonEliminar = By.XPath($"//tbody/tr[td[normalize-space()='{valor}']]//app-button-actions//button[2]");

            var elementos = driver.FindElements(botonEliminar);

            if (elementos.Count > 0)
            {
                utilities.ClickButton(botonEliminar);

                // Confirmación modal
                By botonConfirmar = By.XPath("//button[normalize-space()='Sí, eliminar']");
                utilities.ClickButton(botonConfirmar);
            }
            else
            {
                throw new Exception($"No se encontró el valor '{valor}' para eliminar.");
            }
        }

        public void SeleccionarNuevoValorCaracteristicaComun(string nuevoValor)
        {
            // 1. Esperar a que el modal aparezca
            Thread.Sleep(1000);

            // 2. Dropdown dentro del modal
            By dropdown = By.XPath("//div[contains(@class,'modal-content')]//select[contains(@class,'form-select')]");
            utilities.ClickButton(dropdown);

            Thread.Sleep(500);

            // 3. Opción dinámica
            By opcion = By.XPath($"//div[contains(@class,'modal-content')]//select[contains(@class,'form-select')]//option[normalize-space()='{nuevoValor}']");

            utilities.ClickButton(opcion);
        }


        public void eliminarconceptoEliminarValorCaracteristicaComun()
        {
            By checkeliminarconceptoEliminarValorCaracteristicaComun = By.XPath("//input[@class='ng-untouched ng-pristine ng-valid ng-star-inserted']");
            utilities.ClickButton(checkeliminarconceptoEliminarValorCaracteristicaComun);
        }


        //-------------------------------------------------------------------------------------------------------------


        //---------------------------------------------------ASIGNAR VALOR CARACTERISTICA POR FAMILIA --------------------------------------------------


        public void SeleccionarOpcionAsignarValorCaracteristicaFamilia()
        {
            utilities.ClickButton(opcionAsignarValorCaracteristicaFamilia);
        }

        public void SeleccionarFamiliaParaAsignarValor(string familia)
        {
            utilities.ClickButton(dropdownFamilia);
            By opcion = By.XPath($"//select[@id='familia']/option[normalize-space()='{familia}']");
            utilities.ClickButton(opcion);
        }


        public void IngresarValorParaAsignarFamilia(string valor)
        {
            utilities.ClickButton(botonAgregarValorParaAsignar);
            By campoValorAgregar = By.XPath("//input[@placeholder='Ej. Rojo, Grande, 10 kg']");
            utilities.EnterText(campoValorAgregar, valor);
            By botonguardar = By.XPath("//button[normalize-space()='Guardar']");
            utilities.ClickButton(botonguardar);
            By botonok = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonok);
        }


        public void ArrastrarValorAFamilia(string valor)
        {
            By valorDisponible = By.XPath($"//span[@draggable='true' and normalize-space()='{valor}']");
            By contenedorAsignado = By.XPath("//div[contains(@class,'assigned-zone')]");

            IWebElement origen = driver.FindElement(valorDisponible);
            IWebElement destino = driver.FindElement(contenedorAsignado);

            Actions actions = new Actions(driver);
            actions.DragAndDrop(origen, destino).Perform();
        }

        public void guardarAsignacion()
        {
            By botonGuardar = By.XPath("//button[normalize-space()='Guardar asignaciones']");
            utilities.ClickButton(botonGuardar);
        }
        

        //-------------------------------------------------------------------------------------------------------------

        public void GuardarRegistro()
        {
            utilities.ClickButton(botonGuardar);
            By botonok = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonok);
        }

        public void NoguardarRegistro()
        {
            utilities.ClickButton(botonGuardar);
            By botonok = By.XPath("//button[normalize-space()='OK']");
            utilities.ClickButton(botonok);
        }
    }
}
