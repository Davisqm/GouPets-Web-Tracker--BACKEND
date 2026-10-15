using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        Dictionary<string, string> MimeTypes { get; }
        public FilesController(IConfiguration configuration)
        {
            Configuration = configuration;
            MimeTypes = new Dictionary<string, string>  
            {  
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        // POST: api/files
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            var pathToSave = Configuration.GetValue<string>("FilesFolder");
            var fileObject = Request.Form.Files[0];
            var fileName = DateTime.Now.ToString("yyMMddHHmmssfff_") + fileObject.FileName;

            DirectoryInfo dir = new DirectoryInfo(pathToSave);
            if (!dir.Exists) dir.Create();

            if (fileObject.Length > 0)
            {
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = fileName;
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    fileObject.CopyTo(stream);
                }
                return Ok(new { dbPath });
            }
            else
            {
                return BadRequest();
            }
        }

        // POST: api/files/folder/file
        [HttpGet("{base64DbPath}")]
        public async Task<IActionResult> DownloadFile(string base64DbPath)
        {

            var base64EncodedBytes = System.Convert.FromBase64String(base64DbPath);
            string dbPath = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            var filesFolder = Configuration.GetValue<string>("FilesFolder");

            var fullPath = Path.Combine(filesFolder, dbPath);
            FileInfo fileRef = new FileInfo(fullPath);
            if (!fileRef.Exists) {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;  
            return File(memory, GetContentType(dbPath), fileRef.Name);
        }


        // POST: api/files
        [HttpGet("zip/{folder}")]
        public async Task<IActionResult> DownloadFolder(string folder)
        {
            var filesFolder = Configuration.GetValue<string>("FilesFolder");
            return await DownloadZipFromDir(filesFolder, folder);
        }

        [HttpPost("zip")]
        public async Task<IActionResult> DownloadZip(string[] base64DbPaths)
        {
            var filesFolder = Configuration.GetValue<string>("FilesFolder");
            DirectoryInfo tempFolder = new DirectoryInfo(Path.Combine(filesFolder, Guid.NewGuid().ToString()));
            if (!tempFolder.Exists)
                tempFolder.Create();
            foreach(var base64DbPath in base64DbPaths)
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64DbPath);
                string dbPath = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                var fullPath = Path.Combine(filesFolder, dbPath);
                FileInfo fileRef = new FileInfo(fullPath);
                if (fileRef.Exists)
                    fileRef.CopyTo(Path.Combine(tempFolder.FullName, fileRef.Name));
            }
            var result = await DownloadZipFromDir(filesFolder, tempFolder.Name);
            // tempFolder.Delete();
            return result;
        }

        private async Task<IActionResult> DownloadZipFromDir(string filesFolder, string folder)
        {
            var fullFolderPath = Path.Combine(filesFolder, folder);
            DirectoryInfo direcotyRef = new DirectoryInfo(fullFolderPath);
            if (!direcotyRef.Exists)
                return NotFound();

            FileInfo tempZipFile = new FileInfo(Path.Combine(filesFolder, Guid.NewGuid().ToString() + ".zip"));

            ZipFile.CreateFromDirectory(fullFolderPath, tempZipFile.FullName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(tempZipFile.FullName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            tempZipFile.Delete();

            memory.Position = 0;  
            return File(memory, GetContentType(tempZipFile.FullName), "folder.zip");
        }

        private string GetContentType(string path)  
        {    
            var ext = Path.GetExtension(path).ToLowerInvariant();  
            if (MimeTypes.ContainsKey(ext)) {
                return MimeTypes[ext];
            }
            return "image/jpeg";
        }
    }
}
