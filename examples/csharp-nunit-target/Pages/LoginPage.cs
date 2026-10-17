using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Sel2pw.Eval.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver driver;

        [FindsBy(How = How.Id, Using = "user-name")]
        private IWebElement UsernameInput;

        [FindsBy(How = How.Id, Using = "password")]
        private IWebElement PasswordInput;

        [FindsBy(How = How.Id, Using = "login-button")]
        private IWebElement LoginButton;

        [FindsBy(How = How.CssSelector, Using = "[data-test='error']")]
        private IWebElement ErrorMessage;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public void Open()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        public void EnterUsername(string username)
        {
            UsernameInput.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            PasswordInput.SendKeys(password);
        }

        public void ClickLogin()
        {
            LoginButton.Click();
        }

        public string GetErrorMessage()
        {
            return ErrorMessage.Text;
        }

        public void LoginAs(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();
        }
    }
}
