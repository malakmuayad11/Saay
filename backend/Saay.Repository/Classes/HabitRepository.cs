using Microsoft.EntityFrameworkCore;
using Saay.Data;
using Saay.Data.Entities;
using Saay.Repository.Interfaces;

namespace Saay.Repository.Classes
{
    public class HabitRepository : IHabitRepository
    {
        private readonly SaayContext _context;

        public HabitRepository(SaayContext context)
        {
            _context = context;
        }

        public async Task<int?> AddHabitAsync(Habit habit, int userId)
        {
            Habit newHabit = new Habit
            {
                UserId = userId,
                Title = habit.Title,
                ReasonForHabit = habit.ReasonForHabit,
                Steps = habit.Steps,
                TargetDuration = habit.TargetDuration
            };

            _context.Habits.Add(newHabit);
            if (await _context.SaveChangesAsync() > 0)
                return newHabit.HabitId;

            return null;
        }

        public async Task<List<Habit>> GetUserHabitsAsync(int userId, int pageNumber, int pageSize) =>
            await _context.Habits
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

        public async Task<int> UserHabitsCountAsync(int userId) =>
            await _context.Habits
                .Where(habit => habit.UserId == userId)
                .CountAsync();

        public async Task<bool?> UpdateHabitAsync(int habitId, Habit newHabit)
        {
            Habit habit = await _context.Habits.FindAsync(habitId);

            if (habit == null) return null; // Habit not found

            habit.Title = newHabit.Title;
            habit.ReasonForHabit = newHabit.ReasonForHabit;
            habit.Steps = newHabit.Steps;
            habit.TargetDuration = newHabit.TargetDuration;

            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool?> DeleteHabitAsync(int habitId)
        {
            Habit habit = await _context.Habits.FindAsync(habitId);
            if (habit == null) return null;
            _context.Habits.Remove(habit);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> UserCompletedHabitsCount(int userId)
        {
            return await _context.Habits
            .CountAsync(h =>
                h.UserId == userId &&
                _context.HabitsLogs.Count(hl => hl.HabitId == h.HabitId) ==
                (h.TargetDuration == 0 ? 30 :
                 h.TargetDuration == 1 ? 60 :
                 h.TargetDuration == 2 ? 90 : 0)
            );
        }

        public async Task<int> UserPendingHabitsCount(int userId) =>
            await _context.Habits.CountAsync(h =>
                h.UserId == userId &&
                _context.HabitsLogs.Count(hl => hl.HabitId == h.HabitId)
                <
                (h.TargetDuration == 0 ? 30 :
                 h.TargetDuration == 1 ? 60 :
                 h.TargetDuration == 2 ? 90 : 0)
            );

        public async Task<Habit> GetHabitByIdAsync(int habitId) =>
            await _context.Habits.FindAsync(habitId);

        public Task<bool> DoesHabitExistAsync(int habitId) =>
            _context.Habits.AnyAsync(h => h.HabitId == habitId);

        public Task<byte?> GetHabitTargetDurationAsync(int habitId) =>
            _context.Habits
                .Where(h => h.HabitId == habitId)
                .Select(h => (byte?)h.TargetDuration)
                .FirstOrDefaultAsync();

        public async Task<bool> IsHabitOwner(int userId, int habitId) =>
            await _context.Habits.AnyAsync(h => h.UserId == userId && h.HabitId == habitId);
    }
}
