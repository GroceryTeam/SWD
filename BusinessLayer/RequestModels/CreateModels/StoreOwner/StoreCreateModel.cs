using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.CreateModels.StoreOwner
{
    public class StoreCreateModel
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

    }
}
