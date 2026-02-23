using OpenQA.Selenium;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class Button : BaseControl
    {
        public Button(IWebDriver driver, By locator) 
            : base(driver, locator) { }
    }
};
