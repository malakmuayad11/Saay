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
    }
}
