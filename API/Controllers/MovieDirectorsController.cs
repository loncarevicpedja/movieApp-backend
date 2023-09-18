using AutoMapper;
using Internal.Application.Common.Interfaces.Persistance;
using Internal.Contracts.DTOs.MovieDir;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieDirectorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uof;

        public MovieDirectorsController(IMapper mapper, IUnitOfWork uof)
        {
            _mapper = mapper; _uof = uof;
        }



        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _uof._movieDirectorRepository.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CreateMovieDir req)
        {
            await _uof._movieDirectorRepository.AddAsync(new Internal.Domain.Entities.MovieDirector
            {
                FirstName = req.FirstName,
                LastName = req.LastName
            });
            await _uof.CompleteAsync();

            return Ok("Created");
        }
    }
}