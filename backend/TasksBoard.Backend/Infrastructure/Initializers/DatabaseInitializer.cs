using Microsoft.EntityFrameworkCore;

using Serilog;

using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Backend.Infrastructure.Initializers
{
    public static class DatabaseInitializer
    {
        public static void Init(string connectionString)
        {
            Log.Information("Database init started");
            Migrate(connectionString, true);
            Log.Information("Database init complete");
        }

        public static void Migrate(string connectionString, bool createDefaultData = false)
        {
            var migrationOptions = new DbContextOptionsBuilder<TasksBoardContext>()
                .UseNpgsql(connectionString)
                .Options;

            using var migrationContext = new TasksBoardContext(migrationOptions);
            migrationContext.Database.Migrate();
        }
    }
}