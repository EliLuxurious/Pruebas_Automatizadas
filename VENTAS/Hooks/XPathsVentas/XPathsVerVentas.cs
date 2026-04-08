using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigesCore.Hooks.XPaths
{
    public class RedeemVoucher
    {
        public static readonly By modal = By.Id("modal-canje-de-comprobante");

        public static readonly By initialDate = By.XPath("//input[@id='fechaInicio']");

        public static readonly By finalDate = By.XPath("//input[@id='fechaFin']");

        public static readonly By salesQueryButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[4]/button[1]");

        public static readonly By searchDocument = By.XPath("//thead/tr[2]/th[4]/input[1]");

        public static readonly By searchSale = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[3]/div[1]/div[1]/div[2]/div[1]/label[1]/input[1]");

        public static readonly By activateRedeemPath = By.XPath("//input[@id='canjeComprobante']");

        public static readonly By selectedSalePath = By.XPath("//tbody/tr[1]/td[1]/div[1]/input[1]");

        public static readonly By redeemButton = By.XPath("//button[contains(text(),'CANJEAR')]");

        public static readonly By voucherType = By.CssSelector("select.tipoDocumento");

        public static readonly By acceptRedeemButton = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[3]/button[1]");   
    }

    public class DebitNote
    {
        public static readonly By modal = By.Id("modal-ver-venta");

        public static readonly By debitNoteButton = By.XPath("//button[contains(text(),'NOTA DE DÉBITO')]");

        public static readonly By TypeDebitNotePath = By.Id("tipoDeNota");

        public static readonly By documentTypePath = By.Id("documentoParaNota");

        public static readonly By reasonDebitNotePath = By.XPath("//textarea[@id='motivo']");

        public static readonly By totalinterestPath = By.XPath("//input[@id='montoNota']");

        public static readonly By totalAmountPath = By.XPath("//tbody/tr[1]/td[4]/input[1]");

        public static readonly By saveNotePath = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/div[5]/registrador-nota[1]/div[1]/div[1]/div[1]/div[2]/div[2]/button[2]");
    }

    public class CreditNote
    {
        public static readonly By creditNoteButton = By.XPath("//button[contains(text(),'NOTA DE CRÉDITO')]");

        public static readonly By quantityPath = By.XPath("//tbody/tr[1]/td[8]/input[1]");

        public static readonly By immediate = By.XPath("//label[@for='radioNota1']");

        public static readonly By deferred = By.XPath("//label[@for='radioNota2']");
    }

    public class InvalidateDocumentClass
    {
        public static readonly By invalidateButton = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/button[1]");

        public static readonly By observation = By.XPath("//body/div[6]/div[1]/div[1]/div[1]/div[2]/form[1]/div[1]/div[1]/div[3]/textarea[1]");

        public static readonly By accept = By.XPath("//a[contains(text(),'SI')]");
    }

    public class CloneDocumentClass
    {
        public static readonly By cloneButton = By.XPath("//a[contains(text(),'CLONAR')]");     
    }

    public class PrintDocumentClass
    {
        public static readonly By print = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/div[3]/button[1]");
    }

    public class DownloadDocumentClass
    {
        public static readonly By pdfButton = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/div[3]/div[1]/a[1]");

        public static readonly By dropdown = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/div[3]/div[1]/a[2]");

        public static readonly By xmlButton = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/div[3]/div[1]/div[1]/a[1]");

        public static readonly By zipButton = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/div[3]/div[1]/div[1]/a[2]");

    }

    public class SendDocumentClass
    {
        public static readonly By sendButton = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/div[3]/button[2]");

        public static readonly By inputEmail = By.XPath("//input[@id='correoImput']");

        public static readonly By addEmailPath = By.XPath("//tbody/tr[2]/td[2]/a[1]");

        public static readonly By send = By.XPath("//body/div[6]/div[1]/div[1]/div[1]/div[3]/button[1]");
    }

    public class Additional
    {
        public static readonly By seeSalePath = By.XPath("//tbody/tr[1]/td[13]/a[1]");
    }
}
