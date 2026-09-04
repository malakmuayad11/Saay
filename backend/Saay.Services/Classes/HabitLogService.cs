using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;

namespace Saay.Services.Classes
{
    public class HabitLogService : IHabitLogService
    {
        private readonly IHabitLogRepository _habitLogRepository;
        private readonly IHabitService _habitService;

        public HabitLogService(IHabitLogRepository habitLogRepository, IHabitService habitService)
        {
            _habitLogRepository = habitLogRepository;
            _habitService = habitService;
        }

        public async Task<(bool? isMarked, string message)> MarkHabitAsCompletedTodayAsync(int habitId)
        {
            if(!await _habitService.DoesHabitExistAsync(habitId))
                return (null, "Habit does not exist");

            byte? targetDuration = await _habitService.GetHabitTargetDurationAsync(habitId);

            byte targetDurationValue = 
                targetDuration == 0 ? (byte)30 :
                targetDuration == 1 ? (byte)60 :
                targetDuration == 2 ? (byte)90 :
                (byte)0;

            byte logsCount = await _habitLogRepository.GetHabitLogsCountAsync(habitId);

            if (targetDurationValue <= logsCount)
                return (false, "Target duration exceeded");

            if (await _habitLogRepository.IsHabitCompletedToday(habitId) == true)
                return (false, "Habit already marked as completed today");

            return (await _habitLogRepository.MarkHabitAsCompletedTodayAsync(habitId), "Habit marked as completed today");
        }
    }
}
