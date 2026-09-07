using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.GoalDTOs
{
    public class AddGoalDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
        public int UserId { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "CategoryId must be an integer between 1 and 12.")]
        public byte GoalCategoryId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Title must be a string with a maximum length of 255 characters.")]
        public string Title { get; set; } = null!;

        [Required]
        [Range(0, 3, ErrorMessage = "TimePeriod must be an integer between 0 and 3.")]
        public byte TimeFrame { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "Deadline must be a future date.")]
        public DateOnly Deadline { get; set; }
    }
}
