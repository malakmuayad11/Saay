using Microsoft.AspNetCore.Authorization;
using Saay.Infrastructure.Authorization.Requirements;
using System.Security.Claims;

namespace Saay.Infrastructure.Authorization.Handlers
{
    public class UserOwnerHandler : AuthorizationHandler<UserOwnerRequirement, int>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserOwnerRequirement requirement,
        int userID)
        {
            string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int authenticatedUserId) &&
                authenticatedUserId == userID)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
