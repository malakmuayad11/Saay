namespace Saay.Infrastructure.DTOs.GoalDTOs
{
    public class GoalDto
    {
        public int GoalId { get; set; }
        public string CategoryTitle { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string TimeFrame { get; set; }
        public DateOnly Deadline { get; set; }
        public bool IsDone { get; set; }
    }
}
