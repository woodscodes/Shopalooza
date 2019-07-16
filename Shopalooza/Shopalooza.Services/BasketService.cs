using Shopalooza.Core.Contracts;
using Shopalooza.Core.Models;
using Shopalooza.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Shopalooza.Services
{
    public class BasketService : IBasketService
    {
        private IRepository<Product> _productContext;
        private IRepository<Basket> _basketContext;

        public const string BasketSessionName = "ShopaloozaBasket";

        public BasketService(IRepository<Product> productContext, IRepository<Basket> basketContext)
        {
            _productContext = productContext;
            _basketContext = basketContext;
        }

        private Basket GetBasket(HttpContextBase httpContextBase, bool createIfNull)
        {
            HttpCookie cookie = httpContextBase.Request.Cookies.Get(BasketSessionName);

            var basket = new Basket();

            if (cookie != null)
            {
                var basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                    basket = _basketContext.Find(basketId);
                else
                {
                    if (createIfNull)
                        basket = createNewBasket(httpContextBase);
                }
            }
            else
            {
                if (createIfNull)
                    basket = createNewBasket(httpContextBase);
            }

            return basket;
        }

        private Basket createNewBasket(HttpContextBase httpContextBase)
        {
            var basket = new Basket();
            _basketContext.Insert(basket);
            _basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContextBase.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContextBase, string productId)
        {
            var basket = GetBasket(httpContextBase, true);
            var basketItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if(basketItem == null)
            {
                basketItem = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(basketItem);
            }
            else
            {
                basketItem.Quantity = basketItem.Quantity + 1;
            }

            _basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContextBase, string basketItemId)
        {
            var basket = GetBasket(httpContextBase, true);
            var basketItem = basket.BasketItems.FirstOrDefault(i => i.Id == basketItemId);

            if(basketItem != null)
            {
                basket.BasketItems.Remove(basketItem);
                _basketContext.Commit();
            }
            
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContextBase)
        {
            var basket = GetBasket(httpContextBase, false);

            if(basket != null)
            {
                var results = (from b in basket.BasketItems
                              join p in _productContext.Collection() on b.ProductId equals p.Id
                              select new BasketItemViewModel()
                              {
                                  Id = b.Id,
                                  Quantity = b.Quantity,
                                  ProductName = p.Name,
                                  ProductImage = p.Image,
                                  ProductPrice = p.Price
                              }).ToList();

                return results;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContextBase)
        {
            var basket = GetBasket(httpContextBase, false);
            var basketSummaryViewModel = new BasketSummaryViewModel(0, 0);

            if (basket != null)
            {
                int? basketCount = (from i in basket.BasketItems
                                    select i.Quantity).Sum();

                decimal? totalBasketValue = (from i in basket.BasketItems
                                             join p in _productContext.Collection() on i.ProductId equals p.Id
                                             select i.Quantity * p.Price).Sum();

                basketSummaryViewModel.BasketCount = basketCount ?? 0;
                basketSummaryViewModel.TotalBasketValue = totalBasketValue ?? decimal.Zero;

                return basketSummaryViewModel;
            }
            else
                return basketSummaryViewModel;
        }
    }
}
