 using System;
using System.Collections.Generic;

#nullable disable

namespace DataAcessLayer.Models
{
  
    public partial class Store
    {
        public enum StoreApproveStatus
        {
            Approved,
            Pending,
            Rejected,
            Disabled
        }
        public Store()
        {
            Bills = new HashSet<Bill>();
            Cashiers = new HashSet<Cashier>();
            Receipts = new HashSet<Receipt>();
            Stocks = new HashSet<Stock>();
        }

        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public StoreApproveStatus ApprovedStatus { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<Cashier> Cashiers { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
