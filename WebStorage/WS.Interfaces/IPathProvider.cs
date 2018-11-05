using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Interfaces
{
    public interface IPathProvider
    {
        string MapPath(string name, string path = null);
        string MapId(string id);
        string SplitPath(string fullpath);
        string GetRootPath();
    }
}
