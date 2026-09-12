using Saay.Infrastructure.DTOs.GoalDTOs;

namespace Saay.Services.Interfaces
{
    public interface IGoalService
    {
        public Task<int?> AddGoalAsync(AddGoalDto addGoalDto);

        public Task<List<GoalDto>> GetUserGoalsAsync(int userId, int pageNumber, int pageSize);

        public Task<int?> UserGoalsCountAsync(int userId);

        public Task<bool?> UpdateGoalAsync(UpdateGoalDto updateGoalDto);

        public Task<bool?> DeleteGoalAsync(int goalId);

        public Task<int?> UserCompletedGoalsCountAsync(int userId);

        public Task<int?> UserPendingGoalsCountAsync(int userId);

        public Task<GoalDto> GetGoalByIdAsync(int goalId);

        public Task<bool> IsGoalOwner(int userId, int goalId);
    }
}
