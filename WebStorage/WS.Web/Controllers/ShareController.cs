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
        EmailSender _emailSender;

        public ShareController(SharingService sharingService, DocumentService documentService, EmailSender emailSender)
        {
            _sharingService = sharingService;
            _documentService = documentService;
            _emailSender = emailSender;
        }
        public DocumentLinkJsonView GetPublicAccessLink(int documentId)
        {

            DocumentLinkView docLink = _sharingService.GetPublicAccessLink(documentId, User);

            if (docLink == null)
            {
                return new DocumentLinkJsonView { Link = "", IsEditable = false };
            }
            return new DocumentLinkJsonView { Link = Request.Host.Value + "/Share/Get?id=" + docLink.Link + "&adm=pub", IsEditable = docLink.IsEditable };
        }

        public async Task<IActionResult> AddAccessForUser(int documentId, string guestEmail, bool isEditable)
        {
            string guid = _sharingService.OpenLimitedAccesToFile(documentId, isEditable, User, guestEmail);
            string link = Request.Host.Value + "/Share/Get?id=" + guid + "&adm=lim";
            string subject = "Пользователь " + User.Identity.Name + " поделился с вами файлом";
            string message = "<p>Пользователь " + User.Identity.Name +  $" поделился с вами файлом по следующей ссылке {link}</p>";
            await _emailSender.SendEmailAsync(guestEmail, subject, message);
            return Content(link);
        }

        public IActionResult DeleteAccessForUser(int documentId, string guestEmail)
        {
            _sharingService.RemoveAccessForUser(documentId, User, guestEmail);
            return Ok();
        }
        public IActionResult OpenPublicAccess(int documentId, bool IsEditable)
        {
            string guid = _sharingService.OpenPublicAccesToFile(documentId, IsEditable, User);
            string link = Request.Host.Value + "/Share/Get?id=" + guid + "&adm=pub";
            return Content(link);
        }
        public IActionResult ClosePublicAccess(int documentId)
        {
            _sharingService.ClosePublicAccesToFile(documentId, User);
            return Ok();
        }
        public IActionResult CloseLimitedAccess(int documentId)
        {
            _sharingService.CloseLimitedAccesToFileEntire(documentId, User);
            return Ok();
        }

        public IActionResult Get(string id, string adm)
        {
            DocumentView doc = null;
            if (adm == "pub")
            {
                doc = _sharingService.GetPublicSharedDocument(id, User, out bool isEditable);
            }
            else if (adm == "lim")
            {
                doc = _sharingService.GetLimitedSharedDocument(id, User, out bool isEditable);
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

                var docs = _documentService.GetAllChildrensForFolder(doc.Id);

                return RedirectToAction("Index", "Document", new { id = doc.Id });


                //var docs = _documentService.GetAllChildren(doc.Id);
                //ViewBag.FolderName = doc.Name;
                //return View("GetFolder", docs);
            }

        }
        public ICollection<UserDocumentJsonView> GetAllUsersForSharedDocument(int documentId)
        {
            var userdocs = _sharingService.GetAllUsersForSharedDocument(documentId, User);
            return userdocs;
        }
    }

}