using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Data;
using WS.Interfaces;
using AutoMapper;
using WS.Business.ViewModels;

namespace WS.Business.Services
{
    public class DocumentService
    {
        private IRepository<Document> repo;
        public DocumentService(IRepository<Document> r)
        {
            repo = r;
        }

        public IEnumerable<DocumentView> GetAll()
        {
            IEnumerable<Document> documents = repo.GetAll();
            return Mapper.Map<IEnumerable<Document>, IEnumerable<DocumentView>>(documents); ;
        }

        public DocumentView Get(int? id)
        {
            Document document = repo.Get(id);
            return Mapper.Map<Document, DocumentView>(document);
        }

        public void Create(DocumentView documentView)
        {
            Document document = Mapper.Map<DocumentView, Document>(documentView);
            repo.Create(document);
        }
   
        public void Update(DocumentView documentView)
        {
            Document document = Mapper.Map<DocumentView, Document>(documentView);
            repo.Update(document);
        }

        public void Delete(int? id)
        {
            repo.Delete(id);
        }
    }
}
