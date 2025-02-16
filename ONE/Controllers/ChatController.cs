using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ONE.Data;
using ONE.Models;
using System.Text;
using System.Text.Json;

namespace ONE.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly AddDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "AIzaSyC0rGmQeBDBx0QSzjm6rL3Utbx7pEdfQOI";

        public ChatController(HttpClient httpClient, AddDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
        {
            var userid = User.Identity.Name;
            if (string.IsNullOrEmpty(userid))
                return Unauthorized(new { Reply = "يجب تسجيل الدخول أولا" });

            var Url = $"https://generativelanguage.googleapis.com/v1/models/gemini-pro:generateContent?key={_apiKey}";
            var request = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = message.UserMessage }
                        }
                    }
                }
            };

            
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Url, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return BadRequest(new { Reply = "حدث خطأ أثناء الاتصال بـ Gemini API", ErrorDetails = errorResponse });
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(responseContent);
            var reply = result.GetProperty("candidates")[0]
                              .GetProperty("content")
                              .GetProperty("parts")[0]
                              .GetProperty("text")
                              .GetString();

            
            var chatMessage = new ChatMessage
            {
                UserId = userid,
                UserMessage = message.UserMessage,
                BotResponse = reply
            };

            _context.ChatMessage.Add(chatMessage);
            await _context.SaveChangesAsync();

            return Json(new { Reply = reply ?? "لم يتم الرد" });
        }

        [HttpGet]
        public async Task<IActionResult> GetChatHistory()
        {
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Reply = "يجب تسجيل الدخول أولا" });
            var chatHistory = await _context.ChatMessage.Where(m => m.UserId == userId)
                .OrderByDescending(m => m.Timestamp)
                .ToListAsync();
            return Json(chatHistory);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
