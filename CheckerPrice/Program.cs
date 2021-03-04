using CheckerPrice.ConsoleApp.Models;
using CheckerPrice.ConsoleApp.Services;
using CheckerPrice.Services;
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
            var service = services.GetService<ICheckerPriceConsoleService>();

            try
            {
                var result = await parser.ParseArguments<AddActionParameters, CheckActionParameters, CheckUrlActionParameters, DeleteActionParameters, ShowActionParameters>(args)
                    .MapResult(
                        (AddActionParameters model) => Ok(() => service.AddAsync(model.Url)),
                        (CheckActionParameters model) => Ok(() => service.CheckAsync(model.Id)),
                        (CheckUrlActionParameters model) => Ok(() => service.CheckAsync(model.Url)),
                        (DeleteActionParameters model) => Ok(() => service.DeleteAsync(model.Id)),
                        (ShowActionParameters model) => Ok(() => service.ShowAsync()),
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
            services.AddSingleton<ICheckerPriceService, CheckerPriceService>();
            services.AddSingleton<ICheckerPriceConsoleService, CheckerPriceConsoleService>();

            return services;
        }
    }
}
