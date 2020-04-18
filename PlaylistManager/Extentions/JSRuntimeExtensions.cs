using Google.Apis.YouTube.v3.Data;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaylistManager.Extentions
{
    public static class JSRuntimeExtensions
    {
        public static ValueTask SaveAs(this IJSRuntime js, string filename, byte[] data)
            => js.InvokeVoidAsync(
                "playlistManager.interop.saveAsFile",
                filename,
                Convert.ToBase64String(data));
        public static ValueTask<object> StartYouTube(this IJSRuntime JSRuntime)
        {
            return JSRuntime.InvokeAsync<object>("startYouTube");
        }
        public static ValueTask<object> AddYouTubePlayer(this IJSRuntime JSRuntime)
        {
            return JSRuntime.InvokeAsync<object>("addPlayer");
        }
        public static ValueTask<object> StopYouTubePlayer(this IJSRuntime JSRuntime)
        {
            return JSRuntime.InvokeAsync<object>("removeYouTube");
        }
        public static ValueTask Log(this IJSRuntime js, string message) => js.InvokeVoidAsync("logitem", message);
        
        
    }
}
