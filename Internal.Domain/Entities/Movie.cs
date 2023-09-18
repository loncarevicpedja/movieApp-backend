using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public float Duration { get; set; }
        public string Description { get; set; }
        public List<Rating> Ratings { get; set; } = new();
        public List<User> HaveWatched { get; set; } = new();
        public List<WatchList> WatchLists { get; set; } = new();
        public Category Category { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public MovieDirector Director { get; set; }

        [ForeignKey("Director")]
        public int MovieDirectorId { get; set; }
        public string? Cover { get; set; }

        // methods

        public void Rate(Rating rating) => Ratings.Add(rating);

    }
}
