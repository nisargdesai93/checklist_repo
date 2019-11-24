using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class CalenderEvent
    {
        public long Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
        public bool AllDay { get; set; }
        public bool Draggable { get; set; }
        public Resizable Resizable { get; set; }


        public CalenderEvent()
        {
            Resizable = new Resizable();
        }
    }

    public class Resizable
    {
        public bool BeforeStart { get; set; }
        public bool AfterEnd { get; set; }
    }
}