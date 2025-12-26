using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ONE.Services
{
    public interface IGeminiService
    {
        Task<string> GetChatResponse(string prompt);
        
    }
}