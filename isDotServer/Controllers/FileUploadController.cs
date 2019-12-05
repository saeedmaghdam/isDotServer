using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isDotServer.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IHostingEnvironment _environment;
        public FileUploadController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(/*IFormCollection files*/)
        {
            var files = Request.Form.Files;
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

                var filePath = Path.Combine(uploadsRootFolder, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream).ConfigureAwait(false);
                }
            }
            return Content("Uploaded successfully!");
        }

        [System.Web.Http.HttpPost]
        public async Task<IActionResult> UploadSingleFile(IFormFile file)
        {
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            var filePath = Path.Combine(uploadsRootFolder, file.FileName);
            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream).ConfigureAwait(false);
                }
            }

            return Ok(new
            {
                count = 1,
                path = filePath
            });
        }
    }
}