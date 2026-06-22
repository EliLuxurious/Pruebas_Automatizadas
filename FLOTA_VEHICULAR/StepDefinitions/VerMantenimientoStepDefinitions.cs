using FLOTA_VEHICULAR.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using System;
using System.IO;

namespace FLOTA_VEHICULAR.StepDefinitions
    {
        [Binding]
        public class VerMantenimientoStepDefinitions
        {
            private readonly IWebDriver driver;
            private readonly VerMantenimientoPage verMantenimientoPage;

            public VerMantenimientoStepDefinitions(IWebDriver driver)
            {
                this.driver = driver;
                verMantenimientoPage = new VerMantenimientoPage(driver);
            }

            // ===============================
            // NAVEGACIÓN
            // ===============================

            [When(@"Se ingresa al módulo Mantenimiento y submódulo Ver Mantenimientos")]
            public void WhenSeIngresaAlModuloMantenimientoYSubmoduloVerMantenimientos()
            {
                // Este método asume que lo agregaste en VerMantenimientoPage como te indiqué antes.
                // Si lo dejaste en VerCatalogoPage, puedes inyectar VerCatalogoPage aquí también.
                verMantenimientoPage.IngresarSubmoduloVerMantenimientos();
                Console.WriteLine("Navegación exitosa a Ver Mantenimientos.");
            }

            // ===============================
            // ACCIONES DE FORMULARIO
            // ===============================

            [When(@"Se hace clic en el botón Nuevo Mantenimiento")]
            public void WhenSeHaceClicEnElBotonNuevoMantenimiento()
            {
                verMantenimientoPage.ClicNuevoMantenimiento();
            }

            [When(@"Se busca la placa ""(.*)"" para autocompletar los datos")]
            public void WhenSeBuscaLaPlacaParaAutocompletarLosDatos(string placa)
            {
                verMantenimientoPage.BuscarPlaca(placa);
                Console.WriteLine($"Se ingresó la placa '{placa}' y se hizo clic en la lupa.");
            }

            [When(@"Se ingresan los detalles del mantenimiento:")]
            public void WhenSeIngresanLosDetallesDelMantenimiento(DataTable table)
            {
                foreach (var row in table.Rows)
                {
                    string campo = row["Campo"].Trim().ToUpper();
                    string valor = row["Valor"].Trim();

                    switch (campo)
                    {
                        
                        case "KM":
                            verMantenimientoPage.IngresarKm(valor);
                            break;
                        case "MONTO TOTAL":
                            verMantenimientoPage.IngresarMonto(valor);
                            break;
                        case "FECHA":
                            verMantenimientoPage.IngresarFechaMantenimiento(valor);
                            break;
                    }
                }
            }

            [When(@"Se agrega la actividad ""(.*)""")]
            public void WhenSeAgregaLaActividad(string actividad)
            {
                verMantenimientoPage.AgregarActividad(actividad);
                Console.WriteLine($"Actividad agregada: {actividad}");
            }

            [When(@"Se agrega el repuesto ""(.*)""")]
            public void WhenSeAgregaElRepuesto(string repuesto)
            {
                verMantenimientoPage.AgregarRepuesto(repuesto);
                Console.WriteLine($"Repuesto agregado: {repuesto}");
            }

            [When(@"Se adjunta el documento ""(.*)""")]
            public void WhenSeAdjuntaElDocumento(string nombreArchivo)
            {
                // Ruta dinámica hacia la carpeta TestData de tu proyecto
                string rutaDirectorioBase = AppDomain.CurrentDomain.BaseDirectory;
                string rutaCompleta = Path.GetFullPath(Path.Combine(rutaDirectorioBase, @"..\..\..\TestData\", nombreArchivo));

                verMantenimientoPage.AdjuntarDocumento(rutaCompleta);
                Console.WriteLine($"Documento adjuntado desde: {rutaCompleta}");
            }

            // ===============================
            // GUARDADO Y VALIDACIÓN
            // ===============================

            [Then(@"Se guarda el registro de mantenimiento")]
            public void ThenSeGuardaElRegistroDeMantenimiento()
            {
                verMantenimientoPage.GuardarMantenimiento();
                Console.WriteLine("Se hizo clic en Guardar.");
            }

            [Then(@"el sistema debe mostrar el mensaje de éxito ""(.*)""")]
            public void ThenElSistemaDebeMostrarElMensajeDeExito(string mensajeEsperado)
            {
                bool exito = verMantenimientoPage.ValidarMensajeExito(mensajeEsperado);

                // Si no aparece el mensaje, el Assert hace estallar la prueba y nos avisa del bug
                Assert.IsTrue(exito, $"[ERROR QA]: No se mostró el mensaje verde de confirmación con el texto '{mensajeEsperado}'.");

                Console.WriteLine($"Validación QA exitosa: El sistema mostró el mensaje '{mensajeEsperado}'.");
            }

            [Then(@"el sistema debe mostrar el mensaje de error ""(.*)""")]
            public void ThenElSistemaDebeMostrarElMensajeDeError(string mensajeEsperado)
            {
                bool aparecioError = verMantenimientoPage.ValidarMensajeError(mensajeEsperado);

                // Si el error NO aparece (aparecioError es false), significa que el sistema 
                // dejó pasar el KM inválido, por lo tanto, la prueba falla y reporta el Bug.
                Assert.IsTrue(aparecioError, $"[BUG DETECTADO]: El sistema permitió guardar el registro con un KM menor al anterior, o no mostró la alerta con el texto '{mensajeEsperado}'.");

                Console.WriteLine($"Validación QA exitosa: El sistema bloqueó correctamente el registro por KM inferior.");
            }



            // ===============================
            // HISTORIAL DE MANTENIMIENTOS
            // ===============================

            [When(@"Se hace clic en el botón HISTORIAL")]
            public void WhenSeHaceClicEnElBotonHISTORIAL()
            {
                verMantenimientoPage.ClicBotonHistorial();
            }

            [When(@"Se busca la placa ""(.*)"" en el historial")]
            public void WhenSeBuscaLaPlacaEnElHistorial(string placa)
            {
                verMantenimientoPage.BuscarPlacaEnHistorial(placa);
                Console.WriteLine($"Se buscó la placa '{placa}' en el modal de historial.");
            }

            [Then(@"el sistema debe mostrar el listado de mantenimientos históricos relacionados a la placa")]
            public void ThenElSistemaDebeMostrarElListadoDeMantenimientosHistoricos()
            {
                bool tieneRegistros = verMantenimientoPage.ValidarHistorialTieneRegistros();

                // Si tieneRegistros es FALSE, la prueba revienta avisando del Bug
                Assert.IsTrue(tieneRegistros, "[BUG DETECTADO]: El sistema muestra 'No hay Vehículo' o la tabla está vacía, a pesar de que el vehículo tiene mantenimientos registrados.");

                Console.WriteLine("Validación QA exitosa: La tabla se llenó correctamente con el historial de mantenimientos.");
            }

            [Then(@"el sistema no debe mostrar mantenimientos registrados para la placa")]
            public void ThenElSistemaNoDebeMostrarMantenimientosRegistrados()
            {
                // Llamamos a nuestro escáner de la tabla
                bool tieneRegistros = verMantenimientoPage.ValidarHistorialTieneRegistros();

                // Usamos Assert.IsFalse porque queremos asegurar que la tabla NO tenga datos
                Assert.IsFalse(tieneRegistros, "[BUG DETECTADO]: El sistema cargó registros en la tabla para un vehículo recién creado que no debería tener ningún mantenimiento histórico.");

                Console.WriteLine("Validación QA exitosa: La tabla se mostró correctamente vacía (sin mantenimientos) para este vehículo.");
            }



                // ===============================
            // ELIMINAR MANTENIMIENTO
            // ===============================

            [When(@"Se filtra el mantenimiento por placa ""(.*)"" y monto ""(.*)""")]
            public void WhenSeFiltraElMantenimientoPorPlacaYMonto(string placa, string monto)
            {
                verMantenimientoPage.FiltrarMantenimiento(placa, monto);
                Console.WriteLine($"Se filtró la grilla por la Placa '{placa}' y el Monto '{monto}'.");
            }

            [When(@"Se abre el detalle del primer registro encontrado")]
            public void WhenSeAbreElDetalleDelPrimerRegistroEncontrado()
            {
                verMantenimientoPage.AbrirDetallePrimerRegistro();
            }

            [When(@"Se hace clic en el botón Eliminar Mantenimiento")]
            public void WhenSeHaceClicEnElBotonEliminarMantenimiento()
            {
                verMantenimientoPage.ClicEliminarMantenimiento();
            }



            [Then(@"al filtrar nuevamente por placa ""(.*)"" y monto ""(.*)"" el registro ya no debe aparecer en la grilla")]
            public void ThenAlFiltrarNuevamentePorPlacaYMontoElRegistroYaNoDebeAparecer(string placa, string monto)
            {
                // Volvemos a filtrar ingresando ambos datos
                verMantenimientoPage.FiltrarMantenimiento(placa, monto);

                bool eliminadoExitosamente = verMantenimientoPage.ValidarRegistroNoExisteEnGrilla();

                // Si el registro todavía existe, reportamos el bug
                Assert.IsTrue(eliminadoExitosamente, $"[BUG DETECTADO]: El mantenimiento de la placa '{placa}' y monto '{monto}' fue eliminado, pero sigue apareciendo en la grilla.");

                Console.WriteLine($"Validación QA exitosa: El registro (Placa: {placa}, Monto: {monto}) fue dado de baja exitosamente.");
            }


            [When(@"Se confirma la eliminación del mantenimiento")]
            public void WhenSeConfirmaLaEliminacionDelMantenimiento()
            {
                verMantenimientoPage.ConfirmarEliminacion();
                Console.WriteLine("Se hizo clic en el botón rojo de Eliminar dentro del modal de confirmación.");
            }

        // ===============================
        // EDITAR MANTENIMIENTO
        // ===============================

        [When(@"Se hace clic en el botón Editar Mantenimiento")]
        public void WhenSeHaceClicEnElBotonEditarMantenimiento()
        {
            verMantenimientoPage.ClicEditarMantenimiento();
            Console.WriteLine("Se hizo clic en el botón Editar (Lápiz) y los campos se habilitaron.");
        }

        [When(@"Se actualiza el monto total a ""(.*)""")]
        public void WhenSeActualizaElMontoTotalA(string nuevoMonto)
        {
            // ¡Reutilizamos tu método maestro que limpia la caja y escribe el nuevo dato!
            verMantenimientoPage.IngresarMonto(nuevoMonto);
            Console.WriteLine($"Se actualizó el monto total a: {nuevoMonto}");
        }


        [When(@"Se agrega la actividad ""(.*)"" en edición")]
        public void WhenSeAgregaLaActividadEnEdicion(string actividad)
        {
            verMantenimientoPage.AgregarActividadEnEdicion(actividad);
            Console.WriteLine($"Se agregó la actividad '{actividad}' directamente desde el desplegable.");
        }

        [When(@"Se agrega el repuesto ""(.*)"" en edición")]
        public void WhenSeAgregaElRepuestoEnEdicion(string repuesto)
        {
            verMantenimientoPage.AgregarRepuestoEnEdicion(repuesto);
            Console.WriteLine($"Se agregó el repuesto '{repuesto}' directamente desde el desplegable.");
        }




        // ===============================
        // FILTROS PRINCIPALES
        // ===============================

        [When(@"Se selecciona el tipo de mantenimiento ""(.*)"" en el filtro")]
        public void WhenSeSeleccionaElTipoDeMantenimientoEnElFiltro(string tipo)
        {
            verMantenimientoPage.SeleccionarFiltroTipoMantenimiento(tipo);
            Console.WriteLine($"Se seleccionó '{tipo}' en el desplegable de Tipo de Mantenimiento.");
        }

        [When(@"Se hace clic en el botón BUSCAR de Mantenimientos")]
        public void WhenSeHaceClicEnElBotonBuscarDeMantenimientos()
        {
            verMantenimientoPage.ClicBotonBuscarPrincipal();
            Console.WriteLine("Se hizo clic en BUSCAR.");
        }

        [Then(@"todos los resultados en la columna Tipo de Mantenimiento deben coincidir con ""(.*)""")]
        public void ThenTodosLosResultadosEnLaColumnaTipoDeMantenimientoDebenCoincidirCon(string valorEsperado)
        {
            verMantenimientoPage.ValidarColumnaTipoMantenimiento(valorEsperado);
            Console.WriteLine($"Validación QA exitosa: Todas las filas de la tabla coinciden estrictamente con el tipo '{valorEsperado}'.");
        }







    }
    }















