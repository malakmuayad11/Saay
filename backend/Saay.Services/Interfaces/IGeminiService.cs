using Saay.Infrastructure.DTOs.AIConversationDTOs;

namespace Saay.Services.Interfaces
{
    public interface IGeminiService
    {
        public Task<GeminiResponseDto?> SendInteractionAsync(string input, string? previousInteractionId);
    }
}
