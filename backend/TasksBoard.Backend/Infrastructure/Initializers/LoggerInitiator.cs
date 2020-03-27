using System;
using System.IO;

using Serilog;
using Serilog.Events;

namespace TasksBoard.Backend.Infrastructure.Initializers
{
    /// <summary>
    /// Helper class with logic for logger initialize.
    /// </summary>
    public static class LoggerInitiator
    {
        /// <summary>
        /// Initialize logger.
        /// </summary>
        public static void Init()
        {
            var appSetting = new AppSettings(AppSettings.SourceType.EnvironmentVariables);
            if (string.IsNullOrWhiteSpace(appSetting.LogDirectory))
                throw new InvalidOperationException("Unknown directory to log");

            var totalLogFile = Path.Combine(appSetting.LogDirectory, "total.log");
            var dailyLogFile = Path.Combine(appSetting.LogDirectory, "daily.log");

            InitInner(totalLogFile, dailyLogFile);
        }

        /// <summary>
        /// Add logger entity to <see cref="Log.Logger"/> property.
        /// </summary>
        /// <param name="totalLogFile">Path to total logs file.</param>
        /// <param name="dailyLogFile">Path to daily logs file.</param>
        private static void InitInner(string totalLogFile, string dailyLogFile)
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .WriteTo.File(totalLogFile, rollOnFileSizeLimit: true)
                            .WriteTo.File(dailyLogFile, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                            .CreateLogger();
        }
    }
}