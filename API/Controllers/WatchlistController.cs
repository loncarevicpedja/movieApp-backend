using Internal.Application.Common.Interfaces.Persistance;
using Internal.Contracts.DTOs.Movie;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WatchlistController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public WatchlistController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var watchList = await _uof._watchlistRepository.GetByUserIdAsync(id);
            return Ok(new { Id = watchList.Id, list = watchList.Movies.Select(n => new { n.Name, n.Id }) });
        }

        [HttpPost]
        public async Task<IActionResult> AddTo([FromBody] AddToWatchListDto req)
        {
            var watchList = await _uof._watchlistRepository.GetByUserIdAsync(req.UserId);

            var movie = await _uof._movieRepository.GetByIdAsync(req.MovieId);

            if (watchList == null || movie == null)
            {
                throw new Exception("Not Found");
            }

            if (!watchList.Movies.Any(n => n.Id == req.MovieId))
                watchList.Movies.Add(movie);

            await _uof.CompleteAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFrom([FromBody] AddToWatchListDto req)
        {
            var watchList = await _uof._watchlistRepository.GetByUserIdAsync(req.UserId);

            var movie = await _uof._movieRepository.GetByIdAsync(req.MovieId);

            if (watchList == null || movie == null)
            {
                throw new Exception("Not Found");
            }

            if (watchList.Movies.Any(n => n.Id == req.MovieId))
            {
                watchList.Movies.Remove(movie);
            }

            await _uof.CompleteAsync();

            return Ok();
        }
    }
}