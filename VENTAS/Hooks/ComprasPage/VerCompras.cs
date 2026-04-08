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

namespace SigesCore.Hooks.ComprasPage
{

    public class VerComprasPage
    {

        private IWebDriver driver;
        UtilityVenta utilityPage;

        public VerComprasPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilityPage = new UtilityVenta(driver);
        }
        /* private readonly IWebDriver driverViewPayment;
         WebDriverWait wait;
         UtilityVenta utilityPage;
         UtilityVerVentas utilityViewPage;

         public VerComprasPage(IWebDriver driverViewPayment)
         {
             this.driverViewPayment = driverViewPayment;
             this.utilityPage = new UtilityVenta(driverViewPayment);
         }
        */
        public class viewshoppingsection
        {

            //Ingresar a la sección ver compras

            public static readonly By PurchaseButton = By.XPath("//span[contains(text(),'Compra')]");
            public static readonly By ViewPurchasesButton = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[5]/ul[1]/li[2]/a[1]");

            //Fechas

            public static readonly By FromDateField = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/input[1]");
            public static readonly By ToDateField = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/input[1]");

            //Filtros

            public static readonly By KeywordField = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[3]/div[1]/div[1]/div[2]/div[1]/label[1]/input[1]");
            public static readonly By DateField = By.XPath("//thead/tr[2]/th[2]/input[1]");
            public static readonly By TypeDocumentField = By.XPath("//thead/tr[2]/th[3]/input[1]");
            public static readonly By DocumentField = By.XPath("//thead/tr[2]/th[4]/input[1]");

            //Botónes

            public static readonly By SearchPurchasesButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[3]/button[1]");
            public static readonly By ExportPurchasesSearchButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[3]/button[2]\r\n");

        }


        // Encontrar el modal FACTURACION
        //IWebElement modalFacturacion = driver.FindElement(By.Id("modal-registro-guia-remision"));

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

