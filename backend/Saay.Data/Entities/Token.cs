namespace Saay.Data.Entities;

public partial class Token
{
    public int TokenId { get; set; }

    public int UserId { get; set; }

    public string RefreshTokenHash { get; set; } = null!;

    public DateTime? ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
