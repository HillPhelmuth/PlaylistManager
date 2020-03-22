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


namespace PlaylistManager.Controllers
{
    [Route("api/excel")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly PlaylistDatabaseService _databaseService;        

        public ExcelController(IWebHostEnvironment hostEnvironment, PlaylistDatabaseService databaseService)
        {
            _hostEnvironment = hostEnvironment;
            _databaseService = databaseService;            
        }

        [HttpGet]
        public async Task<DemoResponse<string>> Export(PlaylistModel playlist, string baseUri)
        {
            string downloadUrl = baseUri;
            string folder = _hostEnvironment.WebRootPath;
            string excelName = $"{playlist.Name}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            //string downloadUrl = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, excelName);
            FileInfo file = new FileInfo(Path.Combine(folder, excelName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(folder, excelName));
            }
            List<VideoModel> videos = await _databaseService.GetPlaylistVideos(playlist);
            using (var package = new ExcelPackage(file))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(videos, true);
                package.Save();
            }

            return DemoResponse<string>.GetResult(0, "OK", downloadUrl);
        }
        [HttpGet("exportV2")]
        public async Task<byte[]> ExportV2(PlaylistModel playlist)
        {
            List<VideoModel> videos = await _databaseService.GetPlaylistVideos(playlist);
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(videos, true);
                return await Task.FromResult(package.GetAsByteArray());
            }
        }
    }
}