        public void SearchPurchases(string fromdate, string todate)
        {

            ClickButton(viewshoppingsection.PurchaseButton);
            Thread.Sleep(4000);

            ClickButton(viewshoppingsection.ViewPurchasesButton);
            Thread.Sleep(4000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            EnterField(viewshoppingsection.FromDateField, fromdate);
            Thread.Sleep(4000);

            EnterField(viewshoppingsection.ToDateField, todate);
            Thread.Sleep(4000);


            ClickButton(viewshoppingsection.SearchPurchasesButton);
            Thread.Sleep(6000);
        }

        public void SearchFiltersGeneral(string optionsearch, string datasearch)
        {
            switch (optionsearch)
            {
                case "BUSQUEDA GENERAL":

                    EnterField(viewshoppingsection.KeywordField, datasearch);
                    Thread.Sleep(4000);
                    break;
                default:
                    throw new ArgumentException($"La opción {optionsearch} no es válida.");
            }
        }

        public void SearchFiltersSpecific(string optionsearch, string specificoptionsearch, string datasearch1, string datasearch2)
        {
            switch (optionsearch)
            {
                case "BUSQUEDA ESPECIFICA":
                    switch (specificoptionsearch)
                    {
                        case "FECHA, TIPO DOC.":

                            EnterField(viewshoppingsection.DateField, datasearch1);
                            Thread.Sleep(4000);
                            EnterField(viewshoppingsection.TypeDocumentField, datasearch2);
                            Thread.Sleep(4000);
                            break;

                        case "FECHA, DOCUMENTO":

                            EnterField(viewshoppingsection.DateField, datasearch1);
                            Thread.Sleep(4000);
                            EnterField(viewshoppingsection.DocumentField, datasearch2);
                            Thread.Sleep(4000);
                            break;

                        case "TIPO DOC., DOCUMENTO":

                            EnterField(viewshoppingsection.TypeDocumentField, datasearch1);
                            Thread.Sleep(4000);
                            EnterField(viewshoppingsection.DocumentField, datasearch2);
                            Thread.Sleep(4000);
                            break;

                        default:
                            throw new ArgumentException($"El filtro {specificoptionsearch} no es válido.");
                    }
                    break;
                default:
                    throw new ArgumentException($"La opción {optionsearch} no es válida.");
            }
        }
        public void SearchFiltersOneField(string optionsearch, string specificoptionsearch, string datasearch)
        {

            switch (optionsearch)
            {
                case "BUSQUEDA GENERAL":

                    EnterField(viewshoppingsection.KeywordField, datasearch);
                    Thread.Sleep(4000);
                    break;

                case "BUSQUEDA ESPECIFICA":
                    switch (specificoptionsearch)
                    {
                        case "FECHA":

                            EnterField(viewshoppingsection.DateField, datasearch);
                            Thread.Sleep(4000);
                            break;

                        case "TIPO DOC.":

                            EnterField(viewshoppingsection.TypeDocumentField, datasearch);
                            Thread.Sleep(4000); break;

                        case "DOCUMENTO":

                            EnterField(viewshoppingsection.DocumentField, datasearch);
                            Thread.Sleep(4000); break;

                        default:
                            throw new ArgumentException($"El filtro {specificoptionsearch} no es válido.");
                    }
                    break;

                default:
                    throw new ArgumentException($"La opción {optionsearch} no es válida.");
            }
        }
        public void ExportResults()
        {
            ClickButton(viewshoppingsection.ExportPurchasesSearchButton);
            Thread.Sleep(6000);
        }

        By Almacen = By.XPath("//span[contains(text(),'Almacén')]");
        By guiasderemision = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[7]/ul[1]/li[4]/a[1]");
        By nuevo = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/input[1]");
        By documento = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/input[1]");

        public void nuevaguia(string dni)
        {

            ClickButton(Almacen);
            Thread.Sleep(4000);

            ClickButton(guiasderemision);
            Thread.Sleep(4000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            ClickButton(nuevo);
            Thread.Sleep(4000);

            // Encontrar el modal FACTURACION
            IWebElement modalFacturacion = driver.FindElement(By.Id("modal-registro-guia-remision"));

            EnterField(documento, dni);
            Thread.Sleep(4000);

        }

        public class PathsRegisterNewPurchaseFromViewPrchases
        {

            //Ingresar a la sección ver compras

            public static readonly By PurchaseButton = By.XPath("//span[contains(text(),'Compra')]");
            public static readonly By ViewPurchasesButton = By.XPath("//body/div[@id='wrapper']/aside[1]/div[1]/section[1]/ul[1]/li[5]/ul[1]/li[2]/a[1]");

            //Nueva compra

            //public static readonly By NewPurchaseButton = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[4]/button[1]");


            public static readonly By SupplierField = By.XPath("//input[@id='DocumentoIdentidad']");

            //Datos sección1
            public static readonly By DateField = By.XPath("//input[@id='fechaRegistro']");
            public static readonly By SeriesField = By.XPath("//input[@ng-model='compra.TipoDeComprobante.SerieIngresada']");
            public static readonly By NumberDocumentField = By.XPath("//input[@ng-model='compra.TipoDeComprobante.NumeroIngresado']");
            public static readonly By ObservationField = By.Id("observacion");

            //Datos sección2

            //Tipo de entrega
            public static readonly By ImmediateButton = By.XPath("//label[contains(text(),'INMEDIATA')]");
            public static readonly By DiferredButton = By.XPath("//label[contains(text(),'DIFERIDA')]");

            //Almacénes
            public static readonly By OneWarehousesButton = By.XPath("//label[@for='radioEntrega3']");
            public static readonly By SeveralWarehousesButton = By.XPath("//label[@for='radioEntrega4']");

            public static readonly By concepto = By.CssSelector("div.ui-select-container.ui-select-bootstrap.dropdown");
           
            //Tipo de compra
            public static readonly By ExoneratedIGVButton = By.XPath("//label[contains(text(),'EXONERADAS IGV')]");
            public static readonly By GButton = By.Id("radio-2");
            public static readonly By NGIGVButton = By.Id("radio-3");
            public static readonly By GAndNGIGVButton = By.Id("radio-4");

            //Concepto

            public static readonly By ModalRegisterPurchase = By.Id("modal-registro-compra");
            public static readonly By ModalConcept = By.Id("idCodigoBarra");
            public static readonly By AmountField = By.CssSelector("input[ng-model='item.Importe']\r\n");

            //DEPCU

            public static readonly By CashField = By.Id("cuentaBancaria");
            public static readonly By BankAccountField = By.XPath("//input[@ng-model='$ctrl.trazaPago.Info.CuentaProveedor']");
            

            //PTS
            public static readonly By PtsField = By.XPath("//input[@ng-model='$ctrl.trazaPago.Info.MontoCajeado']\r\n");

            //Registrar nuevo usuario

            public static readonly By RegisterNewUserButton = By.XPath("//a[@title='NUEVO PROVEEDOR']");
            public static readonly By DNIField = By.XPath("//input[@id='numeroDocumento']");
            public static readonly By confirmar = By.XPath("By.XPath(\"//button[contains(@class, 'swal-button') and text()='OK']\")");

            //Registrar nuevo concepto

            public static readonly By RegisterNewConceptButton = By.XPath("//a[@title='Registrar una Nueva Mercaderia']");
            public static readonly By CodeField = By.XPath("//input[@ng-model='mercaderia.CodigoBarra']");
            public static readonly By SuffixField = By.XPath("//input[@ng-model='mercaderia.Sufijo']");

            public static readonly By modalmercaderia = By.Id("modal-mercaderia");
            public static readonly By FamilyComboBox = By.Id("nombre-basico");
            public static readonly By UMCComboBox = By.Id("select-model");
            public static readonly By BrandComboBox = By.CssSelector("[ng-model='mercaderia.IdsCaracteristicas[$index]']");
            public static readonly By NameComboBox = By.CssSelector("[ng-model='mercaderia.Presentacion']");
            public static readonly By QuantityField = By.XPath("//input[@ng-model='mercaderia.CantidadPresentacion']");

            public static readonly By PublicPriceField = By.XPath("//input[@ng-model='item.Valor']");
            public static readonly By FrequentCustomerPriceField = By.XPath("//tbody/tr[2]/td[3]/input[1]");
            public static readonly By WhosalePriceField = By.XPath("//tbody/tr[3]/td[3]/input[1]");
            public static readonly By RegisterNewFamilyButton = By.XPath("//a[@title='Nueva Familia' and @data-toggle='modal' and @data-target='#modal-concepto']");

            public static readonly By name = By.XPath("//input[@id='nombre' and @placeholder='Nombre' and @ng-model='concepto.Nombre']");

            public static readonly By FamilyCategoryComboBox = By.Id("categoria");
            public static readonly By BrandButton = By.XPath("//label[contains(text(),'MARCA')]");
            public static readonly By FamilySaveButton = By.XPath("//a[@ng-click='guardarConcepto(concepto)' and contains(@class, 'btn-primary')]\r\n");

            //a[@ng-click='guardarConcepto(concepto)' and contains(@class, 'btn-primary')]

        }
        public void RegisterNewUser(string dni, string sexo)
        {
            IWebElement Guardar = driver.FindElement(By.XPath("//a[@title='Guardar' and contains(text(), 'GUARDAR')]"));

            ClickButton(PathsRegisterNewPurchaseFromViewPrchases.RegisterNewUserButton);
            Thread.Sleep(2000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            EnterField(PathsRegisterNewPurchaseFromViewPrchases.DNIField, dni);
            Thread.Sleep(4000);

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            //ClickButton(PathsRegisterNewPurchaseFromViewPrchases.confirmar);

            // Ubicar el elemento <select>
            IWebElement selectElement = driver.FindElement(By.Id("sexo"));

            // Crear el objeto SelectElement
            SelectElement dropdown = new SelectElement(selectElement);

            // Seleccionar el ROL pasado como parámetro
            dropdown.SelectByText(sexo);

            // Validar que la opción seleccionada es la esperada
            Assert.That(dropdown.SelectedOption.Text, Is.EqualTo(sexo));
            Thread.Sleep(4000);

            ScrollViewElement(Guardar);

            Guardar.Click();
        }
        public void RegisterNewConcept(string code, string suffix, string family, string umc, string brand, string name, string quantity)
        {
            ClickButton(PathsRegisterNewPurchaseFromViewPrchases.RegisterNewConceptButton);
            Thread.Sleep(2000);

            //Datos Generales

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            IWebElement modal1 = driver.FindElement(By.Id("modal-registro-compra"));
            IWebElement modal2 = driver.FindElement(By.Id("modal-mercaderia"));


            EnterField(PathsRegisterNewPurchaseFromViewPrchases.CodeField, code);
            Thread.Sleep(2000);

            SelectComboBox(PathsRegisterNewPurchaseFromViewPrchases.FamilyComboBox, family);
            Thread.Sleep(2000);

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            EnterField(PathsRegisterNewPurchaseFromViewPrchases.SuffixField, suffix);
            Thread.Sleep(2000);

            SelectComboBox(PathsRegisterNewPurchaseFromViewPrchases.UMCComboBox, umc);
            Thread.Sleep(2000);

            SelectComboBox(PathsRegisterNewPurchaseFromViewPrchases.BrandComboBox, brand);
            Thread.Sleep(2000);

            //Presentación

            SelectComboBox(PathsRegisterNewPurchaseFromViewPrchases.NameComboBox, name);
            Thread.Sleep(2000);

            EnterField(PathsRegisterNewPurchaseFromViewPrchases.QuantityField, quantity);
            Thread.Sleep(2000);


        }

        public void SupplementaryData(string option, string price, string option2)
        {
            ClickButton(PathsRegisterNewPurchaseFromViewPrchases.RegisterNewConceptButton);
            Thread.Sleep(2000);

            IWebElement Save = driver.FindElement(By.XPath("//button[@ng-click='guardar()']"));
            IWebElement SaveAndClone = driver.FindElement(By.XPath("//button[@ng-click='guardarYClonar()']"));
            IWebElement Cancel = driver.FindElement(By.XPath("//button[@data-dismiss='modal' and @title='Cerrar']"));


            IWebElement Public = driver.FindElement(By.XPath("//label[contains(text(),'PUBLICO')]"));
            IWebElement FrequentCustomer = driver.FindElement(By.XPath("//label[contains(text(),'CLIENTE FRECUENTE')]"));
            IWebElement Wholesale = driver.FindElement(By.XPath("//label[contains(text(),'POR MAYOR')]"));

            //Datos complementarios

            ScrollViewElement(Save);

            switch (option)
            {
                case "PUBLICO":

                    if (!Public.Selected) // Si no está seleccionado, hacer clic
                    {
                        Public.Click();
                        Thread.Sleep(4000);
                    }

                    EnterField(PathsRegisterNewPurchaseFromViewPrchases.PublicPriceField, price);
                    break;

                case "CLIENTE FRECUENTE":

                    if (!FrequentCustomer.Selected) // Si no está seleccionado, hacer clic
                    {
                        FrequentCustomer.Click();
                        Thread.Sleep(4000);
                    }

                    EnterField(PathsRegisterNewPurchaseFromViewPrchases.FrequentCustomerPriceField, price);
                    Thread.Sleep(4000);
                    break;

                case "AL POR MAYOR":
                    if (!Wholesale.Selected) // Si no está seleccionado, hacer clic
                    {
                        Wholesale.Click();
                        Thread.Sleep(4000);
                    }

                    EnterField(PathsRegisterNewPurchaseFromViewPrchases.WhosalePriceField, price);
                    Thread.Sleep(4000);
                    break;
                default:
                    throw new ArgumentException($"La opción {option} no es válido");
            }

            switch (option2)
            {
                case "GUARDAR":

                    Save.Click();
                    Thread.Sleep(4000);
                    break;

                case "GUARDAR Y CLONAR":

                    SaveAndClone.Click();
                    Thread.Sleep(4000);
                    break;

                case "CANCELAR":

                    Cancel.Click();
                    Thread.Sleep(4000);
                    break;
                default:
                    throw new ArgumentException($"La opción {option2} no es válido");
            }
            Save.Click();
        }


        public void RegisterNewFamily(string option, string name, string category, string brand)
        {
            ClickButton(PathsRegisterNewPurchaseFromViewPrchases.RegisterNewConceptButton);
            Thread.Sleep(2000);

            ClickButton(PathsRegisterNewPurchaseFromViewPrchases.RegisterNewFamilyButton);
            Thread.Sleep(2000);

            //IWebElement modalfamily = driver.FindElement(By.XPath("modal-concepto"));
            IWebElement goods = driver.FindElement(By.XPath("//label[contains(text(),'Bien')]"));
            IWebElement services = driver.FindElement(By.XPath("//label[contains(text(),'Servicio')]"));

            switch (option)
            {
                case "BIEN":
                    if (!goods.Selected) // Si no está seleccionado, hacer clic
                    {
                        goods.Click();
                        Thread.Sleep(2000);
                    }
                    break;

                case "SERVICIO":
                    if (!services.Selected) // Si no está seleccionado, hacer clic
                    {
                        services.Click();
                        Thread.Sleep(2000);
                    }
                    break;

                default:
                    throw new ArgumentException($"La opción {option} no es válido");
            }

            EnterField(PathsRegisterNewPurchaseFromViewPrchases.name, name);
            Thread.Sleep(2000);

            SelectComboBox(PathsRegisterNewPurchaseFromViewPrchases.FamilyCategoryComboBox, category);
            Thread.Sleep(2000);

            switch (brand)
            {
                case "SI":

                    ClickButton(PathsRegisterNewPurchaseFromViewPrchases.BrandButton);
                    Thread.Sleep(2000);

                    ClickButton(PathsRegisterNewPurchaseFromViewPrchases.FamilySaveButton);
                    Thread.Sleep(2000);

                    break;

                case "NO":

                    ClickButton(PathsRegisterNewPurchaseFromViewPrchases.FamilySaveButton);
                    Thread.Sleep(2000);
                    break;

                default:
                    throw new ArgumentException($"La opción {brand} no es válido");
            }
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


        public void RegisterNewPurchaseFromViewPrchasesPandD(string supplier, string date)
        {
            ClickButton(PathsRegisterNewPurchaseFromViewPrchases.PurchaseButton);
            Thread.Sleep(4000);

            ClickButton(PathsRegisterNewPurchaseFromViewPrchases.ViewPurchasesButton);
            Thread.Sleep(4000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            // Hacer clic en el botón "NUEVA COMPRA"
            IWebElement nuevaCompraBtn = driver.FindElement(By.CssSelector("button[title='NUEVA COMPRA']"));
            nuevaCompraBtn.Click();

            Thread.Sleep(4000);

            EnterField(PathsRegisterNewPurchaseFromViewPrchases.SupplierField, supplier);
            Thread.Sleep(4000);

            EnterField(PathsRegisterNewPurchaseFromViewPrchases.DateField, date);
            Thread.Sleep(4000);

        }
        public void RegisterNewPurchaseFromViewPrchasesDataDocs(string comprobante, string serie, string numberdocument, string observation)
        {

            // Ubicar el elemento <select>
            IWebElement selectElement = driver.FindElement(By.XPath("//select[@ng-model='compra.TipoDeComprobante']"));

            // Crear el objeto SelectElement
            SelectElement dropdown = new SelectElement(selectElement);

            // Seleccionar el comprobante pasado como parámetro
            dropdown.SelectByText(comprobante);

            // Validar que la opción seleccionada es la esperada
            Assert.That(dropdown.SelectedOption.Text, Is.EqualTo(comprobante));
            Thread.Sleep(4000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            EnterFieldEspecific(PathsRegisterNewPurchaseFromViewPrchases.SeriesField, serie);
            Thread.Sleep(4000);

            EnterFieldEspecific(PathsRegisterNewPurchaseFromViewPrchases.NumberDocumentField, numberdocument);
            Thread.Sleep(4000);

            EnterFieldEspecific(PathsRegisterNewPurchaseFromViewPrchases.ObservationField, observation);
            Thread.Sleep(4000);
        }

        public void RegisterNewPurchaseFromViewPrchasesEandA(string optiondelivery, string optionwarehouse, string rol, string warehouse)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            IWebElement immediateRadioButton = driver.FindElement(By.Id("radioEntrega1"));
            IWebElement deferredRadioButton = driver.FindElement(By.Id("radioEntrega2"));
            IWebElement singleStorageRadioButton = driver.FindElement(By.Id("radioEntrega3"));
            IWebElement multipleStorageRadioButton = driver.FindElement(By.Id("radioEntrega4"));

            switch (optiondelivery)
            {
                case "INMEDIATA":
                    if (!immediateRadioButton.Selected) // Si no está seleccionado, hacer clic
                    {
                        immediateRadioButton.Click();
                        Thread.Sleep(4000);
                    }
                    break;
                case "DIFERIDA":
                    if (!deferredRadioButton.Selected) // Si no está seleccionado, hacer clic
                    {
                        deferredRadioButton.Click();
                        Thread.Sleep(2000);
                    }
                    break;
                default:
                    throw new ArgumentException($"La opción '{optiondelivery}' no es válida.");
            }

            switch (optionwarehouse)
            {
                case "UNO":
                    if (!singleStorageRadioButton.Selected)
                    {
                        singleStorageRadioButton.Click();
                        Thread.Sleep(2000);
                    }

                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

                    // Ubicar el elemento <select>
                    IWebElement selectElement = driver.FindElement(By.XPath("//select[@ng-model='compra.Rol']"));

                    // Crear el objeto SelectElement
                    SelectElement dropdown = new SelectElement(selectElement);

                    // Seleccionar el ROL pasado como parámetro
                    dropdown.SelectByText(rol);

                    // Validar que la opción seleccionada es la esperada
                    Assert.That(dropdown.SelectedOption.Text, Is.EqualTo(rol));
                    Thread.Sleep(4000);

                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

                    // Ubicar el elemento <select> para almacenes
                    IWebElement selectElement1 = driver.FindElement(By.XPath("//select[@ng-model='compra.Almacen']"));

                    // Crear el objeto SelectElement
                    SelectElement dropdown1 = new SelectElement(selectElement1);  // Corregido, antes se usaba selectElement incorrectamente

                    // Seleccionar el ALMACÉN pasado como parámetro
                    dropdown1.SelectByText(warehouse);

                    // Validar que la opción seleccionada es la esperada
                    Assert.That(dropdown1.SelectedOption.Text, Is.EqualTo(warehouse));
                    Thread.Sleep(4000);
                    break;

                case "VARIOS":
                    if (!multipleStorageRadioButton.Selected)
                    {
                        multipleStorageRadioButton.Click();
                        Thread.Sleep(2000);
                    }
                    break;
                default:
                    throw new ArgumentException($"La opción '{optionwarehouse}' no es válida.");
            }

            

        }



        public void RegisterNewPurchaseFromViewPrchasesTypePurchase(string optiontypepurchase)
        {
            IWebElement ExoneratedRadioButton = driver.FindElement(By.XPath("//label[contains(text(),'EXONERADAS IGV')]"));
            IWebElement GRadioButton = driver.FindElement(By.Id("radio-2"));
            IWebElement NGRadioButton = driver.FindElement(By.Id("radio-3"));
            IWebElement GyNGRadioButton = driver.FindElement(By.Id("radio-4"));

            switch (optiontypepurchase)
            {
                case "EXONERADAS":
                    if (!ExoneratedRadioButton.Selected) // Si no está seleccionado, hacer clic
                    {
                        ExoneratedRadioButton.Click();
                        Thread.Sleep(4000);
                    }
                    break;
                case "G":
                    if (!GRadioButton.Selected) // Si no está seleccionado, hacer clic
                    {
                        GRadioButton.Click();
                        Thread.Sleep(2000);
                    }
                    break;
                case "NG":
                    if (!NGRadioButton.Selected) // Si no está seleccionado, hacer clic
                    {
                        NGRadioButton.Click();
                        Thread.Sleep(2000);
                    }
                    break;
                case "G y NG":
                    if (!GyNGRadioButton.Selected) // Si no está seleccionado, hacer clic
                    {
                        GyNGRadioButton.Click();
                        Thread.Sleep(2000);
                    }
                    break;
                default:
                    throw new ArgumentException($"La opción '{optiontypepurchase}' no es válida.");
            }


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

        public void RegisterNewPurchaseFromViewPrchasesConcept(string concept, string amount)
        {

            EnterFieldModal(PathsRegisterNewPurchaseFromViewPrchases.ModalRegisterPurchase, PathsRegisterNewPurchaseFromViewPrchases.ModalConcept, concept);
            Thread.Sleep(4000);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            EnterFieldN(PathsRegisterNewPurchaseFromViewPrchases.AmountField, amount);
            Thread.Sleep(4000);

            ClickButton(PathsRegisterNewPurchaseFromViewPrchases.GAndNGIGVButton);
            Thread.Sleep(4000);
        }

        public void SelectTypePayment(string optiontypepayment)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            //Tipos de pago
            IWebElement CoRadioButton = driver.FindElement(By.Id("radio1"));
            IWebElement CrRadioButton = driver.FindElement(By.Id("radio2"));
            IWebElement CcRadioButton = driver.FindElement(By.Id("radio3"));

            //IWebElement GyNGRadioButton = driver.FindElement(By.Id("radio-4"));

            switch (optiontypepayment)
            {
                case "CONTADO":

                    if (!CoRadioButton.Selected) // Si no está seleccionado, hacer clic
                    {
                        CoRadioButton.Click();
                        Thread.Sleep(2000);
                    }
                    break;

                case "CREDITO RAPIDO":

                    if (!CrRadioButton.Selected) // Si no está seleccionado, hacer clic
                    {
                        CrRadioButton.Click();
                        Thread.Sleep(2000);
                    }
                    break;

                case "CREDITO CONFIGURADO":

                    if (!CcRadioButton.Selected) // Si no está seleccionado, hacer clic
                    {
                        CcRadioButton.Click();
                        Thread.Sleep(2000);
                    }
                    break;

                default:
                    throw new ArgumentException($"Payment type '{optiontypepayment}' is not recognized.");
            }
        }

        public void SelecTMethodPayment(string optionmethodpayment, string information)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));

            //Medios de pago
            IWebElement DepcuRadioButton = driver.FindElement(By.XPath("//label[@id='labelMedioPago-0-14']"));
            IWebElement TranfonRadioButton = driver.FindElement(By.XPath("//label[@id='labelMedioPago-0-16']"));
            IWebElement TdebRadioButton = driver.FindElement(By.XPath("//label[@id='labelMedioPago-0-18']"));
            IWebElement TcreRadioButton = driver.FindElement(By.XPath("//label[@id='labelMedioPago-0-19']"));
            IWebElement EfectivoRadioButton = driver.FindElement(By.XPath("//label[@id='labelMedioPago-0-281']"));
            IWebElement PuntosRadioButton = driver.FindElement(By.XPath("//label[@id='labelMedioPago-0-13901']"));

            //IWebElement GyNGRadioButton = driver.FindElement(By.Id("radio-4"));


            switch (optionmethodpayment)
            {
                case "DEPCU":

                    // Seleccionar el método de pago
                    DepcuRadioButton.Click();
                    Thread.Sleep(2000);

                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.block-ui-overlay")));
                    var contenedorDeposito = driver.FindElement(By.XPath("//div[contains(@class, 'box box-primary box-solid')]"));
                    var selectCuentaBancaria = contenedorDeposito.FindElement(By.Id("cuentaBancaria"));

                    /* var dropdown = new SelectElement(modalFacturacion.FindElement(comprobanteSelect));
                     dropdown.SelectByText(_comprobante);

                     EnterField(PathsRegisterNewPurchaseFromViewPrchases.InformationDepcuField, information);
                     Thread.Sleep(2000);*/
                    break;


                case "TRANFON":

                    TranfonRadioButton.Click();
                    Thread.Sleep(2000);
                    break;

                case "TDEB":

                    TdebRadioButton.Click();
                    Thread.Sleep(2000);
                    break;

                case "TCRE":

                    TcreRadioButton.Click();
                    Thread.Sleep(2000);
                    break;

                case "EF":

                    if (!EfectivoRadioButton.Selected) // Si no está seleccionado, hacer clic
                    {
                        EfectivoRadioButton.Click();
                        Thread.Sleep(4000);
                    }
                    break;

                case "PTS":
                    PuntosRadioButton.Click();
                    Thread.Sleep(2000);

                    EnterFieldEspecific(PathsRegisterNewPurchaseFromViewPrchases.PtsField, information);
                    Thread.Sleep(4000);

                    break;
                default:
                    throw new ArgumentException($"La opción {optionmethodpayment} no es válido");
            }
        }

        public void SelecTMethodPayment(string information)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

           
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
