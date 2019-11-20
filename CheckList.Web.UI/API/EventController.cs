using Infrastructure.Implementation;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CheckList.Web.UI.API
{
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("createevent")]
        public CalenderEventEditModel CreateEvent([FromBody]CalenderEventEditModel model)
        {
           return _eventService.CreateEvent(model);
        }

        [HttpGet("getevent/{personId}")]
        public IEnumerable<CalenderEventEditModel> GetEvents([FromBody]CalenderEventSearchEditModel model)
        {
            return _eventService.GetAllEvents(model);
        }
    }
}