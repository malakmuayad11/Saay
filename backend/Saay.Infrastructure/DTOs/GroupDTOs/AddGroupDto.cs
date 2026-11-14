using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.GroupDTOs
{
    public class AddGroupDto
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "UserID must be a positive integer.")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "GroupName cannot exceed 100 characters.")]
        public string GroupName { get; set; }

        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string? Description { get; set; }
    }
}
