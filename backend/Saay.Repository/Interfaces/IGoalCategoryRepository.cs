using Saay.Data.Entities;

namespace Saay.Repository.Interfaces
{
    public interface IGoalCategoryRepository
    {
        public Task<List<GoalCategory>> GetAllGoalCategoriesAsync();
    }
}
