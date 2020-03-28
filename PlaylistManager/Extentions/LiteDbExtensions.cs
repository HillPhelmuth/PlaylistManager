using LiteDB;
using PlaylistManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaylistManager.Extentions
{
    public static class LiteDbExtensions
    {
        public static PlaylistModel ConvertToPlaylist(this LiteDbPlaylistModel liteDbModel)
        {
            return new PlaylistModel()
            {
                ID = liteDbModel.ID,
                Name = liteDbModel.Name,
                User_ID = liteDbModel.User_ID
            };
        }
        public static List<VideoModel> ConvertToVideos(this LiteDbPlaylistModel liteDbModel)
        {
            return liteDbModel.Videos;
        }
    }
}
