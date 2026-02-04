using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesterBudAutomationFramework.Core.Controls
{
    internal class Checkbox : BaseControl
    {
        public Checkbox(IWebDriver driver, By locator, int timeout) 
            : base(driver, locator, timeout) { }

    }
}
