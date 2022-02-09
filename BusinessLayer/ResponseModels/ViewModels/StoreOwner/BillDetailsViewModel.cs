using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class BillDetailsViewModel
    {
        public BillDetailsViewModel() {}

        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public int Quantity { get; set; }

        public ProductsViewModel Product { get; set; }
    }
}
