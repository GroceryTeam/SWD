using System;
using System.Collections.Generic;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class Product
    {
        public enum ProductStatus
        {
            Selling,
            Disabled,
            NearlyOutOfStock
        }
        public Product()
        {
            BillDetails = new HashSet<BillDetail>();
            EventDetails = new HashSet<EventDetail>();
            InverseUnpackedProduct = new HashSet<Product>();
            ReceiptDetails = new HashSet<ReceiptDetail>();
            Stocks = new HashSet<Stock>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? UnpackedProductId { get; set; }
        public int SellPrice { get; set; }
        public int CategoryId { get; set; }
        public int? ConversionRate { get; set; }
        public string UnitLabel { get; set; }
        public int BrandId { get; set; }
        public int? LowerThreshold { get; set; }
        public ProductStatus Status { get; set; }
        public string Sku { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        public virtual Product UnpackedProduct { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }
        public virtual ICollection<EventDetail> EventDetails { get; set; }
        public virtual ICollection<Product> InverseUnpackedProduct { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
