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
        private UserDocumentService _userDocumentService;
        private UserService _userService;

        public SharingService
            (
            DocumentService documentService,
            DocumentLinkService documentLinkService,
            UserDocumentService userDocumentService,
            UserService userService
            )
        {
            _documentService = documentService;
            _documentLinkService = documentLinkService;
            _userDocumentService = userDocumentService;
            _userService = userService;
        }
        public DocumentLinkView GetPublicAccessLink(int documentId, string userName)
        {
            if (!_userService.IsUserTheOwnerOfTheDocument(userName, documentId))
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }
            return _documentLinkService.Get(documentId);
        }
        public string OpenPublicAccesToFile(int documentId, bool isEditable, string userName)
        {

            if (!_documentService.IsDocumentExist(documentId))
            {
                throw new Exception("Document is not exist");
            }

            if (!_userService.IsUserTheOwnerOfTheDocument(userName, documentId))
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }

            DocumentLinkView docLink = _documentLinkService.Get(documentId);
            if (docLink != null)
            {
                if(docLink.IsEditable==isEditable)
                {
                    return docLink.Link;
                }
                docLink.IsEditable = isEditable;
                _documentLinkService.Update(docLink);
                return docLink.Link;
            }
            string guid = Guid.NewGuid().ToString();
            _documentLinkService.Create(new DocumentLinkView { Id = documentId, IsEditable = isEditable, Link = guid });
            return guid;
        }

        public ICollection<UserDocumentJsonView> GetAllUsersForSharedDocument(int documentId, string userName)
        {
            if (!_documentService.IsDocumentExist(documentId))
            {
                throw new Exception("Document is not exist");
            }

            if (!_userService.IsUserTheOwnerOfTheDocument(userName, documentId))
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }
            var docs = _userDocumentService.GetUserDocumentsByDocumentId(documentId)
                .Select(n => new UserDocumentJsonView { GuestEmail = n.GuestEmail, IsEditable = n.IsEditable, Link = n.Link });
            return docs.ToList();
        }
        public void ClosePublicAccesToFile(int documentId, string userName)
        {

            if (!_userService.IsUserTheOwnerOfTheDocument(userName, documentId))
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }
        
            DocumentLinkView documentLink = _documentLinkService.Get(documentId);
            if (documentLink != null)
            {
                _documentLinkService.Delete(documentId);
            }
        }

        public DocumentView GetPublicSharedDocument(string guid, string userName, out bool isEditable)
        {
            DocumentLinkView docLink = _documentLinkService.GetAll().FirstOrDefault(p => p.Link == guid);
            if (docLink == null)
            {
                throw new Exception("Id not found");
            }
            DocumentView doc = _documentService.Get(docLink.Id);
            isEditable = docLink.IsEditable;
            return doc;
        }
        public DocumentView GetLimitedSharedDocument(string guid, string userName, out bool isEditable)
        {
            UserView user = _userService.GetUserByName(userName);
            UserDocumentView userDoc = _userDocumentService.GetUserDocumentsByGuestId(user.Email).FirstOrDefault(p => p.Link == guid);
            DocumentView doc = _documentService.Get(userDoc.DocumentId);
            isEditable = userDoc.IsEditable;
            return doc;
        }
        public string OpenLimitedAccesToFile(int documentId, bool isEditable, string ownerName, string guestEmail)
        {
            if (!_documentService.IsDocumentExist(documentId))
            {
                throw new Exception("Document is not exist");
            }
            if (!_userService.IsUserTheOwnerOfTheDocument(ownerName, documentId))
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }
            var userDoc = _userDocumentService.Get(guestEmail, documentId);
            if (userDoc != null)
            {
                if (userDoc.IsEditable == isEditable)
                {
                    return userDoc.Link;
                }
                userDoc.IsEditable = isEditable;
                _userDocumentService.Update(userDoc);
                return userDoc.Link;
            }

            //If we have already had guid for this document, we use it
            UserDocumentView userDocForSampleGuid = _userDocumentService.GetUserDocumentsByDocumentId(documentId).FirstOrDefault();
            string guid;
            if (userDocForSampleGuid != null)
            {
                guid = userDocForSampleGuid.Link;
            }
            else
            {
                guid = Guid.NewGuid().ToString();
            }
            //need rewriting
            UserDocumentView userDocum = _userDocumentService.Get(guestEmail, documentId);
            if (userDocum == null)
            {
                _userDocumentService.Create(new UserDocumentView { DocumentId = documentId, Link = guid, GuestEmail = guestEmail, IsEditable = isEditable });
            }
            return guid;
        }
        public void RemoveAccessForUser(int documentId, string ownerName, string guestName)
        {
            if (!_documentService.IsDocumentExist(documentId))
            {
                throw new Exception("Document is not exist");
            }
            if (!_userService.IsUserTheOwnerOfTheDocument(ownerName, documentId))
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }
            var userDoc = _userDocumentService.Get(guestName, documentId);
            if (userDoc != null)
            {
                _userDocumentService.Delete(guestName, documentId);
            }

        }
        public void CloseLimitedAccesToFileEntire(int documentId, string userName)
        {

            if (!_userService.IsUserTheOwnerOfTheDocument(userName, documentId))
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }

            IEnumerable<UserDocumentView> documents = _userDocumentService.GetUserDocumentsByDocumentId(documentId);
            foreach (var p in documents)
            {
                _userDocumentService.Delete(p.GuestEmail, p.DocumentId);
            }
            
        }
    }
}
