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
        public IEnumerable<Document> GetAllWithotDeleted(string id)
        {
            return db.Document.Where(d => d.User.Id == id && d.Type_change != "Delete" && d.Type_change != "SaveForFile").ToList();
        }

        public int FindSame(Document document)
        {
            var doc = db.Document.Where(d => d.Name == document.Name && d.ParentId == document.ParentId && d.UserId == document.UserId && d.Type_change != "Delete");
            if (doc.Count() == 0)
            {
                return -1;
            }
            return doc.FirstOrDefault().Id;
        }

        public int Create(Document document)
        {
            var doc = db.Document.Where(d => d.Name == document.Name && d.ParentId == document.ParentId && d.UserId == document.UserId && d.Type_change!="Delete");
            if (doc.Count() == 0)
            {
                var finDoc = db.Document.Add(document);
                db.SaveChanges();
                return finDoc.Entity.Id;
            }
            return doc.FirstOrDefault().Id;
        }
        public Document Get(int? id)
        {
            return db.Document.FirstOrDefault(p => p.Id == id);
        }
        public int GetIdByName(string userId,string name, int parentId)
        {
            return db.Document.LastOrDefault(d => d.Name == name && d.ParentId == parentId && d.UserId == userId && d.Type_change!="Delete").Id;
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
        public IEnumerable<Document> GetAllDeletedWithIt(int? id)
        {
            var doc = Get(id);
            var documents = db.Document.Where(d => d.Type_change == "Delete" && d.Date_change == doc.Date_change).ToList();
            return documents;
        }

        public IEnumerable<Document> GetAllChildrenDeletedWithIt(int? id)
        {
            var doc = Get(id);
            var documents = db.Document.Where(d => d.ParentId == id && d.Type_change=="Delete" && d.Date_change== doc.Date_change).ToList();
            return documents;
        }
        public IEnumerable<Document> GetAllWirtualChildrenDeletedWithIt(int? id)
        {
            var doc = Get(id);
            var documents = db.Document.Where(d => d.ParentId == id && (d.Type_change == "Delete"|| d.Type_change == "SaveForFile") && d.Date_change == doc.Date_change).ToList();
            return documents;
        }
        public IEnumerable<Document> GetAllRootElements(string userId)
        {
            var documents = db.Document.Where(d => d.ParentId == 0 && d.UserId==userId).ToList();
            return documents;
        }

        public IEnumerable<Document> GetAllChildrenWithoutDeleted(int? id)
        {
            var documents = db.Document.Where(d => d.ParentId == id && d.Type_change != "Delete" && d.Type_change != "SaveForFile").ToList();
            return documents;
        }
        public IEnumerable<Document> GetAllRootElementsWithoutDeleted(string userId)
        {
            var documents = db.Document.Where(d => d.ParentId == 0 && d.UserId == userId && d.Type_change != "Delete" && d.Type_change != "SaveForFile").ToList();
            return documents;
        }

        public IEnumerable<Document> GetAllDeletedFiles()
        {
            var documents = db.Document.Where(d => d.Type_change == "Delete").ToList();

            return documents;
        }
    }
}
