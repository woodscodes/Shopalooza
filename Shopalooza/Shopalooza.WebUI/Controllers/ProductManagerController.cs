using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopalooza.Core.Models;
using Shopalooza.Core.ViewModels;
using Shopalooza.DataAccess.InMemory;

namespace Shopalooza.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        // GET: ProductManager
        private InMemoryRepository<Product> _context;
        private InMemoryRepository<ProductCategory> _productCategories;

        public ProductManagerController()
        {
            _context = new InMemoryRepository<Product>();
            _productCategories = new InMemoryRepository<ProductCategory>();
        }


        public ActionResult Index()
        {
            var products = _context.Collection().ToList();

            return View(products);
        }

        public ActionResult Create()
        {
            var productManagerViewModel = new ProductManagerViewModel
            {
                Product = new Product(),
                ProductCategories = _productCategories.Collection()
            };
                     
            return View(productManagerViewModel);
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
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var product = _context.Find(id);

            if (product == null)
                return HttpNotFound();
            else
            {
                var productManagerViewModel = new ProductManagerViewModel
                {
                    Product = product,
                    ProductCategories = _productCategories.Collection()
                };

                return View(productManagerViewModel);
            }                
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