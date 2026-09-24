using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Data.Entities;
using Saay.Infrastructure.DTOs.GoalDTOs;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class GoalRepository : IGoalRepository
    {
        private readonly SaayContext _context;

        public GoalRepository(SaayContext context)
        {
            _context = context;
        }

        public async Task<int?> AddGoalAsync(Goal goal, int userId)
        {
            Goal newGoal = new Goal
            {
                UserId = userId,
                GoalCategoryId = goal.GoalCategoryId,
                Title = goal.Title,
                TimeFrame = goal.TimeFrame,
                Deadline = goal.Deadline,
                IsDone = false
            };

            _context.Goals.Add(newGoal);
            if (await _context.SaveChangesAsync() > 0)
                return newGoal.GoalId;

            return null;
        }

        public async Task<List<GoalDto>> GetUserGoalsAsync(int userId,
            int pageNumber, int pageSize) =>
            await _context.Goals
                .Where(goal => goal.UserId == userId)
                .Select(goal => new GoalDto
                {
                    GoalId = goal.GoalId,
                    CategoryTitle = goal.GoalCategory.Title,
                    Title = goal.Title,
                    TimeFrame = goal.TimeFrame,
                    Deadline = goal.Deadline,
                    IsDone = goal.IsDone,
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

        public async Task<int> UserGoalsCountAsync(int userId) => await _context.Goals
                .Where(goal => goal.UserId == userId)
                .CountAsync();

        public async Task<bool?> UpdateGoalAsync(int goalId, Goal newGoal)
        {
            Goal goal = await _context.Goals.FindAsync(goalId);

            if (goal == null) return null; // Goal not found

            goal.GoalCategoryId = newGoal.GoalCategoryId;
            goal.Title = newGoal.Title;
            goal.TimeFrame = newGoal.TimeFrame;
            goal.Deadline = newGoal.Deadline;
            goal.IsDone = newGoal.IsDone;

            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool?> DeleteGoalAsync(int goalId)
        {
            Goal goal = await _context.Goals.FindAsync(goalId);
            if (goal == null) return null;
            _context.Goals.Remove(goal);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> UserCompletedGoalsCount(int userId) =>
             await _context.Goals
                .Where(goal => goal.UserId == userId && goal.IsDone)
                .CountAsync();

        public async Task<int> UserPendingGoalsCount(int userId) =>
             await _context.Goals
                .Where(goal => goal.UserId == userId && !goal.IsDone)
                .CountAsync();

        public async Task<GoalDto> GetGoalByIdAsync(int goalId) =>
            await _context.Goals
            .Where(goal => goal.GoalId == goalId)
            .Select(goal => new GoalDto
            {
                GoalId = goal.GoalId,
                CategoryTitle = goal.GoalCategory.Title,
                Title = goal.Title,
                TimeFrame = goal.TimeFrame,
                Deadline = goal.Deadline,
                IsDone = goal.IsDone
            })
            .FirstOrDefaultAsync();

        public async Task<bool> IsGoalOwner(int userId, int goalId) =>
            await _context.Goals.AnyAsync(g => g.UserId == userId && g.GoalId == goalId);
    }
}
