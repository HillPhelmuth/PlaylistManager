using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaylistManager.Models
{
    [Serializable]
    public class LiteDbPlaylistModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string User_ID { get; set; }
        public List<VideoModel> Videos { get; set; }
    }
}
