using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopalooza.Core.ViewModels
{
    public class BasketSummaryViewModel
    {
        public int BasketCount { get; set; }
        public decimal TotalBasketValue { get; set; }

        public BasketSummaryViewModel()
        {

        }

        public BasketSummaryViewModel(int basketCount, decimal totalBasketValue)
        {
            BasketCount = basketCount;
            TotalBasketValue = totalBasketValue;
        }
    }
}
