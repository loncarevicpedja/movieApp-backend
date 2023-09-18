using Internal.Contracts.DTOs.UsersDto;
using Internal.Domain.Entities;

namespace Internal.Contracts.DTOs.Movie
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Duration { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public float AvgRating { get; set; }
        public Category Category { get; set; }
        public MovieDirector Director { get; set; }
        public List<UserWOPass> HaveWatched { get; set; }
        public string? Cover { get; set; }
    }
}
