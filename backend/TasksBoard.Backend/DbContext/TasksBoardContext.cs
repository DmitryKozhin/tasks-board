using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.DbContext.Tables;

namespace TasksBoard.Backend.DbContext
{
    public sealed class TasksBoardContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public TasksBoardContext()
        {
        }

        public TasksBoardContext(DbContextOptions options, int databaseTimeoutSec)
            : base(options)
        {
            Database.SetCommandTimeout(databaseTimeoutSec);
        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<ColumnTask> ColumnTasks { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<BoardColumn> BoardColumns { get; set; }
        public DbSet<Column> Columns { get; set; }
    }
}