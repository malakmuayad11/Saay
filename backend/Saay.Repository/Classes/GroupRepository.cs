using Saay.Data;
using Saay.Data.Entities;
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
    }
}
