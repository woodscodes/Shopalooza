using Shopalooza.Core.Contracts;
using Shopalooza.Core.Models;
using Shopalooza.Core.ViewModels;
using Shopalooza.DataAccess.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopalooza.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Product> _context;
        private IRepository<ProductCategory> _productCategoryContext;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            _context = productContext;
            _productCategoryContext = productCategoryContext;
        }

        public ActionResult Index(string category = null)
        {
            var products = new List<Product>();
            var categories = _productCategoryContext.Collection().ToList();

            if (category == null)
                products = _context.Collection().ToList();
            else
                products = _context.Collection().Where(p => p.Category == category).ToList();

            var productListViewModel = new ProductListViewModel
            {
                Products = products,
                ProductCategories = categories
            };

            return View(productListViewModel);
        }

        public ActionResult Details(string id)
        {
            var product = _context.Find(id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}