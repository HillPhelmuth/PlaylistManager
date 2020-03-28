using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaylistManager.Interfaces
{
    public interface IPlaylistLiteDbContext
    {
        LiteDatabase Database { get; }        
    }
}
