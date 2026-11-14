namespace Saay.Services.Interfacess
{
    public interface IGroupInvitationTokenService
    {
        public Task<int?> AddGroupInvitationTokenAsync(int groupId, int userId, string token);

        public Task<(int groupId, int userId)?> GetGroupInvitationTokenAsync(string token);
    }
}
