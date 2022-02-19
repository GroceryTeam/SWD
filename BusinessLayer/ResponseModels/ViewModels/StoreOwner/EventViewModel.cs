using DataAcessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public int Status { get; set; }
        public int ProductCount { get; set; }
        public List<EventDetailViewModel> EventDetails { get; set; }
    }
}
