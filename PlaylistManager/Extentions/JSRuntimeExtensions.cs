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
        public static ValueTask SaveAs(this IJSRuntime jsRuntime, string filename, byte[] data)
            => jsRuntime.InvokeVoidAsync(
                "playlistManager.interop.saveAsFile",
                filename,
                Convert.ToBase64String(data));
        public static ValueTask<object> StartYouTube(this IJSRuntime jsRuntime)
        {
            return jsRuntime.InvokeAsync<object>("startYouTube");
        }
        public static ValueTask<object> AddYouTubePlayer(this IJSRuntime jsRuntime)
        {
            return jsRuntime.InvokeAsync<object>("addPlayer");
        }
        public static ValueTask<object> StopYouTubePlayer(this IJSRuntime jsRuntime)
        {
            return jsRuntime.InvokeAsync<object>("removeYouTube");
        }
        public static ValueTask Log(this IJSRuntime jsRuntime, string message) => jsRuntime.InvokeVoidAsync("logitem", message);


    }
}
