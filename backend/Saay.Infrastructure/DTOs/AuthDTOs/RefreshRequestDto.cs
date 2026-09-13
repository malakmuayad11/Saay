using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.AuthDTOs
{
    public class RefreshRequestDto
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage="User id must be a positive integer.")]
        public int UserID { get; set; }
    }
}
