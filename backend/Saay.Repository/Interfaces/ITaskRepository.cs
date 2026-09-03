using Saay.Infrastructure.DTOs.TaskDTOs;

namespace Saay.Repository.Interfaces
{
    public interface ITaskRepository
    {
        public Task<int?> AddTaskAsync(Data.Entities.Task task, int userId);

        public Task<List<TaskDto>> GetUserTasksAsync(int userId, int pageNumber, int pageSize);

        public Task<int> UserTasksCountAsync(int userId);

        public Task<bool?> UpdateTaskAsync(int taskId, Data.Entities.Task newTask);

        public Task<bool?> DeleteTaskAsync(int taskId);

        public Task<int> UserCompletedTasksCount(int userId);

        public Task<int> UserPendingTasksCount(int userId);

        public Task<Data.Entities.Task> GetTaskByIdAsync(int taskId);
    }
}
