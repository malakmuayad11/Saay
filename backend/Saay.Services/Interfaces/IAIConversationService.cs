namespace Saay.Services.Interfaces
{
    public interface IAIConversationService
    {
        public Task<string> SendMessage(int userId, string message);
    }
}
