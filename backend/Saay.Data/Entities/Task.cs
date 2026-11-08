namespace Saay.Data.Entities;

public partial class Task
{
    public int TaskId { get; set; }

    public int UserId { get; set; }

    public byte TaskCategoryId { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly DueDate { get; set; }

    public TimeOnly? DueTime { get; set; }

    public bool IsDone { get; set; }

    public bool IsReminderSent { get; set; }

    public byte? PrayerNumber { get; set; }

    public int? GoalId { get; set; }

    public virtual TaskCategory TaskCategory { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Goal? Goal { get; set; }
}
