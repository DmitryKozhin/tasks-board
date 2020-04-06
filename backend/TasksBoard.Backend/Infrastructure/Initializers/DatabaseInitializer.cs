using System.Linq;

using Dapper;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Backend.Infrastructure.Initializers
{
    public static class DatabaseInitializer
    {
        public static void CleanUp(TasksBoardContext context)
        {
            var getTablesQuery =
                "SELECT table_name FROM information_schema.tables WHERE table_schema = " +
                $"'{TasksBoardContext.SCHEMA_NAME}';";

            var dbConnection = context.Database.GetDbConnection();

            var tableNames = dbConnection.Query<string>(getTablesQuery).ToList();

            foreach (var tableName in tableNames)
            {
                var query = $"ALTER TABLE \"{TasksBoardContext.SCHEMA_NAME}\".\"{tableName}\" DISABLE TRIGGER ALL;";
                dbConnection.Execute(query);
            }

            foreach (var tableName in tableNames)
            {
                var query = $"DELETE FROM \"{TasksBoardContext.SCHEMA_NAME}\".\"{tableName}\";";
                dbConnection.Execute(query);
            }

            foreach (var tableName in tableNames)
            {
                var query = $"ALTER TABLE \"{TasksBoardContext.SCHEMA_NAME}\".\"{tableName}\" ENABLE TRIGGER ALL;";
                dbConnection.Execute(query);
            }
        }

        public static void Migrate(string connectionString)
        {
            var migrationOptions = new DbContextOptionsBuilder<TasksBoardContext>()
                .UseNpgsql(connectionString)
                .Options;

            using var migrationContext = new TasksBoardContext(migrationOptions);
            migrationContext.Database.Migrate();
        }
    }
}