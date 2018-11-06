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
    public class UserDocumentRepository
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
        public UserDocument Get(string guestEmail, int? documentId)
        {
            return db.UserDocument.Where(e => e.GuestEmail == guestEmail).Where(e => e.DocumentId == documentId).SingleOrDefault();
        }
        public IEnumerable<UserDocument> GetUserDocumentsByDocumentId(int? documentId)
        {
            return db.UserDocument.Where(e => e.DocumentId == documentId);
        }
        public IEnumerable<UserDocument> GetUserDocumentsByGuestId(string guestEmail)
        {
            return db.UserDocument.Where(e => e.GuestEmail == guestEmail);
        }
        public void Update(UserDocument userDocument)
        {
            db.UserDocument.Update(userDocument);
            db.SaveChanges();
        }
        public void Delete(string id1, int? id2)
        {
            db.UserDocument.Remove(Get(id1, id2));
            db.SaveChanges();
        }
    }
}
