using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.BrowsingContext;
using OpenQA.Selenium.DevTools.V142.DOM;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class Select : BaseControl
    {
        public Select(IWebDriver driver, By locator, int timeout)
            : base(driver, locator, timeout) { }

        public void SelectOption(string text)
        {
            var elementToClick = _wait.Clickable(_locator);
            SelectElement select = new SelectElement(elementToClick);
            select.SelectByText(text);
        }
    }
}
