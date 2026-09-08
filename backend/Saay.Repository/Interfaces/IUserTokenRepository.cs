namespace Saay.Repository.Interfaces
{
    public interface IUserTokenRepository
    {
        public Task<bool?> LoginAsync(int userId, string refreshTokenHash, DateTime refreshTokenExpiresAt);
        public Task<(DateTime? expiresAt, DateTime? revokedAt, string hash)> GetTokenDataForUserAsync(int userId);
    }
}
