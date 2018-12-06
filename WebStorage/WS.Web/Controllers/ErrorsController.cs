using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WS.Web.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.StatusCose = statusCode;
                    ViewBag.ErrorMessage = "Sorry the page you requested could not be found";
                    break;
                case 500:
                    ViewBag.StatusCose = statusCode;
                    ViewBag.ErrorMessage = "Sorry something went wrong on the server";
                    break;
            }

            return View();
        }

        [Route("Error/500")]
        public IActionResult Error500()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                ViewBag.StatusCose = 500;
                ViewBag.ErrorMessage = exceptionFeature.Error.Message;
            
            }

            return View();
        }
    }
}