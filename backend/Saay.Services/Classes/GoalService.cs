using Saay.Infrastructure.DTOs.GoalDTOs;
using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;
using Saay.Data.Entities;

namespace Saay.Services.Classes
{
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IUserRepository _userRepository;
        

        public GoalService(IGoalRepository goalRepository, IUserRepository userRepository)
        {
            _goalRepository = goalRepository;
            _userRepository = userRepository;
        }

        public async Task<int?> AddGoalAsync(AddGoalDto addGoalDto)
        {
            if (!await _userRepository.DoesUserExist(addGoalDto.UserId))
                return null; // User does not exist

            Goal goalEntity = new Goal
            {
                UserId = addGoalDto.UserId,
                GoalCategoryId = addGoalDto.GoalCategoryId,
                Title = addGoalDto.Title,
                TimeFrame = addGoalDto.TimeFrame,
                Deadline = addGoalDto.Deadline,
            };

            return await _goalRepository.AddGoalAsync(goalEntity, addGoalDto.UserId);
        }

        public async Task<List<GoalDto>> GetUserGoalsAsync(int userId, int pageNumber, int pageSize)
        {
            if (!await _userRepository.DoesUserExist(userId))
                return null; // User does not exist

            List<Goal> goals = await _goalRepository.GetUserGoalsAsync(userId, pageNumber, pageSize);

            return goals.Select(goal => new GoalDto
            {
                GoalId = goal.GoalId,
                CategoryTitle = ((IGoalCategoryService.GoalCategory)goal.GoalCategoryId).ToString(),
                Title = goal.Title,
                TimeFrame = ((IGoalService.GoalTimeFrame)goal.TimeFrame).ToString(),
                Deadline = goal.Deadline,
                IsDone = goal.IsDone
            }).ToList();
        }

        public async Task<int?> UserGoalsCountAsync(int userId)
        {
            if (!await _userRepository.DoesUserExist(userId))
                return null; // User not found

            return await _goalRepository.UserGoalsCountAsync(userId);
        }

        public async Task<bool?> UpdateGoalAsync(UpdateGoalDto updateGoalDto)
        {
            Goal goal = new Goal
            {
                GoalId = updateGoalDto.GoalId,
                GoalCategoryId = updateGoalDto.GoalCategoryId,
                Title = updateGoalDto.Title,
                TimeFrame = updateGoalDto.TimeFrame,
                Deadline = updateGoalDto.Deadline,
                IsDone = updateGoalDto.IsDone,
            };

            return await _goalRepository.UpdateGoalAsync(updateGoalDto.GoalId, goal);
        }

        public async Task<bool?> DeleteGoalAsync(int goalId) =>
            await _goalRepository.DeleteGoalAsync(goalId);

        public async Task<int?> UserCompletedGoalsCountAsync(int userId)
        {
            if (!await _userRepository.DoesUserExist(userId))
                return null; // User not found

            return await _goalRepository.UserCompletedGoalsCount(userId);
        }

        public async Task<int?> UserPendingGoalsCountAsync(int userId)
        {
            if (!await _userRepository.DoesUserExist(userId))
                return null; // User not found
            return await _goalRepository.UserPendingGoalsCount(userId);
        }

        public async Task<GoalDto> GetGoalByIdAsync(int goalId)
        {
           Goal goal = await _goalRepository.GetGoalByIdAsync(goalId);

            if (goal == null) return null;

            return new GoalDto
            {
                GoalId = goal.GoalId,
                CategoryTitle = ((IGoalCategoryService.GoalCategory)goal.GoalCategoryId).ToString(),
                Title = goal.Title,
                TimeFrame = ((IGoalService.GoalTimeFrame)goal.TimeFrame).ToString(),
                Deadline = goal.Deadline,
                IsDone = goal.IsDone
            };
        }

        public async Task<bool> IsGoalOwner(int userId, int goalId) =>
            await _goalRepository.IsGoalOwner(userId, goalId);
    }
}
