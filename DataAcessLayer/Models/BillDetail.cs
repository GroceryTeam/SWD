using System;
using System.Collections.Generic;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class BillDetail
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int ProductId { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public int Quantity { get; set; }
        public int StockId { get; set; }

        public virtual Bill Bill { get; set; }
        public virtual Product Product { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
