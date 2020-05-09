using PlaylistManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaylistManager.Interfaces
{
    public interface IPlaylistService
    {
        Task<List<VideoModel>> GetYouTubeVideos(string input, int max = 10);
    }
}
