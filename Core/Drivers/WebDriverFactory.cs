using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;
using TesterBudAutomationFramework.Core.Constants;

namespace TesterBudAutomationFramework.Core.Drivers
{
    public class WebDriverFactory
    {
        public static IWebDriver CreateWebDriver(string browser, int timeout)
        {
            Log.Information("WebDriverFactory.CreateWebDriver: requested browser='{Browser}', timeout={Timeout}s", browser, timeout);
            switch (browser?.Trim().ToLowerInvariant())
            {
                case TestConstants.DEFAULT_BROWSER:
                    return CreateChromeDriver(timeout);
                default:
                    Log.Error("WebDriverFactory.CreateWebDriver: unsupported browser '{Browser}'", browser);
                    throw new ArgumentException(
                        $"Unsupported browser: '{browser}'",
                        nameof(browser));
            }
        }

        private static IWebDriver CreateChromeDriver(int timeout)
        {
            var options = new ChromeOptions();

            try
            {
                var driver = new ChromeDriver(options);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeout);
                Log.Information("WebDriverFactory.CreateChromeDriver: driver created. PageLoad={PageLoad}s, ImplicitWait={ImplicitWait}s",
                                    timeout, timeout);
                return driver;

            }
            catch (WebDriverException wde)
            {
                Log.Error(wde, "WebDriverFactory.CreateChromeDriver: WebDriverException while creating ChromeDriver");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "WebDriverFactory.CreateChromeDriver: unexpected error while creating ChromeDriver");
                throw;
            }
        }
    }
};
