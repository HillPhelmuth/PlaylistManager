using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlaylistManager.Data
{
    public class PlaylistDatabaseService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PlaylistManagerDbContext _context;

        public PlaylistDatabaseService(IHttpContextAccessor httpContextAccessor, PlaylistManagerDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        private string UserId => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public bool HasUser => !string.IsNullOrEmpty(UserId);
        [HttpPost]
        public async Task AddPlaylist(string playlistName)
        {
            var userId = UserId;
            var playlist = new PlaylistModel() { Name = playlistName, User_ID = userId };
            var context = _context;
            var userPlaylist = context.PlaylistsTable.ToList().Where(x => x.Name == playlistName).Select(y => y.Name).FirstOrDefault();
            if (userPlaylist == playlistName)
            {
                await context.DisposeAsync();
            }
            else
            {
                await context.AddAsync(playlist);
                await context.SaveChangesAsync();
            }

        }
        [HttpGet]
        public async Task<List<PlaylistModel>> GetUserPlaylists()
        {
            var userId = UserId;
            var context = _context;
            return await context.PlaylistsTable.Where(x => x.User_ID == userId).ToListAsync();
        }
        [HttpPost]
        public async Task AddVideoToPlaylist(VideoModel video, PlaylistModel playlist)
        {
            var context = _context;
            // Match name and user_id to find stored PlaylistsTable primary key
            video.Playlist_ID = context.PlaylistsTable
                .Where(x => x.User_ID == UserId && x.Name == playlist.Name)
                .Select(x => x.ID).FirstOrDefault();
            await context.AddAsync(video);                     
            await context.SaveChangesAsync();
        }
        [HttpPut]
        public async Task UpdatePlaylistVideos(VideoModel video, PlaylistModel playlist)
        {
            var context = _context;

            var videoToUpdate = await context.VideosTable.Where(x => x.ID == video.ID).FirstOrDefaultAsync();
            videoToUpdate.PreferenceID = video.PreferenceID;
            await context.SaveChangesAsync();
        }
        [HttpGet]
        public async Task<List<VideoModel>> GetPlaylistVideos(PlaylistModel playlist)
        {
            var context = _context;
            var playlistId = playlist.ID;
            return await context.VideosTable.Where(x => x.Playlist_ID == playlistId).ToListAsync();
        }
        [HttpDelete]
        public async Task RemoveVideoFromPlaylist(VideoModel video)
        {
            var context = _context;
            context.Attach(video);
            context.Remove(video);
            await context.SaveChangesAsync();
        }
    }
}
