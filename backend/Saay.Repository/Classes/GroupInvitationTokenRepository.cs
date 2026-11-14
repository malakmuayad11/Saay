using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Data.Entities;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class GroupInvitationTokenRepository : IGroupInvitationTokenRepository
    {
        private readonly SaayContext _context;

        public GroupInvitationTokenRepository(SaayContext context)
        {
            _context = context;
        }   
        public async Task<int?> AddGroupInvitationAsync(GroupInvitationToken groupInvitationToken)
        {
            await _context.GroupInvitationTokens.AddAsync(groupInvitationToken);
            if( await _context.SaveChangesAsync() > 0)
            {
                return groupInvitationToken.GroupInvitationTokenId;
            }
            return null;
        }

        public async Task<GroupInvitationToken?> GetGroupInvitationTokenAsync(string token) =>
            await _context.GroupInvitationTokens.SingleOrDefaultAsync(g => g.TokenHash == token);
    }
}
