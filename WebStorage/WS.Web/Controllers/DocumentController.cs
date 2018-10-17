using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WS.Data;
using WS.Interfaces;

namespace WS.Web.Controllers
{
    public class DocumentController : Controller
    {
        private IRepository<Document>  repo;
        public DocumentController(IRepository<Document> r)
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
        public IActionResult Create(Document document)
        {
            repo.Create(document);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            if (id != null)
            {
                Document document = repo.Get(id);
                if (document != null)
                    return View(document);
            }
            return NotFound();
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                Document document = repo.Get(id);
                if (document != null)
                    return View(document);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(Document document)
        {
            repo.Update(document);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Document document = repo.Get(id);
                if (document != null)
                    return View(document);
            }
            return NotFound();
        }

        [HttpPost]
        public  IActionResult Delete(int? id)
        {
            if (id != null)
            {
                Document document = repo.Get(id);
                if (document != null)
                {
                    repo.Delete(id);
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}