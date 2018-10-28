using System;
using WS.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WS.Business.ViewModels;

namespace WS.Business.Services
{
    public class SharingService
    {
        private DocumentService _documentService;
        private DocumentLinkService _documentLinkService;
        private UserDocumentService _userDocument;
        private UserService _userService;
        private UserManager<User> _userManager;
        private char DocumentLinkIdentifier = 'p';
        private char UserDocumentIdentifier = 'u';
        public SharingService
            (
            DocumentService documentService,
            DocumentLinkService documentLinkService,
            UserDocumentService userDocumentService,
            UserService userService,
            UserManager<User> manager
            )
        {
            _documentService = documentService;
            _documentLinkService = documentLinkService;
            _userDocument = userDocumentService;
            _userService = userService;
            _userManager = manager;
        }

        public string OpenPublicAccesToFile(int documentId, bool isEditable, string userName)
        {
            DocumentView document = _documentService.Get(documentId);
            if (document == null)
            {
                throw new Exception("Document is not exist");
            }
            string userId = _userService.GetUserIdByDocumentId(documentId);
            string userId2 = _userService.GetUserIdByName(userName);
            if (userId != userId2)
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }
            string guid = GenerateUniqueGuid();

            DocumentLinkView docLink = _documentLinkService.Get(documentId);
            if (docLink == null)
            {
                _documentLinkService.Create(new DocumentLinkView { Id = documentId, IsEditable = isEditable, Link = guid });
            }

            return DocumentLinkIdentifier + guid;
        }
        public void ClosePublicAccesToFile(int documentId, string userName)
        {
            string userId = _userService.GetUserIdByDocumentId(documentId);
            string userId2 = _userService.GetUserIdByName(userName);
            if (userId != userId2)
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }
       
            DocumentView document = _documentService.Get(documentId);
            if (document != null)
            {
                _documentLinkService.Delete(documentId);
            
            }
        }
        public DocumentView GetSharedDocument(string guid, string userName)
        {
           if(guid[0]==DocumentLinkIdentifier)
            {
                return _documentService.Get(_documentLinkService.GetAll().FirstOrDefault(p => p.Link == guid.Substring(1))?.Id);
            }
            return null;
        }
        
        
        

        private string GenerateUniqueGuid()
        {
            var links = _documentLinkService.GetAll().Select(n => n.Link);
            string guid;
            do
            {
                guid = Guid.NewGuid().ToString();
            } while (links.Contains(guid));
            return guid;
        }

        public void Createdocuments()
        {
            _documentService.Create(new DocumentView { Date_change = DateTime.Now, IsFile = true, Name = "333", ParentId = 0, Size = 545, UserId = "7a85225a-0682-43e7-847d-cb11a641ede3" });
            _documentService.Create(new DocumentView { Date_change = DateTime.Now, IsFile = false, Name = "444", ParentId = 0, Size = 0, UserId = "3f0ff592-f8a8-4937-9421-12082599f890" });
        }

    }
}
