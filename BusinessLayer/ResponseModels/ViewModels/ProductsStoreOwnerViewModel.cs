﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ViewModels
{
    public class ProductsStoreOwnerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? UnpackedProductId { get; set; }
        public string UnpackedProductName { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ConversionRate { get; set; }
        public string UnitLabel { get; set; }
        public int? LowerThreshold { get; set; }
        public int Status { get; set; }
    }

}
