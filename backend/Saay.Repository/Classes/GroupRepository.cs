using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Data.Entities;
using Saay.Infrastructure.DTOs.GroupDTOs;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class GroupRepository : IGroupRepository
    {

        private readonly SaayContext _context;

        public GroupRepository(SaayContext context)
        {
            _context = context;
        }
        public async Task<int?> AddGroupAsync(Group group)
        {
            await _context.Groups.AddAsync(group);

            if (await _context.SaveChangesAsync() > 0)
                return group.GroupId;
            return null;    
        }

        public async Task<bool?> DeleteGroup(int groupId)
        {
            Group group = await _context.Groups.FindAsync(groupId);

            if (group == null) return null;

            _context.Groups.Remove(group);

            if (await _context.SaveChangesAsync() > 0)
                return true;

            return false;
        }

        public async Task<string?> GetGroupName(int groupId) =>
            await _context.Groups
            .Where(g => g.GroupId == groupId)
            .Select(g => g.Name).SingleOrDefaultAsync();

        public async Task<List<GroupDto>> GetUserGroupsAsync(int userId) =>
            await _context.UsersGroups
                .Where(ug => ug.UserId == userId)
                .Select(ug => new GroupDto
                {
                    GroupId = ug.Group.GroupId,
                    GroupDescription = ug.Group.Description,
                    IsRequestedUserOwner = ug.Role == 0,
                    GroupName = ug.Group.Name,

                    MembersCount = (byte)ug.Group.UsersGroups.Count,

                    Members = ug.Group.UsersGroups
                        .Select(member => new MemberDto
                        {
                            UserId = member.User.UserId,
                            FullName = member.User.FirstName + " " + member.User.LastName,
                            ProfilePictureUrl = member.User.ProfilePictureUrl,
                        })
                        .ToList()
                })
                .ToListAsync();
    }
}
