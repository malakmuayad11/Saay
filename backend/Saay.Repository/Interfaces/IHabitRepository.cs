using Saay.Data.Entities;

namespace Saay.Repository.Interfaces
{
    public interface IHabitRepository
    {
        public Task<int?> AddHabitAsync(Habit habit, int userId);

        public Task<List<Habit>> GetUserHabitsAsync(int userId, int pageNumber, int pageSize);

        public Task<int> UserHabitsCountAsync(int userId);

        public Task<bool?> UpdateHabitAsync(int habitId, Habit newHabit);

        public Task<bool?> DeleteHabitAsync(int habitId);

        public Task<int> UserCompletedHabitsCount(int userId);

        public Task<int> UserPendingHabitsCount(int userId);

        public Task<Habit> GetHabitByIdAsync(int habitId);

        public Task<bool> DoesHabitExistAsync(int habitId);

        public Task<byte?>GetHabitTargetDurationAsync(int habitId);

        public Task<bool> IsHabitOwner(int userId, int habitId);

    }
}
