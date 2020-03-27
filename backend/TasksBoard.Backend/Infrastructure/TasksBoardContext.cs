using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Domain;

namespace TasksBoard.Backend.Infrastructure
{
    public sealed class TasksBoardContext : DbContext
    {
        public TasksBoardContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Column> Columns { get; set; }

        public void BeginTransaction()
        {
            throw new System.NotImplementedException();
        }

        public void CommitTransaction()
        {
            throw new System.NotImplementedException();
        }

        public void RollbackTransaction()
        {
            throw new System.NotImplementedException();
        }
    }
}