using System;

namespace Infrastructure.Models
{
    public class CalenderEventEditModel
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
        public bool AllDay { get; set; }
        public bool Draggable { get; set; }
        public bool ResizableBeforeStart { get; set; }
        public bool ResizableAfterEnd { get; set; }
        public string Message { get; set; }
    }
}
