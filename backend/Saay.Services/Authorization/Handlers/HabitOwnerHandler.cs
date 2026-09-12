using Microsoft.AspNetCore.Authorization;
using Saay.Services.Authorization.Requirements;
using Saay.Services.Interfaces;
using System.Security.Claims;

namespace Saay.Services.Authorization.Handlers
{
    public class HabitOwnerHandler : AuthorizationHandler<HabitOwnerRequirement, int>
    {
        private readonly IHabitService _HabitService;

        public HabitOwnerHandler(IHabitService HabitService)
        {
            _HabitService = HabitService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HabitOwnerRequirement requirement,
            int habitId)
        {
            string? userId = context.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int authenticatedUserId) &&
                await _HabitService.IsHabitOwner(authenticatedUserId, habitId))
            {
                context.Succeed(requirement);
            }
        }
    }
}