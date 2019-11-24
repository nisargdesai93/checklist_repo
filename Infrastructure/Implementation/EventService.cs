using Infrastructure.Application.Implementation;
using Infrastructure.Entity;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Implementation
{
    public class EventService
    {
        private readonly Repository<Event> _eventRepository;

        public EventService(UnitOfWork unitOfWork)
        {
            _eventRepository = unitOfWork.Repository<Event>();
        }

        public CalenderEventEditModel CreateEvent(CalenderEventEditModel model)
        {
            var calenderEvent = new CalenderEventEditModel();

            try
            {
                var eventModel = CreateModel(model);
                _eventRepository.Insert(eventModel);
                calenderEvent.Message = "Event successfully added to your calender between " + calenderEvent.Start + " to " + calenderEvent.End;
                return calenderEvent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private Event CreateModel(CalenderEventEditModel model)
        {
            var eventModel = new Event
            {
                PersonId = model.PersonId,
                StartsAt = model.Start,
                EndsAt = model.End,
                Description = model.Title,
                AllDayEvent = false
            };

            return eventModel;
        }

        public List<CalenderEvent> GetAllEvents(CalenderEventSearchEditModel model)
        {
            var events = new List<Event>();
            var result = new List<CalenderEvent>();

            var firstDayOfMonth = new DateTime((int)model.Year, (int)model.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            events = (from e in _eventRepository.List
                      where e.PersonId == model.PersonId && e.StartsAt >= firstDayOfMonth && e.EndsAt <= lastDayOfMonth
                      select e).ToList();

            foreach (var e in events)
            {
                result.Add(CreateModel(e));
            }

            return result;
        }

        private CalenderEvent CreateModel(Event model)
        {
            return new CalenderEvent
            {
                Id = model.Id,
                Start = model.StartsAt,
                End = model.EndsAt,
                AllDay = model.AllDayEvent,
                Title = model.Description,
                Draggable = true,
                Resizable = new Resizable
                {
                    BeforeStart = true,
                    AfterEnd = true,
                }

            };
        }
    }
}

