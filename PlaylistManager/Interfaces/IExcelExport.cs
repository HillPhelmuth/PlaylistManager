using PlaylistManager.Models;
using System.Threading.Tasks;

namespace PlaylistManager.Interfaces
{
    public interface IExcelExport
    {
        Task<byte[]> ExportV2(PlaylistModel playlist);
    }
}
