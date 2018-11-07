using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WS.Business.Services;
using WS.Business.ViewModels;
using WS.Data;

namespace WS.Web.Controllers
{
    public class ShareController : Controller
    {
        private SharingService _sharingService;

    
        public ShareController ( SharingService sharingService)
        {
            _sharingService = sharingService;
        }
        public IActionResult Index()
        {
            //_sharingService.Createuserdoc();
            //_sharingService.OpenPublicAccesToFile(2,false, HttpContext.User.Identity.Name);
            //_sharingService.Createdocuments();
            return View();
        }
        
        public IActionResult AddAccessForUser(int documentId, string guestEmail, bool IsEditable)
        {
            string link =_sharingService.OpenLimitedAccesToFile(documentId, IsEditable, User.Identity.Name, guestEmail);
            return Content(link);
        }
        public IActionResult DeleteAccessForUser(int documentId, string guestEmail)
        {
             _sharingService.RemoveAccessForUser(documentId, User.Identity.Name, guestEmail);
            return Content("Ok");
        }
        public IActionResult OpenPublicAccess(int documentId, bool IsEditable)
        {
            string link = _sharingService.OpenPublicAccesToFile(documentId, IsEditable, User.Identity.Name);
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

        public IActionResult Get(string id)
        {
            
            
            DocumentView doc = _sharingService.GetSharedDocument(id, HttpContext.User.Identity.Name, out bool isEditable);
            if (doc.IsFile)
            {
                return View(doc);
            }
            else
            {
                return null; //TODO
            }
            
        }
    }
}