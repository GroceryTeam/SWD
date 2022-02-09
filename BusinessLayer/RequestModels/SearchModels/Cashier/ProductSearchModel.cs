using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DataAcessLayer.Models.Product;

namespace BusinessLayer.RequestModels.SearchModels.Cashier
{
    public class ProductSearchModel
    {
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
    }
}
