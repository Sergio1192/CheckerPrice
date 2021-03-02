using CheckerPrice.Services.Models;
using System;
using System.Threading.Tasks;

namespace CheckerPrice.Services.Scrappers
{
    public static class Scrapper
    {
        public static Task<PriceModel> GetPriceAsync(string url)
        {
            return url switch
            {
                string u when u.Contains(PlaystationStoreScrapper.KEY) => PlaystationStoreScrapper.GetPrice(u),
                _ => throw new InvalidOperationException($"We can't get the price from '{url}'"),
            };
        }
    }
}
