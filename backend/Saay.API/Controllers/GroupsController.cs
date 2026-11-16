using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saay.Infrastructure.DTOs.GroupDTOs;
using Saay.Services.Interfaces;
using System.Security.Claims;

namespace Saay.API.Controllers
{
    [Route("api/saay/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IOwnershipAuthorizationService _ownershipAuthorizationService;
        private readonly ILogger<GroupsController> _logger;
        private readonly IUserGroupService _userGroupService;

        public GroupsController(IGroupService groupService,
            IOwnershipAuthorizationService ownershipAuthorizationService,
            ILogger<GroupsController> logger, IUserGroupService userGroupService)
        {
            _groupService = groupService;
            _ownershipAuthorizationService = ownershipAuthorizationService;
            _logger = logger;
            _userGroupService = userGroupService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateGroup(AddGroupDto addGroupDto)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, addGroupDto.UserId))
            {
                _logger.LogWarning("User {userId} attempted to create group without ownership.",
                    addGroupDto.UserId);
                return Forbid();
            }

            int? groupId = await _groupService.CreateGroupAsync(addGroupDto);
            if (groupId == null)
                return StatusCode(500, "An error occurred while creating the group.");

            return Ok(groupId);
        }

        [Authorize]
        [HttpPost("invite")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> InviteUserToGroup(InviteMemberDto inviteMemberDto)
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId) ||
                ! await _ownershipAuthorizationService.IsGroupOwner(User, inviteMemberDto.GroupId))
            {
                _logger.LogWarning("User {userId} attempted to invite a member to a group without ownership.",
                   userId);
                return Forbid();
            }

            bool? result = await _groupService.InviteUserToGroup(inviteMemberDto.GroupId, inviteMemberDto.UserEmail);

            if (result == null)
                return NotFound("Member with email is not found");

            if(result == false)
                return StatusCode(500, "An error occurred while inviting the member to the group.");

            return Ok(result);
        }

        [Authorize]
        [HttpPost("users")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> AddUserToGroup(AddMemberDto addMemberDto)
        {
            bool? result = await _userGroupService.AddUserGroupAsync(addMemberDto.Token);

            if (result == null)
                return NotFound("User is not found");

            if (result == false)
                return BadRequest("User is already a member of the group.");

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ICollection<GroupDto>>> GetUserGroupsAsync(int userId)
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int id) ||
                !await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
            {
                _logger.LogWarning("User {userId} attempted to invite a member to a group without ownership.",
                   userId);
                return Forbid();
            }

            List<GroupDto> groups = await _groupService.GetUserGroupsAsync(userId);

            if (groups == null) return NotFound("User not found");

            return Ok(groups);
        }

        [Authorize]
        [HttpDelete("{groupId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            if (!await _ownershipAuthorizationService.IsGroupOwner(User, groupId))
            {
                _logger.LogWarning("User {userId} attempted to delete group without ownership.",
                    groupId);
                return Forbid();
            }

            bool? result = await _groupService.DeleteGroupAsync(groupId);

            if (result == null)
                return NotFound("Group not found");

            if(result == false)
                return StatusCode(500, "An error occurred while deleting the group.");

            return Ok(result);
        }
    }
}
