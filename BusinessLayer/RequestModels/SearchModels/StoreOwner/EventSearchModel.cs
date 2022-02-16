using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAcessLayer.Models.Event;

namespace BusinessLayer.RequestModels.SearchModels.StoreOwner
{
    public class EventSearchModel
    {
        public string SearchTerm { get; set; }
        public int Status { get; set; }
    }
}
