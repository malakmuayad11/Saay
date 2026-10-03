using Saay.Data.Entities;

namespace Saay.Repository.Interfaces
{
    public interface IHabitLogRepository
    {
        public Task<bool?> MarkHabitAsCompletedTodayAsync(int habitId, byte dayNumber);

        public Task<bool> IsHabitCompleted(int habitId, byte dayNumber);

        public Task<byte> GetHabitLogsCountAsync(int habitId);

        public Task<List<HabitLog>> GetHabitLogs(int habitId);
    }
}
