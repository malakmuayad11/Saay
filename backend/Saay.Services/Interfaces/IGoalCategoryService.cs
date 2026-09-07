using Saay.Infrastructure.DTOs.GoalCategoryDTOs;

namespace Saay.Services.Interfaces
{
    public interface IGoalCategoryService
    {
        public Task<List<GoalCategoryDto>> GetAllGoalCategoriesAsync();
    }
}
