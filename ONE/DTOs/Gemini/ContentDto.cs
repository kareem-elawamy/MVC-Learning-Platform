using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ONE.DTOs.Gemini
{
    public class ContentDto
    {
       public string role { get; set; } = "user";
        public List<PartDto> parts { get; set; }
        
    }
}