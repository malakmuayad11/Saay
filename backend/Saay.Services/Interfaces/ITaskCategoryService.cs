using Saay.Infrastructure.DTOs.CategoryDTOs;

namespace Saay.Services.Interfaces
{
    public interface ITaskCategoryService
    {
        public Task<List<TaskCategoryDto>> GetAllTasksCategoriesAsync();
    }
}
