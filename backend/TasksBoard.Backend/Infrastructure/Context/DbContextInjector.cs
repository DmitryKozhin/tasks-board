using Microsoft.EntityFrameworkCore;

namespace TasksBoard.Backend.Infrastructure.Context
{
    public class DbContextInjector : IDbContextInjector
    {
        private readonly string _masterReplicaConnectionString;
        private readonly string _syncReplicaConnectionString;

        private TasksBoardContext _masterDbContext;
        private TasksBoardContext _syncDbContext;

        public DbContextInjector(
            string masterReplicaConnectionString,
            string syncReplicaConnectionString
        )
        {
            _masterReplicaConnectionString = masterReplicaConnectionString;
            _syncReplicaConnectionString = syncReplicaConnectionString;
        }

        public TasksBoardContext ReadContext
        {
            get
            {
                if (_syncDbContext != null)
                    return _syncDbContext;

                _syncDbContext = CreateContext(_syncReplicaConnectionString);
                return _syncDbContext;
            }
        }

        public TasksBoardContext WriteContext
        {
            get
            {
                if (_masterDbContext != null)
                    return _masterDbContext;

                _masterDbContext = CreateContext(_masterReplicaConnectionString);
                return _masterDbContext;
            }
        }

        public void Dispose()
        {
            _masterDbContext?.Dispose();
            _syncDbContext?.Dispose();
        }

        private TasksBoardContext CreateContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TasksBoardContext>();
            optionsBuilder.UseNpgsql(connectionString);
            var result = new TasksBoardContext(optionsBuilder.Options);
            return result;
        }
    }
}