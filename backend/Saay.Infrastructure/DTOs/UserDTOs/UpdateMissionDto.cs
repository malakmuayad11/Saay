using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.UserDTOs
{
    public class UpdateMissionDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
        public int UserId { get; set; }

        [StringLength(255, ErrorMessage = "NewMission must be a string with a maximum length of 255 characters.")]
        public string? NewMission { get; set; }
    }
}
