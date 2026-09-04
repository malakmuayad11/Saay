using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.HabitDTOs
{
    public class AddHabitDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(255, ErrorMessage = "ReasonForHabit cannot exceed 255 characters.")]
        public string ReasonForHabit { get; set; } = null!;
        
        [Required]
        [MaxLength(255, ErrorMessage = "Steps cannot exceed 255 characters.")]  
        public string Steps { get; set; } = null!;

        [Required]
        [Range(0, 2, ErrorMessage = "TargetDuration must be between 0 and 2.")]
        public byte TargetDuration { get; set; } 
    }
}
