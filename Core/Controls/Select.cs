using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.BrowsingContext;
using OpenQA.Selenium.DevTools.V142.DOM;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TesterBudAutomationFramework.Core.Waits;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class Select : BaseControl
    {
        //IWebDriver _driver;

        public Select(IWebDriver driver, By locator, int timeout)
            : base(driver, locator, timeout) 
        {
            //_driver = driver;
        }

        public void SelectOption(string text)
        {
            var elementToClick = Wait.WaitUntilClickable(_driver, _locator, Wait.DefaultTimeout);
            SelectElement select = new SelectElement(elementToClick);
            select.SelectByText(text);
        }
    }
};
