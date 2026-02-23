using OpenQA.Selenium;
using Serilog;
using System.Threading;
using TesterBudAutomationFramework.Core.Config;
using TesterBudAutomationFramework.Core.Controls;
using TesterBudAutomationFramework.Core.Waits;

namespace TesterBudAutomationFramework.Pages
{
    public class BasePage
    {
        protected IWebDriver _driver;

        private Link FlightBookingPage => FindComponent<Link>(By.CssSelector("a[href*='flight']"));

        public BasePage(IWebDriver driver)
        {
            _driver = driver;
            Log.Debug("BasePage: created page object {Page} with driver session", GetType().Name);
        }

        protected T FindComponent<T>(By locator) where T : BaseControl
        {
            Log.Debug("BasePage.FindComponent: locating {Control} by {Locator}", typeof(T).Name, locator);
            Wait.WaitUntilVisible(_driver, locator);
            var instance = Activator.CreateInstance(typeof(T), _driver, locator)
                ?? throw new InvalidOperationException($"Failed to create instance of {typeof(T).Name}");

            return (T)instance;

        }

        protected List<T> FindComponents<T>(By locator) where T : BaseControl
        {
            Log.Debug("BasePage.FindComponents: locating multiple {Control} by {Locator}", typeof(T).Name, locator);
            Wait.WaitUntilVisible(_driver, locator);
            return _driver.FindElements(locator)
                .Select(_ =>
                    Activator.CreateInstance(typeof(T), _driver, locator) as T
                    ?? throw new InvalidOperationException($"Failed to create {typeof(T).Name}"))
                .ToList();
        }

        public void OpenAutomationTestingPracticeHubPage()
        {
            Log.Information("Navigate To BaseUrl: {Url}", TestConfig.CurrentSetting.BaseUrl);

            try
            {
                _driver.Navigate().GoToUrl(TestConfig.CurrentSetting.BaseUrl);
                Log.Debug("Navigation Success: {Url}", TestConfig.CurrentSetting.BaseUrl);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Navigation FAILED for {Url}", TestConfig.CurrentSetting.BaseUrl);
                throw;
            }
        }

        public FlightBookingPage GoToFlightBookingPage()
        {
            Log.Information("GoToFlightBookingPage: opening hub page...");
            OpenAutomationTestingPracticeHubPage();

            Log.Information("GoToFlightBookingPage: clicking Flight Booking link");
            try
            {
                FlightBookingPage.ScrollToCenterAndClick();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GoToFlightBookingPage: FAILED while clicking Flight Booking link");
                throw;
            }

            Log.Information("GoToFlightBookingPage: navigation success, returning FlightBookingPage");
            return new FlightBookingPage(_driver);
        }
    }
};
