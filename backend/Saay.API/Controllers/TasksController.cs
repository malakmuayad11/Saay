using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Saay.Infrastructure.DTOs.CategoryDTOs;
using Saay.Infrastructure.DTOs.TaskDTOs;
using Saay.Services.Interfaces;

namespace Saay.API.Controllers
{
    [Route("api/saay/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ITaskCategoryService _taskCategoryService;

        public TasksController(ITaskService taskService, ITaskCategoryService taskCategoryService)
        {
            _taskService = taskService;
            _taskCategoryService = taskCategoryService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTaskAsync(AddTaskDto addTaskDto)
        {
            int? taskId = await _taskService.AddTaskAsync(addTaskDto);

            if (taskId == null)
                return NotFound("User with the specified ID does not exist.");

            return CreatedAtRoute("GetTaskById", new { taskId = taskId }, 
                new
                {
                    TaskId = taskId.Value,
                    UserId = addTaskDto.UserId,
                    TaskCategoryId = addTaskDto.TaskCategoryId,
                    Title = addTaskDto.Title,
                    DueDate = addTaskDto.DueDate,
                    DueTime = addTaskDto.DueTime,
                });
        }

        [HttpGet("{userId}/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ICollection<TaskDto>>> GetUserTasksAsync(int userId, int pageNumber = 1, int pageSize = 10)
        {
            List<TaskDto> tasks = await _taskService.GetUserTasksAsync(userId, pageNumber, pageSize);

            if (tasks == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(tasks);
        }

        [HttpGet("count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int?>> GetUserTasksCountAsync(int userId)
        {
            int? count = await _taskService.UserTasksCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTask(UpdateTaskDto updateTaskDto)
        {
            bool? result = await _taskService.UpdateTaskAsync(updateTaskDto);
            if (result == null)
                return NotFound("Task with the specified ID does not exist.");

            if (result == false)
                return StatusCode(500, "An error occurred while updating the task.");

            return Ok(result);
        }

        [HttpDelete("{taskId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            bool? result = await _taskService.DeleteTaskAsync(taskId);
            if (result == null)
                return NotFound("Task with the specified ID does not exist.");
            if (result == false)
                return StatusCode(500, "An error occurred while deleting the task.");
            return Ok(result);
        }

        [HttpGet("completed/count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int?>> GetUserCompletedTasksCountAsync(int userId)
        {
            int? count = await _taskService.UserCompletedTasksCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [HttpGet("pending/count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int?>> GetUserPendingTasksCountAsync(int userId)
        {
            int? count = await _taskService.UserPendingTasksCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [HttpGet("{taskId}", Name = "GetTaskById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskDto>> GetTaskByIdAsync(int taskId)
        {
            TaskDto task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
                return NotFound("Task with the specified ID does not exist.");
            return Ok(task);
        }

        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ICollection<TaskCategoryDto>>> GetAllTasksCategoriesAsync()
        {
            List<TaskCategoryDto> tasksCategories = await _taskCategoryService.GetAllTasksCategoriesAsync();

            if (tasksCategories == null)
                return NotFound("No categories found.");

            return Ok(tasksCategories);
        }
    }
}
