using Saay.Data.Entities;
using Saay.Infrastructure.DTOs.HabitLogDTOs;
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

        public async Task<(bool? isMarked, string message)> MarkHabitAsCompletedTodayAsync(int habitId, byte dayNumber)
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

            if (await _habitLogRepository.IsHabitCompleted(habitId, dayNumber) == true)
                return (false, "Habit already marked as completed today");

            return (await _habitLogRepository.MarkHabitAsCompletedTodayAsync(habitId, dayNumber), "Habit marked as completed today");
        }
    
        public async Task<List<HabitLogDto>> GetHabitLogs(int habitId)
        {
            if (!await _habitService.DoesHabitExistAsync(habitId))
                return null;

            List<HabitLog> habitLogs = await _habitLogRepository.GetHabitLogs(habitId);
            List<HabitLogDto> habitLogDtos = new List<HabitLogDto>();

            foreach(HabitLog habitLog in habitLogs)
            {
                habitLogDtos.Add(new HabitLogDto
                {
                    HabitId = habitLog.HabitId,
                    DayNumber = habitLog.DayNumber,
                    IsDone = habitLog.IsDone
                });
            }
            return habitLogDtos;
        }
    }
}
