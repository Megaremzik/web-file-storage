using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WS.Business;
using WS.Business.Services;

namespace WS.Web.Controllers
{

    public class DownloadController : Controller
    {
        DocumentService _documentService;
        DownloadService _downloadService;
        PathProvider _pathProvider;
        public DownloadController(DownloadService downloadService, PathProvider pathProvider, DocumentService documentService)
        {
            _downloadService = downloadService;
            _pathProvider = pathProvider;
            _documentService = documentService;
        }

        public IActionResult Get(int documentId)
        {
            var doc = _documentService.Get(documentId);
            string path = _pathProvider.GetFullPath(documentId);
    
            if (doc.IsFile)
            {
                string name = Path.GetFileName(path);
                return PhysicalFile(path, GetContentType(name), name);
            }
            string zipName = _downloadService.CreateZip(path);
            return PhysicalFile(_pathProvider.GetRootPath() + "\\" + zipName, GetContentType(zipName), zipName);
        }

        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }


    }
}