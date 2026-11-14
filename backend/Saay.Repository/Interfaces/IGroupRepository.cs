using Saay.Data.Entities;
using Saay.Infrastructure.DTOs.GroupDTOs;

namespace Saay.Repository.Interfaces
{
    public interface IGroupRepository
    {
        public Task<int?> AddGroupAsync(Group group);

        public Task<string?> GetGroupName (int groupId);

        public Task<List<GroupDto>> GetUserGroupsAsync(int userId);
    }
}
