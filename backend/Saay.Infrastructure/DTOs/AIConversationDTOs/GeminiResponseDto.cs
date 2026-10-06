namespace Saay.Infrastructure.DTOs.AIConversationDTOs
{
    public class GeminiResponseDto
    {
        public string Id { get; set; } = string.Empty;

        public List<GeminiStepDto> Steps { get; set; } = new();

        public string? OutputText =>
            Steps
                .Where(step => step.Type == "model_output")
                .SelectMany(step => step.Content ?? new List<GeminiContentDto>())
                .Where(content => content.Type == "text")
                .Select(content => content.Text)
                .FirstOrDefault();
    }

    public class GeminiStepDto
    {
        public string Type { get; set; } = string.Empty;

        public string? Signature { get; set; }

        public List<GeminiContentDto>? Content { get; set; }
    }

    public class GeminiContentDto
    {
        public string Type { get; set; } = string.Empty;

        public string? Text { get; set; }
    }
}