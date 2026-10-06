using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.AIConversationDTOs
{
    public class MessageDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
        public int UserId { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;
    }
}
