using Serilog;
using TesterBudAutomationFramework.Core.Constants;

namespace TesterBudAutomationFramework.Core.Loggers
{
    public static class LogConfig
    {
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(TestConstants.LOG_PATH,
                              rollingInterval: RollingInterval.Day,
                              retainedFileCountLimit: 7)
                .CreateLogger();
        }
    }
}
