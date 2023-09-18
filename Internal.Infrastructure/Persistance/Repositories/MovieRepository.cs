using Internal.Application.Common.Helpers;
using Internal.Application.Common.Interfaces.Persistance;
using Internal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal.Infrastructure.Persistance.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _context;

        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Movie>> GetAllAsync() => 
            await _context.Movies.Include(m => m.Ratings).Include(m => m.Category).AsNoTracking().ToListAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Movie?> GetByIdAsync(int id) => 
            await _context.Movies
            .Include(m => m.Ratings)
            .Include(m => m.Director)
            .Include(m => m.Category)
            .Include(m => m.HaveWatched)
            .FirstOrDefaultAsync(m => m.Id == id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Movie>> GetByIdsAsync(int[] ids)
        {
            if (ids.Length == 0) return new();

            return await _context.Movies
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movie"></param>
        public void Delete(Movie movie) =>
            _context.Movies.Remove(movie);

        public async Task<PagedList<Movie>> GetFiltered(MovieFilter filter)
        {
            IQueryable<Movie> query = _context.Movies
                .Include(n => n.Category)
                .Include(n => n.Ratings);

            if (filter.Duration.HasValue && filter.Duration> 0)
            {
                query = query.Where(n => n.Duration < filter.Duration);
            }

            if (filter.Category.HasValue && filter.Category != 0)
            {
                query = query.Where(n => n.CategoryId == filter.Category);
            }

            if(filter.RatingsAsc.HasValue)
            {
                if (filter.RatingsAsc == true)
                {
                    query = query.OrderBy(n => n.Ratings.Average(r => r.Value));
                }
                else
                {
                    query = query.OrderByDescending(n => n.Ratings.Average(r => r.Value));
                }
            }

            var pagedQuery = await PagedList<Movie>.CreateAsync(query, 
                filter.Page, 
                filter.PageSize);

            return pagedQuery;
        }
    }
}
