using System.Security.Claims;

namespace Saay.Services.Interfaces
{
    public interface IOwnershipAuthorizationService
    {
        public Task<bool> IsOwnerAsync(ClaimsPrincipal user, int resourceId);

        public Task<bool> IsEmailOwnerAsync(ClaimsPrincipal user, string email);

        public Task<bool> IsGoalOwner(ClaimsPrincipal user, int goalId);

        public Task<bool> IsHabitOwner(ClaimsPrincipal user, int habitId);

        public Task<bool> IsTaskOwner(ClaimsPrincipal user, int taskId);
    }
}
