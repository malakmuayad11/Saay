using System.Security.Claims;

namespace Saay.Services.Interfaces
{
    public interface IOwnershipAuthorizationService
    {
        public Task<bool> IsOwnerAsync(ClaimsPrincipal user, int resourceId);
    }
}
