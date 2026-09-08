using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saay.Infrastructure.DTOs.GoalCategoryDTOs;
using Saay.Infrastructure.DTOs.GoalDTOs;
using Saay.Services.Interfaces;

namespace Saay.API.Controllers
{
    [Route("api/saay/goals")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalService _goalService;
        private readonly IGoalCategoryService _goalCategoryService;

        public GoalsController(IGoalService goalService, IGoalCategoryService goalCategoryService)
        {
            _goalService = goalService;
            _goalCategoryService = goalCategoryService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddGoalAsync(AddGoalDto addGoalDto)
        {
            int? goalId = await _goalService.AddGoalAsync(addGoalDto);

            if (goalId == null)
                return NotFound("User with the specified ID does not exist.");

            return CreatedAtRoute("GetGoalById", new { goalId = goalId }, new
            {
                GoalID = goalId,
                CategoryID = addGoalDto.GoalCategoryId,
                Title = addGoalDto.Title,
                TimeFrame = addGoalDto.TimeFrame,
                Deadline = addGoalDto.Deadline,
                IsDone = false // Assuming a new goal is not done by default
            });
        }

        [Authorize]
        [HttpGet("{userId}/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<GoalDto>>> GetUserGoalsAsync(int userId, int pageNumber = 1, int pageSize = 10)
        {
            List<GoalDto> goals = await _goalService.GetUserGoalsAsync(userId, pageNumber, pageSize);

            if (goals == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(goals);
        }

        [Authorize]
        [HttpGet("count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int?>> GetUserGoalsCountAsync(int userId)
        {
            int? count = await _goalService.UserGoalsCountAsync(userId);

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
        public async Task<IActionResult> UpdateGoal(UpdateGoalDto updateGoalDto)
        {
            bool? result = await _goalService.UpdateGoalAsync(updateGoalDto);
            if (result == null)
                return NotFound("Goal with the specified ID does not exist.");

            if (result == false)
                return StatusCode(500, "An error occurred while updating the goal.");

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{goalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteGoal(int goalId)
        {
            bool? result = await _goalService.DeleteGoalAsync(goalId);
            if (result == null)
                return NotFound("Goal with the specified ID does not exist.");
            if (result == false)
                return StatusCode(500, "An error occurred while deleting the goal.");
            return Ok(result);
        }

        [Authorize]
        [HttpGet("completed/count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int?>> GetUserCompletedGoalsCountAsync(int userId)
        {
            int? count = await _goalService.UserCompletedGoalsCountAsync(userId);

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
        public async Task<ActionResult<int?>> GetUserPendingGoalsCountAsync(int userId)
        {
            int? count = await _goalService.UserPendingGoalsCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [Authorize]
        [HttpGet("{goalId}", Name = "GetGoalById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GoalDto>> GetGoalByIdAsync(int goalId)
        {
            GoalDto goal = await _goalService.GetGoalByIdAsync(goalId);
            if (goal == null)
                return NotFound("Goal with the specified ID does not exist.");
            return Ok(goal);
        }

        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ICollection<GoalCategoryDto>>> GetAllTasksCategoriesAsync()
        {
            List<GoalCategoryDto> tasksCategories = await _goalCategoryService.GetAllGoalCategoriesAsync();

            if (tasksCategories == null)
                return NotFound("No categories found.");

            return Ok(tasksCategories);
        }
    }
}
