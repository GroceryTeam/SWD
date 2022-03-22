using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class StockViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public int ReceiptId { get; set; }
        public DateTime ReceiptCreateDate { get; set; }
        public int Quantity { get; set; }
        public int BuyPrice { get; set; }
        public int Status { get; set; }

    }
}
