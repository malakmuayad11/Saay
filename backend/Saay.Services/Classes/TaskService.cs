using Saay.Infrastructure.DTOs.TaskDTOs;
using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;

namespace Saay.Services.Classes
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<int?> AddTaskAsync(Infrastructure.DTOs.TaskDTOs.AddTaskDto addTaskDto)
        {
            if(!await _userRepository.DoesUserExist(addTaskDto.UserId))
                return null; // User does not exist
            
            var taskEntity = new Data.Entities.Task
            {
                UserId = addTaskDto.UserId,
                CategoryId = addTaskDto.CategoryId,
                Title = addTaskDto.Title,
                Repetition = addTaskDto.Repetition,
                DueDate = addTaskDto.DueDate,
                DueTime = addTaskDto.DueTime
            };

            return await _taskRepository.AddTaskAsync(taskEntity, addTaskDto.UserId);
        }
    
        public async Task<List<TaskDto>> GetUserTasksAsync(int userId, int pageNumber, int pageSize)
        {
            if(!await _userRepository.DoesUserExist(userId))
                return null; // User does not exist

            return await _taskRepository.GetUserTasksAsync(userId, pageNumber, pageSize);
        }
    
        public async Task<int?> UserTasksCountAsync(int userId)
        {
            if (!await _userRepository.DoesUserExist(userId))
                return null; // User not found

            return await _taskRepository.UserTasksCountAsync(userId);
        }

        public async Task<bool?> UpdateTaskAsync(UpdateTaskDto updateTaskDto)
        {
            Data.Entities.Task task = new Data.Entities.Task
            {
                TaskId = updateTaskDto.TaskId,
                CategoryId = updateTaskDto.CategoryId,
                Title = updateTaskDto.Title,
                Repetition = updateTaskDto.Repetition,
                DueDate = updateTaskDto.DueDate,
                DueTime = updateTaskDto.DueTime,
                IsDone = updateTaskDto.IsDone,
                IsReminderSent = updateTaskDto.IsReminderSent
            };

            return await _taskRepository.UpdateTaskAsync(updateTaskDto.TaskId, task);
        }

        public async Task<bool?> DeleteTaskAsync(int taskId) =>
            await _taskRepository.DeleteTaskAsync(taskId);

        public async Task<int?> UserCompletedTasksCountAsync(int userId)
        {
            if (!await _userRepository.DoesUserExist(userId))
                return null; // User not found

            return await _taskRepository.UserCompletedTasksCount(userId);
        }

        public async Task<int?> UserPendingTasksCountAsync(int userId)
        {
            if (!await _userRepository.DoesUserExist(userId))
                return null; // User not found
            return await _taskRepository.UserPendingTasksCount(userId);
        }

        public async Task<TaskDto> GetTaskByIdAsync(int taskId) =>
            await _taskRepository.GetTaskByIdAsync(taskId);
    }
}
