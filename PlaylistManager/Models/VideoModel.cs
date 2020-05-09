using System;
using LiteDB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PlaylistManager.Models
{
    [Serializable]
    [Table("Videos")]
    public class VideoModel
    {

        public string Title { get; set; }

        public string VideoID { get; set; }

        public int ID { get; set; }

        public int PreferenceID { get; set; }

        public int Playlist_ID { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Description { get; set; }
        [NotMapped]
        public bool IsRemoved { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
