using System;
using System.Collections.Generic;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class Bill
    {
        public Bill()
        {
            BillDetails = new HashSet<BillDetail>();
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public int CashierId { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalPrice { get; set; }

        public virtual Cashier Cashier { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
