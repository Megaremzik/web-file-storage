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
        private UserDocumentRepository repo;
        private readonly IMapper mapper;

        public UserDocumentService(IMapper map, UserDocumentRepository r)
        {
            mapper = map;
            repo = r;
        }

        public IEnumerable<UserDocumentView> GetAll()
        {
            IEnumerable<UserDocument> userDocuments = repo.GetAll();
            return mapper.Map<IEnumerable<UserDocument>, IEnumerable<UserDocumentView>>(userDocuments);
        }
        public UserDocumentView Get(string guestEmail, int? documentId)
        {
            UserDocument userDocument = repo.Get(guestEmail, documentId);
            return mapper.Map<UserDocument, UserDocumentView>(userDocument);
        }
        public IEnumerable<UserDocumentView> GetUserDocumentsByDocumentId(int? documentId)
        {
            IEnumerable<UserDocument> userDocuments = repo.GetUserDocumentsByDocumentId(documentId);
            return mapper.Map<IEnumerable<UserDocument>, IEnumerable<UserDocumentView>>(userDocuments);
        }
        public IEnumerable<UserDocumentView> GetUserDocumentsByGuestId(string guestId)
        {
            IEnumerable<UserDocument> userDocuments = repo.GetUserDocumentsByGuestId(guestId);
            return mapper.Map<IEnumerable<UserDocument>, IEnumerable<UserDocumentView>>(userDocuments);

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
