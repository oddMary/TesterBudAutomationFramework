using System;
using System.Collections.Generic;
using System.Text;

namespace TesterBudAutomationFramework.Core.Config
{
    public class Settings
    {
        public string BaseUrl { get; set; } = "https://testerbud.com/practice-page-selection";
        public string Browser { get; set; } = "Chrome";

        public int TimeoutSec { get; set; } = 5;
        public int PageLoadSec { get; set; } = 3;
    }
};
