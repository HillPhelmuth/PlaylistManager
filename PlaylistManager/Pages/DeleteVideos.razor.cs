using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlaylistManager.Models;
using PlaylistManager.Shared;

namespace PlaylistManager.Pages
{
    public class DeleteVideosModel : PlaylistComponentBase
    {
        public PlaylistModel Playlist { get; set; }
        protected List<VideoModel> LocalVideos = new List<VideoModel>();

        protected override async Task OnInitializedAsync()
        {
            LocalVideos = await Database.GetPlaylistVideos(Playlist);
        }
        protected async Task DeleteSelected()
        {
            var selectedVideos = LocalVideos.Where(x => x.IsRemoved);
            foreach (var video in selectedVideos)
            {
                await Database.RemoveVideoFromPlaylist(video, Playlist);
            }
            LocalVideos = await Database.GetPlaylistVideos(Playlist);
            StateHasChanged();
        }
    }
}
