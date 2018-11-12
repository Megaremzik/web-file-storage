using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WS.Business.Services;
using WS.Business.ViewModels;

namespace WS.Web.Controllers
{
    public class DocumentLinkController : Controller
    {
        private DocumentLinkService service;
        public DocumentLinkController(DocumentLinkService s)
        {
            service = s;
        }
 
        public IActionResult Index()
        {
            return View(service.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DocumentLinkView documentLink)
        {
            service.Create(documentLink);
            return RedirectToAction("Index"); 
        }

        public IActionResult Details(int? id)
        {
            if (id != null)
            {
                DocumentLinkView documentLink = service.Get(id);
                if (documentLink != null)
                    return View(documentLink);
            }
            return NotFound();
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                DocumentLinkView documentLink = service.Get(id);
                if (documentLink != null)
                    return View(documentLink);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(DocumentLinkView documentLink)
        {
            service.Update(documentLink);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int? id)
        {
            if (id != null)
            {
                DocumentLinkView documentLink = service.Get(id);
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
                DocumentLinkView documentLink = service.Get(id);
                if (documentLink != null)
                {
                    service.Delete(id);
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}