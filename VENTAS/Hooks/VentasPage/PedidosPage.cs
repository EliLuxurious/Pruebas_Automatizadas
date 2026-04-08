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
using FluentAssertions.Execution;
using OpenQA.Selenium.DevTools.V130.Debugger;

namespace SigesCore.Hooks.VentasPage
{
    public class PedidosPage
    {
        private readonly IWebDriver driverOrder;
        WebDriverWait wait;
        UtilityVenta utilityPage;

        public PedidosPage(IWebDriver driverOrder)
        {
            this.driverOrder = driverOrder;
            this.utilityPage = new UtilityVenta(driverOrder);
        }

        // CLICK EN EL MÓDULO PEDIDO
        public void ClickOrder()
        {
            utilityPage.ClickButton(NewOrders.order);
        }

        // CLICK EN VER PEDIDO
        public void ClickViewOrder()
        {
            utilityPage.ClickButton(NewOrders.viewOrder);
            Thread.Sleep(12000);
        }

        // CLICK EN NUEVO PEDIDO
        public void ClickNewOrder()
         {
            utilityPage.ClickButton(NewOrders.newOrder);
         }

        // AGREGAR CONCEPTO POR CÓDIGO BARRA
        public void ConceptOrder(string value)
        {
            utilityPage.InputAndEnterModal(NewOrders.modal, NewOrders.concept, value);
        }

        // INGRESAR LA CANTIDAD DEL CONCEPTO
        public void QuantityconceptOrder(string value)
        {
            utilityPage.CleanFieldModal(NewOrders.modal, NewOrders.quantity, value);
        }

        // INGRESAR LA PRECIO UNITARIO
        public void UnitPriceConceptOrder(string value)
        {
            utilityPage.CleanFieldModal(NewOrders.modal, NewOrders.unitPrice, value);
        }

        // OPCIÓN DE MARCAR O NO EL IGV
        public void IGVOrder(string option)
        {
            utilityPage.CheckBox(NewOrders.modal, NewOrders.igv, option);
        }

        // OPCIÓN DE MARCAR O NO EL DETALLE UNIFICADO
        public void UnifiedDetailOrder(string option)
        {
            utilityPage.CheckBox(NewOrders.modal, NewOrders.unifDetail, option);
        }

        // SELECCIONAR EL TIPO DE CLIENTE
        public void CustomerTypeOrder(string option, string value)
        {
            utilityPage.CustomerType(NewOrders.modal, NewOrders.client, NewOrders.alias, option, value);
        }

        // SELECCIONAR EL TIPO DE COMPROBANTE
        public void SelectInvoiceType(string option)
        {
            utilityPage.OptionsSelector(NewOrders.modal, NewOrders.voucher, option);
        }

        // SELECCIONAR EL TIPO DE ENTREGA
        public void SelectDeliveryType(string option)
        {
            utilityPage.SelectDeliveryType(NewOrders.modal, NewOrders.inmediate, NewOrders.deferred, option);
        }

        // INGRESO DE LA FECHA INICIAL
        public void InitialDate(string value)
        {
            Thread.Sleep(11000);
            utilityPage.ClearAndSetInputField(ConfirmOrderClass.initialDate, value);
        }

        // INGRESO DE LA FECHA FINAL
        public void FinalDate(string value)
        {
            utilityPage.ClearAndSetInputField(ConfirmOrderClass.finalDate, value);
        }

        // INGRESO DE LA FECHA INICIAL
        public void OrderQuery()
        {
            utilityPage.ClickButton(ConfirmOrderClass.orderQueryButton);
            Thread.Sleep(3000);
        }

        // CLICK EN CONSULTAR PEDIDOS
        public void InvoiceType(string option)
        {
            utilityPage.OptionsSelector(ConfirmOrderClass.modal, ConfirmOrderClass.voucher, option);
        }

        // CLICK EN CONFIRMAR UN PEDIDO
        public void ConfirmOrder()
        {
            utilityPage.ClickButton(ConfirmOrderClass.confirmOrderPath);
            Thread.Sleep(4000);
        }

        // CLICK EN INVALIDAR UN PEDIDO
        public void InvalidateOrder()
        {
            utilityPage.ClickButton(InvalidateOrderClass.invalidateOrderButton);
        }

        // INGRESO DE LA OBSERVACIÓN PARA INVALIDAR UN PEDIDO
        public void AddObservation(string value)
        {
            utilityPage.ClearAndSetInputField(InvalidateOrderClass.observation, value);
            Thread.Sleep(2000);
        }

        // CLICK EN ACEPTAR INVALIDACIÓN DE PEDIDO
        public void ClickAcceptInvalidation()
        {
            utilityPage.ClickButton(InvalidateOrderClass.accept);
            Thread.Sleep(4000);
        }

        // CLICK EN GUARDAR ORDEN
        public void SaveOrder()
        {
            driverOrder.FindElement(NewOrders.save).Click();
            Thread.Sleep(3000);
        }
    }
}
