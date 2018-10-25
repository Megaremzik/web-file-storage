using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WS.Business.Services;
using WS.Business.ViewModels;

namespace WS.Web.Controllers
{
    public class DocumentController : Controller
    {
        private DocumentService _service;
        private IHostingEnvironment _hostingEnvironment;

        public DocumentController(IHostingEnvironment environment,DocumentService service)
        {
            _service = service;
            _hostingEnvironment = environment;
        }

        public IActionResult Index(List<DocumentView> documents)
        {
            return View(documents);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    //_service.Create()
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DocumentView document)
        {
            _service.Create(document);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            if (id != null)
            {
                DocumentView document = _service.Get(id);
                if (document != null)
                    return View(document);
            }
            return NotFound();
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                DocumentView document = _service.Get(id);
                if (document != null)
                    return View(document);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(DocumentView document)
        {
            _service.Update(document);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int? id)
        {
            if (id != null)
            {
                DocumentView document = _service.Get(id);
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
                DocumentView document = _service.Get(id);
                if (document != null)
                {
                    _service.Delete(id);
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}