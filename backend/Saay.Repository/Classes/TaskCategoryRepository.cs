using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Data.Entities;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class TaskCategoryRepository : ITaskCategoryRepository
    {
        private SaayContext _context;

        public TaskCategoryRepository(SaayContext context) => _context = context;
        public async Task<List<TaskCategory>> GetAllTasksCategoriesAsync() =>
            await _context.TasksCategories.AsNoTracking().ToListAsync();
    }
}
