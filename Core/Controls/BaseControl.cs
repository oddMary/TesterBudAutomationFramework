using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.BrowsingContext;
using OpenQA.Selenium.Support.UI;
using TesterBudAutomationFramework.Core.Waits;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class BaseControl
    {
        protected IWebDriver _driver;
        protected By _locator;

        public BaseControl(IWebDriver driver, By locator, int timeout)
        {
            _driver = driver;
            _locator = locator;
        }

        public IWebElement Element => Wait.WaitUntilVisible(_driver, _locator, Wait.DefaultTimeout);

        public void Click()
        {
            var element = Wait.WaitUntilClickable(_driver, _locator, Wait.DefaultTimeout);
            element.Click();
        }

        public void ScrollToCenterAndClick()
        {
            var element = Wait.WaitUntilClickable(_driver, _locator, Wait.DefaultTimeout);
            ScrollToCenter(_driver, element);
            element.Click();
        }

        public static void ScrollToCenter(IWebDriver driver, IWebElement element)
        {
                var js = (IJavaScriptExecutor)driver;
                js.ExecuteScript(@"
                const el = arguments[0];
                const rect = el.getBoundingClientRect();
                const y = rect.top + window.pageYOffset - (window.innerHeight / 2) + (rect.height / 2);
                window.scrollTo({ top: y, behavior: 'instant' });
            ", element);

        }


        protected IWebElement? TryFind()
        {
            var elements = _driver.FindElements(_locator);
            return elements.Count > 0 ? elements[0] : null;
        }

        public bool IsPresent() => _driver.FindElements(_locator).Count > 0;

        public bool IsVisible()
        {
            var el = TryFind();
            try
            {
                return el != null && el.Displayed;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }


        public bool WaitUntilVisible(TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(_driver, timeout ?? TimeSpan.FromSeconds(5));
            try
            {
                return wait.Until(d =>
                {
                    var e = TryFind();
                    return e != null && e.Displayed;
                });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

    }
};
