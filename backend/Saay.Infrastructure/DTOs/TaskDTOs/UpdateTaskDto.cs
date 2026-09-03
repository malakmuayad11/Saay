using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.TaskDTOs
{
    public class UpdateTaskDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TaskId must be a positive integer.")]
        public int TaskId { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "CategoryId must be a positive integer.")]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [Required]
        [Range(0, 3, ErrorMessage = "Repetition must be between 0 and 3.")]
        public byte Repetition { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "DueDate must be a future date.")]
        public DateOnly DueDate { get; set; }

        [FutureTime(ErrorMessage = "DueTime must be a future time.")]
        public TimeOnly? DueTime { get; set; }

        [Required]
        public bool IsDone { get; set; }

        [Required]
        public bool IsReminderSent { get; set; }
    }
}
