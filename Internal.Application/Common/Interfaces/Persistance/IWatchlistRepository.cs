using Internal.Domain.Entities;

namespace Internal.Application.Common.Interfaces.Persistance
{
    public interface IWatchlistRepository
    {
        public Task<WatchList?> GetByUserIdAsync(int id);
    }
}
