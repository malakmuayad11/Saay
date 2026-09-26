using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Saay.Infrastructure.DTOs.GoalCategoryDTOs;
using Saay.Infrastructure.DTOs.GoalDTOs;
using Saay.Services.Interfaces;
using System.Security.Claims;

namespace Saay.API.Controllers
{
    [Route("api/saay/goals")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalService _goalService;
        private readonly IGoalCategoryService _goalCategoryService;
        private readonly IOwnershipAuthorizationService _ownershipAuthorizationService;
        private readonly ILogger<GoalsController> _logger;

        public GoalsController(IGoalService goalService, IGoalCategoryService goalCategoryService
            , IOwnershipAuthorizationService ownershipAuthorizationService, ILogger<GoalsController> logger)
        {
            _goalService = goalService;
            _goalCategoryService = goalCategoryService;
            _ownershipAuthorizationService = ownershipAuthorizationService;
            _logger = logger;
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> AddGoalAsync(AddGoalDto addGoalDto)
        {

            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, addGoalDto.UserId))
            {
                _logger.LogWarning("User {UserId} attmpted to add a goal without ownership.",
                    addGoalDto.UserId);
                return Forbid();
            }

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

        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize]
        [HttpGet("{userId}/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<ICollection<GoalDto>>> GetUserGoalsAsync(int userId, int pageNumber = 1, int pageSize = 10)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
            {
                _logger.LogWarning("User {userId} attmpted to view a goal without ownership.",
                  userId);
                return Forbid();
            }

            List<GoalDto> goals = await _goalService.GetUserGoalsAsync(userId, pageNumber, pageSize);

            if (goals == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(goals);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize]
        [HttpGet("count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<int?>> GetUserGoalsCountAsync(int userId)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
            {
                _logger.LogWarning("User {userId} attmpted to get another user's goals count without ownership.",
                  userId);
                return Forbid();
            }

            int? count = await _goalService.UserGoalsCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> UpdateGoal(UpdateGoalDto updateGoalDto)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _ownershipAuthorizationService.IsGoalOwner(User, updateGoalDto.GoalId))
            {
                _logger.LogWarning("User {userId} attmpted to update a goal without ownership.",
                   userId);
                return Forbid();
            }

            bool? result = await _goalService.UpdateGoalAsync(updateGoalDto);
            if (result == 
                null)
                return NotFound("Goal with the specified ID does not exist.");

            if (result == false)
                return StatusCode(500, "An error occurred while updating the goal.");

            return Ok(result);
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [Authorize]
        [HttpDelete("{goalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> DeleteGoal(int goalId)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _ownershipAuthorizationService.IsGoalOwner(User, goalId))
            {
                _logger.LogWarning("User {userId} attmpted to delete a goal without ownership.",
                   userId);
                return Forbid();
            }

            bool? result = await _goalService.DeleteGoalAsync(goalId);
            if (result == null)
                return NotFound("Goal with the specified ID does not exist.");
            if (result == false)
                return StatusCode(500, "An error occurred while deleting the goal.");
            return Ok(result);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize]
        [HttpGet("completed/count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<int?>> GetUserCompletedGoalsCountAsync(int userId)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
            {
                _logger.LogWarning("User {userId} attmpted to get another user's completed goals count without ownership.",
                   userId);
                return Forbid();
            }

            int? count = await _goalService.UserCompletedGoalsCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize]
        [HttpGet("pending/count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<int?>> GetUserPendingGoalsCountAsync(int userId)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
            {
                _logger.LogWarning("User {userId} attmpted to get another user's pending goals count without ownership.",
                   userId);
                return Forbid();
            }

            int? count = await _goalService.UserPendingGoalsCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize]
        [HttpGet("{goalId}", Name = "GetGoalById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<GoalDto>> GetGoalByIdAsync(int goalId)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _ownershipAuthorizationService.IsGoalOwner(User, goalId))
            {
                _logger.LogWarning("User {userId} attmpted to get a goal without ownership.",
                   userId);
                return Forbid();
            }

            GoalDto goal = await _goalService.GetGoalByIdAsync(goalId);
            if (goal == null)
                return NotFound("Goal with the specified ID does not exist.");
            return Ok(goal);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<ICollection<GoalCategoryDto>>> GetAllTasksCategoriesAsync()
        {
            List<GoalCategoryDto> tasksCategories = await _goalCategoryService.GetAllGoalCategoriesAsync();

            if (tasksCategories == null)
                return NotFound("No categories found.");

            return Ok(tasksCategories);
        }
    }
}
