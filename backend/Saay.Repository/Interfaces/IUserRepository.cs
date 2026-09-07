using Saay.Data.Entities;

namespace Saay.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<int?> AddUserAsync(User user);
        public Task<bool> DoesEmailExist(string email);
        public Task<bool> IsEmailUsedByAnotherUser(int userId, string email);
        public Task<bool?> DeleteUserAsync(int userId);
        public Task<bool?> UpdateMissionAsync(int userId, string newMission);
        public Task<bool?> UpdateUserAsync(int userId, User newUser);
        public Task<bool> DoesUserExist(int userId);
        public Task<bool?> UpdatePasswordAsync(int userId, string newPasswordHash);
        public Task<User?> GetUserByIdAsync(int userId);
        public Task<User?> GetUserByEmailAsync(string email);
    }
}
