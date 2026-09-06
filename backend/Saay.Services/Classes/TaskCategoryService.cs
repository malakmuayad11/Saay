using Saay.Data.Entities;
using Saay.Infrastructure.DTOs.CategoryDTOs;
using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;

namespace Saay.Services.Classes
{
    public class TaskCategoryService : ITaskCategoryService
    {
        private readonly ITaskCategoryRepository _taskCategoryRepository;

        public TaskCategoryService(ITaskCategoryRepository taskCategoryRepository) =>
            _taskCategoryRepository = taskCategoryRepository;

        public async Task<List<TaskCategoryDto>> GetAllTasksCategoriesAsync()
        {
            List<TaskCategoryDto> categoryDtos = new List<TaskCategoryDto>();

            foreach (TaskCategory category in await _taskCategoryRepository.GetAllTasksCategoriesAsync())
            {
                categoryDtos.Add(new TaskCategoryDto
                {
                    Title = category.Title
                });
            }

            return categoryDtos;
        }
    }
}
