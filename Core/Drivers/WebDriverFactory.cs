using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TesterBudAutomationFramework.Core.Drivers
{
    public class WebDriverFactory
    {
        public static IWebDriver CreateWebDriver(string browser, int timeout)
        {
            switch (browser?.Trim().ToLowerInvariant())
            {
                case "chrome":
                    return CreateChromeDriver(timeout);
                default:
                    throw new ArgumentException(
                        $"Unsupported browser: '{browser}'",
                        nameof(browser));
            }
        }

        private static IWebDriver CreateChromeDriver(int timeout)
        {
            var options = new ChromeOptions();
            var driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeout);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);
            return driver;
        }
    }
};
