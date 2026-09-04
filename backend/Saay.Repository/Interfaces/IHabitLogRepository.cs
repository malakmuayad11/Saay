namespace Saay.Repository.Interfaces
{
    public interface IHabitLogRepository
    {
        public Task<bool?> MarkHabitAsCompletedTodayAsync(int habitId);

        public Task<bool?> IsHabitCompletedToday(int habitId);
    }
}
