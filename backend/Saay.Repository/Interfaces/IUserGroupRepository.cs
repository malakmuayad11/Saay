using Saay.Data.Entities;

namespace Saay.Repository.Interfaces
{
    public interface IUserGroupRepository
    {
        public Task<bool?> AddUserGroupAsync(UserGroup userGroup);

        public Task<bool?> IsGroupOwnerAsync(int userId, int groupId);
    }
}
