using System;
using System.IO;

using Serilog;
using Serilog.Events;

namespace TasksBoard.Backend
{
    /// <summary>
    /// Вспомогательный класс для настройки логгера.
    /// </summary>
    public static class LoggerInitiator
    {
        /// <summary>
        /// Инициализация логгера.
        /// Вызывать как можно раньше, чтобы можно было логировать запуск основного сервиса.
        /// </summary>
        public static void Init()
        {
            var logDir = Environment.GetEnvironmentVariable("LOG_DIR");
            if (string.IsNullOrWhiteSpace(logDir))
            {
                throw new InvalidOperationException("Не задана переменная окружения LOG_DIR.");
            }

            var totalLogFile = Path.Combine(logDir, "total.log");
            var dailyLogFile = Path.Combine(logDir, "daily.log");

            InitInner(totalLogFile, dailyLogFile);
        }

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