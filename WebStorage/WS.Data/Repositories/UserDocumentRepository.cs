using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Data;
using WS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WS.Data
{
    public class UserDocumentRepository : IRepository<UserDocument>
    {
        private ApplicationDbContext db;
        public UserDocumentRepository(ApplicationDbContext context)
        {
            db = context;
        }
        public IEnumerable<UserDocument> GetAll()
        {
            return db.UserDocument.ToList();
        }
        public void Create(UserDocument userDocument)
        {
            db.UserDocument.Add(userDocument);
            db.SaveChanges();
        }
        public UserDocument Get(string id1, int? id2)
        {
            return db.UserDocument.Where(e => e.UserId == id1).Where(e => e.DocumentId == id2).SingleOrDefault();
        }
        public UserDocument Get(int? id)
        {
            return db.UserDocument.Where(e => e.DocumentId == id).SingleOrDefault();
        }

            public void Update(UserDocument userDocument)
        {
            db.UserDocument.Update(userDocument);
            db.SaveChanges();
        }

        public void Delete(string id1, int? id2)
        {
            db.UserDocument.Remove(Get(id1,id2));
            db.SaveChanges();
        }
        public void Delete(int? id)
        {
            db.UserDocument.Remove(Get(id));
            db.SaveChanges();
        }

    }
}
