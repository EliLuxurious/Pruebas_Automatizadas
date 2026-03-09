using FLOTA_VEHICULAR.Pages;
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

        // ===============================
        // GUARDAR
        // ===============================

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
    }
}