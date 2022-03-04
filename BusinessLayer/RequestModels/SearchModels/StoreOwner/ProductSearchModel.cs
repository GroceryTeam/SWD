using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DataAcessLayer.Models.Product;

namespace BusinessLayer.RequestModels.SearchModels.StoreOwner
{
    public class ProductSearchModel
    {
        public string Sku { get; set; }
        public string SearchTerm { get; set; }
        public int? MinimumSellingPrice { get; set; }
        public int? MaximumSellingPrice { get; set; }
        public int? CategoryId { get; set; }
        public ProductStatus? Status { get; set; }
        public bool? IncludeDisabledProduct { get; set; }
    }
}
