using OpenQA.Selenium;
using TesterBudAutomationFramework.Core.Waits;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class Label : BaseControl
    {
        public Label(IWebDriver driver, By locator)
            : base(driver, locator) { }

        public string? Text
        {
            get
            {
                var el = Wait.WaitUntilClickable(_driver, _locator);
                return el?.Text?.Trim();
            }
        }
    }
};
