using Microsoft.AspNetCore.Authorization;
using Saay.Services.Authorization.Requirements;
using Saay.Services.Interfaces;
using System.Security.Claims;

namespace Saay.Services.Authorization.Handlers
{
    public class GroupOwnerHandler : AuthorizationHandler<GroupOwnerRequirement, int>
    {
        private readonly IUserGroupService _groupService;

        public GroupOwnerHandler(IUserGroupService groupService)
        {
            _groupService = groupService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GroupOwnerRequirement requirement,
            int groupId)
        {
            string? userId = context.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int authenticatedUserId))
            {
                bool isOwner = await _groupService.IsGroupOwnerAsync(
                    authenticatedUserId, groupId) ?? false;

                if (isOwner)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
