using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Serilog;
using TesterBudAutomationFramework.Core.Config;


namespace TesterBudAutomationFramework.Core.Waits
{
    public static class Wait
    {
        public static int DefaultTimeout => TestConfig.CurrentSetting.TimeoutSec;

        public static IWebElement WaitUntilVisible(this IWebDriver webDriver, By elementLocator, int timeout = 10)
        {
            Log.Debug("WaitUntilVisible: waiting for {Locator} for {Timeout}s", elementLocator, timeout);
            try
            {                
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeout));
                var element = wait.Until(ExpectedConditions.ElementIsVisible(elementLocator));

                Log.Debug("WaitUntilVisible: element is visible => {Locator}", elementLocator);
                return element;
            }

            catch (WebDriverTimeoutException ex)
            {
                Log.Warning(ex, "WaitUntilVisible: timeout waiting for {Locator}", elementLocator);
                throw new WebDriverTimeoutException(
                    $"Element with locator '{elementLocator}' was not found after timeout of {timeout}s.", ex);

            }
            catch (NoSuchElementException ex)
            {
                Log.Warning(ex, "WaitUntilVisible: no such element {Locator}", elementLocator);
                throw new NoSuchElementException(
                    $"Element with locator '{elementLocator}' was not found.", ex);
            }
        }

        public static IWebElement WaitUntilClickable(this IWebDriver webDriver, By elementLocator, int timeout = 10)
        {
            Log.Debug("WaitUntilClickable: waiting for {Locator} for {Timeout}s", elementLocator, timeout);
            try
            {   var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeout));
                var element = wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));

                Log.Debug("WaitUntilClickable: element is clickable => {Locator}", elementLocator);
                return element;

            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Warning(ex, "WaitUntilClickable: timeout waiting for {Locator}", elementLocator);
                throw new WebDriverTimeoutException (
                    $"Element with locator: '{elementLocator}' was not found after Timeout limit.");
            }
            catch (NoSuchElementException ex)
            {
                Log.Warning(ex, "WaitUntilClickable: no such element {Locator}", elementLocator);
                throw new NoSuchElementException(
                    $"Element with locator: '{elementLocator}' was not found.");
            }
        }
    }
};
