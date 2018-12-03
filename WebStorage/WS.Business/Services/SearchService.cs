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
        public ICollection<DocumentView> FindTopByPattern(string pattern, int count, ClaimsPrincipal user)
        {
          
            return FindOnPattern(pattern, user).Take(count).ToList();
        }
        public ICollection<DocumentView> GetDocumentsByPattern(string pattern, ClaimsPrincipal user)
        {
            return FindOnPattern(pattern, user).ToList();
        }
        private IEnumerable<DocumentView> FindOnPattern(string pattern, ClaimsPrincipal user)
        {
            var userId = _userService.GetUserId(user);
            pattern = pattern.ToLower();
            return _documentService.GetAll(userId)
                .Where(n => n.Name.ToLower().Contains(pattern))
                .OrderBy(n => n.Name.ToLower().IndexOf(pattern));
        }
    }
}
