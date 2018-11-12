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
    public class PathProvider
    {
        private IHostingEnvironment _hostingEnvironment;
        private string rootpath;
        public PathProvider(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            rootpath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            CreateFolder(rootpath);
        }
        public string GetRootPath()
        {
            return rootpath;
        }
        public void MapId(string id)
        {
            if (id != null)
            {
                rootpath = Path.Combine(rootpath, id);
                CreateFolder(rootpath);
            }
            
        }
        public string MapPath(string name, string path = null)
        {
            if (path == null) path = rootpath;
            path = rootpath + "/" + path;
            var filepath = Path.Combine(path, name);
            CreateFolder(filepath);
            return filepath;
        }

        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }
        public string SplitPath(string path)
        {
            if (path == null) return null;
            var str = path.Split('/');
            string folders="";
            for(int i = 0; i < str.Length - 1; i++)
            {
                MapPath(str[i],folders);
                folders += str[i] + "/";
            }

            return folders;
        }
    }
}
