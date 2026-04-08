using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTAURANTE.Hoks.Pages.Atencion
{
    public class AtencionPage
    {
        private IWebDriver driver;
        Utilities utilities;


        public AtencionPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
        }


        // TIPO DE ATENCION
        private By _atencionConMesa = By.XPath("//label[@id='labelatencionconmesa']");
        private By _atencionSinMesa = By.Id("labelatencionsinmesa");

        // AMBIENTE
        private By _ambientePrincipal = By.Id("ambienteconmesa-0");
        private By _ambienteReservaciones = By.Id("ambienteconmesa-1");
        private By fieldLocator;

        public void tipoAtencion(string _atencion)
        {
            switch (_atencion)
            {
                case "CON MESA":
                    utilities.buttonClickeable(_atencionConMesa);
                    return;

                case "SIN MESA":
                    utilities.buttonClickeable(_atencionSinMesa);

                    fieldLocator = _atencionSinMesa;
                    break;

                default:
                    throw new ArgumentException($"El {_atencion} no es válido");
            }

            utilities.buttonClickeable(fieldLocator);
            // Thread.Sleep(4000);
        }

        




    }
}
