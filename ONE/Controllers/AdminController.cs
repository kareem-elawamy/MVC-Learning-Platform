using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONE.Data;
using ONE.DTOs;
using ONE.Models;
using System.Diagnostics;

namespace ONE.Controllers
{
    // [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly AddDbContext _context;
        public AdminController(AddDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var model = new TrackMaterialDTO
            {
                Tracks = await _context.Tracks.ToListAsync(),
                Materials = await _context.Materials.Include(T=>T.Track).ToListAsync()
            };

            return View(model);
        }
        [HttpPost]
        public JsonResult AddTrack([FromBody] Track track)
        {
            if (track != null && !string.IsNullOrWhiteSpace(track.Name))
            {
                _context.Tracks.Add(track);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public JsonResult GetTracks()
        {
            var tracks = _context.Tracks.Select(t => new { t.Id, t.Name }).ToList();
            return Json(tracks);
        }

        [HttpPost]
        public JsonResult AddMaterial([FromBody] Material material)
        {
            if (material != null && !string.IsNullOrWhiteSpace(material.Name) && material.TrackId > 0)
            {
                _context.Materials.Add(material);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult DeleteTrack(int id)
        {
            var track = _context.Tracks.Find(id);
            if (track != null)
            {
                _context.Tracks.Remove(track);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpDelete]
        public JsonResult DeleteMaterial(int id)
        {
            var material = _context.Materials.Find(id);
            if (material     != null)
            {
                _context.Materials.Remove(material);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
