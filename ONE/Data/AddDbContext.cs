using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ONE.Models;
using ONE.DTOs;

namespace ONE.Data
{
    public class AddDbContext : IdentityDbContext<ApplicationUser>
    {
        public AddDbContext(DbContextOptions<AddDbContext> options)
           : base(options)
        {
        }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<ChatMessage> ChatMessage { get; set; }

        public DbSet<FavoriteVideo> Favorites { get; set; }


    }
}
