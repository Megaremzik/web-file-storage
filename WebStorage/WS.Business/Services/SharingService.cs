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
    class SharingService
    {
        DocumentService _documentService;
        DocumentLinkService _linkService;
        UserDocumentService _userDocument;
        UserService _userService;
        UserManager<User> _userManager;

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
            _linkService = documentLinkService;
            _userDocument = userDocumentService;
            _userService = userService;
            _userManager = manager;
        }

        public string OpenPublicAccesToFile(int documentId, bool isEditable, User user)
        {
            DocumentView document = _documentService.Get(documentId);
            if(document==null)
            {
                throw new Exception("Document is not exist");
            }
            string userId = _userService.GetUserIdByDocumentId(documentId);
            if (userId != user.Id)
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }
            string guid = GenerateUniqueGuid();

            _linkService.Create(new DocumentLinkView { Id = documentId, IsEditable = isEditable, Link = guid });
            return guid;
        }
        public void ClosePublicAccesToFile(int documentId, User user)
        {
            DocumentView document = _documentService.Get(documentId);
            if (document == null)
            {
                throw new Exception("Document is not exist");
            }
            string userId = _userService.GetUserIdByDocumentId(documentId);
            if (userId != user.Id)
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }
            if (_linkService.Get(documentId) != null)
            {
                _linkService.Delete(documentId);
            }
        }

        private string GenerateUniqueGuid()
        {
            var links = _linkService.GetAll().Select(n => n.Link);
            string guid;
            do
            {
                guid = Guid.NewGuid().ToString();
            } while (links.Contains(guid));
            return guid;
        }
    }
}
