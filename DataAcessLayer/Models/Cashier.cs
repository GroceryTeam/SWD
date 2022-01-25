﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class Cashier
    {
        public Cashier()
        {
            Bills = new HashSet<Bill>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int StoreId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
