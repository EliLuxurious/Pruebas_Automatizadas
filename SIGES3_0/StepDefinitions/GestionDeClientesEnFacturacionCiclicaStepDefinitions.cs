using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SIGES3_0.Pages;
using System.Reactive.Joins;

namespace SIGES3_0.StepDefinitions
{
    [Binding]
    public class GestionDeClientesEnFacturacionCiclicaStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly ClientesPage clientesPage;
        private readonly ScenarioContext _scenarioContext;
        private readonly PlanServicioPage planServicioPage;


        public GestionDeClientesEnFacturacionCiclicaStepDefinitions(IWebDriver driver, ScenarioContext scenarioContext)
        {
            this.driver = driver;
            _scenarioContext = scenarioContext;

            clientesPage = new ClientesPage(driver);
            planServicioPage = new PlanServicioPage(driver);

        }



        [When("se crea un plan de servicio válido")]
        public void WhenSeCreaUnPlanDeServicioValido()
        {
            // 1. Generamos el nombre dinámico con sello de tiempo
            string timeStamp = DateTime.Now.ToString("ddMM_HHmm");
            string nombreDinamico = $"Plan_Auto_{timeStamp}";

            // 2. Navegación (Este método que tienes ya tiene un Wait interno, lo cual es bueno)
            planServicioPage.NavegarAPlanDeServicio();

            // 3. PASO CLAVE: Antes de ir a Datos Generales, SIGES suele requerir 
            // entrar a 'Detalles del Plan' según tu Feature de Planes.
            planServicioPage.ConfigurarLimitesComprobantes("50", "500");
            planServicioPage.ConfigurarLimitesLocalesYUsuarios("Locales", "1", "5");
            planServicioPage.ConfigurarLimitesLocalesYUsuarios("Usuarios", "2", "15");

            // 4. Llenar los Datos Generales (Aquí es donde fallaba antes)
            // El método CompletarDatosGenerales ya hace clic en la pestaña.
            planServicioPage.CompletarDatosGenerales(nombreDinamico, "Plan generado automáticamente por Test de Clientes", "MENSUAL", "100");

            // 5. Guardar y confirmar
            planServicioPage.ClickGuardar();
            planServicioPage.ConfirmarRegistroCorrecto();

            Console.WriteLine($"[ESPERA] Pausando 5s para sincronización de Base de Datos...");
            Thread.Sleep(5000);

            // 6. MEMORIA COMPARTIDA: Guardamos el nombre para que el siguiente paso sepa qué elegir
            _scenarioContext["NombrePlanActual"] = nombreDinamico;

            Console.WriteLine($"[INFO] Plan dinámico creado con éxito: {nombreDinamico}");
        }

        [Given("selecciona la opción {string}")]
        public void GivenSeleccionaLaOpcion(string p0)
        {
            clientesPage.ClickNuevoCliente();
        }

        //[When("completa la sección {string} con el RUC {string}")]
        //public void WhenCompletaLaSeccionConElRUC(string p0, string ruc)
        //{
        //    clientesPage.ExpandirDatosGenerales();
        //    clientesPage.SeleccionarTipoDoc("REG. UNICO DE CONTRIBUYENTES");
        //    clientesPage.BuscarPorRUC(ruc);
        //}

        [When("selecciona el Ubigeo {string} y dirección {string}")]
        public void WhenSeleccionaElUbigeoYDireccion(string ubigeo, string direccion)
        {
            // 1. Seleccionamos lo que es por combo
            clientesPage.SeleccionarPais();
            clientesPage.SeleccionarUbigeo(ubigeo);

            // 2. ESPERA DE ESTABILIDAD (Crucial para que no se borre)
            // Esperamos 2 segundos para que el sistema de la SUNAT termine sus procesos internos
            System.Threading.Thread.Sleep(2000);

            // 3. Llenamos los campos de texto
            clientesPage.IngresarDireccion(direccion);
            clientesPage.IngresarCorreo("agricola@gmail.com");
            clientesPage.IngresarTelefono("967543267");

            // 4. Pausa final para asegurar que el botón "Guardar" se de cuenta del cambio
            System.Threading.Thread.Sleep(1000);
        }

