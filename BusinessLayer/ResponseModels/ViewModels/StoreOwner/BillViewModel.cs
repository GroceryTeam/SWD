using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Models;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class BillViewModel
    {
        public BillViewModel()
        {
            BillDetails = new HashSet<BillDetailViewModel>();
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public int CashierId { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalPrice { get; set; }

        public virtual DataAcessLayer.Models.Cashier Cashier { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<BillDetailViewModel> BillDetails { get; set; }
    }
}
