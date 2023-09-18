using Internal.Application.Common.Interfaces.Persistance;
using Internal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Internal.Infrastructure.Persistance.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly MovieDbContext _context;

        public WatchlistRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<WatchList?> GetByUserIdAsync(int id)
        {
            return await _context.WatchLists.Include(n => n.Movies).Where(n => n.UserId == id).FirstOrDefaultAsync();
        }
    }
}
