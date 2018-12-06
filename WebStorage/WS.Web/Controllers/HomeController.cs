using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WS.Business.ViewModels;
using WS.Data;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace WS.Web.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly SignInManager<User> _signInManager;

        public HomeController(IHostingEnvironment environment, SignInManager<User> signInManager)
        {
            _hostingEnvironment = environment;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            if(User != null && User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Document"); 
            else
                return RedirectToAction("Login", "Account");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
