using Saay.Data;
using Saay.Data.Entities;
using Saay.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Saay.Repository.Classes
{
    public class GoalCategoryRepository : IGoalCategoryRepository
    {
        private SaayContext _context;

        public GoalCategoryRepository(SaayContext context) => _context = context;

        public async Task<List<GoalCategory>> GetAllGoalCategoriesAsync() =>
            await _context.GoalsCategories.AsNoTracking().ToListAsync();
    }
}
