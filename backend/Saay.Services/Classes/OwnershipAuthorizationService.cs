using Microsoft.AspNetCore.Authorization;
using Saay.Services.Interfaces;
using System.Security.Claims;

namespace Saay.Services.Classes
{
    public class OwnershipAuthorizationService : IOwnershipAuthorizationService
    {
        private readonly IAuthorizationService _authorizationService;

        public OwnershipAuthorizationService(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        public async Task<bool> IsOwnerAsync(
        ClaimsPrincipal user,
        int resourceId)
        {
            var result = await _authorizationService.AuthorizeAsync(
                user,
                resourceId,
                "UserOwner");

            return result.Succeeded;
        }

        public async Task<bool> IsEmailOwnerAsync(
        ClaimsPrincipal user,
        string email)
        {
            var result = await _authorizationService.AuthorizeAsync(
                user,
                email,
                "EmailOwner");

            return result.Succeeded;
        }
        public async Task<bool> IsGoalOwner(
        ClaimsPrincipal user,
        int goalId)
        {
            var result = await _authorizationService.AuthorizeAsync(
                user,
                goalId,
                "GoalOwner");

            return result.Succeeded;
        }

        public async Task<bool> IsHabitOwner(
        ClaimsPrincipal user,
        int habitId)
        {
            var result = await _authorizationService.AuthorizeAsync(
                user,
                habitId,
                "HabitOwner");

            return result.Succeeded;
        }

        public async Task<bool> IsTaskOwner(
        ClaimsPrincipal user,
        int taskId)
        {
            var result = await _authorizationService.AuthorizeAsync(
                user,
                taskId,
                "TaskOwner");

            return result.Succeeded;
        }
    }
}
