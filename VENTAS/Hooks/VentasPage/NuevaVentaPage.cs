using FluentAssertions.Equivalency;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SigesCore.Hooks.Utility;
using SigesCore.Hooks.XPaths;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static OpenQA.Selenium.BiDi.Modules.BrowsingContext.Locator;

namespace SigesCore.Hooks.VentasPage
{
    public class NuevaVentaPage
    {
        private readonly IWebDriver driver;
        WebDriverWait wait;
        UtilityVenta utilityPage;

        public NuevaVentaPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilityPage = new UtilityVenta(driver);
        }

        // SELECCIÓN DE MÓDULO Y SUBMÓDULO
        public void SelectModule(string option)
        {
            utilityPage.ClickButton(SalesModule.btnSalesMenu);

            switch (option)
            {
                case "Nueva Venta":

                    utilityPage.ClickButton(SalesModule.btnNewSale);
                    Thread.Sleep(5000);
                    break;

                case "Venta Modo Caja":

                    utilityPage.ClickButton(SalesModule.btnSaleCashMode);
                    break;

                case "Venta Por Contingencia":

                    utilityPage.ClickButton(SalesModule.btnContingencySale);
                    break;

                case "Ver Ventas":

                    utilityPage.ClickButton(SalesModule.btnViewSales);
                    break;

                default:
                    throw new ArgumentException($"El {option} no es válido.");
            }
        }

        // AGREGACIÓN DE CONCEPTO
        public void TypeSelectConcept(string option, string value)
        {
            option = option.ToUpper();
            utilityPage.ElementExists(Concept.txtBarCode);
            switch (option)
            {
                case "BARRA":
                    //utilityPage.EnterDate(Concept.txtBarCode, value);
                    utilityPage.WaitExistsVisible(Concept.txtBarCode, AdditionalElements.OverlayElement);
                    utilityPage.InputAndEnter(Concept.txtBarCode, value);
                    Thread.Sleep(4000);
                    break;

                case "SELECCION":
                    Thread.Sleep(4000);
                    utilityPage.SelectOption(Concept.selConceptSelection, value);
                    Thread.Sleep(5000);
                    break;

                default:
                    throw new ArgumentException($"El {option} no es válido");
            }
        }

        // INGRESAR CANTIDAD Y PRECIO UNITARIO
        public void QuantityAndUnitPrice(string quantity, string unitPrice)
        {
            utilityPage.ClearAndSetInputField(Concept.txtQuantity, quantity);
            utilityPage.ClearAndSetInputField(Concept.txtUnitPrice, unitPrice);
        }

        // SELECCIONAR IGV Y DET.UNIF
        public class CheckboxHelper
        {
            public static void CheckOption(By locator, string option, IWebDriver driver)
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                option = option.ToUpper();
                var checkbox = driver.FindElement(locator);

                if (option == "SI")
                {
                    if (!checkbox.Selected)
                    {
                        checkbox.Click();
                        Console.WriteLine($"El checkbox {locator} ha sido activado.");
                    }
                    else
                    {
                        Console.WriteLine($"El checkbox {locator} ya estaba activado.");
                    }
                }
                else if (option == "NO")
                {
                    Console.WriteLine($"El checkbox {locator} sigue desactivado.");
                }
                else
                {
                    throw new ArgumentException($"Opción no válida: {option}. Use 'SI' o 'NO'.");
                }
            }

            public static void EnableIGV(string option, IWebDriver driver)
            {
                CheckOption(CheckBox.chkIGV, option, driver);
            }

