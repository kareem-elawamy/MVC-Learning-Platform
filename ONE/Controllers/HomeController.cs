using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ONE.Data;
using ONE.Models;
using System.Diagnostics;

namespace ONE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AddDbContext _context;

        public HomeController(ILogger<HomeController> logger , AddDbContext context)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<IActionResult> Index(int? trackId, int? materialId)
        {
            ViewBag.Tracks = new SelectList(await _context.Tracks.ToListAsync(), "Id", "Name");
            var materialsQuery = _context.Materials.AsQueryable();
            if (trackId.HasValue)
            {
                materialsQuery = materialsQuery.Where(m => m.TrackId == trackId);
            }
            ViewBag.Materials = new SelectList(await materialsQuery.ToListAsync(), "Id", "Name");
            var videos = _context.Videos.Include(v => v.Material).ThenInclude(m => m.Track).AsQueryable();
            if (materialId.HasValue)
            {
                videos = videos.Where(v => v.MaterialID == materialId);
            }
            else if (trackId.HasValue)
            {
                videos = videos.Where(v => v.Material.TrackId == trackId);
            }

            return View(await videos.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetMaterialsByTrack(int trackId)
        {
            var materials = await _context.Materials
                .Where(m => m.TrackId == trackId)
                .Select(m => new { m.Id, m.Name })
                .ToListAsync();

            return Json(materials);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
