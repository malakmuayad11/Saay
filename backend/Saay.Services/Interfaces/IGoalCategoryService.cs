using Saay.Infrastructure.DTOs.GoalCategoryDTOs;

namespace Saay.Services.Interfaces
{
    public interface IGoalCategoryService
    {
        public Task<List<GoalCategoryDto>> GetAllGoalCategoriesAsync();

        public enum GoalCategory : byte
        {
            Health = 1,
            Career = 2,
            Relationships = 3,
            Finance = 4,
            PersonalGrowth = 5,
            Spirituality = 6,
            Community = 7,
            Lifestyle = 8,
            Travel = 9,
            Technology = 10,
            LegalAndLegacy = 11,
            Environment = 12
        }
    }
}
