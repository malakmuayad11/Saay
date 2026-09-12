using Microsoft.AspNetCore.Authorization;
using Saay.Services.Authorization.Requirements;
using Saay.Services.Interfaces;
using System.Security.Claims;

namespace Saay.Services.Authorization.Handlers
{
    public class EmailOwnerHandler
        : AuthorizationHandler<UserOwnerRequirement, string>
    {
        private readonly IUserService _userService;

        public EmailOwnerHandler(IUserService userService)
        {
            _userService = userService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UserOwnerRequirement requirement,
            string email)
        {
            string? userId = context.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int authenticatedUserId) &&
                await _userService.IsEmailOwner(
                    authenticatedUserId,
                    email))
            {
                context.Succeed(requirement);
            }
        }
    }
}