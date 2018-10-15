using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WS.Web.Models.IRepository
{
    public interface ILimitedAccessRepository
    {
        void Create(LimitedAccess LimitedAccess);
        void Delete(int file_id, int user_id);
        void Update(LimitedAccess user);
        List<LimitedAccess> GetFileAccess(int file_id);
        List<LimitedAccess> GetUserAccess(int user_id);
    }
}
