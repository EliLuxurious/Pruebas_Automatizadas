using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SIGESLogin.Pages;

namespace SIGESLogin
{
    public class LoginTests
    {
        IWebDriver driver;
        LoginPage loginPage;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://testcore.sigesonline.com/");

            loginPage = new LoginPage(driver);
        }

        [Test]
        public void LoginTest_POM()
        {
            loginPage.Login("usuario@siges.com", "12345");

            Assert.That(driver.Title, Is.EqualTo("SIGES - Dashboard"));
        }

        [TearDown]
        public void Close()
        {
            driver.Quit();
        }
    }
}
