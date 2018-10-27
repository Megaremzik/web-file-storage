using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WS.Business.Services;
using WS.Data;

namespace WS.Web.Controllers
{
    public class ShareController : Controller
    {
        UserDocumentService userDocumentService;

        public IActionResult Index()
        {
            return View();
        }
        HttpContext con;
        public ShareController (UserDocumentService userDocservice)
        {
            userDocumentService = userDocservice;
            
        }
    }
}