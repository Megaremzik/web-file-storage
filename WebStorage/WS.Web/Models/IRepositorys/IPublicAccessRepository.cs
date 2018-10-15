using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WS.Web.Models.IRepository
{
    interface IPublicAccessRepository
    {
        void Create(PublicAccess LimitedAccess);
        void Delete(int id);
        void Update(PublicAccess user);
        PublicAccess Get(int id);
        List<PublicAccess> GetFileAccess(int file_id);
    }
}
