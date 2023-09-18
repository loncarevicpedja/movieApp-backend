using AutoMapper;
using Internal.Application.Common.Helpers;
using Internal.Application.Common.Interfaces.Persistance;
using Internal.Contracts.DTOs.Movie;
using Internal.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uof;
        private readonly UserManager<User> _um;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

        public MoviesController(IMapper mapper, IUnitOfWork uof, UserManager<User> um, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _mapper = mapper;
            _uof = uof;
            _um = um;
            this.hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMovies() =>
            Ok(_mapper.Map<IEnumerable<MovieResponse>>(
                await _uof._movieRepository.GetAllAsync()
            )
        );

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IActionResult> GetAllMoviesFilteredAndSorted([FromQuery] MovieFilter filter) =>
            Ok(_mapper.Map<PagedList<MovieResponse>>(await _uof._movieRepository.GetFiltered(filter)));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _uof._movieRepository.GetByIdAsync(id);

            if (movie == null)
                return NotFound("Movie not found");

            return Ok(_mapper.Map<MovieResponse>(movie));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ids")]
        public async Task<IActionResult> GetMovies([FromQuery] int[] ids) =>
            Ok(_mapper.Map<IEnumerable<MovieResponse>>(
                await _uof._movieRepository.GetByIdsAsync(ids)
            )
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _uof._movieRepository.GetByIdAsync(id);

            if (movie == null)
                return NotFound();

            _uof._movieRepository.Delete(movie);

            await _uof.CompleteAsync();

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromForm] MovieCreateRequest movie)
        {
            if (movie == null)
                return NotFound();

            var dir = await _uof._movieDirectorRepository.GetByIdAsync(movie.MovieDirectorId);
            if (dir == null)
                return NotFound();

            var cat = await _uof._categoryRepository.GetByIdAsync(movie.CategoryId);
            if (cat == null)
                return NotFound();

            var toAdd = _mapper.Map<Movie>(movie);

            if(movie.Cover != null)
            {
                string imageName = new String(Path.GetFileNameWithoutExtension(movie.Cover.Name).Take(10).ToArray()).Replace(' ', '-');

                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(movie.Cover.FileName);

                var imagePath = Path.Combine(hostingEnvironment.ContentRootPath, "wwwroot/Images", imageName);

                using var fileStream = new FileStream(imagePath, FileMode.Create);
                await movie.Cover.CopyToAsync(fileStream);

                toAdd.Cover = imageName;
            }

            await _uof._movieRepository.AddAsync(toAdd);

            await _uof.CompleteAsync();

            return Ok(toAdd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("watch")]
        public async Task<IActionResult> WatchMovie([FromBody] WatchRequest req)
        {
            var movie = await _uof._movieRepository.GetByIdAsync(req.MovieId);

            if (movie is null)
            {
                return NotFound("Movie has not been found in DB");
            }

            var user = await _um.FindByIdAsync(req.UserId.ToString());

            if (movie is null)
            {
                return NotFound("User has not been found in DB");
            }

            user.AddToWatched(movie);

            await _uof.CompleteAsync();

            return Ok();
        }
    }
}