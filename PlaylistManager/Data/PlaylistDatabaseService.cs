using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace PlaylistManager.Data
{
    public class PlaylistDatabaseService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        private readonly PlaylistManagerDbContext _context;
      

        public PlaylistDatabaseService(AuthenticationStateProvider authenticationStateProvider, PlaylistManagerDbContext context)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _context = context;
            
        }
       
        private string UserId
        {
            get
            {
                var authState = _authenticationStateProvider.GetAuthenticationStateAsync();
                return authState.Result.User.Identity.Name;
            }
        }
        public bool HasUser => !string.IsNullOrEmpty(UserId);
        [HttpPost]
        public async Task AddPlaylist(string playlistName)
        {
            var userId = UserId;
            var playlist = new PlaylistModel() { Name = playlistName, User_ID = userId };
            var context = _context;
            var userPlaylist = context.PlaylistsTable.ToList().Where(x => x.Name == playlistName && x.User_ID == userId).FirstOrDefault();
            if (userPlaylist != playlist)
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
        [HttpGet]
        public async Task<PlaylistModel> GetPlaylistWithKey(PlaylistModel playlist)
        {
            var userId = UserId;
            var context = _context;
            var playlists = await context.PlaylistsTable.Where(x => x.User_ID == userId).ToListAsync();
            return playlists.Where(x => x.Name == playlist.Name).FirstOrDefault();
        }
        [HttpPost]
        public async Task AddVideoToPlaylist(VideoModel video, PlaylistModel playlist)
        {
            if (video.ID > 0)
                return;
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
            if (playlist.ID != video.Playlist_ID)
            {
                videoToUpdate.PreferenceID = video.PreferenceID;
                await context.SaveChangesAsync();
            }
        }
        [HttpGet]
        public async Task<List<VideoModel>> GetPlaylistVideos(PlaylistModel playlist)
        {
            var context = _context;
            var playlistId = playlist.ID;
            return await context.VideosTable.Where(x => x.Playlist_ID == playlistId).ToListAsync();
        }
        [HttpDelete]
        public async Task RemoveVideoFromPlaylist(VideoModel video, PlaylistModel playlist = null)
        {
            var context = _context;
            context.Attach(video);
            context.Remove(video);
            await context.SaveChangesAsync();
        }
    }
}
