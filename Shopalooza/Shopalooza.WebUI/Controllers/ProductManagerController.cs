﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopalooza.Core.Models;
using Shopalooza.DataAccess.InMemory;

namespace Shopalooza.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        // GET: ProductManager
        private ProductRepository _context;

        public ProductManagerController()
        {
            _context = new ProductRepository();
        }


        public ActionResult Index()
        {
            List<Product> products = _context.Collection().ToList();

            return View(products);
        }

        public ActionResult Create()
        {
            Product product = new Product();            
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);
            else
            {
                _context.Insert(product);
                _context.Commit();
                return RedirectToAction("Index");
            }            
        }

        public ActionResult Edit(string id)
        {
            var product = _context.Find(id);

            if (product == null)
                return HttpNotFound();
            else
                return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string id)
        {
            var productToEdit = _context.Find(id);
            if (productToEdit == null)
                return HttpNotFound();
            else
            {
                if (!ModelState.IsValid)
                    return View(product);

                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                _context.Commit();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var productToDelete = _context.Find(id);
            if (productToDelete == null)
                return HttpNotFound();
            else
                return View(productToDelete);            
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            var productToDelete = _context.Find(id);

            if (productToDelete == null)
                return HttpNotFound();
            else
            {
                _context.Delete(id);
                _context.Commit();
            }
                

            return RedirectToAction("Index");
        }
    }
}