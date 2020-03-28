using LiteDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using PlaylistManager.Interfaces;
using PlaylistManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaylistManager.Data
{
    public class PlaylistLiteDbContext : IPlaylistLiteDbContext
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public LiteDatabase Database { get; }
        
        public PlaylistLiteDbContext(IOptions<LiteDbOptions> options, IWebHostEnvironment webHost)
        {
            var mapper = BsonMapper.Global;
            mapper.Entity<LiteDbPlaylistModel>().DbRef(x => x.Videos, "videos").Field(x => x.Videos, "video");
            mapper.Entity<VideoModel>();
            _hostEnvironment = webHost;
            var databaseLocation = $@"{_hostEnvironment.WebRootPath}/PlaylistVideosLite.db";
            Database = new LiteDatabase(databaseLocation, mapper);
        }
    }
    public class LiteDbOptions
    {
        public string DatabaseLocation { get; set; }
    }
}
