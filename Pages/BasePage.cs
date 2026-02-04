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

        protected T FindComponent<T>(By by) where T : BaseControl
        {
            if (typeof(T) == typeof(Button))
                return new Button(_driver, by, TestConfig.CurrentSetting.TimeoutSec) as T;

            if (typeof(T) == typeof(Checkbox))
                return new Checkbox(_driver, by, TestConfig.CurrentSetting.TimeoutSec) as T;

            if (typeof(T) == typeof(Select))
                return new Select(_driver, by, TestConfig.CurrentSetting.TimeoutSec) as T;

            if (typeof(T) == typeof(Link))
                return new Link(_driver, by, TestConfig.CurrentSetting.TimeoutSec) as T;

            if (typeof(T) == typeof(TextInput))
                return new TextInput(_driver, by, TestConfig.CurrentSetting.TimeoutSec) as T;

            throw new NotImplementedException($"Component type {typeof(T).Name} not supported");
        }


        public void OpenAutomationTestingPracticeHubPage() => 
            _driver.Navigate().GoToUrl(TestConfig.CurrentSetting.BaseUrl);

        public FlightBookingPage GoToFlightBookingPage()
        {
            OpenAutomationTestingPracticeHubPage();
            FlightBookingPage.Click();
            return new FlightBookingPage(_driver);
        }
    }
}
