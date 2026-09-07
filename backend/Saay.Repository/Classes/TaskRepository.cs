using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Infrastructure.DTOs.TaskDTOs;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class TaskRepository : ITaskRepository
    {
        private readonly SaayContext _context;

        public TaskRepository(SaayContext context)
        {
            _context = context;
        }

        public async Task<int?> AddTaskAsync(Data.Entities.Task task, int userId)
        {
            Data.Entities.Task newTask = new Data.Entities.Task
            {
                UserId = userId,
                TaskCategoryId = task.TaskCategoryId,
                Title = task.Title,
                Repetition = task.Repetition,
                DueDate = task.DueDate,
                DueTime = task.DueTime,
                IsDone = false,
                IsReminderSent = false
            };

            _context.Tasks.Add(newTask);
            if (await _context.SaveChangesAsync() > 0)
                return newTask.TaskId;

            return null;
        }

        public async Task<List<TaskDto>> GetUserTasksAsync(int userId,
            int pageNumber, int pageSize) =>
            await _context.Tasks
                .Select(task => new TaskDto
                {
                    TaskId = task.TaskId,
                    TaskCategoryTitle = task.TaskCategory.Title,
                    Title = task.Title,
                    IsDone = task.IsDone,
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

        public async Task<int> UserTasksCountAsync(int userId) => await _context.Tasks
                .Where(task => task.UserId == userId)
                .CountAsync();

        public async Task<bool?> UpdateTaskAsync(int taskId, Data.Entities.Task newTask)
        {
            Data.Entities.Task task = await _context.Tasks.FindAsync(taskId);

            if (task == null) return null; // Task not found

            task.TaskCategoryId = newTask.TaskCategoryId;
            task.Title = newTask.Title;
            task.Repetition = newTask.Repetition;
            task.DueDate = newTask.DueDate;
            task.DueTime = newTask.DueTime;
            task.IsDone = newTask.IsDone;
            task.IsReminderSent = newTask.IsReminderSent;

            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool?> DeleteTaskAsync(int taskId)
        {
            Data.Entities.Task task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return null;
            _context.Tasks.Remove(task);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> UserCompletedTasksCount(int userId) =>
             await _context.Tasks
                .Where(task => task.UserId == userId && task.IsDone)
                .CountAsync();

        public async Task<int> UserPendingTasksCount(int userId) =>
             await _context.Tasks
                .Where(task => task.UserId == userId && !task.IsDone)
                .CountAsync();

        public async Task<TaskDto> GetTaskByIdAsync(int taskId) =>
            await _context.Tasks
            .Select(task => new TaskDto
            {
                TaskId = task.TaskId,
                TaskCategoryTitle = task.TaskCategory.Title,
                Title = task.Title,
                IsDone = task.IsDone
            })
            .FirstOrDefaultAsync(task => task.TaskId == taskId);
    }
}
