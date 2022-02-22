using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.CreateModels.Cashier
{
    public class UnpackProductRequestModel
    {
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public int Quantity { get; set; }

    }
}
