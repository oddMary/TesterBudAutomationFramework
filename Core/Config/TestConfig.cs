using OpenQA.Selenium.DevTools.V142.ServiceWorker;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace TesterBudAutomationFramework.Core.Config
{
    public class TestConfig
    {
        private static Settings? _settings;

        public static Settings? CurrentSetting => _settings ?? SetupSettings();

        private static Settings SetupSettings()
        {
            var basePath = AppContext.BaseDirectory;
            var path = Path.Combine(basePath, "appsettings.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File {path} is not found");
            }

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }
    }
};
