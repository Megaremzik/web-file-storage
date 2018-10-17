using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WS.Data;
using WS.Interfaces;

namespace WS.Web.Controllers
{
    public class DocumentLinkController : Controller
    {
        private IRepository<DocumentLink> repo;
        public DocumentLinkController(IRepository<DocumentLink> r)
        {
            repo = r;
        }

        public IActionResult Index()
        {
            return View(repo.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DocumentLink documentLink)
        {
            repo.Create(documentLink);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            if (id != null)
            {
                DocumentLink documentLink = repo.Get(id);
                if (documentLink != null)
                    return View(documentLink);
            }
            return NotFound();
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                DocumentLink documentLink = repo.Get(id);
                if (documentLink != null)
                    return View(documentLink);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(DocumentLink documentLink)
        {
            repo.Update(documentLink);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int? id)
        {
            if (id != null)
            {
                DocumentLink documentLink = repo.Get(id);
                if (documentLink != null)
                    return View(documentLink);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                DocumentLink documentLink = repo.Get(id);
                if (documentLink != null)
                {
                    repo.Delete(id);
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}