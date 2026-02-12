using OpenQA.Selenium;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class Link : BaseControl
    {
        public Link(IWebDriver driver, By locator, int timeout) 
            : base(driver, locator, timeout) { }
    }
};
