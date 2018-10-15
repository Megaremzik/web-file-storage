using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WS.Web.Models.IRepository
{
    public interface IFileRepository
    {
        void Create(File file);
        void Delete(int id);
        File Get(int id);
        List<File> GetUserFiles(int owner_id);
        void Update(File user);
    }
}
