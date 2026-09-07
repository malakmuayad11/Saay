namespace Saay.Data.Entities;

public partial class Goal
{
    public int GoalId { get; set; }

    public int UserId { get; set; }

    public byte GoalCategoryId { get; set; }

    public string Title { get; set; } = null!;

    /// <summary>
    /// 0- Monthly, 1- Quarterly, 2- Biannual, 3- Annually
    /// </summary>
    public byte TimePeriod { get; set; }

    public DateOnly Deadline { get; set; }

    public bool IsDone { get; set; }

    public virtual GoalCategory GoalCategory { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
