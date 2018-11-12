using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Business.ViewModels;
using WS.Data;
using WS.Interfaces;

namespace WS.Business.Services
{
    public class DocumentLinkService
    {
        private DocumentLinkRepository repo;
        private readonly IMapper mapper;

        public DocumentLinkService(IMapper map, DocumentLinkRepository r)
        {
            mapper = map;
            repo = r;
        }

        public IEnumerable<DocumentLinkView> GetAll()
        {
            IEnumerable<DocumentLink> documentLinks = repo.GetAll();
            return mapper.Map<IEnumerable<DocumentLink>, IEnumerable<DocumentLinkView>>(documentLinks); ;
        }

        public DocumentLinkView Get(int? id)
        {
            DocumentLink documentLink = repo.Get(id);
            return mapper.Map<DocumentLink, DocumentLinkView>(documentLink);
        }

        public void Create(DocumentLinkView documentLinkView)
        {
            DocumentLink documentLink = mapper.Map<DocumentLinkView, DocumentLink>(documentLinkView);
            repo.Create(documentLink);
        }

        public void Update (DocumentLinkView documentLinkView)
        {
            DocumentLink documentLink = mapper.Map<DocumentLinkView, DocumentLink> (documentLinkView);
            repo.Update(documentLink);
        }

        public void Delete(int? id)
        {
            repo.Delete(id);
        }

    }
} 
