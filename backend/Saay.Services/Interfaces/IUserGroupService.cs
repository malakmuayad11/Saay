using Saay.Data.Entities;

namespace Saay.Services.Interfaces
{
    public interface IUserGroupService
    {
        public enum enRole : byte
        {
            Admin = 0,
            Member = 1
        }

        public Task<bool?> AddUserGroupAsync(string token);

        public Task<bool?> IsGroupOwnerAsync(int userId, int groupId);

  
    }
}
