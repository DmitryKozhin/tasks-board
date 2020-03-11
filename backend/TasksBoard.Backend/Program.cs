using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace TasksBoard.Backend
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .UseSerilog();
        }

        public static void Main(string[] args)
        {
            LoggerInitiator.Init();
            CreateHostBuilder(args).Build().Run();
        }
    }
}