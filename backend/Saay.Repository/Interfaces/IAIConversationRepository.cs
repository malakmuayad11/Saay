namespace Saay.Repository.Interfaces
{
    public interface IAIConversationRepository
    {
        public Task<bool?> AddInteractionId(string interactionId, int userId);

        public Task<bool?> UpdateInteractionId(string newInteractionId, int userId);

        public Task<string?> GetInteractionId(int userId);

        public Task<bool> IsFirstTime(int userId);
    }
}
