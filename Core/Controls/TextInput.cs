using OpenQA.Selenium;
using Serilog;
using TesterBudAutomationFramework.Core.Constants;
using TesterBudAutomationFramework.Core.Waits;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class TextInput : BaseControl
    {
        public TextInput(IWebDriver driver, By locator)
            : base(driver, locator) { }

        public void SetText(string text)
        {
            Log.Information("SetText: '{Text}' => {Locator}", text, _locator);
            var elementToClick = Wait.WaitUntilClickable(_driver, _locator, Wait.DefaultTimeout);
            elementToClick.Clear();
            elementToClick.SendKeys(text);
        }

        public void SetFormattedDate(DateTime date)
        {
            var formatted = date.ToString(TestConstants.DATE_FORMAT);
            Log.Information("SetFormattedDate: '{Date}' (format \"MM-dd-yyyy\") => {Locator}", formatted, _locator);
            SetText(formatted);
        }
    }
};
