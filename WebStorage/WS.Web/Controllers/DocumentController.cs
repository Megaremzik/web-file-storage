using System;
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
using WS.Business;
using Microsoft.AspNetCore.Routing;

namespace WS.Web.Controllers
{
    public class DocumentController : Controller
    {
        private DocumentService _service;
        private PathProvider _pathProvider;
        private readonly UserManager<User> _userManager;


        public DocumentController(PathProvider pathProvider,DocumentService service, UserManager<User> userManager)
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
        public IActionResult ReturnDocumentList(int parentId=0)
        {
            string userId = _userManager.GetUserId(User);
            IEnumerable<DocumentView> documents;
            if (parentId != 0) documents = _service.GetAllChildren(parentId);
            else documents = _service.GetAllRootElements(userId);
            ViewBag.ParentId = parentId;
            return PartialView("_GetDocuments",documents);
        }
        public IActionResult ReturnParent(int id)
        {
            var parentId = _service.Get(id).ParentId;
            return RedirectToAction("ReturnDocumentList","Document",new { parentId});
        }
        public IActionResult FileOptions(int id)
        {
            var doc = _service.Get(id);
            return PartialView("_FileOptions", doc);
        }
        public IActionResult Paste(int id, int parentId, string type)
        {
            var userId = _userManager.GetUserId(User);
            if (type == "copy")
            {
                _service.CreateACopy(id, parentId);
            }
            else if (type == "cut")
            {
                _service.UpdateParentId(id,parentId);
            }
            return RedirectToAction("ReturnDocumentList", "Document", new { parentId });
        }
        public IActionResult Rename(int id, string name)
        {
            var userId = _userManager.GetUserId(User);
            _service.RenameFile(id, name);
            var parentId = _service.Get(id).ParentId;
            return RedirectToAction("ReturnDocumentList", "Document", new { parentId });
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file, string fullpath,int parentId=0)
        {
            var currentpath = "";
            if (parentId != 0)
            {
                currentpath=_service.GetFilePath(parentId);
            }
            string userId = _userManager.GetUserId(User);
            var folders = _pathProvider.SplitPath(fullpath,_userManager.GetUserId(User),currentpath);
            var uploads = _pathProvider.GetRootPath();
            if (folders != null)
            {
                uploads = Path.Combine(uploads, folders);
                parentId = _service.CreateFolders(fullpath, userId, parentId);
            }
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
            //_service.Update(document);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int? id)
        {
            bool result = false;
            if (id != null)
            {
                _service.MoveToTrash(id);
                result = true;
            }
            return Json(result);
        }



        [HttpPost]
        public IActionResult ViewFile(DocumentView document)
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Share(DocumentView document)
        {
            return RedirectToAction("Index");
        }
    }
}