using Microsoft.EntityFrameworkCore;
using PlaylistManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public DbSet<TempUser> TempUsers { get; set; }
    }
}
