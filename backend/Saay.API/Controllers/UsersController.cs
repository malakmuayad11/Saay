using Microsoft.AspNetCore.Mvc;
using Saay.Infrastructure.DTOs.UserDTOs;
using Saay.Services.Interfaces;

namespace Saay.API.Controllers
{
    [Route("api/saay/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddUserAsync(AddUserDto addUserDto)
        {
            int? userId = await _userService.AddUserAsync(addUserDto);

            if (userId == null)
                return BadRequest("User already exists with the provided email.");
            else
            {
                GetUserDto getUserDto = new GetUserDto
                {
                    UserId = userId.Value,
                    FirstName = addUserDto.FirstName,
                    LastName = addUserDto.LastName,
                    Email = addUserDto.Email,
                    ProfilePictureUrl = addUserDto.ProfilePictureURL,
                    Mission = null
                };
                return CreatedAtRoute("GetUserById", new { userId = userId }, getUserDto);
            }
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUserAsync(int userId)
        {
            bool? result = await _userService.DeleteUserAsync(userId);
            if (result == true)
                return Ok("Account is deleted.");
            else if (result == null)
                return NotFound("User not found.");
            else
                return StatusCode(500, "An error occurred while deleting the user.");
        }

        [HttpPut("missions")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateMissionAsync(UpdateMissionDto updateMissionDto)
        {
            bool? result = await _userService.UpdateMissionAsync(updateMissionDto);
            if (result == true)
                return Ok("Mission is updated successfully.");
            else if (result == null)
                return NotFound("User is not found.");
            else
                return StatusCode(500, "An error occurred while updating the mission.");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            bool? result = await _userService.UpdateUserAsync(updateUserDto);
            if (result == true)
                return Ok("User is updated successfully.");
            else if (result == false)
                return BadRequest("Email already exists.");
            else if (result == null)
                return NotFound("User is not found.");
            else
                return StatusCode(500, "An error occurred while updating the user.");
        }

        [HttpPut("passwords")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdatePasswordAsync(UpdatePasswordDto updatePasswordDto)
        {
            bool? result = await _userService.UpdatePasswordAsync(updatePasswordDto);
            if (result == true)
                return Ok("Password is updated successfully.");
            else if (result == null)
                return NotFound("User is not found.");
            else
                return StatusCode(500, "An error occurred while updating the password.");
        }
        
        [HttpGet("{userId}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetUserDto>> GetUserByIdAsync(int userId)
        {
            GetUserDto? user = await _userService.GetUserByIdAsync(userId);

            if (user != null)
                return Ok(user);
            else
                return NotFound("User not found.");
        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetUserDto>> GetUserByEmailAsync(string email)
        {
            GetUserDto? user = await _userService.GetUserByEmailAsync(email);
            if (user != null)
                return Ok(user);
            else
                return NotFound("User not found.");
        }
    }
}
