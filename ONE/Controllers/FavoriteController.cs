using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONE.Services;
using System.Security.Claims;

namespace ONE.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly FavoriteService _favoriteService;

        public FavoriteController(FavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var favorites = await _favoriteService.GetUserFavorites(userId);
            return View(favorites);
        }
        [HttpPost]
        public async Task<IActionResult> Add(int videoId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            await _favoriteService.AddToFavorite(userId, videoId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Remove(int videoId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            await _favoriteService.RemoveFromFavorite(userId, videoId);
            return RedirectToAction("Index");
        }
    }
}
