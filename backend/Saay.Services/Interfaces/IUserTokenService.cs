namespace Saay.Services.Interfaces
{
    public interface IUserTokenService
    {
        public Task<bool?> LoginAsync(int userId, string refreshToken, DateTime refreshTokenExpiresAt);
    }
}
