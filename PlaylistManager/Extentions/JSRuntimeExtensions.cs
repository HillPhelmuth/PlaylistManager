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
    }
}
