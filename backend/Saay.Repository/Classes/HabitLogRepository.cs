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

        public async Task<bool?> MarkHabitAsCompletedTodayAsync(int habitId, byte dayNumber)
        {
            HabitLog habitLog;

            // because a habit log is already created for the first day of a habit, we need to update it instead of creating a new one
            if (dayNumber == 1)
            {
                habitLog = await _context.HabitsLogs
                    .FirstOrDefaultAsync(hl => hl.HabitId == habitId && hl.DayNumber == dayNumber);

                habitLog.IsDone = true;

                return await _context.SaveChangesAsync() >= 0;
            }


            habitLog = new HabitLog
            {
                HabitId = habitId,
                DayNumber = dayNumber,
                IsDone = true
            };

            _context.HabitsLogs.Add(habitLog);
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool> IsHabitCompleted(int habitId, byte dayNumber) =>
            await _context.HabitsLogs
                .AnyAsync(hl => hl.HabitId == habitId
                && hl.DayNumber == dayNumber
                && hl.IsDone);

        public async Task<byte> GetHabitLogsCountAsync(int habitId)
        {
            int result = await _context.HabitsLogs
                .Where(hl => hl.HabitId == habitId)
                .CountAsync();

            return (byte)result;
        }

        public async Task<List<HabitLog>> GetHabitLogs(int habitId) =>
            await _context.HabitsLogs
            .Where(habitLog => habitLog.HabitId == habitId)
            .ToListAsync();
    }
}
