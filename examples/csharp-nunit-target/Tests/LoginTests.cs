using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Sel2pw.Eval.Pages;

namespace Sel2pw.Eval.Tests
{
    [TestFixture]
    public class LoginTests
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
        }

        [Test]
        public void ValidLoginLandsOnInventory()
        {
            var loginPage = new LoginPage(driver);
            loginPage.Open();
            loginPage.LoginAs("standard_user", "secret_sauce");

            var inventoryPage = new InventoryPage(driver);
            Assert.That(inventoryPage.GetPageTitle(), Is.EqualTo("Products"));
            Assert.That(inventoryPage.IsInventoryVisible(), Is.True);
            Assert.That(inventoryPage.IsCartVisible(), Is.True);
        }

        [Test]
        public void LockedOutUserSeesError()
        {
            var loginPage = new LoginPage(driver);
            loginPage.Open();
            loginPage.LoginAs("locked_out_user", "secret_sauce");

            Assert.That(loginPage.GetErrorMessage(), Does.Contain("locked out"));
        }
    }
}
