namespace Saay.Infrastructure.DTOs.GroupDTOs
{
    public class MemberDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
    }
    public class GroupDto
    {
        public int GroupId { get; set; }
        public bool IsRequestedUserOwner { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string GroupDescription { get; set; } = string.Empty;
        public byte MembersCount { get; set; }
        public List<MemberDto> Members { get; set; } = new List<MemberDto>();
    }
}
