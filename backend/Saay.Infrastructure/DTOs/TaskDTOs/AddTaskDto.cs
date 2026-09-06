using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.TaskDTOs
{
    public class AddTaskDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
        public int UserId { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "CategoryId must be between 1 and 3.")]
        public int TaskCategoryId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Title must be at most 255 characters long.")]
        public string Title { get; set; }

        [Range(0, 3, ErrorMessage = "Repetition must be between 0 and 3.")]
        public byte Repetition { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "DueDate must be a future date.")]
        public DateOnly DueDate { get; set; }

        [FutureTime(ErrorMessage = "DueTime must be a future time.")]
        public TimeOnly? DueTime { get; set; }
    }
}
