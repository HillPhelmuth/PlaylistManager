using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PlaylistManager.Models;
using PlaylistManager.Shared;

namespace PlaylistManager.Pages
{
    public class DeleteVideosModel : PlaylistComponentBase
    {
        [CascadingParameter]
        public PlaylistModel SelectedPlaylist { get; set; }
        protected List<VideoModel> LocalVideos = new List<VideoModel>();

        protected override async Task OnInitializedAsync()
        {
            LocalVideos = await Database.GetPlaylistVideos(SelectedPlaylist);
        }
        protected async Task DeleteSelected()
        {
            var selectedVideos = LocalVideos.Where(x => x.IsRemoved);
            foreach (var video in selectedVideos)
            {
                await Database.RemoveVideoFromPlaylist(video, SelectedPlaylist);
            }
            LocalVideos = await Database.GetPlaylistVideos(SelectedPlaylist);
            StateHasChanged();
        }
    }
}
