using Saay.Data.Entities;

namespace Saay.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAllCategoriesAsync();
    }
}
