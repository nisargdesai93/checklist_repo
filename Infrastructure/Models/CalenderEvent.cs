using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public class CalenderEvent
    {
        public long Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
        public EventColor Color { get; set; }
        public List<EventAction> Actions { get; set; }
        public bool AllDay { get; set; }
        public bool Draggable { get; set; }
        public Resizable Resizable { get; set; }


        public CalenderEvent()
        {
            Resizable = new Resizable();
            Color = new EventColor();
            Actions = new List<EventAction>();
        }
    }

    public class Resizable
    {
        public bool BeforeStart { get; set; }
        public bool AfterEnd { get; set; }
    }

    public class EventColor
    {
        public string Primary { get; set; }
        public string Secondary { get; set; }
    }

    public class EventAction
    {
        public long Id { get; set; }
        public string Label { get; set; }
        public string CssClass { get; set; }
        public string A11Label { get; set; }
    }


}