﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PlaylistManager.Interfaces;
using PlaylistManager.Models;
using PlaylistManager.Shared;

namespace PlaylistManager.Pages
{
    public class SearchResultsModel : PlaylistComponentBase
    {
        [Inject]
        protected IPlaylistService PlaylistService { get; set; }

        [Parameter]
        public List<VideoModel> VideoUrl { get; set; }
        [CascadingParameter]
        public PlaylistModel Playlist { get; set; }
        public string SearchYouTube { get; set; }
        public List<VideoModel> Videos { get; set; }
        public List<VideoModel> VideosAdded = new List<VideoModel>();
        protected bool VideosReady { get; set; }
        protected bool IsPlayerReady { get; set; }
        protected bool HasAdded { get; set; }
        protected bool MaxReady { get; set; }
        public async Task GetVideoResults()
        {
            Videos = await PlaylistService.GetYouTubeVideos(SearchYouTube);
            await DisplayResults();
            VideosReady = true;
            MaxReady = false;
        }
        public async Task GetMaxResults()
        {
            Videos = await PlaylistService.GetYouTubeVideos(SearchYouTube, 20);
            await DisplayResults();
            MaxReady = true;
            VideosReady = false;
        }
        private async Task DisplayResults()
        {
            var playlistVideos = await Database.GetPlaylistVideos(Playlist);
            var matchedVideos = Videos.Intersect(playlistVideos).ToList();
            VideosAdded.AddRange(matchedVideos);
            HasAdded = matchedVideos.Any();
        }
        protected async void SearchVideoKeyup(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
                await GetVideoResults();
        }
        public Task PlayVideo(VideoModel video)
        {
            VideoUrl = new List<VideoModel> { video };
            IsPlayerReady = true;
            return Task.CompletedTask;
        }
        public async Task AddVideoToPlaylist(VideoModel video)
        {
            VideosAdded.Add(video);
            HasAdded = true;
            await Database.AddVideoToPlaylist(video, Playlist);
            StateHasChanged();
        }
        public async Task RemoveFromPlaylist(VideoModel video)
        {
            VideosAdded.Remove(video);
            HasAdded = VideosAdded.Any();
            await Database.RemoveVideoFromPlaylist(video, Playlist);
        }
        public async Task AddVideosToDatabase()
        {
            foreach (var video in Videos)
            {
                VideosAdded.Clear();
                HasAdded = false;
                await Database.AddVideoToPlaylist(video, Playlist);
            }
        }
        protected async Task AddSelectedVideos()
        {
            var videos = Videos.Where(x => x.IsSelected).ToList();
            await Database.AddVideosToPlaylist(videos, Playlist);
            MaxReady = false;
        }
        protected Task SelectAll()
        {
            foreach (var video in Videos)
            {
                video.IsSelected = true;
            }
            return Task.CompletedTask;
        }
        protected void OnPlayerReadyChanged(bool isClosePlayer)
        {
            IsPlayerReady = isClosePlayer;
            StateHasChanged();
        }
    }
}
