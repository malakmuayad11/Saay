using Saay.Infrastructure.DTOs.CategoryDTOs;

namespace Saay.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<CategoryDto>> GetAllCategoriesAsync();
    }
}
