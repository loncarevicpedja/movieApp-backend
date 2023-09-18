using Internal.Application.Common.Interfaces.Persistance;
using Internal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Internal.Infrastructure.Persistance.Repositories
{
    public class MovieDirectorRepository : IMovieDirectorRepository
    {
        private readonly MovieDbContext _context;

        public MovieDirectorRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MovieDirector movieDirector)
        {
            await _context.Directors.AddAsync(movieDirector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<MovieDirector?> GetByIdAsync(int id) =>
            await _context.Directors.FindAsync(id);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<MovieDirector>> GetAllAsync() =>
            await _context.Directors.ToListAsync();
    }
}
