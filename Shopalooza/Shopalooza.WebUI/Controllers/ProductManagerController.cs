using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopalooza.Core.Contracts;
using Shopalooza.Core.Models;
using Shopalooza.Core.ViewModels;
using Shopalooza.DataAccess.InMemory;

namespace Shopalooza.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        // GET: ProductManager
        private IRepository<Product> _context;
        private IRepository<ProductCategory> _productCategories;

        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoriesContext)
        {
            _context = productContext;
            _productCategories = productCategoriesContext;
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
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(product);
            else
            {
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }

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
        public ActionResult Edit(Product product, string id, HttpPostedFileBase file)
        {
            var productToEdit = _context.Find(id);
            if (productToEdit == null)
                return HttpNotFound();
            else
            {
                if (!ModelState.IsValid)
                    return View(product);

                if(file != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                }

                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
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