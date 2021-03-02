using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace CheckerPrice.Services.Scrappers
{
    public static class WebUtilities
    {
        public static async Task<HtmlDocument> GetParsedHtmlFromUrlAsync(string url)
        {
            string html = await CallUrlAsync(url);

            return ParseHtml(html);
        }

        private static Task<string> CallUrlAsync(string fullUrl)
        {
            HttpClient client = new HttpClient();

            return client.GetStringAsync(fullUrl);
        }

        private static HtmlDocument ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }
    }
}
