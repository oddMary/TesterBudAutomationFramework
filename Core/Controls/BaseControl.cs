using OpenQA.Selenium;
using TesterBudAutomationFramework.Core.Waits;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class BaseControl
    {
        protected IWebDriver _driver;
        protected By _locator;
        protected Wait _wait;

        public BaseControl(IWebDriver driver, By locator, int timeout)
        {
            _driver = driver;
            _locator = locator;
            _wait = new Wait(driver, timeout);
        }

        public IWebElement Element => _driver.FindElement(_locator);

        public void Click()
        {
            ScrollToCenter(_driver, Element);
            var elementToClick = _wait.Clickable(_locator);
            elementToClick.Click();
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

    }
}
