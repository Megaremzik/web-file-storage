using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WS.Data;
using WS.Interfaces;

namespace WS.Web.Controllers
{
    public class UserDocumentController : Controller
    {
        private I2Repository<UserDocument> repo;
        public UserDocumentController(I2Repository<UserDocument> r)
        {
            repo = r;
        }

        public IActionResult Index()
        {
            return View(repo.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(UserDocument userDocument)
        {
            repo.Create(userDocument);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id1, int? id2)
        {
            if (id1 != null && id2 != null)
            {
                UserDocument userDocument = repo.Get(id1, id2);
                if (userDocument != null)
                    return View(userDocument);
            }
            return NotFound();
        }

        public IActionResult Edit(int? id1, int? id2)
        {
            if (id1 != null && id2 != null)
            {
                UserDocument userDocument = repo.Get(id1, id2);
                if (userDocument != null)
                    return View(userDocument);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(UserDocument userDocument)
        {
            repo.Update(userDocument);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int? id1, int? id2)
        {
            if (id1 != null && id2 != null)
            {
                UserDocument userDocument = repo.Get(id1, id2);
                if (userDocument != null)
                    return View(userDocument);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(int? id1, int? id2)
        {
            if (id1 != null && id2 != null)
            {
                UserDocument userDocument = repo.Get(id1, id2);
                if (userDocument != null)
                {
                    repo.Delete(id1, id2);
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}