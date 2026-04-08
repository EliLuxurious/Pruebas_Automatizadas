using FluentAssertions.Equivalency;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SigesCore.Hooks.Utility;
using SigesCore.Hooks;
using SigesCore.Hooks.Utility;
using SigesCore.Hooks.XPaths;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using static OpenQA.Selenium.BiDi.Modules.BrowsingContext.Locator;
using Microsoft.Win32;
using Microsoft.VisualBasic;
using Reqnroll.Assist;

namespace SigesCore.Hooks.ComprasPage
{
    public class VerReportesPage
    {

        private IWebDriver driver;
        UtilityVenta utilityPage;

        public VerReportesPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilityPage = new UtilityVenta(driver);
        }

        public class PathsPurchasesReports
        {
            //Ingresar a la sección ver compras

            public static readonly By PurchaseButton = By.XPath("//span[contains(text(),'Compra')]");
            public static readonly By PurchaseReportsButton = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[5]/ul[1]/li[3]/a[1]");

            //Reportes

            //Por tipo
            public static readonly By FromDateTypeField = By.XPath("//input[@id='dateStart2']");
            public static readonly By ToDateTypeField = By.XPath("//input[@id='dateEnd2']");

            public static readonly By AllButton = By.XPath("//label[contains(text(),'Todos')]");
            public static readonly By CourtsButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[3]/div[1]/div[2]/div[2]/label[1]");
            public static readonly By NoCourtsButton = By.XPath("//label[contains(text(),'No Tributables')]");

            public static readonly By ViewReportsByTypeButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[5]/div[1]/a[1]");
            
            //Por comprobante
            public static readonly By FromDateTypeProofField = By.XPath("//input[@id='dateStart']");
            public static readonly By ToDateTypeProofField = By.XPath("//input[@id='dateEnd']");
            
            public static readonly By ViewReportsByProofButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[3]/div[1]/div[2]/div[1]/div[5]/div[1]/a[1]");

            //Por concepto
            public static readonly By FromDateConceptField = By.XPath("//input[@id='dateStart1']");
            public static readonly By ToDateConceptField = By.XPath("//input[@id='dateEnd1']");

            public static readonly By ViewReportsByConceptButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[4]/div[1]/div[2]/div[1]/div[4]/div[1]/a[1]");


        }

        public void EnterPurchaseReports()
        {
            ClickButton(PathsPurchasesReports.PurchaseButton);
            Thread.Sleep(2000);

            ClickButton(PathsPurchasesReports.PurchaseReportsButton);
            Thread.Sleep(2000);

        }

        public void SelectPurchaseReports(string option, string fromdate, string todate)
        {
            switch (option)
            {
                case "COMPROBANTE":
                    EnterField(PathsPurchasesReports.FromDateTypeProofField, fromdate);
                    Thread.Sleep(2000);

                    EnterField(PathsPurchasesReports.ToDateTypeProofField, todate);
                    Thread.Sleep(2000);
                    break;

                case "CONCEPTO":
                    EnterField(PathsPurchasesReports.FromDateConceptField, fromdate);
                    Thread.Sleep(2000);

                    EnterField(PathsPurchasesReports.ToDateConceptField, todate);
                    Thread.Sleep(2000);
                    break;
                default:
                    throw new ArgumentException($"La opción {option} no es válido");
            }
        }
        public void ReportByType(string option, string fromdate, string todate)
        {
            EnterField(PathsPurchasesReports.FromDateTypeField, fromdate);
            Thread.Sleep(2000);

            EnterField(PathsPurchasesReports.ToDateTypeField, todate);
            Thread.Sleep(2000);

            switch (option)
            {
                case "TODOS":

                    ClickButton(PathsPurchasesReports.AllButton);
                    Thread.Sleep(2000);

                    break;

                case "TRIBUNALES":

                    ClickButton(PathsPurchasesReports.CourtsButton);
                    Thread.Sleep(2000);

                    break;

                case "NO TRIBUNALES":

                    ClickButton(PathsPurchasesReports.NoCourtsButton);
                    Thread.Sleep(2000);

                    break;
                default:
                    throw new ArgumentException($"La opción {option} no es válido");
            }
        }

