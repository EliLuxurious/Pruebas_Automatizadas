using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SigesCore.Features.Ventas;
using SigesCore.Hooks.XPaths;
using SigesCore.Hooks.Utility;

namespace SigesCore.Hooks.LoginPage
{
    public class LoginPage
    {
        private IWebDriver driver;
        UtilityVenta utilityPage;
        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilityPage = new UtilityVenta(driver);
        }

        public void LoginToApplication(string email, string password)
        {
            utilityPage.ClearAndSetInputField(Login.txtEmail, email);
            utilityPage.ClearAndSetInputField(Login.txtPassword, password);
            utilityPage.ClickButton(Login.btnSignIn);
            utilityPage.ClickButton(Login.btnConfirm);
        }
    }
}