using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Data.Entities;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class UserTokenRpository : IUserTokenRepository
    {
        private readonly SaayContext _context;
        public UserTokenRpository(SaayContext context)
        {
            _context = context;
        }
        public async Task<bool?> LoginAsync(int userId, string refreshTokenHash, DateTime refreshTokenExpiresAt)
        {
            if (!await _context.Users.AnyAsync(u => u.UserId == userId))
                return null;

            Token token = new Token
            {
                UserId = userId,
                RefreshTokenHash = refreshTokenHash,
                ExpiresAt = refreshTokenExpiresAt,
                RevokedAt = null
            };

            await _context.Tokens.AddAsync(token);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<(DateTime? expiresAt, DateTime? revokedAt, string hash)> GetTokenDataForUserAsync(int userId)
        {
            if (!await _context.Users.AnyAsync(u => u.UserId == userId))
                return (null, null, null);

            var tokenData = await _context.Tokens
                .Where(u => u.UserId == userId)
                .Select(ut => new { ut.ExpiresAt, ut.RevokedAt, ut.RefreshTokenHash })
                .OrderByDescending(ut => ut.ExpiresAt)
                .FirstOrDefaultAsync();

            if (tokenData is null)
                return (null, null, null);

            return (tokenData.ExpiresAt, tokenData.RevokedAt, tokenData.RefreshTokenHash);
        }

    }
}
