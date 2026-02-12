using NUnit.Framework;
using Serilog;
using System.Runtime.InteropServices.JavaScript;
using TesterBudAutomationFramework.Core.Loggers;

[SetUpFixture]
public class BaseTest
{
    public ScreenshotService _shots;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        LogConfig.Configure();
        Log.Information("=== Test run started ===");
        _shots = new ScreenshotService();
        CleanArtifacts();
    }

    public void CleanArtifacts()
    {
        var baseDir = AppContext.BaseDirectory;

        var paths = new[]
        {
            Path.Combine(baseDir, "logs"),
            Path.Combine(baseDir, "allure-results"),
            Path.Combine(baseDir, "allure-report")
        };

        foreach (var dir in paths)
        {
            try
            {
                if (Directory.Exists(dir))
                    Directory.Delete(dir, recursive: true);
            }
            catch { }
        }
        Directory.CreateDirectory(Path.Combine(baseDir, "logs", "Screenshots"));
        Directory.CreateDirectory(Path.Combine(baseDir, "allure-results"));

        TestContext.Progress.WriteLine("[CLEAN] Артефакты очищены перед запуском тестов.");
    }


    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Log.Information("=== Test run finished ===");
        Log.CloseAndFlush();
    }
}
