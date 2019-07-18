using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shopalooza.Core.Contracts;
using Shopalooza.Core.Models;
using Shopalooza.Core.ViewModels;
using Shopalooza.Services;
using Shopalooza.WebUI.Controllers;
using Shopalooza.WebUI.Tests.Mocks;

namespace Shopalooza.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            //setup
            IRepository<Basket> _basketContext = new MockContext<Basket>();
            IRepository<Product> _productContext = new MockContext<Product>();
            var httpContext = new HttpMockContext();

            IBasketService basketService = new BasketService(_productContext, _basketContext);
                        
            var controller = new BasketController(basketService);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
            //basketService.AddToBasket(httpContext, "1");

            //act
            controller.AddToBasket("1");


            Basket basket = _basketContext.Collection().FirstOrDefault();

            //assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count());
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            IRepository<Basket> _basketContext = new MockContext<Basket>();
            IRepository<Product> _productContext = new MockContext<Product>();

            _productContext.Insert(new Product { Id = "1", Price = 50 });
            _productContext.Insert(new Product { Id = "2", Price = 10 });


            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 1 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 1 });

            _basketContext.Insert(basket);

            IBasketService basketService = new BasketService(_productContext, _basketContext);           

            var controller = new BasketController(basketService);
            var httpContext = new HttpMockContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerce basket") { Value = basket.Id });

            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;

            Assert.AreEqual(3, basketSummary.BasketCount);
            Assert.AreEqual(110, basketSummary.TotalBasketValue);
        }
    }
}
