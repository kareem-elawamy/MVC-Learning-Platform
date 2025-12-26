using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ONE.DTOs.Gemini;

namespace ONE.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        public GeminiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            // هنجيب المفتاح من appsettings.json
            _apiKey = config["Gemini:ApiKey"] ?? throw new ArgumentNullException(nameof(config), "Gemini:ApiKey is missing from configuration");
            // بنستخدم موديل gemini-1.5-flash عشان سريع ورخيص
_apiUrl = $"https://generativelanguage.googleapis.com/v1/models/gemini-pro:generateContent?key={_apiKey}";        }
        public async Task<string> GetChatResponse(string message)
        {
            var requestBody = new GeminiRequestDto
            {
                contents = new List<ContentDto>
        {
            new ContentDto
            {
                parts = new List<PartDto> { new PartDto { text = message } }
            }
        }
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            try
            {
                var response = await _httpClient.PostAsync(_apiUrl, jsonContent);

                // ========================================================
                // التعديل هنا: لو فيه خطأ، نقرأ تفاصيله من جوجل
                // ========================================================
                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    return $"⚠️ خطأ من جوجل ({response.StatusCode}): {errorDetails}";
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var geminiData = JsonSerializer.Deserialize<GeminiResponseDto>(jsonResponse);

                var botReply = geminiData?.candidates?.FirstOrDefault()?.content?.parts?.FirstOrDefault()?.text;

                return botReply ?? "الرد فارغ.";
            }
            catch (Exception ex)
            {
                return $"خطأ في السيرفر: {ex.Message}";
            }
        }
    }
}