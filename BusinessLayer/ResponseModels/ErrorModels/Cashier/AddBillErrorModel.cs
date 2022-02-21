using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ErrorModels.Cashier
{
    public class ProductErrorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RemainingQuantity { get; set; }
    }
    public class AddBillErrorModel
    {
        public List<ProductErrorModel> ErrorProducts { get; set; }
    }
}
