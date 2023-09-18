using Internal.Domain.Entities;

namespace Internal.Application.Common.Interfaces.Persistance
{
    public interface IMovieDirectorRepository
    {
        public Task<MovieDirector?> GetByIdAsync(int id);
        public Task AddAsync(MovieDirector movieDirector);
        public Task<List<MovieDirector>> GetAllAsync();
    }
}
