using System;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using TasksBoard.Backend;
using TasksBoard.Backend.Infrastructure.Context;
using TasksBoard.Backend.Infrastructure.Initializers;

using Xunit;
using Xunit.Abstractions;

namespace TasksBoard.Tests
{
    [Collection("Serialize")]
    public class BackendTestBase : IDisposable
    {
        private static readonly IConfiguration Config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        private readonly AppSettings _appSettingsForTests =
            new AppSettings(AppSettings.SourceType.TestEnvironmentVariables);

        private readonly IServiceScopeFactory _scopeFactory;

        protected readonly IDbContextInjector ContextInjector;

        public BackendTestBase(ITestOutputHelper output)
        {
            var startup = new Startup(Config, AppSettings.SourceType.TestEnvironmentVariables);
            var services = new ServiceCollection();

            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            ContextInjector = provider.GetService<IDbContextInjector>();
            DatabaseInitializer.CleanUp(ContextInjector.WriteContext);

            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.TestOutput(output)
                .WriteTo.Console();

            var logFileName = _appSettingsForTests.LogDirectory;
            if (!string.IsNullOrEmpty(logFileName))
                loggerConfiguration.WriteTo.File(logFileName, rollingInterval: RollingInterval.Day);

            Log.Logger = loggerConfiguration.CreateLogger();
            _scopeFactory = provider.GetService<IServiceScopeFactory>();
        }

        public void Dispose()
        {
            DatabaseInitializer.CleanUp(ContextInjector.WriteContext);
            ContextInjector?.Dispose();
        }

        public Task ExecuteDbContextAsync(Func<TasksBoardContext, Task> action)
        {
            return ExecuteScopeAsync(sp => action(ContextInjector.WriteContext));
        }

        public Task<T> ExecuteDbContextAsync<T>(Func<TasksBoardContext, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(ContextInjector.WriteContext));
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using var scope = _scopeFactory.CreateScope();
            await action(scope.ServiceProvider);
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using var scope = _scopeFactory.CreateScope();
            return await action(scope.ServiceProvider);
        }

        public Task InsertAsync(params object[] entities)
        {
            return ExecuteDbContextAsync(db =>
            {
                foreach (var entity in entities)
                {
                    db.Add(entity);
                }

                return db.SaveChangesAsync();
            });
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }
    }
}