using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data;
using InternetShop.Models;
using Microsoft.AspNetCore.Mvc; 


namespace InternetShop.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db; 
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objectList = _db.ApplicationType;
            return View(objectList);
        }

        // Create - get 
        public IActionResult Create()
        {
            return View();
        }

        // Create - post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        // Edit - get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.ApplicationType.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View("Edit", obj);
        }

        // Edit - post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // Delete - get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.ApplicationType.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // Delete - post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.ApplicationType.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            _db.ApplicationType.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}

