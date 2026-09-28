using Saay.Infrastructure.DTOs.TaskDTOs;

namespace Saay.Services.Interfaces
{
    public interface ITaskService
    {
        public Task<int?> AddTaskAsync(AddTaskDto addTaskDto);

        public Task<List<TaskDto>> GetUserTasksTodayAsync(int userId, int pageNumber, int pageSize);
        public Task<List<TaskDto>> GetUserTasksTomorrowAsync(int userId, int pageNumber, int pageSize);
        public Task<List<TaskDto>> GetUserTasksForWeekAsync(int userId, int pageNumber, int pageSize);

        public Task<int?> UserTasksCountAsync(int userId);

        public Task<bool?> UpdateTaskAsync(UpdateTaskDto updateTaskDto);

        public Task<bool?> DeleteTaskAsync(int taskId);

        public Task<int?> UserCompletedTasksCountAsync(int userId);

        public Task<int?> UserPendingTasksCountAsync(int userId);

        public Task<TaskDto> GetTaskByIdAsync(int taskId);

        public Task<bool?> MarkTaskAsCompletedAsync(int taskId);

        public Task<bool> IsTaskOwner(int userId, int taskId);
    }
}
