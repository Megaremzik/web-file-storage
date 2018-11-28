using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WS.Business.ViewModels;

namespace WS.Business.Services
{
    public class SearchService
    {
        private readonly DocumentService _documentService;
        private readonly UserService _userService;
        public SearchService(DocumentService documentService, UserService userService)
        {
            _documentService = documentService;
            _userService = userService;
        }

        public ICollection<DocumentView> GetDocumentsByPattern(string pattern, ClaimsPrincipal user)
        {
            string id = _userService.GetUserId(user);
            pattern = pattern.ToLower();
            return _documentService.GetAll(id)
                .Where(n => n.Name.ToLower().Contains(pattern))
                .OrderBy(n=>n.Name.ToLower().IndexOf(pattern))
                .ToList();
        }
    }
}
