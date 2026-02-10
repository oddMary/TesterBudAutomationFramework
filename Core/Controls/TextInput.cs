using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TesterBudAutomationFramework.Core.Waits;
using TesterBudAutomationFramework.Pages;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class TextInput : BaseControl
    {
        public TextInput(IWebDriver driver, By locator, int timeout)
            : base(driver, locator, timeout) { }

        public void SetText(string text)
        {
            var elementToClick = Wait.WaitUntilClickable(_driver, _locator, Wait.DefaultTimeout);
            elementToClick.Clear();
            elementToClick.SendKeys(text);
        }

        public void SetFormattedDate(DateTime date)
        {
            var formatted = date.ToString("MM-dd-yyyy");
            var elementToClick = Wait.WaitUntilClickable(_driver, _locator, Wait.DefaultTimeout);
            elementToClick.Clear();
            elementToClick.SendKeys(formatted);
        }

    }
};
