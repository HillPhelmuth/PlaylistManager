using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlaylistManager.Models
{
    [Serializable]
    [Table("Playlists")]
    public class PlaylistModel
    {        
        [JsonIgnore]
        public int ID { get; set; }
        
        public string Name { get; set; }
        
        public string User_ID { get; set; }
    }
}
