using System.Threading.Tasks;
using Scraper.Domain.Commands;

namespace Scraper.Domain.Contracts
{
    public interface IShowCommandRepository
    {
        Task AddShowAsync(AddShowCommand addShowCommand);
    }
}
