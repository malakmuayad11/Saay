namespace Saay.Data.Entities
{
    public class AIConversation
    {
        public int ConversationId { get; set; }
        public int UserId { get; set; }
        public string? PreviousInteractionId { get; set; }
        public User User { get; set; } = null!;
    }
}
