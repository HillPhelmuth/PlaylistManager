using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlaylistManager.Shared;
using PlaylistManager.Extentions;
using PlaylistManager.Models;

namespace PlaylistManager.Pages
{
    public class VideoPlayerModel : PlaylistComponentBase, IDisposable
    {
        [CascadingParameter]
        public List<VideoModel> VideoUrls { get; set; }
        [Parameter]
        public EventCallback<bool> PlayerReadyChanged { get; set; }
        protected List<VideoModel> PrivateVideos { get; set; }
        protected string VideoId { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            PrivateVideos = VideoUrls;
            var refThis = DotNetObjectReference.Create(this);
            var firstVideo = PrivateVideos.OrderBy(x => x.PreferenceID).FirstOrDefault();
            VideoId = firstVideo?.VideoID;
            PrivateVideos.Remove(firstVideo);
            await JSRuntime.StartYouTube();
            await Task.Delay(1000);
            await JSRuntime.InvokeAsync<object>("getYouTube", refThis, VideoId);
        }
        protected async Task OkClick()
        {
            await JSRuntime.StopYouTubePlayer();
            await Task.Delay(1000);
            await PlayerReadyChanged.InvokeAsync(false);
        }
        [JSInvokable]
        // ReSharper disable once UnusedMember.Global -JSInvokable used by javascript code
        public async Task GetNextVideo()
        {
            var refThis = DotNetObjectReference.Create(this);
            var firstVideo = PrivateVideos.OrderBy(x => x.PreferenceID).FirstOrDefault();
            VideoId = firstVideo?.VideoID;
            PrivateVideos.Remove(firstVideo);
            await JSRuntime.StopYouTubePlayer();
            await JSRuntime.StartYouTube();
            await JSRuntime.AddYouTubePlayer();
            await Task.Delay(1000);
            await JSRuntime.InvokeAsync<object>("getYouTube", refThis, VideoId);
        }
        public void Dispose() => JSRuntime.StopYouTubePlayer();
    }
}
