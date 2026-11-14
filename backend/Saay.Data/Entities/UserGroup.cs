namespace Saay.Data.Entities;

public class UserGroup
{
    public int GroupId { get; set; }

    public int UserId { get; set; }

    public byte Role { get; set; }

    public Group Group { get; set; } = null!;

    public User User { get; set; } = null!;
}