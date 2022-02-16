using System;
using System.Collections.Generic;

#nullable disable

namespace DataAcessLayer.Models
{
    public partial class Event
    {
        public enum EventStatus
        {
            Enabled,
            Disabled
        }
        public Event()
        {
            EventDetails = new HashSet<EventDetail>();
        }

        public int Id { get; set; }
        public int BrandId { get; set; }
        public string EventName { get; set; }
        public EventStatus Status { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<EventDetail> EventDetails { get; set; }
    }
}
