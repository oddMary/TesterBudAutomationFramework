using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TesterBudAutomationFramework.Core.Config;
using TesterBudAutomationFramework.Core.Controls;

namespace TesterBudAutomationFramework.Pages
{
    public class BasePage
    {
        protected IWebDriver _driver;

        private Link FlightBookingPage => FindComponent<Link>(By.CssSelector("a[href*='flight']"));

        public BasePage(IWebDriver driver)
        {
            _driver = driver;
        }

        protected T FindComponent<T>(By locator) where T : BaseControl
        {
            return (T)Activator.CreateInstance(typeof(T), _driver, locator, TestConfig.CurrentSetting.TimeoutSec);
        }

        protected List<T> FindComponents<T>(By locator) where T : BaseControl
        {
            return _driver.FindElements(locator)
                .Select(item => (T)Activator.CreateInstance(typeof(T), _driver, locator, TestConfig.CurrentSetting.TimeoutSec))
                .ToList();
        }

        public void OpenAutomationTestingPracticeHubPage() => 
            _driver.Navigate().GoToUrl(TestConfig.CurrentSetting.BaseUrl);

        public FlightBookingPage GoToFlightBookingPage()
        {
            OpenAutomationTestingPracticeHubPage();
            FlightBookingPage.ScrollToCenterAndClick();
            return new FlightBookingPage(_driver);
        }
    }
};
