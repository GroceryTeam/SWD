using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.SearchModels.StoreOwner
{
    public class CashierSearchModel
    {
        public string SearchTerm { get; set; }
        public int? BrandId { get; set; }
        public int? StoreId { get; set; }
        public bool IncludeDisabledCashier { get; set; }
    }
}
