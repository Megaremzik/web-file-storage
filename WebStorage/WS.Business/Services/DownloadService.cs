using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;

namespace WS.Business.Services
{
    public class DownloadService
    {
        private readonly string PathForZip = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\zips");
        PathProvider _pathProvider;
        public DownloadService(PathProvider pathProvider)
        {
            _pathProvider = pathProvider;
        }
        public string GetPathForZip()
        {
            return PathForZip;
        }

        public string CreateZip(string fullPath)
        {
            
         
            string folderName = fullPath.Substring(fullPath.LastIndexOf("\\") + 1);
            string zipName = DateTime.Now.ToString("o").Replace(':', ';') + ".zip";
            System.IO.Compression.ZipFile.CreateFromDirectory(fullPath, Path.Combine(GetPathForZip(), zipName));
            return zipName;
        }
    }
}