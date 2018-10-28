using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Data.Repositories;

namespace WS.Business.Services
{
    public class UserService
    {
        private UserRepository repo;
        public UserService(UserRepository r)
        {
            repo = r;
        }
        public string GetUserIdByDocumentId(int documentId)
        {
            return repo.GetUserIdByDocumentId(documentId);
        }
        public string GetUserIdByName(string name)
        {
            return repo.GetUserIdByName(name);
        }
    }
}
