using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TesterBudAutomationFramework.Pages;

namespace TesterBudAutomationFramework.Core.Controls
{
    internal class TextInput : BaseControl
    {
        public TextInput(IWebDriver driver, By locator, int timeout)
            : base(driver, locator, timeout) { }

        public void SetText(string text)
        {
            var elementToClick = _wait.Clickable(_locator);
            elementToClick.Clear();
            elementToClick.SendKeys(text);
        }

        public void SetDate(DateTime date)
        {
            var formatted = date.ToString("dd-MM-yyyy");
            var elementToClick = _wait.Clickable(_locator);
            elementToClick.Clear();
            elementToClick.SendKeys(formatted);
        }

    }
}
