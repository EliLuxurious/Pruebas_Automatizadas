using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SigesCore.Hooks.Utility;
using SigesCore.Hooks.XPaths;
using RESTAURANTE.Hoks.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Equivalency;
using static OpenQA.Selenium.BiDi.Modules.BrowsingContext.Locator;
using SeleniumExtras.WaitHelpers;

namespace SigesCore.Hooks.VentasPage
{
    public class CotizacionPage
    {
        private readonly IWebDriver driverQuota;
        WebDriverWait wait;
        UtilityVenta utilityPage;

        public CotizacionPage(IWebDriver driverQouta)
        {
            this.driverQuota = driverQouta;
            this.utilityPage = new UtilityVenta(driverQouta);
        }

        // CLICK EN EL MÓDULO DE COTIZACIÓN
        public void ClickQouta()
        {
            utilityPage.ClickButton(NewQuote.qoute);
            Thread.Sleep(4000);
        }

        // CLICK EN NUEVA COTIZACIÓN
        public void ClickNewQouta()
        {
            utilityPage.ClickButton(NewQuote.newQoute);
        }

        // AGREGAR CONCEPTO POR CÓDIGO BARRA
        public void ConceptQuota(string value)
        {
            utilityPage.InputAndEnterModal(NewQuote.modal, NewOrders.concept, value);
        }

        // INGRESAR LA CANTIDAD DEL CONCEPTO
        public void QuantityconceptQuota(string value)
        {
            utilityPage.CleanFieldModal(NewQuote.modal, NewOrders.quantity, value);
        }

        // INGRESAR LA PRECIO UNITARIO
        public void UnitPriceConceptQuota(string value)
        {
            utilityPage.CleanFieldModal(NewQuote.modal, NewQuote.unitPrice, value);
        }

        // OPCIÓN DE MARCAR O NO EL IGV
        public void IGVQuota(string option)
        {
            utilityPage.CheckBox(NewQuote.modal, NewQuote.igv, option);
        }

        // SELECCIONAR EL TIPO DE CLIENTE
        public void CustomerTypeQuota(string option, string value)
        {
            utilityPage.CustomerType(NewQuote.modal, NewOrders.client, NewOrders.alias, option, value);
        }
        
        // SELECCIONAR EL TIPO DE CLIENTE
        public void ExpirationDateQouta(string value)
        {
            utilityPage.InputAndEnterModal(NewQuote.modal, NewQuote.expirationDate, value);
            Thread.Sleep(2000);
        }

        // CLICK EN PREGENERAR PEDIDO
        public void ClickPregenerateOrder()
        {
            utilityPage.ClickButton(NewQuote.pregenerateOrderPath);
            Thread.Sleep(12000);
        }

        // CLICK EN PREGENERAR VENTA
        public void ClickPregenerateSale()
        {
            utilityPage.ClickButton(NewQuote.pregenerateSalePath);
            Thread.Sleep(7000);
        }

        // MARCAR EL CHECKBOX PARA DETALLE UNIFICADO
        public void UnifiedDetailOrderModal(string option)
        {
            utilityPage.NewWindow(driverQuota);
            utilityPage.CheckBox(NewOrders.modal, NewOrders.unifDetail, option);
            Thread.Sleep(2000);
        }

        // SELECCIONAR EL TIPO DE CLIENTE
        public void CustomerTypeOrderModal(string option, string value)
        {
            utilityPage.NewWindow(driverQuota);
            utilityPage.CustomerType(NewOrders.modal, NewOrders.client, NewOrders.alias, option, value);
            Thread.Sleep(2000);
        }

        // GUARDAR EL PEDIDO PREGENERADO DESDE COTIZACIÓN
        public void SaveOrderFromQuote()
        {
            utilityPage.NewWindow(driverQuota);
            driverQuota.FindElement(NewOrders.save).Click();
            Thread.Sleep(2000);
        }

        // GUARDAR LA VENTA PREGENERADA DESDE COTIZACIÓN
        public void SavePregenerateSale()
        {
            utilityPage.NewWindow(driverQuota);
            driverQuota.FindElement(SaveSalePath.SaveSaleButton).Click();
            Thread.Sleep(4000);
        }
    }
}
