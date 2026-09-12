using Microsoft.AspNetCore.Authorization;
using Saay.Services.Authorization.Requirements;
using Saay.Services.Interfaces;
using System.Security.Claims;

namespace Saay.Services.Authorization.Handlers
{
    public class GoalOwnerHandler : AuthorizationHandler<GoalOwnerRequirement, int>
    {
        private readonly IGoalService _goalService;

        public GoalOwnerHandler(IGoalService goalService)
        {
            _goalService = goalService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GoalOwnerRequirement requirement,
            int goalId)
        {
            string? userId = context.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int authenticatedUserId) &&
                await _goalService.IsGoalOwner(authenticatedUserId, goalId))
            {
                context.Succeed(requirement);
            }
        }
    }
}