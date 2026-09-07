using Saay.Data.Entities;
using Saay.Infrastructure.DTOs.GoalCategoryDTOs;
using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;

namespace Saay.Services.Classes
{
    public class GoalCategoryService : IGoalCategoryService
    {
        private readonly IGoalCategoryRepository _goalCategoryRepository;

        public GoalCategoryService(IGoalCategoryRepository goalCategoryRepository) =>
            _goalCategoryRepository = goalCategoryRepository;

        public async Task<List<GoalCategoryDto>> GetAllGoalCategoriesAsync()
        {
            List<GoalCategoryDto> categoryDtos = new List<GoalCategoryDto>();

            foreach (GoalCategory category in await _goalCategoryRepository.GetAllGoalCategoriesAsync())
            {
                categoryDtos.Add(new GoalCategoryDto
                {
                    Title = category.Title
                });
            }

            return categoryDtos;
        }
    }
}
