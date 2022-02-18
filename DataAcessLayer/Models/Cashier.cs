using System;
using System.Collections.Generic;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class Cashier
    {
        public Cashier()
        {
            Bills = new HashSet<Bill>();
        }
        public enum CashierStatus
        {
            Working,
            Disabled
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int StoreId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public CashierStatus Status { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
