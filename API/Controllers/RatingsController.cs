using Internal.Application.Common.Interfaces.Persistance;
using Internal.Contracts.DTOs.Rating;
using Internal.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public RatingsController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpPost("{movieId}")]
        public async Task<IActionResult> RateMovie(int movieId, [FromBody] RatingCreateRequest rating)
        {
            var movie = await _uof._movieRepository.GetByIdAsync(movieId);

            if (movie is null)
            {
                return NotFound("Movie has not been found in DB");
            }

            // user can only rate the movie if he watched it
            // check if the list of watchers contains the current user
            if (!movie.HaveWatched.Any(n => n.Id == rating.RaterId))
            {
                return NotFound("Cannot rate without watching");
            }

            if (movie.Ratings.Any(n => n.RaterId == rating.RaterId))
            {
                var rat = movie.Ratings.Where(n => n.RaterId == rating.RaterId).FirstOrDefault();
                rat.Value = rating.Rating;
            }
            else
            {
                movie.Rate(new Rating
                {
                    Value = rating.Rating,
                    RaterId = rating.RaterId
                });
            }

            await _uof.CompleteAsync();

            return Ok();
        }
    }
}