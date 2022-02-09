using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.CreateModels.Cashier
{

    public class BillCreateModel
    {
        public int TotalPrice { get; set; }
        public List<BillDetailCreateModel> Details { get; set; }
    }
}
