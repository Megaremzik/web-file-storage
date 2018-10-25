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
    public class UserDocumentService
    {
        private IRepository<UserDocument> repo;
        private readonly IMapper mapper;

        public UserDocumentService(IMapper map, IRepository<UserDocument> r)
        {
            mapper = map;
            repo = r;
        }

        public IEnumerable<UserDocumentView> GetAll()
        {
            IEnumerable<UserDocument> userDocuments = repo.GetAll();
            return mapper.Map<IEnumerable<UserDocument>, IEnumerable<UserDocumentView>>(userDocuments);
        }
        public UserDocumentView Get(string id1, int? id2)
        {
            UserDocument userDocument = repo.Get(id1,id2);
            return mapper.Map<UserDocument, UserDocumentView>(userDocument);
        }

        public void Create(UserDocumentView userDocumentView)
        {
            UserDocument userDocument = mapper.Map<UserDocumentView, UserDocument>(userDocumentView);
            repo.Create(userDocument);
        }

        public void Update(UserDocumentView userDocumentView)
        {
            UserDocument userDocument = mapper.Map<UserDocumentView, UserDocument>(userDocumentView);
            repo.Update(userDocument);
        }

        public void Delete(string id1, int? id2)
        {
            repo.Delete(id1, id2);
        }
    }
}
