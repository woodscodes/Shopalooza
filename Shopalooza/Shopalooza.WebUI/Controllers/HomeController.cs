using Shopalooza.Core.Contracts;
using Shopalooza.Core.Models;
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

        public ActionResult Index()
        {
            var products = _context.Collection().ToList();
            return View(products);
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