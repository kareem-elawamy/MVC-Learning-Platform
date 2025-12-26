using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ONE.Controllers
{
    [Route("[controller]")]
    public class TrackController : Controller
    {
        private readonly ILogger<TrackController> _logger;
        public readonly Data.AddDbContext _context;

        public TrackController(ILogger<TrackController> logger, Data.AddDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index(int id)
        {
            var track = _context.Tracks
                .Include(t => t.Materials)
                    .ThenInclude(m => m.Videos)
                .FirstOrDefault(t => t.Id == id);

            if (track == null)
                return NotFound();

            return View(track);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}