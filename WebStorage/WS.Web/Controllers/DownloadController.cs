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
            string path = _documentService.GetFullPath(documentId);
    
            if (doc.IsFile)
            {
                string contentType = MimeTypeMap.GetMimeType(doc.Name);
                return PhysicalFile(path, contentType, doc.Name);
            }
            string zipName = _downloadService.CreateZip(path);
            return PhysicalFile(Path.Combine( _downloadService.GetPathForZip(), zipName), "application/zip" , doc.Name + ".zip");
        }

      


    }
}