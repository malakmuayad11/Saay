using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Data.Entities;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class HabitLogRepository : IHabitLogRepository
    {
        private readonly SaayContext _context;

        public HabitLogRepository(SaayContext context)
        {
            _context = context;
        }

        public async Task<bool?> MarkHabitAsCompletedTodayAsync(int habitId)
        {
            byte today = (byte)DateTime.Today.DayOfWeek;

            HabitLog habitLog = new HabitLog
            {
                HabitId = habitId,
                DayNumber = today,
                IsDone = true
            };

            _context.HabitsLogs.Add(habitLog);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsHabitCompletedToday(int habitId)
        {
            byte today = (byte)DateTime.Today.DayOfWeek;

            return await _context.HabitsLogs
                .AnyAsync(hl => hl.HabitId == habitId
                && hl.DayNumber == today
                && hl.IsDone);
        }

        public async Task<byte> GetHabitLogsCountAsync(int habitId)
        {
            int result = await _context.HabitsLogs
                .Where(hl => hl.HabitId == habitId)
                .CountAsync();

            return (byte)result;
        }
    }
}
