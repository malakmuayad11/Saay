namespace Saay.Infrastructure.DTOs.AuthDTOs
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
