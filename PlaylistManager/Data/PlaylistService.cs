using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlaylistManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaylistManager.Data
{
    public class PlaylistService
    {
        private IConfiguration _configuration;
        public PlaylistService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public List<SearchResult> SearchYouTube(string input, int max = 10)
        {
            List<SearchResult> searchResults = new List<SearchResult>();
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _configuration["YouTubeApiKey"],
                ApplicationName = GetType().ToString()
            });

            SearchResource.ListRequest listRequest = youtubeService.Search.List("snippet");
            listRequest.Q = input;
            listRequest.MaxResults = max;
            listRequest.Type = "video";
            SearchListResponse response = listRequest.Execute();

            return response.Items.ToList();

        }
        public async Task<List<VideoModel>> GetYouTubeVideos(string input, int max = 10)
        {
            var searchResults = SearchYouTube(input, max);
            var videosList = new List<VideoModel>();
            foreach (var result in searchResults)
            {
                var videoData = new VideoModel()
                {
                    ThumbnailUrl = result.Snippet.Thumbnails.Default__.Url,
                    Title = result.Snippet.Title,
                    Description = result.Snippet.Description,
                    VideoID = result.Id.VideoId
                };
                videosList.Add(videoData);
            }
            return await Task.FromResult(videosList);
        }


    }
}
