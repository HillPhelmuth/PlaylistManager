using Microsoft.EntityFrameworkCore;
using PlaylistManager.Models;

namespace PlaylistManager.Data
{
    public class PlaylistManagerDbContext : DbContext
    {
        public PlaylistManagerDbContext(DbContextOptions<PlaylistManagerDbContext> options)
            : base(options)
        {

        }
        public DbSet<PlaylistModel> PlaylistsTable { get; set; }
        public DbSet<VideoModel> VideosTable { get; set; }
    }
}
