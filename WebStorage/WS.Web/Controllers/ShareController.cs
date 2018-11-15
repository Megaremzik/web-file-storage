using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using WS.Business;
using WS.Business.Services;
using WS.Business.ViewModels;
using WS.Data;

namespace WS.Web.Controllers
{
    public class ShareController : Controller
    {
        private SharingService _sharingService;
        private DocumentService _documentService;
        public ShareController(SharingService sharingService, DocumentService documentService)
        {
            _sharingService = sharingService;
            _documentService = documentService;

        }
        public IActionResult GetPublicAccessLink(int documentId)
        {

            string guid = _sharingService.GetPublicAccessLink(documentId, User.Identity.Name);
            string link;
            if (guid == null)
            {
                link = "";
            }
            else
            {
                link = Request.Host.Value + "/Share/Get?id=" + guid;
            }
            return Content(link);
        }
        public IActionResult AddAccessForUser(int documentId, string guestEmail, bool isEditable)
        {
            string guid = _sharingService.OpenLimitedAccesToFile(documentId, isEditable, User.Identity.Name, guestEmail);
            //   string link = Request.Host.Value + "/Share/Get?id=" + guid;
            //   string message = $"Вам открыли доступ к файлу по следующей ссылке <a href=\"{link}\">Ссылка</a>";
            //     await _emailSender.SendEmailAsync(guestEmail, "WebStorage", message);
            return Content(Request.Host.Value + "/Share/Get?id=" + guid);

        }
        public IActionResult DeleteAccessForUser(int documentId, string guestEmail)
        {
            _sharingService.RemoveAccessForUser(documentId, User.Identity.Name, guestEmail);
            return Content("Ok");
        }
        public IActionResult OpenPublicAccess(int documentId, bool IsEditable)
        {
            string guid = _sharingService.OpenPublicAccesToFile(documentId, IsEditable, User.Identity.Name);
            string link = Request.Host.Value + "/Share/Get?id=" + guid;
            return Content(link);
        }
        public IActionResult ClosePublicAccess(int documentId)
        {
            _sharingService.ClosePublicAccesToFile(documentId, User.Identity.Name);
            return Content("Ok");
        }
        public IActionResult CloseLimitedAccess(int documentId)
        {
            _sharingService.CloseLimitedAccesToFileEntire(documentId, User.Identity.Name);
            return Content("Ok");
        }

        public IActionResult Get(string id, string adm)
        {
            DocumentView doc = null;
            if (adm == "pub")
            {
                doc = _sharingService.GetPublicSharedDocument(id, HttpContext.User.Identity.Name, out bool isEditable);
            }
            else if (adm == "lim")
            {
                doc = _sharingService.GetLimitedSharedDocument(id, HttpContext.User.Identity.Name, out bool isEditable);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }

            if (doc.IsFile)
            {
                return View(doc);
            }
            else
            {
                var docs = _documentService.GetAllChildren(doc.Id);
                ViewBag.FolderName = doc.Name;
                return View("GetFolder", docs);
            }

        }
        public UserDocumentsView GetAllUsersForSharedDocument(int documentId)
        {
            var userdocs = _sharingService.GetAllUsersForSharedDocument(documentId, HttpContext.User.Identity.Name);
            return new UserDocumentsView { users = _sharingService.GetAllUsersForSharedDocument(documentId, HttpContext.User.Identity.Name) };
        }
    }

}