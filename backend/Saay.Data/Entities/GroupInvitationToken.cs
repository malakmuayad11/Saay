namespace Saay.Data.Entities;

public class GroupInvitationToken
{
    public int GroupInvitationTokenId { get; set; }

    public int UserId { get; set; }

    public int GroupId { get; set; }

    public string TokenHash { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime? UsedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;
}