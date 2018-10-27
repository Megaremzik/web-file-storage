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
        UserManager<User> _userManager;

        public SharingService(
            DocumentService documentService,
            DocumentLinkService documentLinkService,
            UserDocumentService userDocumentService,
            UserManager<User> manager
            )
        {
            _documentService = documentService;
            _linkService = documentLinkService;
            _userDocument = userDocumentService;
            _userManager = manager;
        }

        public string OpenPublicAccesToFile(int fileId, User user)
        {
            
            return null;
        }
    }
}
