using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WS.Business.Services;
using WS.Business.ViewModels;

namespace WS.Web.Controllers
{
    public class UserDocumentController : Controller
    {
        private UserDocumentService service;
        public UserDocumentController(UserDocumentService s)
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
        public IActionResult Create(UserDocumentView userDocument)
        {
            service.Create(userDocument);
            return RedirectToAction("Index");
        }

        public IActionResult Details(string id1, int? id2)
        {
            if (id1 != null && id2 != null)
            {
                UserDocumentView userDocument = service.Get(id1, id2);
                if (userDocument != null)
                    return View(userDocument);
            }
            return NotFound();
        }

        public IActionResult Edit(string id1, int? id2)
        {
            if (id1 != null && id2 != null)
            {
                UserDocumentView userDocument = service.Get(id1, id2);
                if (userDocument != null)
                    return View(userDocument);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(UserDocumentView userDocument)
        {
            service.Update(userDocument);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(string id1, int? id2)
        {
            if (id1 != null && id2 != null)
            {
                UserDocumentView userDocument = service.Get(id1, id2);
                if (userDocument != null)
                    return View(userDocument);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(string id1, int? id2)
        {
            if (id1 != null && id2 != null)
            {
                UserDocumentView userDocument = service.Get(id1, id2);
                if (userDocument != null)
                {
                    service.Delete(id1, id2);
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}