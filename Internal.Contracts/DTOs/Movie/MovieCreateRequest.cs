using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal.Contracts.DTOs.Movie
{
    public class MovieCreateRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int Year { get; set; }
        public int MovieDirectorId { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Cover { get; set; }
    }
}
