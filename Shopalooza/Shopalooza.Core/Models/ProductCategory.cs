using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopalooza.Core.Models
{
    public class ProductCategory
    {
        public string Id { get; set; }

        [DisplayName("Product Category")]
        public string Name { get; set; }

        public ProductCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
