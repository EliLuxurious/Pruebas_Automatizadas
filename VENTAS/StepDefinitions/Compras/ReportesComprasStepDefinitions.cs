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
    public class VerReportesStepDefinitions
    {
        private IWebDriver driver;
        LoginPage login;
        VerReportesPage purchaseReports;
        NuevaVentaPage newSale;

        public VerReportesStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            this.login = new LoginPage(driver);
            this.purchaseReports = new VerReportesPage(driver);
            this.newSale = new NuevaVentaPage(driver);
        }

        [When("Ingresar a Compras - Reportes")]
        public void WhenIngresarACompras_Reportes()
        {
            purchaseReports.EnterPurchaseReports();
        }

        [When("Reporte por Tipo, Tipo de comprobante {string} Fecha Inicial {string} Fecha Final {string}")]
        public void WhenReportePorTipoTipoDeComprobanteFechaInicialFechaFinal(string option, string fromdate, string todate)
        {
            purchaseReports.ReportByType(option, fromdate, todate);
        }


        [When("Reporte por {string} Fecha Inicial {string} Fecha Final {string}")]
        public void WhenReportePorFechaInicialFechaFinal(string typereport, string fromdate, string todate)
        {
            purchaseReports.SelectPurchaseReports(typereport, fromdate, todate);
        }

        [Then("Generar reporte por {string}")]
        public void ThenRealizarReporte(string typereport)
        {
            purchaseReports.MakeReport(typereport);
        }



    }

}