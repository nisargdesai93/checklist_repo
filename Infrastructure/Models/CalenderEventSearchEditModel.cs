using System;

namespace Infrastructure.Models
{
    public class CalenderEventSearchEditModel
    {
        public long PersonId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
