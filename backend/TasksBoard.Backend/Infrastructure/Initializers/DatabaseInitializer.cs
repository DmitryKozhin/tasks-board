using Microsoft.EntityFrameworkCore;

using Serilog;

using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Backend.Infrastructure.Initializers
{
    public static class DatabaseInitializer
    {
        public static void Migrate(string connectionString)
        {
            var migrationOptions = new DbContextOptionsBuilder<TasksBoardContext>()
                .UseNpgsql(connectionString)
                .Options;

            using (var migrationContext = new TasksBoardContext(migrationOptions))
            {
                migrationContext.Database.Migrate();
            }
        }
    }
}