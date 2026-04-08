using OpenQA.Selenium;
using System;
using SigesCore.Hooks.LoginPage;
using SigesCore.Hooks.VentasPage;
using SigesCore.Hooks.ComprasPage;
using System.Net;
using static SigesCore.Hooks.ComprasPage.VerComprasPage;
using static SigesCore.Hooks.VentasPage.NuevaVentaPage;

namespace SigesCore.StepDefinitions.Compras
{
    [Binding]
    public class VerComprasStepDefinitions
    {
        private IWebDriver driver;
        LoginPage login;
        VerComprasPage verCompraspage;
        NuevaVentaPage newSale;

        public VerComprasStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            this.login = new LoginPage(driver);
            this.verCompraspage = new VerComprasPage(driver);
            this.newSale = new NuevaVentaPage(driver);
        }

        [When("Rango de busqueda {string} y {string}")]
        public void WhenSearchRange(string fromdate, string todate)
        {
            verCompraspage.SearchPurchases(fromdate, todate);
        }

        [When(@"Se realiza una busqueda con tipo {string} y {string}")]
        public void WhenTipoDeBusquedaGeneral(string optionSearch, string dataSearch)
        {
            verCompraspage.SearchFiltersGeneral(optionSearch, dataSearch);
        }

        [When(@"Se realiza una busqueda con tipo '([^']+)' y opcion '([^']+)' y criterio '([^']+)'")]
        public void WhenTipoDeBusquedaEspecifica(string optionSearch, string specificOptionSearch, string dataSearch)
        {
            Console.WriteLine($"Tipo: {optionSearch}, Opcion: {specificOptionSearch}, Criterio: {dataSearch}");
            verCompraspage.SearchFiltersOneField(optionSearch, specificOptionSearch, dataSearch);
        }

        [When("Busqueda especifica {string} y {string} y {string} y {string}")]
        public void WhenSearchSpecific(string optionsearch, string specificoptionsearch, string datasearch1, string datasearch2)
        {
            verCompraspage.SearchFiltersSpecific(optionsearch, specificoptionsearch, datasearch1, datasearch2);
        }

        [When("Exportar resultados")]
        public void WhenExportarResultados()
        {
            verCompraspage.ExportResults();
        }

        /*[When("Intento {string} y {string} y {string} y {string}")]
        public void WhenIntento(string supplier, string date, string comprobante, string serie)
        {
            verCompraspage.RegisterNewPurchaseFromViewPrchases(supplier, date, comprobante, serie);
        }*/

        [When("Proveedor {string} Fecha {string}")]
        public void WhenDatos(string supplier, string date)
        {
            verCompraspage.RegisterNewPurchaseFromViewPrchasesPandD(supplier, date);
        }

        [When("Tipo Doc. {string} Serie {string} Num. Doc.{string} Observacion {string}")]
        public void WhenIntento(string comprobante, string series, string numberdocument, string observation)
        {
            verCompraspage.RegisterNewPurchaseFromViewPrchasesDataDocs(comprobante, series, numberdocument, observation);
        }

        [When("Entrega {string} Cant. Almacenes {string} Rol {string} Almacen {string}")]
        public void WhenRolAlmacen(string optiondelivery, string optionwarehouse, string rol, string warehouse)
        {
            verCompraspage.RegisterNewPurchaseFromViewPrchasesEandA(optiondelivery, optionwarehouse, rol, warehouse);
        }

        [When("Tipo de compra {string}")]
        public void WhenTipoDeCompra(string optiontypepurchase)
        {
            verCompraspage.RegisterNewPurchaseFromViewPrchasesTypePurchase(optiontypepurchase);
        }

        [When("Concepto {string} Importe {string}")]
        public void WhenConcepto(string concept, string amount)
        {
            verCompraspage.RegisterNewPurchaseFromViewPrchasesConcept(concept, amount);
        }

        [When("Tipo de pago {string}")]
        public void WhenTipoDePagoMedioDePago(string optiontypepayment)
        {
            verCompraspage.SelectTypePayment(optiontypepayment);
        }

        [When("Medio de Pago {string} y {string}")]
        public void WhenMedioDePagoY(string optionmethodpayment, string information)
        {
            verCompraspage.SelecTMethodPayment(optionmethodpayment, information);
        }

        [When("Registrar nuevo usuario {string} y {string}")]
        public void WhenRegistrarNuevoConcepto(string dni, string sexo)
        {
            verCompraspage.RegisterNewUser(dni, sexo);
        }
        
        [When("Codigo {string} Sufijo {string} y {string} y {string} y {string} y {string} y {string}")]
        public void WhenCodigoSufijo(string code, string suffix, string family, string umc, string brand, string name, string quantity)
        {
            verCompraspage.RegisterNewConcept(code, suffix, family, umc, brand, name, quantity);
        }

        [When("Tarifa {string} Precio {string} y {string}")]
        public void WhenTarfiaPrecio(string option, string price, string option2)
        {
            verCompraspage.SupplementaryData(option, price, option2);
        }

        [When("Registrar nueva familia {string} y {string} y {string} y {string}")]
        public void WhenRegistrarNuevaFamilia(string option, string name, string category, string brand)
        {
            verCompraspage.RegisterNewFamily(option, name, category, brand);
        }


        /*[When("Prueba {string} y {string} y {string} y {string} y {string}")]
        public void WhenPrueba(string fromdate, string todate, string optionsearch, string specificoptionsearch, string datasearch)
        {
            verCompraspage.SearchPurchases(fromdate, todate, optionsearch, specificoptionsearch, datasearch);
        }*/

        [When("hola {string}")]
        public void WhenHola(string p0)
        {
            throw new PendingStepException();
        }

    }

}
