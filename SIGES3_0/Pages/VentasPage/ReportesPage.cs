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
            utilities.ClickButton(SalesLocators.Reports.PurchaseMenu);
            utilities.ClickButton(SalesLocators.Reports.PurchaseReports);
        }

        public void ConfigureReportByType(string option, string fromDate, string toDate)
        {
            utilities.ClearAndEnterText(SalesLocators.Reports.TypeFromDate, fromDate);
            utilities.ClearAndEnterText(SalesLocators.Reports.TypeToDate, toDate);

            switch (option.Trim().ToUpperInvariant())
            {
                case "TODOS":
                    utilities.ClickButton(SalesLocators.Reports.AllProofs);
                    break;

                case "TRIBUTABLES":
                    utilities.ClickButton(SalesLocators.Reports.TaxedProofs);
                    break;

                case "NO TRIBUTABLES":
                    utilities.ClickButton(SalesLocators.Reports.NoTaxedProofs);
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
                    utilities.ClearAndEnterText(SalesLocators.Reports.ProofFromDate, fromDate);
                    utilities.ClearAndEnterText(SalesLocators.Reports.ProofToDate, toDate);
                    break;

                case "CONCEPTO":
                    utilities.ClearAndEnterText(SalesLocators.Reports.ConceptFromDate, fromDate);
                    utilities.ClearAndEnterText(SalesLocators.Reports.ConceptToDate, toDate);
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
                    utilities.ClickButton(SalesLocators.Reports.ReportByType);
                    break;

                case "COMPROBANTE":
                    utilities.ClickButton(SalesLocators.Reports.ReportByProof);
                    break;

                case "CONCEPTO":
                    utilities.ClickButton(SalesLocators.Reports.ReportByConcept);
                    break;

                default:
                    throw new ArgumentException($"No se puede generar el reporte '{reportType}'.");
            }
        }
    }
}
