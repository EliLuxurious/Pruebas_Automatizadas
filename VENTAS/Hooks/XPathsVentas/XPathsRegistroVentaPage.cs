using Microsoft.Win32;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigesCore.Hooks.XPaths
{
    public class Login
    {
        public static readonly By txtEmail = By.XPath("//input[@id='Email']");

        public static readonly By txtPassword = By.XPath("//input[@id='Password']");

        public static readonly By btnSignIn = By.XPath("//button[contains(text(),'Iniciar')]");

        public static readonly By btnConfirm = By.XPath("//button[contains(text(),'Aceptar')]");
    }

    public class SalesModule
    {
        public static readonly By btnSalesMenu = By.XPath("//span[contains(text(),'Venta')]");

        public static readonly By btnNewSale = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[1]/ul[1]/li[1]/a[1]");

        public static readonly By btnSaleCashMode = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[1]/ul[1]/li[2]/a[1]");

        public static readonly By btnContingencySale = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[1]/ul[1]/li[3]/a[1]");

        public static readonly By btnViewSales = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[1]/ul[1]/li[4]/a[1]");
    }

    public class Concept
    {
        public static readonly By txtBarCode= By.Id("idCodigoBarra");

        public static readonly By selConceptSelection = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[1]/div[1]/div[1]/registrador-detalles[1]/div[1]/div[1]/selector-concepto-comercial[1]/ng-form[1]/div[1]/div[3]/div[1]/div[1]/span[1]/span[1]/span[1]");

        public static readonly By txtQuantity = By.XPath("//input[@id='cantidad-0']");

        public static readonly By txtUnitPrice = By.XPath("//input[@id='precio-0']");

    }

    public class CheckBox
    {
        public static readonly By chkIGV = By.XPath("//input[@id='ventaigv0']");

        public static readonly By chkUnifiedDetail = By.XPath("//input[@id='detalleunificado0']");
    }

    public class Dates
    {
        public static readonly By chkTypeClient = By.Id("DocumentoIdentidad");

        public static readonly By txtAlias = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[5]/input[1]");

        public static readonly By RegisteredCustomerButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/selector-actor-comercial[1]/ng-form[1]/div[1]/div[1]/div[1]/a[3]");

        public static readonly By RegisteredCustomerField = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/selector-actor-comercial[1]/ng-form[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/input[1]");

        public static readonly By IssueDateFieldContingency = By.XPath("/html[1]/body[1]/div[1]/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[6]/input[1]");

        public static readonly By IssueDateNameContingency = By.XPath("//body[1]/div[1]/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[6]/label[1]");

        public static readonly By DocNumberContingency = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[7]/selector-comprobante[1]/div[1]/ng-form[1]/div[3]/input[1]");
    }

    public class CashSales
    {
        public static readonly By PointSalePath = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[3]/div[1]/span[1]/span[1]/span[1]");

        public static readonly By SellerPath = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[3]/div[2]/span[1]/span[1]/span[1]");
    }
    public class Voucher
    {
        public static readonly By DocNewSaleField = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[6]/selector-comprobante[1]/div[1]/ng-form[1]/div[1]/div[1]/span[1]/span[1]/span[1]");

        public static readonly By DocInputField = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[6]/selector-comprobante[1]/div[1]/ng-form[1]/div[1]/div[1]/span[2]/span[1]/span[1]/input[1]");

        public static readonly By DocContingencyField = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[7]/selector-comprobante[1]/div[1]/ng-form[1]/div[1]/div[1]/span[1]/span[1]/span[1]");
    }

    public class Delivery
    {
        public static readonly By immediate = By.XPath("//label[@for='radioEntrega1']");

        public static readonly By deferredLabel = By.XPath("//label[@for='radioEntrega2']");
    }

    public class DispatchGuide
    {
        public static readonly By Modal = By.Id("modal-registro-guia-remision");

        public static readonly By Option = By.TagName("option");

        public static readonly By DispatchGuideButton = By.XPath("//a[@id='id-registro-guia-remision']");

        public static readonly By StartDateTransferPath = By.XPath("//input[@id='fechaInicioTraslado']");

        public static readonly By TotalGrossWeightPath = By.XPath("//input[@id='pesobrutototal']");

        public static readonly By NumberOfPackagesPath = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/registrador-guia-remision[1]/form[1]/div[1]/div[1]/div[1]/div[1]/div[7]/div[1]/div[2]/input[1]");

        public static readonly By CarrierRUCPath = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/registrador-guia-remision[1]/form[1]/div[1]/div[2]/div[1]/div[1]/selector-actor-comercial[1]/ng-form[1]/div[1]/div[1]/div[1]/input[1]");

        public static readonly By DriverDNIPath = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/registrador-guia-remision[1]/form[1]/div[1]/div[2]/div[1]/div[1]/selector-actor-comercial[2]/ng-form[1]/div[1]/div[1]/div[1]/input[1]");

        public static readonly By DriverLicensePath = By.XPath("//input[@id='nLicencia']");

        public static readonly By VehiclePlatePath = By.XPath("//input[@id='marcaPlaca']");

        public static readonly By TransportModePath = By.Id("modalidad");

        public static readonly By OriginAddressUbigeoPath = By.Id("ubigeoOrigen");

        public static readonly By OriginAddressDetailPath = By.XPath("//input[@id='direccionOrigen']");

        public static readonly By DestinationAddressUbigeoPath = By.Id("ubigeoDestino");

        public static readonly By DestinationAddressDetailPath = By.XPath("//input[@id='direccionDestino']");

        public static readonly By AcceptDispatchGuideButtonPath = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[3]/a[2]");
    }

    public class PaymentTypePath
    {
        public static readonly By CashPaymentOption = By.XPath("//label[@for='radio1' and text()='CO']");

        public static readonly By QuickPaymentOption = By.XPath("//label[@for='radio2' and text()='CR']");

        public static readonly By ConfiguredPaymentOption = By.XPath("//label[@for='radio3' and text()='CC']");
    }

    public class PaymentMethodPath
    {
        public static readonly By DepositButton = By.XPath("//label[@id='labelMedioPago-0-14']");

        public static readonly By TransferButton = By.XPath("//label[@id='labelMedioPago-0-16']");

        public static readonly By DebitCardButton = By.XPath("//label[@id='labelMedioPago-0-18']");

        public static readonly By CreditCardButton = By.XPath("//label[@id='labelMedioPago-0-19']");

        public static readonly By CashButton = By.XPath("//label[@id='labelMedioPago-0-281']");

        public static readonly By PointsButton = By.XPath("//label[@id='labelMedioPago-0-13901']");
    }

    public class DepositNewSale
    {
        public static readonly By BankSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[8]/div[1]/span[1]/span[1]/span[1]");

        public static readonly By PaymentDetails = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[8]/div[1]/textarea[1]");
    }

    public class DepositContingency
    {
        public static readonly By BankSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[8]/div[1]/span[1]/span[1]/span[1]");


        public static readonly By PaymentDetails = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[8]/div[1]/textarea[1]");
    }

    public class TransferNewSale
    {
        public static readonly By BankSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[7]/div[1]/span[1]/span[1]/span[1]");

        public static readonly By PaymentDetails = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[7]/div[1]/textarea[1");
    }

    public class TransferContingency
    {
        public static readonly By BankSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[7]/div[1]/span[1]/span[1]/span[1]");

        public static readonly By PaymentDetails = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[7]/div[1]/textarea[1]");
    }

    public class DebitPaymentNewSale
    {
        public static readonly By BankSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[6]/div[1]/span[1]/span[1]/span[1]");

        public static readonly By CardSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[6]/div[1]/span[2]/span[1]/span[1]");

        public static readonly By PaymentDetails = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[6]/div[1]/textarea[1]");
    }

    public class DebitPaymentContingency
    {
        public static readonly By BankSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[6]/div[1]/span[1]/span[1]/span[1]");

        public static readonly By CardSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[6]/div[1]/span[2]/span[1]/span[1]");

        public static readonly By PaymentDetails = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[6]/div[1]/textarea[1]");
    }

    public class CreditPaymentNewSale
    {
        public static readonly By BankSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[5]/div[1]/span[1]/span[1]/span[1]");

        public static readonly By CardSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[5]/div[1]/span[2]/span[1]/span[1]");

        public static readonly By PaymentDetails = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[5]/div[1]/textarea[1]");
    }

    public class CreditPaymentContingency
    {
        public static readonly By BankSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[5]/div[1]/span[1]/span[1]/span[1]");

        public static readonly By CardSelector = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[5]/div[1]/span[1]/span[1]/span[1]");

        public static readonly By PaymentDetails = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[5]/div[1]/span[1]/span[1]/span[1]");
    }

    public class CashNewSale
    {
        public static readonly By Received = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[2]/div[1]/input[1]");
    }

    public class CashContingency
    {
        public static readonly By Received = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/editor-traza-pago[1]/div[1]/div[2]/div[1]/input[1]");
    }

    public class QuickCreedit
    {
        public static readonly By InitialMountNewSale = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[8]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/input[1]");

        public static readonly By InitialMountContingency = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[9]/editor-pago[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/input[1]");
    }

    public class ConfiguredCreditPopup
    {
        public static readonly By Modal = By.Id("registro-financiamiento-0");

        public static readonly By InitialField = By.XPath("//input[@id='inicial']");

        public static readonly By InitialName = By.XPath("//label[contains(text(),'INICIAL')]");

        public static readonly By CoutaField = By.XPath("//input[@id='cuota']");

        public static readonly By PaymentDay = By.Id("select2-diavencimiento-container");

        public static readonly By GenerateQuotaButton = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[6]/button[1]");

        public static readonly By ExpirationDate = By.Id("diavencimiento");

        public static readonly By Option = By.TagName("option");

        public static readonly By Accept = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[3]/button[1]");
    }

    public class SaveSalePath
    {
        public static readonly By SaveSaleButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]");
    }

    public class AdditionalElements
    {
        public static readonly By SelectODropdownOptions = By.CssSelector(".select2-results__options");

        public static readonly By SelectODropdownOptionsConfigured = By.Id("select2-diavencimiento-results");

        public static readonly By OverlayElement = By.ClassName("block-ui-overlay");
    }
}