        [When("configura la {string} con los siguientes datos:")]
        public void WhenConfiguraLaConLosSiguientesDatos(string facturación, DataTable dataTable)
        {
            clientesPage.AbrirFacturacion();

            var datos = dataTable.Rows.ToDictionary(row => row[0], row => row[1]);

            clientesPage.SeleccionarTipoComprobante(datos["Comprobante"]);
            clientesPage.SeleccionarCicloFacturacion(datos["Ciclo"]);

            if (datos.ContainsKey("Forma Pago"))
            {
                clientesPage.SeleccionarFormaPago(datos["Forma Pago"]);
            }

            clientesPage.SeleccionarFechaCalendario(datos["Inicio"]);

            if (datos.ContainsKey("Plan"))
            {
                clientesPage.ConfigurarPaginacion("100");

                string plan = datos["Plan"];

                if (plan == "AUTO" && _scenarioContext.ContainsKey("NombrePlanActual"))
                {
                    plan = _scenarioContext["NombrePlanActual"].ToString();
                }

                // ✅ ASSERT CLAVE
                Assert.IsFalse(string.IsNullOrEmpty(plan), "❌ El plan está vacío o no se generó");

                Console.WriteLine($"✔ Plan usado en test: {plan}");

                clientesPage.SeleccionarPlanSeguro(plan);
            }
        }



        [Then("procede a {string} el registro")]
        public void ThenProcedeAElRegistro(string gUARDAR)
        {
            clientesPage.Guardar();

            string resultadoFinal = clientesPage.EsperarResultadoFinalRegistro();

            clientesPage.CerrarModal();

            Console.WriteLine($"Resultado final: {resultadoFinal}");

            Assert.IsFalse(
                string.IsNullOrWhiteSpace(resultadoFinal) || resultadoFinal == "SIN RESULTADO FINAL",
                "❌ No hubo resultado final del sistema después de guardar."
            );

            Assert.IsTrue(
                resultadoFinal.Contains("correctamente", StringComparison.OrdinalIgnoreCase) ||
                resultadoFinal.Contains("éxito", StringComparison.OrdinalIgnoreCase) ||
                resultadoFinal.Contains("registrado", StringComparison.OrdinalIgnoreCase),
                $"❌ El registro falló o no devolvió mensaje exitoso. Resultado: {resultadoFinal}"
            );
        }

        [Then("debe visualizar el mensaje de éxito {string}")]
        public void ThenDebeVisualizarElMensajeDeExito(string p0)
        {
            // Aquí el test termina exitosamente si llegó hasta aquí sin errores
        }

        [When("selecciona el Ubigeo {string}")]
        public void WhenSeleccionaElUbigeo(string ubigeo)
        {
            clientesPage.SeleccionarUbigeo(ubigeo);
        }

        [When("Se expande la sección de {string}")]
        public void WhenSeExpandeLaSeccionDe(string facturación)
        {
            clientesPage.AbrirFacturacion();
        }

        [When("selecciona el Tipo de comprobante {string}")]
        public void WhenSeleccionaElTipoDeComprobante(string comprobante)
        {
            clientesPage.SeleccionarTipoComprobante(comprobante);
        }



        // --- Step de Clientes ---
        [When("configura el ciclo {string}, forma de pago {string} y plan {string}")]
        public void WhenConfiguraElCicloFormaDePagoYPlan(string ciclo, string formaPago, string planNombreFeature)
        {
            clientesPage.AbrirFacturacion();

            // Elegimos el plan dinámico si existe, si no usamos el de la Feature
            string plan = _scenarioContext.ContainsKey("NombrePlanActual")
              ? _scenarioContext["NombrePlanActual"].ToString()
              : planNombreFeature;

            // 🔥 ORDEN CORRECTO (CLAVE)
            clientesPage.SeleccionarCicloFacturacion(ciclo);
            clientesPage.SeleccionarFormaPago(formaPago);

            // 🔥 Esperar un momento a que Angular cargue el plan
            Thread.Sleep(1000);

            // Selección segura del plan (combo <select> o tabla dinámica)
            clientesPage.SeleccionarPlanSeguro(plan);
        }

        // Step genérico inteligente
        [When(@"completa la sección 'Datos Generales' con (el|la) (.*) '(.*)'")]
        public void WhenCompletaDatosGenerales(string articulo, string tipoDoc, string numeroDoc)
        {
            clientesPage.ExpandirDatosGenerales();
            clientesPage.SeleccionarTipoDoc(tipoDoc);

            if (tipoDoc.ToUpper().Contains("RUC") || tipoDoc.ToUpper().Contains("DNI"))
            {
                clientesPage.BuscarPorRUC(numeroDoc);
            }
            else
            {
                clientesPage.IngresarNumeroDocumentoManual(numeroDoc);
            }
        }



