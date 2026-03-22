using SIGES3_0.Pages.Helpers;
using OpenQA.Selenium;

namespace SIGES3_0.Pages.VentasPage
{
    public class ReportesPage
    {
        private readonly Utilities utilities;

        public ReportesPage(IWebDriver driver)
        {
            utilities = new Utilities(driver);
        }

        public void OpenReports()
        {
            utilities.ClickButton(VentasLocators.Reports.PurchaseMenu);
            utilities.ClickButton(VentasLocators.Reports.PurchaseReports);
        }

        public void ConfigureReportByType(string option, string fromDate, string toDate)
        {
            utilities.ClearAndEnterText(VentasLocators.Reports.TypeFromDate, fromDate);
            utilities.ClearAndEnterText(VentasLocators.Reports.TypeToDate, toDate);

            switch (option.Trim().ToUpperInvariant())
            {
                case "TODOS":
                    utilities.ClickButton(VentasLocators.Reports.AllProofs);
                    break;

                case "TRIBUTABLES":
                    utilities.ClickButton(VentasLocators.Reports.TaxedProofs);
                    break;

                case "NO TRIBUTABLES":
                    utilities.ClickButton(VentasLocators.Reports.NoTaxedProofs);
                    break;

                default:
                    throw new ArgumentException($"El filtro por tipo '{option}' no esta soportado.");
            }
        }

        public void ConfigureReport(string reportType, string fromDate, string toDate)
        {
            switch (reportType.Trim().ToUpperInvariant())
            {
                case "COMPROBANTE":
                    utilities.ClearAndEnterText(VentasLocators.Reports.ProofFromDate, fromDate);
                    utilities.ClearAndEnterText(VentasLocators.Reports.ProofToDate, toDate);
                    break;

                case "CONCEPTO":
                    utilities.ClearAndEnterText(VentasLocators.Reports.ConceptFromDate, fromDate);
                    utilities.ClearAndEnterText(VentasLocators.Reports.ConceptToDate, toDate);
                    break;

                default:
                    throw new ArgumentException($"El tipo de reporte '{reportType}' no esta soportado.");
            }
        }

        public void Generate(string reportType)
        {
            switch (reportType.Trim().ToUpperInvariant())
            {
                case "TIPO":
                    utilities.ClickButton(VentasLocators.Reports.ReportByType);
                    break;

                case "COMPROBANTE":
                    utilities.ClickButton(VentasLocators.Reports.ReportByProof);
                    break;

                case "CONCEPTO":
                    utilities.ClickButton(VentasLocators.Reports.ReportByConcept);
                    break;

                default:
                    throw new ArgumentException($"No se puede generar el reporte '{reportType}'.");
            }
        }
    }
}
