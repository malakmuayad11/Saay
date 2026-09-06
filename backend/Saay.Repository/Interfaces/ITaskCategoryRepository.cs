using Saay.Data.Entities;

namespace Saay.Repository.Interfaces
{
    public interface ITaskCategoryRepository
    {
        public Task<List<TaskCategory>> GetAllTasksCategoriesAsync();
    }
}
