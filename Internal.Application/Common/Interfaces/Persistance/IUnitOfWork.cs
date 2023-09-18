namespace Internal.Application.Common.Interfaces.Persistance
{
    public interface IUnitOfWork
    {
        IMovieRepository _movieRepository { get; set; }
        ICategoryRepository _categoryRepository { get; set; }
        IMovieDirectorRepository _movieDirectorRepository { get; set; }
        IWatchlistRepository _watchlistRepository { get; set; }
        Task CompleteAsync();
    }
}
