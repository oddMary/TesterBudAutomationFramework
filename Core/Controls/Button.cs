using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TesterBudAutomationFramework.Core.Waits;

namespace TesterBudAutomationFramework.Core.Controls
{
    public class Button : BaseControl
    {
        public Button(IWebDriver driver, By locator, int timeout) 
            : base(driver, locator, timeout) { }

    }
};
