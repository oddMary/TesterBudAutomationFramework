using OpenQA.Selenium;
using Serilog;
using TesterBudAutomationFramework.Core.Waits;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class BaseControl
    {
        protected IWebDriver _driver;
        protected By _locator;

        public BaseControl(IWebDriver driver, By locator)
        {
            _driver = driver;
            _locator = locator;
        }

        public IWebElement Element => Wait.WaitUntilVisible(_driver, _locator, Wait.DefaultTimeout);

        public void Click()
        {
            var element = Wait.WaitUntilClickable(_driver, _locator, Wait.DefaultTimeout);
            element.Click();
            Log.Information("Click: clicked => {Locator}", _locator);
        }

        public void ScrollToCenterAndClick()
        {
            var element = Wait.WaitUntilClickable(_driver, _locator, Wait.DefaultTimeout);

            try
            {
                ScrollToCenter(_driver, element);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "ScrollToCenter: failed for => {Locator}", _locator);
            }

            element.Click();
            Log.Information("ScrollToCenterAndClick: clicked => {Locator}", _locator);

        }

        public void ScrollToCenter(IWebDriver driver, IWebElement element)
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
    }
};
