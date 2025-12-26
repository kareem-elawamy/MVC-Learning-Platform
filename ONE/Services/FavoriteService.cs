using Microsoft.EntityFrameworkCore;
using ONE.Data;
using ONE.Models;

namespace ONE.Services
{
    public class FavoriteService
    {
        private readonly AddDbContext _context;

        public FavoriteService(AddDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToFavorite(string userId, int videoId)
        {
            if (_context.Favorites.Any(f => f.UserId == userId && f.VideoId == videoId))
                return false;

            var favorite = new FavoriteVideo
            {
                UserId = userId,
                VideoId = videoId
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromFavorite(string userId, int videoId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.VideoId == videoId);
            if (favorite == null)
                return false;

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Video>> GetUserFavorites(string userId)
        {
            return await _context.Favorites.Include(v => v.Video).ThenInclude(v => v.Material)
                .Where(f => f.UserId == userId)
                .Select(f => f.Video)
                .ToListAsync();
        }
    }
}
