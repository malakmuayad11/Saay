namespace Saay.Infrastructure.DTOs.HabitDTOs
{
    public class HabitDto
    {
        public int HabitId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public string ReasonForHabit { get; set; } = null!;
        public string Steps { get; set; } = null!;
        public byte TargetDuration { get; set; }
    }
}
