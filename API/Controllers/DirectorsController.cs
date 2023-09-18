using AutoMapper;
using Internal.Application.Common.Interfaces.Persistance;
using Internal.Contracts.DTOs.Directors;
using Internal.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirectorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uof;

        public DirectorsController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateDirector([FromBody] DirectorCreateRequest dir)
        {
            if (dir == null)
                return NotFound();

            var toAdd = _mapper.Map<MovieDirector>(dir);

            await _uof._movieDirectorRepository.AddAsync(toAdd);

            await _uof.CompleteAsync();

            return Ok(toAdd);
        }
    }
}