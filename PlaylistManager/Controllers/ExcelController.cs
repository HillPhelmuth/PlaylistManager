using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Microsoft.AspNetCore.Hosting;
using PlaylistManager.Data;
using PlaylistManager.Models;
using Microsoft.AspNetCore.Http.Extensions;
using System.Diagnostics;
using MatBlazor;
using PlaylistManager.Interfaces;

namespace PlaylistManager.Controllers
{
    [Route("api/excel")]
    [ApiController]
    public class ExcelController : ControllerBase, IExcelExport, IExcelImport
    {        
        private readonly PlaylistDatabaseService _databaseService;

        public ExcelController(PlaylistDatabaseService databaseService)
        {            
            _databaseService = databaseService;
        }
        [HttpGet("exportV2")]
        public async Task<byte[]> ExportV2(PlaylistModel playlist)
        {
            List<VideoModel> videos = await _databaseService.GetPlaylistVideos(playlist);
            var stream = new MemoryStream();
            using var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells.LoadFromCollection(videos, true);
            return await Task.FromResult(package.GetAsByteArray());
        }
        [HttpPost("import")]
        public async Task<List<VideoModel>> Import(IMatFileUploadEntry file)
        {

            var filename = file.Name;
            Console.WriteLine($"Filename is {filename}");
            Console.WriteLine($"File type is {file.Type}");
            if (!file.Name.Contains(".xls"))
                return null;
            var videos = new List<VideoModel>();
            using (var stream = new MemoryStream())
            {
                var sw = Stopwatch.StartNew();
                await file.WriteToStreamAsync(stream);
                sw.Stop();
                CreatePlaylistSheet(videos, stream);
            }
            return videos;
        }

        private static void CreatePlaylistSheet(List<VideoModel> videos, MemoryStream stream)
        {
            using var package = new ExcelPackage(stream);
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;
            AddVideosToExcel(videos, worksheet, rowCount);
        }

        private static void AddVideosToExcel(List<VideoModel> videos, ExcelWorksheet worksheet, int rowCount)
        {
            for (int row = 2; row <= rowCount; row++)
            {
                var video = new VideoModel()
                {
                    Title = worksheet.Cells[row, 1].Value.ToString().Trim(),
                    VideoID = worksheet.Cells[row, 2].Value.ToString().Trim(),
                    ThumbnailUrl = worksheet.Cells[row, 6].Value.ToString().Trim(),
                    Description = worksheet.Cells[row, 7].Value.ToString().Trim()
                };
                videos.Add(video);
            }
        }
    }
}