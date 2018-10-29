using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Interfaces;

namespace WS.Business
{
    public class PathProvider : IPathProvider
    {
        private IHostingEnvironment _hostingEnvironment;
        private string rootpath;
        public PathProvider(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            rootpath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            CreateFolder(rootpath);
        }

        public string MapPath(string path)
        {
            var filePath = Path.Combine(rootpath, path);
            CreateFolder(filePath);
            return filePath;
        }

        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}
