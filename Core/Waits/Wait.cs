using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace TesterBudAutomationFramework.Core.Waits
{
    public class Wait
    {
        private readonly WebDriverWait _wait;

        public Wait(IWebDriver driver, int timeout)
        {
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
        }

        public IWebElement Visible(By locator) => _wait.Until(ExpectedConditions.ElementIsVisible(locator));

        public IWebElement Clickable(By locator) => _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
    }
}
