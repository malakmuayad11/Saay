using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Data.Entities;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class CategoryRepository : ICategoryRepository
    {
        private SaayContext _context;

        public CategoryRepository(SaayContext context) => _context = context;
        public async Task<List<Category>> GetAllCategoriesAsync() =>
            await _context.Categories.ToListAsync();
    }
}
