using System;

namespace Infrastructure.Entity
{
    public class Event:DomainBase
    {
        public long PersonId { get; set; }
        public string Description { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public bool AllDayEvent { get; set; }
    }
}
