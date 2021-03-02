using CheckerPrice.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckerPrice.Services
{
    public interface ICheckerPrice
    {
        Task<CheckModel> AddAsync(string url);
        IAsyncEnumerable<UrlModel> ShowAsync();
        IAsyncEnumerable<CheckModel> CheckAsync();
        Task<CheckModel> CheckAsync(int id);
        Task<PriceModel> CheckAsync(string url);
        Task<CheckModel> DeleteAsync(int id);
    }
}
