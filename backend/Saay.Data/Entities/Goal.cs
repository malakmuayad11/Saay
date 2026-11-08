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
    public byte TimeFrame { get; set; }

    public DateOnly Deadline { get; set; }

    public bool IsDone { get; set; }

    public virtual GoalCategory GoalCategory { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Saay.Data.Entities.Task> Tasks { get; set; } = new List<Saay.Data.Entities.Task>();
}
