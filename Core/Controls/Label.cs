using OpenQA.Selenium;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class Label : BaseControl
    {
        public Label(IWebDriver driver, By locator, int timeout)
            : base(driver, locator, timeout) { }

        public string? Text
        {
            get
            {
                var el = TryFind();
                return el?.Text?.Trim();
            }
        }
    }
};
