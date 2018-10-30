using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Interfaces
{
    public interface IPathProvider
    {
        string MapPath(string path);
        string[] SplitPath(string fullpath);
    }
}
