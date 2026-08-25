namespace Saay.Data.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
