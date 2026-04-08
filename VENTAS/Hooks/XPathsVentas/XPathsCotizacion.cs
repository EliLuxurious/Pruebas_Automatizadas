using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigesCore.Hooks.XPaths
{
    public class NewQuote
    {
        public static readonly By modal = By.Id("modal-registro-cotizacion");

        public static readonly By qoute = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[3]/a[1]");

        public static readonly By newQoute = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[4]/button[1]");

        public static readonly By unitPrice = By.Id("precio-unitario-0");

        public static readonly By igv = By.Id("cotizacionigv");
             
        public static readonly By expirationDate = By.Id("fechaVencimiento");

        public static readonly By pregenerateSalePath = By.XPath("//tbody/tr[1]/td[10]/a[3]");

        public static readonly By pregenerateOrderPath = By.XPath("//tbody/tr[1]/td[10]/a[4]");
    }
    /*
    public class PregenerateOrder
    {
        public static readonly By saveButton = By.XPath("//body/div[4]/div[1]/div[1]/div[1]/div[3]/button[1]");
    }

    public class PregenerateSale
    {
        public static readonly By saveSale = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/button[1]");
    }*/
}
