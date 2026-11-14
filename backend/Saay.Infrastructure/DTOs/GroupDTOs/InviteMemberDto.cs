using Saay.Infrastructure.Validation;
using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.GroupDTOs
{
    public class InviteMemberDto
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "GroupID must be a positive integer.")]
        public int GroupId { get; set; }

        [Required]
        [RegularExpression(ValidationRules.EMAIL_REGX, ErrorMessage = "Invalid email format.")]
        public string UserEmail { get; set; } = null!;
    }
}
