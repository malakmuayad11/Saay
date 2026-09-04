namespace Saay.Services.Interfaces
{
    public interface IHabitLogService
    {
        public Task<bool?> MarkHabitAsCompletedTodayAsync(int habitId);
    }
}
