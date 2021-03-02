using CheckerPrice.Services.Models;
using CheckerPrice.Services.Scrappers;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CheckerPrice.Services
{
    public class CheckerPrice : ICheckerPrice
    {
        private const string PATH = "./links.csv";

        public async Task<CheckModel> AddAsync(string url)
        {
            var newId = (await ShowAsync().MaxAsync(e => e.Id)) + 1;
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
            using var writer = new StreamWriter(PATH, append);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

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

        public IAsyncEnumerable<UrlModel> ShowAsync()
        {
            using var reader = new StreamReader(PATH);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecordsAsync<UrlModel>();
        }
    }
}
