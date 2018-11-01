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
            rootpath = Path.Combine(rootpath, path);
            CreateFolder(rootpath);
            return rootpath;
        }

        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }
        public string[] SplitPath(string path)
        {
            if (path == null) return null;
            var str = path.Split('/');
            for(int i = 0; i < str.Length - 1; i++)
            {
                MapPath(str[i]);
            }
            return str;
        }
    }
}
