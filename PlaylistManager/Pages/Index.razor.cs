using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PlaylistManager.Interfaces;
using PlaylistManager.Models;
using PlaylistManager.Shared;

namespace PlaylistManager.Pages
{
    public class IndexModel : PlaylistComponentBase
    {
        [Inject]
        protected IExcelImport ExcelService { get; set; }
        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Parameter]
        public List<PlaylistModel> UserPlaylists { get; set; }
        [Parameter]
        public PlaylistModel SelectedPlaylist { get; set; }
        protected string PlaylistName { get; set; }
        public string NewPlaylist { get; set; }
        protected bool PageReady { get; set; }
        protected bool TogglePlaylist { get; set; }
        protected bool ShowPlaylists { get; set; }
        protected string ErrorMessage { get; set; }

        protected string UserId
        {
            get
            {
                var authState = AuthenticationStateProvider.GetAuthenticationStateAsync();
                return authState.Result.User.Identity.Name;
            }
        }
        protected override async Task OnInitializedAsync()
        {
            PageReady = false;

            UserPlaylists = await Database.GetUserPlaylists();
            if (Database.HasUser)
            {
                PageReady = true;
                ShowPlaylists = true;
            }
        }
        protected async Task AddNewPlaylist(string listName)
        {
            PlaylistModel playlist = new PlaylistModel() { Name = listName, User_ID = UserId };
            UserPlaylists.Add(playlist);
            await Database.AddPlaylist(listName);
            SelectedPlaylist = await Database.GetPlaylistWithKey(playlist);
            ShowPlaylists = false;
        }
        protected void SelectPlaylist()
        {
            SelectedPlaylist = UserPlaylists.Find(x => x.Name == PlaylistName);
            ShowPlaylists = false;
        }
        protected async Task ChangePlaylist()
        {
            SelectedPlaylist = null;
            NewPlaylist = null;
            UserPlaylists = await Database.GetUserPlaylists();
            ShowPlaylists = true;
        }
        protected void ToggleNewPlaylist()
        {
            TogglePlaylist = !TogglePlaylist;
        }
        protected async Task ReadExcelFile(IMatFileUploadEntry[] files)
        {
            var file = files.FirstOrDefault();
            if (file == null)
            {
                ErrorMessage = "No File Found";
                return;
            }

            var filename = file.Name;
            if (!filename.Contains(".xls"))
            {
                ErrorMessage = "Invalid file format. Please use .xlsx or .xls file type.";
                StateHasChanged();
                return;
            }
            var playlistName = filename.Substring(0, filename.IndexOf("-", StringComparison.Ordinal));
            await AddNewPlaylist(playlistName);
            var videos = await ExcelService.Import(file);
            if (SelectedPlaylist == null)
            {
                return;
            }
            foreach (var video in videos)
            {
                await Database.AddVideoToPlaylist(video, SelectedPlaylist);
            }
        }
    }
}
