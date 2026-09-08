using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saay.Infrastructure.DTOs.HabitDTOs;
using Saay.Services.Interfaces;

namespace Saay.API.Controllers
{
    [Authorize]
    [Route("api/saay/habits")]
    [ApiController]
    public class HabitsController : ControllerBase
    {
        private readonly IHabitService _habitService;
        private readonly IHabitLogService _habitLogService;

        public HabitsController(IHabitService habitService, IHabitLogService habitLogService)
        {
            _habitService = habitService;
            _habitLogService = habitLogService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddHabitAsync(AddHabitDto addHabitDto)
        {
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

        [HttpGet("{userId}/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<HabitDto>>> GetUserHabitsAsync(int userId, int pageNumber = 1, int pageSize = 10)
        {
            List<HabitDto> habits = await _habitService.GetUserHabitsAsync(userId, pageNumber, pageSize);

            if (habits == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(habits);
        }

        [HttpGet("count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int?>> GetUserHabitsCountAsync(int userId)
        {
            int? count = await _habitService.UserHabitsCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateHabit(UpdateHabitDto updateHabitDto)
        {
            bool? result = await _habitService.UpdateHabitAsync(updateHabitDto);
            if (result == null)
                return NotFound("Habit with the specified ID does not exist.");

            if (result == false)
                return StatusCode(500, "An error occurred while updating the habit.");

            return Ok(result);
        }

        [HttpDelete("{habitId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteHabit(int habitId)
        {
            bool? result = await _habitService.DeleteHabitAsync(habitId);
            if (result == null)
                return NotFound("Habit with the specified ID does not exist.");
            if (result == false)
                return StatusCode(500, "An error occurred while deleting the habit.");
            return Ok(result);
        }

        [HttpGet("completed/count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int?>> GetUserCompletedHabitsCountAsync(int userId)
        {
            int? count = await _habitService.UserCompletedHabitsCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [HttpGet("pending/count/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int?>> GetUserPendingHabitsCountAsync(int userId)
        {
            int? count = await _habitService.UserPendingHabitsCountAsync(userId);

            if (count == null)
                return NotFound("User with the specified ID does not exist.");

            return Ok(count);
        }

        [HttpGet("{habitId}", Name = "GetHabitById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HabitDto>> GetHabitByIdAsync(int habitId)
        {
            HabitDto habit = await _habitService.GetHabitByIdAsync(habitId);
            if (habit == null)
                return NotFound("Habit with the specified ID does not exist.");
            return Ok(habit);
        }

        [HttpPost("mark-completed/{habitId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult>MarkHabitAsCompletedTodayAsync(int habitId)
        {
            (bool? isMarked, string message) result = await _habitLogService.MarkHabitAsCompletedTodayAsync(habitId);
            if (result.isMarked == null)
                return NotFound("Habit with the specified ID does not exist.");
            if (result.isMarked == false)
                return BadRequest(result.message);
            return Ok(result.isMarked);
        }
    }
}
