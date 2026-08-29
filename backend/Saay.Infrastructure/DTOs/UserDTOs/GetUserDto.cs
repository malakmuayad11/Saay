namespace Saay.Infrastructure.DTOs.UserDTOs
{
    public class GetUserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }
        public string? Mission { get; set; }
    }
}
