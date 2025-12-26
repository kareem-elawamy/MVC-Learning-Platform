using Google.Apis.YouTube.v3;
using Google;
using Microsoft.AspNetCore.Mvc;
using ONE.Data;
using ONE.Services;
using ONE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ONE.Controllers
{
    // [Authorize(Roles = "Admin")]


    public class VideoController : Controller
    {
        private readonly AddDbContext _context;
        private readonly YouTubeServiceApi _serviceApi;
        public VideoController(AddDbContext context, YouTubeServiceApi serviceApi)
        {
            _context = context;
            _serviceApi = serviceApi;
        }

        public async Task<IActionResult> Index(int? trackId, int? materialId)
        {
            ViewBag.Tracks = new SelectList(await _context.Tracks.ToListAsync(), "Id", "Name");

            var material = _context.Materials.AsQueryable();
            if (trackId.HasValue)
            {
                material = material.Where(m => m.TrackId == trackId);
            }
            ViewBag.Materials = new SelectList(await material.ToListAsync(), "Id", "Name");

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


        public IActionResult Create()
        {
            ViewData["MaterialID"] = new SelectList(_context.Materials, "Id", "Name");
            return View(new Video());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Video video)
        {

            if (!ModelState.IsValid)
            {

                ViewData["MaterialID"] = new SelectList(_context.Materials, "Id", "Name");
                return View(video);
            }

            var videoDY = await _serviceApi.GetVideoDetails(video.VideoId);
            if (videoDY == null)
            {

                ViewData["MaterialID"] = new SelectList(_context.Materials, "Id", "Name");
                return View(video);
            }


            var videoDb = new Video
            {
                VideoId = video.VideoId,
                Title = videoDY.Title,
                ThumbnailUrl = videoDY.ThumbnailUrl,
                Description = videoDY.Description,
                MaterialID = video.MaterialID
            };

            _context.Videos.Add(videoDb);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}