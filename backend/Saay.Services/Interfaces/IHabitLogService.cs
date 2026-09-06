namespace Saay.Services.Interfaces
{
    public interface IHabitLogService
    {
        public Task<(bool? isMarked, string message)> MarkHabitAsCompletedTodayAsync(int habitId);
    }
}
