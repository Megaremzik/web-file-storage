using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Data.Repositories
{
    public class UserRepository
    {
        private ApplicationDbContext db;
        public UserRepository(ApplicationDbContext context)
        {
            db = context;
        }
        public string GetUserIdByDocumentId(int documentId)
        {
            return db.Document.SingleOrDefault(n => n.Id == documentId)?.UserId;
        }
    }
}
