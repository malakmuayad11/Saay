using Saay.Infrastructure.DTOs.GroupDTOs;

namespace Saay.Services.Interfaces
{
    public interface IGroupService
    {
        public Task<int?> CreateGroupAsync(AddGroupDto addGroupDto);
        public Task<bool?> InviteUserToGroup(int groupId, string email);
    }
}
