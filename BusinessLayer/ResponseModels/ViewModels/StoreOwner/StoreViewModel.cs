using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class StoreViewModel
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public BrandViewModel Brand { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int ApprovedStatus { get; set; }
    }
}
