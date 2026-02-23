using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;
using TesterBudAutomationFramework.Core.Waits;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class Select : BaseControl
    {
        public Select(IWebDriver driver, By locator)
            : base(driver, locator) { }

        public void SelectOption(string text)
        {
            Log.Information("SelectByText: '{Text}' => {Locator}", text, _locator);
            var elementToClick = Wait.WaitUntilClickable(_driver, _locator, Wait.DefaultTimeout);
            SelectElement select = new SelectElement(elementToClick);
            select.SelectByText(text);
        }
    }
};
