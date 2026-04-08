using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SigesCore.Hooks.Utility;
using SigesCore.Hooks.XPaths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigesCore.Hooks.VentasPage
{
    public class VerVentasPage
    {
        private readonly IWebDriver driverViewPayment;
        WebDriverWait wait;
        UtilityVenta utilityPage;

        public VerVentasPage (IWebDriver driverViewPayment)
        {
            this.driverViewPayment = driverViewPayment;
            this.utilityPage = new UtilityVenta(driverViewPayment);
        }

        // INGRESO DE FECHA INICIAL 
        public void SetInitialDate(string value)
        {
            Thread.Sleep(4000);
            utilityPage.ClearAndSetInputField(RedeemVoucher.initialDate, value);
        }

        // INGRESO DE FECHA FINAL
        public void SetFinalDate(string value)
        {
            utilityPage.ClearAndSetInputField(RedeemVoucher.finalDate, value);
        }

        // CLICK EN CONSULTAR VENTAS
        public void SetSalesQuery()
        {
            utilityPage.ClickButton(RedeemVoucher.salesQueryButton);
            Thread.Sleep(3000);
        }

        // BUSCAR UNA VENTA POR SU CÓDIGO
        public void SearchSaleField(string value)
        {
            utilityPage.ClearAndSetInputField(RedeemVoucher.searchSale, value);
        }

        // ACTIVAR CHECKBOX DE CANJE
        public void ActivateRedeem()
        {
            driverViewPayment.FindElement(RedeemVoucher.activateRedeemPath).Click();
        }

        // SELECCIONAR LA VENTA BUSCADA
        public void SelectSale()
        {
            driverViewPayment.FindElement(RedeemVoucher.selectedSalePath).Click();
        }

        // CLICK EN EL BOTÓN CANJEAR
        public void ClickRedeemButton()
        {
            utilityPage.ClickButton(RedeemVoucher.redeemButton);
        }

        // SELECCIONAR EL TIPO DE COMPROBANTE
        public void SetVoucherType(string option)
        {
            utilityPage.OptionsSelector(RedeemVoucher.modal, RedeemVoucher.voucherType, option);
        }

        // CLICK EN EL BOTÓN ACEPTAR
        public void ClickAcceptRedeemButton()
        {
            utilityPage.ClickButton(RedeemVoucher.acceptRedeemButton);
            Thread.Sleep(4000);
        }

        // VER UNA VENTA (PRIMERA VENTA DE LA TABLA)
        public void SeeSale()
        {
            utilityPage.ClickButton(Additional.seeSalePath);
            Thread.Sleep(5000);
        }

        // SELECCIONAR EL TIPO DE NOTA
        public void ClickTypeNote(string option)
        {
            option = option.ToUpper();
            utilityPage.ElementExists(DebitNote.debitNoteButton);
            switch (option)
            {
                case "DÉBITO":
                    utilityPage.ClickButton(DebitNote.debitNoteButton);
                    break;

                case "CRÉDITO":
                    utilityPage.ClickButton(CreditNote.creditNoteButton);
                    break;

                default:
                    throw new ArgumentException($"El {option} no es válido");
            }
        }

        // SELECCIONAR EL TIPO DE NOTA DE DÉBITO (NOTA DE DÉBITO)
        public void TypeDebitNote(string option)
        {
            utilityPage.OptionsSelector(DebitNote.modal, DebitNote.TypeDebitNotePath, option);
        }

        // SELECCIONAR NOTA DE DÉBITO (NOTA DE DÉBITO)
        public void DocumentType(string option)
        {
            utilityPage.OptionsSelector(DebitNote.modal, DebitNote.documentTypePath, option);
        }

        // INGRESO DEL MOTIVO DE LA NOTA (NOTA DE DÉBITO)
        public void ReasonDebitNote(string value)
        {
            utilityPage.ClearAndSetInputField(DebitNote.reasonDebitNotePath, value);
        }

        // INGRESO DEL MOTIVO DE LA NOTA (NOTA DE DÉBITO)
        public void DeliveryTypeNoteCredit(string option)
        {
            utilityPage.SelectDeliveryType(DebitNote.modal, CreditNote.immediate, CreditNote.deferred, option);
        }

        // INGRESO DEL TOTAL AUMENTO DEL VALOR (NOTA DE DÉBITO)
        public void TotalAmount(string value)
        {
            utilityPage.ClearAndSetInputField(DebitNote.totalAmountPath, value);
        }

        // INGRESO  DEL INTERÉS TOTAL (NOTA DE DÉBITO)
        public void NoteAmount(string value)
        {
            utilityPage.ClearAndSetInputField(DebitNote.totalinterestPath, value);
        }

        // INGRESAR LA CANTIDAD PARA DEVOLUCIÓN POR ÍTEM (NOTA DE CRÉDITO)
        public void QuantityCreditNote(string value)
        {
            utilityPage.ClearAndSetInputField(CreditNote.quantityPath, value);
        }

        // CLICK PARA GUARDA LA NOTA
        public void SaveNote()
        {
            utilityPage.ClickButton(DebitNote.saveNotePath);
            Thread.Sleep(4000);
        }

        // CLICK EN INVALIDAR EL DOCUMENTO
        public void ClickInvalidateDocument()
        {
            utilityPage.ClickButton(InvalidateDocumentClass.invalidateButton);
        }

        // INGRESO DE LA OBSERVACIÓN
        public void Observation(string value)
        {
            utilityPage.ClearAndSetInputField(InvalidateDocumentClass.observation, value);
        }

        // CLICK EN 'SI' PARA ACEPTAR LA INVALIDACIÓN
        public void AcceptInvalidation()
        {
            utilityPage.ClickButton(InvalidateDocumentClass.accept);
            Thread.Sleep(4000);
        }

        // CLICK EN CLONAR UNA VENTA
        public void CloneSale()
        {
            utilityPage.ClickButton(CloneDocumentClass.cloneButton);
            Thread.Sleep(4000);
        }

        // CLICK EN IMPRIMIR DOCUMENTO
        public void PrintDocument()
        {
            utilityPage.ClickButton(PrintDocumentClass.print);
        }

        // CLICK EN DESCARGAR DOCUMENTO EN DIFERENTES FORMATOS
        public void DownloadType(string option)
        {
            option = option.ToUpper();

            if (option == "PDF")
            {
                utilityPage.ClickButton(DownloadDocumentClass.pdfButton);
                Thread.Sleep(4000);
            }
            else if (option == "XML" || option == "ZIP")
            {
                utilityPage.ClickButton(DownloadDocumentClass.dropdown);
                if (option == "XML")
                {
                    utilityPage.ClickButton(DownloadDocumentClass.xmlButton);
                }
                else
                {
                    utilityPage.ClickButton(DownloadDocumentClass.zipButton);
                }
            }
            else
            {
                throw new ArgumentException($"El {option} no es válido");
            }
        }

        // CLICK EN ENVIAR EL DOCUMENTO
        public void SendDocument()
        {
            utilityPage.ClickButton(SendDocumentClass.sendButton);
        }

        // INGRESAR EL CORREO ELECTRÓNICO
        public void EmailField(string value)
        {
            utilityPage.ClearAndSetInputField(SendDocumentClass.inputEmail, value);
        }

        // AGREGAR EL CORREO INGRESADO
        public void AddEmail()
        {
            utilityPage.ClickButton(SendDocumentClass.addEmailPath);
            Thread.Sleep(2000);
        }

        // CLICK EN ENVIAR EL COMPROBANTE POR CORREO
        public void Send()
        {
            utilityPage.ClickButton(SendDocumentClass.send);
        }
    }
}
