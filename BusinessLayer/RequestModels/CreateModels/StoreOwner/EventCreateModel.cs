using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.CreateModels.StoreOwner
{
    public  class EventCreateModel
    {
        public int BrandId { get; set; }
        public string EventName { get; set; }
        public List<EventDetailCreateModel> Details { get; set; }

    }
}
