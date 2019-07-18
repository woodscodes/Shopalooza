using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shopalooza.Core.Contracts;
using Shopalooza.Core.Models;
using Shopalooza.Core.ViewModels;
using Shopalooza.WebUI;
using Shopalooza.WebUI.Controllers;
using Shopalooza.WebUI.Tests.Mocks;

namespace Shopalooza.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexPageDoesProducts()
        {
            IRepository<Product> productContext = new MockContext<Product>();
            IRepository<ProductCategory> categoryContext = new MockContext<ProductCategory>();

            productContext.Insert(new Product());

            HomeController controller = new HomeController(productContext, categoryContext);

            // Act
            var result = controller.Index() as ViewResult;
            var viewModel = (ProductListViewModel)result.ViewData.Model;

            Assert.AreEqual(1, viewModel.Products.Count());
            // Assert
        }

    }
}
