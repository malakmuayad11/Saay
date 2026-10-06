using Saay.Infrastructure.DTOs.AIConversationDTOs;
using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;

namespace Saay.Services.Classes
{
    public class AIConversationService : IAIConversationService
    {
        private readonly IGeminiService _geminiService;
        private readonly IAIConversationRepository _aiConversationRepository;
        private readonly IUserRepository _userRepository;

        public AIConversationService(IGeminiService geminiService, IAIConversationRepository aiConversationRepository, IUserRepository userRepository)
        {
            _geminiService = geminiService;
            _aiConversationRepository = aiConversationRepository;
            _userRepository = userRepository;
        }
        public async Task<string> SendMessage(int userId, string message)
        {
            string? previousInteractionId =
                await _aiConversationRepository.GetInteractionId(userId);

            if(await _aiConversationRepository.IsFirstTime(userId))
            {
                string? userName = await _userRepository.GetNameAsync(userId);
                message = $"""
                    You are Saay's AI assistant.

                    You are assisting the user named {userName}.

                    Your role is to help the user with their tasks, goals, habits, planning, productivity, and questions within the Saay application. Be helpful, concise, friendly, and practical. Use the user's name naturally when appropriate.

                    User's name: {userName}

                    User's message:
                    {message}
                    """;
            }

            GeminiResponseDto response =
                await _geminiService.SendInteractionAsync(
                    message,
                    previousInteractionId);

            if (response == null)
                throw new Exception("Gemini returned a null response.");

            if (string.IsNullOrWhiteSpace(response.OutputText))
                throw new Exception("Gemini returned no output text.");
            
            else
            {
                await _aiConversationRepository.UpdateInteractionId(
                    response.Id,
                    userId);
            }

            return response.OutputText;
        }
    }
}
