using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.SearchModels.StoreOwner
{
    public class RevenueSearchModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ReturnDateResult { get; set; }
        public bool  ReturnMonthResult { get; set; }
        public int? BrandId { get; set; }
        public int? StoreId { get; set; }
    }
}
