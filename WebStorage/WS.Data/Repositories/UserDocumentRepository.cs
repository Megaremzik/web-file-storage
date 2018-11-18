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
        public IEnumerable<UserDocument> GetAll(string s = null)
        {

            var userDocs = db.UserDocument.ToList();
            db.SaveChanges();
            return userDocs;
        }
        public void Create(UserDocument userDocument)
        {
            db.UserDocument.Add(userDocument);
            db.SaveChanges();
        }
        public UserDocument Get(string guestEmail, int? documentId)
        {
            var userDocs = db.UserDocument.Where(e => e.GuestEmail == guestEmail && e.DocumentId == documentId).SingleOrDefault();
            db.SaveChanges();
            return userDocs;
        }
        public IEnumerable<UserDocument> GetUserDocumentsByDocumentId(int? documentId)
        {
            var userDocs = db.UserDocument.Where(e => e.DocumentId == documentId);
            db.SaveChanges();
            return userDocs;
        }
        public IEnumerable<UserDocument> GetUserDocumentsByGuestId(string guestEmail)
        {
            var userDocs = db.UserDocument.Where(e => e.GuestEmail == guestEmail);
            db.SaveChanges();
            return userDocs;
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
