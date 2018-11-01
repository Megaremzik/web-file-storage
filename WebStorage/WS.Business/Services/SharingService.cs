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
        private char DocumentLinkIdentifier = 'p';
        private char UserDocumentIdentifier = 'u';
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

        public string OpenPublicAccesToFile(int documentId, bool isEditable, string userName)
        {
            
            if (!IsDocumentExist(documentId))
            {
                throw new Exception("Document is not exist");
            }

            if (!IsUserTheOwnerOfTheDocument(userName, documentId))
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

            if (!IsUserTheOwnerOfTheDocument(userName, documentId))
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }
            //--------------------------------------------------------
            DocumentLinkView documentLink = _documentLinkService.Get(documentId);
            if (documentLink != null)
            {
                _documentLinkService.Delete(documentId);
            }
        }
        public DocumentView GetSharedDocument(string guid, string userName, out bool isEditable)
        {
            if (guid[0] == DocumentLinkIdentifier)
            {
                DocumentLinkView docLink = _documentLinkService.GetAll().FirstOrDefault(p => p.Link == guid.Substring(1));
                if (docLink == null)
                {
                    throw new Exception("Id not found");
                }
                DocumentView doc = _documentService.Get(docLink.Id);
                isEditable = docLink.IsEditable;
                return doc;
            }
            else if (guid[0] == UserDocumentIdentifier)
            {
                UserView user = _userService.GetUserByName(userName);
                UserDocumentView userDoc = _userDocumentService.GetUserDocumentsByGuestId(user.Email).FirstOrDefault(p => p.Link == guid.Substring(1));
                DocumentView doc = _documentService.Get(userDoc.DocumentId);
                isEditable = userDoc.IsEditable;
                return doc;
            }

            isEditable = false;
            return null;
        }
        public string OpenLimitedAccesToFile(int documentId, bool isEditable, string ownerName, string guestEmail)
        {
            
            if (!IsDocumentExist(documentId))
            {
                throw new Exception("Document is not exist");
            }
            
            if (!IsUserTheOwnerOfTheDocument(ownerName, documentId))
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
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
                guid = GenerateUniqueGuid();
            }
            UserDocumentView userDoc = _userDocumentService.Get(guestEmail, documentId);
            if (userDoc == null)
            {
                _userDocumentService.Create(new UserDocumentView { DocumentId = documentId, Link = guid, UserId = guestEmail, IsEditable = isEditable });
            }
            return UserDocumentIdentifier + guid;
        }

        public void CloseLimitedAccesToFileEntire(int documentId, string userName)
        {
            
            if (IsUserTheOwnerOfTheDocument(userName, documentId))
            {
                throw new UnauthorizedAccessException("User is not the owner of the file");
            }

            IEnumerable<UserDocumentView> documents = _userDocumentService.GetUserDocumentsByDocumentId(documentId);
            foreach (var p in documents)
            {
                _userDocumentService.Delete(p.UserId, p.DocumentId);
            }
        }
        private bool IsUserTheOwnerOfTheDocument(string userName, int documentId)
        {
            string userId = _userService.GetUserIdByDocumentId(documentId);
            string userId2 = _userService.GetUserByName(userName).Id;
            return userId == userId2;
        }
        private bool IsDocumentExist(int documentId)
        {
            DocumentView document = _documentService.Get(documentId);
            return document != null ? true : false;
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

        //public void Createdocuments()
        //{
        //    _documentService.Create(new DocumentView { Date_change = DateTime.Now, IsFile = true, Name = "333", ParentId = 0, Size = 545, UserId = "7a85225a-0682-43e7-847d-cb11a641ede3" });
        //    _documentService.Create(new DocumentView { Date_change = DateTime.Now, IsFile = false, Name = "444", ParentId = 0, Size = 0, UserId = "3f0ff592-f8a8-4937-9421-12082599f890" });
        //}
    }
}
