using Internal.Domain.Entities;

namespace Internal.Application.Common.Interfaces.Persistance
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAllAsync();
        public Task<Category?> GetByIdAsync(int id);
        public Task AddAsync(Category c);
        public void Remove(Category c);
    }
}
