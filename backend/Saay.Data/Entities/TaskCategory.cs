namespace Saay.Data.Entities;

public partial class TaskCategory
{
    public byte TaskCategoryId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
