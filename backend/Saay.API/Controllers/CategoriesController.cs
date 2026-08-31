using Microsoft.AspNetCore.Mvc;
using Saay.Infrastructure.DTOs.CategoryDTOs;
using Saay.Services.Interfaces; 

namespace Saay.API.Controllers
{
    [Route("api/saay/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ICollection<CategoryDto>>> GetAllCategoriesAsync()
        {
            List<CategoryDto> categories = await _categoryService.GetAllCategoriesAsync();

            if (categories == null)
                return NotFound("No categories found.");

            return Ok(categories);
        }
    }
}
