namespace Saay.Infrastructure.DTOs.GroupDTOs
{
    public class MemberDto
    {
        public string FullName { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public bool IsOwner { get; set; }
    }
    public class GroupDto
    {
        public string GroupName { get; set; } = string.Empty;
        public byte MembersCount { get; set; }
        public List<MemberDto> Members { get; set; } = new List<MemberDto>();
    }
}
