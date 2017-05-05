using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventTicket.Service.Interface;

namespace EventTicket.Web.Controllers
{
    public class TicketController : ApiController
    {
        private readonly IEventService _eventService;
        private readonly ITicketService _ticketService;
        public TicketController(IEventService eventService, ITicketService ticketService)
        {
            _eventService = eventService;
            _ticketService = ticketService;
        }
    }
}
