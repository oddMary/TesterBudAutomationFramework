using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TesterBudAutomationFramework.Core.Config;


namespace TesterBudAutomationFramework.Core.Waits
{
    public static class Wait
    {
        //public IWebElement Visible(By locator) => _wait.Until(ExpectedConditions.ElementIsVisible(locator));

        //public IWebElement Clickable(By locator) => _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        public static int DefaultTimeout => TestConfig.CurrentSetting.TimeoutSec;

        public static IWebElement WaitUntilVisible(this IWebDriver webDriver, By elementLocator, int timeout = 10)
        {
            //try
            //{
                //StaticLogger.Logger.LogTrace($"Waiting for element {elementLocator} to be visible.");
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeout));
                return wait.Until(ExpectedConditions.ElementIsVisible(elementLocator));
            //}
            //catch (WebDriverTimeoutException)
            //{
            //    throw new Exception()<WebDriverTimeoutException>($"Element with locator: '{elementLocator}' was not found after Timeout limit.");
            //}
            //catch (NoSuchElementException)
            //{
            //    throw new Exception()<NoSuchElementException>($"Element with locator: '{elementLocator}' was not found.");
            //}
        }

        public static IWebElement WaitUntilClickable(this IWebDriver webDriver, By elementLocator, int timeout = 10)
        {
            //try
            //{
                //StaticLogger.Logger.LogTrace($"Waiting for element {elementLocator} to be visible.");
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeout));
                return wait.Until(ExpectedConditions.ElementIsVisible(elementLocator));
            //}
            //catch (WebDriverTimeoutException)
            //{
            //    throw new SeleniumException<WebDriverTimeoutException>($"Element with locator: '{elementLocator}' was not found after Timeout limit.");
            //}
            //catch (NoSuchElementException)
            //{
            //    throw new SeleniumException<NoSuchElementException>($"Element with locator: '{elementLocator}' was not found.");
            //}
        }
    }
};
