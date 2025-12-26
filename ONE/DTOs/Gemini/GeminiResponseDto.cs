using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ONE.DTOs.Gemini
{
    public class GeminiResponseDto
    {
        public List<CandidateDto> candidates { get; set; }
    }
}