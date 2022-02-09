﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class ReceiptDetailViewModel
    {
        public int ReceiptId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int BuyPrice { get; set; }
        public int Quantity { get; set; }

    }
}
