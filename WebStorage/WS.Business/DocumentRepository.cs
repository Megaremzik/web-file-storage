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
    public class DocumentRepository : IRepository<Document>
    {
        private StorageContext db;
        public DocumentRepository(StorageContext context)
        {
            db = context;
        }
        public IEnumerable<Document> GetAll()
        {
            return db.Document.ToList();
        }
        public void Create(Document document)
        {
            db.Document.Add(document);
            db.SaveChanges();
        }
        public Document Get(int? id)
        {
            return db.Document.FirstOrDefault(p => p.Id == id);
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
    }
}
