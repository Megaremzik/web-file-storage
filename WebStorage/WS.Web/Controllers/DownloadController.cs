using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WS.Business.Services;

namespace WS.Web.Controllers
{
    public class DownloadController : Controller
    {
        DownloadService _downloadService;
        public DownloadController(DownloadService downloadService)
        {
            _downloadService = downloadService;
        }
        
  


        public IActionResult Get(int Id)
        {
           return Content(_downloadService.Get(Id));
        }



    }
}