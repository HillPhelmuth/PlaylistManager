using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlaylistManager.Interfaces;
using PlaylistManager.Models;
using PlaylistManager.Extentions;
using PlaylistManager.Shared;

namespace PlaylistManager.Pages
{
    public class PlaylistsModel : PlaylistComponentBase
    {
        
        [Inject]
        public IExcelExport ExcelService { get; set; }

        [CascadingParameter]
        public PlaylistModel PlaylistName { get; set; }
        public List<VideoModel> Videos { get; set; }
        [Parameter]
        public List<VideoModel> VideoUrls { get; set; }
        protected List<VideoModel> OrderedVideos = new List<VideoModel>();
        [Parameter]
        public bool VideosReady { get; set; } = false;
        protected bool PageReady { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Videos = await Database.GetPlaylistVideos(PlaylistName);
            PageReady = true;
        }
        public async Task PlayVideosInOrder()
        {
            OrderedVideos = Videos.OrderBy(x => x.PreferenceID).ToList();
            await JSRuntime.InvokeAsync<object>("startYouTube");
            VideoUrls = OrderedVideos;
            foreach (var video in VideoUrls)
            {
                await Database.UpdatePlaylistVideos(video, PlaylistName);
            }
            VideosReady = true;
        }
        protected async Task ExportVideosExcel()
        {
            var excelBytes = await ExcelService.ExportV2(PlaylistName);
            await JSRuntime.SaveAs($"{PlaylistName.Name}-{PlaylistName.ID}.xlsx", excelBytes);
        }
    }
}
