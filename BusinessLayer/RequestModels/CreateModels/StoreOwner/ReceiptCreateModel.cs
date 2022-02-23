using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.CreateModels.StoreOwner
{
    public class ReceiptCreateModel
    {
        public int StoreId { get; set; }
        public int TotalCost { get; set; }

        public List<ReceiptDetailCreateModel> Details { get; set; }

    }
}
