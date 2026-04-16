using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.Items.NewItem;

namespace SIGES3_0.StepDefinitions.Items.NewItems
{
    [Binding]
    public class NewItemStepDefinitions
    {
        private IWebDriver driver;
        NewItemsPage conceptosPage;

        public NewItemStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            conceptosPage = new NewItemsPage(driver);
        }

        [When("el usuario accede al módulo Conceptos")]
        public void WhenElUsuarioAccedeAlModuloConceptos()
        {
            conceptosPage.AbrirModuloConceptos();
        }

        [When("el usuario selecciona Nuevo Concepto")]
        public void WhenElUsuarioSeleccionaNuevoConcepto()
        {
            conceptosPage.SeleccionarNuevoConcepto();
        }

        [When("el usuario selecciona la Familia {string}")]
        public void WhenElUsuarioSeleccionaLaFamilia(string familia)
        {
            if (string.IsNullOrEmpty(familia)) return;

            conceptosPage.SeleccionarFamilia(familia);
        }

        [When("el usuario selecciona Auto al Código")]
        public void WhenElUsuarioSeleccionaAutoAlCodigo()
        {
            conceptosPage.SeleccionarAutoCodigoDeBarra();
        }

        [When("el usuario ingresa el Código {string}")]
        public void WhenElUsuarioIngresaElCodigo(string codigo)
        {
            if (string.IsNullOrEmpty(codigo)) return;

            conceptosPage.IngresarCodigoDeBarra(codigo);
        }

        [When("el usuario ingresa el Sufijo {string}")]
        public void WhenElUsuarioIngresaElSufijo(string sufijo)
        {
            if (string.IsNullOrEmpty(sufijo)) return;

            conceptosPage.AgregarSufijo(sufijo);
        }

        [When("el usuario selecciona la U.M.Comercial {string}")]
        public void WhenElUsuarioSeleccionaLaUMComercial(string umComercial)
        {
            if (string.IsNullOrEmpty(umComercial)) return;

            conceptosPage.SeleccionarUMComercial(umComercial);
        }

        [When("el usuario selecciona la U.Medida {string}")]
        public void WhenElUsuarioSeleccionaLaUMedida(string umMedida)
        {
            if (string.IsNullOrEmpty(umMedida)) return;

            conceptosPage.SeleccionarUMedida(umMedida);
        }

        [When("el usuario selecciona el Rol {string}")]
        public void WhenElUsuarioSeleccionaElRol(string rol)
        {
            if (string.IsNullOrEmpty(rol))
            {
                return;
            }

            if (rol.ToUpper() == "VACIO")
            {
                conceptosPage.EliminarRolPredefinido();
            }
            else
            {
                conceptosPage.SeleccionarRol(rol);
            }
        }

        [When("el usuario selecciona el Módulo a Mostrar {string}")]
        public void WhenElUsuarioSeleccionaElModuloAMostrar(string modulo)
        {
            if (string.IsNullOrEmpty(modulo))
            {
                return;
            }

            if (modulo.ToUpper() == "VACIO")
            {
                conceptosPage.EliminarModuloPredefinido();
            }
            else
            {
                conceptosPage.SeleccionarModulo(modulo);
            }
        }

        [When("el usuario selecciona la Marca {string}")]
        public void WhenElUsuarioSeleccionaLaMarca(string marca)
        {
            if (string.IsNullOrEmpty(marca)) return;

            conceptosPage.SeleccionarMarca(marca);
        }

        [When("el usuario selecciona la Presentación {string}")]
        public void WhenElUsuarioSeleccionaLaPresentacion(string presentacion)
        {
            if (string.IsNullOrEmpty(presentacion)) return;

            conceptosPage.SeleccionarPresentacion(presentacion);
        }

        [When("el usuario ingresa la Cantidad {string}")]
        public void WhenElUsuarioIngresaLaCantidad(string cantidad)
        {
            if (string.IsNullOrEmpty(cantidad)) return;

            conceptosPage.IngresarCantidad(cantidad);
        }

        [When("el usuario selecciona la Unidad de Medida {string}")]
        public void WhenElUsuarioSeleccionaLaUnidadDeMedida(string unidadMedida)
        {
            if (string.IsNullOrEmpty(unidadMedida)) return;

            conceptosPage.SeleccionarUnidadMedida(unidadMedida);
        }

        [When("el usuario selecciona la tarifa {string}")]
        public void WhenElUsuarioSeleccionaLaTarifa(string tarifa)
        {
            if (string.IsNullOrEmpty(tarifa)) return;

            conceptosPage.SeleccionarTarifa(tarifa);
        }

        [When("el usuario ingresa el Precio {string}")]
        public void WhenElUsuarioIngresaElPrecio(string precio)
        {
            if (string.IsNullOrEmpty(precio)) return;

            conceptosPage.IngresarPrecio(precio);
        }

        [Then("Guardar concepto")]
        public void ThenGuardarConcepto()
        {
            conceptosPage.GuardarConcepto();
        }

        [Then("No se guarda concepto")]
        public void ThenNoSeGuardaConcepto()
        {
            conceptosPage.NoGuardarConcepto();
        }
    }
}
