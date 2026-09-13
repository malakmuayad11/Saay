using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Saay.Infrastructure.DTOs.HabitDTOs;
using Saay.Services.Interfaces;
using System.Security.Claims;

namespace Saay.API.Controllers
{
    [Authorize]
    [Route("api/saay/habits")]
    [ApiController]
    public class HabitsController : ControllerBase
    {
        private readonly IHabitService _habitService;
        private readonly IHabitLogService _habitLogService;
        private readonly IOwnershipAuthorizationService _ownershipAuthorizationService;
        private readonly ILogger<GoalsController> _logger;

        public HabitsController(IHabitService habitService, IHabitLogService habitLogService, 
             IOwnershipAuthorizationService ownershipAuthorizationService, ILogger<GoalsController> logger)
        {
            _habitService = habitService;
            _habitLogService = habitLogService;
            _ownershipAuthorizationService = ownershipAuthorizationService;
            _logger = logger;
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> AddHabitAsync(AddHabitDto addHabitDto)
        {

            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, addHabitDto.UserId))
            {
                _logger.LogWarning("User {UserId} attmpted to add a habit without ownership.",
                   addHabitDto.UserId);
                return Forbid();
            }

            int? habitId = await _habitService.AddHabitAsync(addHabitDto);

            if (habitId == null)
                return NotFound("User with the specified ID does not exist.");

            return CreatedAtRoute("GetHabitById", new { habitId = habitId }, new HabitDto
            {
                HabitId = habitId.Value,
                UserId = addHabitDto.UserId,
                Title = addHabitDto.Title,
                ReasonForHabit = addHabitDto.ReasonForHabit,
                Steps = addHabitDto.Steps,
                TargetDuration = addHabitDto.TargetDuration
            });
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("{userId}/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<ICollection<HabitDto>>> GetUserHabitsAsync(int userId, int pageNumber = 1, int pageSize = 10)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
            {
                _logger.LogWarning("User {userId} attmpted to get another user's habit without ownership.",
                  userId);
                return Forbid();
            }

            List<HabitDto> habits = await _habitService.GetUserHabitsAsync(userId, pageNumber, pageSize);

            if (habits == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(habits);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<int?>> GetUserHabitsCountAsync(int userId)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
            {
                _logger.LogWarning("User {userId} attmpted to get another user's habits count without ownership.",
                   userId);
                return Forbid();
            }

            int? count = await _habitService.UserHabitsCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> UpdateHabit(UpdateHabitDto updateHabitDto)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _ownershipAuthorizationService.IsHabitOwner(User, updateHabitDto.HabitId))
            {
                _logger.LogWarning("User {userId} attmpted to update a habit without ownership.",
                   userId);
                return Forbid();
            }

            bool? result = await _habitService.UpdateHabitAsync(updateHabitDto);
            if (result == null)
                return NotFound("Habit with the specified ID does not exist.");

            if (result == false)
                return StatusCode(500, "An error occurred while updating the habit.");

            return Ok(result);
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpDelete("{habitId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> DeleteHabit(int habitId)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _ownershipAuthorizationService.IsHabitOwner(User, habitId))
            {
                _logger.LogWarning("User {userId} attmpted to update a habit without ownership.",
                   userId);
                return Forbid();
            }

            bool? result = await _habitService.DeleteHabitAsync(habitId);
            if (result == null)
                return NotFound("Habit with the specified ID does not exist.");
            if (result == false)
                return StatusCode(500, "An error occurred while deleting the habit.");
            return Ok(result);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("completed/count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<int?>> GetUserCompletedHabitsCountAsync(int userId)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
            {
                _logger.LogWarning("User {userId} attmpted to get another user's completed habits count without ownership.",
                   userId);
                return Forbid();
            }

            int? count = await _habitService.UserCompletedHabitsCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("pending/count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<int?>> GetUserPendingHabitsCountAsync(int userId)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
            {
                _logger.LogWarning("User {userId} attmpted to get another user's pending habits count without ownership.",
                   userId);
                return Forbid();
            }

            int? count = await _habitService.UserPendingHabitsCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("{habitId}", Name = "GetHabitById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<HabitDto>> GetHabitByIdAsync(int habitId)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _ownershipAuthorizationService.IsHabitOwner(User, habitId))
            {
                _logger.LogWarning("User {userId} attmpted to get a habit without ownership.",
                   userId);
                return Forbid();
            }

            HabitDto habit = await _habitService.GetHabitByIdAsync(habitId);
            if (habit == null)
                return NotFound("Habit with the specified ID does not exist.");
            return Ok(habit);
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpPost("mark-completed/{habitId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult>MarkHabitAsCompletedTodayAsync(int habitId)
        {
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _ownershipAuthorizationService.IsHabitOwner(User, habitId))
            {
                _logger.LogWarning("User {userId} attmpted to mark a habit as completed without ownership.",
                   userId);
                return Forbid();
            }

            (bool? isMarked, string message) result = await _habitLogService.MarkHabitAsCompletedTodayAsync(habitId);
            if (result.isMarked == null)
                return NotFound("Habit with the specified ID does not exist.");
            if (result.isMarked == false)
                return BadRequest(result.message);
            return Ok(result.isMarked);
        }
    }
}
