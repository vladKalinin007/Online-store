﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data;
using InternetShop.Models;
using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objectList = _db.Product;

            //foreach (var obj in objectList)
            //{
            //    obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.Category.Id);
            //}

            return View(objectList);
        }

        // GET - UPSERT 
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),

                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);

                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
                
            }
        }

        // POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _db.Product.Add(productVM.Product);
                }
                else
                {
                    // Update
                    
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            //productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            return View(productVM);
        }


        // Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Category.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost (int? id)
        {
            var obj = _db.Category.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            _db.Category.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}

