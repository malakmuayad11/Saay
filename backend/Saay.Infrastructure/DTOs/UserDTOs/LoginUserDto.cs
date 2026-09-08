using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.UserDTOs
{
    public class LoginUserDto
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [RegularExpression(Validation.ValidationRules.EMAIL_REGX, ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(Validation.ValidationRules.PASSWORD_REGX, ErrorMessage ="Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number")]
        public string PasswordHash { get; set; }
    }
}
