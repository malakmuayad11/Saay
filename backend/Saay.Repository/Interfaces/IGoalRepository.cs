using Saay.Data.Entities;

namespace Saay.Repository.Interfaces
{
    public interface IGoalRepository
    {
        public Task<int?> AddGoalAsync(Goal goal, int userId);

        public Task<List<Goal>> GetUserGoalsAsync(int userId, int pageNumber, int pageSize);

        public Task<int> UserGoalsCountAsync(int userId);

        public Task<bool?> UpdateGoalAsync(int goalId, Data.Entities.Goal newGoal);

        public Task<bool?> DeleteGoalAsync(int goalId);

        public Task<int> UserCompletedGoalsCount(int userId);

        public Task<int> UserPendingGoalsCount(int userId);

        public Task<Goal> GetGoalByIdAsync(int goalId);

        public Task<bool> IsGoalOwner(int userId, int goalId);
    }
}
