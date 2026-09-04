using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;

namespace Saay.Services.Classes
{
    public class HabitLogService : IHabitLogService
    {
        private readonly IHabitLogRepository _habitLogRepository;

        public HabitLogService(IHabitLogRepository habitLogRepository)
        {
            _habitLogRepository = habitLogRepository;
        }

        public async Task<bool?> MarkHabitAsCompletedTodayAsync(int habitId)
        {
            if(await _habitLogRepository.IsHabitCompletedToday(habitId) == true)
                return false; // Habit already marked as completed today
            else if(await _habitLogRepository.IsHabitCompletedToday(habitId) == null)
                return null; // Habit does not exist

            return await _habitLogRepository.MarkHabitAsCompletedTodayAsync(habitId);
        }
    }
}
