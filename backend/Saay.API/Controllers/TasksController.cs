using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saay.Infrastructure.DTOs.CategoryDTOs;
using Saay.Infrastructure.DTOs.TaskDTOs;
using Saay.Services.Interfaces;
using System.Security.Claims;

namespace Saay.API.Controllers
{
    [Route("api/saay/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ITaskCategoryService _taskCategoryService;
        private readonly IOwnershipAuthorizationService _ownershipAuthorizationService;

        public TasksController(ITaskService taskService, ITaskCategoryService taskCategoryService,
            IOwnershipAuthorizationService ownershipAuthorizationService)
        {
            _taskService = taskService;
            _taskCategoryService = taskCategoryService;
            _ownershipAuthorizationService = ownershipAuthorizationService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddTaskAsync(AddTaskDto addTaskDto)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, addTaskDto.UserId))
                return Forbid();

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

        [Authorize]
        [HttpGet("{userId}/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ICollection<TaskDto>>> GetUserTasksAsync(int userId, int pageNumber = 1, int pageSize = 10)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
                return Forbid();

            List<TaskDto> tasks = await _taskService.GetUserTasksAsync(userId, pageNumber, pageSize);

            if (tasks == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(tasks);
        }

        [Authorize]
        [HttpGet("count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<int?>> GetUserTasksCountAsync(int userId)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
                return Forbid();

            int? count = await _taskService.UserTasksCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateTask(UpdateTaskDto updateTaskDto)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _ownershipAuthorizationService.IsTaskOwner(User, updateTaskDto.TaskId))
                return Forbid();

            bool? result = await _taskService.UpdateTaskAsync(updateTaskDto);
            if (result == null)
                return NotFound("Task with the specified ID does not exist.");

            if (result == false)
                return StatusCode(500, "An error occurred while updating the task.");

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{taskId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _ownershipAuthorizationService.IsTaskOwner(User, taskId))
                return Forbid();

            bool? result = await _taskService.DeleteTaskAsync(taskId);
            if (result == null)
                return NotFound("Task with the specified ID does not exist.");
            if (result == false)
                return StatusCode(500, "An error occurred while deleting the task.");
            return Ok(result);
        }

        [Authorize]
        [HttpGet("completed/count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<int?>> GetUserCompletedTasksCountAsync(int userId)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
                return Forbid();

            int? count = await _taskService.UserCompletedTasksCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [Authorize]
        [HttpGet("pending/count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<int?>> GetUserPendingTasksCountAsync(int userId)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
                return Forbid();
            int? count = await _taskService.UserPendingTasksCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [Authorize]
        [HttpGet("{taskId}", Name = "GetTaskById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<TaskDto>> GetTaskByIdAsync(int taskId)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _ownershipAuthorizationService.IsTaskOwner(User, taskId))
                return Forbid();

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
