using Serilog;

namespace TesterBudAutomationFramework.Core.Loggers
{
    public static class LogConfig
    {
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/test.log",
                              rollingInterval: RollingInterval.Day,
                              retainedFileCountLimit: 7)
                .CreateLogger();
        }
    }
}
