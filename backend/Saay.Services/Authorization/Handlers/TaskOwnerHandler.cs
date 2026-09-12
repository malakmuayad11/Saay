using Microsoft.AspNetCore.Authorization;
using Saay.Services.Authorization.Requirements;
using Saay.Services.Interfaces;
using System.Security.Claims;

namespace Saay.Services.Authorization.Handlers
{
    public class TaskOwnerHandler : AuthorizationHandler<TaskOwnerRequirement, int>
    {
        private readonly ITaskService _taskService;

        public TaskOwnerHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TaskOwnerRequirement requirement,
            int taskId)
        {
            string? userId = context.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int authenticatedUserId) &&
                await _taskService.IsTaskOwner(authenticatedUserId, taskId))
            {
                context.Succeed(requirement);
            }
        }
    }
}