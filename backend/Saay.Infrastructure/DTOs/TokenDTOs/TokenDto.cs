namespace Saay.Infrastructure.DTOs.TokenDTOs
{
    public class TokenDto
    {
        public DateTime? ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string Hash { get; set; }
    }
}
