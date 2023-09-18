using Internal.Application.Common.Helpers;
using Internal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal.Application.Common.Interfaces.Persistance
{
    public interface IMovieRepository
    {
        public Task<List<Movie>> GetAllAsync();
        public Task<Movie?> GetByIdAsync(int id);
        public Task<List<Movie>> GetByIdsAsync(int[] ids);
        public Task AddAsync(Movie movie);
        public void Delete(Movie movie);
        Task<PagedList<Movie>> GetFiltered(MovieFilter filter);
    }
}
