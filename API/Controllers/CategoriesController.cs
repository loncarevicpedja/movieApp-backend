using Internal.Application.Common.Interfaces.Persistance;
using Internal.Contracts.DTOs.CategoryDTO;
using Internal.Contracts.DTOs.CategoryDTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public CategoriesController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
            => Ok(await _uof._categoryRepository.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CreateCategory req)
        {
            await _uof._categoryRepository.AddAsync(new Internal.Domain.Entities.Category
            {
                Caption = req.Name
            });
            await _uof.CompleteAsync();

            return Ok("Created");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _uof._categoryRepository.GetByIdAsync(id);
            if(cat == null)
                return NotFound("Movie has not been found in DB");

            _uof._categoryRepository.Remove(cat);

            await _uof.CompleteAsync();
            return Ok("Removed");
        }
    }
}