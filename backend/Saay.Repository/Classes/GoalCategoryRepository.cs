using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Data.Entities;
using Saay.Infrastructure.DTOs.GoalCategoryDTOs;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class GoalCategoryRepository : IGoalCategoryRepository
    {
        private SaayContext _context;

        public GoalCategoryRepository(SaayContext context) => _context = context;

        public async Task<List<GoalCategory>> GetAllGoalCategoriesAsync() =>
            await _context.GoalsCategories.AsNoTracking().ToListAsync();

        public async Task<List<GoalCategoryCountsDto>> GetAllGoalCategoriesWithCountsAsync(int userId) =>
            await _context.GoalsCategories
            .AsNoTracking()
            .Select(gc => new GoalCategoryCountsDto
            {
                Title = gc.Title,
                Count = (byte)gc.Goals.Count(g => g.UserId == userId)
            })
            .Where(r => r.Count > 0)
            .ToListAsync();
    }
}
