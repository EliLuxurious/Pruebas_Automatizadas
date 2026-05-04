using SIGES3_0.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SIGES3_0.Pages.VentasPage
{
    public class VerVentasPage
    {
        private readonly IWebDriver driver;
        private readonly Utilities utilities;

        public VerVentasPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        public void SetInitialDate(string value)
        {
            utilities.ClearAndEnterText(SalesLocators.ViewSales.InitialDate, value);
        }

        public void SetFinalDate(string value)
        {
            utilities.ClearAndEnterText(SalesLocators.ViewSales.FinalDate, value);
        }

        public void QuerySales()
        {
            utilities.ClickButton(SalesLocators.ViewSales.QueryButton);
        }

        public void SearchSale(string value)
        {
            utilities.ClearAndEnterText(SalesLocators.ViewSales.SearchSale, value);
        }

        public void ActivateRedeem()
        {
            utilities.ClickButton(SalesLocators.ViewSales.ActivateRedeem);
        }

        public void SelectFirstSale()
        {
            utilities.ClickButton(SalesLocators.ViewSales.FirstRowCheck);
        }

        public void ClickRedeem()
        {
            utilities.ClickButton(SalesLocators.ViewSales.RedeemButton);
        }

        public void SetVoucherType(string option)
        {
            var select = new SelectElement(utilities.WaitUntilVisible(SalesLocators.ViewSales.RedeemVoucherType));
            select.SelectByText(option);
        }

        public void AcceptRedeem()
        {
            utilities.ClickButton(SalesLocators.ViewSales.AcceptRedeemButton);
        }

        public void OpenSale()
        {
            utilities.ClickButton(SalesLocators.ViewSales.ViewSaleButton);
        }

        public void ChooseNoteType(string option)
        {
            switch (option.Trim().ToUpperInvariant())
            {
                case "DEBITO":
                case "DÉBITO":
                    utilities.ClickButton(SalesLocators.ViewSales.DebitNoteButton);
                    break;

                case "CREDITO":
                case "CRÉDITO":
                    utilities.ClickButton(SalesLocators.ViewSales.CreditNoteButton);
                    break;

                default:
                    throw new ArgumentException($"El tipo de nota '{option}' no esta soportado.");
            }
        }

        public void SelectNoteCategory(string option)
        {
            var select = new SelectElement(utilities.WaitUntilVisible(SalesLocators.ViewSales.NoteTypeSelect));
            select.SelectByText(option);
        }

        public void SelectNoteDocument(string option)
        {
            var select = new SelectElement(utilities.WaitUntilVisible(SalesLocators.ViewSales.NoteDocumentSelect));
            select.SelectByText(option);
        }

        public void EnterReason(string value)
        {
            utilities.ClearAndEnterText(SalesLocators.ViewSales.NoteReason, value);
        }

        public void EnterNoteAmount(string value)
        {
            utilities.ClearAndEnterText(SalesLocators.ViewSales.NoteAmount, value);
        }

        public void EnterRowAmount(string value)
        {
            utilities.ClearAndEnterText(SalesLocators.ViewSales.NoteRowAmount, value);
        }

        public void EnterQuantity(string value)
        {
            utilities.ClearAndEnterText(SalesLocators.ViewSales.NoteQuantity, value);
        }

        public void SelectCreditDelivery(string option)
        {
            if (option.Trim().Equals("INMEDIATA", StringComparison.OrdinalIgnoreCase))
            {
                utilities.ClickButton(SalesLocators.ViewSales.NoteImmediate);
                return;
            }

            utilities.ClickButton(SalesLocators.ViewSales.NoteDeferred);
        }

        public void SaveNote()
        {
            utilities.ClickButton(SalesLocators.ViewSales.SaveNote);
        }

        public void InvalidateDocument()
        {
            utilities.ClickButton(SalesLocators.ViewSales.InvalidateButton);
        }

        public void EnterObservation(string value)
        {
            utilities.ClearAndEnterText(SalesLocators.ViewSales.Observation, value);
        }

        public void AcceptInvalidation()
        {
            utilities.ClickButton(SalesLocators.ViewSales.AcceptInvalidation);
        }

        public void CloneSale()
        {
            utilities.ClickButton(SalesLocators.ViewSales.CloneButton);
        }

        public void PrintDocument()
        {
            utilities.ClickButton(SalesLocators.ViewSales.PrintButton);
        }

        public void DownloadDocument(string option)
        {
            switch (option.Trim().ToUpperInvariant())
            {
                case "PDF":
                    utilities.ClickButton(SalesLocators.ViewSales.PdfButton);
                    break;

                case "XML":
                    utilities.ClickButton(SalesLocators.ViewSales.DownloadDropdown);
                    utilities.ClickButton(SalesLocators.ViewSales.XmlButton);
                    break;

                case "ZIP":
                    utilities.ClickButton(SalesLocators.ViewSales.DownloadDropdown);
                    utilities.ClickButton(SalesLocators.ViewSales.ZipButton);
                    break;

                default:
                    throw new ArgumentException($"El tipo de descarga '{option}' no esta soportado.");
            }
        }

        public void OpenSendModal()
        {
            utilities.ClickButton(SalesLocators.ViewSales.SendButton);
        }

        public void EnterEmail(string value)
        {
            utilities.ClearAndEnterText(SalesLocators.ViewSales.EmailInput, value);
        }

        public void AddEmail()
        {
            utilities.ClickButton(SalesLocators.ViewSales.AddEmail);
        }

        public void SendMail()
        {
            utilities.ClickButton(SalesLocators.ViewSales.SendMail);
        }
    }
}
