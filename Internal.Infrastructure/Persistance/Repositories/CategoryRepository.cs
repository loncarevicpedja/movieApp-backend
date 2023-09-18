using Internal.Application.Common.Interfaces.Persistance;
using Internal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Internal.Infrastructure.Persistance.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MovieDbContext _context;

        public CategoryRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category c)
        {
            await _context.Category.AddAsync(c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Category>> GetAllAsync() =>
            await _context.Category.AsNoTracking().ToListAsync();

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Category.FindAsync(id);
        }

        public void Remove(Category c)
        {
           _context.Category.Remove(c);
        }
    }
}
