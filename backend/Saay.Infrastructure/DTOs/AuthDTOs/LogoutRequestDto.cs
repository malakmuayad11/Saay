using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.AuthDTOs
{
    public class LogoutRequestDto
    {
        [Required]
        public string Email {  get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
