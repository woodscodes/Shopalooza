using Shopalooza.Core.Models;
using Shopalooza.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopalooza.WebUI.Controllers
{
    public class ProductCategoryController : Controller
    {
        private InMemoryRepository<ProductCategory> _context;

        public ProductCategoryController()
        {
            _context = new InMemoryRepository<ProductCategory>();
        }
        
        // GET: ProductCategory
        public ActionResult Index()
        {
            var productCategorys = _context.Collection().ToList();
            return View(productCategorys);
        }

        public ActionResult Create()
        {
            var productCategory = new ProductCategory();            
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
                return View(productCategory);
            else
            {
                _context.Insert(productCategory);
                _context.Commit();                
            }

            return RedirectToAction("Index");
        }


        public ActionResult Edit(string id)
        {
            var productCategoryToEdit = _context.Find(id);

            if (productCategoryToEdit == null)
                return HttpNotFound();
            else
                return View(productCategoryToEdit);
        }


        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string id)
        {
            var productCategoryToEdit = _context.Find(id);

            if (productCategoryToEdit == null)
                return HttpNotFound();
            else
            {
                if (!ModelState.IsValid)
                    return View(productCategory);

                productCategoryToEdit.Name = productCategory.Name;
                _context.Commit();
            }

            return RedirectToAction("Index");
        }

        // delete

        public ActionResult Delete(string id)
        {
            var productCategoryToDelete = _context.Find(id);

            if (productCategoryToDelete == null)
                return HttpNotFound();

            return View(productCategoryToDelete);
        }

        // confirmdelete

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            var productCategoryToDelete = _context.Find(id);

            if (productCategoryToDelete == null)
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