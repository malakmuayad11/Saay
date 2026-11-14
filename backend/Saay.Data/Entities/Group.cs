namespace Saay.Data.Entities;

public class Group
{
    public int GroupId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly CreatedAt { get; set; }

    public ICollection<UserGroup> UsersGroups { get; set; }
        = new List<UserGroup>();

    public ICollection<Challenge> Challenges { get; set; }
        = new List<Challenge>();
}