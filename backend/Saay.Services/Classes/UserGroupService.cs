using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;
using Saay.Services.Interfacess;

namespace Saay.Services.Classes
{
    public class UserGroupService : IUserGroupService
    {
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IGroupInvitationTokenService _groupInvitationTokenService;

        public UserGroupService(IUserGroupRepository userGroupRepository, IGroupInvitationTokenService groupInvitationTokenService)
        {
            _userGroupRepository = userGroupRepository;
            _groupInvitationTokenService = groupInvitationTokenService;
        }

        public async Task<bool?> AddUserGroupAsync(string token)
        {
            var groupInvitationToken = await _groupInvitationTokenService.GetGroupInvitationTokenAsync(token);
            
            if (groupInvitationToken == null)
                return null;

            var (groupId, userId) = groupInvitationToken.Value;

            return await _userGroupRepository.AddUserGroupAsync(new Data.Entities.UserGroup
            {
                GroupId = groupId,
                UserId = userId,
                Role = (byte)IUserGroupService.enRole.Member
            });
        }

        public Task<bool?> IsGroupOwnerAsync(int userId, int groupId) =>
            _userGroupRepository.IsGroupOwnerAsync(userId, groupId);
    }
}
