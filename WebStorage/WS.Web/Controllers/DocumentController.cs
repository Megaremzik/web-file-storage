using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WS.Business.Services;
using WS.Business.ViewModels;

namespace WS.Web.Controllers
{
    public class DocumentController : Controller
    {
        private DocumentService service;
        public DocumentController(DocumentService s)
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
        public IActionResult Create(DocumentView document)
        {
            service.Create(document);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            if (id != null)
            {
                DocumentView document = service.Get(id);
                if (document != null)
                    return View(document);
            }
            return NotFound();
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                DocumentView document = service.Get(id);
                if (document != null)
                    return View(document);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(DocumentView document)
        {
            service.Update(document);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int? id)
        {
            if (id != null)
            {
                DocumentView document = service.Get(id);
                if (document != null)
                    return View(document);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                DocumentView document = service.Get(id);
                if (document != null)
                {
                    service.Delete(id);
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}