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
            //_sharingService.OpenPublicAccesToFile(2,false, HttpContext.User.Identity.Name);
            //_sharingService.Createdocuments();
            return View();
        }
        public IActionResult Get()
        {
            DocumentView doc = _sharingService.GetSharedDocument("p3a3ee20a-06da-4d24-b752-ad5e70333fac", HttpContext.User.Identity.Name);
            return View(doc);
        }
        
    }
}