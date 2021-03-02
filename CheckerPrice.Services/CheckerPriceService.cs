using CheckerPrice.Services.Models;
using CheckerPrice.Services.Scrappers;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CheckerPrice.Services
{
    public class CheckerPriceService : ICheckerPriceService
    {
        private const string PATH = "./Data/links.csv";

        public CheckerPriceService()
        {
            string directory = Path.GetDirectoryName(PATH);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public async Task<CheckModel> AddAsync(string url)
        {
            var newId = (await ShowAsync().DefaultIfEmpty(new UrlModel { Id = 0 }).MaxAsync(e => e.Id)) + 1;
            var model = new UrlModel
            {
                Id = newId,
                Url = url
            };

            await AddAsync(new List<UrlModel> { model }, true);

            return await CheckAsync(model);
        }

        private Task AddAsync(IEnumerable<UrlModel> list, bool append)
        {
            var exist = File.Exists(PATH);

            using var writer = new StreamWriter(PATH, append);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = !(append && exist)
            });

            return csv.WriteRecordsAsync(list);
        }

        public async IAsyncEnumerable<CheckModel> CheckAsync()
        {
            await foreach (var model in ShowAsync())
            {
                yield return await CheckAsync(model);
            }
        }

        public async Task<CheckModel> CheckAsync(int id)
        {
            var model = (await ShowAsync().FirstAsync(e => e.Id == id));

            return await CheckAsync(model);
        }

        public Task<PriceModel> CheckAsync(string url)
        {
            return Scrapper.GetPriceAsync(url);
        }

        private async Task<CheckModel> CheckAsync(UrlModel model)
        {
            return new CheckModel()
            {
                Identifier = model,
                Information = await CheckAsync(model.Url)
            };
        }

        public async Task<CheckModel> DeleteAsync(int id)
        {
            var list = await ShowAsync().ToListAsync();

            var model = list.First(e => e.Id == id);
            list.Remove(model);

            await AddAsync(list, false);

            return await CheckAsync(model);
        }

        public async IAsyncEnumerable<UrlModel> ShowAsync()
        {
            if (!File.Exists(PATH))
                yield break;

            using var reader = new StreamReader(PATH);
            using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);

            await foreach (var record in csv.GetRecordsAsync<UrlModel>())
            {
                yield return record;
            }
        }
    }
}
