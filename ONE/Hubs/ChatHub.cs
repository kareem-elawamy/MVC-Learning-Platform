using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.SignalR;
using ONE.Services;

namespace ONE.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IGeminiService _aiService; // استخدمنا السيرفس الجديدة

        public ChatHub(IGeminiService aiService)
        {
            _aiService = aiService;
        }

        public async Task SendMessage(string userMessage)
        {
            // ابعت رسالة انتظار وهمية (اختياري)
            // await Clients.Caller.SendAsync("ReceiveMessage", "bot", "جاري الكتابة...");

            try
            {
                // كلم السيرفس الحقيقية
                var botReply = await _aiService.GetChatResponse(userMessage);

                // ابعت الرد لليوزر
                await Clients.Caller.SendAsync("ReceiveMessage", "bot", botReply);
            }
            catch
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "bot", "حدث خطأ غير متوقع.");
            }
        }
    }
}