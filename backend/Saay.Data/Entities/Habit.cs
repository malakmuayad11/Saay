namespace Saay.Data.Entities;

public partial class Habit
{
    public int HabitId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string ReasonForHabit { get; set; } = null!;

    public string Steps { get; set; } = null!;

    /// <summary>
    /// 0- 30 days, 1- 60 days, 2- 90 days
    /// </summary>
    public byte TargetDuration { get; set; }

    public virtual User User { get; set; } = null!;
}
