using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Internal.Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int MovieId { get; set; }
        public User Rater { get; set; }

        [ForeignKey("Rater")]
        public int RaterId { get; set; }
    }
}
