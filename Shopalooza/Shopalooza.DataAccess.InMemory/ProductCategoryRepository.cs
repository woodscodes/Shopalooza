using Shopalooza.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Shopalooza.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            if(productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }

        public void Insert(ProductCategory productCategory)
        {
            productCategories.Add(productCategory);
        }

        public void Update(ProductCategory productCategory, string id)
        {
            var productCategoryToUpdate = productCategories.Find(p => p.Id == id);

            if (productCategoryToUpdate != null)
                productCategoryToUpdate = productCategory;
            else
                throw new Exception("Product category not found");
        }

        public ProductCategory Find(string id)
        {
            var productCategory = productCategories.Find(p => p.Id == id);

            if (productCategory != null)
                return productCategory;
            else
                throw new Exception("Product category not found");
        }
        
        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string id)
        {
            var productToDelete = productCategories.Find(p => p.Id == id);

            if (productToDelete != null)
                productCategories.Remove(productToDelete);
            else
                throw new Exception("Product not found");
        }     



    }
}
