﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WS.Interfaces;
using WS.Business.Services;
using WS.Business.ViewModels;
using WS.Data;
namespace WS.Web.Controllers
{
    public class DocumentController : Controller
    {
        private DocumentService _service;
        private IPathProvider _pathProvider;
        private readonly UserManager<User> _userManager;


        public DocumentController(IPathProvider pathProvider,DocumentService service, UserManager<User> userManager)
        {
            _service = service;
            _pathProvider = pathProvider;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            _pathProvider.MapId(userId);
            var documents = _service.GetAll(userId);
            return View(documents);
        }
        public IActionResult ReturnDocumentList()
        {
            string userId = _userManager.GetUserId(User);
            var documents = _service.GetAll(userId);
            return PartialView("_GetDocuments",documents);
        }
        public IActionResult FileOptions(DocumentView doc)
        {
            return PartialView("_FileOptions", doc);
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file, string fullpath)
        {
            string userId = _userManager.GetUserId(User);
            var folders = _pathProvider.SplitPath(fullpath);
            var uploads = Path.Combine(_pathProvider.GetRootPath(), folders);
            var parentId=_service.CreateFolders(folders,userId);
            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    _service.Create(file,userId,parentId);
                }
            }
            var documents = _service.GetAll(userId);
            return PartialView("_GetDocuments", documents);
            //return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
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