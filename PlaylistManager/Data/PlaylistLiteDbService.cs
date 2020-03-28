using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistManager.Interfaces;
using PlaylistManager.Models;
using PlaylistManager.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace PlaylistManager.Data
{
    public class PlaylistLiteDbService
    {
        private readonly LiteDatabase _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostEnvironment;

        private string UserId => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public bool HasUser => !string.IsNullOrEmpty(UserId);

        public PlaylistLiteDbService(IPlaylistLiteDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHost)
        {
            _httpContextAccessor = httpContextAccessor;
            _database = context.Database;
            _hostEnvironment = webHost;
        }
        
        [HttpPost]
        public async Task AddPlaylist(string playlistName)
        {           
            var dbPlaylists = _database.GetCollection<LiteDbPlaylistModel>("Playlists");
            var matched = dbPlaylists.Find(x => x.User_ID == UserId && x.Name == playlistName);
            if (matched.Any())
                return;
            var newPlaylist = new LiteDbPlaylistModel()
            {
                Name = playlistName,
                User_ID = UserId,
                Videos = new List<VideoModel>()
            };
            await Task.Run(() =>dbPlaylists.Insert(newPlaylist));
        }
        [HttpGet]
        public async Task<List<PlaylistModel>> GetUserPlaylists()
        {
            var dbPlaylists = _database.GetCollection<LiteDbPlaylistModel>("Playlists");
            var allPlaylists = dbPlaylists.Find(x => x.User_ID == UserId);
            var playlists = new List<PlaylistModel>();
            foreach (var list in allPlaylists)
            {
                playlists.Add(list.ConvertToPlaylist());
            }
            return await Task.FromResult(playlists);

        }
        [HttpGet]
        public async Task<PlaylistModel> GetPlaylistWithKey(PlaylistModel playlist)
        {
            var dbPlaylists = _database.GetCollection<LiteDbPlaylistModel>("Playlists");
            var matched = dbPlaylists.Find(x => x.User_ID == playlist.User_ID && x.Name == playlist.Name).FirstOrDefault();
            return await Task.FromResult(matched.ConvertToPlaylist());

        }
        [HttpPost]
        public async Task AddVideoToPlaylist(VideoModel video, PlaylistModel playlist)
        {            
            var dbPlaylists = _database.GetCollection<LiteDbPlaylistModel>("Playlists");
            var dbVideos = _database.GetCollection<VideoModel>("Videos");
            var matched = dbPlaylists.Include(x => x.Videos).Find(x => x.Name == playlist.Name && x.User_ID == playlist.User_ID).FirstOrDefault();
            matched.Videos.Add(video);
            await Task.Run(() => dbVideos.Upsert(video));
            await Task.Run(() => dbPlaylists.Upsert(matched));
                   
           
        }
        [HttpPut]
        public async Task UpdatePlaylistVideos(VideoModel video)
        {            
            var dbVideos = _database.GetCollection<VideoModel>("Videos");            
            var matchVideo = dbVideos.FindById(video.ID);                   
            matchVideo.PreferenceID = video.PreferenceID;
            await Task.Run(() => dbVideos.Update(matchVideo));            
        }
        [HttpGet]
        public async Task<List<VideoModel>> GetPlaylistVideos(PlaylistModel playlist)
        {
            var dbPlaylists = _database.GetCollection<LiteDbPlaylistModel>("Playlists");
            var matched = dbPlaylists.Include(x => x.Videos).Find(x => x.Name == playlist.Name && x.User_ID == playlist.User_ID).FirstOrDefault();
            var videos = matched.Videos;
            return await Task.FromResult(videos);
        }
        [HttpDelete]
        public async Task RemoveVideoFromPlaylist(VideoModel video, PlaylistModel playlist)
        {
            var dbPlaylists = _database.GetCollection<LiteDbPlaylistModel>("Playlists");
            var dbVideos = _database.GetCollection<VideoModel>("Videos");
            var matched = dbPlaylists.Include(x => x.Videos).Find(x => x.Name == playlist.Name && x.User_ID == playlist.User_ID).FirstOrDefault();
            matched.Videos.Remove(video);
            dbPlaylists.Update(matched);
           await Task.Run(() => dbVideos.Delete(video.ID));
        }
    }
}
