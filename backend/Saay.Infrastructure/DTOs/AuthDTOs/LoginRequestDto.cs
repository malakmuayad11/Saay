using System.ComponentModel.DataAnnotations;

using Saay.Infrastructure.Validation;

namespace Saay.Infrastructure.DTOs.AuthDTOs
{
    public class LoginRequestDto
    {
        [Required]
        [RegularExpression(ValidationRules.EMAIL_REGX, ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
