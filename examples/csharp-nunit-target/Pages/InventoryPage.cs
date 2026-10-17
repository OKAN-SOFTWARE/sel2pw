using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Sel2pw.Eval.Pages
{
    public class InventoryPage
    {
        private readonly IWebDriver driver;

        [FindsBy(How = How.CssSelector, Using = ".title")]
        private IWebElement PageTitle;

        [FindsBy(How = How.CssSelector, Using = ".inventory_item")]
        private IWebElement FirstInventoryItem;

        [FindsBy(How = How.Id, Using = "shopping_cart_container")]
        private IWebElement CartLink;

        public InventoryPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public string GetPageTitle()
        {
            return PageTitle.Text;
        }

        public bool IsInventoryVisible()
        {
            return FirstInventoryItem.Displayed;
        }

        public bool IsCartVisible()
        {
            return CartLink.Displayed;
        }
    }
}
