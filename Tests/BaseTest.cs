using Allure.Net.Commons;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Serilog;
using TesterBudAutomationFramework.Core.Config;
using TesterBudAutomationFramework.Core.Constants;
using TesterBudAutomationFramework.Core.Drivers;
using TesterBudAutomationFramework.Core.Loggers;
using TesterBudAutomationFramework.Services;

public class BaseTest
{
    protected FlightBookingService _flightService;
    protected PaymentModalService _paymentService;
    protected IWebDriver _driver;

    protected DateTime _departureDate;
    protected DateTime _returnDate;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        LogConfig.Configure();
        Log.Information("=== Test run started ===");
        CleanArtifacts();
    }

    [SetUp]
    public void SetUp()
    {
        var config = TestConfig.CurrentSetting;
        _driver = WebDriverFactory.CreateWebDriver(config.Browser, config.PageLoadSec);
        _flightService = new FlightBookingService(_driver);
        _paymentService = new PaymentModalService(_driver);
        _departureDate = DateTime.Today.AddDays(7);
        _returnDate = DateTime.Today.AddDays(8);
    }

    [TearDown]
    public void AfterEach()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Passed)
        {
            try
            {
                var testName = TestContext.CurrentContext.Test.Name;
                var path = Save(_driver, testName);
                AllureApi.AddAttachment(testName, TestConstants.SCREENSHOTS_ATTACHMENT_TYPE, path);
            }
            catch { }
        }
        _driver.Dispose();
    }    

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Log.Information("=== Test run finished ===");
        Log.CloseAndFlush();
    }
    public void CleanArtifacts()
    {
        var baseDir = AppContext.BaseDirectory;

        var paths = new[]
        {
            Path.Combine(baseDir, TestConstants.LOGS_FOLDER),
            Path.Combine(baseDir, TestConstants.ALLURE_RESULTS_FOLDER),
            Path.Combine(baseDir, TestConstants.ALLURE_REPORT_FOLDER)
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
        Directory.CreateDirectory(Path.Combine(baseDir, TestConstants.LOGS_FOLDER, TestConstants.SCREENSHOTS_FOLDER));
        Directory.CreateDirectory(Path.Combine(baseDir, TestConstants.ALLURE_REPORT_FOLDER));
        Directory.CreateDirectory(Path.Combine(baseDir, TestConstants.ALLURE_RESULTS_FOLDER));
    }

    public string? Save(IWebDriver driver, string testName)
    {
        var baseDir = AppContext.BaseDirectory;
        Directory.CreateDirectory(baseDir);

        try
        {
            if (driver is not ITakesScreenshot taker)
            {
                Log.Warning("ScreenshotHelper.Save: driver does not implement ITakesScreenshot. Skipping.");
                return null;
            }

            var date = DateTime.Now.ToString(TestConstants.SCREENSHOT_DATE_FORMAT);
            var time = DateTime.Now.ToString(TestConstants.SCREENSHOT_TIME_FORMAT);

            var folder = Path.Combine(baseDir, date, testName);
            Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, $"{time}.{TestConstants.IMG_FORMAT}");
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(path);

            Log.Information("📸 Screenshot saved: {Path}", path);
            return path;

        }
        catch (Exception ex)
        {
            Log.Error(ex, "ScreenshotHelper.Save: failed to save screenshot for test {TestName}", testName);
            return null;
        }
    }
}
