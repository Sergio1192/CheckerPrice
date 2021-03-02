using CheckerPrice.Services;
using CheckerPrice.Services.Models;
using System;
using System.Threading.Tasks;

namespace CheckerPrice.ConsoleApp.Services
{
    public class CheckerPriceConsoleService : ICheckerPriceConsoleService
    {
        private readonly ICheckerPriceService _service;

        public CheckerPriceConsoleService(ICheckerPriceService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task AddAsync(string url)
        {
            var model = await _service.AddAsync(url);

            ShowModel(model);
        }

        public async Task CheckAsync(int? id)
        {
            if (id == null)
            {
                await foreach (var model in _service.CheckAsync())
                {
                    ShowModel(model);
                }
            }
            else
            {
                var model = await _service.CheckAsync(id.Value);

                ShowModel(model);
            }
        }

        public async Task CheckAsync(string url)
        {
            var model = await _service.CheckAsync(url);

            ShowModel(model);
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _service.DeleteAsync(id);

            ShowModel(model);
        }

        public async Task ShowAsync()
        {
            await foreach(var model in _service.ShowAsync())
            {
                ShowModel(model);
            }
        }

        private void ShowModel(CheckModel model)
        {
            ShowModel(model.Information, false);
            Console.Write($" --> ");
            ShowModel(model.Identifier);
        }

        private void ShowModel(UrlModel model, bool newLine = true)
        {
            Console.Write($"[{model.Id}] - {model.Url}");
            NewLine(newLine);
        }

        private void ShowModel(PriceModel model, bool newLine = true)
        {
            Console.Write($"{model.Name}: {model.Price}");
            NewLine(newLine);
        }

        private void NewLine(bool newLine)
        {
            if (newLine)
                Console.WriteLine();
        }
    }
}
