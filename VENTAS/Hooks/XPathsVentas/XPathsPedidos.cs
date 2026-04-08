using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigesCore.Hooks.XPaths
{
    public class NewOrders
    {
        public static readonly By order = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[2]/a[1]");

        public static readonly By viewOrder = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[2]/ul[1]/li[1]/a[1]");

        public static readonly By newOrder = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[4]/button[1]");

        public static readonly By modal = By.Id("modal-registro-pedido");

        public static readonly By concept = By.Id("idCodigoBarra");

        public static readonly By quantity = By.Id("cantidad-0");

        public static readonly By unitPrice = By.Id("precio-0");

        public static readonly By client = By.Id("DocumentoIdentidad");

        public static readonly By igv = By.Id("ventaigv0");

        public static readonly By unifDetail = By.Id("detalleunificado0");

        public static readonly By alias = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/div[1]/form[1]/div[2]/div[1]/div[2]/datos-facturacion[1]/form[1]/div[1]/div[1]/div[5]/input[1]");

        public static readonly By voucher = By.Id("comprobantePredeterminado");

        public static readonly By inmediate = By.XPath("//label[@for='entrega-inmediata-pedido']");

        public static readonly By deferred = By.XPath("//label[@for='entrega-diferida-pedido']");

        public static readonly By save = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[3]/button[1]");
    }

    public class ConfirmOrderClass
    {
        public static readonly By modal = By.Id("modal-confirmar-pedido");

        public static readonly By initialDate = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/input[1]");

        public static readonly By finalDate = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/input[1]");

        public static readonly By orderQueryButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[3]/button[1]");

        public static readonly By confirmOrderPath = By.XPath("//tbody/tr[1]/td[9]/a[3]");

        public static readonly By voucher = By.Id("comprobante");
    }

    public class InvalidateOrderClass
    {
        public static readonly By invalidateOrderButton = By.XPath("//tbody/tr[1]/td[9]/a[2]");

        public static readonly By observation = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/form[1]/div[1]/div[1]/div[1]/textarea[1]");

        public static readonly By accept = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[2]/form[1]/div[1]/div[1]/div[3]/div[1]/a[1]");
    }
}
