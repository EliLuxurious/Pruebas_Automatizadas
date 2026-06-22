using FLOTA_VEHICULAR.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace FLOTA_VEHICULAR.StepDefinitions
{
    [Binding]
    public class VehiculoStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly VehiculoPage vehiculoPage;

        public VehiculoStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            vehiculoPage = new VehiculoPage(driver);
        }

        // ===============================
        // NAVEGACION MODULO
        // ===============================

        [Given("Se ingresa al módulo {string}")]
        [When("Se ingresa al módulo {string}")]
        public void SeIngresaAlModulo(string modulo)
        {
            vehiculoPage.IngresarModuloVehiculo();
        }

        [Given("Se selecciona {string}")]
        [When("Se selecciona {string}")]
        public void SeSelecciona(string opcion)
        {
            vehiculoPage.ClickNuevoVehiculo();
        }

        // ===============================
        // INGRESO DE DATOS
        // ===============================

         [When("Se ingresan los datos del vehículo:")]
        public void WhenSeIngresanLosDatosDelVehiculo(DataTable table)
        {
            foreach (var row in table.Rows)
            {
                string campo = row["Campo"].Trim().ToUpper();
                string valor = row["Valor"].Trim();

                switch (campo)
                {
                    case "PLACA":
                        vehiculoPage.IngresarPlaca(valor);
                        break;

                    case "AREA ASIGNADA":
                        vehiculoPage.SeleccionarArea(valor);
                        break;

                    case "PROPIETARIO":
                        vehiculoPage.SeleccionarPropietario(valor);
                        break;

                    case "MARCA":
                        vehiculoPage.SeleccionarMarca(valor);
                        break;

                    case "MODELO":
                        vehiculoPage.SeleccionarModelo(valor);
                        break;

                    case "AÑO":
                        vehiculoPage.SeleccionarAnio(valor);
                        break;

                    case "TIPO DE VEHICULO":
                        vehiculoPage.SeleccionarTipoVehiculo(valor);
                        break;

                    case "CLASIFICADOR":
                        vehiculoPage.SeleccionarClasificador(valor);
                        break;

                    case "COLOR":
                        vehiculoPage.IngresarColor(valor);
                        break;

                    case "NUMERO MOTOR":
                        vehiculoPage.IngresarMotor(valor);
                        break;

                    case "TIPO COMBUSTIBLE":
                        vehiculoPage.SeleccionarCombustible(valor);
                        break;

                    case "TIPO MOTOR":
                        vehiculoPage.SeleccionarTipoMotor(valor);
                        break;

                    case "RANGO CONSUMO":
                        vehiculoPage.IngresarConsumo(valor);
                        break;

                    case "NUMERO SERIE":
                        vehiculoPage.IngresarNumeroSerie(valor);
                        break;
                }
            }
        }


       /* [When("Se ingresan los datos del vehículo:")]
        public void WhenSeIngresanLosDatosDelVehiculo(DataTable table)
        {
            foreach (var row in table.Rows)
            {
                string campo = row["Campo"].Trim().ToUpper();
                string valor = row["Valor"].Trim();

                switch (campo)
                {
                    case "PLACA":
                        vehiculoPage.IngresarPlaca(valor);

                        // --- AGREGAMOS ESTA VALIDACIÓN AQUÍ ---
                        // Si sale el aviso de error (//div[@role='alert']), lanzamos la falla de QA
                        if (vehiculoPage.ValidarAvisoPlacaDuplicada())
                        {   
                            throw new Exception($"[BUG DETECTADO]: El sistema mostró un aviso de 'Placa Duplicada' para la placa {valor}, impidiendo la reasignación a pesar de estar DE BAJA.");
                        }
                        break;

                    case "AREA ASIGNADA":
                        vehiculoPage.SeleccionarArea(valor);
                        break;

                    case "PROPIETARIO":
                        vehiculoPage.SeleccionarPropietario(valor);
                        break;

                    case "MARCA":
                        vehiculoPage.SeleccionarMarca(valor);
                        break;

                    case "MODELO":
                        vehiculoPage.SeleccionarModelo(valor);
                        break;

                    case "AÑO":
                        vehiculoPage.SeleccionarAnio(valor);
                        break;

                    case "TIPO DE VEHICULO":
                        vehiculoPage.SeleccionarTipoVehiculo(valor);
                        break;

                    case "CLASIFICADOR":
                        vehiculoPage.SeleccionarClasificador(valor);
                        break;

                    case "COLOR":
                        vehiculoPage.IngresarColor(valor);
                        break;

                    case "NUMERO MOTOR":
                        vehiculoPage.IngresarMotor(valor);
                        break;

                    case "TIPO COMBUSTIBLE":
                        vehiculoPage.SeleccionarCombustible(valor);
                        break;

                    case "TIPO MOTOR":
                        vehiculoPage.SeleccionarTipoMotor(valor);
                        break;

                    case "RANGO CONSUMO":
                        vehiculoPage.IngresarConsumo(valor);
                        break;

                    case "NUMERO SERIE":
                        vehiculoPage.IngresarNumeroSerie(valor);
                        break;
                }
            }
        }
        */
        // ===============================
        // GUARDAR
        // ===============================
        [When("Se procede a {string} el vehículo")]
        [Then("Se procede a {string} el vehículo")]
        public void ThenSeProcedeA(string accion)
        {
            if (accion.ToUpper().Contains("GUARDAR"))
            {
                vehiculoPage.GuardarVehiculo();
            }
        }

        // ===============================
        // DAR DE BAJA
        // ===============================

        [When("Se busca el vehículo por placa {string}")]
        public void WhenSeBuscaElVehiculoPorPlaca(string placa)
        {
            vehiculoPage.BuscarVehiculoPorPlaca(placa);
        }

        [When("Se hace clic en ver vehículo")]
        public void WhenSeHaceClicEnVerVehiculo()
        {
            vehiculoPage.ClicVerVehiculo();
        }

        [When("Se hace clic en dar de baja")]
        public void WhenSeHaceClicEnDarDeBaja()
        {
            vehiculoPage.ClicDarDeBaja();
        }

        [When("Se ingresan las observaciones {string}")]
        public void WhenSeIngresanLasObservaciones(string observaciones)
        {
            vehiculoPage.IngresarObservaciones(observaciones);
        }



        [When("Se confirma la baja del vehículo")]
        [Then("Se confirma la baja del vehículo")]
        public void ThenSeConfirmaLaBajaDelVehiculo()
        {
            vehiculoPage.ConfirmarBaja();
        }


        [When("Se hace clic en editar")]
        public void WhenSeHaceClicEnEditar()
        {
            vehiculoPage.ClicEditarVehiculo();
        }


        //Registrar vehiculo con motor de baja

        [Then(@"El sistema valida que el motor ""(.*)"" es aceptado por estar DE BAJA")]
        public void ThenElSistemaValidaQueElMotorEsAceptadoPorEstarDEBAJA(string motor)
        {
            // Ejecutamos la validación técnica basada en el ID EngineNumber
            bool esAceptado = vehiculoPage.ValidarEstadoCampoMotor();

            if (!esAceptado)
            {
                throw new Exception($"Error en el Paso 14: El motor '{motor}' fue rechazado por el sistema (se mostró alerta de duplicado).");
            }

            Console.WriteLine($"Confirmado: El motor '{motor}' fue validado correctamente en tiempo real.");
        
        
        }

        [When(@"Se cierra la ventana de detalles")]
        public void WhenSeCierraLaVentanaDeDetalles()
        {
            vehiculoPage.CerrarVentanaDetalles();
        }

        [Then(@"el botón GUARDAR debe permanecer inhabilitado")]
        public void ThenElBotonGUARDARDebePermanecerInhabilitado()
        {
            bool estaBloqueado = vehiculoPage.ValidarBotonGuardarInhabilitado();
            Assert.IsTrue(estaBloqueado, "¡bug! El botón GUARDAR está habilitado a pesar de que el motor es un duplicado ACTIVO.");
        }

        [Then(@"el botón GUARDAR debe estar habilitado para permitir la reasignación")]
        public void ThenElBotonGuardarDebeEstarHabilitado()
        {
            bool estaHabilitado = vehiculoPage.ValidarBotonGuardarHabilitado();

           
            Assert.IsTrue(estaHabilitado, "FALLO DE SISTEMA: El botón GUARDAR está inhabilitado. El sistema no permite reutilizar una placa de un vehículo 'DE BAJA'.");
        }

        
        [When(@"se ingresa la placa ""(.*)"" y se valida que no exista el error de duplicado")]
        public void WhenSeIngresaLaPlacaYValidaDuplicado(string placa)
        {
            // Ingresamos la placa
            vehiculoPage.IngresarPlaca(placa);

            // Validamos el error inmediatamente usando el XPath //div[@role='alert']
            if (vehiculoPage.ValidarAvisoPlacaDuplicada())
            {
                throw new Exception($"[BUG DETECTADO]: El sistema bloqueó la placa '{placa}' indicando que está duplicada, pero debería permitirla porque está DE BAJA.");
            }
        }


        [When(@"se ingresa la placa ""(.*)"" y el sistema debe mostrar error de duplicado por estar ACTIVO")]
        public void WhenSeIngresaPlacaActivaYEsperaError(string placa)
        {
            // 1. Ingresamos la placa
            vehiculoPage.IngresarPlaca(placa);

            // 2. Validamos que el mensaje de error aparezca
            bool hayError = vehiculoPage.ValidarAvisoPlacaDuplicada();

            // Si NO hay error, es un fallo del test (porque debería estar bloqueado)
            if (!hayError)
            {
                throw new Exception($"[FALLO DE VALIDACIÓN]: El sistema permitió la placa '{placa}' que está ACTIVA. Debería haber mostrado un error de duplicado.");
            }

            Console.WriteLine("Éxito: El sistema bloqueó correctamente la placa duplicada activa.");
        }

        [Then(@"el botón GUARDAR debe estar deshabilitado")]
        public void ThenElBotonGuardarDeshabilitado()
        {
            // Reutilizamos la lógica inversa de tu método de habilitado
            bool estaHabilitado = vehiculoPage.ValidarBotonGuardarHabilitado();

            if (estaHabilitado)
            {
                throw new Exception("[BUG]: El botón GUARDAR está habilitado a pesar de que la placa está duplicada y activa.");
            }

            Console.WriteLine("Éxito: El botón GUARDAR está bloqueado correctamente.");
        }


        [When(@"se ingresa el motor ""(.*)"" y el sistema debe impedir el registro por estar asociado a un vehículo ACTIVO")]
        public void WhenSeIngresaMotorActivoYEsperaError(string motor)
        {
            // 1. Ingresamos el motor
            vehiculoPage.IngresarMotor(motor);

            // 2. Esperamos a que el sistema valide
            System.Threading.Thread.Sleep(2000);

            // 3. Validamos si apareció el error
            bool hayError = vehiculoPage.ValidarAvisoMotorDuplicado();

            // LÓGICA: Si NO hay error (hayError == false), significa que el sistema tiene el BUG
            if (!hayError)
            {
                throw new Exception($"[BUG DETECTADO]: El sistema permitió ingresar el motor '{motor}' que ya pertenece a un vehículo ACTIVO. Debería mostrar un aviso de error.");
            }

            Console.WriteLine("Éxito: El sistema bloqueó el motor duplicado activo correctamente.");
        }


        [Then(@"el botón GUARDAR debe permanecer inhabilitado por falta de cambios")]
        public void ThenElBotonGuardarInhabilitadoPorFaltaDeCambios()
        {
            // Esperamos un segundo para que Angular procese que no hubo inputs
            System.Threading.Thread.Sleep(1000);

            bool estaInhabilitado = vehiculoPage.ValidarBotonGuardarInhabilitado();

            Assert.IsTrue(estaInhabilitado, "[BUG]: El botón GUARDAR se habilitó pero no se ha realizado ninguna modificación en el formulario.");
        }

        [Then(@"el botón GUARDAR debe habilitarse al detectar cambios en el formulario")]
        public void ThenElBotonGuardarHabilitarse()
        {
            // Usamos tu método de la Page
            bool estaHabilitado = vehiculoPage.ValidarBotonGuardarHabilitado();

            Assert.IsTrue(estaHabilitado, "[BUG]: El botón GUARDAR sigue inhabilitado. El sistema no detectó los cambios realizados en el vehículo.");
        }

        [Then(@"el sistema no debe mostrar la opción de editar para vehículos con estado de baja")]
        public void ThenNoDebeMostrarOpcionEditar()
        {
            bool botonVisible = vehiculoPage.ExisteBotonEditar();

            // Si botonVisible es TRUE, significa que hay un BUG de seguridad
            if (botonVisible)
            {
                throw new Exception("[BUG DETECTADO]: La opción 'Editar' sigue disponible para un vehículo en estado DE BAJA. Por seguridad, esta opción debe estar oculta.");
            }

            Console.WriteLine("Éxito: El botón de editar está oculto correctamente para registros de baja.");
        }


        [Then(@"al buscar la placa ""(.*)"" en la bandeja, su estado debe ser ""(.*)""")]
        public void ThenAlBuscarLaPlacaEnLaBandejaSuEstadoDebeSer(string placa, string estadoEsperado)
        {
            // Validamos que la fila de esa placa tenga el estado correcto en la grilla
            bool estadoCorrecto = vehiculoPage.ValidarEstadoVehiculoEnGrilla(placa, estadoEsperado);
            Assert.IsTrue(estadoCorrecto, $"[BUG DETECTADO]: En la bandeja principal, la placa {placa} NO muestra el estado {estadoEsperado}.");
        }

        [When(@"se hace clic en reportar avería")]
        public void WhenSeHaceClicEnReportarAveria()
        {
            vehiculoPage.ClicReportarAveria();
        }

        [When(@"se ingresan las observaciones de avería ""(.*)""")]
        public void WhenSeIngresanObservacionesAveria(string obs)
        {
            // Reutilizamos tu método de ingresar observaciones que ya existía para dar de baja
            vehiculoPage.IngresarObservaciones(obs);
        }

        [When(@"se confirma el reporte de la avería")]
        public void WhenSeConfirmaElReporteAveria()
        {
            // Reutilizamos tu método de confirmar que ya existía
            vehiculoPage.ConfirmarBaja();

            // Le damos un respiro al sistema para que procese el guardado
            System.Threading.Thread.Sleep(2000);
        }

        [When(@"se hace clic en registrar reparación")]
        public void WhenSeHaceClicEnRegistrarReparacion()
        {
            vehiculoPage.ClicCambiarAOperativo();
        }


        [Then(@"no deben mostrarse ni habilitarse los botones de flujo de mantenimiento")]
        public void ThenNoDebenMostrarseBotonesMantenimiento()
        {
            bool botonesVisibles = vehiculoPage.ExistenBotonesDeMantenimiento();

            // Si encuentra los botones, lanza la alerta roja de QA
            Assert.IsFalse(botonesVisibles, "[BUG DETECTADO]: Los botones 'Reportar Avería' o 'Registrar Reparación' están visibles para un vehículo DE BAJA. La vista debería ser solo informativa.");

            Console.WriteLine("Validación exitosa: La vista es puramente informativa y el flujo de mantenimiento está bloqueado.");
        }

        [Then(@"no debe mostrarse el botón Registrar reparación y solo debe mostrarse Reportar avería")]
        public void ThenNoDebeMostrarseBotonReparacion()
        {
            bool existeReparacion = vehiculoPage.ExisteBotonRegistrarReparacion();
            bool existeAveria = vehiculoPage.ExisteBotonReportarAveria();

            // 1. Validar que NO se pueda reparar un vehículo operativo
            Assert.IsFalse(existeReparacion, "[BUG DE LÓGICA]: El botón 'Registrar Reparación' está visible en un vehículo que ya está en estado OPERATIVO.");

            // 2. Validar que SÍ se pueda averiar (opción lógica correcta)
            Assert.IsTrue(existeAveria, "[BUG DE LÓGICA]: El botón 'Reportar Avería' NO está visible, y es la única opción válida para un vehículo OPERATIVO.");

            Console.WriteLine("Validación QA exitosa: El sistema respeta la lógica de estados para vehículos operativos.");
        }

        [Then(@"no debe mostrarse el botón Reportar avería y solo debe mostrarse Registrar reparación")]
        public void ThenNoDebeMostrarseBotonAveria()
        {
            // Usamos los escáneres que ya programamos en el Page
            bool existeReparacion = vehiculoPage.ExisteBotonRegistrarReparacion();
            bool existeAveria = vehiculoPage.ExisteBotonReportarAveria();

            // 1. Validar que NO se pueda averiar un vehículo que ya está averiado
            Assert.IsFalse(existeAveria, "[BUG DE LÓGICA]: El botón 'Reportar Avería' está visible en un vehículo que ya está en estado AVERIADO.");

            // 2. Validar que SÍ se pueda reparar
            Assert.IsTrue(existeReparacion, "[BUG DE LÓGICA]: El botón 'Registrar Reparación' NO está visible, y es la única opción válida para un vehículo AVERIADO.");

            Console.WriteLine("Validación QA exitosa: El sistema respeta la lógica ocultando el botón de avería para vehículos ya averiados.");
        }

        //FILTROS 

        [When(@"se desmarcan los filtros adicionales dejando solo el estado de vehículo OPERATIVO")]
        public void WhenSeDesmarcanLosFiltrosAdicionales()
        {
            vehiculoPage.DesmarcarFiltrosParaDejarSoloOperativo();
        }

        [When(@"se hace clic en el botón Buscar de la sección de filtros")]
        public void WhenSeHaceClicEnElBotonBuscarFiltros()
        {
            vehiculoPage.ClicBotonBuscarFiltros();
        }

        [Then(@"la grilla debe mostrar unicamente vehiculos con estado ""(.*)""")]
        public void ThenLaGrillaDebeMostrarUnicamenteVehiculosConEstado(string estadoEsperado)
        {
            bool filtradoCorrecto = vehiculoPage.ValidarColumnaEstadoVehiculo(estadoEsperado);

            Assert.IsTrue(filtradoCorrecto, $"[BUG DETECTADO]: El filtro no está funcionando correctamente. Aparecen registros en la tabla que no coinciden con el estado '{estadoEsperado}'.");

            Console.WriteLine("Validación QA exitosa: El filtro de la grilla funciona a la perfección.");
        }



        [When(@"se desmarcan los filtros adicionales dejando solo el estado de vehículo AVERIADO")]
        public void WhenSeDesmarcanFiltrosDejandoSoloAveriado()
        {
            vehiculoPage.DesmarcarFiltrosParaDejarSoloAveriado();
        }

        [When(@"se configuran los filtros dejando solo el estado de registro ACTIVO")]
        public void WhenSeConfiguranFiltrosSoloRegistroActivo()
        {
            vehiculoPage.DesmarcarFiltrosParaDejarSoloRegistroActivo();
        }

        [When(@"se configuran los filtros dejando solo el estado de registro DE BAJA")]
        public void WhenSeConfiguranFiltrosSoloRegistroDeBaja()
        {
            vehiculoPage.DesmarcarFiltrosParaDejarSoloRegistroDeBaja();
        }

        [Then(@"la grilla debe mostrar unicamente vehiculos con estado de registro ""(.*)""")]
        public void ThenLaGrillaDebeMostrarUnicamenteVehiculosConEstadoRegistro(string estadoEsperado)
        {
            bool filtradoCorrecto = vehiculoPage.ValidarColumnaEstadoRegistro(estadoEsperado);
            Assert.IsTrue(filtradoCorrecto, $"[BUG DETECTADO]: El filtro falló. Aparecen registros en la tabla que no son {estadoEsperado}.");
        }

        [When(@"se hace clic en el botón Exportar a Excel")]
        public void WhenSeHaceClicEnElBotonExportarAExcel()
        {
            vehiculoPage.ClicExportarExcel();
        }

        [Then(@"el sistema debe descargar un archivo Excel exitosamente")]
        public void ThenElSistemaDebeDescargarUnArchivoExcel()
        {
            bool archivoExiste = vehiculoPage.ValidarArchivoDescargado();

            Assert.IsTrue(archivoExiste, "[BUG DETECTADO]: El botón Exportar fue presionado pero no se detectó ningún archivo Excel nuevo en la carpeta de Descargas tras 10 segundos.");
        }






    }





    





    }

