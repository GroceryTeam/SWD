using System;
using System.Collections.Generic;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class Receipt
    {
        public Receipt()
        {
            ReceiptDetails = new HashSet<ReceiptDetail>();
            Stocks = new HashSet<Stock>();
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalCost { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
