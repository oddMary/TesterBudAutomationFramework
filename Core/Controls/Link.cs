using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesterBudAutomationFramework.Core.Controls
{
    internal class Link : BaseControl
    {
        public Link(IWebDriver driver, By locator, int timeout) 
            : base(driver, locator, timeout) { }

    }
}
