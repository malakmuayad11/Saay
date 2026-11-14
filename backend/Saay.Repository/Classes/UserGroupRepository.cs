using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Data.Entities;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly SaayContext _context;

        public UserGroupRepository(SaayContext context)
        {
            _context = context;
        }
        public async Task<bool?> AddUserGroupAsync(UserGroup userGroup)
        {
            await _context.AddAsync(userGroup);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool?> IsGroupOwnerAsync(int userId, int groupId) =>
            await _context.UsersGroups
            .AnyAsync(ug => ug.UserId == userId 
            && ug.GroupId == groupId
            && ug.Role == 0);
    }
}
