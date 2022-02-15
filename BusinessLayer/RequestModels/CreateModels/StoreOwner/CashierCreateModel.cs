using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.CreateModels.StoreOwner
{
    public class CashierCreateModel
    {
        public string Name { get; set; }
        public int StoreId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
