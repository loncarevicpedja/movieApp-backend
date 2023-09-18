using System.ComponentModel.DataAnnotations.Schema;

namespace Internal.Domain.Entities
{
    public class WatchList
    {
        public int Id { get; set; }
        public User User { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public List<Movie> Movies { get; set; } = new();
    }
}
