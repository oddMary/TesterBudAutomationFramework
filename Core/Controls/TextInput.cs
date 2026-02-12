using OpenQA.Selenium;
using Serilog;
using TesterBudAutomationFramework.Core.Waits;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class TextInput : BaseControl
    {
        public TextInput(IWebDriver driver, By locator, int timeout)
            : base(driver, locator, timeout) { }

        public void SetText(string text)
        {
            Log.Information("SetText: '{Text}' => {Locator}", text, _locator);
            var elementToClick = Wait.WaitUntilClickable(_driver, _locator, Wait.DefaultTimeout);
            elementToClick.Clear();
            elementToClick.SendKeys(text);
            Log.Debug("SetText: keys sent to {Locator}", _locator);
        }

        public void SetFormattedDate(DateTime date)
        {
            var formatted = date.ToString("MM-dd-yyyy");
            Log.Information("SetFormattedDate: '{Date}' (format \"MM-dd-yyyy\") => {Locator}", formatted, _locator);
            var elementToClick = Wait.WaitUntilClickable(_driver, _locator, Wait.DefaultTimeout);
            elementToClick.Clear();
            elementToClick.SendKeys(formatted);
            Log.Debug("SetFormattedDate: keys sent to {Locator}", _locator);
        }
    }
};
