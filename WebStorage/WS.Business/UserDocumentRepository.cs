using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Data;
using WS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WS.Business
{
    public class UserDocumentRepository : I2Repository<UserDocument>
    {
        private StorageContext db;
        public UserDocumentRepository(StorageContext context)
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
        public UserDocument Get(int? id1, int? id2)
        {
            return db.UserDocument.Where(e => e.IdUser == id1).Where(e => e.IdDocument == id2).SingleOrDefault();
        }

        public void Update(UserDocument userDocument)
        {
            db.UserDocument.Update(userDocument);
            db.SaveChanges();
        }

        public void Delete(int? id1, int? id2)
        {
            db.UserDocument.Remove(Get(id1,id2));
            db.SaveChanges();
        }
    }
}
