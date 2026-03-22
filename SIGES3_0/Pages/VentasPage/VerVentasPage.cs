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
            utilities.ClearAndEnterText(VentasLocators.ViewSales.InitialDate, value);
        }

        public void SetFinalDate(string value)
        {
            utilities.ClearAndEnterText(VentasLocators.ViewSales.FinalDate, value);
        }

        public void QuerySales()
        {
            utilities.ClickButton(VentasLocators.ViewSales.QueryButton);
        }

        public void SearchSale(string value)
        {
            utilities.ClearAndEnterText(VentasLocators.ViewSales.SearchSale, value);
        }

        public void ActivateRedeem()
        {
            utilities.ClickButton(VentasLocators.ViewSales.ActivateRedeem);
        }

        public void SelectFirstSale()
        {
            utilities.ClickButton(VentasLocators.ViewSales.FirstRowCheck);
        }

        public void ClickRedeem()
        {
            utilities.ClickButton(VentasLocators.ViewSales.RedeemButton);
        }

        public void SetVoucherType(string option)
        {
            var select = new SelectElement(utilities.WaitUntilVisible(VentasLocators.ViewSales.RedeemVoucherType));
            select.SelectByText(option);
        }

        public void AcceptRedeem()
        {
            utilities.ClickButton(VentasLocators.ViewSales.AcceptRedeemButton);
        }

        public void OpenSale()
        {
            utilities.ClickButton(VentasLocators.ViewSales.ViewSaleButton);
        }

        public void ChooseNoteType(string option)
        {
            switch (option.Trim().ToUpperInvariant())
            {
                case "DEBITO":
                case "DÉBITO":
                    utilities.ClickButton(VentasLocators.ViewSales.DebitNoteButton);
                    break;

                case "CREDITO":
                case "CRÉDITO":
                    utilities.ClickButton(VentasLocators.ViewSales.CreditNoteButton);
                    break;

                default:
                    throw new ArgumentException($"El tipo de nota '{option}' no esta soportado.");
            }
        }

        public void SelectNoteCategory(string option)
        {
            var select = new SelectElement(utilities.WaitUntilVisible(VentasLocators.ViewSales.NoteTypeSelect));
            select.SelectByText(option);
        }

        public void SelectNoteDocument(string option)
        {
            var select = new SelectElement(utilities.WaitUntilVisible(VentasLocators.ViewSales.NoteDocumentSelect));
            select.SelectByText(option);
        }

        public void EnterReason(string value)
        {
            utilities.ClearAndEnterText(VentasLocators.ViewSales.NoteReason, value);
        }

        public void EnterNoteAmount(string value)
        {
            utilities.ClearAndEnterText(VentasLocators.ViewSales.NoteAmount, value);
        }

        public void EnterRowAmount(string value)
        {
            utilities.ClearAndEnterText(VentasLocators.ViewSales.NoteRowAmount, value);
        }

        public void EnterQuantity(string value)
        {
            utilities.ClearAndEnterText(VentasLocators.ViewSales.NoteQuantity, value);
        }

        public void SelectCreditDelivery(string option)
        {
            if (option.Trim().Equals("INMEDIATA", StringComparison.OrdinalIgnoreCase))
            {
                utilities.ClickButton(VentasLocators.ViewSales.NoteImmediate);
                return;
            }

            utilities.ClickButton(VentasLocators.ViewSales.NoteDeferred);
        }

        public void SaveNote()
        {
            utilities.ClickButton(VentasLocators.ViewSales.SaveNote);
        }

        public void InvalidateDocument()
        {
            utilities.ClickButton(VentasLocators.ViewSales.InvalidateButton);
        }

        public void EnterObservation(string value)
        {
            utilities.ClearAndEnterText(VentasLocators.ViewSales.Observation, value);
        }

        public void AcceptInvalidation()
        {
            utilities.ClickButton(VentasLocators.ViewSales.AcceptInvalidation);
        }

        public void CloneSale()
        {
            utilities.ClickButton(VentasLocators.ViewSales.CloneButton);
        }

        public void PrintDocument()
        {
            utilities.ClickButton(VentasLocators.ViewSales.PrintButton);
        }

        public void DownloadDocument(string option)
        {
            switch (option.Trim().ToUpperInvariant())
            {
                case "PDF":
                    utilities.ClickButton(VentasLocators.ViewSales.PdfButton);
                    break;

                case "XML":
                    utilities.ClickButton(VentasLocators.ViewSales.DownloadDropdown);
                    utilities.ClickButton(VentasLocators.ViewSales.XmlButton);
                    break;

                case "ZIP":
                    utilities.ClickButton(VentasLocators.ViewSales.DownloadDropdown);
                    utilities.ClickButton(VentasLocators.ViewSales.ZipButton);
                    break;

                default:
                    throw new ArgumentException($"El tipo de descarga '{option}' no esta soportado.");
            }
        }

        public void OpenSendModal()
        {
            utilities.ClickButton(VentasLocators.ViewSales.SendButton);
        }

        public void EnterEmail(string value)
        {
            utilities.ClearAndEnterText(VentasLocators.ViewSales.EmailInput, value);
        }

        public void AddEmail()
        {
            utilities.ClickButton(VentasLocators.ViewSales.AddEmail);
        }

        public void SendMail()
        {
            utilities.ClickButton(VentasLocators.ViewSales.SendMail);
        }
    }
}
