using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Data.Entities;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly SaayContext _context;
        public UserRepository(SaayContext context)
        {
            _context = context;
        }

        public async Task<int?> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            if (await _context.SaveChangesAsync() > 0) return user.UserId;
            return null;
        }

        public async Task<bool> DoesEmailExist(string email) =>
            await _context.Users.AnyAsync(user => user.Email == email);

        public async Task<bool> IsEmailUsedByAnotherUser(int userId, string email) =>
            await _context.Users.AnyAsync(user => user.Email == email && user.UserId != userId);
        
        public async Task<bool?> DeleteUserAsync(int userId)
        {
            User user = await _context.Users.FindAsync(userId);
            if (user == null) return null;
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool?> UpdateMissionAsync(int userId, string newMission)
        {
            User user = await _context.Users.FindAsync(userId);

            if (user == null) return null;

            user.Mission = newMission; 

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool?> UpdateUserAsync(int userId, User newUser)
        {
            User user = await _context.Users.FindAsync(userId);

            if (user == null) return null;

            // Update user properties
            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.Email = newUser.Email;
            user.ProfilePictureUrl = newUser.ProfilePictureUrl;

            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool> DoesUserExist(int userId) =>
            await _context.Users.AnyAsync(user => user.UserId == userId);

        public async Task<bool?> UpdatePasswordAsync(int userId, string newPasswordHash)
        {
            User user = await _context.Users.FindAsync(userId);

            if (user == null) return null; // User not found

            user.PasswordHash = newPasswordHash;

            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<User?> GetUserByIdAsync(int userId) =>
            await _context.Users.FindAsync(userId);

        public async Task<User?> GetUserByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }
}
