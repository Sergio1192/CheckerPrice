using System.Threading.Tasks;

namespace CheckerPrice.ConsoleApp.Services
{
    public interface ICheckerPriceConsoleService
    {
        Task AddAsync(string url);
        Task ShowAsync();
        Task CheckAsync(int? id);
        Task CheckAsync(string url);
        Task DeleteAsync(int id);
    }
}
