using System;
using System.Collections.Generic;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class Stock
    {
        public Stock()
        {
            BillDetails = new HashSet<BillDetail>();
        }
        public enum StockDetail
        {
            SoldOut,
            Selling,
            Available
        }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public int ReceiptId { get; set; }
        public int Quantity { get; set; }
        public StockDetail Status { get; set; }
        public int BuyPrice { get; set; }

        public virtual Product Product { get; set; }
        public virtual Receipt Receipt { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
