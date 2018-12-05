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
            return db.Document.FirstOrDefault(n => n.Id == documentId)?.UserId;
        }

        public User GetUserByName(string userName)
        {
            return db.Users.FirstOrDefault(n => n.UserName == userName);
        }
        public User GetUserById(string userId)
        {
            return db.Users.FirstOrDefault(n => n.Id==userId);
        }
    }
}
