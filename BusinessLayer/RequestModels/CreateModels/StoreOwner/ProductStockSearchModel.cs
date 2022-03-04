using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.CreateModels.StoreOwner
{
    public class ProductStockSearchModel
    {
        public int BrandId { get; set; }
        public int? StoreId { get; set; }
        public int? ProductId { get; set; }
        public string Sku { get; set; }
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public bool OnlyNearlyOutOfStockProduct { get; set; }

    }
}
