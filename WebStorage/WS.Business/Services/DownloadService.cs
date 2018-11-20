using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;

namespace WS.Business.Services
{
    public class DownloadService
    {
        PathProvider _pathProvider;
        public DownloadService(PathProvider pathProvider)
        {
            _pathProvider = pathProvider;
        }
        public string Get(int documentId)
        {
            return _pathProvider.GetFullPath(documentId);
        }
        public string GetContentType(string fileName)
        {
            return MimeMapping.GetMimeMapping(fileName);
        }
        public string CreateZip(string fullPath)
        {
            string folderName = fullPath.Substring(fullPath.LastIndexOf("\\")+1);
            string zipName = folderName + "-" + Guid.NewGuid().ToString() + ".zip";
            System.IO.Compression.ZipFile.CreateFromDirectory(fullPath, _pathProvider.GetRootPath() + "\\" + zipName);
            return zipName;
        }
    }
}