        public void MakeReport(string option)
        {
            switch (option)
            {
                case "TIPO":

                    ClickButton(PathsPurchasesReports.ViewReportsByTypeButton);
                    Thread.Sleep(2000);
                    break;

                case "COMPROBANTE":
                    
                    ClickButton(PathsPurchasesReports.ViewReportsByProofButton);
                    Thread.Sleep(2000);
                    break;

                case "CONCEPTO":

                    ClickButton(PathsPurchasesReports.ViewReportsByConceptButton);
                    Thread.Sleep(2000);
                    break;
                default:
                    throw new ArgumentException($"La opción {option} no es válido");
            }
        }
        public void EnterField(By _path, string _field)
        {
            var element = driver.FindElement(_path);
            element.Click();  // Asegura que el campo esté activo

            // Borra el campo completamente con Ctrl + A y Suprimir
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);

            // Espera un momento para asegurarse de que el campo esté vacío
            Thread.Sleep(100);

            // Verifica si el campo quedó vacío antes de ingresar el nuevo texto
            if (!string.IsNullOrEmpty(element.GetAttribute("value")))
            {
                element.Clear();
            }

            // Ingresa el nuevo valor
            element.SendKeys(_field);
            element.SendKeys(Keys.Enter);
        }
        public void EnterFieldEspecific(By _path, string _field)
        {
            var element = driver.FindElement(_path);
            element.Click();  // Asegura que el campo esté activo

            // Borra el campo completamente con Ctrl + A y Suprimir
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);

            // Espera un momento para asegurarse de que el campo esté vacío
            Thread.Sleep(100);

            // Verifica si el campo quedó vacío antes de ingresar el nuevo texto
            if (!string.IsNullOrEmpty(element.GetAttribute("value")))
            {
                element.Clear();
            }

            // Ingresa el nuevo valor
            element.SendKeys(_field);
            //element.SendKeys(Keys.Enter);
        }

        public void ClickButton(By _button)
        {
            driver.FindElement(_button).Click();
        }

        public void ExportResults(By _path)
        {
            ClickButton(_path);
            Thread.Sleep(6000);
        }

        public void SelectComboBox(By _path, string data)
        {
            // Ubicar el elemento <select>
            IWebElement selectElement = driver.FindElement(_path);

            // Crear el objeto SelectElement
            SelectElement dropdown = new SelectElement(selectElement);

            // Seleccionar el ROL pasado como parámetro
            dropdown.SelectByText(data);

            // Validar que la opción seleccionada es la esperada
            Assert.That(dropdown.SelectedOption.Text, Is.EqualTo(data),
                $"La opción seleccionada '{dropdown.SelectedOption.Text}' no coincide con '{data}'");

            // Pequeña pausa para visualización (opcional)
            Thread.Sleep(1000);
        }

        public void EnterFieldModal(By pathModal, By pathComponent, string value)
        {
            Thread.Sleep(3000);
            IWebElement orderModal = driver.FindElement(pathModal);
            orderModal.FindElement(pathComponent).Clear();
            orderModal.FindElement(pathComponent).SendKeys(value);
            orderModal.FindElement(pathComponent).SendKeys(Keys.Enter);
        }


        public void EnterFieldN(By _path, string _field)
        {
            //driver.FindElement(_path).Clear();
            driver.FindElement(_path).SendKeys(_field);
            //driver.FindElement(_path).SendKeys(Keys.Enter);
        }

        // SCROLL
        public void ScrollViewElement(IWebElement _path)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", _path);
            Thread.Sleep(2000);
        }

        public void ScrollViewTop()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, 0);");
            Thread.Sleep(2000);
        }
    }
}
