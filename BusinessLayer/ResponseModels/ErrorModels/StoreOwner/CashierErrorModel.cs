using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ErrorModels.StoreOwner
{
    public class CashierErrorModel
    {
        public enum CashierCreateError
        {
            UsernameDuplicated
        }
        public CashierCreateError Error { get; set; }
    }
}
