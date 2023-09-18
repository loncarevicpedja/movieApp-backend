using Microsoft.AspNetCore.Identity;

namespace Internal.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            WatchList = new WatchList
            {
                User = this
            };
        }

        public List<Movie> Watched { get; set; } = new();
        public void AddToWatched(Movie movie) => Watched.Add(movie);
        public WatchList WatchList { get; set; }
    }
}
