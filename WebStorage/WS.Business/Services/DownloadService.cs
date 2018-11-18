using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
