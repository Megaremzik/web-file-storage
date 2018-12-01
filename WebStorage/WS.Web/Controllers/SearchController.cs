using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WS.Business;
using WS.Business.Services;
using WS.Business.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WS.Web.Controllers
{
    public class SearchController : Controller
    {
        SearchService _searchService;
        DocumentService _documentService;
        public SearchController(SearchService searchService, DocumentService documentService)
        {
            _searchService = searchService;
            _documentService = documentService;
        }

        public IActionResult Find(string pattern)
        {
            var docs = _searchService.GetDocumentsByPattern(pattern, User)
                .Select(n => new DocumentSearch { Id = n.Id, IsFile = n.IsFile, Name = n.Name, Size = n.Size, Path = "FoxBox" + _documentService.GetPathToFile(n.Id) })
                .ToList();
            return View(docs);
        }
        public ICollection<DocumentSearch> FindTop(string pattern, int count)
        {
            if (pattern == null || pattern == string.Empty)
            {
                return new List<DocumentSearch>();
            }
            var a = _searchService.FindTopByPattern(pattern, count, User)
                .Select(n => new DocumentSearch { Id = n.Id, IsFile = n.IsFile, Name = n.Name, Size = n.Size, Path = "FoxBox" + _documentService.GetPathToFile(n.Id) })
                .ToList();
            return a;
        }
        public IActionResult GetDocument(int documentId)
        {
            var doc = _documentService.Get(documentId);
            if (doc.IsFile)
            {
                return View("../Share/Get", doc);
            }
            return RedirectToAction("Index", "Document", new { id = documentId });
        }
    }
}
