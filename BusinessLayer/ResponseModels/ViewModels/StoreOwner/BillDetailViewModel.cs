using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class BillDetailViewModel
    {
        public BillDetailViewModel() {}

        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public int Quantity { get; set; }

        public ProductViewModel Product { get; set; }
    }
}
