using Saay.Data.Entities;

namespace Saay.Repository.Interfaces
{
    public interface IGroupInvitationTokenRepository
    {
        public Task<int?> AddGroupInvitationAsync(GroupInvitationToken groupInvitationToken);

        public Task<GroupInvitationToken?> GetGroupInvitationTokenAsync(string token);
    }
}
