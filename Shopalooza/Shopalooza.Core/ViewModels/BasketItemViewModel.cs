using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopalooza.Core.ViewModels
{
    public class BasketItemViewModel
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }

    }
}
