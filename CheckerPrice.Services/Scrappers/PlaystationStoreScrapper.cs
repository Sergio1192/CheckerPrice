using CheckerPrice.Services.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CheckerPrice.Services.Scrappers
{
    public static class PlaystationStoreScrapper
    {
        internal const string KEY = "store.playstation";

        public static async Task<PriceModel> GetPrice(string url)
        {
            var htmlDoc = await WebUtilities.GetParsedHtmlFromUrlAsync(url);

            string priceString = htmlDoc.DocumentNode.Descendants("span").First(e => e.GetAttributeValue("data-qa", string.Empty) == "mfeCtaMain#offer0#finalPrice").InnerText;
            double price = double.Parse(priceString, System.Globalization.NumberStyles.Currency);

            string name = htmlDoc.DocumentNode.Descendants("h1").First(e => e.GetAttributeValue("data-qa", string.Empty) == "mfe-game-title#name").InnerText;

            return new PriceModel
            {
                Name = name,
                Price = price
            };
        }
    }
}
