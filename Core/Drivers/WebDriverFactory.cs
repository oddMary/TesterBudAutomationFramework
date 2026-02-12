using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;

namespace TesterBudAutomationFramework.Core.Drivers
{
    public class WebDriverFactory
    {
        public static IWebDriver CreateWebDriver(string browser, int timeout)
        {
            Log.Information("WebDriverFactory.CreateWebDriver: requested browser='{Browser}', timeout={Timeout}s", browser, timeout);
            switch (browser?.Trim().ToLowerInvariant())
            {
                case "chrome":
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
            Log.Information("WebDriverFactory.CreateChromeDriver: initializing Chrome with timeout={Timeout}s", timeout);
            var options = new ChromeOptions();

            try
            {
                var driver = new ChromeDriver(options);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeout);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);
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
