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
            if (!await _context.Habits.AnyAsync(h => h.HabitId == habitId))
                return null;

            HabitLog habitLog = new HabitLog
            {
                HabitId = habitId,
                DayNumber = (byte)DateTime.UtcNow.DayOfWeek,
                IsDone = true
            };

            _context.HabitsLogs.Add(habitLog);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool?> IsHabitCompletedToday(int habitId)
        {
            if (!await _context.Habits.AnyAsync(h => h.HabitId == habitId))
                return null;

            return await _context.HabitsLogs
                .AnyAsync(hl => hl.HabitId == habitId 
                && hl.DayNumber == (byte)DateTime.UtcNow.DayOfWeek 
                && hl.IsDone);
        }
    }
}
