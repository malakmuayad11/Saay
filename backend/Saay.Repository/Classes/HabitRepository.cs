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

            if (habit == null) return null;

            // Update habit properties
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

        public async Task<int> UserCompletedHabitsCount(int userId) =>
             await _context.HabitsLogs
                .Where(log => log.Habit.UserId == userId && log.IsDone)
                .CountAsync();

        public async Task<int> UserPendingHabitsCount(int userId) =>
             await _context.HabitsLogs
                .Where(log => log.Habit.UserId == userId && !log.IsDone)
                .CountAsync();

        public async Task<Habit> GetHabitByIdAsync(int habitId) =>
            await _context.Habits.FindAsync(habitId);
     
    }
}
