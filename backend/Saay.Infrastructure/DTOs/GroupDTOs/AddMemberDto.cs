using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.DTOs.GroupDTOs
{
    public class AddMemberDto
    {
        [Required]
        public string Token { get; set; } = null!;
    }
}
