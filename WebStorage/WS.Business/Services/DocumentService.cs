using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Data;
using WS.Interfaces;
using AutoMapper;
using WS.Business.ViewModels;
using Microsoft.AspNetCore.Http;

namespace WS.Business.Services
{
    public class DocumentService
    {
        private IRepository<Document> repo;
        private readonly IMapper mapper;

        public DocumentService(IMapper map, IRepository<Document> r)
        {
            mapper = map;
            repo = r;
        }

        public IEnumerable<DocumentView> GetAll(string id)
        {
            IEnumerable<Document> documents = repo.GetAll(id);
            return mapper.Map<IEnumerable<Document>, IEnumerable<DocumentView>>(documents); ;
        }

        public DocumentView Get(int? id)
        {
            Document document = repo.Get(id);
            return mapper.Map<Document, DocumentView>(document);
        }
        public void Create(IFormFile file)
        {
            Document doc = new Document { IsFile = true, Size = (int)file.Length, Name = file.FileName,Extention=file.ContentType };
            repo.Create(doc);
        }

        public void Update(DocumentView documentView)
        {
            Document document = mapper.Map<DocumentView, Document>(documentView);
            repo.Update(document);
        }

        public void Delete(int? id)
        {
            repo.Delete(id);
        }
    }
}
