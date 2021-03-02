using CheckerPrice.ConsoleApp.Models;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace CheckerPrice.ConsoleApp
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            //args = "add -u sdsd".Split();
            //args = "check".Split();
            //args = "help".Split();
            //args = "add".Split();

            // Configure parser
            var parser = new Parser(settings =>
            {
                settings.AutoHelp = true;
                settings.AutoVersion = true;
                settings.CaseInsensitiveEnumValues = true;
                settings.CaseSensitive = false;
                settings.IgnoreUnknownArguments = false;
                settings.ParsingCulture = CultureInfo.CurrentCulture;

                // Default
                settings.EnableDashDash = Parser.Default.Settings.EnableDashDash;
                settings.HelpWriter = Parser.Default.Settings.HelpWriter;
                settings.MaximumDisplayWidth = Parser.Default.Settings.MaximumDisplayWidth;

            });

            using ServiceProvider services = ConfigureServices();
            //Services.My.IMyService myService = services.GetService<Services.My.IMyService>();

            try
            {
                var result = await parser.ParseArguments<AddActionParameters, CheckActionParameters>(args)
                    .MapResult(
                        (AddActionParameters model) => Ok(() => Task.CompletedTask),
                        (CheckActionParameters model) => Ok(() => Task.CompletedTask),
                        _ => Task.FromResult(1)
                    );

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
                return 1;
            }
        }

        private static async Task<int> Ok(Func<Task> func)
        {
            await func();

            return 0;
        }

        private static ServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services
                .AddMyServices();

            return services.BuildServiceProvider();
        }

        private static IServiceCollection AddMyServices(this IServiceCollection services)
        {
            // Add services
            //services.AddTransient<Services.My.IMyService, Services.My.MyService>();

            return services;
        }
    }
}
