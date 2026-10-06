using Microsoft.Extensions.Configuration;
using Saay.Infrastructure.DTOs.AIConversationDTOs;
using Saay.Services.Interfaces;
using System.Text;
using System.Text.Json;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _URL;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _URL = "https://generativelanguage.googleapis.com/v1beta/interactions";
    }

    public async Task<GeminiResponseDto?> SendInteractionAsync(string input, string? previousInteractionId)
    {
        string? apiKey = _configuration["GEMINI_API_KEY"];

        Console.WriteLine(
    $"Gemini API key loaded: {!string.IsNullOrWhiteSpace(apiKey)}");

        var requestBody = new
        {
            model = "gemini-3.8-flash",
            input = input,
            previous_interaction_id = previousInteractionId
        };

        string? json = JsonSerializer.Serialize(requestBody);

        using HttpRequestMessage? request = new HttpRequestMessage(
            HttpMethod.Post,
            _URL);

        request.Headers.Add("x-goog-api-key", apiKey);

        request.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        //HttpResponseMessage response = await _httpClient.SendAsync(request);

        //response.EnsureSuccessStatusCode();

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        string responseJson = await response.Content.ReadAsStringAsync();

        Console.WriteLine("GEMINI RESPONSE:");
        Console.WriteLine(responseJson);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Gemini API error {(int)response.StatusCode} ({response.StatusCode}): {responseJson}");
        }

        return JsonSerializer.Deserialize<GeminiResponseDto>(
            responseJson,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })
            ?? throw new Exception("Failed to deserialize Gemini response.");
    }
}