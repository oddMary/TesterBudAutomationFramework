using OpenQA.Selenium;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class Checkbox : BaseControl
    {
        public Checkbox(IWebDriver driver, By locator, int timeout) 
            : base(driver, locator, timeout) { }
    }
};
