using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.SearchModels.SystemAdmin
{
    public class StoreSearchModel
    {
        public string SearchTerm { get; set; }
        public int? BrandId { get; set; }
        public int ApproveStatus { get; set; }
    }
}
