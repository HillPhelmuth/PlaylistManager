using MatBlazor;
using PlaylistManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaylistManager.Interfaces
{
    public interface IExcelImport
    {
        Task<List<VideoModel>> Import(IMatFileUploadEntry file);
    }
}
