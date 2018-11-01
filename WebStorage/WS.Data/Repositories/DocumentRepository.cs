﻿using System;
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
    public class DocumentRepository : IRepository<Document>
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
            var doc=db.Document.Where(d => d.Name == document.Name && d.ParentId==document.ParentId);
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
        public int GetIdByName(string name, int parentId)
        {
            return db.Document.FirstOrDefault(p => p.Name == name).Id;
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
        public void Delete(string id1, int? id2)
        {
            Delete(id2);
        }
    }
}
