using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ONE.DTOs
{
    public class CreateVideoDto
    {
        public string YouTubeUrl { get; set; } = string.Empty;
        public int? MaterialId { get; set; }
        
    }
}