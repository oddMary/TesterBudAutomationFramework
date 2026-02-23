using System.Text.Json;
using TesterBudAutomationFramework.Core.Constants;

namespace TesterBudAutomationFramework.Core.Config
{
    public class TestConfig
    {
        private static Settings? _settings;

        public static Settings? CurrentSetting => _settings ?? SetupSettings();

        private static Settings SetupSettings()
        {
            var basePath = AppContext.BaseDirectory;
            var path = Path.Combine(basePath, TestConstants.JSON_FILE_NAME);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File {path} is not found");
            }

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }
    }
};
