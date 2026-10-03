using Saay.Infrastructure.DTOs.HabitLogDTOs;

namespace Saay.Services.Interfaces
{
    public interface IHabitLogService
    {
        public Task<(bool? isMarked, string message)> MarkHabitAsCompletedTodayAsync(int habitId, byte dayNumber);

        public Task<List<HabitLogDto>> GetHabitLogs(int habitId);
    }
}
