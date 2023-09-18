using Internal.Application.Common.Interfaces.Persistance;

namespace Internal.Infrastructure.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieDbContext _context;
        public IMovieRepository _movieRepository { get; set; }
        public ICategoryRepository _categoryRepository { get; set; }
        public IMovieDirectorRepository _movieDirectorRepository { get; set; }
        public IWatchlistRepository _watchlistRepository { get ; set ; }

        public UnitOfWork(MovieDbContext context,
            IMovieRepository movieRepository,
            ICategoryRepository categoryRepository,
            IMovieDirectorRepository movieDirectorRepository,
            IWatchlistRepository watchlistRepository)
        {
            _context = context;
            _movieRepository = movieRepository;
            _categoryRepository = categoryRepository;
            _movieDirectorRepository = movieDirectorRepository;
            _watchlistRepository = watchlistRepository;
        }

        public async Task CompleteAsync() => await _context.SaveChangesAsync();
    }
}
