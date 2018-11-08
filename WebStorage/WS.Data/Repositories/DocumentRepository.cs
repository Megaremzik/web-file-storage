using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Data;
using WS.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace WS.Data
{
    public class DocumentRepository
    {
        private ApplicationDbContext db;
        public DocumentRepository(ApplicationDbContext context)
        {
            db = context;
        }
        public IEnumerable<Document> GetAll()
        {
            return db.Document.ToList();
        }
        public IEnumerable<Document> GetAll(string id)
        {
            return db.Document.Where(d=>d.User.Id==id).ToList();
        }
        public void Create(Document document)
        {
            var doc=db.Document.Where(d => d.Name == document.Name && d.ParentId==document.ParentId && d.UserId == document.UserId);
            if (doc.Count() == 0)
            {
                db.Document.Add(document);
                db.SaveChanges();
            }   
        }
        public Document Get(int? id)
        {
            return db.Document.FirstOrDefault(p => p.Id == id);
        }
        public int GetIdByName(string userId,string name, int parentId)
        {
            return db.Document.FirstOrDefault(d => d.Name == name && d.ParentId == parentId && d.UserId == userId).Id;
        }
        public Document Get(string id1, int? id2)
        {
            return Get(id2);
        }

        public void Update(Document document)
        {
            db.Document.Update(document);
            db.SaveChanges();
        }

        public void Delete(int? id)
        {
            db.Document.Remove(Get(id));
            db.SaveChanges();
        }
        public IEnumerable<Document> GetAllChildren(int? id)
        {
            var documents= db.Document.Where(d => d.ParentId == id).ToList();
            return documents;
        }
    }
}