            public static void EnableUnifiedDetail(string option, IWebDriver driver)
            {
                CheckOption(CheckBox.chkUnifiedDetail, option, driver);
            }
        }

        //SELECCIONAR EL PUNTO DE VENTA (PROPIO DE VENTA POR CAJA)
        public void PointSale(string option)
        {
            utilityPage.SelectOption(CashSales.PointSalePath, option);
        }

        // SELECCIONAR EL PUNTO DE VENTA(PROPIO DE VENTA POR CAJA)
        public void Seller(string option)
        {
            utilityPage.SelectOption(CashSales.SellerPath, option);
        }

        // SELECCIONAR EL TIPO DE CLIENTE
        public void SelectCustomerType(string option, string value)
        {
            option = option.ToUpper();
            utilityPage.ElementExists(Dates.chkTypeClient);

            switch (option)
            {
                case "VARIOS":
                    break;

                case "DNI":
                case "RUC":
                    utilityPage.InputAndEnter(Dates.chkTypeClient, value);
                    break;

                case "ALIAS":
                    utilityPage.InputAndEnter(Dates.txtAlias, value);
                    break;

                default:
                    throw new ArgumentException($"La {option} no es válido");
            }
            Thread.Sleep(3000);
        }

        //SELECCIONAR TIPO DE COMPROBANTE
        public void SelectInvoiceType(string option, string module)
        {

            if (module == "Nueva Venta" || module == "Venta Modo Caja")
            {
                utilityPage.SelectInvoiceType(Voucher.DocNewSaleField, option);
            }

            else if (module == "Venta por Contingencia")
            {
                utilityPage.SelectInvoiceType(Voucher.DocContingencyField, option);
            }
            else
            {
                throw new ArgumentException($"Módulo '{module}' no reconocido.");
            }
        }

        // GUÍA DE REMISIÓN
        public void ClickDispatchGuide()
        {
            utilityPage.ClickButton(DispatchGuide.DispatchGuideButton);
        }

        // FECHA DE INICIO DE TRASLADO
        public void StartDateTransfer(string value)
        {
            Thread.Sleep(3000);
            //utilityPage.EnterDate(DispatchGuide.StartDateTransferPath, value);
            utilityPage.ClearAndSetInputField(DispatchGuide.StartDateTransferPath, value);
        }

        // PESO BRUTO TOTAL
        public void TotalGrossWeight(string value)
        {
            utilityPage.ClearAndSetInputField(DispatchGuide.TotalGrossWeightPath, value);
        }

        // NÚMERO DE BULTOS
        public void NumberOfPackages(string value)
        {
            utilityPage.ClearAndSetInputField(DispatchGuide.NumberOfPackagesPath, value);
        }

        // RUC DE TRANSPORTISTA
        public void CarrierRUC(string value)
        {
            utilityPage.InputAndEnter(DispatchGuide.CarrierRUCPath, value);
        }

        // MODALIDAD DE TRANSPORTE
        public void TransportMode(string option)
        {
            utilityPage.OptionsSelector(DispatchGuide.Modal, DispatchGuide.TransportModePath, option);
        }

        // INGRESO DEL NRO DE DNI DEL CONDUCTOR
        public void DriverDNI(string option)
        {
            utilityPage.ClearAndSetInputField(DispatchGuide.DriverDNIPath, option);
            driver.FindElement(DispatchGuide.DriverDNIPath).SendKeys(Keys.Enter);
        }

        // INGRESO DEL NRO DE LICENCIA DEL CONDUCTOR
        public void DriverLicense(string option)
        {
            utilityPage.ClearAndSetInputField(DispatchGuide.DriverLicensePath, option);
        }

        // INGRESO DE LA PLACA DEL VEHÍCULO
        public void VehiclePlate(string option)
        {
            utilityPage.ClearAndSetInputField(DispatchGuide.VehiclePlatePath, option);
        }

        // UBIGEO DE DIRECCIÓN DE ORIGEN
        public void OriginAddressUbigeo(string option)
        {
            utilityPage.OptionsSelector(DispatchGuide.Modal, DispatchGuide.OriginAddressUbigeoPath, option);
        }

        // DETALLE DE DIRECCIÓN DE ORIGEN
        public void OriginAddressDetail(string option)
        {
            utilityPage.ClearAndSetInputField(DispatchGuide.OriginAddressDetailPath, option);
        }

        // UBIGEO DE DIRECCIÓN DE DESTINO
        public void DestinationAddressUbigeo(string option)
        {
            utilityPage.OptionsSelector(DispatchGuide.Modal, DispatchGuide.DestinationAddressUbigeoPath, option);
        }

        // DETALLE DE DIRECCIÓN DE DESTINO
        public void DestinationAddressDetail(string option)
        {
            utilityPage.ClearAndSetInputField(DispatchGuide.DestinationAddressDetailPath, option);
        }

        // CLICK EN ACEPTAR REGISTRO DE GUÍA DE REMISIÓN
        public void AcceptDispatchGuideButton()
        {
            utilityPage.ClickButton(DispatchGuide.AcceptDispatchGuideButtonPath);
            Thread.Sleep(2000);
        }

        // INGRESAR FECHA DE EMISIÓN (PROPIO DE VENTA POR CONTINGENCIA)
        public void IssueDateContingency(string value) 
        {
            utilityPage.EnterDateClick(Dates.IssueDateFieldContingency, Dates.IssueDateNameContingency, value);
        }

        // INGRESAR EL NRO DEL COMPROBANTE (PROPIO DE VENTA POR CONTINGENCIA)
        public void DocumentNumberContingency (string value)
        {
            utilityPage.ClearAndSetInputField(Dates.DocNumberContingency, value);
        }

        // SELECCIÓN DE TIPO DE ENTREGA
        public void SelectDeliveryType(string option)
        {
            option = option.ToUpper();

            if (option == "INMEDIATA")
            {
                utilityPage.ClickButton(Delivery.immediate);
            }
            else if (option == "DIFERIDA")
            {
                utilityPage.ClickButton(Delivery.deferredLabel);
            }
            else
            {
                throw new ArgumentException($"El {option} no es válido");
            }
        }

        // SELECCIONAR TIPO DE PAGO
        public void SelectPaymentType(string option)
        {

            switch (option.ToLower())
            {
                case "contado":
                    utilityPage.ClickButton(PaymentTypePath.CashPaymentOption);
                    break;

                case "credito rapido":
                    utilityPage.ClickButton(PaymentTypePath.QuickPaymentOption);
                    break;

                case "credito configurado":
                    utilityPage.ClickButton(PaymentTypePath.ConfiguredPaymentOption);
                    break;

                default:
                    throw new ArgumentException($"Payment type '{option}' is not recognized.");
            }
        }

        // INGRESAR LA INICIAL DEL MONTO A PAGAR (FUNCIÓN PROPIO PARA CRÉDITO RÁPIDO)
        public void InitialQuickPayment(string value, string module)
        {

            if (module == "Nueva Venta" || module == "Venta Modo Caja")
            {
                utilityPage.ClearAndSetInputField(QuickCreedit.InitialMountNewSale, value);
            }
            else if (module == "Venta por Contingencia")
            {
                utilityPage.ClearAndSetInputField(QuickCreedit.InitialMountContingency, value);
            }
            else
            {
                throw new ArgumentException($"Módulo '{module}' no reconocido.");
            }
        }

        // SELECCIONAR EL MEDIO DE PAGO
        public void PaymentMethod(string option)
        {
            option = option.ToUpper();

            switch (option)
            {
                case "DEPCU":

                    utilityPage.PaymentMethodUtility(PaymentMethodPath.DepositButton, option);
                    break;

                case "TRANFON":

                    utilityPage.PaymentMethodUtility(PaymentMethodPath.TransferButton, option);
                    break;

                case "TDEB":

                    utilityPage.PaymentMethodUtility(PaymentMethodPath.DebitCardButton, option);
                    break;

                case "TCRE":

                    utilityPage.PaymentMethodUtility(PaymentMethodPath.CreditCardButton, option);
                    break;

                case "EF":

                    utilityPage.PaymentMethodUtility(PaymentMethodPath.CashButton, option);
                    break;

                case "PTS":

                    utilityPage.PaymentMethodUtility(PaymentMethodPath.PointsButton, option);
                    break;

                default:
                    throw new ArgumentException($"La opción {option} no es válido");
            }
            Thread.Sleep(4000);
        }
        
        // INGRESAR DETALLES DEL PAGO
        public void EnterCardDetails(string bank, string card, string info, string module)
        {

            if (module == "Nueva Venta" || module == "Venta Modo Caja")
            {
                utilityPage.EnterCardDetails("Nueva Venta o Modo Caja", bank, card, info);
            }

            else if (module == "Venta por Contingencia")
            {
                utilityPage.EnterCardDetails("Contingency", bank, card, info);
            }
            else
            {
                throw new ArgumentException($"Módulo '{module}' no reconocido.");
            }
        }

        // INGRESAR LA INICIAL(PROPIO PARA CRÉDITO CONFIGURADO)
        public void Initial(string value)
        {
            utilityPage.WaitModalAndEnterField(ConfiguredCreditPopup.Modal, ConfiguredCreditPopup.InitialField, value);
        }

        // INGRESAR EL NRO DE COUTAS (PROPIO PARA CRÉDITO CONFIGURADO)
        public void Cuota(string value)
        {
            utilityPage.ClearAndSetInputField(ConfiguredCreditPopup.CoutaField, value);
        }

        //INGRESAR EL NRO DE COUTAS SIN INICIAL (PROPIO PARA CRÉDITO CONFIGURADO)
        public void CoutasWithoutInitial(string value)
        {
            utilityPage.WaitModalAndEnterField(ConfiguredCreditPopup.Modal,ConfiguredCreditPopup.CoutaField, value);
        }

        //INGRESAR EL DÍA DE PAGO DE COUTAS (PROPIO PARA CRÉDITO CONFIGURADO)
        public void DateCuota(string value)
        {
            utilityPage.OptionsSelector(ConfiguredCreditPopup.Modal, ConfiguredCreditPopup.ExpirationDate, value);
        }

        //GENERAR COUTAS (PROPIO PARA CRÉDITO CONFIGURADO)
        public void GenerateQuota()
        {
            utilityPage.ClickButton(ConfiguredCreditPopup.GenerateQuotaButton);
        } 

        //BOTÓN ACEPTAR (PROPIO PARA CRÉDITO CONFIGURADO) 
        public void Accept()
        {
            utilityPage.ClickButton(ConfiguredCreditPopup.Accept);
            Thread.Sleep(2000);
        }

        //GUARDAR VENTA
        public void SaveSale()
        {
            utilityPage.ClickButton(SaveSalePath.SaveSaleButton);
            Thread.Sleep(7000);
        }
    }
}
