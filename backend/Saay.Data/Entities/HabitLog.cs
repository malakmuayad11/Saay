namespace Saay.Data.Entities
{
    public class HabitLog
    {
        public int HabitLogId { get; set; }

        public int HabitId { get; set; }

        public byte DayNumber { get; set; }

        public bool IsDone { get; set; }

        public Habit Habit { get; set; } = null!;
    }
}
