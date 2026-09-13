using Saay.Infrastructure.DTOs.TokenDTOs;

namespace Saay.Services.Interfaces
{
    public interface IUserTokenService
    {
        public Task<bool?> LoginAsync(int userId, string refreshToken, DateTime refreshTokenExpiresAt);

        public Task<TokenDto> GetTokenDataForUserAsync(int userId);

        public Task<bool?> RefreshAsync(int userId, string refreshToken, DateTime refreshTokenExpiresAt);
    }
}
