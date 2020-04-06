using System.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using TasksBoard.Backend.Domain;

namespace TasksBoard.Backend.Infrastructure.Context
{
    public sealed class TasksBoardContext : DbContext
    {
        public const string SCHEMA_NAME = "TasksBoardContext";

        private IDbContextTransaction _currentTransaction;

        public TasksBoardContext()
        {
        }

        public TasksBoardContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<UserBoard> UserBoards { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var appSetting = new AppSettings(AppSettings.SourceType.EnvironmentVariables);
        //    optionsBuilder.UseNpgsql(appSetting.MasterDatabaseConnectionString);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SCHEMA_NAME);

            modelBuilder.ApplyConfiguration(new UserBoardEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserTaskEntityTypeConfiguration());
        }

        #region Transaction Handling

        public void BeginTransaction()
        {
            if (_currentTransaction != null) return;

            if (!Database.IsInMemory()) _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void CommitTransaction()
        {
            try
            {
                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #endregion
    }
}