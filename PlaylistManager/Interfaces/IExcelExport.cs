using PlaylistManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaylistManager.Interfaces
{
    public interface IExcelExport
    {
        Task<byte[]> ExportV2(PlaylistModel playlist);
    }
}
