using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using Serilog;

using TasksBoard.Backend.Infrastructure.Initializers;

namespace TasksBoard.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggerInitiator.Init();

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseUrls($"http://+:5000")
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();

            host.Run();
        }
    }
}