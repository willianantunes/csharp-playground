using System.IO;
using System.Threading.Tasks;
using CliFx;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using TicTacToeCSharpPlayground.EntryCommands;

namespace TicTacToeCSharpPlayground
{
    public class Program
    {
        internal static IConfiguration Configuration { get; set; }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        private static async Task<int> InitiateCommandLineInterfaceProgram()
        {
            // Know more at: https://github.com/Tyrrrz/CliFx
            return await new CliApplicationBuilder()
                .SetExecutableName("TicTacToeCSharpPlayground")
                .AddCommandsFromThisAssembly()
                .Build()
                .RunAsync();
        }

        private static async Task<int> Main(string[] args)
        {
            Configuration = BuildConfiguration();

            ConfigureLogger();

            return await InitiateCommandLineInterfaceProgram();
        }

        public static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            // It's here because of EF: https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-application-services
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<ApiCommand.Startup>(); });
    }
}
