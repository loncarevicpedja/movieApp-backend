using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal.Contracts.DTOs.Rating
{
    public class RatingCreateRequest
    {
        [InRange(1,5, ErrorMessage = "Value must be in range of 1 to 5")]
        public int Rating { get; set; }
        public int RaterId { get; set; }
    }
}
