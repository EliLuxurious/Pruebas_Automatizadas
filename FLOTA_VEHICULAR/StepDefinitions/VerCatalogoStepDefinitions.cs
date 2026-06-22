using FLOTA_VEHICULAR.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace FLOTA_VEHICULAR.StepDefinitions
{
    [Binding]
    public class VerCatalogoStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly VerCatalogoPage verCatalogoPage;

        public VerCatalogoStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            verCatalogoPage = new VerCatalogoPage(driver);
        }

        // ===============================
        // NAVEGACIÓN AL CATÁLOGO
        // ===============================

        [When(@"Se ingresa al módulo Mantenimiento y submódulo Ver Catálogos")]
        public void WhenSeIngresaAlModuloMantenimientoYSubmoduloVerCatalogos()
        {
            verCatalogoPage.IngresarSubmoduloVerCatalogo();
        }

        [When(@"Se hace clic en el botón Nuevo Catálogo")]
        public void WhenSeHaceClicEnElBotonNuevoCatalogo()
        {
            verCatalogoPage.ClicNuevoCatalogo();
        }

        [When(@"Se ingresan los datos del nuevo catálogo:")]
        public void WhenSeIngresanLosDatosDelNuevoCatalogo(DataTable table)
        {
            // Variables para guardar las fechas y enviarlas juntas
            string fInicio = "";
            string fFin = "";

            foreach (var row in table.Rows)
            {
                string campo = row["Campo"].Trim().ToUpper();
                string valor = row["Valor"].Trim();

                switch (campo)
                {
                    case "CLASIFICADOR":
                    case "CLASE DE MANTENIMIENTO":
                        verCatalogoPage.SeleccionarRadioButton(valor);
                        break;

                    case "TIPO DE MOTOR":
                        verCatalogoPage.SeleccionarDeLista("TIPO DE MOTOR", valor);
                        break;

                    case "FECHA DE INICIO":
                        fInicio = valor;
                        break;

                    case "FECHA FIN":
                        fFin = valor;
                        break;

                    case "ACTIVIDADES":
                        verCatalogoPage.SeleccionarDeLista("ACTIVIDAD", valor);
                        break;
                }
            }

            // Ingresamos las fechas si fueron enviadas en la tabla
            if (!string.IsNullOrEmpty(fInicio) && !string.IsNullOrEmpty(fFin))
            {
                verCatalogoPage.IngresarFechas(fInicio, fFin);
            }
        }

        [When(@"Se procede a Guardar el catálogo")]
        [Then(@"Se procede a Guardar el catálogo")]
        public void ThenSeProcedeAGuardarElCatalogo()
        {
            verCatalogoPage.GuardarCatalogo();
            Console.WriteLine("El catálogo fue guardado exitosamente.");
        }

        //

        // ¡Mira cómo apilamos los Then! Ahora este mismo código responderá a ambas frases en cualquier Feature
        [Then(@"el sistema debe impedir el registro y mostrar un error de fechas solapadas")]
        [Then(@"el sistema debe mostrar un error de solapamiento de fechas")]
        public void ThenElSistemaDebeImpedirElRegistroYMostrarUnError()
        {
            // Llamamos a nuestro "cazador de alertas"
            bool hayError = verCatalogoPage.ValidarErrorFechasSolapadas();

            // Si no hay error (hayError es false), la prueba de QA falla porque el sistema permitió el solapamiento
            Assert.IsTrue(hayError, "[BUG DETECTADO]: El sistema guardó/clonó el catálogo permitiendo fechas inválidas o no mostró ninguna alerta visual de error.");

            Console.WriteLine("Validación QA exitosa: El sistema impidió correctamente la acción y mostró la alerta roja.");
        }


        [Then(@"el catálogo debe guardarse exitosamente sin errores de solapamiento")]
        public void ThenElCatalogoDebeGuardarseExitosamente()
        {
            // Reutilizamos nuestro "cazador de alertas"
            // Pero esta vez, si encuentra un error (true), la prueba debe FALLAR
            bool aparecioError = verCatalogoPage.ValidarErrorFechasSolapadas();

            // Assert.IsFalse espera que el resultado sea FALSE (que no haya error)
            Assert.IsFalse(aparecioError, "[BUG DETECTADO - CP029]: El sistema mostró un error de solapamiento para fechas que son correctas y no se cruzan.");

            Console.WriteLine("Validación QA exitosa: El catálogo se guardó sin bloqueos de fecha.");
        }


        [When(@"Se busca por fechas ""(.*)"" y ""(.*)"" y se da de baja el catálogo")]
        public void WhenSeBuscaPorFechasYSeDaDeBajaElCatalogo(string fInicio, string fFin)
        {
            verCatalogoPage.BuscarYDarDeBaja(fInicio, fFin);
            Console.WriteLine($"Se filtró la grilla, se ingresó al detalle de {fInicio} - {fFin} y se le dio de baja.");
        }





        [Then(@"el botón Guardar debe estar deshabilitado para impedir el registro")]
        public void ThenElBotonGuardarDebeEstarDeshabilitadoParaImpedirElRegistro()
        {
            bool botonBloqueado = verCatalogoPage.ValidarBotonGuardarDeshabilitado();

            // Si el botón NO está bloqueado (false), el Assert hace fallar la prueba
            Assert.IsTrue(botonBloqueado, "[BUG DETECTADO - CP026]: ¡Alerta! El botón 'Guardar' está habilitado a pesar de que los campos obligatorios están completamente vacíos.");

            Console.WriteLine("Validación QA exitosa: El sistema protegió el formulario y mantuvo el botón Guardar deshabilitado.");
        }



        [When(@"Se busca por fechas ""(.*)"" y ""(.*)"" y se abre el detalle")]
        public void WhenSeBuscaPorFechasYAbreElDetalle(string fInicio, string fFin)
        {
            verCatalogoPage.BuscarYAbrirDetalle(fInicio, fFin);
        }

        [When(@"Se edita el catálogo agregando la actividad ""(.*)"" y eliminando la fila (.*)")]
        public void WhenSeEditaElCatalogoAgregandoYEliminando(string nuevaActividad, int filaAEliminar)
        {
            // 1. Entramos en modo edición (Lápiz amarillo)
            verCatalogoPage.ClicEditarCatalogo();

            // 2. Reutilizamos el método de listas para agregar la nueva actividad
            verCatalogoPage.SeleccionarDeLista("ACTIVIDAD", nuevaActividad);

            // 3. Eliminamos la actividad de la fila indicada
            verCatalogoPage.EliminarActividadFila(filaAEliminar);
        }


        [Then(@"al buscar el catálogo nuevamente el estado debe figurar como ""(.*)""")]
        public void ThenElEstadoDebeFigurarComo(string estadoEsperado)
        {
            // Podrías necesitar un pequeño delay para que la BD procese el cambio
            System.Threading.Thread.Sleep(2000);

            string estadoActual = verCatalogoPage.ObtenerEstadoPrimerRegistro();

            Assert.AreEqual(estadoEsperado, estadoActual,
                $"[BUG DE TRANSICIÓN]: Se esperaba que el catálogo fuera {estadoEsperado} tras la edición, pero sigue como {estadoActual}.");

            Console.WriteLine($"Confirmado: El catálogo ahora está {estadoActual}.");
        }

        [When(@"Se busca el primer catálogo en estado ""(.*)"" y se edita")]
        public void WhenSeBuscaPrimerCatalogoEstadoYEdita(string estado)
        {
            verCatalogoPage.BuscarPrimerCatalogoPorEstadoYEditar(estado);
        }

        [When(@"Se actualizan las fechas del catálogo a inicio ""(.*)"" y fin ""(.*)""")]
        public void WhenSeActualizanLasFechasDelCatalogo(string fInicio, string fFin)
        {
            // ¡Reutilizamos tu método maestro de calendarios!
            verCatalogoPage.IngresarFechas(fInicio, fFin);
        }

        [Then(@"el estado del catálogo editado debe cambiar a ""(.*)""")]
        public void ThenElEstadoDelCatalogoEditadoDebeCambiarA(string estadoEsperado)
        {
            string estadoActual = verCatalogoPage.ObtenerEstadoPrimerRegistro();

            Assert.AreEqual(estadoEsperado.ToUpper(), estadoActual.ToUpper(),
                $"[BUG DETECTADO - CP039]: Se esperaba que el estado fuera {estadoEsperado}, pero se quedó como {estadoActual}.");

            Console.WriteLine($"Validación Exitosa: El catálogo cambió su estado a {estadoActual}.");
        }


        [Then(@"el sistema debe impedir asignar una fecha pasada bloqueando el botón guardar")]
        public void ThenElSistemaDebeImpedirAsignarFechaPasada()
        {
            // Reutilizamos el método del CP026 que verifica si el botón Guardar está deshabilitado
            bool estaBloqueado = verCatalogoPage.ValidarBotonGuardarDeshabilitado();

            // Si el botón está clickeable (false), entonces hay un bug
            Assert.IsTrue(estaBloqueado, "[BUG DETECTADO - CP038]: ¡Alerta! El sistema permite asignar una fecha pasada a un catálogo CADUCADO. El botón Guardar no se bloqueó.");

            Console.WriteLine("Validación Exitosa: El sistema bloqueó correctamente la asignación de fechas pasadas.");
        }



        [When(@"Se hace clic en el botón Clonar")]
        public void WhenSeHaceClicEnElBotonClonar()
        {
            verCatalogoPage.ClicBotonClonar();
        }
        [When(@"Se modifica la lista de actividades agregando ""(.*)"" y eliminando la fila (.*)")]
        public void WhenSeModificaLaListaDeActividades(string nuevaActividad, int filaAEliminar)
        {
            // Como ya estamos dentro del modal de Clonar, solo llamamos a los métodos directos
            verCatalogoPage.SeleccionarDeLista("ACTIVIDAD", nuevaActividad);
            verCatalogoPage.EliminarActividadFila(filaAEliminar);
        }


        [When(@"Se busca el primer catálogo en estado ""(.*)"" y se clona")]
        public void WhenSeBuscaElPrimerCatalogoEnEstadoYSeClona(string estado)
        {
            verCatalogoPage.BuscarPrimerCatalogoPorEstadoYClonar(estado);
        }


        [When(@"Se eliminan todas las actividades precargadas")]
        public void WhenSeEliminanTodasLasActividadesPrecargadas()
        {
            verCatalogoPage.EliminarTodasLasActividades();
            Console.WriteLine("Se vació la tabla eliminando todas las actividades heredadas en la clonación.");
        }

        [When(@"Se limpian los filtros y se selecciona el estado ""(.*)""")]
        public void WhenSeLimpianLosFiltrosYSeSeleccionaElEstado(string estadoBuscado)
        {
            verCatalogoPage.PrepararFiltrosYSeleccionarEstado(estadoBuscado);
            Console.WriteLine($"Se desmarcaron los filtros generales y se seleccionó únicamente el estado: {estadoBuscado}");
        }

        [When(@"Se hace clic en el botón BUSCAR principal")]
        public void WhenSeHaceClicEnElBotonBuscarPrincipal()
        {
            verCatalogoPage.ClicBotonBuscarPrincipal();
        }

        [Then(@"todos los resultados en la columna ""(.*)"" deben coincidir con ""(.*)""")]
        public void ThenTodosLosResultadosEnLaColumnaDebenCoincidirCon(string nombreColumna, string valorEsperado)
        {
            verCatalogoPage.ValidarColumnaGrilla(nombreColumna, valorEsperado);
            Console.WriteLine($"Validación QA exitosa: Todos los registros en '{nombreColumna}' muestran el valor '{valorEsperado}'.");
        }






















    }
















}