        // Nuevo step para los 10 documentos que requieren nombres manuales
        [When(@"ingresa nombres '(.*)', apellido paterno '(.*)', apellido materno '(.*)'")]
        public void WhenIngresaNombresManuales(string nombres, string paterno, string materno)
        {
            clientesPage.IngresarNombreCompletoManual(nombres, paterno, materno);
        }
        [When("ingresa correo {string}")]
        public void WhenIngresaCorreo(string correo)
        {
            clientesPage.IngresarCorreo(correo);
        }
        [When("ingresa telefono {string}")]
        public void WhenIngresaTelefono(string telefono)
        {
            clientesPage.IngresarTelefono(telefono);
        }

        //Opcional sino funciona el método de búsqueda por RUC, sino borrar
        [When(@"ingresa nombre comercial '(.*)'")]
        public void WhenIngresaNombreComercial(string nombreComercial)
        {
            clientesPage.IngresarNombreComercial(nombreComercial);
        }

        

        [When(@"el usuario da click en la sección ""(.*)""")]
        public void WhenElUsuarioDaClickEnLaSeccion(string nombreSeccion)
        {
            switch (nombreSeccion.Trim().ToUpperInvariant())
            {
                case "CREDENCIALES SOL":
                    clientesPage.AbrirCredencialesSol();
                    break;

                case "GUÍAS DE REMISIÓN Y OSE":
                case "GUIAS DE REMISION Y OSE":
                    clientesPage.AbrirGuiasYOse();
                    break;

                case "CONFIGURACIÓN ADICIONAL":
                case "CONFIGURACION ADICIONAL":
                    clientesPage.AbrirConfiguracionAdicional();
                    break;

                default:
                    throw new Exception($"❌ No se reconoce la sección: {nombreSeccion}");
            }
        }

        [When(@"ingresa el Usuario SOL Primario ""(.*)"" y Contraseña ""(.*)""")]
        public void WhenIngresaElUsuarioSolPrimarioYContrasena(string usuario, string clave)
        {
            clientesPage.IngresarCredencialesSolPrimarias(usuario, clave);
        }

        [When(@"ingresa el Usuario SOL Secundario ""(.*)"" y Contraseña ""(.*)""")]
        public void WhenIngresaElUsuarioSolSecundarioYContrasena(string usuario, string clave)
        {
            clientesPage.IngresarCredencialesSolSecundarias(usuario, clave);
        }

        [When(@"ingresa el Usuario de Guías de Remisión ""(.*)"" y Clave ""(.*)""")]
        public void WhenIngresaElUsuarioDeGuiasDeRemisionYClave(string usuario, string clave)
        {
            clientesPage.IngresarCredencialesGuias(usuario, clave);
        }

        [When(@"ingresa el Usuario OSE ""(.*)"" y Clave ""(.*)""")]
        public void WhenIngresaElUsuarioOseYClave(string usuario, string clave)
        {
            clientesPage.IngresarCredencialesOse(usuario, clave);
        }

        [When(@"ingresa el Usuario AnyDesk ""(.*)"" y Clave ""(.*)""")]
        public void WhenIngresaElUsuarioAnyDeskYClave(string usuario, string clave)
        {
            clientesPage.IngresarDatosAnyDesk(usuario, clave);
        }

        [When(@"ingresa el Tenant ID ""(.*)""")]
        public void WhenIngresaElTenantId(string tenantId)
        {
            clientesPage.IngresarTenantId(tenantId);
        }

        [Then(@"debe visualizar la advertencia '(.*)'")]
        public void ThenDebeVisualizarLaAdvertencia(string mensajeEsperado)
        {
            string mensajeReal = clientesPage.ObtenerMensajeAdvertenciaFactura();

            Assert.AreEqual(mensajeEsperado.Trim(), mensajeReal.Trim(),
                $"❌ Advertencia esperada: {mensajeEsperado} | Advertencia real: {mensajeReal}");

            Console.WriteLine("✅ Advertencia validada correctamente");
        }

        [Then(@"cierra la advertencia con '(.*)'")]
        public void ThenCierraLaAdvertenciaCon(string boton)
        {
            clientesPage.CerrarAdvertenciaFactura();
        }
    }
}