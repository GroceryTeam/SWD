using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class CashierViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StoreId { get; set; }
        public string Username { get; set; }
        public int Status { get; set; }
    }
}
