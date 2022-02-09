using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class EventDetailViewModel
    {
        public int EventId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int NewPrice { get; set; }
        public int OriginalPrice { get; set; }

    }
}
