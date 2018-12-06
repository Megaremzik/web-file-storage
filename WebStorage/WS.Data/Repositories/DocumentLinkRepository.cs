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
    public class DocumentLinkRepository
    {
        private ApplicationDbContext db;
        public DocumentLinkRepository(ApplicationDbContext context)
        {
            db = context;
        }
        public IEnumerable<DocumentLink> GetAll(string s=null)
        {
            var docLinks= db.DocumentLink.ToList();
            db.SaveChanges();
            return docLinks;
        }
        public void Create(DocumentLink documentLink)
        {
            db.DocumentLink.Add(documentLink);
            db.SaveChanges();
        }
        public DocumentLink Get(int? id)
        {
            var docLink = db.DocumentLink.FirstOrDefault(p => p.Id == id);
            db.SaveChanges();
            return docLink;
        }
        public void Update(DocumentLink documentLink)
        {
            db.DocumentLink.Update(documentLink);
            db.SaveChanges();
        }

        public void Delete(int? id)
        {
            db.DocumentLink.Remove(Get(id));
            db.SaveChanges();
        }
    }
}
