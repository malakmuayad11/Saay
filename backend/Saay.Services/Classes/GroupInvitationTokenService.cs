using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;
using Saay.Data.Entities;
using Saay.Services.Interfacess;

namespace Saay.Services.Classes
{
    public class GroupInvitationTokenService : IGroupInvitationTokenService
    {
        private readonly IGroupInvitationTokenRepository _groupInvitationTokenRepository;

        private readonly IHashService _hashService;

        public GroupInvitationTokenService(IGroupInvitationTokenRepository groupInvitationTokenRepository, IHashService hashService)
        {
            _groupInvitationTokenRepository = groupInvitationTokenRepository;
            _hashService = hashService;
        }

        public async Task<int?> AddGroupInvitationTokenAsync(int groupId, int userId, string token) =>
            await _groupInvitationTokenRepository.AddGroupInvitationAsync(new GroupInvitationToken
            {
                GroupId = groupId,
                UserId = userId,
                TokenHash = _hashService.HashToken(token),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            });

        public async Task<(int groupId, int userId)?> GetGroupInvitationTokenAsync(string token)
        {
            string tokenHash = _hashService.HashToken(token);

            GroupInvitationToken?  groupInvitationToken = await _groupInvitationTokenRepository.GetGroupInvitationTokenAsync(tokenHash);
            if (groupInvitationToken == null)
                return null;

            return (groupInvitationToken.GroupId, groupInvitationToken.UserId);
        }
    }
}
