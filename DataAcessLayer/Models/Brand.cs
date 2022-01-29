using System;
using System.Collections.Generic;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class Brand
    {
        public enum BrandStatus
        {
            Enabled,
            Disabled
        }
        public Brand()
        {
            Categories = new HashSet<Category>();
            Events = new HashSet<Event>();
            Products = new HashSet<Product>();
            Stores = new HashSet<Store>();
            UserBrands = new HashSet<UserBrand>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public BrandStatus Status { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<UserBrand> UserBrands { get; set; }
    }
}
