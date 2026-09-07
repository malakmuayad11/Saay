namespace Saay.Data.Entities
{
    public class GoalCategory
    {
        public byte GoalCategoryId { get; set; }

        public string Title { get; set; } = null!;

        public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
    }
}
