using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlaylistManager.Data;

namespace PlaylistManager.Shared
{
    public class PlaylistComponentBase : ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public PlaylistDatabaseService Database { get; set; }
    }
}
