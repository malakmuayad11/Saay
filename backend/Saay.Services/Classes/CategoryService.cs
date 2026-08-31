using Saay.Data.Entities;
using Saay.Infrastructure.DTOs.CategoryDTOs;
using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;

namespace Saay.Services.Classes
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository) =>
            _categoryRepository = categoryRepository;

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            List<CategoryDto> categoryDtos = new List<CategoryDto>();

            foreach (Category category in await _categoryRepository.GetAllCategoriesAsync())
            {
                categoryDtos.Add(new CategoryDto
                {
                    Title = category.Title
                });
            }

            return categoryDtos;
        }
    }
}
